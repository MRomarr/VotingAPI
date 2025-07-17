namespace VotingAPI.DTOs.VoteDTOs
{
    public class UpdateVoteDto
    {
        public string Id { get; set; }
        public string PollId { get; set; }
        public string NewOptionId { get; set; } 
        public string UserId { get; set; } 
    }
}
