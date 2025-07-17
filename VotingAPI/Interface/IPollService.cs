

namespace VotingAPI.Interface
{
    public interface IPollService
    {
        Task<IEnumerable<PollDto>> GetAllPollsAsync();
        Task<IEnumerable<PollDto>> GetUserPollsAsync(string UserId);
        Task<ServiceResult<PollDto>> GetPollById(string Id);
        Task<ServiceResult<PollDto>> CreatePollAsync(string UserId,CreatePollDto dto);
        Task<ServiceResult<PollDto>> UpdatePollAsync(string UserId, UpdatePollDto dto);
        Task<ServiceResult<string>> DeletePollAsync(string UserId, string Id); 
    }
}
