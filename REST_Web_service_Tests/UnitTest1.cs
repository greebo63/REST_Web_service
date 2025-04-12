using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_Web_service.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Assert.Equal("�� �� ��������� ����", badRequestResult.Value);
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
            Assert.Equal("������������ ������ ������: ����������� �������� ���� ������� ��������� ��������", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_invalidNameAdPlatform_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "            :/some/name/location";//������������ ��������� ��������� �������� ���� ���������� �������� ���������
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("������������ �������� ��������� ��������", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_firstExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "������.������:       ";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("������������ �������� �������", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_secondExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "������.������:/ru/";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("������������ �������� �������", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_thirdExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "������.������:ru";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("������������ �������� �������", badRequestResult.Value);
        }

        [Fact]
        public async Task adPlatformsRefresh_invalidLocationName_fourthExample_badRequest()
        {
            //Arrange
            var controller = new testController();
            string testString = "������.������:/ru,/";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("������������ �������� �������", badRequestResult.Value);
        }
        [Fact]
        public async Task adPlatformsRefresh_goodFile_Ok()
        {
            //Arrange
            var controller = new testController();
            string testString = "������.������:/ru";
            byte[] arr = Encoding.UTF8.GetBytes(testString);
            var testFile = new FormFile(new MemoryStream(arr), 0, arr.Length, "testString", "testFileName");

            //Act
            var res = await controller.adPlatformsRefresh(testFile);

            //Assert
            OkResult okRequestResult = Assert.IsType<OkResult>(res);
        }
    }
}