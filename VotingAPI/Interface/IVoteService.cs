namespace VotingAPI.Interface
{
    public interface IVoteService
    {
        Task<ServiceResult<VoteDto>> AddVoteAsync(string UserId, CreateVoteDto dto);
        Task<ServiceResult<VoteDto>> UpdateVoteAsync(string UserId, UpdateVoteDto dto);
        Task<ServiceResult<string>> DeleteVoteAsync(string UserId, DeleteVoteDto dto);

    }
}
