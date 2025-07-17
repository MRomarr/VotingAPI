namespace VotingAPI.DTOs.AuthDTOs
{
    public class GoogleSignInDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
