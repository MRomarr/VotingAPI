namespace VotingAPI.DTOs.VoteDTOs
{
    public class CreateVoteDto
    {
        public string PollId { get; set; } 
        public string OptionId { get; set; }
        public string UserId { get; set; } 
    }
}
