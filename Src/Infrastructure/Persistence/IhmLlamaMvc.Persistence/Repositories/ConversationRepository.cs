using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Persistence.EF;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace IhmLlamaMvc.Persistence.Repositories
{
    internal class ConversationRepository : IConversationRepository
    {
        private readonly ChatIaDbContext _dBContext;
        public ConversationRepository(ChatIaDbContext dBContext) {
            _dBContext = dBContext;
        }
        void IConversationRepository.CreerConversation(Conversation conversation)
        {
            throw new NotImplementedException();
        }

        public async  Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginWindows)
        {
            var conversations = await _dBContext.Conversations
               .Where(c => c.Agent.LoginWindows ==loginWindows)
               .ToListAsync();

           return  conversations.AsReadOnly();
        }


        void IConversationRepository.SupprimerToutesLesConversations(int agentId)
        {
            throw new NotImplementedException();
        }

        void IConversationRepository.SupprimerUneConversation(int conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
