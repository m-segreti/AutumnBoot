using Newtonsoft.Json;

namespace Server.Domain;

public sealed record AccessEntry(
    [property: JsonProperty("timestamp")] long Timestamp,
    [property: JsonProperty("id")] String Id
);