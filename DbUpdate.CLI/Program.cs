using CommandLine;
using DbUpdate.Domain;
using DbUpdate.Infrastructure;

await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(RunAsync);

static async Task RunAsync(Options o)
{
    var factory = new SqlServerConnectionFactory();
    var fileSystem = new PhysicianFileSystem();

    var updater = new DbUpdater(o.ConnectionString, factory, fileSystem);
    var result = await updater.ExecuteAsync(o.Path, CancellationToken.None);

    if (o.ReportType == null)
        return;

    var writerFactory = new TextWriterFactory(o.GenerateReportFile);
    await using var report = new ReportFactory(writerFactory).Create(o.ReportType);
    await report.ReportAsync(result, CancellationToken.None);
}