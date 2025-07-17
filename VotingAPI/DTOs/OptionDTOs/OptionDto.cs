
namespace VotingAPI.DTOs.OptionDTOs
{
    public class OptionDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<VoteDto> Votes { get; set; } = new List<VoteDto>();
    }
}
