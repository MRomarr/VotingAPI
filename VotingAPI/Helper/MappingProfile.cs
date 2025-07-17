using AutoMapper;

namespace VotingAPI.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poll, PollDto>()
                .ForMember(dest => dest.Options, opt => opt
                .MapFrom(src => src.Options.Select(o => new OptionDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    Votes = src.Votes.Where(v => v.OptionId == o.Id)
                        .Select(vote => new VoteDto
                        {
                            Id = vote.Id,
                            UserName = vote.User.UserName,
                            VotedAt = vote.VotedAt
                        }).ToList()
                })));

            CreateMap<CreatePollDto, Poll>()
                .ForMember(dest => dest.Options, opt => opt
                .MapFrom(src => src.Options.Select(o => new Option
                {
                    Text = o.Text,
                    PollId = o.PollId,
                })));

            CreateMap<UpdatePollDto, Poll>();

            CreateMap<CreateOptionDto, Option>()
                .ForMember(dest => dest.Text, opt => opt
                .MapFrom(src => src.Text))
                .ForMember(dest => dest.PollId,
                opt => opt
                .MapFrom(src => src.PollId));

            CreateMap<UpdateOptionDto, Option>()
                .ForMember(dest => dest.Text, opt => opt
                .MapFrom(src => src.NewText));

            CreateMap<CreateVoteDto,Vote>()
                .ForMember(dest => dest.UserId, opt => opt
                .MapFrom(src => src.UserId))
                .ForMember(dest => dest.PollId, opt => opt
                .MapFrom(src => src.PollId))
                .ForMember(dest => dest.OptionId, opt => opt
                .MapFrom(src => src.OptionId));

            CreateMap<UpdateVoteDto, Vote>()
                .ForMember(dest => dest.UserId, opt => opt
                .MapFrom(src => src.UserId))
                .ForMember(dest => dest.PollId, opt => opt
                .MapFrom(src => src.PollId))
                .ForMember(dest => dest.OptionId, opt => opt
                .MapFrom(src => src.NewOptionId));

            CreateMap<Vote, VoteDto>()
                .ForMember(dest => dest.Id, opt => opt
                .MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt
                .MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.OptionId, opt => opt
                .MapFrom(src => src.OptionId))
                .ForMember(dest => dest.PollId, opt => opt
                .MapFrom(src => src.PollId))
                .ForMember(dest => dest.VotedAt, opt => opt
                .MapFrom(src => src.VotedAt));
        }
    }
}
