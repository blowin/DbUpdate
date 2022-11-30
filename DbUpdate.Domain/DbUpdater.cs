namespace DbUpdate.Domain;

public record SuccessExecution(string Path);
public record FailExecution(string Path, Exception Exception);
public record DbUpdateResult(List<SuccessExecution> SuccessExecutions, List<FailExecution> FailExecutions);

public sealed class DbUpdater
{
    private readonly string _connectionString;
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IFileSystem _fileSystem;

    public DbUpdater(string connectionString, ISqlConnectionFactory connectionFactory, IFileSystem fileSystem)
    {
        _connectionString = connectionString;
        _connectionFactory = connectionFactory;
        _fileSystem = fileSystem;
    }

    public async Task<DbUpdateResult> ExecuteAsync(string path, CancellationToken token = default)
    {
        await using var connection = await _connectionFactory.OpenAsync(_connectionString, token);
        
        var success = new List<SuccessExecution>();
        var failFiles = new List<FailExecution>();

        var orderedScripts = new OrderedScripts(path, _fileSystem);
        foreach (var filePath in orderedScripts)
        {
            try
            {
                var commandString = await _fileSystem.ReadAllTextAsync(filePath, token);
                commandString = commandString.Trim();

                await connection.ExecuteNonQueryAsync(commandString, token);
                success.Add(new SuccessExecution(filePath));
            }
            catch (Exception e)
            {
                failFiles.Add(new FailExecution(filePath, e));
            }
        }

        return new DbUpdateResult(success, failFiles);
    }
}
