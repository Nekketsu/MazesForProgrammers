using System.Drawing;
using System.Threading.Tasks;

namespace Mazes.Renderers
{
    public class ImageRenderer : IRenderer
    {
        public Image Image { get; private set; }

        public Task<IGraphics> CreateAsync(int width, int height)
        {
            Image = new Bitmap(width, height);

            return Task.FromResult<IGraphics>(new ImageGraphics(Image));
        }

        private class ImageGraphics : IGraphics
        {
            private Graphics graphics;

            public ImageGraphics(Image image)
            {
                graphics = Graphics.FromImage(image);
            }

            public void Dispose()
            {
                graphics.Dispose();
            }

            public Task ClearAsync(Color color)
            {
                graphics.Clear(color);

                return Task.CompletedTask;
            }

            public Task DrawArcAsync(Color color, int x, int y, int width, int height, float startAngle, float sweepAngle)
            {
                graphics.DrawArc(new Pen(color), x, y, width, height, startAngle, sweepAngle);

                return Task.CompletedTask;
            }

            public Task DrawEllipseAsync(Color color, int x, int y, int width, int height)
            {
                graphics.DrawEllipse(new Pen(color), x, y, width, height);

                return Task.CompletedTask;
            }

            public Task DrawLineAsync(Color color, int x1, int y1, int x2, int y2)
            {
                graphics.DrawLine(new Pen(color), x1, y1, x2, y2);

                return Task.CompletedTask;
            }

            public Task FillPolygonAsync(Color color, Point[] points)
            {
                graphics.FillPolygon(new SolidBrush(color), points);

                return Task.CompletedTask;
            }

            public Task FillRectangleAsync(Color color, int x, int y, int width, int height)
            {
                graphics.FillRectangle(new SolidBrush(color), x, y, width, height);

                return Task.CompletedTask;
            }

            public Task DrawRectangleAsync(Color color, int x, int y, int width, int height)
            {
                graphics.DrawRectangle(new Pen(color), x, y, width, height);

                return Task.CompletedTask;
            }
        }
    }
}
