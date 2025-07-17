
namespace VotingAPI.Services
{
    public class PollService : IPollService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PollService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PollDto>> GetAllPollsAsync()
        {
            var polls = await _unitOfWork.GetRepository<Poll>().GetAllAsync( x=>x.Options,x=>x.Votes);
            var Data = _mapper.Map<IEnumerable<PollDto>>(polls);
            return Data;
        }
        public async Task<IEnumerable<PollDto>> GetUserPollsAsync(string UserId)
        {
            var polls = await _unitOfWork.GetRepository<Poll>().GetAllAsync(x => x.Options, x => x.Votes);
            var userPolls = polls.Where(p => p.UserId == UserId);
            var Data = _mapper.Map<IEnumerable<PollDto>>(userPolls);
            return Data;
        }
        public async Task<ServiceResult<PollDto>> GetPollById(string Id)
        {
            var poll = await _unitOfWork.GetRepository<Poll>().GetByIdAsync(Id, x => x.Options, x => x.Votes);
            if (poll is null )
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Poll not found."
                };
            }
            var Data = _mapper.Map<PollDto>(poll);
            return new ServiceResult<PollDto>
            {
                Success = true,
                Data = Data
            };
        }
        public async Task<ServiceResult<PollDto>> CreatePollAsync(string UserId,CreatePollDto dto)
        {
            
            var poll = _mapper.Map<Poll>(dto);
            poll.UserId = UserId;
            await _unitOfWork.GetRepository<Poll>().AddAsync(poll);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Failed to create poll."
                };
            }
            var Data = _mapper.Map<PollDto>(poll);

            return new ServiceResult<PollDto>
            {
                Success = true,
                Data = Data
            };
        }
        public async Task<ServiceResult<PollDto>> UpdatePollAsync(string UserId, UpdatePollDto dto)
        {
            var poll = await _unitOfWork.GetRepository<Poll>().GetByIdAsync(dto.PollId);
            if(poll is null)
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Poll not found."
                };
            }
            if (poll.UserId != UserId)
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Poll not found."
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Poll Expired."
                };
            }
            var polls = await GetUserPollsAsync(UserId);
            if (polls.Any(p => p.Name == dto.Name))
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Poll with this Name already exists."
                };
            }
            _mapper.Map(dto, poll);
            _unitOfWork.GetRepository<Poll>().Update(poll);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<PollDto>
                {
                    Success = false,
                    Message = "Failed to update poll."
                };
            }
            var Data = _mapper.Map<PollDto>(poll);
            return new ServiceResult<PollDto>
            {
                Success = true,
                Data = Data
            };

        }

        public async Task<ServiceResult<string>> DeletePollAsync(string UserId, string Id)
        {
            var poll = await _unitOfWork.GetRepository<Poll>().GetByIdAsync(Id);
            if (poll is null)
            {
                return new ServiceResult<string>
                {
                    Success = false,
                    Message = "Poll not found."
                };
            }
            if (poll.UserId != UserId)
            {
                return new ServiceResult<string>
                {
                    Success = false,
                    Message = "Poll not found."
                };
            }
            _unitOfWork.GetRepository<Poll>().Delete(poll);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<string>
                {
                    Success = false,
                    Message = "Failed to delete poll."
                };
            }
            return new ServiceResult<string>
            {
                Success = true,
                Data = "Poll deleted successfully."
            };

        }
    }
}
