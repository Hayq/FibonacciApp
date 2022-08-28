using FibonacciDTO.Request;

namespace FibonacciService.FibonacciGenerator
{
    public class FibNumGenerator : IFibNumGenerator
    {
        private const int MinWorkerThreadCount = 2;//TODO configurable

        private bool _running = true;
        private uint[] _sequence;
        private FibonacciGenerateRequestModel _requestModel;
        private CancellationTokenSource _cancelSource;


#if DEBUG
        private static Dictionary<int, int> threadIds = new Dictionary<int, int>();
#endif

        //TODO complete time out exception
        ////throw new OperationCanceledException()

        //TODO Memory reach exception
        //var pr_memory = Process.GetCurrentProcess().PrivateMemorySize64;

        public async Task<IEnumerable<uint>> GenerateSubSequence(FibonacciGenerateRequestModel requestModel)
        {
            InitSequence(requestModel);
            InitThreadPool();

            ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(2, 0, 1));

            while (_running)
            {
                //Console.WriteLine($"-->IsRun {_running} ThreadCount:{ThreadPool.ThreadCount} PendingWorkItemCount:{ThreadPool.PendingWorkItemCount} CompletedWorkItemCount:{ThreadPool.CompletedWorkItemCount}");
                await Task.CompletedTask;
            }

            return _sequence;
        }

        private void InitThreadPool()
        {
            ThreadPool.GetAvailableThreads(out int availableTh, out int availableCompTh);
            ThreadPool.GetMaxThreads(out int maxThreads, out int maxAvailableCompTh);
            ThreadPool.SetMinThreads(2, availableCompTh);
            ThreadPool.SetMaxThreads(1024, maxAvailableCompTh);
        }

        private void InitSequence(FibonacciGenerateRequestModel requestModel)
        {
            _running = true;
            _requestModel = requestModel;
            _cancelSource = new CancellationTokenSource((int)requestModel.TimeLimit);

            var secuenceCapacity = (int)(_requestModel.LastIndex - _requestModel.FirstIndex + 1);
            _sequence = new uint[secuenceCapacity];

            if (_requestModel.FirstIndex == 1)
            {
                _sequence[0] = 0;
                if (_requestModel.LastIndex > 1)
                    _sequence[1] = 1;
            }
            if (_requestModel.FirstIndex == 2)
                _sequence[0] = 1;
        }

        private void GenerateNumber(object stateObj)
        {
            if (!_running)
            {
                return;
            }

            var state = (GenerationState)stateObj;
            var sequenceNumb = state.prev + state.last;

            _ = ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(state.index + 1, state.last, sequenceNumb));

#if DEBUG
            CalculateUniqueThreads();
#endif

            if (state.index >= _requestModel.FirstIndex - 1 && state.index <= _requestModel.LastIndex - 1)
            {
                var indexInSequence = (int)(state.index - _requestModel.FirstIndex) + 1;
                _sequence[indexInSequence] = sequenceNumb;
                //Console.WriteLine($"--> T-ID:{Thread.CurrentThread.ManagedThreadId} Mum:{sequenceNumb} Index:{state.index} SeqIndex {indexInSequence}");
            }
            else if (state.index >= _requestModel.LastIndex)
            {
                _running = false;
                //Console.WriteLine($"--> TID:{Thread.CurrentThread.ManagedThreadId} Mum:{sequenceNumb} Index:{state.index}");
            }
        }

#if DEBUG
        private static void CalculateUniqueThreads()
        {
            int currentManagedThreadId = Environment.CurrentManagedThreadId;
            if (threadIds.TryGetValue(currentManagedThreadId, out var entryCount))
            {
                threadIds[currentManagedThreadId] = ++entryCount;
            }
            else
            {
                threadIds[currentManagedThreadId] = 0;
            }
        }
#endif

        private record class GenerationState(uint index, uint prev, uint last);
    }
}
