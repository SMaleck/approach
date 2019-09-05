using _Source.Entities.Novatar;
using _Source.Util;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public abstract class AbstractBehaviour : AbstractDisposable, IBehaviourTreeBuilder
    {
        protected readonly INovatar NovatarEntity;
        protected readonly INovatarStateModel NovatarStateModel;

        protected AbstractBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel)
        {
            NovatarEntity = novatarEntity;
            NovatarStateModel = novatarStateModel;
        }

        public abstract IBehaviourTreeNode Build();

        protected bool IsInFollowRange()
        {
            var isInRange = NovatarStateModel.CurrentDistanceToAvatar.Value <= NovatarEntity.SqrRange;
            return isInRange && !IsInTouchRange();
        }

        protected bool IsInTouchRange()
        {
            return NovatarStateModel.CurrentDistanceToAvatar.Value <= NovatarEntity.SqrTargetReachedThreshold;
        }
    }
}
