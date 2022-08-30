namespace FibonacciDTO.Response
{
    public class FibonacciSequenceResponseModel : ResponseModelBase
    {
        public IEnumerable<KeyValuePair<string, object>> Detailes { get; set; }

        public FibonacciSequenceResponseModel(IEnumerable<ulong> sequence, IEnumerable<KeyValuePair<string, object>>  detailes = null) 
        {
            Data = sequence;
            Detailes = detailes;
        }
    }
}
