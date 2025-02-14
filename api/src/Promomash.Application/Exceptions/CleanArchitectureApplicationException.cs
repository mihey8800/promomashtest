﻿using System.Runtime.Serialization;

namespace Promomash.Application.Exceptions;

[Serializable]
public class CleanArchitectureApplicationException : Exception
{
    protected CleanArchitectureApplicationException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public CleanArchitectureApplicationException()
    { }

    public CleanArchitectureApplicationException(string message)
       : base(message)
    {
    }

    public CleanArchitectureApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}