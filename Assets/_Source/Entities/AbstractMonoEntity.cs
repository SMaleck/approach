using _Source.Util;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Vector3 LocalPosition => transform.localPosition;

        public Vector3 HeadingTo(AbstractMonoEntity otherEntity)
        {
            return Position - otherEntity.Position;
        }
    }
}
