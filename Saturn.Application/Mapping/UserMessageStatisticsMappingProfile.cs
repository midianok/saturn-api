using AutoMapper;
using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mapping;

public class UserMessageStatisticsMappingProfile : Profile
{
    public UserMessageStatisticsMappingProfile()
    {
        CreateMap<UserMessageStatistics, UserMessageStatisticsResponseDto>();
    }
}