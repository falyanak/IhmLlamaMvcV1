using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Application.Services
{
    public partial class ChatIaService 
    {
        
        public Conversation DemarrerConversation()
        {
            throw new NotImplementedException();
        }

        public void TerminerConversation(Conversation conversation)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginUser)
        {
            return await _conversationRepository.ListerConversationAgent(loginUser);
        }
    }
}
