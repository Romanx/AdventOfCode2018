using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayFive
{
    public static class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DayFive/input.txt";

        public static void Main(string[] args)
        {
            var allText = File.ReadAllText(inputFile, Encoding.Default);

            Part2(allText);

            Console.ReadLine();
        }

        private static void Part1(string line)
        {
            var builder = new StringBuilder(line);
            CollapsePolymers(builder);

            Console.WriteLine($"Remaining in buffer: '{builder}' and length {builder.Length}.");
        }

        private static void Part2(string line)
        {
            var stopwatch = Stopwatch.StartNew();
            var distinctChars = line
                .Select(c => char.ToLowerInvariant(c))
                .Distinct()
                .ToList();

            Console.WriteLine($"Found {distinctChars.Count} distinct characters");
            var bag = new ConcurrentBag<string>();

            Parallel.ForEach(distinctChars, @char =>
            {
                var builder = new StringBuilder(line);
                RemoveCharacter(builder, @char);
                CollapsePolymers(builder);

                bag.Add(builder.ToString());
            });

            var result = bag
                .OrderBy(x => x.Length)
                .First();

            stopwatch.Stop();
            Console.WriteLine($"Best polymer is '{result}' and length {result.Length}.");
            Console.WriteLine($"Time taken {stopwatch.Elapsed}");
        }

        public static void RemoveCharacter(StringBuilder builder, char @char)
        {
            for (var i = 0; i < builder.Length; i++)
            {
                var current = builder[i];
                if (current == @char || current == char.ToUpperInvariant(@char))
                {
                    builder.Remove(i, 1);
                    i--;
                }
            }
        }

        public static void CollapsePolymers(StringBuilder builder)
        {
            for (var i = 0; i < builder.Length; i++)
            {
                var current = builder[i];

                if (i + 1 >= builder.Length)
                {
                    break;
                }

                var next = builder[i + 1];
                var isUpper = char.IsUpper(current);
                var sameCharacter = char.ToLowerInvariant(current)
                    == char.ToLowerInvariant(next);

                if (((char.IsUpper(current) && char.IsLower(next)) || (char.IsLower(current) && char.IsUpper(next))) && sameCharacter)
                {
                    builder.Remove(i, 2);
                    i = i - 2 < 0
                        ? - 1
                        : i - 2;
                }
            }
        }
    }
}
