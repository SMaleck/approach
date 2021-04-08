using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.GameRound.Duration
{
    public class GameRoundDurationView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, GameRoundDurationView> { }

        [SerializeField] private Transform _lifeBarParent;

        [Inject] private readonly IGameRoundDurationModel _gameRoundDurationModel;

        public void Initialize()
        {
            _gameRoundDurationModel.Progress
                .Subscribe(OnProgressChanged)
                .AddTo(Disposer);
        }

        private void OnProgressChanged(double progress)
        {
            _lifeBarParent.localScale = new Vector3((float)progress, 1, 1);
        }
    }
}
