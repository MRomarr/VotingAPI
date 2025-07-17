namespace VotingAPI.Models
{
    public class Poll
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive => !EndDate.HasValue || DateTime.UtcNow <= EndDate;  
        public List<Option> Options { get; set; } = new List<Option>();
        public List<Vote> Votes { get; set; } = new List<Vote>();

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
