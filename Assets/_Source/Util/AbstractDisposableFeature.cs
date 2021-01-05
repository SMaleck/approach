using Zenject;

namespace _Source.Util
{
    public abstract class AbstractDisposableFeature : AbstractDisposable
    {
        [Inject]
        private void Inject([InjectLocal] DisposableManager disposableManager)
        {
            disposableManager.Add(this);
        }
    }
}