using System.Runtime.Serialization;

namespace Promomash.Application.Exceptions;

[Serializable]
public class UnauthorizedException : CleanArchitectureApplicationException
{
    protected UnauthorizedException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}