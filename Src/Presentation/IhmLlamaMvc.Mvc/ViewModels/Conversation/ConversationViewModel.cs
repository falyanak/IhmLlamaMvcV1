using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Domain.Entites.Questions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReferentielAPI.Entites;
using System.ComponentModel.DataAnnotations;

namespace IhmLlamaMvc.Mvc.ViewModels.Conversation;

public class ConversationViewModel
{
    [Required(ErrorMessage = "La saisie de la question est obligatoire.")]
    [Display(Name = "Question")]
    public string Question { get; set; } = "";


    [Required(ErrorMessage = "L'utilisation d'un modèle d'IA est obligatoire.")]
    [Range(1, int.MaxValue, ErrorMessage = "La saisie du modèle d'IA est obligatoire.")]
    [Display(Name = "Modèle d'IA")]
    public int ModeleId { get; set; }

    public string InitialesAgent { get; set; }
    public string IdentiteAgent { get; set; }
    public AgentPermissions AgentPermissions { get; set; }
    public IEnumerable<SelectListItem> listeModeles { get; set; }

    public List<Question> listeQuestions { get; set; }
    public string HistoriqueChats { get; set; }
}