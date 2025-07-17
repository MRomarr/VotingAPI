using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VotingAPI.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Google.Apis.Auth;

namespace VotingAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTHelper _jwt;
        private readonly IConfiguration _config;
        public AuthService(
            UserManager<ApplicationUser> userManger,
            IOptions<JWTHelper> jwt,
            RoleManager<IdentityRole> roleManager
,
            IConfiguration config)
        {
            _userManager = userManger;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _config = config;
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return new AuthDto
                {
                    Message = "Email already exists."
                };
            }
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
            {
                return new AuthDto
                {
                    Message = "Username already exists."
                };
            }
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return new AuthDto
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            
            await _userManager.AddToRoleAsync(user, RoleHelper.User);

            var jwtToken = await CreateJwtToken(user);

            return new AuthDto
            {
                Message = "Registration successful.",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                TokenExpiration = jwtToken.ValidTo,
                Role = new List<string> { RoleHelper.User },
                IsAuthenticated = true
            };

        }
        public async Task<AuthDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.EmailOrUserName);
            if (user is null)
                user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user,dto.Password))
            {
                return new AuthDto
                {
                    Message = "Invalid username or password."
                };
            }
            var jwtToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthDto
            {
                Message = "Login successful.",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                TokenExpiration = jwtToken.ValidTo,
                Role = roles.ToList(),
                IsAuthenticated = true
            };
        }
        public async Task<string> AddRoleAsync(AddRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
                return "Invalid user ID";

            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
                return "Invalid role";

            if (await _userManager.IsInRoleAsync(user, dto.RoleName))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
            return result.Succeeded ? "Role Added successfuly" : "Something went wrong";
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var UserClimas = await _userManager.GetClaimsAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var Climas = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };
            Climas.AddRange(UserClimas);
            Climas.AddRange(RoleClaims);

            if (_jwt.Issuer is null)
                throw new InvalidOperationException("JWT configuration is missing.");

            var key = _jwt.Key ?? throw new InvalidOperationException("JWT key is missing.");
            var issuer = _jwt.Issuer ?? throw new InvalidOperationException("JWT issuer is missing.");
            var audience = _jwt.Audience ?? throw new InvalidOperationException("JWT audience is missing.");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: Climas,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );
            return jwtToken;
        }

        public async Task<ServiceResult<AuthDto>> GoogleSignIn(GoogleSignInDto model)
        {
            Payload payload;

            try
            {
                payload = await ValidateAsync(model.IdToken, new ValidationSettings
                {
                    Audience = new[] { _config["GoogleAuth:ClientId"] }
                });

            }
            catch (Exception)
            {
                return new ServiceResult<AuthDto>()
                {
                    Message = "Invalid Google token."
                };
            }
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return new ServiceResult<AuthDto>()
                    {
                        Message = errors.ToString()
                    };
                }
            }
            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new ServiceResult<AuthDto>()
            {
                Success = true,
                Data = new AuthDto
                {
                    Message = "Google Sign In successful.",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpiration = token.ValidTo,
                    Role = roles.ToList(),
                    IsAuthenticated = true
                }
            };

        }

    }

}
