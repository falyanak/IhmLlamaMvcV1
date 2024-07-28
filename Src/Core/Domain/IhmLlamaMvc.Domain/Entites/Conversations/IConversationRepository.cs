using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Domain.Entites.Conversations
{
    public interface IConversationRepository
    {
        void CreerConversation(Conversation conversation);
        Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginWindows);

        void SupprimerUneConversation(int conversationId);
        void SupprimerToutesLesConversations(int agentId);
    }
}
