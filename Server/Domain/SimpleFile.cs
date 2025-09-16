using Newtonsoft.Json;

namespace Server.Domain;

public sealed record SimpleFile(
    [property: JsonProperty("path")] string Path,
    [property: JsonProperty("content")] string Content
);