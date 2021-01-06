using _Source.Features.Actors.Data;
using _Source.Util;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class DamageDataComponent : AbstractDisposable, IDataComponent
    {
        public class Factory : PlaceholderFactory<IDamageData, DamageDataComponent> { }

        private readonly IDamageData _data;

        public int Damage => _data.TouchDamage;

        public DamageDataComponent(IDamageData data)
        {
            _data = data;
        }
    }
}
