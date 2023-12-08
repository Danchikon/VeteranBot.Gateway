
namespace VeteranBot.Gateway.Infrastructure.Implementations;

public class AvatarGenerator
{
    public string GenerateUrl()
    {
        return $"https://api.dicebear.com/7.x/thumbs/svg?seed={Guid.NewGuid().ToString()}";
    }
}