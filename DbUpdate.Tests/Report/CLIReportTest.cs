using DbUpdate.Domain;
using DbUpdate.Infrastructure.Reports;
using FluentAssertions;

namespace DbUpdate.Tests.Report;

public class CLIReportTest
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>(), new List<FailExecution>()), @"
" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt")
            }, new List<FailExecution>()), @"Success:1.txt

" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt"),
                new SuccessExecution("2.txt"),
            }, new List<FailExecution>()), @"Success:1.txt
Success:2.txt

" };
            
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt"),
                new SuccessExecution("2.txt"),
            }, new List<FailExecution>
            {
                new FailExecution("3.txt", new Exception("Error 1")),
                new FailExecution("4.txt", new Exception("Error 2")),
            }), @"Success:1.txt
Success:2.txt

Fail:3.txt
Error 1
Fail:4.txt
Error 2
" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>(), new List<FailExecution>
            {
                new FailExecution("3.txt", new Exception("Error 1")),
                new FailExecution("4.txt", new Exception("Error 2")),
            }), @"
Fail:3.txt
Error 1
Fail:4.txt
Error 2
" };
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public async Task ReportAsync(DbUpdateResult result, string expectedResult)
    {
        var writer = new StringWriter();
        var report = new CLIReport(writer, new FakeConsoleProvider());

        await report.ReportAsync(result);
        var reportResult = writer.ToString();

        reportResult.Should().BeEquivalentTo(expectedResult);
    }
    
    private sealed class FakeConsoleProvider : IConsoleProvider
    {
        public ConsoleColor ForegroundColor { get; set; }
        public void ResetColor() { }
    }
}