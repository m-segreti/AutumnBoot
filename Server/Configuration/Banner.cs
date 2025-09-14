using System;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace Server.Configuration;

public static class Banner
{
    public static void Load(IHostApplicationBuilder builder)
    {
        string bannerPath = Path.Combine(builder.Environment.ContentRootPath, "Resources", "banner.txt");
        
        if (!File.Exists(bannerPath))
        {
            return;
        }
        
        string content = File.ReadAllText(bannerPath);
        content = content.Replace("{datetime}",  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        
        Console.WriteLine(content);
    }
}