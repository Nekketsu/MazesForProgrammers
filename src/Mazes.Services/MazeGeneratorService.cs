using Mazes.Algorithms;
using Mazes.Services.Models.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Mazes.Services
{
    public class MazeGeneratorService
    {
        public IEnumerable<string> GetAlgorithmNames()
        {
            var iAlgorithm = typeof(IAlgorithm);

            var algorithmNames = iAlgorithm.Assembly.GetTypes()
                .Where(type => iAlgorithm.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(type => type.Name);

            return algorithmNames;
        }

        private IAlgorithm CreateAlgorithm(string algorithmName)
        {
            var iAlgorithm = typeof(IAlgorithm);

            var algorithmType = iAlgorithm.Assembly.GetTypes()
                .SingleOrDefault(type => type.Name == algorithmName && iAlgorithm.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            if (algorithmType == null)
            {
                return null;
            }

            var algorithm = (IAlgorithm)Activator.CreateInstance(algorithmType);

            return algorithm;
        }

        public Dictionary<string, MazeParameter[]> GetGrids()
        {
            var iGrid = typeof(IGrid);

            var gridTypes = iGrid.Assembly.GetTypes()
                .Where(type => iGrid.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            var grids = gridTypes
                .Select(gridType => new KeyValuePair<string, MazeParameter[]>(gridType.Name, GetConstructorParameters(gridType)))
                .Where(p => !p.Value.Any(p => p.GetType() == typeof(MazeParameter)));

            return new Dictionary<string, MazeParameter[]>(grids);
        }

        private MazeParameter[] GetConstructorParameters(Type type)
        {
            var constructor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();

            return constructor.GetParameters().Select(GetParameter).ToArray();
        }

        private MazeParameter GetParameter(ParameterInfo parameterInfo)
        {
            var defaultValue = parameterInfo.GetCustomAttribute<DefaultValueAttribute>()?.Value;

            if (parameterInfo.ParameterType == typeof(int))
            {
                return new IntParameter(parameterInfo.Name, defaultValue is int value ? value : default);
            }
            else if (parameterInfo.ParameterType == typeof(Color))
            {
                Color color = defaultValue switch
                {
                    string colorName => Color.FromName(colorName),
                    int colorArgb => Color.FromArgb(colorArgb),
                    KnownColor knownColor => Color.FromKnownColor(knownColor),
                    _ => default
                };
                return new ColorParameter(parameterInfo.Name, color);
            }
            else
            {
                return new MazeParameter(parameterInfo.ParameterType, parameterInfo.Name, defaultValue);
            }
        }

        private IGrid CreateGrid(string gridName, MazeParameter[] parameters)
        {
            var iGrid = typeof(IGrid);

            var gridType = iGrid.Assembly.GetTypes()
                .SingleOrDefault(type => type.Name == gridName && iGrid.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            if (gridType == null)
            {
                return null;
            }

            var gridParameterTypes = parameters.Select(parameter => parameter.Type).ToArray();
            var gridParameters = parameters.Select(parameter => parameter.ObjectValue).ToArray();

            var constructor = gridType.GetConstructor(gridParameterTypes);
            var grid = (IGrid)constructor.Invoke(gridParameters);

            return grid;
        }

        public IGrid GenerateMaze(string algorithmName, string gridName, MazeParameter[] parameters, Func<IEnumerable<Cell>, Cell> growingTreeMethod)
        {
            var algorithm = CreateAlgorithm(algorithmName);
            var grid = CreateGrid(gridName, parameters);

            if (algorithm == null || grid == null)
            {
                return null;
            }

            if (algorithm is GrowingTree growingTree)
            {
                growingTree.On(grid, growingTreeMethod);
            }
            else
            {
                algorithm.On(grid);
            }

            if (grid is InterpolatedColoredGrid interpolatedColoredGrid)
            {
                var start = interpolatedColoredGrid[interpolatedColoredGrid.Rows / 2, interpolatedColoredGrid.Columns / 2];
                interpolatedColoredGrid.Distances = start.Distances();
            }

            return grid;
        }
    }
}
