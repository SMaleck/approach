using Zenject;

namespace _Source.Util
{
    public abstract class AbstractDisposableFeature : AbstractDisposable
    {
        [Inject]
        private void Inject([InjectLocal] DisposableManager disposableManager)
        {
            // ToDo V0 Is this really needed?
            //disposableManager.Add(this);
        }
    }
}