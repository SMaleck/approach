using _Source.Entities.Novatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataSystems
{
    public class EntityStateNotificationSystem : AbstractDataSystem
    {
        public class Factory : PlaceholderFactory<IActorStateModel, EntityStateNotificationSystem> { }

        private readonly RelationshipDataComponent _relationshipDataComponent;
        private readonly IEntityStateSensitiveDataComponent[] _components;

        public EntityStateNotificationSystem(IActorStateModel actor)
            : base(actor)
        {
            _relationshipDataComponent = actor.Get<RelationshipDataComponent>();

            _components = actor.GetAll<IEntityStateSensitiveDataComponent>();

            _relationshipDataComponent.Relationship
                .Subscribe(OnRelationShipChanged)
                .AddTo(Disposer);
        }

        private void OnRelationShipChanged(EntityState state)
        {
            _components.ForEach(e => e.OnRelationshipChanged(state));
        }
    }
}
