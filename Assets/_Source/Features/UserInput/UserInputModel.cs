using System;
using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public class UserInputModel : AbstractDisposable, IReadOnlyUserInputModel
    {
        private const float DeadZone = 0.001f;

        private readonly ReactiveProperty<float> _horizontalAxis;
        public IReadOnlyReactiveProperty<float> HorizontalAxis => _horizontalAxis;

        private readonly ReactiveProperty<float> _verticalAxis;
        public IReadOnlyReactiveProperty<float> VerticalAxis => _verticalAxis;

        private readonly ReactiveProperty<Vector2> _inputVector;
        public IReadOnlyReactiveProperty<Vector2> InputVector => _inputVector;

        public bool HasInput => Mathf.Abs(_horizontalAxis.Value) > DeadZone || 
                                Mathf.Abs(_verticalAxis.Value) > DeadZone;

        public UserInputModel()
        {
            _verticalAxis = new ReactiveProperty<float>().AddTo(Disposer);
            _horizontalAxis = new ReactiveProperty<float>().AddTo(Disposer);
            _inputVector = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
        }

        public void SetInputAxis(float horizontal, float vertical)
        {
            _horizontalAxis.Value = horizontal;
            _verticalAxis.Value = vertical;

            _inputVector.Value = new Vector2(horizontal, vertical);
        }
    }
}
