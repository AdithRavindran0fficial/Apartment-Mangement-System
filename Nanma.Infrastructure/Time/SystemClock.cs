using Nanma.Application.Common.Abstractions;

namespace Nanma.Infrastructure.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
