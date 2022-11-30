using System.Collections;

namespace DbUpdate.Domain;

public readonly struct OrderedScripts : IEnumerable<string>
{
    private readonly string _path;
    private readonly IFileSystem _fileSystem;

    public OrderedScripts(string path, IFileSystem fileSystem)
    {
        _path = path;
        _fileSystem = fileSystem;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        if (_fileSystem.FileExists(_path))
        {
            yield return _path;
            yield break;
        }

        if (_fileSystem.DirectoryExists(_path))
        {
            foreach (var file in _fileSystem.EnumerateFiles(_path).OrderBy(_fileSystem.GetLastWriteTime))
                yield return file;
            yield break;
        }

        throw new ArgumentException("Unknown path", nameof(_path));
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}