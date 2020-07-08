using Mazes;
using Mazes.Algorithms;
using System;

namespace LongestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new DistanceGrid(5, 5);
            new BinaryTree().On(grid);

            var start = grid[0, 0];

            var distances = start.Distances();
            var (newStart, _) = distances.Max();

            var newDistances = newStart.Distances();
            var (goal, distance) = newDistances.Max();

            grid.Distances = newDistances.PathTo(goal);
            Console.WriteLine(grid);
        }
    }
}
