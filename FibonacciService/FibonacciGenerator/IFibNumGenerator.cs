using FibonacciDTO.Request;

namespace FibonacciService.FibonacciGenerator
{
    public interface IFibNumGenerator
    {
        Task<IEnumerable<uint>> GenerateSubSequence(FibonacciGenerateRequestModel requestModel);
    }
}
