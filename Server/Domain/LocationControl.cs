using Newtonsoft.Json;

namespace Server.Domain;

public sealed record LocationControl (
    [property: JsonProperty("location")] float Location,
    [property: JsonProperty("enabled")] bool Enabled
);