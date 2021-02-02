using _Source.Util;
using System;
using UnityEngine;

namespace _Source.Debug
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
