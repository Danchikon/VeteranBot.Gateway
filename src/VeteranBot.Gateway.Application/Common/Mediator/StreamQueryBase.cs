using MediatR;

namespace VeteranBot.Gateway.Application.Common.Mediator;

public record StreamQueryBase<TResponse> : IStreamRequest<TResponse>;