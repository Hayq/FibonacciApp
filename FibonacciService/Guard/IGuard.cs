namespace FibonacciService.Guard
{
    public interface IGuard
    {
        //void Init<T>(T guardInit);

        bool IsValid();
    }
}