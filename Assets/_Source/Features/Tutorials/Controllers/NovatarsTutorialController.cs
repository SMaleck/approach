using _Source.Util;

namespace _Source.Features.Tutorials.Controllers
{
    public class NovatarsTutorialController : AbstractDisposableFeature
    {
        private TutorialId Id => TutorialId.Novatars;
        private readonly ITutorialModel _model;

        public NovatarsTutorialController(ITutorialsCollectionModel collectionModel)
        {
            _model = collectionModel[Id];
        }
    }
}
