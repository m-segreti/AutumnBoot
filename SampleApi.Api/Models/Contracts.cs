namespace SampleApi.Api.Models;
public record AccessEntry(long timestamp, string id);
public record InnerObj(float location, bool enabled);

// Matches incoming JSON shape
public record MyRequest(
    string name,
    string value,
    InnerObj obj,
    List<AccessEntry> access
);

// Outgoing response
public record MyResponse(int status, string message);