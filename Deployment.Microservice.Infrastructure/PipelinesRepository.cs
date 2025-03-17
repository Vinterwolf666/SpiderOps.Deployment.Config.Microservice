using Deployment.Microservice.APP;
using Deployment.Microservice.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Microservice.Infrastructure
{
    public class PipelinesRepository : IPipelinesRepository
    {
        private readonly PipelinesDBContext _dbContext;
        public PipelinesRepository(PipelinesDBContext dbContext)
        {

            _dbContext = dbContext;

        }

        public async Task<FileContentResult> DownloadPipeline(int id,string file_name)
        {
            var pipeline = await _dbContext.PipelinesDomain.FindAsync(id);

            if (pipeline == null || pipeline.YAML_FILE == null || pipeline.YAML_FILE.Length == 0)
            {
                throw new FileNotFoundException("Invalid YAML file.");
            }

            return new FileContentResult(pipeline.YAML_FILE, "application/x-yaml")
            {
                FileDownloadName = $"{file_name}.yaml"
            };
        }

       

    }
}
