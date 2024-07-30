using IhmLlamaMvc.Domain.Entites.Conversations;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Commands
{
    public  class BackupConversationRequete : IRequest<Conversation>
    {
        public BackupConversationRequete(Conversation? conversation,
            Guid identifiantSession)
        {
            Conversation = conversation;
            IdentifiantSession = identifiantSession;
        }

        public Conversation? Conversation { get; set; }
        public Guid IdentifiantSession { get; set; }
    }
}
