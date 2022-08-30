using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class MemoryGuard : IGuard, IGuardDetail
    {
        private long _memoryLimit;
        private KeyValuePair<string, object> _guardDetails;

        public MemoryGuard(long memoryLimit)
        {
            _memoryLimit = memoryLimit;
        }

        public bool IsValid()
        {
            var prMemory = Process.GetCurrentProcess().PrivateMemorySize64;
            if (prMemory > _memoryLimit)
            {
                FinalizeDetails();
                return false;
            }

            return true;
        }

        public KeyValuePair<string, object> GetDetails()
        {
            return _guardDetails;
        }

        public void FinalizeDetails()
        {
            _guardDetails = new KeyValuePair<string, object>("ProcessMemoryBytes", Process.GetCurrentProcess().PrivateMemorySize64);
        }
    }
}
