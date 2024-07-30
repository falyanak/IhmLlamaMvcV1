using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Application.Services
{
    public partial class ChatIaService : IChatIaService
    {
        private readonly ICallIaModel _callIaModel;
        private readonly IAgentRepository _agentRepository;
        private readonly IModelIARepository _modelIaRepository;
        private readonly IConversationRepository _conversationRepository;

        public ChatIaService(ICallIaModel callIaModel, 
            IAgentRepository agentRepository,
            IModelIARepository modelIaRepository,
            IConversationRepository conversationRepository)
        {
            _callIaModel = callIaModel;
            _agentRepository = agentRepository;
            _modelIaRepository = modelIaRepository;
            _conversationRepository = conversationRepository;
        }

      
    }
}
