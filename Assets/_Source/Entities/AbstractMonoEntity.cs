using _Source.Util;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour
    {
        public bool IsActive => isActiveAndEnabled;
        public Vector3 Position => transform.position;
        public Vector3 LocalPosition => transform.localPosition;

        // ToDo Implement Size
        public Vector2 Size => Vector2.one;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public float GetSquaredDistanceTo(AbstractMonoEntity otherEntity)
        {
            var distance = Position - otherEntity.Position;

            return distance.sqrMagnitude;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
