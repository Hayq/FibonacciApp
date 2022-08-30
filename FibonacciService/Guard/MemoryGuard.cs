using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class MemoryGuard : IGuard, IGuardDetail
    {
        private Func<int> _getSequenceCount;
        private long _memoryLimit;

        public MemoryGuard(long memoryLimit, Func<int> sequenceCount)
        {
            _getSequenceCount = sequenceCount;
            _memoryLimit = memoryLimit;
        }

        public bool IsValid()
        {
            var prMemory = Process.GetCurrentProcess().PrivateMemorySize64;
            if (prMemory >= _memoryLimit)
            {
                Console.WriteLine($"-->MemoryReached:{_memoryLimit} bytes");

                if (_getSequenceCount() > 0)
                {
                    return false;
                }
                else
                {
                    throw new OutOfMemoryException();
                }
            }

            return true;
        }

        public Dictionary<string, object> GetDetails()
        {
            return new Dictionary<string, object>
            {
                { "ProcessMemory_bytes", Process.GetCurrentProcess().PrivateMemorySize64 }
            };
        }
    }
}
