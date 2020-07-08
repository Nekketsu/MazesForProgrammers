using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class TruePrims : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            return On(grid, grid.RandomCell());
        }

        public IGrid On(IGrid grid, Cell startAt)
        {
            var random = new Random();

            var active = new List<Cell>();
            active.Add(startAt);

            var costs = new Dictionary<Cell, int>();
            foreach (var cell in grid.EachCell())
            {
                costs[cell] = random.Next();
            }

            while (active.Any())
            {
                var cell = active.OrderBy(a => costs[a]).First();
                var availableNeighbors = cell.Neighbors().Where(n => !n.Links.Any());

                if (availableNeighbors.Any())
                {
                    var neighbor = availableNeighbors.OrderBy(n => costs[n]).First();
                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                {
                    active.Remove(cell);
                }
            }

            return grid;
        }
    }
}
