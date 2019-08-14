using _Source.Features.SceneManagement;
using _Source.Features.ViewManagement;
using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace _Source.Features.TitleMenu
{
    public class TitleView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, TitleView> { }

        [SerializeField] private Button _startGameButton;
        [SerializeField] private TextMeshProUGUI _startGameButtonText;
        [SerializeField] private Button _howToPlayButton;
        [SerializeField] private TextMeshProUGUI _howToPlayButtonText;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TextMeshProUGUI _settingsButtonText;

        private IViewManagementController _viewManagementController;
        private ISceneManagementController _sceneManagementController;

        [Inject]
        private void Inject(
            IViewManagementController viewManagementController,
            ISceneManagementController sceneManagementController)
        {
            _viewManagementController = viewManagementController;
            _sceneManagementController = sceneManagementController;
        }

        public void Initialize()
        {
            _startGameButton.OnClickAsObservable()
                .Subscribe(_ => _sceneManagementController.ToGame())
                .AddTo(Disposer);

            _settingsButton.OnClickAsObservable()
                .Subscribe(_ => _viewManagementController.OpenView(ViewType.Settings))
                .AddTo(Disposer);

            _howToPlayButton.OnClickAsObservable()
                .Subscribe(_ => _viewManagementController.OpenView(ViewType.HowToPlay))
                .AddTo(Disposer);

            Localize();
        }

        public void Localize()
        {
            _startGameButtonText.text = TextService.StartGame();
            _howToPlayButtonText.text = TextService.HowToPlay();
            _settingsButtonText.text = TextService.Settings();
        }
    }
}
