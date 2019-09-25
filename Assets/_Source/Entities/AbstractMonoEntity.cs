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
    }
}
