using Azure.Core;
using IhmLlamaMvc.Application.UseCases.Conversations.Queries;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.Mvc.Constants;
using IhmLlamaMvc.Mvc.Extensions;
using IhmLlamaMvc.Mvc.ViewModels.Conversation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using ReferentielAPI.Entites;

namespace IhmLlamaMvc.Mvc.Controllers;

public partial class HomeController
{


    private async Task<ConversationViewModel> CopierInfosVersConversationViewModel(
        IReadOnlyList<ModeleIA> listeModeles)
    {
        var agentPermissions = (AgentPermissions)HttpContext.Session.GetJson<AgentPermissions>(
            Constantes.SessionKeyInfosUser);

        if (agentPermissions == null)
        {
            agentPermissions = await GetInfosAgent();
        }

        var historiqueConversationsUser = await _sender.Send(new ListerConversationsUserQuery(agentPermissions.CompteAD));

        if (!historiqueConversationsUser.Value.Any())
        {
            ChargerHistoriqueConversationsToStore(agentPermissions.CompteAD, historiqueConversationsUser.Value);
        }

        var conversationViewModel = new ConversationViewModel();

        IEnumerable<SelectListItem> listeFormatee = ConstruireListeFormateeModelesIA(listeModeles);

        conversationViewModel.IdentiteAgent = $"{agentPermissions.Prenom} {agentPermissions.Nom}";
        conversationViewModel.InitialesAgent = $"{agentPermissions.Prenom.First()}{agentPermissions.Nom.First()}";
        conversationViewModel.listeModeles = listeFormatee;
        conversationViewModel.listeQuestions = new List<Question>();
        conversationViewModel.HistoriqueChats = JsonConvert.SerializeObject(historiqueConversationsUser);

        return conversationViewModel;
    }
}