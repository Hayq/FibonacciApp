using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class MemoryGuard : IGuard, IGuardDetail
    {
        private long _memoryLimit;
        private Process _process;
        private KeyValuePair<string, object> _guardDetails;

        public MemoryGuard(long memoryLimit)
        {
            _memoryLimit = memoryLimit;
            _process = Process.GetCurrentProcess();
        }

        public bool IsValid()
        {
            var prMemory = _process.PrivateMemorySize64;
            if (prMemory > _memoryLimit)
            {
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
            _guardDetails = new KeyValuePair<string, object>("ProcessMemoryBytes", _process.PrivateMemorySize64);
        }
    }
}
