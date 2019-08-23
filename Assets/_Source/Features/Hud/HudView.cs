using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Features.TitleMenu;
using _Source.Util;
using Zenject;

namespace _Source.Features.Hud
{
    public class HudView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, SettingsView> { }

        [Inject]
        private void Inject()
        {

        }

        public void Initialize()
        {
            
        }
    }
}
