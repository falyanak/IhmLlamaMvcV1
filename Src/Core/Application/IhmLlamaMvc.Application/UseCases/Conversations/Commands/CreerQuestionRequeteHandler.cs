using IhmLlamaMvc.Application.Interfaces;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Commands
{
    public sealed class CreerQuestionRequeteHandler :
        IRequestHandler<CreerQuestionRequete, string>
    {
        private readonly IChatIaService _chatIaService;

        public CreerQuestionRequeteHandler(IChatIaService chatIaService)
        {
            _chatIaService = chatIaService;
        }

        /// <inheritdoc />
        public async Task<string> Handle(CreerQuestionRequete request,
            CancellationToken cancellationToken)
        {
            var reponse = await _chatIaService.GetAnswer(request.Question, request.ModeleId);

            return reponse;
        }
    }
}
