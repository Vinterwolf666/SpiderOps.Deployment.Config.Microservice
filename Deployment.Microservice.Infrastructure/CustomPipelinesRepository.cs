using Deployment.Microservice.APP;
using Deployment.Microservice.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Microservice.Infrastructure
{
    public class CustomPipelinesRepository : ICustomerPipelinesRepository
    {
        private readonly CustomPipelinesDBContext _dbContext;
        public CustomPipelinesRepository(CustomPipelinesDBContext dbContext)
        {

            _dbContext = dbContext;

        }

        public List<CustomPipelines> AllPipelinesByID(int customer_id)
        {
            var result = _dbContext.CustomPipelinesDomain.Where(a=>a.CUSTOMER_ID == customer_id).ToList();

            return result;
        }

        public async Task<string> DeletePipelineByID(int id)
        {
            var _id = await _dbContext.CustomPipelinesDomain.FirstOrDefaultAsync(a => a.CUSTOMER_ID == id);

            if( _id != null )
            {
                _dbContext.CustomPipelinesDomain.Remove(_id);
                await _dbContext.SaveChangesAsync();

                return "Custom Pipeline removed successfully";
            }
            else
            {
                return "Invalid Custom Pipeline ID";
            }
        }

        public async Task<FileContentResult> UpdatePipeline(int customer_id, int template_id, string cluster_name, string ArtifactRegistry, string REGION, string appname)
        {
             
            string url = $"http://localhost:7143/Deployment.Microservice.API.Controllers/DownloadPipeline?id={template_id}&file_name=build";


            var pipe = new CustomPipelines();

            pipe.CUSTOMER_ID = customer_id;
            pipe.TEMPLATE_ID = template_id;
            pipe.CLUSTER_NAME = cluster_name;
            pipe.ArtifactRegistry = ArtifactRegistry;
            pipe.REGION = REGION;
            pipe.APPNAME = appname;
            pipe.CREATED_AT = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

            using (var client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

                
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();


                content = content.Replace("{{CUSTOMER_ID}}", customer_id.ToString())
                                 .Replace("{{CLUSTER_NAME}}", cluster_name)
                                 .Replace("{{ARTIFACT_REGISTRY}}", ArtifactRegistry)
                                 .Replace("{{REGION}}", REGION)
                                 .Replace("{{APP_NAME}}", appname);

               
                byte[] fileBytes = Encoding.UTF8.GetBytes(content);

                _dbContext.CustomPipelinesDomain.Add(pipe);
                await _dbContext.SaveChangesAsync();

                return new FileContentResult(fileBytes, "text/yaml")
                {
                    FileDownloadName = "build.yaml"
                };
            }
        }
    }
}
