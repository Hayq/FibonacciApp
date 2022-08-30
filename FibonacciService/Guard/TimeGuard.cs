using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class TimeGuard : IGuard, IGuardDetail, IDisposable
    {
        private Func<int> _getSequenceCount;
        private CancellationTokenSource _cancelSource;
        private Stopwatch _stopwatch = new Stopwatch();

        public TimeGuard(int millisecDelay, Func<int> getSecuence)
        {
            _getSequenceCount = getSecuence;
            _cancelSource = new CancellationTokenSource(millisecDelay);
            _stopwatch.Start();
        }

        public bool IsValid()
        {
            if (_cancelSource.IsCancellationRequested)
            {
                _stopwatch.Stop();
                if (_getSequenceCount() > 0)
                {
                    Console.WriteLine($"--> TIME OUT:{_stopwatch.ElapsedMilliseconds}");
                    return false;
                }
                else
                {
                    Console.WriteLine($"--> TIME OUT EX:{_stopwatch.ElapsedMilliseconds}");
                    throw new TimeoutException();
                }
            }

            return true;
        }

        public Dictionary<string, object> GetDetails()
        {
            return new Dictionary<string, object>
            {
                { "TimeElapsed", _stopwatch.Elapsed }
            };
        }

        public void Dispose()
        {
            _cancelSource.Dispose();
        }
    }
}
