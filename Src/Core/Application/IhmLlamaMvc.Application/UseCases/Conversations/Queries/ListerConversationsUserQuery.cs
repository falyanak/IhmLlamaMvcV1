using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using MediatR;

namespace IhmLlamaMvc.Application.UseCases.Conversations.Queries
{
    public sealed class ListerConversationsUserQuery :
        IRequest<Result<IReadOnlyList<Conversation>>>
       
    {
        public ListerConversationsUserQuery(string loginUser)
        {
            LoginUser = loginUser;
        }

        public string LoginUser { get; set; }

    }
}
