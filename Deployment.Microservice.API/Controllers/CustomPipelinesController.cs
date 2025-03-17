using Deployment.Microservice.APP;
using Deployment.Microservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Deployment.Microservice.API.Controllers
{
    [ApiController]
    [Route("Deployment.Microservice.API")]
    public class CustomPipelinesController : Controller
    {
        private readonly ICustomPipelinesServices _customPipelinesServices;
        public CustomPipelinesController(ICustomPipelinesServices c)
        {
            _customPipelinesServices = c;
        }

        [HttpGet]
        [Route("UpdatePipelineAndDownload")]
        public async Task<ActionResult> UpdatePipelineAndDownload(int customer_id, int template_id, string cluster_name, string ArtifactRegistry, string REGION, string APP_NAME)
        {
            try
            {
                var result = await _customPipelinesServices.UpdatePipeline(customer_id, template_id, cluster_name, ArtifactRegistry, REGION, APP_NAME);

                return result;



            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
