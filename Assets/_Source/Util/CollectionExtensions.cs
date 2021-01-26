using System;

namespace _Source.Util
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            Array.ForEach(array, action);
        }
    }
}
