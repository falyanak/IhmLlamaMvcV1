using IhmLlamaMvc.Application.UseCases.Conversations.Commands;
using IhmLlamaMvc.Application.UseCases.IaModels.Queries;
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
   //     [ValidateAntiForgeryToken]
        public async Task<IActionResult> PoserQuestion(CreerQuestionRequete requete)
        {
            requete.Agent = await GetAgent();

            // on reste en mode session sans persistence
            if (requete.ConversationId == 0)
            {
                requete.Conversation = GetConversationCourante(requete.IdentifiantSession);
            }
            else
            {
                requete.Conversation = GetConversationCourante(requete.ConversationId);
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

        [HttpPost, ActionName("SauvegarderConversation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SauvegarderConversation([FromBody] CreerQuestionRequete requete)
        {
            if (requete.IdentifiantSession == Guid.Empty)
            {
                throw new ArgumentException("L'identifiant de la conversation n'a pas été trouvée !");
            }

            requete.sauvegarderEnBase = true;
            requete.Conversation = GetConversationCourante(requete.IdentifiantSession);

            var conversation = await _sender.Send(requete);

            return new JsonResult(new
            {
                ConversationId = conversation.Id

            });
        }

        [HttpPost, ActionName("SupprimerConversationEnSession")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConversationEnSession([FromBody]Data data)
        {

            var result = await EffacerConversationEnSession(data.IdentifiantSession);

            return new JsonResult(new { reponse = result });
        }

      [HttpPost, ActionName("ChargerConversationEnSession")]
      //  [ValidateAntiForgeryToken]
      public async Task<IActionResult> ChargerConversationEnSession([FromBody] Data data)
      {
          var listeQuestionsReponses = await ChargerConversation(data.ConversationId);

          return new JsonResult(new { reponse = listeQuestionsReponses });
      }

    }

    public class Data
    {
        public Guid IdentifiantSession { get; set; }
        public int ConversationId { get; set; }
    }
}