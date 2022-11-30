using DbUpdate.Domain;
using DbUpdate.Tests.Types;
using FluentAssertions;
using Moq;

namespace DbUpdate.Tests;

public class DbUpdateTest
{
    [Fact]
    public async Task Should_Throw_Exception_When_File_And_Directory_Does_Not_Exists()
    {
        const string scriptPath = "1.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => Task.CompletedTask);
        var fileSystem = new Mock<IFileSystem>().Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await dbUpdate.ExecuteAsync(scriptPath, CancellationToken.None);
        });
    }
    
    [Fact]
    public async Task Should_Contain_Single_SuccessExecutions_When_Run_For_File_With_Success_Result()
    {
        const string scriptPath = "1.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => Task.CompletedTask);
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.FileExists(scriptPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.ReadAllTextAsync(scriptPath, CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(scriptPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().ContainSingle(execution => execution == new SuccessExecution(scriptPath));
        result.FailExecutions.Should().BeEmpty();
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_Single_FailExecutions_When_Run_For_File_With_Fail_Result()
    {
        var exception = new InvalidOperationException("Custom error");
        const string scriptPath = "1.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => throw exception);
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.FileExists(scriptPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.ReadAllTextAsync(scriptPath, CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(scriptPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.FailExecutions.Should().ContainSingle(execution => execution == new FailExecution(scriptPath, exception));
        result.SuccessExecutions.Should().BeEmpty();
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_Single_SuccessExecutions_When_Run_For_Directory_1_file_With_Success_Result()
    {
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => Task.CompletedTask);
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(scriptPath, CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().ContainSingle(execution => execution == new SuccessExecution(scriptPath));
        result.FailExecutions.Should().BeEmpty();
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_Single_FailExecutions_When_Run_For_Directory_1_file_With_Fail_Result()
    {
        var exception = new InvalidOperationException("Custom error");
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => throw exception);
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(scriptPath, CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.FailExecutions.Should().ContainSingle(execution => execution == new FailExecution(scriptPath, exception));
        result.SuccessExecutions.Should().BeEmpty();
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_2_SuccessExecutions_When_Run_For_Directory_2_file_With_Success_Result()
    {
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        const string script2Path = "2.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => Task.CompletedTask);
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath, script2Path });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(It.IsAny<string>(), CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().HaveCount(2);
        result.SuccessExecutions.Should().Contain(execution => execution.Path == scriptPath || execution.Path == script2Path);
        result.FailExecutions.Should().BeEmpty();
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_SuccessExecution_And_FailExecution_When_Run_For_Directory_2_file_With_Success_And_Fail_Result()
    {
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        const string script2Path = "2.txt";
        var position = 0;
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) =>
        {
            position += 1;
            if (position == 2)
                throw new InvalidOperationException("Custom error");
            return Task.CompletedTask;
        });
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath, script2Path });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(It.IsAny<string>(), CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().ContainSingle(execution => execution.Path == scriptPath);
        result.FailExecutions.Should().ContainSingle(execution => execution.Path == script2Path);
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_FailExecution_When_Run_For_Directory_2_file_With_Fail_Result()
    {
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        const string script2Path = "2.txt";
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) => throw new InvalidOperationException("Custom error"));
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath, script2Path });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(It.IsAny<string>(), CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().BeEmpty();
        result.FailExecutions.Should().Contain(execution => execution.Path == script2Path || execution.Path == scriptPath);
        fileSystemStub.Verify();
    }
    
    [Fact]
    public async Task Should_Contain_SuccessExecution_And_FailExecution_When_Run_For_Directory_4_file_With_Success_And_Fail_Result()
    {
        const string directoryPath = "scripts";
        const string scriptPath = "1.txt";
        const string script2Path = "2.txt";
        const string script3Path = "3.txt";
        const string script4Path = "4.txt";
        var position = 0;
        var sqlConnectionFactory = new TestSqlConnectionFactory((connectionString, token) =>
        {
            position += 1;
            if (position % 2 == 0)
                throw new InvalidOperationException("Custom error");
            return Task.CompletedTask;
        });
        var fileSystemStub = new Mock<IFileSystem>();
        fileSystemStub.Setup(system => system.DirectoryExists(directoryPath)).Returns(() => true);
        fileSystemStub.Setup(system => system.EnumerateFiles(directoryPath))
            .Returns(() => new string[]{ scriptPath, script2Path, script3Path, script4Path });
        fileSystemStub.Setup(system => system.ReadAllTextAsync(It.IsAny<string>(), CancellationToken.None))
            .Returns(() => Task.FromResult("test content"));
        
        var fileSystem = fileSystemStub.Object;
        var dbUpdate = new DbUpdater("dummy_connection_string", sqlConnectionFactory, fileSystem);

        var result = await dbUpdate.ExecuteAsync(directoryPath, CancellationToken.None);

        result.Should().NotBeNull();
        result.SuccessExecutions.Should().HaveCount(2);
        result.FailExecutions.Should().HaveCount(2);
        
        result.SuccessExecutions.Should().Contain(execution => execution.Path == scriptPath || execution.Path == script3Path);
        result.FailExecutions.Should().Contain(execution => execution.Path == script2Path || execution.Path == script4Path);
        
        fileSystemStub.Verify();
    }
}