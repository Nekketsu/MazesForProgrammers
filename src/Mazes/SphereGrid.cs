using Mazes.Extensions;
using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class SphereGrid : IGrid
    {
        Random random;
        public int Rows { get; protected set; }
        public int Equator { get; }

        public int Size => throw new NotImplementedException();

        const int Hemispheres = 2;

        HemisphereGrid[] grid;

        public SphereGrid([DefaultValue(20)] int rows)
        {
            random = new Random();

            if (rows % Hemispheres != 0)
            {
                throw new ArgumentException("Argument must be an even number");
            }

            Rows = rows;
            Equator = Rows / Hemispheres;

            grid = PrepareGrid();
            ConfigureCells();
        }

        private HemisphereGrid[] PrepareGrid()
        {
            var grid = Enumerable.Range(0, Hemispheres).Select(id => new HemisphereGrid(id, Equator)).ToArray();

            return grid;
        }

        private void ConfigureCells()
        {
            var belt = Equator - 1;
            for (var index = 0; index < GetSize(belt); index++)
            {
                var a = this[0, belt, index];
                var b = this[1, belt, index];

                a.Outward.Add(b);
                b.Outward.Add(a);
            }
        }

        private HemisphereCell this[int hemisphere, int row, int column] => (HemisphereCell)grid[hemisphere][row, column];

        private int GetSize(int row)
        {
            return grid[0].GetSize(row);
        }

        public IEnumerable<Cell> EachCell()
        {
            return grid.SelectMany(hg => hg.EachCell());
        }

        public IEnumerable<Cell[]> EachRow()
        {
            return grid.SelectMany(hg => hg.EachRow());
        }

        public Cell RandomCell()
        {
            return grid.Sample().RandomCell();
        }

        public virtual async Task<Image> ToImageAsync(int cellSize = 10, int inset = 0)
        {
            var renderer = new ImageRenderer();

            await RenderAsync(renderer, cellSize, inset);

            return renderer.Image;
        }

        public async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            var idealSize = cellSize;

            var imgHeight = idealSize * Rows;
            var imgWidth = grid[0].GetSize(Equator - 1) * idealSize;

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var cell in EachCell().Cast<HemisphereCell>())
                {
                    var rowSize = GetSize(cell.Row);
                    var cellWidth = (double)imgWidth / rowSize;

                    var x1Double = cell.Column * cellWidth;
                    var x2Double = x1Double + cellWidth;

                    var y1 = cell.Row * idealSize;
                    var y2 = y1 + idealSize;

                    if (cell.Hemisphere > 0)
                    {
                        y1 = imgHeight - y1;
                        y2 = imgHeight - y2;
                    }

                    var x1 = (int)Math.Round(x1Double);
                    var x2 = (int)Math.Round(x2Double);

                    if (cell.Row > 0)
                    {
                        if (!cell.Linked(cell.Cw))
                        {
                            await graphics.DrawLineAsync(wall, x2, y1, x2, y2);
                        }
                        if (!cell.Linked(cell.Inward))
                        {
                            await graphics.DrawLineAsync(wall, x1, y1, x2, y1);
                        }
                    }

                    if (cell.Hemisphere == 0 && cell.Row == Equator - 1)
                    {
                        if (!cell.Linked(cell.Outward[0]))
                        {
                            await graphics.DrawLineAsync(wall, x1, y2, x2, y2);
                        }
                    }
                }
            }
        }
        public IEnumerable<Cell> DeadEnds()
        {
            var list = new List<Cell>();

            foreach (var cell in EachCell())
            {
                if (cell.Links.Count == 1)
                {
                    list.Add(cell);
                }
            }

            return list;
        }

        public void Braid(float p = 1f)
        {
            foreach (var cell in DeadEnds().Shuffle())
            {
                if (cell.Links.Count != 1 || random.NextDouble() > p)
                {
                    continue;
                }

                var neighbors = cell.Neighbors().Where(n => !cell.Linked(n));
                var best = neighbors.Where(n => n.Links.Count == 1);
                if (!best.Any())
                {
                    best = neighbors;
                }

                var neighbor = best.Sample();
                cell.Link(neighbor);
            }
        }
    }
}
