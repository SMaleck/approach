using _Source.Features.Actors.DataComponents;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
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
            if (entity != null && entity != Entity)
            {
                _sensorDataComponent.Add(entity.ActorStateModel);
            }
        }

        private void OnExit(Collider exitedEntity)
        {
            var entity = exitedEntity.GetComponentInParent<IMonoEntity>();
            if (entity != null)
            {
                _sensorDataComponent.Remove(entity.ActorStateModel);
            }
        }

        #region DEBUG
#if UNITY_EDITOR
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
                Handles.Label(transform.position, _sensorDataComponent.KnownEntities.Count.ToString(), EditorStyles.boldLabel);
            }
        }
#endif
    #endregion
    }
}
