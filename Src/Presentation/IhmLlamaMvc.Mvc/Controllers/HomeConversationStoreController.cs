using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.Mvc.Constants;
using IhmLlamaMvc.Mvc.Extensions;
using System.Collections.Concurrent;

namespace IhmLlamaMvc.Mvc.Controllers
{
    public partial class HomeController
    {
        // la session http n'est pas thread safe
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices?view=aspnetcore-8.0

        // key=loginWindows Value=liste concurrente non ordonnée
        private ConcurrentDictionary<string, ConcurrentBag<Conversation>> conversationsStore;

        private ConcurrentDictionary<string, ConcurrentBag<Conversation>> GetStore()
        {
            if (HttpContext.Session.GetJson<ConcurrentDictionary<string, ConcurrentBag<Conversation>>>(
                    Constants.Constantes.SessionKeyConversationsStore) == null)
            {
                return new ConcurrentDictionary<string, ConcurrentBag<Conversation>>();
            }

            var store = HttpContext.Session
                .GetJson<ConcurrentDictionary<string, ConcurrentBag<Conversation>>>(
                    Constantes.SessionKeyConversationsStore);

            if (store == null)
            {
                throw new ApplicationException("Le store n'a pas été trouvé !");
            }

            return store;
        }


        private void ChargerHistoriqueConversationsToStore(string loginWindows,
            IReadOnlyList<Conversation> historiqueConversations)
        {
            var store = GetStore();

            var conversations = new ConcurrentBag<Conversation>();

            foreach (Conversation conversation in historiqueConversations)
            {
                conversations.Add(conversation);
            }

            store.TryAdd(loginWindows, conversations);

            HttpContext.Session.SetJson<ConcurrentDictionary<string, ConcurrentBag<Conversation>>>(
                Constantes.SessionKeyConversationsStore, store);
        }

        // la question contient la question
        private void AddQuestionToConversation(string loginWindows, Question question)
        {
            var store = GetStore();

            // l'agent n'est pas connu, créer une conversion et ajouter la question
            if (!store.ContainsKey(loginWindows))
            {
                AjouterQuestionPourAgentInconnu(loginWindows, question, store);
                return;
            }

            // le store contient le login mais pas la conversation, la créer
            var conversationUser = store[loginWindows]
                .FirstOrDefault(c => c.Id == question.Conversation.Id);

            if (conversationUser == null)
            {
                AjouterQuestionSiConversationNonTrouvee(loginWindows, question, store);
                return;
            }

            // le login existe, la conversation existe, ajouter la question
            conversationUser.Questions.Add(question);
            SaveToStore(store);
        }

        private void AjouterQuestionSiConversationNonTrouvee(
            string loginWindows, Question question, ConcurrentDictionary<string, ConcurrentBag<Conversation>> store)
        {
            var conversations = store[loginWindows];
            var conversation = new Conversation();

            conversation.Questions.Add(question);
            conversations.Add(conversation);

            store[loginWindows] = conversations;
            SaveToStore(store);
        }

        private void AjouterQuestionPourAgentInconnu(
            string loginWindows, Question question, ConcurrentDictionary<string, ConcurrentBag<Conversation>> store)
        {

            var conversations = new ConcurrentBag<Conversation>();
            var conversation = new Conversation();

            conversation.Questions.Add(question);
            conversations.Add(conversation);

            store[loginWindows] = conversations;
            SaveToStore(store);
        }

        private void SaveToStore(ConcurrentDictionary<string, ConcurrentBag<Conversation>> store)
        {
            HttpContext.Session.SetJson<ConcurrentDictionary<string, ConcurrentBag<Conversation>>>(
                Constantes.SessionKeyConversationsStore, store);
        }

    }
}

