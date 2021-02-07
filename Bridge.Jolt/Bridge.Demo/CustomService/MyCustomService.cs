using System;

namespace Jolt.Demo.CustomService
{
    public class MyCustomService: IMyCustomService
    {
        public MyCustomService(IOtherService other)
        {
            this.other = other ?? throw new ArgumentNullException(nameof(other));
        }

        readonly IOtherService other;
    }
}
