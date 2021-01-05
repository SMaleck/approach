using _Source.Util;
using System;
using _Source.Features.ActorBehaviours.NovatarSpawning;
using UniRx;
using UnityEngine;

namespace _Source.Features.Cheats
{
    public class CheatController : AbstractDisposable
    {
        private const string SpawnNovatarKey = "s";

        private readonly NovatarSpawner _novatarSpawner;

        public CheatController(NovatarSpawner novatarSpawner)
        {
            _novatarSpawner = novatarSpawner;

            Observable.EveryUpdate()
                .Subscribe(_ => CheckAllCheatKeys())
                .AddTo(Disposer);
        }

        private void CheckAllCheatKeys()
        {
            CheckOnInput(SpawnNovatarKey, SpawnNovatar);
        }

        private void CheckOnInput(string keyCode, Action action)
        {
            if (Input.GetKeyDown(keyCode))
            {
                action();
            }
        }

        private void SpawnNovatar()
        {
            _novatarSpawner.Spawn();
        }
    }
}
