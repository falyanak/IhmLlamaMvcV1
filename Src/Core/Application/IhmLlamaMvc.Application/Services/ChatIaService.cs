using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;

namespace IhmLlamaMvc.Application.Services
{
    public partial class ChatIaService : IChatIaService
    {
        private readonly ICallIaModel _callIaModel;
        private readonly IModelIARepository _modelIaRepository;
        private readonly IConversationRepository _conversationRepository;

        public ChatIaService(ICallIaModel callIaModel, 
            IModelIARepository modelIaRepository,
            IConversationRepository conversationRepository)
        {
            _callIaModel = callIaModel;
            _modelIaRepository = modelIaRepository;
            _conversationRepository = conversationRepository;
        }


    }
}
