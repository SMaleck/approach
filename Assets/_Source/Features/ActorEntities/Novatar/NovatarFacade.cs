using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Creation;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarFacade : EntityFacade, INovatarPoolItem
    {
        public new class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, BehaviourTree, NovatarFacade> { }

        private readonly BehaviourTree _behaviourTree;
        private readonly OriginDataComponent _originDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;
        private readonly List<IResettableNode> _resettableNodes;

        public bool IsFree => !HealthDataComponent.IsAlive.Value;
        public bool IsFriend => _relationshipDataComponent.Relationship.Value == EntityState.Friend;
        public Vector3 Size => Entity.Size;

        public NovatarFacade(
            IMonoEntity entity,
            IActorStateModel actor,
            BehaviourTree behaviourTree,
            IPauseStateModel pauseStateModel)
            : base(entity, actor, pauseStateModel)
        {
            _behaviourTree = behaviourTree;
            _originDataComponent = Actor.Get<OriginDataComponent>();
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();

            _resettableNodes = _behaviourTree.Nodes
                .OfType<IResettableNode>()
                .ToList();

            _originDataComponent.SpawnPosition
                .Subscribe(Entity.SetPosition)
                .AddTo(Disposer);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _originDataComponent.SetSpawnPosition(spawnPosition);
            Actor.Reset();

            _resettableNodes.ForEach(
                node => node.Reset());
        }

        protected override void OnTick()
        {
            base.OnTick();
            _behaviourTree.Tick(new TimeData(Time.deltaTime));
        }
    }
}
