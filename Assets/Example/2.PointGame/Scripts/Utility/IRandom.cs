using UnityEngine;

namespace QFramework.Example
{
    public class IRandomUtil : IUtility
    {
        private static System.Random random = new System.Random();
        public float NextSingle(float min = 0f, float max = 1f)
        {
            return Mathf.Lerp(min, max, (float) random.NextDouble());
        } 
    }

}