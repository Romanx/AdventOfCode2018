using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DaySeven
{
    public static class Program
    {
        const string inputFile = @"C:/Development/Projects/AdventOfCode/DaySeven/input.txt";
        private static readonly Regex stepRegex = new Regex("Step ([A-Z]) must be finished before step ([A-Z]) can begin.", RegexOptions.Compiled);

        private static Dictionary<string, int> TaskTimes = new Dictionary<string, int>();

        public static void Main(string[] args)
        {
            var allLines = File.ReadLines(inputFile, Encoding.Default);

            var counter = 1;
            for (var i = 'A'; i <= 'Z'; i++)
            {
                TaskTimes[i.ToString()] = 60 + counter++;
            }

            Part1(allLines);
            Part2(allLines);
            Console.ReadLine();
        }

        public static void Part1(IEnumerable<string> lines)
        {
            var steps = lines.Select(l =>
            {
                var match = stepRegex.Match(l);

                return (Name: match.Groups[1].Value, Dependent: match.Groups[2].Value);
            }).ToList();

            var leters = steps.Select(s => s.Name)
                .Concat(steps.Select(s => s.Dependent))
                .Distinct()
                .OrderBy(c => c);

            var startingNodes = leters.Where(l => !steps.Any(s => s.Dependent == l))
                .OrderBy(c => c);

            var stepClone = new List<(string Name, string Dependent)>(steps);

            var stack = new Stack<string>(startingNodes.Reverse());
            var result = string.Empty;

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                result += current;

                var edges = steps
                    .Where(s => s.Name == current);

                foreach (var node in edges)
                {
                    stepClone.Remove(node);
                    if (!stepClone.Any(s => s.Dependent == node.Dependent))
                    {
                        stack.Push(node.Dependent);
                    }
                }

                stack = new Stack<string>(stack.OrderByDescending(c => c));
            }

            if (stepClone.Count > 0)
            {
                Console.WriteLine("Something is wrong...");
            }

            Console.WriteLine($"[ 2 ] Found Result '{result}'");
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var steps = lines.Select(l =>
            {
                var match = stepRegex.Match(l);

                return (Name: match.Groups[1].Value, Dependent: match.Groups[2].Value);
            }).ToList();

            var letters = steps.Select(s => s.Name)
                .Concat(steps.Select(s => s.Dependent))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var workers = new List<int> { 0, 0, 0, 0, 0 };

            var second = 0;
            var doneList = new List<(string Task, int Finish)>();
            var result = new StringBuilder();

            while (letters.Count > 0 || workers.Any(w => w > second))
            {
                var finished = doneList
                    .Where(x => x.Finish <= second)
                    .Select(x => x.Task)
                    .ToList();

                if (finished.Count > 0)
                {
                    result.Append(string.Concat(finished));
                    steps.RemoveAll(x => finished.Contains(x.Name));
                    doneList.RemoveAll(x => x.Finish <= second);
                }

                var valid = letters.Where(s => !steps.Any(d => d.Dependent == s))
                    .ToList();

                for (var i = 0; i < workers.Count && valid.Count > 0; i++)
                {
                    if (workers[i] <= second)
                    {
                        var validTask = valid[0];
                        workers[i] = TaskTimes[validTask] + second;
                        letters.Remove(validTask);
                        doneList.Add((validTask, workers[i]));
                        valid.RemoveAt(0);
                    }
                }

                second++;
            }

            Console.WriteLine($"[ 2 ] Total Time Taken: '{second}'");
            Console.WriteLine($"[ 2 ] Found Result '{result.ToString()}'");
        }

        private static bool TryAssignWorker(string task, int second, Dictionary<int, (int? Time, string Task)> workers)
        {
            var free = workers.FirstOrDefault(w => w.Value == default);

            if (free.Equals(default))
                return false;

            workers[free.Key] = (second + TaskTimes[task], task);
            return true;
        }

        private static bool WorkerHasFinishedTask(int second, Dictionary<int, (int? Time, string Task)> workers, out (int? Time, string Task) task)
        {
            var worker = workers.FirstOrDefault(w => w.Value.Time < second);

            if (!worker.Equals(default) && second >= worker.Value.Time)
            {
                workers[worker.Key] = default;
                task = worker.Value;
                return true;
            }

            task = default;
            return false;
        }

        private static IEnumerable<string> TopologicalSort(IEnumerable<(string Name, string Dependent)> steps)
        {
            var clone = new List<(string Name, string Dependent)>(steps);
            var leters = clone.Select(s => s.Name)
                .Concat(clone.Select(s => s.Dependent))
                .Distinct()
                .OrderBy(c => c);

            var startingNodes = leters.Where(l => !clone.Any(s => s.Dependent == l))
                .OrderBy(c => c);
            var stack = new Stack<string>(startingNodes.Reverse());

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                yield return current;

                var edges = clone
                    .Where(s => s.Name == current)
                    .ToList();

                foreach (var node in edges)
                {
                    clone.Remove(node);
                    if (!clone.Any(s => s.Dependent == node.Dependent))
                    {
                        stack.Push(node.Dependent);
                    }
                }

                stack = new Stack<string>(stack.OrderByDescending(c => c));
            }

        }
    }
}
