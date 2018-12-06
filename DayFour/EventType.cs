using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DayFour
{
    public enum EventType
    {
        NotSet,
        BeginShift,
        FallsAsleep,
        WakesUp,
    }

    public enum GuardState
    {
        Awake,
        Asleep
    }
}
