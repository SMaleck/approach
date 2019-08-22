using _Source.Util;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour
    {
        public bool IsActive => isActiveAndEnabled;
        public Vector3 Position => transform.position;
        public Vector3 LocalPosition => transform.localPosition;
        public Quaternion Rotation => transform.rotation;

        // ToDo Implement Size
        public Vector2 Size => Vector2.one;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public float GetSquaredDistanceTo(AbstractMonoEntity otherEntity)
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
