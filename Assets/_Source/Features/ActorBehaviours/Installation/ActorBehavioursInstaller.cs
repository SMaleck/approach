using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.ActorBehaviours.Sensors;
using _Source.Features.ActorBehaviours.Sensors.Data;
using _Source.Features.Actors;
using _Source.Features.Movement;
using Zenject;

namespace _Source.Features.ActorBehaviours.Installation
{
    public class ActorBehavioursInstaller : Installer<ActorBehavioursInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<INovatar, IActorStateModel, SensorySystem, SensorySystem.Factory>().AsSingle();
            Container.BindFactory<INovatar, RangeSensorConfig, RangeSensor, RangeSensor.Factory>().AsSingle();

            Container.BindFactory<NodeGenerator, NodeGenerator.Factory>().AsSingle();
            Container.BindFactory<INovatar, ISensorySystem, MovementController, FollowAvatarNode, FollowAvatarNode.Factory>().AsSingle();
            Container.BindFactory<double, IdleTimeoutNode, IdleTimeoutNode.Factory>().AsSingle();
            Container.BindFactory<double, double, IdleTimeoutRandomNode, IdleTimeoutRandomNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, IActorStateModel, ISensorySystem, FirstTouchNode, FirstTouchNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, EntityState, SwitchEntityStateNode, SwitchEntityStateNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, DeactivateSelfNode, DeactivateSelfNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, IActorStateModel, MovementController, LeaveScreenNode, LeaveScreenNode.Factory>().AsSingle();
            Container.BindFactory<ISensorySystem, IActorStateModel, DamageAvatarNode, DamageAvatarNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, LightSwitchNode, LightSwitchNode.Factory>().AsSingle();
            Container.BindFactory<INovatar, IActorStateModel, MovementController, EnterScreenNode, EnterScreenNode.Factory>().AsSingle();

            Container.BindFactory<INovatar, IActorStateModel, ISensorySystem, MovementController, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
        }
    }
}
