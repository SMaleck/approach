using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities
{
    public class Novatar : AbstractMonoEntity, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Novatar> { }

        private NovatarConfig _novatarConfig;
        private Avatar _avatar;

        private float sqrRange;
        private float sqrTargetReachedThreshold;

        [Inject]
        private void Inject(
            NovatarConfig novatarConfig,
            Avatar avatar)
        {
            _novatarConfig = novatarConfig;
            _avatar = avatar;

            sqrRange = Mathf.Pow(_novatarConfig.Range, 2);
            sqrTargetReachedThreshold = Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            var heading = _avatar.HeadingTo(this);
            var sqrDistance = heading.sqrMagnitude;

            var isInRange = sqrDistance <= sqrRange;
            var isTouching = sqrDistance <= sqrTargetReachedThreshold;


            if (!isInRange || isTouching)
            {
                return;
            }

            Follow(heading);
        }

        private void Follow(Vector3 heading)
        {
            var translateTarget = heading.normalized * _novatarConfig.Speed.AsTimeAdjusted();
            transform.Translate(translateTarget);
        }
    }
}
