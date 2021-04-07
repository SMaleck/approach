using UniRx;
using Zenject;

namespace Packages.RxUtils
{
    /// <summary>
    /// Use this for disposable objects, created through Zenject factories
    /// </summary>
    public abstract class AbstractInjectableDisposable : AbstractDisposable
    {
        [Inject]
        private void Inject([InjectLocal] CompositeDisposable disposer)
        {
            Disposer.Add(disposer);
        }
    }
}
