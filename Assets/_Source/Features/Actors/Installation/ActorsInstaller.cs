﻿using _Source.Features.Actors.Creation;
using _Source.Features.Actors.Data;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Actors.DataSystems;
using _Source.Features.Movement.Data;
using _Source.Features.Tokens;
using Zenject;

namespace _Source.Features.Actors.Installation
{
    public class ActorsInstaller : Installer<ActorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<IdGenerator>()
                .AsSingle()
                .WithArguments("E");

            Container.BindInterfacesTo<ActorFactory>().AsSingle();
            Container.BindFactory<ActorStateModel, ActorStateModel.Factory>().AsSingle();

            // ----------------------------- DATA COMPONENTS
            Container.BindFactory<BlackBoardDataComponent, BlackBoardDataComponent.Factory>().AsSingle();
            Container.BindFactory<EntityType, EntityTypeDataComponent, EntityTypeDataComponent.Factory>().AsSingle();
            Container.BindFactory<IHealthData, HealthDataComponent, HealthDataComponent.Factory>().AsSingle();
            Container.BindFactory<IDamageData, DamageDataComponent, DamageDataComponent.Factory>().AsSingle();
            Container.BindFactory<IMovementData, MovementDataComponent, MovementDataComponent.Factory>().AsSingle();
            Container.BindFactory<RelationshipDataComponent, RelationshipDataComponent.Factory>().AsSingle();
            Container.BindFactory<OriginDataComponent, OriginDataComponent.Factory>().AsSingle();
            Container.BindFactory<LightDataComponent, LightDataComponent.Factory>().AsSingle();
            Container.BindFactory<TransformDataComponent, TransformDataComponent.Factory>().AsSingle();
            Container.BindFactory<SensorDataComponent, SensorDataComponent.Factory>().AsSingle();
            Container.BindFactory<TimeoutDataComponent, TimeoutDataComponent.Factory>().AsSingle();
            Container.BindFactory<IWanderData, WanderDataComponent, WanderDataComponent.Factory>().AsSingle();

            // ----------------------------- DATA SYSTEMS
            Container.BindFactory<IActorStateModel, EntityStateNotificationSystem, EntityStateNotificationSystem.Factory>().AsSingle();
        }
    }
}
