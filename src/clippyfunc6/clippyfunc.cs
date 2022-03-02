using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace clippyfunc6
{
    public static class clippyfunc
    {
        [FunctionName("clippyfunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string text = await new StreamReader(req.Body).ReadToEndAsync();

            string top = "";
            string bottom= "";
            for (int i = 0; i < text.Length+2; i++)
            {
                top +="_";
                bottom+="-";
            }

            string output = $" {top}\n< {text} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ \n";
            output += $"\nOS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}\n";
            output += $"\ndotnet version: {System.Environment.Version.ToString()}\n";

            string responseMessage = string.IsNullOrEmpty(output)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : output;

            return new OkObjectResult(responseMessage);
        }
    }
}
