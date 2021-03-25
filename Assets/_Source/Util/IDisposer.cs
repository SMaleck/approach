using System;

namespace _Source.Util
{
    public interface IDisposer
    {
        void Add(IDisposable disposable);
    }
}
