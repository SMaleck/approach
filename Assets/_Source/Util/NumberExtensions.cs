using UnityEngine;

namespace _Source.Util
{
    public static class NumberExtensions
    {
        public static float AsTimeAdjusted(this float value)
        {
            return value * Time.deltaTime;
        }        
    }
}
