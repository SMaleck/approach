using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Config
{
    [CreateAssetMenu(fileName = nameof(LightIntensityConfig), menuName = Constants.ConfigMenu + nameof(LightIntensityConfig))]
    public class LightIntensityConfig : ScriptableObject
    {
        [SerializeField] private float _defaultIntensity;
        public float DefaultIntensity => _defaultIntensity;

        [SerializeField] private float _flashIntensity;
        public float FlashIntensity => _flashIntensity;
    }
}
