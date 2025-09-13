using Newtonsoft.Json;

namespace Server.Domain;

public sealed record DefaultResponse (
    [property: JsonProperty("status")] int status,
    [property: JsonProperty("message")] string message
);