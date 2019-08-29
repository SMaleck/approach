using _Source.App;
using UnityEngine;

namespace _Source.Features.UserInput.Data
{
    [CreateAssetMenu(fileName = nameof(UserInputConfig), menuName = Constants.ConfigRootPath + "/" + nameof(UserInputConfig))]
    public class UserInputConfig : ScriptableObject
    {
        [SerializeField] private float _deadZone;
        public float DeadZone => _deadZone;

        [SerializeField] private float _virtualJoystickMaxMagnitude;
        public float VirtualJoystickMaxMagnitude => _virtualJoystickMaxMagnitude;
    }
}
