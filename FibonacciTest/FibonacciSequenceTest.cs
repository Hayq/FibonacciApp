using FibonacciDTO.AppExceptions;
using FibonacciDTO.Request;
using FibonacciDTO.Response;
using FibonacciService.FibonacciGenerator;
using NUnit.Framework;
using System.Diagnostics;

namespace FibonacciTest
{
    public class FibonacciSequenceTest
    {
        readonly Stopwatch _watch = new();
        private IFibNumGenerator _fibNumGenerator;

        [SetUp]
        public void Setup()
        {
            _fibNumGenerator = new FibNumGenerator();
        }

        [TestCase(1, 10, 1000, 100000000L, false, TestName = "success 10 elements")]
        [TestCase(2, 300, 1000, 100000000L, false, TestName = "success 298 elements")]
        [TestCase(300, 3000, 1000, 100000000L, false, TestName = "success 2701 elements")]
        public async Task FibonacciSequenceGeneration_CheckValidCountOfGeneratedSubSequences(int first, int last, long time, long memory, bool skipCach = false)
        {
            var requestModel = new FibonacciGenerateRequestModel
            {
                FirstIndex = first,
                LastIndex = last,
                TimeLimit = time,
                MemoryLimit = memory,
                SkipCache = skipCach
            };

            _watch.Restart();
            _watch.Start();
            var response = await _fibNumGenerator.GenerateSubSequence(requestModel);
            _watch.Stop();

            var sequence = response.Data as IEnumerable<ulong>;
            int expected = last - first + 1;
            int actual = sequence.Count();

            Assert.IsNotNull(sequence);
            Assert.IsInstanceOf(typeof(FibonacciSequenceResponseModel), response);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(300, 3000, 10, 100000000L, false, TestName = "time limit check")]
        public void FibonacciSequenceGeneration_TimeLimitationException_ShouldThrow(int first, int last, int time, long memory, bool skipCach = false)
        {
            var requestModel = new FibonacciGenerateRequestModel
            {
                FirstIndex = first,
                LastIndex = last,
                TimeLimit = time,
                MemoryLimit = memory,
                SkipCache = skipCach
            };

            _watch.Restart();
            _watch.Start();

            var exception = Assert.ThrowsAsync(
                typeof(AppException),
                async () => await _fibNumGenerator.GenerateSubSequence(requestModel));
            _watch.Stop();
        }

        [TestCase(200, 1000, 1000, 100000L, false, TestName = "memory limit check")]
        public void FibonacciSequenceGeneration_MemoryLimitationException_ShouldThrow(int first, int last, int time, long memory, bool skipCach = false)
        {
            var requestModel = new FibonacciGenerateRequestModel
            {
                FirstIndex = first,
                LastIndex = last,
                TimeLimit = time,
                MemoryLimit = memory,
                SkipCache = skipCach
            };

            _watch.Restart();
            _watch.Start();

            var exception = Assert.ThrowsAsync(
                typeof(AppException),
                async () => await _fibNumGenerator.GenerateSubSequence(requestModel));
            _watch.Stop();
        }

        [TearDown]
        public void OnTestDown()
        {
            ShowThreadPoolState(_watch);
        }

        private static void ShowThreadPoolState(Stopwatch watch)
        {
            Console.WriteLine($"-->" +
                $"\nElapsed Time {watch.Elapsed.Milliseconds}" +
                $"\nCompletedWorkItemCount {ThreadPool.CompletedWorkItemCount}" +
                $"\nPendingWorkItemCount {ThreadPool.PendingWorkItemCount}" +
                $"\nThreadCount {ThreadPool.ThreadCount}");
        }

        private static void ShowSequence(IEnumerable<uint> sequence)
        {
            var index = 0;
            foreach (var element in sequence)
            {
                Console.Write($"[{index++}]:{element} ");
            }
            Console.WriteLine();
        }
    }
}