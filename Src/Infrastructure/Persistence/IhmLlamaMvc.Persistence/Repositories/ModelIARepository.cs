using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Persistence.EF;
using IhmLlamaMvc.SharedKernel.Primitives;
using IhmLlamaMvc.SharedKernel.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace IhmLlamaMvc.Persistence.Repositories
{
    public class ModelIARepository : IModelIARepository
    {
        private readonly ChatIaDbContext _dBContext;

        public ModelIARepository(ChatIaDbContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<Result<IReadOnlyList<ModeleIA>>> ChargerModelesIA()
        {
            var modelesIAList = new List<ModeleIA>();
            try
            {
                modelesIAList = await _dBContext.IaModels.ToListAsync();
            }
            catch (Exception ex)
            {
                return Result.Failure<IReadOnlyList<ModeleIA>>(
                    new Error("ModelIA.Lister.error", ex.Message));
            }

            return modelesIAList;
        }

        public async Task<string> GetModelNameFromModelID(int modelId)
        {
            var modeleIa = await _dBContext.IaModels
                .FirstOrDefaultAsync(m => m.Id == modelId);

            if (modeleIa == null)
            {
                throw new ArgumentException("Le modèle d'IA est introuvable !!");
            }

            return modeleIa.NomModeleApi;
        }
    }
}
