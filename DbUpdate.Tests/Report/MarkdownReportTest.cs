using DbUpdate.Domain;
using DbUpdate.Infrastructure.Reports;
using FluentAssertions;

namespace DbUpdate.Tests.Report;

public class MarkdownReportTest
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>(), new List<FailExecution>()), @"# Success

|Path|
|----|

# Fail

|Path|Message|
|----|-------|
" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt")
            }, new List<FailExecution>()), @"# Success

|Path|
|----|
|1.txt|

# Fail

|Path|Message|
|----|-------|
" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt"),
                new SuccessExecution("2.txt"),
            }, new List<FailExecution>()), @"# Success

|Path|
|----|
|1.txt|
|2.txt|

# Fail

|Path|Message|
|----|-------|
" };
            
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>
            {
                new SuccessExecution("1.txt"),
                new SuccessExecution("2.txt"),
            }, new List<FailExecution>
            {
                new FailExecution("3.txt", new Exception("Error 1")),
                new FailExecution("4.txt", new Exception("Error 2")),
            }), @"# Success

|Path|
|----|
|1.txt|
|2.txt|

# Fail

|Path|Message|
|----|-------|
|3.txt|Error 1|
|4.txt|Error 2|
" };
            yield return new object[] { new DbUpdateResult(new List<SuccessExecution>(), new List<FailExecution>
            {
                new FailExecution("3.txt", new Exception("Error 1")),
                new FailExecution("4.txt", new Exception("Error 2")),
            }), @"# Success

|Path|
|----|

# Fail

|Path|Message|
|----|-------|
|3.txt|Error 1|
|4.txt|Error 2|
" };
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public async Task ReportAsync(DbUpdateResult result, string expectedResult)
    {
        var writer = new StringWriter();
        var report = new MarkdownReport(writer);

        await report.ReportAsync(result);
        var reportResult = writer.ToString();

        reportResult.Should().BeEquivalentTo(expectedResult);
    }
}