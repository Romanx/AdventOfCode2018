using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayTwo
{
    public class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DayTwo/input.txt";
        const bool part1 = false;

        public static void Main(string[] args)
        {
            var allLines = File.ReadLines(inputFile, Encoding.Default);

            if (part1)
            {
                var twoCharacters = new HashSet<string>();
                var threeCharacters = new HashSet<string>();

                foreach (var line in allLines)
                {
                    var (twoChars, threeChars) = CountCharacters(line);

                    if (twoChars)
                        twoCharacters.Add(line);

                    if (threeChars)
                        threeCharacters.Add(line);
                }

                Console.WriteLine($"Checksum: {twoCharacters.Count * threeCharacters.Count}");
                Console.ReadLine();
            }

            var (line1, line2) = GetLinesDifferingByOneCharacter(allLines.ToList());

            var common = GetCommonCharacters(line1, line2);

            Console.WriteLine($"Common Characters '{common}'");
            Console.ReadLine();
        }

        private static (string Line1, string Line2) GetLinesDifferingByOneCharacter(List<string> lines)
        {
            var set = new HashSet<string>();
            for (var i = 0; i < lines.Count; i++)
            {
                var line1 = lines[i];
                for (var j = i; j < lines.Count; j++)
                {
                    var line2 = lines[j];

                    if (line1 == line2)
                        continue;

                    if (line1.Length != line2.Length)
                        continue;

                    var diff = 0;

                    for (var x = 0; x < line1.Length; x++)
                    {
                        if (line1[x] != line2[x])
                        {
                            diff++;
                            if (diff > 1)
                            {
                                break;
                            }
                        }
                    }

                    if (diff == 1)
                    {
                        return (line1, line2);
                    }
                }
            }

            throw new Exception("No matching lines!");
        }

        private static string GetCommonCharacters(string line1, string line2)
        {
            var common = new List<char>(line1.Length);

            for (var i = 0; i < line1.Length; i++)
            {
                if (line1[i] == line2[i])
                {
                    common.Add(line1[i]);
                }
            }

            return new string(common.ToArray());
        }

        private static (bool twoChars, bool threeChars) CountCharacters(string line)
        {
            SortedDictionary<char, int> characterCount = new SortedDictionary<char, int>();

            foreach (var character in line)
            {
                characterCount.TryGetValue(character, out int counter);
                characterCount[character] = counter + 1;
            }

            return (
                characterCount.Values.Any(i => i == 2),
                characterCount.Values.Any(i => i == 3)
            );
        }
    }
}
