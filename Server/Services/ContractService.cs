using Server.Domain;

namespace Server.Services;

public sealed class ContractService(ILogger<ContractService> logger) : IContractService
{
    public DefaultResponse Handle(Contract contract)
    {
        if (contract.Access.Count > 0)
        {
            foreach (AccessEntry entry in contract.Access)
            {
                long ts = entry.Timestamp;
                string id = entry.Id ?? "<null>";

                logger.LogInformation("Access entry => timestamp:{Timestamp} id:{Id}", ts, id);
            }
        }
        else
        {
            logger.LogWarning("No 'access' array provided.");
        }

        return DefaultResponseBuilder.Builder()
            .Message("Processed successfully")
            .Build();
    }
}