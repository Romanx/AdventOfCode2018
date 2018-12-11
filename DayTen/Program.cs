using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayTen
{
    public class Program : DayBase
    {
        private const string _file = @"C:\Projects\Other\AdventOfCode2018\DayTen\input.txt";

        protected Program(string inputFile)
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
            Part1(Lines);
            Console.ReadLine();
        }

        public void Part1(string[] lines)
        {
            var lights = ParseLights(lines);

            (int minX, int minY, int maxX, int maxY) box = CalculateBoundingBox(lights.Select(l => l.PointAtTime(0)).ToList());
            var second = 1;

            while (true)
            {
                var points = lights.Select(l => l.PointAtTime(second)).ToList();
                var currentBox = CalculateBoundingBox(points);

                if (
                    (currentBox.maxX - currentBox.minX) > (box.maxX - box.minX) ||
                    (currentBox.maxY - currentBox.minY) > (box.maxY - box.minY)
                )
                {
                    break;
                }

                box = currentBox;
                second++;
            }

            var finalGrid = CalculateArray(lights.Select(l => l.PointAtTime(second - 1)).ToList());

            PrintGrid(finalGrid);
            Console.WriteLine($"Seconds to message {second - 1}.");
        }

        public List<LightPoint> ParseLights(string[] lines)
        {
            var list = new List<LightPoint>(lines.Length);
            foreach (var line in lines)
            {
                list.Add(ParseLight(line));
            }

            return list;
        }

        private void PrintGrid(string[,] grid)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
        }

        private (int minX, int minY, int maxX, int maxY) CalculateBoundingBox(List<(int X, int Y)> points)
        {
            var maxX = points
                   .Max(p => p.X);

            var maxY = points
                .Max(p => p.Y);

            var minX = points
                .Min(p => p.X);

            var minY = points
                .Min(p => p.Y);

            return (minX, minY, maxX, maxY);
        }

        private string[,] CalculateArray(List<(int X, int Y)> points)
        {
            var box = CalculateBoundingBox(points);

            var gridX = Math.Abs(box.minX) + box.maxX + 1;
            var gridY = Math.Abs(box.minY) + box.maxY + 1;

            var grid = new string[gridX, gridY];

            var offset = (X: Math.Abs(box.minX), Y: Math.Abs(box.minY));

            for (var x = 0; x < grid.GetLength(0); x++)
            {
                for (var y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = ".";
                }
            }

            foreach (var point in points)
            {
                grid[point.X + offset.X, point.Y + offset.Y] = "#";
            }

            return grid;
        }

        private LightPoint ParseLight(in ReadOnlySpan<char> line)
        {
            var arrowIndex = line.IndexOf('<');
            var span = ParsePoint(line.Slice(arrowIndex), out var point);

            arrowIndex = span.IndexOf('<');
            span = ParsePoint(span.Slice(arrowIndex), out var velocity);

            return new LightPoint(point, velocity);
        }

        private ReadOnlySpan<char> ParsePoint(in ReadOnlySpan<char> span, out (int X, int Y) point)
        {
            var commaIndex = span.IndexOf(',');
            var endBracket = span.IndexOf('>');

            point = (
                int.Parse(span.Slice(1, commaIndex - 1)),
                int.Parse(span.Slice(commaIndex + 1, endBracket - commaIndex - 1))
            );

            return span.Slice(endBracket);
        }
    }

    public class LightPoint
    {
        public (int X, int Y) _point;
        public (int X, int Y) _velocity;

        public LightPoint((int X, int Y) point, (int X, int Y) velocity)
        {
            _point = point;
            _velocity = velocity;
        }

        public (int X, int Y) PointAtTime(int second)
        {
            return (
                _point.X + (_velocity.X * second),
                _point.Y + (_velocity.Y * (second))
            );
        }
    }
}
