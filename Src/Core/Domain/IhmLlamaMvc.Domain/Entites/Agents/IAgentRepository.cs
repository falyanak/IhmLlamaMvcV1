using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Domain.Entites.Agents
{
    public interface IAgentRepository
    {
        public Task<Result<Agent>> CreerAgent(Agent agent);
        public Task SupprimerAgent(int agentId);
        public Task<IReadOnlyCollection<Agent>> ListerAgents();

        public Task<Result<Agent>> RechercherUnAgent(string loginWindows);
    }
}
