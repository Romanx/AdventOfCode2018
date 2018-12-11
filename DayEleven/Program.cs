using AdventOfCode.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace DayEleven
{
    public class Program : DayBase
    {
        private const string _file = @"C:\Projects\Other\AdventOfCode2018\DayEleven\input.txt";
        const int Max = 300;

        public Program(string inputFile) 
            : base(inputFile)
        {
        }

        public static void Main(string[] args)
        {
            new Program(_file).Run();
        }

        public override void Run()
        {
            PrintHeader(1);
            Part1();
            Console.WriteLine();
            PrintHeader(2);
            Part2();
            Console.ReadLine();
        }

        private void Part1()
        {
            var serialNumber = 4455;
            const int size = 3;

            var grid = BuildFuelGrid(serialNumber);

            var best = CalculateBestGrid(grid, size);
            var power = CalculateFuel(grid, best.X, best.Y, size);

            Console.WriteLine($"Best 3x3 found starting at ({best.X}, {best.Y}). Power: {power}");
        }

        private void Part2()
        {
            const int serialNumber = 4455;
            var bag = new ConcurrentBag<(int X, int Y, int Size, int Power)>();
            var grid = BuildFuelGrid(serialNumber);

            Parallel.For(3, 301, gridSize =>
            {
                var best = CalculateBestGrid(grid, gridSize);
                var power = CalculateFuel(grid, best.X, best.Y, gridSize);

                Console.WriteLine($"{gridSize}x{gridSize} done.");

                bag.Add((best.X, best.Y, best.Size, power));
            });

            var (X, Y, Size, Power) = bag.OrderByDescending(x => x.Power)
                .First();

            Console.WriteLine($"Best {Size}x{Size} found starting at ({X}, {Y}). Power: {Power}");
        }

        private int[,] BuildFuelGrid(int serialNumber)
        {
            var grid = new int[Max, Max];

            for (var x = 1; x <= Max;  x++)
            {
                for (var y = 1; y <= Max;  y++)
                {
                    grid[x - 1, y - 1] = CalculatePowerLevel((x, y), serialNumber);
                }
            }

            return grid;
        }

        private (int X, int Y, int Size) CalculateBestGrid(int[,] grid, int size)
        {
            (int X, int Y, int Size) best = default;
            int? maxFuel = null;

            for (var x = 1; x <= Max && (x + size - 1) <= Max; x++)
            {
                for (var y = 1; y <= Max && (y + size - 1) <= Max; y++)
                {
                    var gridFuel = CalculateFuel(grid, x, y, size);
                    if (!maxFuel.HasValue || gridFuel > maxFuel.Value)
                    {
                        maxFuel = gridFuel;
                        best = (x, y, size);
                    }
                }
            }

            return best;
        }

        private int CalculateFuel(int[,] grid, int startX, int startY, int size)
        {
            var fuel = 0;
            for (var x = startX; x < startX + size && x <= Max; x++)
            {
                for (var y = startY; y < startY + size && y <= Max; y++)
                {
                    fuel += grid[x - 1, y - 1];
                }
            }

            return fuel;
        }

        private int CalculatePowerLevel((int X, int Y) cell, int serialNumber)
        {
            var rackId = cell.X + 10;
            var powerLevel = ((rackId * cell.Y) + serialNumber) * rackId;

            return Math.Abs((powerLevel / 100) % 10) - 5;
        }
    }
}
