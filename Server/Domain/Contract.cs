using Newtonsoft.Json;

namespace Server.Domain;

public sealed record Contract(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("value")] string Value,
    [property: JsonProperty("locationControl")] LocationControl LocationControl,
    [property: JsonProperty("access")] List<AccessEntry> Access
);