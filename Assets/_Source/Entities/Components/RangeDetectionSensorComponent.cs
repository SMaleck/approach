using _Source.Debug;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Sensors;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

namespace _Source.Entities.Components
{
    public class RangeDetectionSensorComponent : AbstractMonoComponent
    {
        [SerializeField] private SensorType _type;
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
                _sensorDataComponent.Add(entity.ActorStateModel, _type);
            }
        }

        private void OnExit(Collider exitedEntity)
        {
            var entity = exitedEntity.GetComponentInParent<IMonoEntity>();
            if (entity != null)
            {
                _sensorDataComponent.Remove(entity.ActorStateModel, _type);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawRange();

            if (_sensorDataComponent == null)
            {
                return;
            }

            switch (_type)
            {
                case SensorType.Visual:
                    DebugDrawVisual();
                    DrawCountLabel(DebugConstants.SensorVisualLabelOffset);
                    break;
                case SensorType.Touch:
                    DebugDrawTouch();
                    DrawCountLabel(DebugConstants.SensorTouchLabelOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawRange()
        {
            Gizmos.DrawWireSphere(
                transform.position,
                ((SphereCollider)_distanceProbe).radius);
        }

        private void DrawCountLabel(Vector3 labelOffset)
        {
            var knownEntities = _sensorDataComponent.GetInRange(_type);

            Handles.Label(
                transform.position + labelOffset,
                $"{_type}: {knownEntities.Count} | AVTR: {_sensorDataComponent.IsAvatarInRange(_type)}",
                EditorStyles.boldLabel);
        }

        private void DebugDrawVisual()
        {
            var resetColor = Gizmos.color;
            Gizmos.color = Color.magenta;

            var knownEntities = _sensorDataComponent.GetInRange(_type);
            foreach (var entity in knownEntities)
            {
                var transformComponent = entity.Get<TransformDataComponent>();
                Gizmos.DrawLine(transform.position, transformComponent.Position);
            }

            Gizmos.color = resetColor;
        }

        private void DebugDrawTouch()
        {
        }
#endif
    }
}
