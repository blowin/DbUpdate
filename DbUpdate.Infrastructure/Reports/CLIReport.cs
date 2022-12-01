using DbUpdate.Domain;

namespace DbUpdate.Infrastructure.Reports;

public interface IConsoleProvider
{
    ConsoleColor ForegroundColor { get; set; }
    void ResetColor();
}

public sealed class ConsoleProvider : IConsoleProvider
{
    public ConsoleColor ForegroundColor
    {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }

    public void ResetColor() => Console.ResetColor();
}

public sealed class CLIReport : ReportBase
{
    private readonly IConsoleProvider _consoleProvider;
    
    public CLIReport(TextWriter writer, IConsoleProvider consoleProvider) : base(writer)
    {
        _consoleProvider = consoleProvider;
    }

    public override Task ReportAsync(DbUpdateResult result, CancellationToken token = default)
    {
        foreach (var file in result.SuccessExecutions)
        {
            _consoleProvider.ForegroundColor = ConsoleColor.Green;
            Writer.Write("Success:");
            _consoleProvider.ResetColor();
            Writer.WriteLine("{0}", file.Path);
        }

        Writer.WriteLine();

        foreach (var (path, exception) in result.FailExecutions)
        {
            _consoleProvider.ForegroundColor = ConsoleColor.Red;
            Writer.Write("Fail:");
            _consoleProvider.ResetColor();
            Writer.WriteLine("{0}", path);
            Writer.WriteLine("{0}", exception.Message);
        }
        Writer.Flush();
        return Task.CompletedTask;
    }
}