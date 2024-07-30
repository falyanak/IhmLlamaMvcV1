using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Reponses;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using Agent = IhmLlamaMvc.Domain.Entites.Agents.Agent;
using Question = IhmLlamaMvc.Domain.Entites.Questions.Question;

namespace IhmLlamaMvc.Application.Services
{
    public partial class ChatIaService
    {
        public async Task<Result<Conversation>> SauvegarderConversationEnBase(Conversation conversation)
        {
            var agentExisteEnBase =
                await _agentRepository.RechercherUnAgent(conversation.Agent.LoginWindows);

            // l'agent n'est pas connu en base, créer une conversation et lui ajouter la question
            if (agentExisteEnBase.Value is null)
            {
                // supprimer la navigation vers ModeleIA et utiliser uniquement une clé étrangère
                conversation.ModeleIAId = conversation.ModeleIA.Id;
                conversation.ModeleIA = null;
                var result = await _conversationRepository.SauvegarderConversationEnBase(conversation);

                return result;
            }

            // l'agent existe en base mais n'a pas d'historique de chat
            // créer une conversation et lui ajouter la question
            if (!agentExisteEnBase.Value.Conversations.Any())
            {
                var result = await _conversationRepository.SauvegarderConversationEnBase(conversation);

                return result;
            }

            // l'agent existe en base a  1 historique de chat mais la question n'appartient à aucun historique
            // créer une conversation et lui ajouter la question

            // l'agent existe en base a  1 historique de chat et la question appartient à un historique
            // lui ajouter la question

            if (conversation.Id > 0)
            {
                //conversation = agentExisteEnBase.Value.Conversations
                //    .FirstOrDefault(c => c.Id == conversation.Id);

              
                //if (conversation == null)
                //{
                //    throw new ApplicationException("conversationId > 0 et conversation non trouvée en base");
                //}

                // var derniereQuestion = conversation.Agent.ConversationCourante.Questions.LastOrDefault();
                var derniereQuestion = conversation.Questions.LastOrDefault();


                var result = await _conversationRepository.SauvegarderQuestionEnBase(derniereQuestion);
            }

            return conversation;
        }

        public async Task<Conversation?> GetAnswerPourConversationEnBase(
            Conversation conversation, string question)
        {
            var ListemodeleIa = await _modelIaRepository.ChargerModelesIA();

            var modeleIa = ListemodeleIa.Value
                .FirstOrDefault(m => m.Id == conversation.ModeleIAId);
            if (modeleIa is null)
            {
                throw new ApplicationException("Le modèle d'IA n'a pas été trouvé !");
            }

            var modelNameApi = modeleIa.NomModeleApi;

            var reponse = await _callIaModel.GetAnswer(question, modelNameApi);
            var reponseDonnee = new Reponse(reponse);

            //  Conversation? conversation = null;
            var agentExisteEnBase = await _agentRepository
                .RechercherUnAgent(conversation.Agent.LoginWindows);

            // l'agent n'est pas connu en base, créer une conversation et lui ajouter la question
            if (agentExisteEnBase.Value is null)
            {
                //conversation = await CreerConversationPourAgentInconnu(agentConnecte,
                //    question, reponseDonnee, modeleIa);

                conversation = new Conversation(modeleIa, conversation.Agent, question);
                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            // l'agent existe en base mais n'a pas d'historique de chat
            // créer une conversation et lui ajouter la question
            if (!agentExisteEnBase.Value.Conversations.Any())
            {
                conversation = new Conversation(modeleIa, conversation.Agent, question);
                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            // l'agent existe en base a  1 historique de chat mais la question n'appartient à aucun historique
            // créer une conversation et lui ajouter la question
            if (conversation.Id == 0)
            {
                conversation = new Conversation(modeleIa, conversation.Agent, question);
                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            // l'agent existe en base a  1 historique de chat et la question appartient à un historique
            // lui ajouter la question

            if (conversation.Id > 0)
            {
                conversation = agentExisteEnBase.Value.Conversations
                    .FirstOrDefault(c => c.Id == conversation.Id);
                if (conversation == null)
                {
                    throw new ApplicationException("conversationId > 0 et conversation non trouvée en base");
                }

                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            return conversation;
        }

        public async Task<Conversation?> GetAnswerPourConversationEnSession(
            Agent agentConnecte, Conversation? conversation, string question,
            int modelId, Guid identifiantSession)
        {
            var ListemodeleIa = await _modelIaRepository.ChargerModelesIA();
            var modeleIa = ListemodeleIa.Value.FirstOrDefault(m => m.Id == modelId);
            if (modeleIa is null)
            {
                throw new ApplicationException("Le modèle d'IA n'a pas été trouvé !");
            }

            var modelNameApi = modeleIa.NomModeleApi;

            var reponse = await _callIaModel.GetAnswer(question, modelNameApi);
            var reponseDonnee = new Reponse(reponse);

            // l'agent n'est pas connu en base, créer une conversation et lui ajouter la question
            if (identifiantSession == Guid.Empty)
            {
                //conversation = await CreerConversationPourAgentInconnu(agentConnecte,
                //    question, reponseDonnee, modeleIa);

                conversation = new Conversation(modeleIa, agentConnecte, question);
                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            // l'agent existe en base a 1 historique de chat et la question appartient à un historique
            // lui ajouter la question

            if (conversation.IdentifiantSession != Guid.Empty)
            {

                if (conversation.IdentifiantSession != identifiantSession)
                {
                    throw new ApplicationException("conversationId > 0 et conversation non trouvée en session");
                }

                var questionAvecReponse = new Question(question, reponseDonnee, conversation);

                conversation.Questions.Add(questionAvecReponse);

                conversation.Agent.ConversationCourante = conversation;

                return conversation;
            }

            return conversation;
        }


        public async Task<Result<IReadOnlyList<Conversation>>> ListerConversationAgent(string loginUser)
        {
            return await _conversationRepository.ListerConversationAgent(loginUser);
        }


    }
}
