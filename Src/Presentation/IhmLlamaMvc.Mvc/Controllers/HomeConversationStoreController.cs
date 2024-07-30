using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Mvc.Constants;
using IhmLlamaMvc.Mvc.Extensions;
using ReferentielAPI.Entites;
using System.Collections.Concurrent;
using Agent = IhmLlamaMvc.Domain.Entites.Agents.Agent;

namespace IhmLlamaMvc.Mvc.Controllers
{
    public partial class HomeController
    {
        // la session http n'est pas thread safe
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices?view=aspnetcore-8.0

        // key=loginWindows Value=liste concurrente non ordonnée
        //   private ConcurrentDictionary<string, ConcurrentBag<Conversation>> conversationsStore;



        private void ChargerHistoriqueConversationsToStore(string loginWindows,
            IReadOnlyList<Conversation> historiqueConversations)
        {
            var store = GetConversationStore();

            var conversations = new ConcurrentBag<Conversation>();

            foreach (Conversation conversation in historiqueConversations)
            {
                conversations.Add(conversation);
            }

            store.TryAdd(loginWindows, conversations);

            SaveStoreToHtppSession(store);
        }

        private void MettreAJourStore(Conversation conversation)
        {
            // le login Windows de l'agent n'est pas connu du dictionnaire, ajouter clé/valeur = login/conversation
            
            var store = GetConversationStore();

            Agent agent = conversation.Agent;

            if (!store.ContainsKey(agent.LoginWindows))
            {
                var conversations = new ConcurrentBag<Conversation>();
                conversations.Add(conversation);
                var succes = store.TryAdd(agent.LoginWindows, conversations);

                SaveStoreToHtppSession(store);
                return;
            }

            // le login Windows de l'agent est connu du dictionnaire
            // mais la conversation n'a pas été trouvée
            // ajouter la conversation
            if (store[agent.LoginWindows].IsEmpty)
            {
                store[agent.LoginWindows].Add(conversation);
                SaveStoreToHtppSession(store);
                return;
            }

            Conversation conversationExistante = null;
            // la conversation existe dans le store, ajouter la dernière question/réponse
            if (conversation.Id > 0)
            {
                conversationExistante = store[agent.LoginWindows]
                   .FirstOrDefault(c => c.Id == conversation.Id);
            }
            else
            {
                conversationExistante = store[agent.LoginWindows]
                    .FirstOrDefault(c => c.IdentifiantSession == conversation.IdentifiantSession);
            }
            if (conversationExistante != null)
            {
                var derniereQuestion = conversation.Agent.ConversationCourante.Questions.LastOrDefault();

                conversationExistante.Questions.Add(derniereQuestion);
            }

            SaveStoreToHtppSession(store);

        }

        private ConcurrentDictionary<string, ConcurrentBag<Conversation>> GetConversationStore()
        {
            var store = HttpContext.Session
                .GetJson<ConcurrentDictionary<string, ConcurrentBag<Conversation>>>(
                    Constantes.SessionKeyConversationsStore);

            if (store == null)
            {
                return new ConcurrentDictionary<string, ConcurrentBag<Conversation>>();
            }

            return store;
        }


        private void SaveStoreToHtppSession(
            ConcurrentDictionary<string, ConcurrentBag<Conversation>> store)
        {
            HttpContext.Session.SetJson(Constantes.SessionKeyConversationsStore, store);
        }

        private Conversation? GetConversationCourante(Guid? identifiantSession)
        {
            if (identifiantSession == Guid.Empty)
            {
                return null;
            }

            var store = GetConversationStore();

            var agent = HttpContext.Session.GetJson<AgentPermissions>(Constantes.SessionKeyInfosAgent);

            if (!store.ContainsKey(agent.CompteAD))
            {
                throw new ApplicationException("La conversation n'a pas été trouvée !");
            }

            var conversationCourante = store[agent.CompteAD]
                .FirstOrDefault(c => c.IdentifiantSession == identifiantSession);

            if (conversationCourante == null)
            {
                throw new ApplicationException("La conversation n'a pas été trouvée !");
            }

            return conversationCourante;
        }

        private async Task<bool> EffacerConversationEnSession(Guid identifiantSession)
        {
            // le login Windows de l'agent n'est pas connu du dictionnaire, ajouter clé/valeur = login/conversation
            var store = GetConversationStore();

            Agent agent = await GetAgent();

            if (!store.ContainsKey(agent.LoginWindows))
            {
                throw new ApplicationException("L'agent n'a pas été trouvé dans la session serveur !");
            }

            var conversationCourante = store[agent.LoginWindows]
                .FirstOrDefault(c => c.IdentifiantSession == identifiantSession);

            var conversations = store[agent.LoginWindows];
            
            var result=conversations.TryTake(out conversationCourante);

            SaveStoreToHtppSession(store);

            return result;

        }
    }
}

