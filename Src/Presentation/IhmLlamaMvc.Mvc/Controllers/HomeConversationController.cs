using IhmLlamaMvc.Application.Interfaces;
using IhmLlamaMvc.Application.UseCases.Conversations.Commands;
using IhmLlamaMvc.Application.UseCases.IaModels.Queries;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.Domain.Entites.Reponses;
using Microsoft.AspNetCore.Mvc;

namespace IhmLlamaMvc.Mvc.Controllers
{
    public partial class HomeController
    {
        public async Task<IActionResult> ShowIaPrompt()
        {
            var listeModelesIA = await _sender.Send(new ListerModelesIAQuery());

            var conversationViewModel =
               await CopierInfosVersConversationViewModel(listeModelesIA.Value);

            return View(conversationViewModel);
        }



        [HttpPost, ActionName("PoserQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PoserQuestion(CreerQuestionRequete requete)
        {
            requete.Agent = await GetAgent();

            // on reste en mode session sans persistence
            if (requete.ConsersationId == 0)
            {
                requete.Conversation = GetConversationCourante(requete.IdentifiantSession);
            }

            var conversation = await _sender.Send(requete);

            MettreAJourStore(conversation);

            var derniereQuestion = conversation.Agent.ConversationCourante.Questions.LastOrDefault();

            return new JsonResult(new
            {
                Reponse = derniereQuestion.Reponse.Libelle,
                IdentifiantSession = conversation.IdentifiantSession
            });
        }

        [HttpPost, ActionName("SauvegarderQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SauvegarderQuestion(
            BackupConversationRequete requete)
        {
            requete.Conversation = GetConversationCourante(requete.IdentifiantSession);
            var conversation = await _sender.Send(requete);

            return new JsonResult(new
            {
                ConversationId = conversation.Id

            });
        }

        [HttpPost, ActionName("SupprimerConversationEnSession")]
        //     [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConversationEnSession([FromBody]Data data)
        {

            var result = await EffacerConversationEnSession(data.IdentifiantSession);

            return new JsonResult(new { reponse = result });
        }

     
    }

    public class Data
    {
        public Guid IdentifiantSession { get; set; }
    }
}