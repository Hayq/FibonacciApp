using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class TimeGuard : IGuard, IGuardDetail
    {
        private readonly long _interval;
        private Stopwatch _stopwatch = new();
        private KeyValuePair<string, object> _guardDetails;

        public TimeGuard(long interval)
        {
            _interval = interval;
            _stopwatch.Start();
        }

        public bool IsValid()
        {
            if (_stopwatch.ElapsedMilliseconds > _interval)
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
            _stopwatch.Stop();
            _guardDetails = new KeyValuePair<string, object>("TimeElapsedMilliseconds", _stopwatch.ElapsedMilliseconds);
        }
    }
}
