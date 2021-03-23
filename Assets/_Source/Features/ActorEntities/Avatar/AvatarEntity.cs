using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    public class AvatarEntity : AbstractMonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, AvatarEntity> { }

        [SerializeField] private Light _headLight;
        public Light HeadLight => _headLight;
    }
}
