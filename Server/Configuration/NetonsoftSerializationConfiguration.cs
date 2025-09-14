using Microsoft.AspNetCore.Mvc;

namespace Server.Configuration;

public static class NewtonsoftSerializer
{
    public static void Configure(MvcNewtonsoftJsonOptions options)
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    }
}