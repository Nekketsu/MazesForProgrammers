using Mazes.Renderers;
using Mazes.Services.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes.Blazor.Renderers
{
    public class SvgRenderer : IRenderer
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference svgElement;

        public SvgRenderer(IJSRuntime jsRuntime, ElementReference svgElement)
        {
            this.jsRuntime = jsRuntime;
            this.svgElement = svgElement;
        }

        public async Task<IGraphics> CreateAsync(int width, int height)
        {
            var svgGraphics = new SvgGraphics(jsRuntime, svgElement);
            await svgGraphics.SetSizeAsync(width, height);

            return svgGraphics;
        }

        private class SvgGraphics : IGraphics
        {
            private readonly IJSRuntime jsRuntime;
            private readonly ElementReference svgElement;

            public SvgGraphics(IJSRuntime jsRuntime, ElementReference svgElement)
            {
                this.jsRuntime = jsRuntime;
                this.svgElement = svgElement;
            }

            public async Task SetSizeAsync(int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("svg.setSize", new object[] { svgElement, width, height });
            }

            public async Task ClearAsync(Color color)
            {
                await jsRuntime.InvokeAsync<object>("svg.clear", new object[] { svgElement, color.ToWebRgbaColor() });
            }

            public async Task DrawLineAsync(Color pen, int x1, int y1, int x2, int y2)
            {
                await jsRuntime.InvokeAsync<object>("svg.drawLine", new object[] { svgElement, pen.ToWebRgbaColor(), x1, y1, x2, y2 });
            }

            public async Task DrawRectangleAsync(Color pen, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("svg.drawRectangle", new object[] { svgElement, pen.ToWebRgbaColor(), x, y, width, height });
            }

            public async Task FillRectangleAsync(Color brush, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("svg.fillRectangle", new object[] { svgElement, brush.ToWebRgbaColor(), x, y, width - x, height - y });
            }

            public async Task DrawArcAsync(Color pen, int x, int y, int width, int height, float startAngle, float sweepAngle)
            {
                await jsRuntime.InvokeAsync<object>("svg.drawArc", new object[] { svgElement, pen.ToWebRgbaColor(), x, y, width, height, startAngle, sweepAngle });
            }

            public async Task DrawEllipseAsync(Color pen, int x, int y, int width, int height)
            {
                await jsRuntime.InvokeAsync<object>("svg.drawEllipse", new object[] { svgElement, pen.ToWebRgbaColor(), x, y, width, height });
            }

            public async Task FillPolygonAsync(Color pen, Point[] points)
            {
                await jsRuntime.InvokeAsync<object>("svg.fillPolygon", new object[] { svgElement, pen.ToWebRgbaColor(), points.Select(p => new { x = p.X, y = p.Y }) });
            }

            public void Dispose()
            {

            }
        }
    }
}
