using Deployment.Microservice.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Deployment.Microservice.APP
{
    public class PipelinesServices : IPipelinesServices
    {
        private readonly IPipelinesRepository _repository;
        public PipelinesServices(IPipelinesRepository repository)
        {

            _repository = repository;

        }

     
        public async Task<FileContentResult> DownloadPipeline(int id, string file_name)
        {
            var result = await _repository.DownloadPipeline(id, file_name);

            return result;
        }


    }
}
