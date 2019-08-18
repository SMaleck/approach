using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Services.Texts;
using _Source.Util;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.TitleMenu
{
    public class HowToPlayView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HowToPlayView> { }

        [Header("Close Button")]
        [SerializeField] private Button _closeButton;

        public void Initialize()
        {
            
        }

        public void Localize()
        {
            
        }
    }
}
