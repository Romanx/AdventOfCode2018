using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayFour
{
    public static partial class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DayFour/input.txt";

        public static void Main(string[] args)
        {
            var allLines = File.ReadLines(inputFile, Encoding.Default)
                .Where(l => l[0] != '/');

            Part2(allLines);

            Console.ReadLine();
        }

        private static void Part1(IEnumerable<string> allLines)
        {
            var allEvents = Parser.ParseEvents(allLines);

            var guardEvents = ExpandEvents(allEvents);

            var sleepyGuard = guardEvents
                .Where(x => x.State == GuardState.Asleep)
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count())
                .First();

            Console.WriteLine($"Sleepy Guard: #{sleepyGuard.Key} - Asleep {sleepyGuard.Count()} minutes");

            var sleepyMinutes = sleepyGuard
                .GroupBy(x => x.Timestamp.Minute)
                .OrderByDescending(x => x.Count())
                .First();

            Console.WriteLine($"Sleepy Guard: #{sleepyGuard.Key} - Asleep most on minute {sleepyMinutes.Key} ({sleepyMinutes.Count()} times)");

            Console.WriteLine($"Answer is {sleepyGuard.Key * sleepyMinutes.Key}");
        }

        private static void Part2(IEnumerable<string> allLines)
        {
            var allEvents = Parser.ParseEvents(allLines);

            var guardEvents = ExpandEvents(allEvents);

            var guardTotalMinutes = guardEvents
                .Where(x => x.State == GuardState.Asleep)
                .Select(x => (x.Id, x.Timestamp.Minute))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .First();

            var (id, minute) = guardTotalMinutes.Key;

            Console.WriteLine($"Guard asleep the most on the same minute is #{id} and minute {minute}");

            Console.WriteLine($"The result is: {id * minute}");
        }

        public static List<GuardEvent> ExpandEvents(List<Event> events)
        {
            var guardEvents = new List<GuardEvent>(events.Count);
            Guard guard = null;
            DateTime? lastEvent = null;
            foreach (var e in events)
            {
                if (e.Type == EventType.BeginShift)
                {
                    FillUntilEndOfDay();

                    guard = new Guard(e.Id);
                    lastEvent = e.Timestamp;
                    guardEvents.Add(new GuardEvent(guard.Id, e.Timestamp, guard.State));
                    continue;
                }

                if (lastEvent != null)
                {
                    var diff = e.Timestamp - lastEvent.Value;
                    for (var i = 1; i < diff.TotalMinutes; i++)
                    {
                        guardEvents.Add(new GuardEvent(guard.Id, lastEvent.Value.AddMinutes(i), guard.State));
                    }
                }
                guard.ApplyEvent(e);

                guardEvents.Add(new GuardEvent(guard.Id, e.Timestamp, guard.State));
                lastEvent = e.Timestamp;
            }

            FillUntilEndOfDay();

            return guardEvents;

            void FillUntilEndOfDay()
            {
                if (lastEvent != null && lastEvent.Value.Minute != 59)
                {
                    for (var i = 1; i < 60 - lastEvent.Value.Minute; i++)
                    {
                        guardEvents.Add(new GuardEvent(guard.Id, lastEvent.Value.AddMinutes(i), guard.State));
                    }
                }
            }
        }
    }
}
