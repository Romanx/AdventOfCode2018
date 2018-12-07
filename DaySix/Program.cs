using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaySix
{
    public static class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DaySix/input.txt";

        public static void Main(string[] args)
        {
            var allLines = File.ReadLines(inputFile, Encoding.Default);

            var points = allLines.Select((l, idx) =>
            {
                var split = l.Split(',');

                return (Id: idx, X: int.Parse(split[0]), Y: int.Parse(split[1]));
            }).ToList();

            var maxX = points.Max(p => p.X) + 1;
            var maxY = points.Max(p => p.Y) + 1;

            Part1(points, maxX, maxY);
            Part2(points, maxX, maxY);

            Console.ReadLine();
        }

        public static void Part1(List<(int Id, int X, int Y)> points, int maxX, int maxY)
        {
            var grid = new int[maxX, maxY];

            var map = new ConcurrentDictionary<int, int>(points.Select(p => new KeyValuePair<int, int>(p.Id, 0)));

            Parallel.ForEach(GetPoints(maxX, maxY), (cell) =>
            {
                int best = int.MaxValue;
                int cellId = -1;
                foreach (var (Id, X, Y) in points)
                {
                    var dist = ManhattanDistance((X, Y), cell);

                    if (dist == 0)
                    {
                        cellId = Id;
                        break;
                    }

                    if (dist < best)
                    {
                        best = dist;
                        cellId = Id;
                    }
                    else if (dist == best)
                    {
                        cellId = 0;
                    }
                }

                if (cellId != -1)
                {
                    grid[cell.X, cell.Y] = cellId;
                    if (cellId != 0)
                    {
                        map.AddOrUpdate(cellId, 1, (_, v) => v + 1);
                    }
                }
            });

            RemoveInfinite(map, grid);

            var largestArea = map
                .OrderByDescending(x => x.Value)
                .First();

            Console.WriteLine($"Largest found #{largestArea.Key}, Area - {largestArea.Value}");
        }

        public static void Part2(List<(int Id, int X, int Y)> points, int maxX, int maxY)
        {
            ConcurrentDictionary<(int X, int Y), int> region = new ConcurrentDictionary<(int X, int Y), int>();

            const int largestArea = 10000;

            Parallel.ForEach(GetPoints(maxX, maxY), (cell) =>
            {
                var sum = 0;

                foreach (var (Id, X, Y) in points)
                {
                    var dist = ManhattanDistance((X, Y), cell);

                    sum += dist;
                }

                if (sum < largestArea)
                {
                    region.AddOrUpdate(cell, sum, (_, v) => v);
                }
            });

            var regionSize = region.Count;

            Console.WriteLine($"Region size is {regionSize}");
        }

        private static void RemoveInfinite(ConcurrentDictionary<int, int> map, int[,] grid)
        {
            var maxX = grid.GetLength(0);
            var maxY = grid.GetLength(1);

            for (var x = 0; x < maxX; x++)
            {
                var id = grid[x, 0];
                map.Remove(id, out var _);
            }

            for (var x = 0; x < maxX; x++)
            {
                var id = grid[x, maxY - 1];
                map.Remove(id, out var _);
            }

            for (var y = 0; y < maxY; y++)
            {
                var id = grid[0, y];
                map.Remove(id, out var _);
            }

            for (var y = 0; y < maxY; y++)
            {
                var id = grid[maxX - 1, y];
                map.Remove(id, out var _);
            }
        }

        private static IEnumerable<(int X, int Y)> GetPoints(int maxX, int maxY)
        {
            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    yield return(x, y);
                }
            }
        }

        public static string Display(char[,] grid)
        {
            var builder = new StringBuilder();
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    builder.Append(grid[x, y]);
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static int ManhattanDistance((int X, int Y) p1, (int X, int Y) p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
}
