using FibonacciDTO.Request;
using FibonacciService.Guard;
using System.Collections.Concurrent;

namespace FibonacciService.FibonacciGenerator
{
    public class FibNumGenerator : IFibNumGenerator
    {
        private const int MinWorkerThreadCount = 2;//TODO configurable

        private bool _running = true;
        private ConcurrentDictionary<uint, uint>? _sequence;
        private FibonacciGenerateRequestModel _requestModel;
        private readonly List<IGuard> _guards = new();

        private static Dictionary<int, int> threadIds = new Dictionary<int, int>();//TODO delete

        public async Task<IEnumerable<uint>> GenerateSubSequence(FibonacciGenerateRequestModel requestModel)
        {
            InitSequence(requestModel);
            InitThreadPool();
            InitGuards(requestModel);

            ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(2, 0, 1));

            while (_running)
            {
                //Console.WriteLine($"-->IsRun {_running} ThreadCount:{ThreadPool.ThreadCount} PendingWorkItemCount:{ThreadPool.PendingWorkItemCount} CompletedWorkItemCount:{ThreadPool.CompletedWorkItemCount}");
                await Task.CompletedTask;
            }

            return _sequence;
        }

        private void InitGuards(FibonacciGenerateRequestModel requestModel)
        {
            _guards.Add(new TimeGuard((int)requestModel.TimeLimit, () => _sequence));
            _guards.Add(new ProcessGuard(() => _running));
        }

        private void InitThreadPool()
        {
            ThreadPool.GetAvailableThreads(out int availableTh, out int availableCompTh);
            ThreadPool.GetMaxThreads(out int maxThreads, out int maxAvailableCompTh);
            ThreadPool.SetMinThreads(2, availableCompTh);
            ThreadPool.SetMaxThreads(2048, maxAvailableCompTh);
        }

        private void InitSequence(FibonacciGenerateRequestModel requestModel)
        {
            _running = true;
            _requestModel = requestModel;

            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;

            var secuenceCapacity = (int)(_requestModel.LastIndex - _requestModel.FirstIndex + 1);
            _sequence = new ConcurrentDictionary<uint, uint>(concurrencyLevel, secuenceCapacity);

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
            var isInvalid = _guards.Any(g => !g.IsValid());
            if (isInvalid)
            {
                _running = false;
                return;
            }

            var state = (GenerationState)stateObj;
            var sequenceNumb = state.prev + state.last;

            _ = ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(state.index + 1, state.last, sequenceNumb));

            //CalculateUniqueThreads();//TODO delete

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

        //TODO delete
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

        private record class GenerationState(uint index, uint prev, uint last);
    }
}
