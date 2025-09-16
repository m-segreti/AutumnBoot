using AnsiColors;
using Microsoft.Extensions.Logging;
using Server.Discovery;
using Server.Domain;

namespace Server.Services;

/// <inheritdoc cref="IContractService"/>
[Service]
public sealed class ContractService(ILogger<ContractService> logger) : IContractService
{
    public DefaultResponse Handle(Contract contract)
    {
        if (contract.Access.Count <= 0)
        {
            const string warningMessage = "No 'access' array provided.";
            logger.LogWarning("{Warning}", Ansi.Warning(warningMessage));

            return DefaultResponseBuilder.Builder()
                .Status(500)
                .Message(warningMessage)
                .Build();
        }

        foreach (AccessEntry entry in contract.Access)
        {
            long ts = entry.Timestamp;
            string id = entry.Id ?? "<null>";

            logger.LogInformation("Access entry => timestamp:{Timestamp} id:{Id}", ts, id);
        }

        return DefaultResponseBuilder.Of("Processed successfully");
    }
}