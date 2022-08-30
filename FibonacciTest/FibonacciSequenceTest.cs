using FibonacciDTO.Request;
using FibonacciService.FibonacciGenerator;
using NUnit.Framework;
using System.Diagnostics;

namespace FibonacciTest
{
    public class FibonacciSequenceTest
    {
        private IFibNumGenerator _fibNumGenerator;

        [OneTimeSetUp]
        public void Setup()
        {
            _fibNumGenerator = new FibNumGenerator();
        }

        [TestCase(1u,   10u,      20u, 100u, true,  TestName = "10")]
        [TestCase(2u,   300u,     20u, 100u, true,  TestName = "100")]
        [TestCase(300u, 3000u,    1u, 100u, true,  TestName = "1000")]
        [TestCase(1u,   10000u,   20u, 100u, true,  TestName = "10000")]
        [TestCase(1u,   100000u,  20000u, 100u, true,  TestName = "100000")]
        [TestCase(1u,   1000000u, 20000u, 100u, true,  TestName = "1000000")]
        public async Task Test1(uint first, uint last, uint time, uint memory, bool skipCach = false)
        {
            var watch = new Stopwatch();
            watch.Start();
            var requestModel = new FibonacciGenerateRequestModel
            {
                FirstIndex = first,
                LastIndex = last,
                TimeLimit = time,
                MemoryLimit = memory,
                SkipCache = skipCach
            };
            var sequence = await _fibNumGenerator.GenerateSubSequence(requestModel);
            watch.Stop();

            Console.WriteLine();
            //ShowThreadPoolState(watch);
            //ShowSequence(sequence);
            //TODO some assert
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