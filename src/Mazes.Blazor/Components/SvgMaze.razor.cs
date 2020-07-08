using Mazes.Blazor.Renderers;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Mazes.Blazor.Components
{
    public partial class SvgMaze
    {
        [Parameter] public IGrid Grid { get; set; }
        [Parameter] public int CellSize { get; set; }
        [Parameter] public int Inset { get; set; }

        public ElementReference SvgElement { get; set; }

        SvgRenderer renderer;

        private IGrid previousGrid;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                renderer = new SvgRenderer(JSRuntime, SvgElement);
            }

            if (Grid != previousGrid)
            {
                await RefreshAsync();
                previousGrid = Grid;
            }
        }

        public async Task RefreshAsync()
        {
            await Grid.RenderAsync(renderer, CellSize, Inset);
        }
    }
}
