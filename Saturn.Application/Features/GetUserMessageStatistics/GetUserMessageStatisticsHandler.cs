using AutoMapper;
using MediatR;
using Saturn.Application.Dtos;
using Saturn.Domain.Services;

namespace Saturn.Application.Features.GetUserMessageStatistics;

public class GetUserMessageStatisticsHandler : IRequestHandler<GetUserMessageStatisticsQuery, GetUserMessageStatisticsResponseDto>
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public GetUserMessageStatisticsHandler(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    public async Task<GetUserMessageStatisticsResponseDto> Handle(GetUserMessageStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userMessageStatistics = await _messageService.GetUserMessageStatisticsAsync(request.UserId, request.ChatId,request.From, request.To, cancellationToken);
        return _mapper.Map<GetUserMessageStatisticsResponseDto>(userMessageStatistics);
    }
}