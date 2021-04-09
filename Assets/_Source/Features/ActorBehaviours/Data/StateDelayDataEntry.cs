using System;
using UnityEngine;

namespace _Source.Features.ActorBehaviours.Data
{
    [Serializable]
    public class StateDelayDataEntry
    {
        [SerializeField] private double _enemyStaySeconds;
        public double EnemyStaySeconds => _enemyStaySeconds;

        [SerializeField] private double _friendPatienceSeconds;
        public double FriendPatienceSeconds => _friendPatienceSeconds;

        [SerializeField] private double _neutralStaySeconds;
        public double NeutralStaySeconds => _neutralStaySeconds;
    }
}
