using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Domain.Entites.Conversations
{
    public interface IConversationRepository
    {
        Task<Result<Conversation>> SauvegarderConversationEnBase(Conversation? conversation);

        Task<Result<Question>> SauvegarderQuestionEnBase(Question derniereQuestion);

        Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginWindows);

        void SupprimerUneConversation(int conversationId);
        void SupprimerToutesLesConversations(int agentId);
    }
}
