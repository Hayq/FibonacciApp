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

        [TestCase(1,   10,      1000, 100000L, true, TestName = "success 10")]
        [TestCase(2,   300,     1000, 100000L, true, TestName = "success 100")]
        [TestCase(300, 3000,    1500, 100000L, true, TestName = "success 1000")]
        [TestCase(1,   100000, 2000, 100000L, true, TestName = "success 1000000")]
        public async Task Test1(int first, int last, int time, long memory, bool skipCach = false)
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
            var sequence = await _fibNumGenerator.GenerateSubSequence(requestModel);
            _watch.Stop();

            int expected = last - first + 1;
            int actual = sequence.Count();
            Assert.AreEqual(expected, actual);

            //Console.WriteLine();
            //ShowThreadPoolState(watch);
            //ShowSequence(sequence);
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