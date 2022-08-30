using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class MemoryGuard : IGuard/*<IMemortGuard>*/
    {
        private Func<uint[]> _getSequence;
        private int _memoryLimit;

        public void Init(IMemortGuard guardInit)
        {
            _getSequence = guardInit.GetSequence;
            _memoryLimit = guardInit.MemryLimit;
        }

        public bool IsValid()
        {
            //TODO Memory reach exception
            var pr_memory = Process.GetCurrentProcess().PrivateMemorySize64;
            throw new NotImplementedException();
        }
    }

    public interface IMemortGuard : IGetSequence, IGuardInit
    {
        public int MemryLimit { get; set; }
    }

    public interface IGetSequence
    {
        public Func<uint[]> GetSequence { get; set; }
    }
}
