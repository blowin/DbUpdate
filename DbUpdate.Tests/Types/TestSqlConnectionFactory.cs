using DbUpdate.Domain;

namespace DbUpdate.Tests.Types;

internal class TestSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly Func<string, CancellationToken, Task> _handler;

    public TestSqlConnectionFactory(Func<string, CancellationToken, Task> handler)
    {
        _handler = handler;
    }

    public Task<ISqlConnection> OpenAsync(string connectionString, CancellationToken token = default)
        => Task.FromResult<ISqlConnection>(new TestSqlConnection(_handler));
    
    private class TestSqlConnection : ISqlConnection
    {
        private readonly Func<string, CancellationToken, Task> _handler;

        public TestSqlConnection(Func<string, CancellationToken, Task> handler)
        {
            _handler = handler;
        }

        public ValueTask DisposeAsync() => new(Task.CompletedTask);

        public Task ExecuteNonQueryAsync(string sql, CancellationToken token = default) => _handler.Invoke(sql, token);
    }
}