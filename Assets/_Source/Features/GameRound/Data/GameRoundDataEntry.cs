using System;
using UnityEngine;

namespace _Source.Features.GameRound.Data
{
    [Serializable]
    public class GameRoundDataEntry
    {
        [SerializeField] private double _durationSeconds;
        public double DurationSeconds => _durationSeconds;

        [SerializeField] private double _secondsPerHP;
        public double SecondsPerHP => _secondsPerHP;
    }
}
