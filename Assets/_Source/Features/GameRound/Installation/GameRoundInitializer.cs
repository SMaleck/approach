using _Source.Features.GameRound.Config;
using _Source.Features.GameRound.Duration;
using Zenject;

namespace _Source.Features.GameRound.Installation
{
    public class GameRoundInitializer : IInitializable
    {
        [Inject] private GameRoundPrefabConfig _gameRoundPrefabConfig;
        [Inject] private GameRoundDurationView.Factory _gameRoundDurationViewFactory;
        
        public void Initialize()
        {
            _gameRoundDurationViewFactory.Create(_gameRoundPrefabConfig.GameRoundDurationViewPrefab)
                .Initialize();
        }
    }
}
