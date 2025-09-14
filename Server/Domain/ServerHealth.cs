namespace Server.Domain;

public sealed record ServerHealth(
    string Status,
    string Version
);