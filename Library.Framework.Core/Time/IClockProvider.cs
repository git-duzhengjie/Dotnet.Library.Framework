using System;

namespace Library.Framework.Core.Time
{
    public interface IClockProvider
    {
        DateTime Now { get; }

        DateTime Normalize(DateTime dateTime);
    }
}
