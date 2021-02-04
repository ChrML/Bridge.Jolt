using Retyped;

namespace Bridge.Jolt
{
    /// <summary>
    /// Represents any class that can be displayed as a HTML- element by providing its root DOM- element.
    /// </summary>
    public interface IHtmlElement
    {
        /// <summary>
        /// Gets the root DOM- element.
        /// </summary>
        dom.HTMLElement DomElement { get; }
    }
}
