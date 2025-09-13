using Microsoft.AspNetCore.Mvc;

public static class NewtonsoftSerializer {
    public static void configure(MvcNewtonsoftJsonOptions options) {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    }
}