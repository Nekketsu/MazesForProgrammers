using Mazes.Services.Models.Parameters;
using Microsoft.AspNetCore.Components;

namespace Mazes.Blazor.Components
{
    public partial class MazeConfigurator
    {
        [Parameter] public MazeParameter[] Parameters { get; set; }
    }
}
