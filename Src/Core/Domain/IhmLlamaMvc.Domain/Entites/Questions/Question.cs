using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.Reponses;
using IhmLlamaMvc.SharedKernel.Primitives;

namespace IhmLlamaMvc.Domain.Entites.Questions
{
    public class Question : EntityBase<int>
    {
        public Question()
        {
        }
        public Question(string libelle, Reponse reponse, Conversation? conversation)
        {
            Libelle = libelle;
            Reponse = reponse;
            Conversation = conversation;
        }

        public Question(string libelle)
        {
            Libelle = libelle;
        }
     

        public string Libelle { get; set; }


        public Reponse Reponse { get; set; }

        public Conversation? Conversation { get; set; }
    }
}
