using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Queries
{
    internal class ListerConversationsUserQueryHandler :
        IRequestHandler<ListerConversationsUserQuery,
            Result<IReadOnlyList<Conversation>>>
    {
        private readonly IChatIaService _chatIaService;

        public ListerConversationsUserQueryHandler(IChatIaService chatIaService)
        {
            _chatIaService = chatIaService;
        }
       public async Task<Result<IReadOnlyList<Conversation>>> Handle(
           ListerConversationsUserQuery request, CancellationToken cancellationToken)
       {
          return await _chatIaService.ListerConversationAgent(request.LoginUser);
       }
    }
}
