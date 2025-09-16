using System.Reflection;
using Server.AutumnBoot;

namespace Server;

public static class Server
{
    public static void Main(string[] args)
    {
        AutumnBootApplication.Run(Assembly.GetExecutingAssembly(), args);
    }
}