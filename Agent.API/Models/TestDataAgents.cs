namespace Agent.API.Models
{
    public static class TestDataAgents
    {
        // this would be a domain call to get the appropriate agent
        public static AgentResponse GetTestData(int startIndex)
        {
            var customerList = Enumerable.Range(startIndex, 5).Select(index =>
            new CustomerResponse(index, $"Customer{index}", "Active")
            ).ToList();
            var agent = new AgentResponse(startIndex, "Agent1", "Active", customerList);
            return agent;
        }
    }
}
