namespace Server.Configuration;

public static class StaticConfiguration
{
    public static void Default(WebApplication server)
    {
        server.UseDefaultFiles();
        server.UseStaticFiles();
        server.MapFallbackToFile("/index.html");
    }
}