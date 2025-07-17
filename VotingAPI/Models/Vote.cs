namespace VotingAPI.Models
{
    public class Vote
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string PollId { get; set; }
        public Poll Poll { get; set; }

        public string OptionId { get; set; }
        public Option Option { get; set; }
    }
}
