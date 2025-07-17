

namespace VotingAPI.Services
{
    public class OptionService : IOptionService
    {
        private readonly IUnitOfWork _unitOFWork;
        private readonly IMapper _mapper;

        public OptionService(IUnitOfWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<string>> CreateOptionAsync(string UserID, CreateOptionDto dto)
        {
            
            var poll = await _unitOFWork.GetRepository<Poll>().GetByIdAsync(dto.PollId);
            if (poll is null)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll Not Found"
                };
            }
            if(poll.UserId != UserID)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll Not Found"
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll is expire"
                };
            }
            var Data =_mapper.Map<Option>(dto);

            await _unitOFWork.GetRepository<Option>().AddAsync(Data);

            var result = await _unitOFWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<string>()
                {
                    Message = "Failed to create option"
                };
            }
            return new ServiceResult<string>()
            {
                Success = true,
                Message = "Option created successfully"
            };
        }
        public async Task<ServiceResult<string>> UpdateOptionAsync(string UserID, UpdateOptionDto dto)
        {
            
            var poll = await _unitOFWork.GetRepository<Poll>().GetByIdAsync(dto.PollId);
            if (poll is null)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll Not Found"
                };
            }
            if(poll.UserId != UserID)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll Not Found"
                };
            }
            if (!poll.IsActive)
            {
                return new ServiceResult<string>()
                {
                    Message = "Poll is expire"
                };
            }
            var option = await _unitOFWork.GetRepository<Option>().GetByIdAsync(dto.OptionId);
            if (option is null)
            {
                return new ServiceResult<string>()
                {
                    Message = "Option Not Found"
                };
            }
            _mapper.Map(dto,option);

            _unitOFWork.GetRepository<Option>().Update(option);

            var result = await _unitOFWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<string>()
                {
                    Message = "Failed to update option"
                };
            }
            return new ServiceResult<string>()
            {
                Success = true,
                Message = "Option updated successfully"
            };
        }
        public async Task<ServiceResult<string>> DeleteOptionAsync(string UserID, DeleteOptionDto dto)
        {
            var option = await _unitOFWork.GetRepository<Option>().GetByIdAsync(dto.OptionId,o=>o.Poll);
            if (option is null)
            {
                return new ServiceResult<string>()
                {
                    Message = "Option Not Found"
                };
            }
            if (option.PollId != dto.PollId)
            {
                return new ServiceResult<string>()
                {
                    Message = "Option Not Found"
                };
            }
            if (!option.Poll.IsActive)
            {
                return new ServiceResult<string>()
                {
                    Message = "Option is expire"
                };
            }
            if (option.Poll.UserId != UserID)
            {
                return new ServiceResult<string>()
                {
                    Message = "Option Not Found"
                };
            }
            _unitOFWork.GetRepository<Option>().Delete(option);
            var result = await _unitOFWork.SaveAsync();
            if (result <= 0)
            {
                return new ServiceResult<string>()
                {
                    Message = "Failed to delete option"
                };
            }
            return new ServiceResult<string>()
            {
                Success = true,
                Message = "Option deleted successfully"
            };

        }


    }
}
