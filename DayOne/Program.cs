using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayOne
{
    public class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DayOne/input.txt";

        public static void Main(string[] args)
        {
            IEnumerable<int> AllLines = File.ReadLines(inputFile, Encoding.Default)
                .Select(l => int.Parse(l));

            var frequencyCounts = new HashSet<int>()
            {
                0
            };

            var result = AllLines
                .Aggregate(0, (acc, cur) => cur + acc);

            Console.WriteLine($"Total frequency shift {result}");

            var start = 0;
            var i = 0;
            while(true)
            {
                foreach (var line in AllLines)
                {
                    var newFrequency = start + line;

                    if (frequencyCounts.Contains(newFrequency))
                    {
                        Console.WriteLine($"Found existing frequency: {newFrequency}");
                        goto loopend;
                    }
                    frequencyCounts.Add(newFrequency);
                    start = newFrequency;
                }
            }
            loopend:

            Console.ReadLine();
        }
    }
}
