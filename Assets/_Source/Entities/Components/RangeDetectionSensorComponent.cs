using _Source.Features.Actors.DataComponents;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Source.Entities.Components
{
    public class RangeDetectionSensorComponent : AbstractMonoComponent
    {
        [SerializeField] private Collider _distanceProbe;

        private SensorDataComponent _sensorDataComponent;

        protected override void OnSetup()
        {
            _sensorDataComponent = ActorStateModel.Get<SensorDataComponent>();
        }

        protected override void OnStart()
        {
            UnityEngine.Debug.LogWarning($"START");
            _distanceProbe.OnTriggerEnterAsObservable()
                .Subscribe(OnEnter)
                .AddTo(Disposer);

            _distanceProbe.OnTriggerExitAsObservable()
                .Subscribe(OnExit)
                .AddTo(Disposer);
        }

        private void OnEnter(Collider enteredEntity)
        {
            var entity = enteredEntity.GetComponentInParent<IMonoEntity>();
            UnityEngine.Debug.LogWarning($"ENTER {entity?.Name}");
            if (entity != null)
            {
                _sensorDataComponent.Add(entity.ActorStateModel);
            }
        }

        private void OnExit(Collider exitedEntity)
        {
            var entity = exitedEntity.GetComponentInParent<IMonoEntity>();
            UnityEngine.Debug.LogWarning($"EXIT {entity?.Name}");
            if (entity != null)
            {
                _sensorDataComponent.Remove(entity.ActorStateModel);
            }
        }

        #region DEBUG

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, ((SphereCollider)_distanceProbe).radius);

            if (_sensorDataComponent?.KnownEntities == null)
            {
                return;
            }
            foreach (var entity in _sensorDataComponent.KnownEntities)
            {
                var transformComponent = entity.Get<TransformDataComponent>();
                Gizmos.DrawLine(transform.position, transformComponent.Position);
            }

        }

        #endregion
    }
}
