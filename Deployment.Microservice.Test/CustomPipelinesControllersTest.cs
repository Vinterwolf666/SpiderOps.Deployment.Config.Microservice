using Deployment.Microservice.API.Controllers;
using Deployment.Microservice.APP;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Deployment.Microservice.Test
{
    public class CustomPipelinesControllersTest
    {
        private readonly Mock<ICustomPipelinesServices> _serviceMock;
        private readonly CustomPipelinesController _controller;

        public CustomPipelinesControllersTest()
        {
            _serviceMock = new Mock<ICustomPipelinesServices>();  // Inicializa el mock
            _controller = new CustomPipelinesController(_serviceMock.Object);  // Inyecta el servicio mock en el controlador
        }

        [Fact]
        public async Task UpdatePipelineAndDownload_ReturnsActionResult_WhenServiceExecutesSuccessfully()
        {
            // Arrange
            int customerId = 1, templateId = 2;
            string clusterName = "test-cluster", artifactRegistry = "test-registry", region = "us-east1", appName = "test-app";

            var expectedResult = new FileContentResult(new byte[0], "application/octet-stream");

            _serviceMock.Setup(s => s.UpdatePipeline(customerId, templateId, clusterName, artifactRegistry, region, appName))
                        .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdatePipelineAndDownload(customerId, templateId, clusterName, artifactRegistry, region, appName);

            // Assert
            Assert.IsType<FileContentResult>(result);
        }
    }
}