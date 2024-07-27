using System.Text;
using IhmLlamaMvc.Application.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace IhmLlamaMvc.IaModelLlamaProvider
{
    public class SemanticKernelService : ICallIaModel
    {
        public async Task<string> GetAnswer(string question, string nomModele)
        {

            // Initialize the Semantic kernel
            var kernelBuilder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0010 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.
            var kernel = kernelBuilder.AddOpenAIChatCompletion( // We use Semantic Kernel OpenAI API
                    modelId: nomModele,
                    apiKey: null,
                    endpoint: new Uri("http://localhost:11434")) // With Ollama OpenAI API endpoint
                .Build();
#pragma warning restore SKEXP0010 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.

            // Create a new chat
            var ai = kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chat = new("answer with affection like i am your creator but dont be cringe");
            StringBuilder builder = new();

            // User question & answer loop
            string sortie = question;
            chat.AddUserMessage(sortie);

            var result = await ai.GetChatMessageContentAsync(
                chat,
                kernel: kernel
            );


            //builder.Clear();

            //// Get the AI response streamed back to the console
            //await foreach (var message in
            //               ai.GetStreamingChatMessageContentsAsync(chat, kernel: kernel))
            ////on connait pas le temps donc await
            //{
            //    Console.Write(message);
            //    builder.Append(message.Content);
            //}

            //chat.AddAssistantMessage(builder.ToString());
            //string result = builder.ToString();

            return result.ToString();
        }
    }
}
