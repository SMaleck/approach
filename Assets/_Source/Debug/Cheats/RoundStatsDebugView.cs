using _Source.Features.PlayerStatistics;
using _Source.Util;
using System.Text;
using UnityEngine;
using Zenject;

namespace _Source.Debug.Cheats
{
    public class RoundStatsDebugView : AbstractView
    {
        [Inject] private readonly IGameRoundStatisticsModel _gameRoundStatisticsModel;

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

            var textHeight = h * 2f / 100;
            var cornerOffsetX = (h * 2 / 200);
            var cornerOffsetY = h - textHeight;

            var rect = new Rect(cornerOffsetX, cornerOffsetY, w, textHeight);

            GUI.Label(rect, GetDebugString(), Style);
        }

        private string GetDebugString()
        {
            return new StringBuilder()
                .Append($"F {_gameRoundStatisticsModel.Friends.Value} ({_gameRoundStatisticsModel.FriendsLost.Value} lost) | ")
                .Append($"E {_gameRoundStatisticsModel.Enemies.Value} | ")
                .Append($"N {_gameRoundStatisticsModel.Neutral.Value}")
                .ToString();
        }
    }
}
