using _Source.Entities;
using _Source.Features.GameRound;
using _Source.Features.SurvivalStats;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class GameInitializer : IInitializable
    {
        [Inject] private DiContainer _container;

        [Inject] private Avatar.Factory _avatarFactory;
        [Inject] private AvatarConfig _avatarConfig;

        [Inject] private Novatar.Factory _novatarFactory;
        [Inject] private NovatarConfig _novatarConfig;

        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private SurvivalStatsView.Factory _survivalStatsViewFactory;
        [Inject] private RoundEndedView.Factory _roundEndedViewFactory;

        public void Initialize()
        {
            var avatar = _avatarFactory.Create(_avatarConfig.AvatarPrefab);
            avatar.Initialize();

            _container.BindInstance(avatar);

            _novatarFactory
                .Create(_novatarConfig.NovatarPrefab)
                .Initialize();
            
            _survivalStatsViewFactory
                .Create(_viewPrefabsConfig.SurvivalStatsViewPrefab)
                .Initialize();

            _roundEndedViewFactory
                .Create(_viewPrefabsConfig.RoundEndedViewPrefab)
                .Initialize();
        }
    }
}
