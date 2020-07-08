using Mazes.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class GrowingTree : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            return On(grid, cells => cells.Sample());
        }

        public IGrid On(IGrid grid, Func<IEnumerable<Cell>, Cell> pickCellFunction)
        {
            return On(grid, grid.RandomCell(), pickCellFunction);
        }

        public IGrid On(IGrid grid, Cell startAt, Func<IEnumerable<Cell>, Cell> pickCellFunction)
        {
            var active = new List<Cell>();
            active.Add(startAt);

            while (active.Any())
            {
                var cell = pickCellFunction(active);
                var availableNeighbors = cell.Neighbors().Where(n => !n.Links.Any());

                if (availableNeighbors.Any())
                {
                    var neighbor = availableNeighbors.Sample();
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
