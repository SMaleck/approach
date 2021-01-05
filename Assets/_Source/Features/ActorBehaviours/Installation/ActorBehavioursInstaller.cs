using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.ActorBehaviours.Sensors;
using _Source.Features.ActorBehaviours.Sensors.Data;
using _Source.Features.Actors;
using _Source.Features.Movement;
using _Source.Features.NovatarSpawning;
using _Source.Util;
using Zenject;

namespace _Source.Features.ActorBehaviours.Installation
{
    public class ActorBehavioursInstaller : Installer<ActorBehavioursInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<INovatar, IActorStateModel, SensorySystem, SensorySystem.Factory>();
            Container.BindFactory<INovatar, RangeSensorConfig, RangeSensor, RangeSensor.Factory>();

            Container.BindFactory<NodeGenerator, NodeGenerator.Factory>();
            Container.BindFactory<INovatar, ISensorySystem, MovementController, FollowAvatarNode, FollowAvatarNode.Factory>();
            Container.BindFactory<double, IdleTimeoutNode, IdleTimeoutNode.Factory>();
            Container.BindFactory<double, double, IdleTimeoutRandomNode, IdleTimeoutRandomNode.Factory>();
            Container.BindFactory<INovatar, IActorStateModel, ISensorySystem, FirstTouchNode, FirstTouchNode.Factory>();
            Container.BindFactory<INovatar, EntityState, SwitchEntityStateNode, SwitchEntityStateNode.Factory>();
            Container.BindFactory<INovatar, DeactivateSelfNode, DeactivateSelfNode.Factory>();
            Container.BindFactory<INovatar, IActorStateModel, MovementController, LeaveScreenNode, LeaveScreenNode.Factory>();
            Container.BindFactory<ISensorySystem, DamageAvatarNode, DamageAvatarNode.Factory>();
            Container.BindFactory<INovatar, LightSwitchNode, LightSwitchNode.Factory>();
            Container.BindFactory<INovatar, IActorStateModel, MovementController, EnterScreenNode, EnterScreenNode.Factory>();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SpawningOrchestrator>().AsSingleNonLazy();
        }
    }
}
