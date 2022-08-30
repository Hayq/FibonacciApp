using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FibonacciDTO.Request
{
    public class FibonacciGenerateRequestModel
    {
        [Required]
        public int? FirstIndex { get; set; }

        [Required]
        public int? LastIndex { get; set; }

        [Required]
        [Description("Time execution limitation in milliseconds")]
        public int? TimeLimit { get; set; }

        [Required]
        [Description("Memory execution limitation in bytes")]
        public long? MemoryLimit { get; set; }

        public bool SkipCache { get; set; }
    }
}
