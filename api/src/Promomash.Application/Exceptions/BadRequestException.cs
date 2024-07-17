using System.Runtime.Serialization;

namespace Promomash.Application.Exceptions;

[Serializable]
public class BadRequestException : CleanArchitectureApplicationException
{
    protected BadRequestException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public BadRequestException()
    { }

    public BadRequestException(string message)
       : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}