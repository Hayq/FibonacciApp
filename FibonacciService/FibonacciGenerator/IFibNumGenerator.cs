using FibonacciDTO.Request;
using FibonacciDTO.Response;

namespace FibonacciService.FibonacciGenerator
{
    public interface IFibNumGenerator
    {
        Task<FibonacciSequenceResponseModel> GenerateSubSequence(FibonacciGenerateRequestModel requestModel);
    }
}
