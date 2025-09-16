#nullable enable
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Server.Services;

/// <inheritdoc cref="IDisposable" />
public sealed class TimedTask : IDisposable
{
    private readonly ILogger? _logger;
    private readonly string _name;
    private readonly Stopwatch _stopwatch;

    private TimedTask(ILogger? logger, string? name)
    {
        _logger = logger;
        _name = string.IsNullOrEmpty(name) ? $"Thread [{Environment.CurrentManagedThreadId.ToString()}]" : name;
        _stopwatch = Stopwatch.StartNew();

        string message = $"{_name} started";
        
        if (_logger is null)
        {
            Console.WriteLine(message);
        }
        else
        {
            _logger.LogInformation("{Message}", message);
        }
    }
    
    public void Dispose()
    {
        _stopwatch.Stop();
        
        string message = $"{_name} completed in {_stopwatch.ElapsedMilliseconds} ms";
        
        if (_logger is null)
        {
            Console.WriteLine(message);
        }
        else
        {
            _logger.LogInformation("{Message}", message);
        }
    }

    /// <summary>
    /// Creates a new <see cref="IDisposable"/> timer for wrapping tasks with timing logs.
    /// </summary>
    /// <param name="logger">Default logger to use to log messages to the console.</param>
    /// <param name="name">The name of the operation being logged.</param>
    /// <returns>Constructed instance of <see cref="TimedTask"/></returns>
    public static TimedTask Timer(ILogger logger, string name)
    {
        return new TimedTask(logger, name);
    }
    
    /// <summary>
    /// Creates a new <see cref="IDisposable"/> timer for wrapping tasks with timing logs,
    /// using <see cref="Console"/> output instead of <see cref="Microsoft.Extensions.Logging.ILogger"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the operation being logged. If <c>null</c> or empty,
    /// the current managed thread id (<see cref="Environment.CurrentManagedThreadId"/>) is used.
    /// </param>
    /// <returns>Constructed instance of <see cref="TimedTask"/>.</returns>
    public static TimedTask Timer(string name)
    {
        return new TimedTask(null, name);
    }
    
    /// <summary>
    /// Creates an unnamed <see cref="IDisposable"/> timer that logs to <see cref="Console"/>.
    /// </summary>
    /// <returns>
    /// Constructed instance of <see cref="TimedTask"/> named with the current managed thread id
    /// (<see cref="Environment.CurrentManagedThreadId"/>).
    /// </returns>
    /// <remarks>
    /// This overload is convenient when you just want quick timing without DI.
    /// </remarks>
    public static TimedTask Timer()
    {
        return new TimedTask(null, null);
    }
}