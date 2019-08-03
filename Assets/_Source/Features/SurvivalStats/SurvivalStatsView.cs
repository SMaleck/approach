using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.SurvivalStats
{
    public class SurvivalStatsView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, SurvivalStatsView> { }

        [SerializeField] private TextMeshProUGUI _survivalTimeText;

        private IReadOnlySurvivalStatsModel _survivalStatsModel;

        [Inject]
        private void Inject(IReadOnlySurvivalStatsModel survivalStatsModel)
        {
            _survivalStatsModel = survivalStatsModel;
        }
        
        public void Initialize()
        {
            _survivalStatsModel.SurvivalSeconds
                .Subscribe(OnSurvivalSecondsChanged)
                .AddTo(Disposer);
        }

        private void OnSurvivalSecondsChanged(double seconds)
        {
            _survivalTimeText.text = TextService.TimeFromSeconds(seconds);
        }
    }
}
