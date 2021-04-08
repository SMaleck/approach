using _Source.Util;

namespace _Source.Features.Tutorials.Controllers
{
    public class ControlsTutorialController : AbstractDisposableFeature
    {
        private TutorialId Id => TutorialId.Controls;
        private readonly ITutorialModel _model;

        public ControlsTutorialController(ITutorialsCollectionModel collectionModel)
        {
            _model = collectionModel[Id];
        }
    }
}
