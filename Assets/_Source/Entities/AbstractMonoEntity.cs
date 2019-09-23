using _Source.Util;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour, IMonoEntity
    {
        // ToDo Implement Size correctly, this only works implicitly
        public Vector3 Size => Vector3.one;

        [SerializeField] private Transform _locomotionTarget;
        public Transform LocomotionTarget => _locomotionTarget;

        [SerializeField] private Transform _rotationTarget;
        public Transform RotationTarget => _rotationTarget;

        public string ToDebugString()
        {
            return $"{gameObject.name} | POS {Position.ToString()}";
        }

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
