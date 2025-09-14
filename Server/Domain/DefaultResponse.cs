using Newtonsoft.Json;

namespace Server.Domain;

public sealed record DefaultResponse(
    [property: JsonProperty("status")] int Status,
    [property: JsonProperty("message")] string Message
);