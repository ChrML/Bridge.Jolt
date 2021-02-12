using Bridge;
using Jolt.Abstractions;
using Retyped;
using System;

namespace Jolt.Utilities
{
    /// <summary>
    /// Implements the logic for tracking the DOM- tree for changes that should be notified through the optional <see cref="ILifecycle"/> interface of a control.
    /// </summary>
    /// <remarks>
    /// This class has been carefully examined to be implemented as efficiently as possible. <br/>
    /// Any unneccessary overhead here will directly affect the performance of DOM- tree operations.
    /// </remarks>
    sealed class LifecycleTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LifecycleTracker"/> class.
        /// </summary>
        public LifecycleTracker()
        {
            this.observer = new dom.MutationObserver(this.Callback);
            this.observer.observe(dom.document.body, new dom.MutationObserverInit
            {
                attributes = false,
                childList = true,
                characterData = false,
                subtree = true
            });
        }

        /// <summary>
        /// Callback called for every change to the DOM- tree.
        /// </summary>
        void Callback(dom.MutationRecord[] mutations, dom.MutationObserver observer)
        {
            // Check the input. Avoid Bridge.NET's sanity checks by treating the array as a native JS- object.
            dynamic items = mutations;
            if (items == null)
            {
                return;
            }

            dom.NodeList added, removed;
            dom.MutationRecord item;
            int length = items.length;
            for (int i = 0; i < length; i++)
            {
                item = items[i];
                if (item.type == dom.MutationRecordType.childList)
                {
                    // Call Mounted() for controls added.
                    added = item.addedNodes;
                    if (added != null && added.length > 0)
                    {
                        this.AddedNodes(added);

                    }

                    // Call Unmounted() for controls removed.
                    removed = item.removedNodes;
                    if (removed != null && removed.length > 0)
                    {
                        this.RemovedNodes(removed);
                    }
                }
            }
        }

        /// <summary>
        /// Call Mount() for controls added.
        /// </summary>
        void AddedNodes(dom.NodeList added)
        {
            uint count = added.length;
            for (uint i = 0; i < count; i++)
            {
                dom.Node node = added[i];
                object temp = node["$Jolt$Lifecycle"];
                if (temp != null)
                {
                    // Avoid a very expensive cast for something we know is an ILifecycle.
                    this.NotifyMount(Script.Write<ILifecycle>(nameof(temp)));
                }
            }
        }

        /// <summary>
        /// Call Unmount() for controls added.
        /// </summary>
        void RemovedNodes(dom.NodeList removed)
        {
            uint count = removed.length;
            for (uint i = 0; i < count; i++)
            {
                dom.Node node = removed[i];
                object temp = node["$Jolt$Lifecycle"];
                if (temp != null)
                {
                    // Avoid a very expensive cast for something we know is an ILifecycle.
                    this.NotifyUnmount(Script.Write<ILifecycle>(nameof(temp)));
                }
            }
        }

        /// <summary>
        /// Calls a single Mount() and catches any user-exception that occurs.
        /// </summary>
        void NotifyMount(ILifecycle lifecycle)
        {
            try
            {
                lifecycle.Mounted();
            }
            catch (Exception exception)
            {
                AppServices.Default.Resolve<IErrorHandler>().OnError(exception, "Unhandled exception during control mount.");
            }
        }

        /// <summary>
        /// Calls a single Unmount() and catches any user-exception that occurs.
        /// </summary>
        /// <param name="lifecycle"></param>
        void NotifyUnmount(ILifecycle lifecycle)
        {
            try
            {
                lifecycle.Unmounted();
            }
            catch (Exception exception)
            {
                AppServices.Default.Resolve<IErrorHandler>().OnError(exception, "Unhandled exception during control unmount.");
            }
        }


        readonly dom.MutationObserver observer;
    }
}
