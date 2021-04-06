namespace _Source.Services.Random
{
    public class RandomNumberService : IRandomNumberService
    {
        /// <summary>
        /// Return a random integer between minInclusive and maxExclusive
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxExclusive"></param>
        /// <returns></returns>
        public int Range(int minInclusive, int maxExclusive)
        {
            return UnityEngine.Random.Range(minInclusive, maxExclusive);
        }

        /// <summary>
        /// Return a random float between minInclusive and maxInclusive
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxInclusive"></param>
        /// <returns></returns>
        public float Range(float minInclusive, float maxInclusive)
        {
            return UnityEngine.Random.Range(minInclusive, maxInclusive);
        }

        /// <summary>
        /// Return a random double between minInclusive and maxInclusive
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxInclusive"></param>
        /// <returns></returns>
        public double Range(double minInclusive, double maxInclusive)
        {
            return UnityEngine.Random.Range((float)minInclusive, (float)maxInclusive);
        }

        public T FromSet<T>(T[] set)
        {
            var index = Range(0, set.Length);
            return set[index];
        }
    }
}
