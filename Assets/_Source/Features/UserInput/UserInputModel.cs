using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public class UserInputModel : AbstractDisposable, IReadOnlyUserInputModel
    {
        private const float DeadZone = 0.001f;

        private readonly ReactiveProperty<Vector2> _inputVector;
        public IReadOnlyReactiveProperty<Vector2> InputVector => _inputVector;

        public bool HasInput => InputVector.Value.magnitude > DeadZone;

        public UserInputModel()
        {
            _inputVector = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
        }

        public void SetInputVector(float horizontal, float vertical)
        {
            SetInputVector(new Vector2(horizontal, vertical));
        }

        public void SetInputVector(Vector2 inputVector)
        {
            App.Logger.Log(inputVector.magnitude);
            _inputVector.Value = Vector2.ClampMagnitude(inputVector, 1);
        }
    }
}
