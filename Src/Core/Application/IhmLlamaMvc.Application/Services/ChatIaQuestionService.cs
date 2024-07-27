namespace IhmLlamaMvc.Application.Services
{
    public partial class ChatIaService
    {
       
        public async Task<string> GetAnswer(string question, int modelId)
        {
            var modelName = await _modelIaRepository.GetModelNameFromModelID(modelId);
            return await _callIaModel.GetAnswer(question, modelName);
        }

    }
}
