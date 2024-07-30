using IhmLlamaMvc.Domain.Entites.Questions;

namespace IhmLlamaMvc.Application.Interfaces
{
    public interface ICallIaModel
    {
        public Task<string> GetAnswer(string question, string nomModele);
    }
}
