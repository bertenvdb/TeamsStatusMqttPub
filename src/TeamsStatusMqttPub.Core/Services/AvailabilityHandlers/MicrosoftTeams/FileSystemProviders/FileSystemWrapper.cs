namespace TeamsStatusMqttPub.Core.Services.AvailabilityHandlers.MicrosoftTeams.FileSystemProviders;

/// <summary>
///     Wrapper around physical file system.
/// </summary>
public class FileSystemWrapper : IFileSystemProvider
{
    /// <summary>
    ///     Initializes a new instance of the FileSystemWrapper class.
    /// </summary>
    public FileSystemWrapper()
    {
        Directory = new DirectoryWrapper();
    }

    public IDirectoryProvider Directory { get; }

    public IReadOnlyCollection<string> ReadAllLines(string path)
    {
        var lines = new List<string>();

        // Expect the file to be locked by another process.
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fs);

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            if (line is not null) lines.Add(line);
        }

        return lines;
    }
}
