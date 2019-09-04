using _Source.Features.ViewManagement;
using _Source.Util;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.PartialViews
{
    public class CloseLastOpenView : AbstractDisposableMonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private IViewManagementController _viewManagementController;

        [Inject]
        private void Inject(IViewManagementController viewManagementController)
        {
            _viewManagementController = viewManagementController;

            _closeButton.OnClickAsObservable()
                .Subscribe(_ => _viewManagementController.CloseLastOpenView())
                .AddTo(Disposer);
        }

        // ToDo Find a better way of getting this into IInitializable
        private void Awake()
        {
            _closeButton.OnClickAsObservable()
                .Subscribe(_ => _viewManagementController.CloseLastOpenView())
                .AddTo(Disposer);
        }
    }
}
