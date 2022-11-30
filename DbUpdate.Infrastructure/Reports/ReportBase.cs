using DbUpdate.Domain;

namespace DbUpdate.Infrastructure.Reports;

public abstract class ReportBase : IReport
{
    protected readonly TextWriter Writer;

    protected ReportBase(TextWriter writer)
    {
        Writer = writer;
    }
    
    public abstract Task ReportAsync(DbUpdateResult result, CancellationToken token = default);

    public ValueTask DisposeAsync() => Writer.DisposeAsync();
}