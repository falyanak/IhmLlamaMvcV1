using IhmLlamaMvc.Application.UseCases.Conversations.Queries;
using IhmLlamaMvc.Application.UseCases.IaModels.Queries;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Mvc.Constants;
using IhmLlamaMvc.Mvc.Extensions;
using IhmLlamaMvc.Mvc.ViewModels.Conversation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IhmLlamaMvc.Mvc.Controllers;

public partial class HomeController
{
    private async Task<ConversationViewModel> CopierInfosVersConversationViewModel(
        IReadOnlyList<ModeleIA> listeModeles)
    {
        HttpContext.Session.SetJson<IReadOnlyList<ModeleIA>>(Constantes.SessionKeyListeModelesIa, listeModeles);

        Domain.Entites.Agents.Agent agent = await GetAgent();

        var historiqueConversationsUser =
            await _sender.Send(new ListerConversationsUserQuery(agent.LoginWindows));

        if (historiqueConversationsUser.Value.Any())
        {
            ChargerHistoriqueConversationsToStore(agent.LoginWindows, historiqueConversationsUser.Value);
        }

        var conversationViewModel = new ConversationViewModel();

        IEnumerable<SelectListItem> listeFormatee = ConstruireListeFormateeModelesIA(listeModeles);

        conversationViewModel.IdentiteAgent = $"{agent.Prenom} {agent.Nom}";
        conversationViewModel.InitialesAgent = $"{agent.Prenom.First()}{agent.Nom.First()}";
        conversationViewModel.listeModeles = listeFormatee;

        var listeLibelleChats = historiqueConversationsUser.Value
            .Select(h => new {ConversationId=h.Id ,Intitule = h.Intitule });
        conversationViewModel.HistoriqueChats = JsonConvert.SerializeObject(listeLibelleChats.ToList());


        //var listeQuestions = historiqueConversationsUser.Value
        //    .SelectMany(c => c.Questions.Select(q => q.Reponse))
        //    .Select(qr => new
        //    {
        //        QuestionId= qr.QuestionPosee.Id,
        //        Question = qr.QuestionPosee.Libelle,
        //        ReponseId=qr.Id,
        //        Reponse = qr.Libelle
        //    });

        //conversationViewModel.listeQuestions = JsonConvert.SerializeObject(listeQuestions);

        return conversationViewModel;
    }


    private async Task<ModeleIA> GetModeleIa(int modeleId)
    {
        IReadOnlyList<ModeleIA> liste = (IReadOnlyList<ModeleIA>)HttpContext.Session.GetJson<IReadOnlyList<ModeleIA>>(Constantes.SessionKeyListeModelesIa);

        if (liste == null)
        {
            var listeModeles = await _sender.Send(new ListerModelesIAQuery());
            liste = listeModeles.Value;
            HttpContext.Session.SetJson<IReadOnlyList<ModeleIA>>(Constantes.SessionKeyListeModelesIa, liste);
        }

        var modele = liste.FirstOrDefault(m => m.Id == modeleId);

        return modele;
    }

}