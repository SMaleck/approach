﻿using _Source.Features.Actors.Data;
using _Source.Features.Actors.DataComponents;
using Zenject;

namespace _Source.Features.Actors.Installation
{
    public class ActorsInstaller : Installer<ActorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<ActorStateModel, ActorStateModel.Factory>().AsSingle().NonLazy();

            Container.BindFactory<IHealthData, HealthDataComponent, HealthDataComponent.Factory>().AsSingle().NonLazy();
            Container.BindFactory<RelationshipDataComponent, RelationshipDataComponent.Factory>().AsSingle().NonLazy();
            Container.BindFactory<OriginDataComponent, OriginDataComponent.Factory>().AsSingle().NonLazy();
            Container.BindFactory<SurvivalDataComponent, SurvivalDataComponent.Factory>().AsSingle().NonLazy();
        }
    }
}
