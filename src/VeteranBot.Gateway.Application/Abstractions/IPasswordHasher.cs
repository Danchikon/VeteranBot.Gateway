namespace VeteranBot.Gateway.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password);
}