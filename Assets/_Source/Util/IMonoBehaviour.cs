using UnityEngine;

namespace _Source.Util
{
    public interface IMonoBehaviour
    {
        string Name { get; }
        bool IsActive { get; }
        Vector3 Position { get; }
        Vector3 LocalPosition { get; }
        Quaternion Rotation { get; }

        void SetActive(bool value);
        void SetPosition(Vector3 position);
        void SetLocalPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}