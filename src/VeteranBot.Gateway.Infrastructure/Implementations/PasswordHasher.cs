using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Infrastructure.Options;

namespace VeteranBot.Gateway.Infrastructure.Implementations;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherOptions _options;

    public PasswordHasher(IOptions<PasswordHasherOptions> options)
    {
        _options = options.Value;
    }

    public string Hash(string password)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes(_options.Salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _options.IterationCount,
            numBytesRequested: 256 / 8
        ));

        return hashed;
    }
}