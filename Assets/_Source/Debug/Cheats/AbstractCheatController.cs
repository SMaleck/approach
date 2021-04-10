using System;
using _Source.Util;
using UnityEngine;

namespace _Source.Debug.Cheats
{
    public class AbstractCheatController : AbstractDisposableFeature
    {
        protected void CheckInput(string key, Action action)
        {
            if (Input.GetKeyDown(key.ToLower()))
            {
                action.Invoke();
            }
        }
    }
}
