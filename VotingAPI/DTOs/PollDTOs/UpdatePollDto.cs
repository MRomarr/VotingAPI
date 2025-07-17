namespace VotingAPI.DTOs.PollDTOs
{
    public class UpdatePollDto
    {
        public string PollId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
