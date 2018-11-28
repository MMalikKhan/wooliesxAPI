using System;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WooliesXAPI.Common;
using WooliesXAPI.Controllers;
using WooliesXAPI.DataContracts;
using Xunit;

namespace WooliesXAPITest
{
    public class UserControllerTest
    {
        [Fact]
        
        public  void GetDefaultUserTest()
        {   
            //Arrange
            var logger = Mock.Of<ILogger<UserController>>();
            var controller = new UserController(logger);
            
            //Act
            var response = controller.Get();

            //Assert
            Assert.Equal(ApplicationConstants.UserName, response.Value.Name);
            Assert.Equal(ApplicationConstants.UserToken, response.Value.Token);
        }
    }
}
