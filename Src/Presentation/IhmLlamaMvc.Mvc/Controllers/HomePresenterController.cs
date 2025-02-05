using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IhmLlamaMvc.Mvc.Controllers
{
    public partial class HomeController
    {
        private static IEnumerable<SelectListItem> ConstruireListeFormateeModelesIA(
            IReadOnlyList<ModeleIA> listeModelesIA, string modeleParDefaut="Llama" )
        {
            IEnumerable<SelectListItem> listeFormatee = listeModelesIA
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = string.Format("{0} version {1}", x.Libelle, x.Version),
                    Selected = x.Libelle.Contains(modeleParDefaut, StringComparison.OrdinalIgnoreCase)

                });

            return listeFormatee;
        }
    }
}