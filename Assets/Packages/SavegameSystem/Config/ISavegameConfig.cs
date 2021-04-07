namespace Packages.SavegameSystem.Config
{
    public interface ISavegameConfig
    {
        string Filename { get; }
        double RequestSaveTimeoutSeconds { get; }
    }
}
