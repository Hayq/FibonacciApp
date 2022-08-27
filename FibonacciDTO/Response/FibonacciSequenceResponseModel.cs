using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciDTO.Response
{
    public class FibonacciSequenceResponseModel : ResponseModelBase
    {
        public ulong[] Sequence { get; set; }
    }
}
