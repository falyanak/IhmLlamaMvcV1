using IhmLlamaMvc.SharedKernel.Primitives.Result;

namespace IhmLlamaMvc.Domain.Entites.IaModels
{
    public interface IModelIARepository
    {
        Task<Result<IReadOnlyList<ModeleIA>>> ChargerModelesIA();
        public Task<string> GetModelNameFromModelID(int modelID);
    }
}
