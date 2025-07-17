
namespace VotingAPI.DTOs.PollDTOs
{
    public class PollDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
        
    }
}
