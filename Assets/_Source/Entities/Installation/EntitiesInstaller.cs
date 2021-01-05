using _Source.Entities.Actors;
using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Util;
using Zenject;

namespace _Source.Entities.Installation
{
    public class EntitiesInstaller : Installer<EntitiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();

            Container.BindInterfacesAndSelfTo<AvatarStateModel>().AsSingleNonLazy();
            Container.BindFactory<AvatarEntity, AvatarFacade, AvatarFacade.Factory>();
            Container.BindFactory<NovatarEntity, IActorStateModel, NovatarFacade, NovatarFacade.Factory>();
        }
    }
}
