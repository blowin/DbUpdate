using DbUpdate.Domain;

namespace DbUpdate.Infrastructure.Reports;

public sealed class CLIReport : ReportBase
{
    public CLIReport(TextWriter writer) : base(writer)
    {
    }

    public override Task ReportAsync(DbUpdateResult result, CancellationToken token = default)
    {
        foreach (var file in result.SuccessExecutions)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Writer.Write("Success:");
            Console.ResetColor();
            Writer.WriteLine("{0}", file.Path);
        }

        Writer.WriteLine();

        foreach (var (path, exception) in result.FailExecutions)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Writer.Write("Fail:");
            Console.ResetColor();
            Writer.WriteLine("{0}", path);
            Writer.WriteLine("{0}", exception.Message);
        }
        Writer.Flush();
        return Task.CompletedTask;
    }
}