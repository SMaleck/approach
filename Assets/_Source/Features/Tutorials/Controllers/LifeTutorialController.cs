using _Source.Features.FeatureToggles;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials.Controllers
{
    public class LifeTutorialController : AbstractDisposableFeature, IInitializable
    {
        private TutorialId Id => TutorialId.Life;
        private TutorialId PreviousId => TutorialId.Controls;

        private readonly IFeatureToggleCollectionModel _featureToggleCollectionModel;
        private readonly ITutorialModel _model;
        private readonly ITutorialModel _previousModel;

        private const double DelaySeconds = 1d;
        private const double TimeoutSeconds = 10d;

        public LifeTutorialController(
            ITutorialsCollectionModel collectionModel,
            IFeatureToggleCollectionModel featureToggleCollectionModel)
        {
            _featureToggleCollectionModel = featureToggleCollectionModel;
            _model = collectionModel[Id];
            _previousModel = collectionModel[PreviousId];
        }

        public void Initialize()
        {
            if (_model.IsCompleted)
            {
                return;
            }

            _previousModel.State
                .Where(_ => _previousModel.IsCompleted)
                .Delay(TimeSpan.FromSeconds(DelaySeconds))
                .Subscribe(_ => Start())
                .AddTo(Disposer);
        }

        private void Start()
        {
            _model.Start();
            _featureToggleCollectionModel[FeatureId.GameRoundTime].SetIsEnabled(true);

            Observable.Timer(TimeSpan.FromSeconds(TimeoutSeconds))
                .Subscribe(_ => _model.Complete())
                .AddTo(Disposer);
        }
    }
}
