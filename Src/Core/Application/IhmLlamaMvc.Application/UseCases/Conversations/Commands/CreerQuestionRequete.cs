using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Questions;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Commands
{
    public  class CreerQuestionRequete : IRequest<Conversation>
    {
        public string Question { get; set; }
        public int ModeleId { get; set; }
        public Agent Agent { get; set; }
        public int ConsersationId { get; set; }
        public Guid IdentifiantSession { get; set; }
        public Conversation? Conversation { get; set; }
    }
}
