using UnityEngine;
using Zenject;

namespace _Source.Entities.Avatar
{
    public class AvatarEntity : AbstractMonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, AvatarEntity> { }

        [SerializeField] private Transform _visualRepresentationTransform;
        public Transform VisualRepresentationTransform => _visualRepresentationTransform;

        [SerializeField] private Light _headLight;
        public Light HeadLight => _headLight;
    }
}
