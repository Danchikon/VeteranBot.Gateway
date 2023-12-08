using MediatR;

namespace VeteranBot.Gateway.Application.Common.Mediator;

public record QueryBase<TResponse> : IRequest<TResponse>;