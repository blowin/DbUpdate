namespace DbUpdate.Domain;

public interface ISqlConnection : IAsyncDisposable
{
    Task ExecuteNonQueryAsync(string sql, CancellationToken token = default);
}

public interface ISqlConnectionFactory
{
    Task<ISqlConnection> OpenAsync(string connectionString, CancellationToken token = default);
}