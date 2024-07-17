using MediatR;

namespace Promomash.Domain.Common;

public abstract class BaseEvent : INotification
{
    public Dictionary<string, string> MetaData { get; init; } = new();
}
