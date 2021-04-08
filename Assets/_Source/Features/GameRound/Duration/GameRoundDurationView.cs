using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.GameRound.Duration
{
    public class GameRoundDurationView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, GameRoundDurationView> { }

        [SerializeField] private GameObject _lifeBarParent;

        [Inject] private readonly IGameRoundDurationModel _gameRoundDurationModel;

        public void Initialize()
        {
            _gameRoundDurationModel.IsEnabled
                .Subscribe(_lifeBarParent.SetActive)
                .AddTo(Disposer);

            _gameRoundDurationModel.Progress
                .Subscribe(OnProgressChanged)
                .AddTo(Disposer);
        }

        private void OnProgressChanged(double progress)
        {
            _lifeBarParent.transform
                .DOScaleX((float)progress, 0.5f);
        }
    }
}
