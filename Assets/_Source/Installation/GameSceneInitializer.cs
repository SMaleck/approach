using _Source.Entities;
using _Source.Entities.Avatar;
using _Source.Features.AvatarState;
using _Source.Features.GameRound;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class GameSceneInitializer : IInitializable
    {
        [Inject] private DiContainer _container;

        [Inject] private AvatarEntity.Factory _avatarFactory;
        [Inject] private AvatarConfig _avatarConfig;

        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private SurvivalStatsView.Factory _survivalStatsViewFactory;
        [Inject] private RoundEndedView.Factory _roundEndedViewFactory;

        public void Initialize()
        {
            var avatar = _avatarFactory.Create(_avatarConfig.AvatarPrefab);
            avatar.Initialize();

            _container.BindInstance(avatar);

            _survivalStatsViewFactory
                .Create(_viewPrefabsConfig.SurvivalStatsViewPrefab)
                .Initialize();

            _roundEndedViewFactory
                .Create(_viewPrefabsConfig.RoundEndedViewPrefab)
                .Initialize();
        }
    }
}
