using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_Web_service.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Newtonsoft.Json;

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
        [Fact]
        public async Task adPlatformsRefresh_invalidLine_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "some name adplatform /some/name/location";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректный формат строки: отсутствует название либо локация рекламной площадки", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_invalidNameAdPlatform_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "            :/some/name/location";//некорректным названием платформы является лишь отсутствие названия платформы
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректное название рекламной площадки", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_firstExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:       ";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректное название локации", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_secondExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:/ru/";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректное название локации", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_thirdExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:ru";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректное название локации", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_fourthExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:/ru,/";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Некорректное название локации", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_goodFile_Ok()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:/ru";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            OkResult okRequestResult = Assert.IsType<OkResult>(res);
        }
        [Fact]
        public async Task adPlatformsInLocation_locationWithWorkAdPlatforms_Ok()
        {
            //Arrange
            var controller = new testController();
            string testString = "Яндекс.Директ:/ru";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            var expectedDict = new { adPlatforms = new List<string> { "Яндекс.Директ" } };
            var expectedJson = JsonConvert.SerializeObject(expectedDict);

            //Act
            await controller.adPlatformsRefresh(testFile);
            var res = controller.adPlatformsInLocation("/ru");

            //Assert
            
            OkObjectResult okRequestResult = Assert.IsType<OkObjectResult>(res);
            Assert.Equal(expectedJson, okRequestResult.Value);
        }
        [Fact]
        public void adPlatformsInLocation_locationWithoutWorkAdPlatforms_Ok()
        {
            //Arrange
            var controller = new testController();

            var expectedDict = new { adPlatforms = new List<string> { } };
            var expectedJson = JsonConvert.SerializeObject(expectedDict);

            //Act
            var res = controller.adPlatformsInLocation("/ru");

            //Assert

            OkObjectResult okRequestResult = Assert.IsType<OkObjectResult>(res);
            Assert.Equal(expectedJson, okRequestResult.Value);
        }
    }
}