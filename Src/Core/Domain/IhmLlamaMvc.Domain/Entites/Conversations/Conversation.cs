using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.SharedKernel.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhmLlamaMvc.Domain.Entites.Conversations
{
    public class Conversation : EntityBase<int>
    {
        public Conversation()
        {
  
        }

        // pour les tests
        public Conversation(string intitule, DateTime dateCreation,
            ModeleIA modeleIA, Agent agent,
            List<Question> questions)
        {
            Intitule = intitule;
            DateCreation = dateCreation;
            ModeleIA = modeleIA;
            Agent = agent;
            Questions = questions;

        }

        public Conversation(ModeleIA modeleIA, Agent agent, string question)
        {
            IdentifiantSession = Guid.NewGuid();
            DateCreation = DateTime.Now;
            Intitule = question + $"... du {DateCreation.ToShortDateString()}";
            ModeleIA = modeleIA;
            Agent = agent;
        }

        public  string Intitule { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateFin { get; set; }

        public ModeleIA ModeleIA { get; set; }
        public Agent Agent { get; set; }

        // FK 
        public int ModeleIAId { get; set; }

        public List<Question> Questions { get; set; }= new List<Question>();

        [NotMapped]
        public Guid IdentifiantSession { get; set; }
    }
}
