
namespace VotingAPI.Services
{
    public class VoteService : IVoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<VoteDto>> AddVoteAsync(string UserId, CreateVoteDto dto)
        {
            var poll = await _unitOfWork.GetRepository<Poll>()
                .GetByIdAsync(dto.PollId, p=>p.Options,p=>p.Votes);
            if (poll is null)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (poll.UserId != UserId)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (!poll.Options.Where(o => o.Id == dto.OptionId).Any())
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Option not found in this poll.",
                    Success = false
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll is expired.",
                    Success = false
                };
            }
            if (poll.Votes.Where(v => v.UserId == UserId && v.PollId == dto.PollId).Any())
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "You have already voted in this poll.",
                    Success = false
                };
            }
            var Data = _mapper.Map<Vote>(dto);
            await _unitOfWork.GetRepository<Vote>().AddAsync(Data);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Failed to add vote.",
                    Success = false
                };
            }
            return new ServiceResult<VoteDto>
            {
                Success = true,
                Data = _mapper.Map<VoteDto>(Data),
            };
        }
        public async Task<ServiceResult<VoteDto>> UpdateVoteAsync(string UserId, UpdateVoteDto dto)
        {
            var poll = await _unitOfWork.GetRepository<Poll>()
                .GetByIdAsync(dto.PollId, p => p.Options, p => p.Votes);
            if (poll is null)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (poll.UserId != UserId)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (!poll.Options.Where(o => o.Id == dto.NewOptionId).Any())
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Option not found in this poll.",
                    Success = false
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Poll is expired.",
                    Success = false
                };
            }
            var vote = await _unitOfWork.GetRepository<Vote>()
                .GetByIdAsync(dto.Id, v => v.Poll, v => v.Option);
            if (vote is null)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Vote not found.",
                    Success = false
                };
            }
            if (vote.UserId != UserId)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "You are not authorized to update this vote.",
                    Success = false
                };
            }
            if (vote.PollId != dto.PollId)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Vote does not belong to this poll.",
                    Success = false
                };
            }
            _mapper.Map(dto, vote);
            _unitOfWork.GetRepository<Vote>().Update(vote);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<VoteDto>
                {
                    Message = "Failed to update vote.",
                    Success = false
                };
            }
            return new ServiceResult<VoteDto>
            {
                Success = true,
                Data = _mapper.Map<VoteDto>(vote),
            };

        }
        public async Task<ServiceResult<string>> DeleteVoteAsync(string UserId, DeleteVoteDto dto)
        {
            var poll = await _unitOfWork.GetRepository<Poll>()
                .GetByIdAsync(dto.PollId, p => p.Options, p => p.Votes);
            if (poll is null)
            {
                return new ServiceResult<string>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (poll.UserId != UserId)
            {
                return new ServiceResult<string>
                {
                    Message = "Poll not found.",
                    Success = false
                };
            }
            if (!poll.Options.Where(o => o.Id == dto.OptionId).Any())
            {
                return new ServiceResult<string>
                {
                    Message = "Option not found in this poll.",
                    Success = false
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<string>
                {
                    Message = "Poll is expired.",
                    Success = false
                };
            }
            var vote = await _unitOfWork.GetRepository<Vote>()
                .GetByIdAsync(dto.Id, v => v.Poll, v => v.Option);
            if (vote is null)
            {
                return new ServiceResult<string>
                {
                    Message = "Vote not found.",
                    Success = false
                };
            }
            if (vote.UserId != UserId)
            {
                return new ServiceResult<string>
                {
                    Message = "You are not authorized to update this vote.",
                    Success = false
                };
            }
            if (vote.PollId != dto.PollId)
            {
                return new ServiceResult<string>
                {
                    Message = "Vote does not belong to this poll.",
                    Success = false
                };
            }
            _unitOfWork.GetRepository<Vote>().Delete(vote);
            var result = await _unitOfWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<string>
                {
                    Message = "Failed to delete vote.",
                    Success = false
                };
            }
            return new ServiceResult<string>
            {
                Success = true,
                Message = "Vote deleted successfully."
            };
        }

    }
}
