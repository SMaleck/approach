using _Source.Services.Texts;
using _Source.Util;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Tutorials.Views
{
    public class TutorialView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, TutorialView> { }

        [SerializeField] private TutorialId _tutorialId;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [Inject] private readonly ITutorialsCollectionModel _collectionModel;

        public void Initialize()
        {
            var model = _collectionModel[_tutorialId];

            _descriptionText.text = TextService.TutorialDescription(_tutorialId);

            model.State
                .Subscribe(OnTutorialStateChanged)
                .AddTo(Disposer);
        }

        private void OnTutorialStateChanged(TutorialState state)
        {
            switch (state)
            {
                case TutorialState.Running:
                    Open();
                    break;

                case TutorialState.None:
                case TutorialState.Completed:
                    Close();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
