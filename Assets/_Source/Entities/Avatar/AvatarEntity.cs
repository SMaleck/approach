using _Source.Features.UserInput;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Entities.Avatar
{
    public class AvatarEntity : AbstractMonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, AvatarEntity> { }
    }
}
