using System;
using _Source.Util;
using UnityEngine;

namespace _Source.Features.ScreenSize
{
    public class ScreenSizeController : AbstractDisposable
    {
        private readonly ScreenSizeModel _screenSizeModel;

        public ScreenSizeController(ScreenSizeModel screenSizeModel)
        {
            _screenSizeModel = screenSizeModel;
        }

        // ToDo V1 This needs to respect the offset as well
        public bool IsOutOfScreenBounds(Vector3 position, Vector2 size)
        {
            var screenSides = EnumHelper<ScreenSide>.Iterator;
            foreach (var side in screenSides)
            {
                if (IsOutOf(side, position, size))
                {
                    return true;
                }
            }

            return false;
        }

        public Vector3 GetRandomizedOutOfBoundsPosition(
            ScreenSide side, 
            Vector2 size,
            float offset = 0)
        {
            var halfSize = GetHalfSizeFor(side, size) + offset;
            var randomComponent = GetRandomComponentFor(side, size);

            switch (side)
            {
                case ScreenSide.Top:
                    return new Vector3(randomComponent, _screenSizeModel.HeightExtendUnits + halfSize, 0);

                case ScreenSide.Bottom:
                    return new Vector3(randomComponent, -(_screenSizeModel.HeightExtendUnits + halfSize), 0);

                case ScreenSide.Left:
                    return new Vector3(-(_screenSizeModel.WidthExtendUnits + halfSize), randomComponent, 0);

                case ScreenSide.Right:
                    return new Vector3(_screenSizeModel.WidthExtendUnits + halfSize, randomComponent, 0);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsOutOf(ScreenSide side, Vector3 position, Vector2 size)
        {
            var halfSize = GetHalfSizeFor(side, size);

            switch (side)
            {
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    return Math.Abs(position.y) > _screenSizeModel.HeightExtendUnits + halfSize;

                case ScreenSide.Left:
                case ScreenSide.Right:
                    return Math.Abs(position.x) > _screenSizeModel.WidthExtendUnits + halfSize;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private float GetHalfSizeFor(ScreenSide spawnSide, Vector2 entitySize)
        {
            switch (spawnSide)
            {
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    return entitySize.y / 2;

                case ScreenSide.Left:
                case ScreenSide.Right:
                    return entitySize.x / 2;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetRandomComponentFor(ScreenSide side, Vector2 size)
        {
            switch (side)
            {
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    var maxWidthExtend = _screenSizeModel.WidthExtendUnits - (size.x / 2);
                    return UnityEngine.Random.Range(-maxWidthExtend, maxWidthExtend);

                case ScreenSide.Left:
                case ScreenSide.Right:
                    var maxHeightExtend = _screenSizeModel.HeightExtendUnits - (size.y / 2);
                    return UnityEngine.Random.Range(-maxHeightExtend, maxHeightExtend);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
