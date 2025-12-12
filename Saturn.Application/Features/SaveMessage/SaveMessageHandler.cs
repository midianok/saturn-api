using AutoMapper;
using MediatR;
using Saturn.Domain.Model;
using Saturn.Domain.Services;

namespace Saturn.Application.Features.SaveMessage;

public class SaveMessageHandler : IRequestHandler<SaveMessageCommand>
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public SaveMessageHandler(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    public Task Handle(SaveMessageCommand request, CancellationToken cancellationToken)
    {
        var telegramMessage = _mapper.Map<TelegramMessage>(request.SaveMessage);
        return _messageService.SaveMessageAsync(telegramMessage, cancellationToken);
    }
}