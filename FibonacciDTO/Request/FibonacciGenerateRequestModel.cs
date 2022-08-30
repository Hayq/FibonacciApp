using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FibonacciDTO.Request
{
    public class FibonacciGenerateRequestModel
    {
        [Required]
        public uint? FirstIndex { get; set; }

        [Required]
        public uint? LastIndex { get; set; }

        [Required]
        [Description("Execution limitation in milliseconds")]
        public uint? TimeLimit { get; set; }

        [Required]
        [Description("Execution limitation in memory")]
        public uint? MemoryLimit { get; set; }

        public bool SkipCache { get; set; }
    }
}
