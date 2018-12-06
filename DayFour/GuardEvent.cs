using System;

namespace DayFour
{
    public static partial class Program
    {
        public class GuardEvent
        {
            public GuardEvent(int id, DateTime timestamp, GuardState state)
            {
                Id = id;
                Timestamp = timestamp;
                State = state;
            }

            public int Id { get; }
            public DateTime Timestamp { get; }
            public GuardState State { get; }
        }
    }
}
