using _Source.Entities;
using _Source.Features.ActorEntities;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class TransformDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<TransformDataComponent> { }

        private IMonoEntity _monoEntity;

        public Vector3 Position => _monoEntity.Position;
        public Quaternion Rotation => _monoEntity.Rotation;
        public Vector3 Size => _monoEntity.Size;

        public TransformDataComponent()
        {
        }

        public void SetMonoEntity(IMonoEntity monoEntity)
        {
            _monoEntity = monoEntity;
        }
    }
}
