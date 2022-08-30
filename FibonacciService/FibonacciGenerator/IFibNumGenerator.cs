using FibonacciDTO.Request;

namespace FibonacciService.FibonacciGenerator
{
    public interface IFibNumGenerator
    {
        Task<IEnumerable<ulong>> GenerateSubSequence(FibonacciGenerateRequestModel requestModel);
    }
}
