using Mazes.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class Kruskals : IAlgorithm
    {
        public class State
        {
            Random random;

            public List<(Cell left, Cell right)> Neighbors { get; }

            private readonly IGrid grid;
            Dictionary<Cell, int> setForCell;
            Dictionary<int, HashSet<Cell>> cellsInSet;

            public State(IGrid grid)
            {
                random = new Random();

                this.grid = grid;

                Neighbors = new List<(Cell left, Cell right)>();
                setForCell = new Dictionary<Cell, int>();
                cellsInSet = new Dictionary<int, HashSet<Cell>>();

                foreach (var cell in grid.EachCell())
                {
                    var set = setForCell.Count;

                    setForCell.Add(cell, set);
                    cellsInSet.Add(set, new HashSet<Cell> { cell });

                    if (cell.South != null)
                    {
                        Neighbors.Add((cell, cell.South));
                    }
                    if (cell.East != null)
                    {
                        Neighbors.Add((cell, cell.East));
                    }
                }
            }

            public bool CanMerge(Cell left, Cell right)
            {
                return left != null && right != null &&
                       setForCell[left] != setForCell[right];
            }

            public void Merge(Cell left, Cell right)
            {
                left.Link(right);

                var winner = setForCell[left];
                if (setForCell.TryGetValue(right, out var loser))
                {
                    var losers = cellsInSet[loser] ?? new HashSet<Cell> { right };

                    foreach (var cell in losers)
                    {
                        cellsInSet[winner].Add(cell);
                        setForCell[cell] = winner;
                    }

                    cellsInSet.Remove(loser);
                }
            }

            public bool AddCrossing(OverCell cell)
            {
                var grid = (WeaveGrid)this.grid;

                if (cell.Links.Any() ||
                    !CanMerge(cell.East, cell.West) ||
                    !CanMerge(cell.North, cell.South))
                {
                    return false;
                }

                Neighbors.RemoveAll(n => n.left == cell || n.right == cell);

                if (random.Next(2) == 0)
                {
                    Merge(cell.West, cell);
                    Merge(cell, cell.East);

                    grid.TunnelUnder(cell);
                    Merge(cell.North, cell.North.South);
                    Merge(cell.South, cell.South.North);
                }
                else
                {
                    Merge(cell.North, cell);
                    Merge(cell, cell.South);

                    grid.TunnelUnder(cell);
                    Merge(cell.West, cell.West.East);
                    Merge(cell.East, cell.East.West);
                }

                return true;
            }
        }

        public IGrid On(IGrid grid)
        {
            return On(grid, new State(grid));
        }

        public IGrid On(IGrid grid, State state)
        {
            var neighbors = state.Neighbors.Shuffle().ToList();

            while (neighbors.Any())
            {
                var (left, right) = neighbors.Last();
                neighbors.RemoveAt(neighbors.Count - 1);

                if (state.CanMerge(left, right))
                {
                    state.Merge(left, right);
                }
            }

            return grid;
        }
    }
}
