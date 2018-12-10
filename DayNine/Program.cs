using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DayNine
{
    public class Program : DayBase
    {
        private const string _file = @"C:\Development\Projects\AdventOfCode\DayNine\input.txt";
        public static readonly Regex ParseGame = new Regex(@"(\d*) players; last marble is worth (\d*) points", RegexOptions.Compiled);

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
            Console.WriteLine();
            PrintHeader(2);
            Part2(Lines);
            Console.ReadLine();
        }

        private static void Part1(string[] lines)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                var text = lines[i];
                var match = ParseGame.Match(text);
                var marbleCount = int.Parse(match.Groups[2].Value);
                var numPlayers = int.Parse(match.Groups[1].Value);

                var (playerNumber, score) = RunGame(numPlayers, marbleCount);

                Console.WriteLine($"[Game {i + 1}] The player with the highest score is #{playerNumber} with score: {score}");
            }
        }

        private static void Part2(string[] lines)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                var text = lines[i];
                var match = ParseGame.Match(text);
                var marbleCount = int.Parse(match.Groups[2].Value);
                var numPlayers = int.Parse(match.Groups[1].Value);

                var (playerNumber, score) = RunGame(numPlayers, marbleCount * 100);

                Console.WriteLine($"[Game {i + 1}] The player with the highest score is #{playerNumber} with score: {score}");
            }
        }

        private static (int PlayerNumber, ulong Score) RunGame(int numPlayers, int marbleCount)
        {
            var marbles = Enumerable.Range(1, marbleCount).ToList();
            var players = new List<ulong>(new ulong[numPlayers]);

            var ll = new LinkedList<int>();

            var currentMarble = ll.AddFirst(0);

            var marbleIndex = 0;

            while (marbleIndex < marbles.Count)
            {
                for (var p = 0; p < players.Count && marbleIndex < marbles.Count; p++)
                {
                    var marble = marbles[marbleIndex];

                    if (marble % 23 == 0)
                    {
                        var node = GetNodeBackwardsN(currentMarble, 7);
                        players[p] += (ulong)(marble + node.Value);
                        currentMarble = node.Next;
                        node.List.Remove(node);
                    }
                    else
                    {
                        var node = GetNodeForwardsN(currentMarble, 1);
                        currentMarble = node.List.AddAfter(node, marble);
                    }

                    marbleIndex++;
                }
            }

            var highestTotal = players
                .OrderByDescending(x => x)
                .First();

            return (players.IndexOf(highestTotal) + 1, highestTotal);
        }

        private static LinkedListNode<T> GetNodeForwardsN<T>(LinkedListNode<T> node, int distance)
        {
            var localDist = distance;
            var localNode = node;

            while (localDist > 0)
            {
                localNode = localNode.Next ?? localNode.List.First;

                localDist--;
            }

            return localNode;
        }

        private static LinkedListNode<T> GetNodeBackwardsN<T>(LinkedListNode<T> node, int distance)
        {
            var localDist = distance;
            var localNode = node;

            while (localDist > 0)
            {
                localNode = localNode.Previous ?? localNode.List.Last;

                localDist--;
            }

            return localNode;
        }
    }
}
