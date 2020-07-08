using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Mazes.Renderers
{
    public interface IGraphics : IDisposable
    {
        Task ClearAsync(Color color);
        Task DrawArcAsync(Color pen, int x, int y, int width, int height, float startAngle, float sweepAngle);
        Task DrawEllipseAsync(Color pen, int x, int y, int width, int height);
        Task DrawLineAsync(Color pen, int x1, int y1, int x2, int y2);
        Task DrawRectangleAsync(Color color, int x, int y, int width, int height);
        Task FillPolygonAsync(Color pen, Point[] points);
        Task FillRectangleAsync(Color brush, int x, int y, int width, int height);
    }
}
