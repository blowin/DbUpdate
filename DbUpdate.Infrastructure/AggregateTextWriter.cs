using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DbUpdate.Infrastructure;

public sealed class AggregateTextWriter : TextWriter
{
    private readonly TextWriter[] _writers;

    public override Encoding Encoding => _writers.First().Encoding;

    public AggregateTextWriter(TextWriter[] writers)
    {
        _writers = writers;
    }

    public override void Write(char value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }

    public override void Write(char[]? buffer)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(buffer);
    }
        
    public override void Write(char[] buffer, int index, int count)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(buffer, index, count);
    }
        
    public override void Write(ReadOnlySpan<char> buffer)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(buffer);
    }
        
    public override void Write(bool value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(int value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
    
    [CLSCompliant(false)]
    public override void Write(uint value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(long value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    [CLSCompliant(false)]
    public override void Write(ulong value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(float value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(double value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }

    public override void Write(decimal value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(string? value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(object? value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write(StringBuilder? value)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(value);
    }
        
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(format, arg0);
    }
        
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(format, arg0, arg1);
    }
        
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(format, arg0, arg1, arg2);
    }
        
    public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
    {
        foreach (var textWriter in _writers)
            textWriter.Write(format, arg);
    }
        
    public override void WriteLine()
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine();
    }
        
    public override void WriteLine(char value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(char[]? buffer)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(buffer);
    }
        
    public override void WriteLine(char[] buffer, int index, int count)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(buffer, index, count);
    }

    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(buffer);
    }
        
    public override void WriteLine(bool value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(int value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    [CLSCompliant(false)]
    public override void WriteLine(uint value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(long value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    [CLSCompliant(false)]
    public override void WriteLine(ulong value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }

    public override void WriteLine(float value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(double value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }

    public override void WriteLine(decimal value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(string? value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(StringBuilder? value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine(object? value)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(value);
    }
        
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(format, arg0);
    }
        
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(format, arg0, arg1);
    }
        
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(format, arg0, arg1, arg2);
    }
        
    public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
    {
        foreach (var textWriter in _writers)
            textWriter.WriteLine(format, arg);
    }

    public override Task WriteAsync(char value) =>
        Task.WhenAll(_writers.Select(w => w.WriteAsync(value)));

    public override Task WriteAsync(string? value) =>
        Task.WhenAll(_writers.Select(w => w.WriteAsync(value)));

    public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
        => Task.WhenAll(_writers.Select(w => w.WriteAsync(value, cancellationToken)));

    public override Task WriteAsync(char[] buffer, int index, int count) =>
        Task.WhenAll(_writers.Select(w => w.WriteAsync(buffer, index, count)));

    public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) =>
        Task.WhenAll(_writers.Select(w => w.WriteAsync(buffer, cancellationToken)));

    public override Task WriteLineAsync(char value) =>
        Task.WhenAll(_writers.Select(w => w.WriteLineAsync(value)));

    public override Task WriteLineAsync(string? value) =>
        Task.WhenAll(_writers.Select(w => w.WriteLineAsync(value)));

    public override Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
        => Task.WhenAll(_writers.Select(w => w.WriteLineAsync(value, cancellationToken)));

    public override Task WriteLineAsync(char[] buffer, int index, int count) =>
        Task.WhenAll(_writers.Select(w => w.WriteLineAsync(buffer, index, count)));

    public override Task WriteLineAsync(ReadOnlyMemory<char> buffer,
        CancellationToken cancellationToken = default) =>
        Task.WhenAll(_writers.Select(w => w.WriteLineAsync(buffer, cancellationToken)));

    public override Task WriteLineAsync()
    {
        return Task.WhenAll(_writers.Select(w => w.WriteLineAsync()));
    }

    public override void Flush()
    {
        foreach (var textWriter in _writers)
            textWriter.Flush();
    }

    public override Task FlushAsync()
    {
        var tasks = _writers.Select(w => w.FlushAsync());
        return Task.WhenAll(tasks);
    }

    protected override void Dispose(bool disposing)
    {
        if(!disposing)
            return;

        for (var i = _writers.Length - 1; i >= 0; i--)
            _writers[i].Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        for (var i = _writers.Length - 1; i >= 0; i--)
            await _writers[i].DisposeAsync();
    }
}