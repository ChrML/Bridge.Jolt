namespace System.Reflection
{
    /// <summary>
    /// Provides extension features for the Bridge.NET <see cref="PropertyInfo"/> class to bring parity with native .NET.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Returns the set accessor for this property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public. <c>true</c> if a non-public accessor is to be returned; otherwise, <c>false</c>.</param>
        /// <returns>This property's Set method, or <c>null</c>.</returns>
#pragma warning disable IDE0060 // Remove unused parameter
        public static MethodInfo GetSetMethod(this PropertyInfo propertyInfo, bool nonPublic)
        {
            // NOTE: nonPublic is not implemented.
            return propertyInfo.SetMethod;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
