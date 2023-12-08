namespace VeteranBot.Gateway.Domain.Entities;

public class AdminEntity 
{
    public required Guid Id { get; init; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}