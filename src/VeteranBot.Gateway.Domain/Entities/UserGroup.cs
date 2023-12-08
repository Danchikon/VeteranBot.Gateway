namespace VeteranBot.Gateway.Domain.Entities;

public class UserGroup
{
    public required string Title { get; set; }
    public ICollection<string> UsersPhoneNumbers { get; set; } = Array.Empty<string>();
    public DateTime CreatedAt { get; set; }
}