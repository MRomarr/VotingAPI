namespace VotingAPI.DTOs.OptionDTOs
{
    public class UpdateOptionDto
    {
        public string PollId { get; set; }
        public string OptionId { get; set; }
        public string NewText { get; set; }
    }
}
