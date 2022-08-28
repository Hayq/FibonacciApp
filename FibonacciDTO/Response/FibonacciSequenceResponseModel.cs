namespace FibonacciDTO.Response
{
    public class FibonacciSequenceResponseModel : ResponseModelBase
    {
        public IList<ulong> Sequence { get; set; }
    }
}
