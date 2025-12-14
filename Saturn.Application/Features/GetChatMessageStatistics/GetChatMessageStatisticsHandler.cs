using AutoMapper;
using MediatR;
using Saturn.Application.Dtos;
using Saturn.Domain.Services;

namespace Saturn.Application.Features.GetChatMessageStatistics;

public class GetChatMessageStatisticsHandler : IRequestHandler<GetChatMessageStatisticsQuery, GetChatMessageStatisticsResponseDto>
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public GetChatMessageStatisticsHandler(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }
    
    public async Task<GetChatMessageStatisticsResponseDto> Handle(GetChatMessageStatisticsQuery request, CancellationToken cancellationToken)
    {
        var result = await _messageService.GetChatMessageStatisticsAsync(request.ChatId, request.From, request.To, cancellationToken);
        var statistics = _mapper.Map<UserChatStatisticsDto[]>(result);
        return new GetChatMessageStatisticsResponseDto(statistics);
    }
}