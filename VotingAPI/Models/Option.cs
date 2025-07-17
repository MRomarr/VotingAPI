namespace VotingAPI.Models
{
    public class Option
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; } = string.Empty;

        public string PollId { get; set; }
        public Poll Poll { get; set; }

    }
}
