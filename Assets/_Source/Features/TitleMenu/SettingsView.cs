using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.TitleMenu
{
    public class SettingsView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, SettingsView> { }

        [Header("Language Buttons")]
        [SerializeField] private Button _englishLanguageButton;
        [SerializeField] private TextMeshProUGUI _englishLanguageButtonText;
        [SerializeField] private Button _germanLanguageButton;
        [SerializeField] private TextMeshProUGUI _germanLanguageButtonText;

        [Header("Close Button")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _closeButtonText;

        public void Initialize()
        {
            Localize();
        }

        public void Localize()
        {
            _englishLanguageButtonText.text = TextService.LanguageName(Language.English);
            _germanLanguageButtonText.text = TextService.LanguageName(Language.German);
        }
    }
}
