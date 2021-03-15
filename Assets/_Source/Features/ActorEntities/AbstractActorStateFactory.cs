using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using Zenject;

namespace _Source.Features.ActorEntities
{
    public abstract class AbstractActorStateFactory
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
        [Inject] protected readonly SurvivalDataComponent.Factory SurvivalDataComponentFactory;
        [Inject] protected readonly TransformDataComponent.Factory TransformDataComponentFactory;
        [Inject] protected readonly SensorDataComponent.Factory SensorDataComponentFactory;
    }
}
