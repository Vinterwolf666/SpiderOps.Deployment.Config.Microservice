using Deployment.Microservice.APP;
using Deployment.Microservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Deployment.Microservice.API.Controllers
{
    [ApiController]
    [Route("Deployment.Microservice.API.Controllers")]
    public class PipelinesController : Controller
    {
        private readonly IPipelinesServices _service;
        public PipelinesController(IPipelinesServices s)
        {
            _service = s;

        }

        [HttpGet]
        [Route("DownloadPipeline")]
        public async Task<ActionResult> DownloadPipeline(int id, string file_name)
        {
            try
            {

                var result = await _service.DownloadPipeline(id,file_name);

                return result;


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
