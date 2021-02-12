using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt
{
    /// <summary>
    /// Indicates that an element in the DOM should be notified whenever it's mounted or about to be unmounted from the DOM.
    /// </summary>
    public interface ILifecycle
    {
        /// <summary>
        /// Called by the framework when this control was mounted to the DOM- tree.
        /// </summary>
        void Mounted();

        /// <summary>
        /// Called by the framework when this control was unmounted from the DOM- tree.
        /// </summary>
        void Unmounted();
    }
}
