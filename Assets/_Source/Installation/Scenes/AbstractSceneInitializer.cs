using UniRx;
using Zenject;

namespace _Source.Installation
{
    public class AbstractSceneInitializer
    {
        [Inject] protected DiContainer SceneContainer;
        [Inject] protected CompositeDisposable SceneDisposer;
    }
}
