namespace VotingAPI.DTOs.VoteDTOs
{
    public class VoteDto
    {
        public string Id { get; set; }
        public DateTime VotedAt { get; set; }
        public string PollId { get; set; }
        public string OptionId { get; set; }
        public string UserName { get; set; }
    }
}
