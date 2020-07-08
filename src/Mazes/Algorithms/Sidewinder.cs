using Mazes.Extensions;
using System;
using System.Collections.Generic;

namespace Mazes.Algorithms
{
    public class Sidewinder : IAlgorithm
    {
        Random random = new Random();

        public IGrid On(IGrid grid)
        {
            foreach (var row in grid.EachRow())
            {
                var run = new List<Cell>();
                foreach (var cell in row)
                {
                    run.Add(cell);

                    var atEasternBoundary = cell.East == null;
                    var atNorthenBoundary = cell.North == null;

                    var shouldCloseOut = atEasternBoundary
                        || (!atNorthenBoundary && random.Next(2) == 0);

                    if (shouldCloseOut)
                    {
                        var member = run.Sample();
                        if (member.North != null)
                        {
                            member.Link(member.North);
                        }
                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.East);
                    }
                }
            }

            return grid;
        }
    }
}
