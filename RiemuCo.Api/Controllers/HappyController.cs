using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RiemuCo.Api.Controllers
{
    public class HappyController : ApiController
    {
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<bool> Get(string imageUrl)
        {
            return await DetectHappiness(imageUrl);
        }


        [SwaggerOperation("Post")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<bool> Post()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var multipart = await request.Content.ReadAsMultipartAsync();
            var content = multipart.Contents[0];
            var stream = await content.ReadAsStreamAsync();

            var fileName = content.Headers.ContentDisposition.FileName;
            

            var cred = new StorageCredentials("riemu", "rsKKZg4bMaToTm+C7ELEFu6brMYMYVC9C9jWebGE+UJOfGVnY94wz2z/0jT+QnF1dqwLv3avBX3RprEhrtoaJQ==");
            var container = new CloudBlobContainer(new Uri("http://blob.riemu.co/images/"), cred);

            
            var blob = container.GetBlockBlobReference(fileName);

            blob.UploadFromStream(stream);

            return await DetectHappiness(blob.Uri.AbsoluteUri);
            
        }

        private async Task<bool> DetectHappiness(string imageUrl)
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient("a57418dab3db465fa9c28a25635bc5a5");

            Emotion[] emotionResult;
            emotionResult = await emotionServiceClient.RecognizeAsync(imageUrl);
            if (emotionResult[0].Scores.Happiness > 0.8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
