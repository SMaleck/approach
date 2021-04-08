using _Source.Features.Tutorials.Savegame;
using System.Collections.Generic;
using System.Linq;

namespace _Source.Features.Tutorials
{
    public class TutorialsCollectionModel : ITutorialsCollectionModel
    {
        private readonly IReadOnlyDictionary<TutorialId, ITutorialModel> _models;

        public ITutorialModel this[TutorialId id] => _models[id];

        public TutorialsCollectionModel(
            TutorialsCollectionSavegame savegame,
            TutorialModel.Factory modelFactory)
        {
            _models = savegame.Savegames.ToDictionary(
                e => e.Key,
                e => (ITutorialModel)modelFactory.Create(e.Value));
        }
    }
}
