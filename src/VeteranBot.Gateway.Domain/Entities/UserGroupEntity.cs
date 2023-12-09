namespace VeteranBot.Gateway.Domain.Entities;

public class UserGroupEntity
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required int UsersPhoneNumbersCount { get; set; }
    public ICollection<string> UsersPhoneNumbers { get; set; } = Array.Empty<string>();
    public DateTime CreatedAt { get; set; }
}