using _Source.Entities.Avatar;
using _Source.Features.AvatarState;
using _Source.Features.GameRound;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Installation.Data;
using Zenject;
using UniRx;

namespace _Source.Installation
{
    public class GameSceneInitializer : AbstractSceneInitializer, IInitializable
    {
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private IViewManagementRegistrar _viewManagementRegistrar;

        [Inject] private AvatarEntity.Factory _avatarFactory;
        [Inject] private AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private AvatarConfig _avatarConfig;

        [Inject] private SurvivalStatsView.Factory _survivalStatsViewFactory;
        [Inject] private RoundEndedView.Factory _roundEndedViewFactory;
        [Inject] private VirtualJoystickView.Factory _virtualJoystickViewFactory;

        public void Initialize()
        {
            var avatar = _avatarFactory.Create(_avatarConfig.AvatarPrefab);
            
            var avatarFacade = _avatarFacadeFactory.Create(avatar);
            avatarFacade.AddTo(SceneDisposer);

            SceneContainer.BindInterfacesAndSelfTo<AvatarFacade>()
                .FromInstance(avatarFacade);

            _survivalStatsViewFactory
                .Create(_viewPrefabsConfig.SurvivalStatsViewPrefab)
                .Initialize();

            _roundEndedViewFactory
                .Create(_viewPrefabsConfig.RoundEndedViewPrefab)
                .Initialize();

            _virtualJoystickViewFactory
                .Create(_viewPrefabsConfig.VirtualJoystickViewPrefab)
                .Initialize();
        }
    }
}
