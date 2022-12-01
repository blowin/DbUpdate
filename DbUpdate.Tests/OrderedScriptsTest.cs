using DbUpdate.Domain;
using DbUpdate.Tests.Types;
using FluentAssertions;
using Moq;

namespace DbUpdate.Tests;

public class OrderedScriptsTest
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[]
            {
                new string[]{ "1.txt" },
                CreateFileSystem(mock => mock.Setup(system => system.FileExists("1.txt")).Returns(true)),
                "1.txt"
            };
            yield return new object[]
            {
                new string[]{ "1.txt" },
                CreateFileSystem(mock => mock.Setup(system => system.FileExists("1.txt")).Returns(true),
                    mock => mock.Setup(system => system.DirectoryExists("1.txt")).Returns(true)),
                "1.txt"
            };
            
            yield return new object[]
            {
                new string[]{ "3.txt", "1.txt", "2.txt" },
                CreateFileSystem(mock => mock.Setup(system => system.DirectoryExists("folder")).Returns(true),
                    mock => mock.Setup(system => system.EnumerateFiles("folder")).Returns(new string[]{ "1.txt", "2.txt", "3.txt" }),
                    mock => mock.Setup(system => system.GetLastWriteTime("1.txt")).Returns(new DateTime(2002, 1, 1)),
                    mock => mock.Setup(system => system.GetLastWriteTime("2.txt")).Returns(new DateTime(2003, 1, 1)),
                    mock => mock.Setup(system => system.GetLastWriteTime("3.txt")).Returns(new DateTime(2001, 1, 1))),
                "folder"
            };
        }
    }
    
    [Theory]
    [MemberData(nameof(Data))]
    public void Sort(IEnumerable<string> expectedResult, IFileSystem fileSystem, string path)
    {
        var script = new OrderedScripts(path, fileSystem);

        var result = script.ToList();

        result.Should().Equal(expectedResult);
    }
    
    [Fact]
    public void Sort_Throw_When_Invalid_Path()
    {
        var fileSystem = new Mock<IFileSystem>().Object;
        var script = new OrderedScripts("folder", fileSystem);

        Assert.Throws<ArgumentException>(() => script.ToList());
    }
    
    private static IFileSystem CreateFileSystem(params Action<Mock<IFileSystem>>[] configurations)
    {
        var mock = new Mock<IFileSystem>();
        foreach (var configuration in configurations)
            configuration(mock);
        return mock.Object;
    }
}