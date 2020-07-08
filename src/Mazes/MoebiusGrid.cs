using Mazes.Renderers;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

namespace Mazes
{
    public class MoebiusGrid : CylinderGrid
    {
        public MoebiusGrid([DefaultValue(5)] int rows, [DefaultValue(50)] int columns) : base(rows, columns * 2) { }

        public override async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            var paintSteps = new[]
            {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            var gridHeight = cellSize * Rows;
            var midPoint = Columns / 2;

            var imgWidth = cellSize * midPoint;
            var imgHeight = gridHeight * 2;

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell())
                    {
                        var x = (cell.Column % midPoint) * cellSize;
                        var y = cell.Row * cellSize;

                        if (cell.Column >= midPoint)
                        {
                            y += gridHeight;
                        }

                        if (inset < 0)
                        {
                            await RenderWithInsetsAsync(graphics, paintStep, cell, cellSize, wall, x, y, inset);
                        }
                        else
                        {
                            await RenderWithoutInsetsAsync(graphics, paintStep, cell, cellSize, wall, x, y);
                        }
                    }
                }
            }
        }
    }
}
