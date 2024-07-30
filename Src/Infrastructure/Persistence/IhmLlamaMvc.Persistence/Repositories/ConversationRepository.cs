using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.Persistence.EF;
using IhmLlamaMvc.SharedKernel.Primitives;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IhmLlamaMvc.Persistence.Repositories
{
    internal class ConversationRepository : IConversationRepository
    {
        private readonly ChatIaDbContext _dBContext;
        public ConversationRepository(ChatIaDbContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Result<Conversation>> SauvegarderConversationEnBase(Conversation conversation)
        {
            EntityEntry<Conversation> entry = null;
            try
            {
                entry = await _dBContext.Conversations.AddAsync(conversation);
                _dBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Failure<Conversation>(
                    new Error("Conversation.Sauvegarder.error", ex.Message));
            }


            return entry.Entity;
        }

        void CreerConversation(Conversation conversation)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginWindows)
        {
            var conversations = 
                await _dBContext.Conversations
               // .Include(a=>a.Agent)
                .Include(i => i.Questions)
                .ThenInclude(q => q.Reponse)
               .Where(c => c.Agent.LoginWindows == loginWindows)
               .ToListAsync();

            return conversations.AsReadOnly();
        }

        public async Task<Result<Question>> SauvegarderQuestionEnBase(Question derniereQuestion)
        {
            var entry= await _dBContext.Questions.AddAsync(derniereQuestion);
            _dBContext.SaveChanges();

            return entry.Entity;
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
