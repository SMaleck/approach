using _Source.Features.GameRound;
using _Source.Features.SceneManagement;
using _Source.Features.ViewManagement;
using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.PauseMenu
{
    public class PauseView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, PauseView> { }

        [Header("Screen Elements")]
        [SerializeField] private TextMeshProUGUI _screenTitle;

        [Header("Menu Buttons")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _restartButtonText;

        [SerializeField] private Button _goToSettingsButton;
        [SerializeField] private TextMeshProUGUI _goToSettingsButtonText;

        [SerializeField] private Button _goToTitleButton;
        [SerializeField] private TextMeshProUGUI _goToTitleButtonText;

        private GameRoundController _gameRoundController;
        private ISceneManagementController _sceneManagementController;
        private IViewManagementController _viewManagementController;

        [Inject]
        private void Inject(
            GameRoundController gameRoundController,
            ISceneManagementController sceneManagementController,
            IViewManagementController viewManagementController)
        {
            _gameRoundController = gameRoundController;
            _sceneManagementController = sceneManagementController;
            _viewManagementController = viewManagementController;
        }

        public void Initialize()
        {
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => OnRestartClicked())
                .AddTo(Disposer);

            _goToSettingsButton.OnClickAsObservable()
                .Subscribe(_ => OnGoToSettingsClicked())
                .AddTo(Disposer);

            _goToTitleButton.OnClickAsObservable()
                .Subscribe(_ => OnGoToTitleClicked())
                .AddTo(Disposer);

            OnOpened
                .Subscribe(_ => _gameRoundController.PauseRound(true))
                .AddTo(Disposer);

            OnClosed
                .Subscribe(_ => _gameRoundController.PauseRound(false))
                .AddTo(Disposer);

            Localize();            
        }

        private void OnRestartClicked()
        {
            _sceneManagementController.ToGame();
        }

        private void OnGoToSettingsClicked()
        {
            _viewManagementController.OpenView(ViewType.Settings);
        }

        private void OnGoToTitleClicked()
        {
            _sceneManagementController.ToTitle();
        }

        public void Localize()
        {
            _screenTitle.text = TextService.Pause();
            _restartButtonText.text = TextService.Restart();
            _goToSettingsButtonText.text = TextService.Settings();
            _goToTitleButtonText.text = TextService.ExitToMenu();
        }
    }
}
