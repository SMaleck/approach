using _Source.Features.GameRound;
using _Source.Features.SceneManagement;
using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.UiScreens
{
    public class RoundEndedView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, RoundEndedView> { }

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _resultText;

        [Header("Restart Button")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _restartButtonText;

        [Header("GoToMenu Button")]
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _goToMenuButtonText;

        private GameRoundStateController _gameRoundStateController;
        private IGameRoundStateModel _gameRoundStateModel;
        private ISceneManagementController _sceneManagementController;

        [Inject]
        private void Inject(
            GameRoundStateController gameRoundStateController,
            IGameRoundStateModel gameRoundStateModel,
            ISceneManagementController sceneManagementController)
        {
            _gameRoundStateController = gameRoundStateController;
            _gameRoundStateModel = gameRoundStateModel;
            _sceneManagementController = sceneManagementController;
        }

        public void Initialize()
        {
            _gameRoundStateModel.OnRoundEnded
                .Subscribe(_ => Open())
                .AddTo(Disposer);

            OnOpened
                .Subscribe(_ => _gameRoundStateController.PauseRound(true))
                .AddTo(Disposer);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _sceneManagementController.ToGame())
                .AddTo(Disposer);

            _goToMenuButton.OnClickAsObservable()
                .Subscribe(_ => _sceneManagementController.ToTitle())
                .AddTo(Disposer);

            Localize();
        }

        public void Localize()
        {
            _titleText.text = TextService.End();
            _restartButtonText.text = TextService.Restart();
            _goToMenuButtonText.text = TextService.ExitToMenu();
        }
    }
}
