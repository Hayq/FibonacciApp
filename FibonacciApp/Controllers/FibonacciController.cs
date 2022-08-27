
using FibonacciDTO.Request;
using FibonacciDTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciApp.Controllers
{
    [Route("api/[controller]")]
    public class FibonacciController : ControllerBase
    {
        public FibonacciController()
        {

        }

        [HttpPost("generate")]
        public async Task<FibonacciSequenceResponseModel> GenerateSequance(FibonacciGenerateRequestModel generateRequestModel)
        {
            await Task.CompletedTask;
            // todo
            return new FibonacciSequenceResponseModel();
        }
    }
}
