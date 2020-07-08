using Mazes.Renderers;
using Mazes.Services.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes.Blazor.Renderers
{
    public class CanvasRenderer : IRenderer
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference canvasElement;

        public CanvasRenderer(IJSRuntime jsRuntime, ElementReference canvasElement)
        {
            this.jsRuntime = jsRuntime;
            this.canvasElement = canvasElement;
        }

        public async Task<IGraphics> CreateAsync(int width, int height)
        {
            var canvasGraphics = new CanvasGraphics(jsRuntime, canvasElement);
            await canvasGraphics.SetSizeAsync(width, height);

            return canvasGraphics;
        }

        private class CanvasGraphics : IGraphics
        {
            private readonly IJSRuntime jsRuntime;
            private readonly ElementReference canvasElement;

            public CanvasGraphics(IJSRuntime jsRuntime, ElementReference canvasElement)
            {
                this.jsRuntime = jsRuntime;
                this.canvasElement = canvasElement;
            }

            public async Task SetSizeAsync(int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("canvas.setSize", new object[] { canvasElement, width, height });
            }

            public async Task ClearAsync(Color color)
            {
                await jsRuntime.InvokeAsync<object>("canvas.clear", new object[] { canvasElement, color.ToWebRgbaColor() });
            }

            public async Task DrawLineAsync(Color pen, int x1, int y1, int x2, int y2)
            {
                await jsRuntime.InvokeAsync<object>("canvas.drawLine", new object[] { canvasElement, pen.ToWebRgbaColor(), x1, y1, x2, y2 });
            }

            public async Task DrawRectangleAsync(Color pen, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("canvas.drawRectangle", new object[] { canvasElement, pen.ToWebRgbaColor(), x, y, width, height });
            }

            public async Task FillRectangleAsync(Color brush, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("canvas.fillRectangle", new object[] { canvasElement, brush.ToWebRgbaColor(), x, y, width - x, height - y });
            }

            public async Task DrawArcAsync(Color pen, int x, int y, int width, int height, float startAngle, float sweepAngle)
            {
                await jsRuntime.InvokeAsync<object>("canvas.drawArc", new object[] { canvasElement, pen, x, y, width, height, startAngle, sweepAngle });
            }

            public async Task DrawEllipseAsync(Color pen, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("canvas.drawEllipse", new object[] { canvasElement, pen.ToWebRgbaColor(), x, y, width, height });
            }

            public async Task FillPolygonAsync(Color pen, Point[] points)
            {
                await jsRuntime.InvokeAsync<object>("canvas.fillPolygon", new object[] { canvasElement, pen.ToWebRgbaColor(), points.Select(p => new { x = p.X, y = p.Y }) });
            }

            public void Dispose()
            {

            }
        }
    }
}
