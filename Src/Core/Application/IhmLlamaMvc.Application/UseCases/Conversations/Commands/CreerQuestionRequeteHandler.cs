using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Domain.Entites.Questions;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Commands
{
    public sealed class CreerQuestionRequeteHandler :
        IRequestHandler<CreerQuestionRequete, Conversation>
    {
        private readonly IChatIaService _chatIaService;

        public CreerQuestionRequeteHandler(IChatIaService chatIaService)
        {
            _chatIaService = chatIaService;
        }

        /// <inheritdoc />
        public async Task<Conversation?> Handle(CreerQuestionRequete request,
            CancellationToken cancellationToken)
        {
            Conversation? conversation = null;
            if (request.ConsersationId > 0)
            {
                 conversation = await _chatIaService
                    .GetAnswerPourConversationEnBase(
                        request.Agent, request.ConsersationId, request.Question,
                        request.ModeleId);
            }
            else
            {
                conversation = await _chatIaService.GetAnswerPourConversationEnSession(
                        request.Agent, request.Conversation, request.Question,
                        request.ModeleId, request.IdentifiantSession);
            }

            return conversation;
        }
    }
}
