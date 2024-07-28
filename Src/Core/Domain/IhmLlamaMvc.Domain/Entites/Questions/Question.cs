using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Reponses;
using IhmLlamaMvc.SharedKernel.Primitives;

namespace IhmLlamaMvc.Domain.Entites.Questions
{
    public class Question : EntityBase<int>
    {
        public Question(string libelle)
        {
            Libelle = libelle;
        }
        public Question(string libelle, Conversation conversation)
        {
            Libelle = libelle;
            Conversation = conversation;
        }
        public Question(string libelle, Conversation conversation, Reponse reponse)
        {
            Libelle = libelle;
            Conversation = conversation;
            Reponse = reponse;
        }

        public string Libelle { get; set; }


        public Reponse Reponse { get; set; }

        public Conversation Conversation { get; set; }
    }
}
