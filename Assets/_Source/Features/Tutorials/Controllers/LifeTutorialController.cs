using _Source.Util;

namespace _Source.Features.Tutorials.Controllers
{
    public class LifeTutorialController : AbstractDisposableFeature
    {
        private TutorialId Id => TutorialId.Life;
        private readonly ITutorialModel _model;

        public LifeTutorialController(ITutorialsCollectionModel collectionModel)
        {
            _model = collectionModel[Id];
        }
    }
}
