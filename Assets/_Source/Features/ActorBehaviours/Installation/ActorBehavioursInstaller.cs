using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Creation;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using Zenject;

namespace _Source.Features.ActorBehaviours.Installation
{
    public class ActorBehavioursInstaller : Installer<ActorBehavioursInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, MovementController, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
            Container.BindInterfacesAndSelfTo<NovatarBehaviourTreeFactory>().AsSingle();

            // ------------------------------ NODE FACTORIES
            Container.BindFactory<IActorStateModel, FollowAvatarNode, FollowAvatarNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, double, TimeoutDataComponent.Storage, IdleTimeoutNode, IdleTimeoutNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, FirstTouchNode, FirstTouchNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, EntityState, SwitchEntityStateNode, SwitchEntityStateNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, DeactivateSelfNode, DeactivateSelfNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, LeaveScreenNode, LeaveScreenNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, DamageActorNode, DamageActorNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, LightSwitchNode, LightSwitchNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, EnterScreenNode, EnterScreenNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, MovementController, MovementNode, MovementNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, FindDamageReceiversNode, FindDamageReceiversNode.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, NearDeathNode, NearDeathNode.Factory>().AsSingle();
            Container.BindFactory<
                    IActorStateModel,
                    double,
                    TimeoutDataComponent.Storage,
                    double,
                    IdleTimeoutRandomNode,
                    IdleTimeoutRandomNode.Factory>()
                .AsSingle();
        }
    }
}