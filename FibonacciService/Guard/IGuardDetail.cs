using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciService.Guard
{
    public interface IGuardDetail
    {
        Dictionary<string, object> GetDetails();
    }
}
