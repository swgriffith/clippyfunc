//Testing actions again
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;


namespace clippyfunc
{
    public static class clippy
    {
        [FunctionName("clippy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try{
                
            string text = new StreamReader(req.Body).ReadToEnd();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
           
            string top = "";
            string bottom= "";
            for (int i = 0; i < text.Length+2; i++)
            {
                top +="_";
                bottom+="-";
            }

            string output = $" {top}\n< {text} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ \n";

            output += $"\nOS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}\n";

            //Check if KeyVault is enabled, pull the setting and write to output
            if(Convert.ToBoolean(config["USE_KEYVAULT_FLEXVOL"]))
            {
                string secretPath = config["SECRET_PATH"];
                var mySecretSetting = LoadKVSettings(secretPath);
                output += $"\nSecret Value: {mySecretSetting}\n";
            }

            return output != null
                ? (ActionResult)new OkObjectResult(output)
                : new BadRequestObjectResult("Please pass a message in the request body");
                
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException);
            }
        }

        //If key vault flag is set this will load the file from the 
        //mounted FlexVol on the pod
        private static string LoadKVSettings(string secretPath)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(secretPath))
                {
                // Read the stream to a string, and write the string to the console.
                    String secretValue = sr.ReadToEnd();
                    return secretValue;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

    }
}
