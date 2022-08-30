using FibonacciDTO.Request;
using FibonacciService.Guard;
using System.Collections.Concurrent;

namespace FibonacciService.FibonacciGenerator
{
    public class FibNumGenerator : IFibNumGenerator, IDisposable
    {
        private bool _canProcess = true;
        private ConcurrentDictionary<int, ulong> _sequence;
        private FibonacciGenerateRequestModel _requestModel;
        private readonly List<IGuard> _guards = new();

        public async Task<IEnumerable<ulong>> GenerateSubSequence(FibonacciGenerateRequestModel requestModel)
        {
            InitSequence(requestModel);
            InitThreadPool();
            InitGuards(requestModel);

            _ = ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(2, 0, 1));

            while (_canProcess)
            {
                //Console.WriteLine($"-->IsRun {_running} ThreadCount:{ThreadPool.ThreadCount} PendingWorkItemCount:{ThreadPool.PendingWorkItemCount} CompletedWorkItemCount:{ThreadPool.CompletedWorkItemCount}");
                await Task.CompletedTask;
            }

            var sequence = _sequence.OrderBy(s => s.Key)
                                    .Select(s => s.Value);

            return sequence;
        }

        private void InitGuards(FibonacciGenerateRequestModel requestModel)
        {
            _guards.Add(new TimeGuard(requestModel.TimeLimit.Value, () => _sequence.Count));
            //_guards.Add(new MemoryGuard(requestModel.MemoryLimit.Value, () => _sequence.Count));
            _guards.Add(new ProcessGuard(() => _canProcess));
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
            _canProcess = true;
            _requestModel = requestModel;

            var numProcs = Environment.ProcessorCount;
            var concurrencyLevel = numProcs * 2;

            var secuenceCapacity = (int)(_requestModel.LastIndex - _requestModel.FirstIndex + 1);
            _sequence = new ConcurrentDictionary<int, ulong>(concurrencyLevel, secuenceCapacity);

            TryInitSequenceFirstItems();
        }

        private void TryInitSequenceFirstItems()
        {
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
                _canProcess = false;
                return;
            }

            var state = (GenerationState)stateObj;
            var sequenceNumb = state.Prev + state.Last;

            _ = ThreadPool.QueueUserWorkItem(GenerateNumber, new GenerationState(state.Index + 1, state.Last, sequenceNumb));

            if (state.Index >= _requestModel.FirstIndex - 1 && state.Index <= _requestModel.LastIndex - 1)
            {
                var indexInSequence = state.Index - _requestModel.FirstIndex.Value + 1;
                _sequence[indexInSequence] = sequenceNumb;
            }
            else if (state.Index >= _requestModel.LastIndex)
            {
                _canProcess = false;
            }
        }

        public void Dispose()
        {
            var guards = _guards.OfType<IDisposable>();
            foreach(var guard in guards)
            {
                guard.Dispose();
            }
        }

        private record class GenerationState(int Index, ulong Prev, ulong Last);
    }
}
