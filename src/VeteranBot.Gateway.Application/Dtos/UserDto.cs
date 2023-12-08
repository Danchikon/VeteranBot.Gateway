using System.Collections.Immutable;
using NetTopologySuite.Geometries;
using VeteranBot.Gateway.Application.Common.Dtos;

namespace VeteranBot.Gateway.Application.Dtos;

public record UserDto : EntityDtoBase
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? AvatarUrl { get; set; }
    public decimal? LastLoginLongitude { get; init; }
    public decimal? LastLoginLatitude { get; init; }
}