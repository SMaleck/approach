using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Creation;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.ActorSensors;
using _Source.Features.Movement;
using Zenject;

namespace _Source.Features.ActorBehaviours.Installation
{
    public class ActorBehavioursInstaller : Installer<ActorBehavioursInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, ISensorySystem, MovementController, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
            Container.BindFactory<IActorStateModel, ISensorySystem, MovementController, NodeGenerator, NodeGenerator.Factory>().AsSingle();
            Container.BindInterfacesAndSelfTo<NovatarBehaviourTreeFactory>().AsSingle();

            // ------------------------------ NODE FACTORIES
            Container.BindFactory<IActorStateModel, FollowAvatarNode, FollowAvatarNode.Factory>().AsSingle();
            Container.BindFactory<double, IdleTimeoutNode, IdleTimeoutNode.Factory>().AsSingle();
            Container.BindFactory<double, double, IdleTimeoutRandomNode, IdleTimeoutRandomNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, ISensorySystem, FirstTouchNode, FirstTouchNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, EntityState, SwitchEntityStateNode, SwitchEntityStateNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, DeactivateSelfNode, DeactivateSelfNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, LeaveScreenNode, LeaveScreenNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, DamageActorNode, DamageActorNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, LightSwitchNode, LightSwitchNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, EnterScreenNode, EnterScreenNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, MovementNode, MovementNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, FindDamageReceiversNode, FindDamageReceiversNode.Factory>().AsSingle();
        }
    }
}