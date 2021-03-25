using Zenject;

namespace _Source.Util
{
    public abstract class AbstractDisposableFeature : AbstractDisposable
    {
        [Inject]
        private void Inject([InjectLocal] IDisposer disposer)
        {
            disposer.Add(this);
        }
    }
}