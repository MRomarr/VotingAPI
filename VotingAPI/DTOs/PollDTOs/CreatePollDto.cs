namespace VotingAPI.DTOs.PollDTOs
{
    public class CreatePollDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<CreateOptionDto> Options { get; set; } = new List<CreateOptionDto>();
    }
}
