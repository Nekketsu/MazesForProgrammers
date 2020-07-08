using Mazes.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class Ellers : IAlgorithm
    {
        class RowState
        {
            Dictionary<int, List<Cell>> cellsInSet;
            Dictionary<int, int> setForCell;
            int nextSet;

            public RowState(int startingSet = 0)
            {
                cellsInSet = new Dictionary<int, List<Cell>>();
                setForCell = new Dictionary<int, int>();
                nextSet = startingSet;
            }

            public void Record(int set, Cell cell)
            {
                setForCell[cell.Column] = set;

                if (!cellsInSet.ContainsKey(set))
                {
                    cellsInSet[set] = new List<Cell>();
                }
                cellsInSet[set].Add(cell);
            }

            public int SetFor(Cell cell)
            {
                if (!setForCell.ContainsKey(cell.Column))
                {
                    Record(nextSet, cell);
                    nextSet++;
                }

                return setForCell[cell.Column];
            }

            public void Merge(int winner, int loser)
            {
                foreach (var cell in cellsInSet[loser])
                {
                    setForCell[cell.Column] = winner;
                    cellsInSet[winner].Add(cell);
                }

                cellsInSet.Remove(loser);
            }

            public RowState Next()
            {
                return new RowState(nextSet);
            }

            public RowState EachSet(Action<int, List<Cell>> applyFunction)
            {
                foreach (var keyValuePair in cellsInSet)
                {
                    applyFunction(keyValuePair.Key, keyValuePair.Value);
                }

                return this;
            }
        }

        public IGrid On(IGrid grid)
        {
            var random = new Random();

            var rowState = new RowState();

            foreach (var row in grid.EachRow())
            {
                foreach (var cell in row)
                {
                    if (cell.West == null)
                    {
                        continue;
                    }

                    var set = rowState.SetFor(cell);
                    var priorSet = rowState.SetFor(cell.West);

                    var shouldLink = set != priorSet &&
                                     (cell.South == null || random.Next(2) == 0);

                    if (shouldLink)
                    {
                        cell.Link(cell.West);
                        rowState.Merge(priorSet, set);
                    }
                }

                if (row[0].South != null)
                {
                    var nextRow = rowState.Next();

                    rowState.EachSet((set, list) =>
                    {
                        var shuffle = list.Shuffle();
                        for (var index = 0; index < shuffle.Count(); index++)
                        {
                            var cell = shuffle.ElementAt(index);

                            if (index == 0 || random.Next(3) == 0)
                            {
                                cell.Link(cell.South);
                                nextRow.Record(rowState.SetFor(cell), cell.South);
                            }
                        }
                    });

                    rowState = nextRow;
                }
            }

            return grid;
        }
    }
}
