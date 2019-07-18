using System;

namespace Library.Framework.Core.Time
{
    public static class Clock
    {
        private static IClockProvider _clockProvider = (IClockProvider)new LocalClockProvider();

        public static IClockProvider ClockProvider
        {
            get
            {
                return Clock._clockProvider;
            }
            set
            {
                if (value == null)
                    return;
                Clock._clockProvider = value;
            }
        }

        public static DateTime Now
        {
            get
            {
                return Clock._clockProvider.Now;
            }
        }

        public static DateTime Normalize(DateTime dateTime)
        {
            return Clock._clockProvider.Normalize(dateTime);
        }
    }
}
