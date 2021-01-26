using _Source.Entities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;

namespace _Source.Features.ActorBehaviours
{
    public class BehaviourTreeBlackBoard : AbstractDisposable, IResettableDataComponent
    {
        public readonly BlackboardItem<IDamageReceiver> DamageReceiver;

        public BehaviourTreeBlackBoard()
        {
            DamageReceiver = new BlackboardItem<IDamageReceiver>();
        }

        public void Reset()
        {
            DamageReceiver.Consume();
        }
    }
}
