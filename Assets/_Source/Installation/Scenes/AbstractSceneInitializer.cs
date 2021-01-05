using UniRx;
using Zenject;

namespace _Source.Installation.Scenes
{
    public class AbstractSceneInitializer
    {
        [Inject] protected DiContainer SceneContainer;
        [Inject] protected CompositeDisposable SceneDisposer;
    }
}
