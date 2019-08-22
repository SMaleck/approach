using _Source.Entities.Novatar;
using _Source.Util;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public abstract class AbstractBehaviour : AbstractDisposable, IBehaviourTreeBuilder
    {
        protected readonly INovatar NovatarEntity;
        protected readonly NovatarStateModel NovatarStateModel;

        protected AbstractBehaviour(
            INovatar novatarEntity,
            NovatarStateModel novatarStateModel)
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
