using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Application.Interfaces
{
    public interface IChatIaService
    {
        public Conversation DemarrerConversation();
        public void TerminerConversation(Conversation conversation);
        public Task<string> GetAnswer(string question, int modelId);

        public Task<Result<IReadOnlyList<ModeleIA>>> ListerModelesIA();
    }
}
