using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Util;
using Zenject;

namespace _Source.Features.TitleMenu
{
    public class HowToPlayView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HowToPlayView> { }

        public void Initialize()
        {
            
        }
    }
}
