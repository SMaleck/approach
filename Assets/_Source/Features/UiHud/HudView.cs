using _Source.Features.ViewManagement;
using _Source.Util;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.UiHud
{
    public class HudView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HudView> { }

        [SerializeField] private Button _pauseButton;

        private IViewManagementController _viewManagementController;

        [Inject]
        private void Inject(
            IViewManagementController viewManagementController)
        {
            _viewManagementController = viewManagementController;
        }

        public void Initialize()
        {
            _pauseButton.OnClickAsObservable()
                .Subscribe(_ => OnPauseButtonClicked())
                .AddTo(Disposer);
        }

        private void OnPauseButtonClicked()
        {
            _viewManagementController.OpenView(ViewType.Pause);
        }
    }
}
