using System;
using System.IO;
using System.Text;

namespace AdventOfCode.Core
{
    public abstract class DayBase
    {
        private readonly string _inputFile;
        protected string[] Lines => File.ReadAllLines(_inputFile, Encoding.Default);
        protected string AllText => File.ReadAllText(_inputFile, Encoding.Default);

        protected DayBase(string inputFile)
        {
            _inputFile = inputFile;
        }

        public static void PrintHeader(int number)
        {
            var part = $"[Part {number}]";
            var sides = (Console.WindowWidth / 2) - part.Length;
            var side = new string('-', sides);

            Console.WriteLine($"{side}{part}{side}");
        }

        public abstract void Run();
    }
}
