﻿using _Source.App;
using _Source.Features.Actors.Data;
using _Source.Features.Movement.Data;
using UnityEngine;

namespace _Source.Features.ActorEntities.Avatar.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(AvatarDataSource), fileName = nameof(AvatarDataSource))]
    public class AvatarDataSource : ScriptableObject
    {
        [SerializeField, Range(1, 100)]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField] private MovementDataSource _movementDataSource;
        public MovementDataSource MovementDataSource => _movementDataSource;
    }
}
