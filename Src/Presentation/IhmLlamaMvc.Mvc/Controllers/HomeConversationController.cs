using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;


namespace IhmLlamaMvc.Controllers
{


    public partial class HomeController : Controller
    {

        //Ce code C# appartient � un contr�leur ASP.NET Core et d�finit une action HTTP POST
        //Cette annotation indique que la m�thode GetAnswer doit r�pondre aux requ�tes HTTP POST.
        //Cela signifie que cette m�thode sera invoqu�e lorsque le serveur re�oit une requ�te POST
        //� l'URL associ�e � cette action.
        [HttpPost]
        public async Task<IActionResult> GetAnswer([FromBody] Requete requete)
        {

            // Initialize the Semantic kernel
            var kernelBuilder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0010 // Le type est utilis� � des fins d��valuation uniquement et est susceptible d��tre modifi� ou supprim� dans les futures mises � jour. Supprimez ce diagnostic pour continuer.
            var kernel = kernelBuilder
                .AddOpenAIChatCompletion( // We use Semantic Kernel OpenAI API
                    modelId: "llama3",
                    apiKey: null,
                    endpoint: new Uri("http://localhost:11434")) // With Ollama OpenAI API endpoint
                .Build();
#pragma warning restore SKEXP0010 // Le type est utilis� � des fins d��valuation uniquement et est susceptible d��tre modifi� ou supprim� dans les futures mises � jour. Supprimez ce diagnostic pour continuer.

            // Create a new chat
            var ai = kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chat = new("answer with afection like i am your creator but dont be cringe");
            StringBuilder builder = new();

            // User question & answer loop
            string Sortie = requete.question;
            chat.AddUserMessage(Sortie);

            builder.Clear();

            // Get the AI response streamed back to the console
            await foreach (var message in
                           ai.GetStreamingChatMessageContentsAsync(chat, kernel: kernel))
                //on connait pas le temps donc await
            {
                Console.Write(message);
                builder.Append(message.Content);
            }

            chat.AddAssistantMessage(builder.ToString());
            string result = builder.ToString();
            Console.WriteLine("Dernier message : " + result);
            return new JsonResult(result);


        }

    }
}