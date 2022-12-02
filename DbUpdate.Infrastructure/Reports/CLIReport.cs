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

    public override async Task ReportAsync(DbUpdateResult result, CancellationToken token = default)
    {
        foreach (var file in result.SuccessExecutions)
        {
            _consoleProvider.ForegroundColor = ConsoleColor.Green;
            await Writer.WriteAsync("Success:");
            _consoleProvider.ResetColor();
            Writer.WriteLine("{0}", file.Path);
        }

        await Writer.WriteLineAsync();

        foreach (var (path, exception) in result.FailExecutions)
        {
            _consoleProvider.ForegroundColor = ConsoleColor.Red;
            await Writer.WriteAsync("Fail:");
            _consoleProvider.ResetColor();
            await Writer.WriteLineAsync(path);
            await Writer.WriteLineAsync(exception.Message);
        }
        await Writer.FlushAsync();
    }
}