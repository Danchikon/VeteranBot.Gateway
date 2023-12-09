using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Domain.Entities;

public class UserEntity 
{
    public required string PhoneNumber { get; init; }
    public required string FullName { get; set; }
    public required int Age { get; set; }
    public required string Region { get; set; }
    public required UserType Type { get; set; }
    public DateTime RegistrationDate { get; init; }
    public ICollection<string> BotTypes { get; set; } = Array.Empty<string>(); 
}