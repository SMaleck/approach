﻿using System;
using _Source.Features.SceneManagement;
using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.UiScreens
{
    public class SettingsView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, SettingsView> { }

        [Header("Screen Elements")]
        [SerializeField] private TextMeshProUGUI _screenTitle;

        [Header("Language Buttons")]
        [SerializeField] private Button _englishLanguageButton;
        [SerializeField] private TextMeshProUGUI _englishLanguageButtonText;
        [SerializeField] private Button _germanLanguageButton;
        [SerializeField] private TextMeshProUGUI _germanLanguageButtonText;

        private ISceneManagementController _sceneManagementController;

        [Inject]
        private void Inject(ISceneManagementController sceneManagementController)
        {
            _sceneManagementController = sceneManagementController;
        }

        public void Initialize()
        {
            _englishLanguageButton.OnClickAsObservable()
                .Subscribe(_ => SwitchLanguageTo(Language.English))
                .AddTo(Disposer);

            _germanLanguageButton.OnClickAsObservable()
                .Subscribe(_ => SwitchLanguageTo(Language.German))
                .AddTo(Disposer);

            SetupLanguageButtons();
            Localize();
        }

        private void SetupLanguageButtons()
        {
            switch (TextService.CurrentLanguage)
            {
                case Language.English:
                    _englishLanguageButton.interactable = false;
                    _germanLanguageButton.interactable = true;
                    break;
                case Language.German:
                    _englishLanguageButton.interactable = true;
                    _germanLanguageButton.interactable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SwitchLanguageTo(Language language)
        {
            TextService.SetLanguage(language);
            _sceneManagementController.ToTitle();
        }

        public void Localize()
        {
            _screenTitle.text = TextService.Settings();
            _englishLanguageButtonText.text = TextService.LanguageName(Language.English);
            _germanLanguageButtonText.text = TextService.LanguageName(Language.German);
        }
    }
}
