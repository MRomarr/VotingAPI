namespace VotingAPI.Interface
{
    public interface IOptionService
    {
        Task<ServiceResult<string>> CreateOptionAsync(string UserID, CreateOptionDto dto);
        Task<ServiceResult<string>> UpdateOptionAsync(string UserID, UpdateOptionDto dto);
        Task<ServiceResult<string>> DeleteOptionAsync(string UserID, DeleteOptionDto dto);
    }
}
