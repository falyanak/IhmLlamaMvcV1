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

        var conversationViewModel = new ConversationViewModel();

        IEnumerable<SelectListItem> listeFormatee = ConstruireListeFormateeModelesIA(listeModeles);

        conversationViewModel.IdentiteAgent = $"{agentPermissions.Prenom} {agentPermissions.Nom}";
        conversationViewModel.InitialesAgent = $"{agentPermissions.Prenom.First()}{agentPermissions.Nom.First()}";
        conversationViewModel.listeModeles = listeFormatee;
        conversationViewModel.listeQuestions = new List<Question>();
        conversationViewModel.HistoriqueChats = JsonConvert.SerializeObject( new List<string>()
        {
            "Hello il y a 1 jour",
            "Albatros il y a 2 jours",
            "Astronomie il y a 1 semaine"
        });

        return conversationViewModel;
    }
}