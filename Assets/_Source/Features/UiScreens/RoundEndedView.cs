using System;
using _Source.Features.GameRound;
using _Source.Features.PlayerStatistics;
using _Source.Features.SceneManagement;
using _Source.Services.Texts;
using _Source.Util;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.UiScreens
{
    public class RoundEndedView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, RoundEndedView> { }

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _resultText;

        [Header("Restart Button")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _restartButtonText;

        [Header("GoToMenu Button")]
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _goToMenuButtonText;

        [Inject] private readonly GameRoundStateController _gameRoundStateController;
        [Inject] private readonly IGameRoundStateModel _gameRoundStateModel;
        [Inject] private readonly IGameRoundStatisticsModel _gameRoundStatisticsModel;
        [Inject] private readonly ISceneManagementController _sceneManagementController;

        public void Initialize()
        {
            _gameRoundStateModel.OnRoundEnded
                .Subscribe(_ => OnRoundEnded())
                .AddTo(Disposer);

            OnOpened
                .Subscribe(_ => _gameRoundStateController.PauseRound(true))
                .AddTo(Disposer);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _sceneManagementController.ToGame())
                .AddTo(Disposer);

            _goToMenuButton.OnClickAsObservable()
                .Subscribe(_ => _sceneManagementController.ToTitle())
                .AddTo(Disposer);

            Localize();
        }

        public void Localize()
        {
            _titleText.text = TextService.End();
            _restartButtonText.text = TextService.Restart();
            _goToMenuButtonText.text = TextService.ExitToMenu();
        }

        protected void OnRoundEnded()
        {
            _resultText.text = BuildResultMessage();
            Open();
        }

        private string BuildResultMessage()
        {
            var sb = new StringBuilder();

            var friendCount = _gameRoundStatisticsModel.Friends.Value;
            var enemyCount = _gameRoundStatisticsModel.Enemies.Value;
            var friendLostCount = _gameRoundStatisticsModel.FriendsLost.Value;
            var neutralCount = _gameRoundStatisticsModel.Neutral.Value;

            // Ignore other cases if there is no stats
            if (friendCount <= 0 && enemyCount <= 0 && neutralCount <= 0)
            {
                sb.AppendLine(TextService.ResultNobody());
                return sb.ToString();
            }

            if (friendCount > 0 && enemyCount > 0)
            {
                sb.AppendLine(TextService.ResultFriendsAndEnemies(friendCount, enemyCount));
                sb.AppendLine(Environment.NewLine);
            }
            else if (friendCount > 0 && enemyCount <= 0)
            {
                sb.AppendLine(TextService.ResultOnlyFriends(friendCount));
                sb.AppendLine(Environment.NewLine);
            }
            else if (friendCount <= 0 && enemyCount > 0)
            {
                sb.AppendLine(TextService.ResultOnlyEnemies(enemyCount));
                sb.AppendLine(Environment.NewLine);
            }
            else if (friendCount <= 0 && enemyCount <= 0 && neutralCount > 0)
            {
                sb.AppendLine(TextService.ResultOnlyNeutral());
                sb.AppendLine(Environment.NewLine);
            }

            if (friendLostCount > 0)
            {
                sb.AppendLine(TextService.ResultFriendsLost(friendLostCount));
                sb.AppendLine(Environment.NewLine);
            }

            if ((friendCount > 0 || enemyCount > 0) && 
                neutralCount > 0)
            {
                sb.AppendLine(TextService.ResultNeutral());
                sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
