using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild
{
    public static class CiBuilder
    {
        public static void Run()
        {
            EditorApplication.Exit(0);
        }
    }
}
