using System;

namespace Mazes.Services.Models.Parameters
{
    public class MazeParameter
    {
        public Type Type { get; }
        public string Name { get; }
        public virtual object ObjectValue { get; }

        public MazeParameter(Type type, string name, object objectValue)
        {
            Type = type;
            Name = name;
            ObjectValue = objectValue;
        }

        public MazeParameter(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
