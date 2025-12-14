using AutoMapper;
using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mapping;

public class UserChatStatisticsMappingProfile  : Profile
{
    public UserChatStatisticsMappingProfile()
    {
        CreateMap<UserChatStatistics, UserChatStatisticsDto>();
    }
}