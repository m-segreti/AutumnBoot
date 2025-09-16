using Newtonsoft.Json;

namespace Server.Domain;

public sealed record ServerHealth(
    [property: JsonProperty("status")] string Status,
    [property: JsonProperty("version")] string Version
);