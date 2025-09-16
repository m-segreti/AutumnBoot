using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnsiColors;
using Microsoft.Extensions.Logging;

namespace Server.Services;

/// <inheritdoc cref="ISimpleFileService" />
public sealed class SimpleFileService(ILogger<SimpleFileService> logger) : ISimpleFileService
{
    public void Save(string path, string content)
    {
        logger.LogInformation("{Message}", $"Saving file {path} triggered on Thread: [{Environment.CurrentManagedThreadId}]");
        
        // Trigger async process without waiting for response
        _ = Task.Run(async () =>
        {
            using (TimedTask.Timer(logger, $"Async Thread [{Environment.CurrentManagedThreadId}]"))
            {
                try
                {
                    await SaveAsync(path, content, CancellationToken.None);
                    logger.LogInformation("Task Completed");
                }
                catch (OperationCanceledException cancelledException)
                {
                    logger.LogInformation(cancelledException, "Async Thread canceled [{Thread}]", Environment.CurrentManagedThreadId);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "{Error}", Ansi.Error($"Background save failed for {path}"));
                }
            }
        });
    }
    
    private async Task SaveAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        // Simulate 5 seconds delay in processing
        logger.LogInformation("Sleeping for 5 seconds on Thread: [{Thread}]", Environment.CurrentManagedThreadId);
        Thread.Sleep(5000);
        
        // If no absolute file path or content is provided, fail
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(content))
        {
            throw new ArgumentNullException(nameof(path));
        }
        
        string directory = Path.GetDirectoryName(path);
        
        // If parent doesn't exist, ensure it gets created
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // Server message details for visibility
        if (!File.Exists(path))
        {
            logger.LogWarning("{FileWarning}", Ansi.Warning($"File {path} does not exist. Creating new file."));
        }
        else
        {
            logger.LogInformation("File {Path} found. Updating.", path);
        }
        
        // Save the file
        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
        
        logger.LogInformation("{Success}", Ansi.Success($"File {path} saved successfully"));
    }
}