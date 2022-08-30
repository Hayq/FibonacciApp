using System.Diagnostics;

namespace FibonacciService.Guard
{
    public class TimeGuard : IGuard
    {
        private Func<uint[]> _getSequence;
        private CancellationTokenSource _cancelSource;
        private Stopwatch _stopwatch = new Stopwatch();

        public TimeGuard(int millisecDelay, Func<uint[]> getSecuence)
        {
            _getSequence = getSecuence;
            _cancelSource = new CancellationTokenSource(millisecDelay);
            _stopwatch.Start();
        }

        public bool IsValid()
        {
            if (_cancelSource.IsCancellationRequested)
            {
                _stopwatch.Stop();
                if (_getSequence().Length > 0)
                {
                    throw new NotImplementedException();
                    Console.WriteLine($"--> TIME:{_stopwatch.ElapsedMilliseconds}");
                    return false;
                }
                else
                {
                    Console.WriteLine($"--~> TIME:{_stopwatch.ElapsedMilliseconds}");
                    throw new TimeoutException();
                }
            }

            return true;
        }
    }

    public class TimeGuardInit : IGetSequence, IGuardInit
    {
        public int MillisecDelay { get; set; }
        public Func<uint[]> GetSequence { get; set; }
    }
}
