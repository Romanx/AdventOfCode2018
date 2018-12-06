using System;
using System.Collections.Generic;
using System.Linq;

namespace DayFour
{

    public static class Parser
    {
        public static List<Event> ParseEvents(IEnumerable<string> lines)
        {
            return ParseEventsInner(OrderLines(lines));
        }

        public static List<Event> ParseEventsInner(List<(DateTime Timestamp, string RestOfLine)> lines)
        {
            var (timestamp, restOfLine) = lines[0];
            if (!TryParseGuardEvent(timestamp, restOfLine, out var @event))
                throw new Exception("Guard event is not the first event!");

            var events = new List<Event>(lines.Count);
            int currentGuard = @event.Id;
            events.Add(@event);

            foreach (var (Timestamp, RestOfLine) in lines.Skip(1))
            {
                var lineSpan = RestOfLine.AsSpan();
                if (TryParseGuardEvent(Timestamp, lineSpan, out @event))
                {
                    currentGuard = @event.Id;
                    events.Add(@event);
                }
                else
                {
                    ParseEventType(lineSpan, out var type);
                    events.Add(new Event(currentGuard, Timestamp, type));
                }
            }

            return events;
        }

        public static List<(DateTime timestamp, string restOfLine)> OrderLines(IEnumerable<string> lines)
        {
            var ordered = new List<(DateTime timestamp, string restOfLine)>();
            foreach (var line in lines)
            {
                var remaining = ParseTimestamp(line, out var timestamp);
                ordered.Add((timestamp, remaining.ToString()));
            }

            return ordered.OrderBy(x => x.timestamp).ToList();
        }

        public static bool TryParseGuardEvent(in DateTime timestamp, in ReadOnlySpan<char> inSpan, out Event @event)
        {
            if (inSpan.IndexOf('#') == -1)
            {
                @event = null;
                return false;
            }

            var span = ParseGuardId(inSpan, out var id);
            ParseEventType(span, out var type);

            @event = new Event(id, timestamp, type);
            return true;
        }

        public static ReadOnlySpan<char> ParseTimestamp(in ReadOnlySpan<char> span, out DateTime dateTime)
        {
            var endBracket = span.IndexOf(']');
            var dateSpan = span.Slice(1, span.IndexOf(']') - 1);

            dateTime = DateTime.Parse(dateSpan.ToString());
            return span.Slice(endBracket + 1);
        }

        public static ReadOnlySpan<char> ParseGuardId(in ReadOnlySpan<char> span, out int id)
        {
            var hashIndex = span.IndexOf('#');
            var spaceIndex = span.Slice(hashIndex).IndexOf(' ');

            id = int.Parse(span.Slice(hashIndex + 1, spaceIndex));
            return span.Slice(hashIndex + spaceIndex + 1);
        }

        public static void ParseEventType(in ReadOnlySpan<char> span, out EventType type)
        {
            var eventTypeString = span.Trim().ToString();

            if (eventTypeString.Equals("begins shift", StringComparison.OrdinalIgnoreCase))
            {
                type = EventType.BeginShift;
                return;
            }

            if (eventTypeString.Equals("falls asleep", StringComparison.OrdinalIgnoreCase))
            {
                type = EventType.FallsAsleep;
                return;
            }

            if (eventTypeString.Equals("wakes up", StringComparison.OrdinalIgnoreCase))
            {
                type = EventType.WakesUp;
                return;
            }

            type = EventType.NotSet;
        }
    }
}
