using DbUpdate.Domain;
using DbUpdate.Infrastructure.Reports;

namespace DbUpdate.Infrastructure;

public enum ReportType
{
    CLI,
    Markdown
}

public sealed class ReportFactory
{
    private readonly ITextWriterFactory _writerFactory;
    
    public ReportFactory(ITextWriterFactory writerFactory)
    {
        _writerFactory = writerFactory;
    }

    public IReport Create(IEnumerable<ReportType> reportTypes)
    {
        var reports = reportTypes.Select(Create).ToArray();
        return new AggregateReport(reports);
    }

    private IReport Create(ReportType reportType)
    {
        var writer = _writerFactory.Create(reportType);
        return reportType switch
        {
            ReportType.CLI => new CLIReport(writer),
            ReportType.Markdown => new MarkdownReport(writer),
            _ => throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null)
        };
    }

    private sealed class AggregateReport : IReport
    {
        private readonly IReport[] _reports;

        public AggregateReport(IReport[] reports)
        {
            _reports = reports;
        }

        public Task ReportAsync(DbUpdateResult result, CancellationToken token = default)
        {
            var reportTasks = _reports.Select(e => e.ReportAsync(result, token));
            return Task.WhenAll(reportTasks);
        }

        public async ValueTask DisposeAsync()
        {
            for (var i = _reports.Length - 1; i >= 0; i--)
                await _reports[i].DisposeAsync();
        }
    }
}