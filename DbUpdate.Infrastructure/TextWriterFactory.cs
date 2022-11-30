namespace DbUpdate.Infrastructure;

public interface ITextWriterFactory
{
    TextWriter Create(ReportType reportType);
}

public class TextWriterFactory : ITextWriterFactory
{
    private readonly bool _generateFile;

    public TextWriterFactory(bool generateFile)
    {
        _generateFile = generateFile;
    }

    public TextWriter Create(ReportType reportType)
    {
        if (!_generateFile) 
            return Console.Out;
        
        var time = DateTime.Now.ToString("yyyy-MM-dd");
        var fileName = $"{time}_{reportType}{GetFileExtension(reportType)}";
        if (reportType != ReportType.CLI)
            return new StreamWriter(File.OpenWrite(fileName));

        return new AggregateTextWriter(new[]
        {
            new StreamWriter(File.OpenWrite(fileName)),
            Console.Out
        });
    }

    private static string GetFileExtension(ReportType reportType)
    {
        switch (reportType)
        {
            case ReportType.CLI:
                return ".txt";
            case ReportType.Markdown:
                return ".md";
            default:
                throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null);
        }
    }
}