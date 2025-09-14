using Server.Domain;

namespace Server.Services;

public sealed class ContractService(ILogger<ContractService> logger) : IContractService
{
    public DefaultResponse Handle(Contract contract)
    {
        DefaultResponse response;

        if (contract.Access.Count > 0)
        {
            foreach (AccessEntry entry in contract.Access)
            {
                long ts = entry.Timestamp;
                string id = entry.Id ?? "<null>";

                logger.LogInformation("Access entry => timestamp:{Timestamp} id:{Id}", ts, id);
            }

            response = DefaultResponseBuilder.Builder()
                .Message("Processed successfully")
                .Build();
        }
        else
        {
            logger.LogWarning("No 'access' array provided.");
            response = DefaultResponseBuilder.Builder()
                .Status(500)
                .Message("FAILURE!")
                .Build();
        }

        return response;
    }
}