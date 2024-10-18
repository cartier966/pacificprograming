using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.ContentModel;
using PacificPrograming.Controllers;
using Services.Interfaces;
using System.Security.Policy;
using Xunit;

namespace ControllerTests
{
    public class AvatarControllerTests
    {
       /* [Theory()]
        [InlineData("dda34e3d%$6")]
        [InlineData("7")]
        [InlineData("dda34e3d%$8")]
        [InlineData("dda34e3d%$99")]
        public async Task GetAvatarUrl_Should_Call_ConcatenateUrlWithUserIdentifierLastNumber(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.ConcatenateUrlWithUserIdentifierLastNumber(It.IsAny<int>()), Times.Once());

        }*/

        [Theory()]
        [InlineData("dda34e3d%$6")]
        [InlineData("7")]
        [InlineData("dda34e3d%$8")]
        [InlineData("dda34e3d%$99")]
        public async Task GetAvatarUrl_Should_Call_GetUrlFromService(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            mockAvatarUrlService.Setup(e => e.GetUrlFromService(It.IsAny<int>())).ReturnsAsync(new Services.Entities.AvatarUrl());
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);
            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.GetUrlFromService(It.IsAny<int>()), Times.Once());

        }

        [Theory()]
        [InlineData("dda34e3d%$1")]
        [InlineData("2")]
        [InlineData("dda34e3d%$3")]
        [InlineData("dda34e3d%$4")]
        [InlineData("##55")]
        public async Task GetAvatarUrl_Should_Call_GetUrlFromSQLite(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            mockAvatarUrlService.Setup(e => e.GetUrlFromSQLite(It.IsAny<int>())).ReturnsAsync(new Services.Entities.AvatarUrl());
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.GetUrlFromSQLite(It.IsAny<int>()), Times.Once());

        }

        [Theory()]
        [InlineData("dda34e3d%$1")]
        [InlineData("2")]
        [InlineData("dda34e3d%$3")]
        [InlineData("dda34e3d%$4")]
        [InlineData("##55")]
        public async Task GetAvatarUrl_Should_Return_NotFoundResult(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            mockAvatarUrlService.Setup(e => e.GetUrlFromSQLite(It.IsAny<int>()));// not returning anything -> null
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
      
        }

        [Theory()]
        [InlineData("dda34e3d%$")]
        [InlineData("7fe6e")]
        [InlineData("#4dda34e3d%$0")]
        [InlineData("dda34e3d0")]
        public async Task GetAvatarUrl_Should_Call_GetStandardUrlForVowel(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.GetStandardUrlForVowel(), Times.Once());

        }

        [Theory()]
        [InlineData("dd343d%$")]
        [InlineData("7&f")]
        [InlineData("#4dd343d%$0")]
        [InlineData("dd3)43d0")]
        public async Task GetAvatarUrl_Should_Call_ConcatenateUrlWithRandomNumber(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.ConcatenateUrlWithRandomNumber(), Times.Once());

        }


        [Theory()]
        [InlineData("dd3gf43d")]
        [InlineData("7f")]
        [InlineData("4dd3hgfj43d0")]
        [InlineData("dd3fggh43d0")]
        public async Task GetAvatarUrl_Should_Call_GetDefaultUrl(string userIdentifier)
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(userIdentifier);

            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            mockAvatarUrlService.Verify(s => s.GetDefaultUrl(), Times.Once());

        }


        [Fact()]

        public async Task GetAvatarUrl_Should_Call_Throw_BadRequest()
        {
            //arrange
            var mock = new Mock<ILogger<AvatarController>>();
            ILogger<AvatarController> mockLogger = mock.Object;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var mockAvatarUrlService = new Mock<IAvatarUrlService>();
            var controller = new AvatarController(mockLogger, mockAvatarUrlService.Object, mockEnvironment.Object);

            //act
            var result = await controller.GetAvatarUrl(null);

            //assert
            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

        }

    }
}