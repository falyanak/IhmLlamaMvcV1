using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using MediatR;

namespace IhmLlamaMvc.Application.Interfaces
{
    public interface IChatIaService
    {

        public Task<Result<Conversation>> SauvegarderConversationEnBase(Conversation conversation);

        public Task<Conversation?> GetAnswerPourConversationEnSession(
            Agent agentConnecte, Conversation? conversation,
            string question, int modelId, Guid identifiantSession);

        public Task<Conversation?> GetAnswerPourConversationEnBase(
            Conversation  conversation, string question);


        public Task<Result<IReadOnlyList<ModeleIA>>> ListerModelesIA();
        public Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginUser);

    }
}
