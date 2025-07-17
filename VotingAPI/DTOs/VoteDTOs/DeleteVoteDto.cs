namespace VotingAPI.DTOs.VoteDTOs
{
    public class DeleteVoteDto
    {
        public string Id { get; set; }
        public string PollId { get; set; }
        public string OptionId { get; set; }
        public string UserId { get; set; }
    }
}
