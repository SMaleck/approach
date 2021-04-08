using _Source.App;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.ActorEntities.Config;
using _Source.Features.GameRound.Config;
using _Source.Features.Tutorials.Config;
using _Source.Features.UserInput.Data;
using _Source.Installation.Data;
using Packages.SavegameSystem.Config;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    [CreateAssetMenu(fileName = nameof(ConfigInstaller), menuName = Constants.InstallersMenu + nameof(ConfigInstaller))]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] private ActorEntitiesConfig _actorEntitiesConfig;
        [SerializeField] private ViewPrefabsConfig _viewPrefabsConfig;
        [SerializeField] private NovatarSpawnerConfig _novatarSpawnerConfig;
        [SerializeField] private BehaviourTreeConfig _behaviourTreeConfig;
        [SerializeField] private UserInputConfig _userInputConfig;
        [SerializeField] private SavegamesConfig _savegamesConfig;
        [SerializeField] private TutorialsPrefabConfig _tutorialsPrefabConfig;
        [SerializeField] private GameRoundPrefabConfig _gameRoundPrefabConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_actorEntitiesConfig);
            Container.BindInstance(_viewPrefabsConfig);
            Container.BindInstance(_novatarSpawnerConfig);
            Container.BindInstance(_behaviourTreeConfig);
            Container.BindInstance(_userInputConfig);
            Container.BindInterfacesTo<SavegamesConfig>().FromInstance(_savegamesConfig);
            Container.BindInstance(_tutorialsPrefabConfig);
            Container.BindInstance(_gameRoundPrefabConfig);
        }
    }
}
