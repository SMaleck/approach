using _Source.Entities.Novatar;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public abstract class AbstractBehaviour : IBehaviourTreeBuilder
    {
        protected readonly NovatarEntity NovatarEntity;
        protected readonly NovatarStateModel NovatarStateModel;

        protected AbstractBehaviour(
            NovatarEntity novatarEntity,
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
