using _Source.Features.GameRound;
using _Source.Features.ViewManagement;
using _Source.Util;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.Hud
{
    public class HudView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HudView> { }

        [SerializeField] private Button _pauseButton;

        private IViewManagementController _viewManagementController;
        private GameRoundController _gameRoundController;

        [Inject]
        private void Inject(
            IViewManagementController viewManagementController,
            GameRoundController gameRoundController)
        {
            _viewManagementController = viewManagementController;
            _gameRoundController = gameRoundController;
        }

        public void Initialize()
        {
            _pauseButton.OnClickAsObservable()
                .Subscribe(_ => OnPauseButtonClicked())
                .AddTo(Disposer);
        }

        private void OnPauseButtonClicked()
        {
            _gameRoundController.PauseRound(true);
            _viewManagementController.OpenView(ViewType.Pause);
        }
    }
}
