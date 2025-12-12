using AutoMapper;
using MediatR;
using Saturn.Application.Dtos;
using Saturn.Domain.Services;

namespace Saturn.Application.Features.GetUserMessageStatistics;

public class GetMessageStatisticsHandler : IRequestHandler<GetUserMessageStatisticsQuery, UserMessageStatisticsResponseDto>
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public GetMessageStatisticsHandler(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    public async Task<UserMessageStatisticsResponseDto> Handle(GetUserMessageStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userMessageStatistics = await _messageService.GetUserMessageStatisticsAsync(request.UserId, request.ChatId, cancellationToken);
        return _mapper.Map<UserMessageStatisticsResponseDto>(userMessageStatistics);
    }
}