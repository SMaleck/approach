using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarSpawning
{
    public class NovatarPoolItem : AbstractDisposable, IEntityPoolItem
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, NovatarPoolItem> { }

        private readonly NovatarEntity _novatarEntity;
        public NovatarEntity NovatarEntity => _novatarEntity;

        private readonly NovatarStateModel _novatarStateModel;
        public NovatarStateModel NovatarStateModel => _novatarStateModel;

        public bool IsFree { get; private set; }

        public NovatarPoolItem(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;

            _novatarStateModel.IsAlive
                .Subscribe(SetIsFree)
                .AddTo(Disposer);
        }

        public void Reset()
        {
            _novatarStateModel.Reset();
        }

        private void SetIsFree(bool isFree)
        {
            IsFree = isFree;
        }
    }
}
