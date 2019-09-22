using _Source.Util;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour
    {
        [SerializeField] private Transform _locomotionTarget;
        public Transform LocomotionTarget => _locomotionTarget;

        [SerializeField] private Transform _rotationTarget;
        public Transform RotationTarget => _rotationTarget;

        // ToDo Implement Size correctly, this only works implicitly
        public Vector2 Size => Vector2.one;

        public float GetSquaredDistanceTo(IMonoEntity otherEntity)
        {
            return GetSquaredDistanceTo(otherEntity.Position);
        }

        public float GetSquaredDistanceTo(Vector3 targetPosition)
        {
            var distance = Position - targetPosition;

            return distance.sqrMagnitude;
        }

        public void Translate(float x, float y, float z)
        {
            transform.Translate(x, y, z);
        }
    }
}
