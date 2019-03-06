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

            //string sentimentURL = config["sentimentURL"];


            //HttpClient client = new HttpClient();
            //HttpContent content = new StringContent("{ \"documents\": [ { \"language\": \"en\", \"id\": \"1\", \"text\": \"" + text + "\" }]}",
            //                         Encoding.UTF8, 
            //                        "application/json");

            //var response = await client.PostAsync(sentimentURL, content);
            //var responseString = await response.Content.ReadAsStringAsync();

            //sentimentDetail sentimentDetail = JsonConvert.DeserializeObject<sentimentDetail>(responseString);
            //text = text + " Sentiment Score: " + sentimentDetail.documents[0].score;
                
            string top = "";
            string bottom= "";
            for (int i = 0; i < text.Length+2; i++)
            {
                top +="_";
                bottom+="-";
            }

            //string output = $" {top}\n< {text} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ \n\n Sentiment Score: {sentimentDetail.documents[0].score}";
            string output = $" {top}\n< {text} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ \n";

            return output != null
                ? (ActionResult)new OkObjectResult(output)
                : new BadRequestObjectResult("Please pass a message in the request body");
                
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException);
            }
        }

        public class sentimentDetail
        {
            public documents[] documents { get; set; }
            public string[] errors { get; set; }
        }

        public class documents{
            public string id { get; set; }
            public string score { get; set; }

        }
    }
}
