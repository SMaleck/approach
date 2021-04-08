using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Features.FeatureToggles;
using _Source.Util;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Tutorials.Controllers
{
    public class ControlsTutorialController : AbstractDisposableFeature, IInitializable
    {
        private TutorialId Id => TutorialId.Controls;

        private readonly IAvatarLocator _avatarLocator;
        private readonly IFeatureToggleCollectionModel _featureToggleCollectionModel;
        private readonly ITutorialModel _model;

        private const double MinElapsedSeconds = 5;
        private const double DelaySeconds = 2;
        private bool _hasMoved;
        private double _elapsedSeconds;

        public ControlsTutorialController(
            ITutorialsCollectionModel collectionModel,
            IAvatarLocator avatarLocator,
            IFeatureToggleCollectionModel featureToggleCollectionModel)
        {
            _avatarLocator = avatarLocator;
            _featureToggleCollectionModel = featureToggleCollectionModel;

            _model = collectionModel[Id];
        }

        public void Initialize()
        {
            if (_model.IsCompleted)
            {
                return;
            }
            
            _featureToggleCollectionModel[FeatureId.GameRoundTime].SetIsEnabled(false);
            _featureToggleCollectionModel[FeatureId.NovatarSpawning].SetIsEnabled(false);

            Observable.Timer(TimeSpan.FromSeconds(DelaySeconds))
                .Subscribe(_ => Start())
                .AddTo(Disposer);
        }

        private void Start()
        {
            _model.Start();

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _elapsedSeconds += Time.deltaTime;
                    _hasMoved = _hasMoved || GetHasMoved();

                    TryComplete();
                })
                .AddTo(Disposer);
        }

        private bool GetHasMoved()
        {
            var movementDataComponent = _avatarLocator.AvatarActor?.Get<MovementDataComponent>();
            return movementDataComponent?.HasMoveIntention ?? false; 
        }

        private void TryComplete()
        {
            if (_elapsedSeconds > MinElapsedSeconds &&
                _hasMoved)
            {
                _model.Complete();
            }
        }
    }
}
