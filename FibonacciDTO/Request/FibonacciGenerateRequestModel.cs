using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FibonacciDTO.Request
{
    public class FibonacciGenerateRequestModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? FirstIndex { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? LastIndex { get; set; }

        [Required]
        [Description("Time execution limitation in milliseconds")]
        public long? TimeLimit { get; set; }

        [Required]
        [Description("Memory execution limitation in bytes")]
        public long? MemoryLimit { get; set; }

        public bool SkipCache { get; set; }
    }
}
