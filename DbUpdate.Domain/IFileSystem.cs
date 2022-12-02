namespace DbUpdate.Domain;

public interface IFileSystem
{
    bool FileExists(string path);
    bool DirectoryExists(string path);
    IEnumerable<string> EnumerateFiles(string directory);
    Task<string> ReadAllTextAsync(string filePath, CancellationToken token);
}

public class PhysicianFileSystem : IFileSystem
{
    public bool FileExists(string path) => File.Exists(path);

    public bool DirectoryExists(string path) => Directory.Exists(path);

    public IEnumerable<string> EnumerateFiles(string directory) => Directory.EnumerateFiles(directory);

    public Task<string> ReadAllTextAsync(string filePath, CancellationToken token) =>
        File.ReadAllTextAsync(filePath, token);
}