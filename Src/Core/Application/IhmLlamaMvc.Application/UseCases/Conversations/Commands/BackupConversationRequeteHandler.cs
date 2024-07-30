using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Conversations;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Commands
{
    public sealed class BackupConversationRequeteHandler :
        IRequestHandler<BackupConversationRequete, Conversation>
    {
        private readonly IChatIaService _chatIaService;

        public BackupConversationRequeteHandler(IChatIaService chatIaService)
        {
            _chatIaService = chatIaService;
        }

        /// <inheritdoc />
        public async Task<Conversation> Handle(
            BackupConversationRequete backupConversationRequete,
            CancellationToken cancellationToken)
        {
    
           var conversation= 
               await _chatIaService
                   .SauvegarderConversationEnBase(backupConversationRequete.Conversation);

            return conversation.Value;
        }
    }
}
