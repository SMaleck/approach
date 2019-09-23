using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FollowAvatarNode : IBehaviourTreeNode
    {
        public FollowAvatarNode()
        {

        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            throw new NotImplementedException();
        }

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
