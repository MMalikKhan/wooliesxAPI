using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WooliesXAPI.Common;
using WooliesXAPI.DataContracts;

namespace WooliesXAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;

        }

        // GET api/user
        [HttpGet]
        public ActionResult<GetUserResponse> Get()
        {
            _logger.LogInformation("api/user has been called...");
            var response = new GetUserResponse
            {
                Name = ApplicationConstants.UserName,
                Token = ApplicationConstants.UserToken
            };
            _logger.LogInformation($"api/user response {response.Name} {response.Token}");
            return response;
        }
    }
}
