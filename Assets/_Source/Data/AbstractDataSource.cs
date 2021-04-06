using System.Collections.Generic;
using UnityEngine;

namespace _Source.Data
{
    public abstract class AbstractDataSource<T> : ScriptableObject
    {
        [SerializeField] protected List<T> _dataEntries;
        public IReadOnlyList<T> DataEntries => _dataEntries;
    }
}
