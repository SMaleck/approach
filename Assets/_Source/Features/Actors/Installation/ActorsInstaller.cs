﻿using _Source.Features.Actors.Data;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors.Data;
using Zenject;

namespace _Source.Features.Actors.Installation
{
    public class ActorsInstaller : Installer<ActorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<ActorStateModel, ActorStateModel.Factory>().AsSingle();

            Container.BindFactory<BlackBoardDataComponent, BlackBoardDataComponent.Factory>().AsSingle();
            Container.BindFactory<EntityType, EntityTypeDataComponent, EntityTypeDataComponent.Factory>().AsSingle();
            Container.BindFactory<IHealthData, HealthDataComponent, HealthDataComponent.Factory>().AsSingle();
            Container.BindFactory<IDamageData, DamageDataComponent, DamageDataComponent.Factory>().AsSingle();
            Container.BindFactory<IMovementData, MovementDataComponent, MovementDataComponent.Factory>().AsSingle();
            Container.BindFactory<RelationshipDataComponent, RelationshipDataComponent.Factory>().AsSingle();
            Container.BindFactory<OriginDataComponent, OriginDataComponent.Factory>().AsSingle();
            Container.BindFactory<SurvivalDataComponent, SurvivalDataComponent.Factory>().AsSingle();
            Container.BindFactory<LightDataComponent, LightDataComponent.Factory>().AsSingle();
            Container.BindFactory<TransformDataComponent, TransformDataComponent.Factory>().AsSingle();
            Container.BindFactory<SensorDataComponent, SensorDataComponent.Factory>().AsSingle();
        }
    }
}
