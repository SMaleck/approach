using _Source.Entities.Avatar;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class BlackBoardDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<BlackBoardDataComponent> { }

        public readonly BlackboardItem<IDamageReceiver> DamageReceiver;
        public readonly BlackboardItem<Vector3> MovementTarget;

        public BlackBoardDataComponent()
        {
            DamageReceiver = new BlackboardItem<IDamageReceiver>();
            MovementTarget = new BlackboardItem<Vector3>();
        }

        public void Reset()
        {
            DamageReceiver.Consume();
        }
    }
}
