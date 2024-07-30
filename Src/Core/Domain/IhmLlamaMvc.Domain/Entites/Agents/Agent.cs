using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.SharedKernel.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhmLlamaMvc.Domain.Entites.Agents
{
    public class Agent : EntityBase<int>
    {
        public Agent(string nom, string prenom, string loginWindows)
        {
            Nom = nom;
            Prenom = prenom;
            LoginWindows = loginWindows;
            Conversations = new List<Conversation>();
        }

  
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string LoginWindows { get; set; }
        public List<Conversation> Conversations { get; set; }

        [NotMapped]
        public Conversation? ConversationCourante { get; set; }
    }
}
