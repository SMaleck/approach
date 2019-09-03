﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Features.UserInput;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Avatar
{
    public class AvatarFacade : AbstractDisposable, IAvatar, IDamageReceiver
    {
        public class Factory : PlaceholderFactory<AvatarEntity, AvatarFacade> { }

        private readonly AvatarEntity _avatarEntity;
        private readonly AvatarConfig _avatarConfig;
        private readonly AvatarStateModel _avatarStateModel;
        private readonly IReadOnlyUserInputModel _userInputModel;

        public bool IsActive => _avatarEntity.IsActive;
        public Vector3 Position => _avatarEntity.Position;
        public Quaternion Rotation => _avatarEntity.Rotation;
        public Vector3 Size => _avatarEntity.Size;

        public AvatarFacade(
            AvatarEntity avatarEntity,
            AvatarConfig avatarConfig,
            AvatarStateModel avatarStateModel,
            IReadOnlyUserInputModel userInputModel)
        {
            _avatarEntity = avatarEntity;
            _avatarConfig = avatarConfig;
            _avatarStateModel = avatarStateModel;
            _userInputModel = userInputModel;

            _avatarStateModel.SetStartedAt(DateTime.Now);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            HandleInput();
        }

        // ToDo Limit at screenBounds
        private void HandleInput()
        {
            if (!_userInputModel.HasInput)
            {
                return;
            }

            var timeAdjustedSpeed = _avatarConfig.Speed.AsTimeAdjusted();
            var translateTarget = _userInputModel.InputVector.Value * timeAdjustedSpeed;

            _avatarEntity.transform.Translate(translateTarget);
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _avatarStateModel.StartedAt.Value;
            _avatarStateModel.SetSurvivalSeconds(timePassed.TotalSeconds);
        }

        public void ReceiveDamage(double damageAmount)
        {
            var currentHealth = _avatarStateModel.Health.Value;
            var newHealth = Math.Max(0, currentHealth - damageAmount);

            _avatarStateModel.SetHealth(newHealth);
        }
    }
}
