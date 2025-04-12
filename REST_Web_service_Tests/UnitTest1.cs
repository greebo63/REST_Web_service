using Microsoft.AspNetCore.Mvc;
using REST_Web_service.Controllers;

namespace REST_Web_service_Tests
{
    public class AdPlatformsControllerTests
    {
        [Fact]
        public async Task adPlatformsRefresh_noFile_badRequest()
        {
            //Arrange
            var controller = new testController();

            //Act
            var res = await controller.adPlatformsRefresh(null);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Вы не загрузили файл", badRequestResult.Value);
        }
    }
}