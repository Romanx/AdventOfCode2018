using AdventOfCode.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace DayEight
{
    public class Program : DayBase
    {
        private const string _file = @"C:\Development\Projects\AdventOfCode\DayEight\input.txt";

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
            Part1(AllText);
            Console.WriteLine();
            PrintHeader(2);
            Part2(AllText);
            Console.ReadLine();
        }

        private static void Part1(string text)
        {
            var node = ParseNodes(text);

            var sum = SumMetadata(node);

            Console.WriteLine($"Total metadata sum: {sum}");

            int SumMetadata(Node _node)
            {
                var start = _node.Metadata.Sum();
                foreach (var child in _node.Children)
                {
                    start += SumMetadata(child);
                }

                return start;
            }
        }

        private static void Part2(string text)
        {
            var node = ParseNodes(text);

            var nodeValue = CalculateNodeValue(node);

            Console.WriteLine($"Calculated node value: {nodeValue}");
        }

        private static Node ParseNodes(string text)
        {
            var ret = ParseNode(text.AsSpan(), out var node);
            Debug.Assert(ret.IsEmpty);

            return node;
        }

        private static ReadOnlySpan<char> ParseNode(in ReadOnlySpan<char> str, out Node node)
        {
            var localStr = str
                .ParseInt(out var childQuantity)
                .ParseInt(out var metadataQuantity);

            var children = new Node[childQuantity];

            for (var i = 0; i < childQuantity; i++)
            {
                var span = ParseNode(localStr, out var childNode);
                children[i] = childNode;
                localStr = span;
            }

            var metadata = new int[metadataQuantity];
            for (var i = 0; i < metadataQuantity; i++)
            {
                localStr = localStr.ParseInt(out var meta);
                metadata[i] = meta;
            }

            node = new Node { Children = children, Metadata = metadata };
            return localStr;
        }

        private static int CalculateNodeValue(Node node)
        {
            if (node.Children.Length == 0)
            {
                var x = 0;
                foreach (var meta in node.Metadata)
                    x += meta;

                return x;
            }

            var value = 0;

            foreach (var meta in node.Metadata)
            {
                if (meta == 0 || meta > node.Children.Length)
                {
                    continue;
                }

                var child = node.Children[meta - 1];
                value += CalculateNodeValue(child);
            }

            return value;
        }
    }
}
