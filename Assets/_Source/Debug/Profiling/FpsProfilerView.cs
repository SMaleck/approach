using UnityEngine;
using Zenject;

namespace _Source.Util.Debug
{
    public class FpsProfilerView : MonoBehaviour
    {
        [Inject] private readonly FpsProfiler _fpsProfiler;

        private GUIStyle _style;
        private GUIStyle Style => _style ?? (_style = CreateGUIStyle());

        private static GUIStyle CreateGUIStyle()
        {
            var style = new GUIStyle
            {
                normal = { textColor = new Color(0.0f, 1.0f, 0.0f, 0.7f) },
                alignment = TextAnchor.UpperLeft
            };
            var h = Screen.height;
            style.fontSize = h * 2 / 100;

            return style;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            var cornerOffset = h * 2 / 200;
            var rect = new Rect(cornerOffset, cornerOffset, w, h * 2f / 100);

            GUI.Label(rect, GetFpsString(), Style);
        }

        private string GetFpsString()
        {
            return $"{_fpsProfiler.CurrentFps} | AVG: {_fpsProfiler.AverageFps} | MIN: {_fpsProfiler.MinFps} | MAX: {_fpsProfiler.MaxFps}";
        }
    }
}
