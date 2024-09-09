namespace TeamsStatusMqttPub.Core.Services.AvailabilityHandlers.MicrosoftTeams.FileSystemProviders;

/// <summary>
///     Wrapper around <see cref="Directory" />.
/// </summary>
public class DirectoryWrapper : IDirectoryProvider
{
    public bool Exists(string? path)
    {
        return Directory.Exists(path);
    }

    public string[] GetDirectories(string path, string searchPattern)
    {
        return Directory.GetDirectories(path, searchPattern);
    }

    public string[] GetFiles(string path, string searchPattern)
    {
        return Directory.GetFiles(path, searchPattern);
    }
}
