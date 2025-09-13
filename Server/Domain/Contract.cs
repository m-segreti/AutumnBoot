using Newtonsoft.Json;

namespace Server.Domain;

public sealed record Contract(
    [property: JsonProperty("name")] String Name,
    [property: JsonProperty("value")] String Value,
    [property: JsonProperty("locationControl")] LocationControl locationControl,
    [property: JsonProperty("access")] List<AccessEntry> Access
);