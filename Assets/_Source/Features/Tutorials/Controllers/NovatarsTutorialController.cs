using _Source.Features.ActorEntities.Novatar;
using _Source.Features.FeatureToggles;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials.Controllers
{
    public class NovatarsTutorialController : AbstractDisposableFeature, IInitializable
    {
        private TutorialId Id => TutorialId.Novatars;
        private TutorialId PreviousId => TutorialId.Life;

        private readonly IFeatureToggleCollectionModel _featureToggleCollectionModel;
        private readonly IDebugNovatarSpawner _spawningOrchestrator; // ToDo V2 Should expose force spawning on non-debug interface
        private readonly ITutorialModel _model;
        private readonly ITutorialModel _previousModel;

        private const double DelaySeconds = 1d;
        private const double TimeoutSeconds = 10d;

        public NovatarsTutorialController(
            ITutorialsCollectionModel collectionModel,
            IFeatureToggleCollectionModel featureToggleCollectionModel,
            IDebugNovatarSpawner spawningOrchestrator)
        {
            _featureToggleCollectionModel = featureToggleCollectionModel;
            _spawningOrchestrator = spawningOrchestrator;
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
            _spawningOrchestrator.Spawn();

            Observable.Timer(TimeSpan.FromSeconds(TimeoutSeconds))
                .Subscribe(_ => Complete())
                .AddTo(Disposer);
        }

        private void Complete()
        {
            _model.Complete();
            _featureToggleCollectionModel[FeatureId.NovatarSpawning].SetIsEnabled(true);
        }
    }
}
