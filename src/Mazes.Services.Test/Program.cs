using System.Threading.Tasks;

namespace Mazes.Services.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new MazeGeneratorService();
            var grids = generator.GetGrids();

            var algorithmName = "RecursiveBacktracker";
            var gridName = nameof(InterpolatedColoredGrid);
            var parameters = grids[gridName];

            var grid = (InterpolatedColoredGrid)generator.GenerateMaze(algorithmName, gridName, parameters, null);

            var image = await grid.ToImageAsync();
            image.Save("Mazes.Services.Test.png");
        }
    }
}
