using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayThree
{
    public class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DayThree/input.txt";

        public static void Main(string[] args)
        {
            var allLines = File.ReadLines(inputFile, Encoding.Default)
                .Where(l => l[0] != '/');

            Part2(allLines);

            Console.ReadLine();
        }

        public static void Part1(IEnumerable<string> allLines)
        {
            var claims = Parser.ParseClaims(allLines);

            var totalClaimedByMultiple = claims.SelectMany(x => x.Points())
                .GroupBy(c => c)
                .Count(c => c.Count() > 1);

            Console.WriteLine($"Number of overlapping areas: {totalClaimedByMultiple}.");
        }

        public static void Part2(IEnumerable<string> allLines)
        {
            var claims = Parser.ParseClaims(allLines);

            HashSet<int> allIds = new HashSet<int>(claims.Select(c => c.Id));
            var map = new Dictionary<(uint X, uint Y), int>();

            foreach (var claim in claims)
            {
                foreach (var point in claim.Points())
                {
                    if (map.TryGetValue(point, out var existing))
                    {
                        allIds.Remove(existing);
                        allIds.Remove(claim.Id);
                    }

                    map[point] = claim.Id;
                }
            }

            Console.WriteLine($"Remaining claims: {string.Join(", ", allIds.Select(i => $"#{i}"))}");
        }

        private static void PrintFabric(ConcurrentDictionary<(uint X, uint Y), int> map)
        {
            var maxX = (int)map.Max(k => k.Key.X) + 3;
            var maxY = (int)map.Max(k => k.Key.Y) + 3;

            foreach (var y in Enumerable.Range(0, maxY))
            {
                foreach (var x in Enumerable.Range(0, maxX))
                {
                    var m = map.TryGetValue(((uint)x, (uint)y), out var entry)
                        ? entry.ToString()
                        : ".";
                    Console.Write(m);
                }
                Console.WriteLine();
            }
        }
    }
}
