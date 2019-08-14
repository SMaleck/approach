using _Source.Features.TitleMenu;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInitializer : IInitializable
    {
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;

        [Inject] private TitleView.Factory _titleViewFactory;

        public void Initialize()
        {
            _titleViewFactory
                .Create(_viewPrefabsConfig.TitleViewPrefab)
                .Initialize();
        }
    }
}
