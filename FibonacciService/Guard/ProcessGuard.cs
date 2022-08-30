using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FibonacciService.Guard
{
    public class ProcessGuard : IGuard
    {
        private Func<bool> _canProcess;

        public ProcessGuard(Func<bool> canProcess)
        {
            _canProcess = canProcess;
        }

        public bool IsValid()
        {
            return _canProcess();
        }
    }
}
