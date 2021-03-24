using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Actors.DataComponents;
using Zenject;

namespace _Source.Features.Actors.Creation
{
    public class ActorFactory : IAvatarActorFactory, INovatarActorFactory
    {
        [Inject] protected readonly ActorStateModel.Factory ActorStateModelFactory;
        [Inject] protected readonly BlackBoardDataComponent.Factory BlackBoardDataComponentFactory;
        [Inject] protected readonly EntityTypeDataComponent.Factory EntityTypeDataComponentFactory;
        [Inject] protected readonly HealthDataComponent.Factory HealthDataComponentFactory;
        [Inject] protected readonly DamageDataComponent.Factory DamageDataComponentFactory;
        [Inject] protected readonly MovementDataComponent.Factory MovementDataComponentFactory;
        [Inject] protected readonly OriginDataComponent.Factory OriginDataComponentFactory;
        [Inject] protected readonly RelationshipDataComponent.Factory RelationshipDataComponentFactory;
        [Inject] protected readonly LightDataComponent.Factory LightDataComponentFactory;
        [Inject] protected readonly TransformDataComponent.Factory TransformDataComponentFactory;
        [Inject] protected readonly SensorDataComponent.Factory SensorDataComponentFactory;

        private readonly AvatarData _avatarData;
        private readonly NovatarData _novatarData;

        public ActorFactory(
            AvatarData avatarData,
            NovatarData novatarData)
        {
            _avatarData = avatarData;
            _novatarData = novatarData;
        }

        public IActorStateModel CreateAvatar()
        {
            return ActorStateModelFactory.Create()
                .Attach(EntityTypeDataComponentFactory.Create(EntityType.Avatar))
                .Attach(HealthDataComponentFactory.Create(_avatarData))
                .Attach(MovementDataComponentFactory.Create(_avatarData))
                .Attach(TransformDataComponentFactory.Create())
                .Attach(SensorDataComponentFactory.Create());
        }

        public IActorStateModel CreateNovatar()
        {
            return ActorStateModelFactory.Create()
                .Attach(BlackBoardDataComponentFactory.Create())
                .Attach(EntityTypeDataComponentFactory.Create(EntityType.NPC))
                .Attach(HealthDataComponentFactory.Create(_novatarData))
                .Attach(DamageDataComponentFactory.Create(_novatarData))
                .Attach(MovementDataComponentFactory.Create(_novatarData))
                .Attach(OriginDataComponentFactory.Create())
                .Attach(LightDataComponentFactory.Create())
                .Attach(RelationshipDataComponentFactory.Create())
                .Attach(TransformDataComponentFactory.Create())
                .Attach(SensorDataComponentFactory.Create());
        }
    }
}
