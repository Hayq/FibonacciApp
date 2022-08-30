using FibonacciDTO.AppException;
using FibonacciDTO.Request;
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

        [TestCase(1,   10,      1000, 100000L, false, TestName = "success 10")]
        [TestCase(2,   300,     1000, 100000L, false, TestName = "success 298")]
        [TestCase(300, 3000,    1000, 100000L, false, TestName = "success 2701")]
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

            var sequence = (IEnumerable<ulong>)response.Data;
            int expected = last - first + 1;
            int actual = sequence.Count();
            Assert.AreEqual(expected, actual);

            //Console.WriteLine();
            //ShowThreadPoolState(watch);
            //ShowSequence(sequence);
        }

        [TestCase(300, 3000, 10, 100000L, false, TestName = "first 300")]
        public void FibonacciSequenceGeneration_TimeLimitationException(int first, int last, int time, long memory, bool skipCach = false)
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