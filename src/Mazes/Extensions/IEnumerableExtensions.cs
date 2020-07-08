using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Extensions
{
    public static class IEnumerableExtensions
    {
        static Random random = new Random();

        public static T Sample<T>(this IEnumerable<T> elements)
        {
            var index = random.Next(elements.Count());

            return elements.ElementAt(index);
        }

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> elements)
        {
            return elements.SelectMany(element => element);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> elements)
        {
            var elementsList = elements.ToList();

            while (elementsList.Any())
            {
                var element = elementsList.Sample();
                elementsList.Remove(element);

                yield return element;
            }
        }
    }
}
