namespace AgentCustomer.Files.AgentAdapter
{
    public interface IAgentAdapter
    {
        Task<AgentDTO> GetAgent(string id);
    }
}

