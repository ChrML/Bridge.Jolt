using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Demo.CustomService
{
    public class MyCustomService: IMyCustomService
    {
        private readonly IOtherService other;

        public MyCustomService(IOtherService other)
        {
            this.other = other ?? throw new ArgumentNullException(nameof(other));
        }

    }
}
