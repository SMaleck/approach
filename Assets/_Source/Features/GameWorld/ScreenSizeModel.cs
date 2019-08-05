using _Source.Util;
using UnityEngine;

namespace _Source.Features.GameWorld
{
    public class ScreenSizeModel : AbstractDisposable
    {
        public readonly float WidthUnits;
        public readonly float HeightUnits;

        public float WidthExtendUnits => WidthUnits / 2;
        public float HeightExtendUnits => HeightUnits / 2;

        public ScreenSizeModel()
        {
            HeightUnits = 2f * Camera.main.orthographicSize;
            WidthUnits = HeightUnits * Camera.main.aspect;
        }
    }
}
