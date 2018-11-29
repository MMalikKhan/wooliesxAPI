using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WooliesXAPI.DataContracts;
using WooliesXAPI.Services;

namespace WooliesXAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrolleyCalculatorController : ControllerBase
    {
        private readonly ILogger<TrolleyCalculatorController> _logger;
        private readonly ITrolleyTotalCalculator _trolleyTotalCalculator;

        public TrolleyCalculatorController(ILogger<TrolleyCalculatorController> logger,
            ITrolleyTotalCalculator trolleyTotalCalculator)
        {
            _logger = logger;
            _trolleyTotalCalculator = trolleyTotalCalculator;

        }

        // GET api/trolleyTotal
        [HttpPost]
        [Route("trolleyTotal")]
        public ActionResult<double> Get(string token,
            [FromBody] GetTrolleyTotalRequest request)
        {
            _logger.LogTrace($"token: {token}, requestObj: {JsonConvert.SerializeObject(request)}");
            if (!ModelState.IsValid)
            {
                return BadRequest("trolleyCalculator not all data is valid");
            }

            var result = _trolleyTotalCalculator.CalculateLowestTrolleyTotal(request);
            _logger.LogTrace($"calculate lowest trolley total result: {result}");
            return Ok(result);
        }
    }
}
