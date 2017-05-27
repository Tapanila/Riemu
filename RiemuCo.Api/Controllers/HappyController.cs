using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RiemuCo.Api.Controllers
{
    public class HappyController : ApiController
    {
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<bool> Get(string imageUrl)
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
