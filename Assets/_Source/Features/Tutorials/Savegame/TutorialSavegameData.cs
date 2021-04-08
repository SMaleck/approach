using Packages.SavegameSystem.Models;
using System;

namespace _Source.Features.Tutorials.Savegame
{
    [Serializable]
    public class TutorialSavegameData : ISavegameData
    {
        public TutorialId Id;
        public bool IsCompleted;
    }
}
