using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DayFour
{

    public class Event
    {
        public Event(int id, DateTime timestamp, EventType type)
        {
            Id = id;
            Timestamp = timestamp;
            Type = type;
        }

        public int Id { get; }
        public DateTime Timestamp { get; }
        public EventType Type { get; }
    }
}
