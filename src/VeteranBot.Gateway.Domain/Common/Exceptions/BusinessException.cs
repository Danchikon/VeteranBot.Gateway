using VeteranBot.Gateway.Domain.Common.Errors;

namespace VeteranBot.Gateway.Domain.Common.Exceptions;

public class BusinessException : Exception
{
    public required ErrorCode ErrorCode { get; init; }
    public required ErrorKind ErrorKind { get; init; }

    public BusinessException()
    {
        
    }

    public BusinessException(string message) : base(message)
    {
        
    }
}