using Mazes;
using Mazes.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeadEndCounts
{
    class Program
    {
        static void Main(string[] args)
        {
            var iAlgorithm = typeof(IAlgorithm);

            var algorithmTypes = iAlgorithm.Assembly.GetTypes()
                .Where(type => iAlgorithm.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .OrderBy(a => a.Name);

            var tries = 100;
            var size = 20;

            var averages = new Dictionary<string, int>();
            foreach (var algorithmType in algorithmTypes)
            {
                Console.WriteLine($"Running {algorithmType.Name}...");

                var deadEndCounts = new List<int>();
                for (var i = 0; i < tries; i++)
                {
                    var grid = new Grid(size, size);
                    var algorithm = (IAlgorithm)Activator.CreateInstance(algorithmType);
                    algorithm.On(grid);
                    deadEndCounts.Add(grid.DeadEnds().Count());
                }

                var totalDeadEnds = deadEndCounts.Sum();
                averages[algorithmType.Name] = totalDeadEnds / deadEndCounts.Count;
            }

            var totalCells = size * size;
            Console.WriteLine();
            Console.WriteLine($"Average dead-ends per {size}x{size} maze ({totalCells} cells):");

            var sortedAlgorithms = algorithmTypes.Select(a => a.Name).OrderByDescending(a => averages[a]);

            var maxLength = sortedAlgorithms.Max(algorithm => algorithm.Length);
            foreach (var algorithm in sortedAlgorithms)
            {
                var percentage = averages[algorithm] * 100 / (size * size);
                Console.WriteLine($"{algorithm.PadLeft(maxLength + 1)} : {averages[algorithm],3}/{totalCells} {$"({percentage})".PadLeft(4)}%");
            }
        }
    }
}
