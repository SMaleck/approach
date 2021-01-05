using _Source.App;
using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.ActorBehaviours.NovatarSpawning.Data;
using _Source.Features.Movement.Data;
using _Source.Features.UserInput.Data;
using _Source.Installation.Data;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    [CreateAssetMenu(fileName = nameof(ConfigInstaller), menuName = Constants.InstallersMenu + nameof(ConfigInstaller))]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] private AvatarConfig _avatarConfig;
        [SerializeField] private NovatarConfig _novatarConfig;
        [SerializeField] private ViewPrefabsConfig _viewPrefabsConfig;
        [SerializeField] private NovatarSpawnerConfig _novatarSpawnerConfig;
        [SerializeField] private BehaviourTreeConfig _behaviourTreeConfig;
        [SerializeField] private UserInputConfig _userInputConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_avatarConfig);
            Container.BindInstance<IMovementData>(_avatarConfig.MovementConfig);
            Container.BindInstance(_novatarConfig);
            Container.BindInstance(_viewPrefabsConfig);
            Container.BindInstance(_novatarSpawnerConfig);
            Container.BindInstance(_behaviourTreeConfig);
            Container.BindInstance(_userInputConfig);
        }
    }
}
