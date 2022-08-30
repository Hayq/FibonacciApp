namespace FibonacciService.Guard
{
    public interface IGuardDetail
    {
        void FinalizeDetails();
        KeyValuePair<string, object> GetDetails();
    }
}
