using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarEntity : MonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, NovatarEntity> { }

        [SerializeField] private Light _headLight;
        public Light HeadLight => _headLight;
    }
}
