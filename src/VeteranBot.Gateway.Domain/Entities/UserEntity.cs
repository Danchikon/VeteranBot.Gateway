using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Domain.Entities;

public class UserEntity 
{
    public required string PhoneNumber { get; init; }
    public required string FullName { get; set; }
    public required ushort Age { get; set; }
    public required bool IsVeteran { get; set; }
    public ICollection<BotType> BotTypes { get; set; } = Array.Empty<BotType>(); 
    public DateTime RegistrationDate { get; init; }
}