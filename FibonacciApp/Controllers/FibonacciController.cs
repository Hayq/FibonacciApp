
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

        [ResponseCache(CacheProfileName = "Default")]
        [HttpPost("generate")]
        public async Task<FibonacciSequenceResponseModel> GenerateSequance(FibonacciGenerateRequestModel generateRequestModel)
        {
            return await _fibNumGenerator.GenerateSubSequence(generateRequestModel);
        }
    }
}
