using IhmLlamaMvc.SharedKernel.Primitives;

namespace IhmLlamaMvc.Domain.Entites.IaModels
{
    public class ModeleIA : EntityBase<int>
    {
        public ModeleIA(string libelle, string urlApi, string version)
        {
            Libelle = libelle;
            UrlApi = urlApi;
            Version = version;
        }

        public string Libelle { get; set; }
        public string UrlApi { get; set; }
        public string Version { get; set; }
        public string NomModeleApi { get; set; }
    }
}
