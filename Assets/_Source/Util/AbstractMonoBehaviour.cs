using UnityEngine;

namespace _Source.Util
{
    public class AbstractMonoBehaviour : MonoBehaviour, IMonoBehaviour
    {
        public virtual string Name => gameObject.name;
        public virtual bool IsActive => isActiveAndEnabled;

        public Vector3 Position => transform.position;
        public Vector3 LocalPosition => transform.localPosition;
        public Quaternion Rotation => transform.rotation;

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
    }
}
