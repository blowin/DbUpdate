namespace DbUpdate.Domain;

public interface IReport : IAsyncDisposable
{
    Task ReportAsync(DbUpdateResult result, CancellationToken token = default);
}