
using FibonacciDTO.Request;
using FibonacciDTO.Response;
using FibonacciService.FibonacciGenerator;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciApp.Controllers
{
    [Route("api/[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly IFibNumGenerator _fibNumGenerator;

        public FibonacciController(IFibNumGenerator fibNumGenerator)
        {
            _fibNumGenerator = fibNumGenerator;
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
