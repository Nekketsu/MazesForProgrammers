using Mazes.Algorithms;
using Mazes.Blazor.Components;
using Mazes.Extensions;
using Mazes.Services;
using Mazes.Services.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes.Blazor.Pages
{
    public partial class MazesGenerator
    {
        public CanvasMaze canvasMaze;
        public SvgMaze svgMaze;

        public Dictionary<string, MazeParameter[]> GridConfigurations { get; set; }
        public string SelectedGridType { get; set; }
        public IEnumerable<string> GridTypes { get; set; }

        public string SelectedAlgorithm { get; set; }
        public IEnumerable<string> Algorithms { get; set; }

        public string SelectedGrowingTreeMethod { get; set; }
        public IEnumerable<string> GrowingTreeMethods { get; set; }


        public int CellSize { get; set; }
        public int Inset { get; set; }

        public bool IsCanvasMazeVisible { get; set; }
        public bool IsSvgMazeVisible { get; set; }

        public IGrid Grid { get; set; }

        readonly MazeGeneratorService mazeGeneratorService;

        Dictionary<string, Func<IEnumerable<Cell>, Cell>> growingTreeMethods;

        public bool IsLoading { get; set; }


        public MazesGenerator()
        {
            var random = new Random();

            growingTreeMethods = new Dictionary<string, Func<IEnumerable<Cell>, Cell>>
            {
                ["Random"] = list => list.Sample(),
                ["Last"] = list => list.Last(),
                ["Mix random-last"] = list => random.Next(2) == 0 ? list.Last() : list.Sample(),
                ["First"] = list => list.First()
            };

            mazeGeneratorService = new MazeGeneratorService();

            GridConfigurations = mazeGeneratorService.GetGrids();
            GridTypes = GridConfigurations.Keys;
            SelectedGridType = GridTypes.FirstOrDefault(type => type == nameof(InterpolatedColoredGrid));

            Algorithms = mazeGeneratorService.GetAlgorithmNames();
            SelectedAlgorithm = Algorithms.SingleOrDefault(algorithm => algorithm == nameof(RecursiveBacktracker));

            GrowingTreeMethods = growingTreeMethods.Keys;
            SelectedGrowingTreeMethod = GrowingTreeMethods.FirstOrDefault();


            CellSize = 20;
            Inset = 2;

            IsCanvasMazeVisible = true;
            IsSvgMazeVisible = true;

            IsLoading = false;
        }

        public async void GenerateMaze()
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1); // Trick to flush the changes (doesn't just work with StateHasChanged())

            Grid = mazeGeneratorService.GenerateMaze(SelectedAlgorithm, SelectedGridType, GridConfigurations[SelectedGridType], growingTreeMethods[SelectedGrowingTreeMethod]);

            IsLoading = false;
            StateHasChanged();
        }

        public async void BraidAsync()
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1); // Trick to flush the changes (doesn't just work with StateHasChanged())

            Grid.Braid();
            await canvasMaze.RefreshAsync();
            await svgMaze.RefreshAsync();

            IsLoading = false;
            StateHasChanged();
        }
    }
}
