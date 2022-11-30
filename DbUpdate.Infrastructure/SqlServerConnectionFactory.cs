using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DbUpdate.Domain;

namespace DbUpdate.Infrastructure;

public class SqlServerConnectionFactory : ISqlConnectionFactory
{
    public async Task<ISqlConnection> OpenAsync(string connectionString, CancellationToken token = default)
    {
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(token);
        return new SqlServerConnection(connection);
    }

    private class SqlServerConnection : ISqlConnection
    {
        private readonly SqlConnection _connection;

        private static readonly Regex GoRegex = new Regex(@"^[\t ;]*GO[\t ;]*\d*[\t ]*(?:--.*)?$", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex NewLineSeparator = new Regex(@"(\r\n|\n\r|\n|\r)", RegexOptions.Compiled);

        public SqlServerConnection(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task ExecuteNonQueryAsync(string sql, CancellationToken token = default)
        {
            foreach (var query in Normalize(sql))
            {
                await using var command = new SqlCommand(query, _connection);
                await command.ExecuteNonQueryAsync(token);
            }
        }
        
        private static IEnumerable<string> Normalize(string sql)
        {
            // Make line endings standard to match RegexOptions.Multiline
            sql = NewLineSeparator.Replace(sql, "\n");

            // Split by "GO" statements
            var statements = GoRegex.Split(sql);

            // Remove empties, trim, and return
            return statements.Where(x => !string.IsNullOrWhiteSpace(x));
        }

        public ValueTask DisposeAsync() => _connection.DisposeAsync();
    }
}