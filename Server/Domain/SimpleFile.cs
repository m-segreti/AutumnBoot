namespace Server.Domain;

public sealed record SimpleFile(
    string path,
    string content);