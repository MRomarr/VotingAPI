
namespace VotingAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Poll> polls { get; set; } = new List<Poll>();
        public ICollection<Vote> votes { get; set; } = new List<Vote>();
    }
}
