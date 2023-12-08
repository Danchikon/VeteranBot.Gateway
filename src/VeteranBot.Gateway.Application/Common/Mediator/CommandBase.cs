using MediatR;

namespace VeteranBot.Gateway.Application.Common.Mediator;

public record CommandBase : IRequest;

public record CommandBase<TResponse> : IRequest<TResponse>;