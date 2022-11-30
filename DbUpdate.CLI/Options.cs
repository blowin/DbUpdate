using CommandLine;
using DbUpdate.Infrastructure;

public class Options
{
    [Option('c', "connectionString", Required = true)]
    public string ConnectionString { get; set; } = null!;

    [Option('p', "path", Required = true, HelpText = "Path to file or directory")]
    public string Path { get; set; } = null!;

    [Option('r', "reportType", Default = new[]{ DbUpdate.Infrastructure.ReportType.CLI })]
    public IEnumerable<ReportType>? ReportType { get; set; }

    [Option('g', "generateReportFile", HelpText = "Should generate report file")]
    public bool GenerateReportFile { get; set; }
}