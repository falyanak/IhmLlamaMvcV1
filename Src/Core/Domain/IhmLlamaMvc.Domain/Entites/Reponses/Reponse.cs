using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.SharedKernel.Primitives;
using Newtonsoft.Json;

namespace IhmLlamaMvc.Domain.Entites.Reponses
{
    public class Reponse : EntityBase<int>
    {
        public Reponse(string libelle)
        {
            Libelle = libelle;
        }

        public string Libelle { get; set; }

        //    public int QuestionId { get; set; } // Required foreign key property

        public Question QuestionPosee { get; set; }
    }
}
