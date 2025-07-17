
using Microsoft.AspNetCore.Authentication;
using VotingAPI.DTOs.AuthDTOs;

namespace VotingAPI.Interface
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> LoginAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
        Task<ServiceResult<AuthDto>> GoogleSignIn(GoogleSignInDto dto);
    }
}
