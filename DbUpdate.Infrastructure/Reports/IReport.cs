using DbUpdate.Domain;

namespace DbUpdate.Infrastructure.Reports;

public interface IReport : IAsyncDisposable
{
    Task ReportAsync(DbUpdateResult result, CancellationToken token = default);
}