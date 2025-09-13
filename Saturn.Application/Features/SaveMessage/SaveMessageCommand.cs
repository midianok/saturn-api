using MediatR;
using Saturn.Application.Dtos;

namespace Saturn.Application.Features.SaveMessage;

public record SaveMessageCommand(SaveMessageDto SaveMessage) : IRequest;