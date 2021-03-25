using System;
using UniRx;

namespace _Source.Util
{
    public class Disposer : IDisposer, IDisposable
    {
        private readonly CompositeDisposable _disposer = new CompositeDisposable();

        public Disposer()
        {
        }

        public void Add(IDisposable disposable)
        {
            if (_disposer.IsDisposed)
            {
                disposable.Dispose();
            }
            else
            {
                _disposer.Add(disposable);
            }
        }

        public void Dispose()
        {
            if (!_disposer.IsDisposed)
            {
                _disposer.Dispose();
                _disposer.Clear();
            }
        }
    }
}
