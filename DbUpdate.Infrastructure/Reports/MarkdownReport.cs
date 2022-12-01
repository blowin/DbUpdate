using DbUpdate.Domain;

namespace DbUpdate.Infrastructure.Reports;

public sealed class MarkdownReport : ReportBase
{
    public MarkdownReport(TextWriter writer) : base(writer)
    {
    }


    public override async Task ReportAsync(DbUpdateResult result, CancellationToken token = default)
    {
        await Writer.WriteLineAsync("# Success");
        await Writer.WriteLineAsync();

        await WriteTableHeader(new[] { "Path" });
        foreach (var resultSuccessExecution in result.SuccessExecutions)
            await WriteTableLine(new[] { resultSuccessExecution.Path });
        await Writer.WriteLineAsync();

        await Writer.WriteLineAsync("# Fail");
        await Writer.WriteLineAsync();

        await WriteTableHeader(new[] { "Path", "Message" });
        foreach (var (path, exception) in result.FailExecutions)
            await WriteTableLine(new[] { path, exception.Message });

        await Writer.FlushAsync();
    }

    private async Task WriteTableHeader(string[] titles)
    {
        await Writer.WriteAsync("|");
        foreach (var title in titles)
        {
            await Writer.WriteAsync(title);
            await Writer.WriteAsync("|");
        }

        await Writer.WriteLineAsync();
        await Writer.WriteAsync("|");
        foreach (var title in titles)
        {
            await Writer.WriteAsync(new string('-', title.Length));
            await Writer.WriteAsync("|");
        }
        await Writer.WriteLineAsync();
    }

    private async Task WriteTableLine(string[] data)
    {
        if(data.Length == 0)
            return;

        await Writer.WriteAsync("|");
        foreach (var value in data)
        {
            await Writer.WriteAsync(value);
            await Writer.WriteAsync("|");
        }

        await Writer.WriteLineAsync();
    }
}