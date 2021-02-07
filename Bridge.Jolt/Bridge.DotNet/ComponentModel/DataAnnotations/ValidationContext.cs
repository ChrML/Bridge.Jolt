using System;
using System.Collections.Generic;
using System.Globalization;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Describes the context in which a validation is being performed.
    /// </summary>
    /// <remarks>
    /// This class contains information describing the instance on which validation is being performed.
    /// <para>
    /// It supports <see cref="IServiceProvider"/> so that custom validation code can acquire additional services to help it perform its validation.
    /// </para>
    /// <para>
    /// An <see cref="Items"/> property bag is available for additional contextual information about the validation.  Values stored in <see cref="Items"/>
    /// will be available to validation methods that use this <see cref="ValidationContext"/>
    /// </para>
    /// </remarks>
    public sealed class ValidationContext : IServiceProviderPoly
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext"/> class for a given object instance being validated.
        /// </summary>
        /// <param name="instance">The object instance being validated. It cannot be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="instance"/> is <c>null</c></exception>
        public ValidationContext(object instance): this(instance, null, null)
        {
        }

        /// <summary>
        /// Construct a <see cref="ValidationContext"/> for a given object instance and an optional property bag of <paramref name="items"/>.
        /// </summary>
        /// <param name="instance">The object instance being validated. It cannot be null.</param>
        /// <param name="items">Optional set of key/value pairs to make available to consumers via <see cref="Items"/>.
        /// If null, an empty dictionary will be created.  If not null, the set of key/value pairs will be copied into a new dictionary,
        /// preventing consumers from modifying the original dictionary.
        /// </param>
        /// <exception cref="ArgumentNullException">When <paramref name="instance"/> is <c>null</c></exception>
        public ValidationContext(object instance, IDictionary<object, object> items): this(instance, null, items)
        {
        }

        /// <summary>
        /// Construct a <see cref="ValidationContext"/> for a given object instance, an optional <paramref name="serviceProvider"/>, and an optional
        /// property bag of <paramref name="items"/>.
        /// </summary>
        /// <param name="instance">The object instance being validated.  It cannot be null.</param>
        /// <param name="serviceProvider">
        /// Optional <see cref="BridgeSystem.IServiceProvider"/> to use when <see cref="GetService"/> is called.
        /// <para>
        /// If the <paramref name="serviceProvider"/> specified implements <see cref="Design.IServiceContainer"/>,
        /// then it will be used as the <see cref="ServiceContainer"/> but its services can still be retrieved
        /// through <see cref="GetService"/> as well.
        /// </para>
        /// </param>
        /// <param name="items">Optional set of key/value pairs to make available to consumers via <see cref="Items"/>.
        /// If null, an empty dictionary will be created.  If not null, the set of key/value pairs will be copied into a
        /// new dictionary, preventing consumers from modifying the original dictionary.
        /// </param>
        /// <exception cref="ArgumentNullException">When <paramref name="instance"/> is <c>null</c></exception>
        public ValidationContext(object instance, IServiceProviderPoly serviceProvider, IDictionary<object, object> items)
        {
            // Check sanity.
#pragma warning disable IDE0016 // Use 'throw' expression
            if (instance == null) throw new ArgumentNullException(nameof(instance));
#pragma warning restore IDE0016 // Use 'throw' expression

            if (serviceProvider != null)
            {
                this.InitializeServiceProvider(serviceType => serviceProvider.GetService(serviceType));
            }


            if (serviceProvider is System.ComponentModel.Design.IServiceContainer container)
            {
                this._serviceContainer = new ValidationContextServiceContainer(container);
            }
            else
            {
                this._serviceContainer = new ValidationContextServiceContainer();
            }

            if (items != null)
            {
                this._items = new Dictionary<object, object>(items);
            }
            else
            {
                this._items = new Dictionary<object, object>();
            }

            this.ObjectInstance = instance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user-visible name of the type or property being validated.
        /// </summary>
        /// <value>If this name was not explicitly set, this property will consult an associated <see cref="DisplayAttribute"/>
        /// to see if can use that instead.  Lacking that, it returns <see cref="MemberName"/>.  The <see cref="ObjectInstance"/>
        /// type name will be used if MemberName has not been set.
        /// </value>
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(this._displayName))
                {
                    this._displayName = this.GetDisplayName();

                    if (String.IsNullOrEmpty(this._displayName))
                    {
                        this._displayName = this.MemberName;

                        if (String.IsNullOrEmpty(this._displayName))
                        {
                            this._displayName = this.ObjectType.Name;
                        }
                    }
                }
                return this._displayName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                this._displayName = value;
            }
        }

        /// <summary>
        /// Gets the dictionary of key/value pairs associated with this context.
        /// </summary>
        /// <value>This property will never be null, but the dictionary may be empty.  Changes made
        /// to items in this dictionary will never affect the original dictionary specified in the constructor.</value>
        public IDictionary<object, object> Items => this._items;

        /// <summary>
        /// Gets or sets the name of the type or property being validated.
        /// </summary>
        /// <value>This name reflects the API name of the member being validated, not a localized name.  It should be set
        /// only for property or parameter contexts.</value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets the object instance being validated.  While it will not be null, the state of the instance is indeterminate
        /// as it might only be partially initialized during validation.
        /// <para>Consume this instance with caution!</para>
        /// </summary>
        /// <remarks>
        /// During validation, especially property-level validation, the object instance might be in an indeterminate state.
        /// For example, the property being validated, as well as other properties on the instance might not have been
        /// updated to their new values.
        /// </remarks>
        public object ObjectInstance { get; }

        /// <summary>
        /// Gets the type of the object being validated.  It will not be null.
        /// </summary>
        public Type ObjectType => this.ObjectInstance.GetType();

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="ValidationContext"/> with a service provider that can return
        /// service instances by <see cref="Type"/> when <see cref="GetService"/> is called.
        /// </summary>
        /// <param name="serviceProvider">
        /// A <see cref="Func{T, TResult}"/> that can return service instances given the
        /// desired <see cref="Type"/> when <see cref="GetService"/> is called.
        /// If it is <c>null</c>, <see cref="GetService"/> will always return <c>null</c>.
        /// </param>
        public void InitializeServiceProvider(Func<Type, object> serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Looks up the display name using the DisplayAttribute attached to the respective type or property.
        /// </summary>
        /// <returns>A display-friendly name of the member represented by the <see cref="MemberName"/>.</returns>
        string GetDisplayName()
        {
            // NOTE: Not implemented !!!
            return this.MemberName;
        }

        #endregion

        #region IServiceProvider Members

        /// <summary>
        /// See <see cref="BridgeSystem.IServiceProvider.GetService(Type)"/>.
        /// When the <see cref="ServiceContainer"/> is in use, it will be used
        /// first to retrieve the requested service.  If the <see cref="ServiceContainer"/>
        /// is not being used or it cannot resolve the service, then the
        /// <see cref="BridgeSystem.IServiceProvider"/> provided to this <see cref="ValidationContext"/>
        /// will be queried for the service type.
        /// </summary>
        /// <param name="serviceType">The type of the service needed.</param>
        /// <returns>An instance of that service or null if it is not available.</returns>
        public object GetService(Type serviceType)
        {
            object service = null;

            if (this._serviceContainer != null)
            {
                service = this._serviceContainer.GetService(serviceType);
            }

            if (service == null && this._serviceProvider != null)
            {
                service = this._serviceProvider(serviceType);
            }

            return service;
        }

        #endregion

        #region Service Container

        System.ComponentModel.Design.IServiceContainer _serviceContainer;

        /// <summary>
        /// A <see cref="Design.IServiceContainer"/> that can be used for adding,
        /// removing, and getting services during validation.  <see cref="GetService"/>
        /// will query into this container as well as the <see cref="BridgeSystem.IServiceProvider"/>
        /// specified in the constructor.
        /// </summary>
        /// <remarks>
        /// If the <see cref="BridgeSystem.IServiceProvider"/> specified to the constructor implements
        /// <see cref="Design.IServiceContainer"/>, then it will be used as the
        /// <see cref="ServiceContainer"/>, otherwise an empty container will be initialized.
        /// </remarks>
        public System.ComponentModel.Design.IServiceContainer ServiceContainer
        {
            get
            {
                if (this._serviceContainer == null)
                {
                    this._serviceContainer = new ValidationContextServiceContainer();
                }
                return this._serviceContainer;
            }
        }

        /// <summary>
        /// Private implementation of <see cref="Design.IServiceContainer"/> to act as a default service container on <see cref="ValidationContext"/>.
        /// </summary>
        class ValidationContextServiceContainer : System.ComponentModel.Design.IServiceContainer
        {
            #region Member Fields

            readonly System.ComponentModel.Design.IServiceContainer _parentContainer;
            readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
            readonly object _lock = new object();

            #endregion

            #region Constructors

            /// <summary>
            /// Constructs a new service container that does not have a parent container
            /// </summary>
            internal ValidationContextServiceContainer()
            {
            }

            /// <summary>
            /// Contstructs a new service container that has a parent container, making this container
            /// a wrapper around the parent container.  Calls to <c>AddService</c> and <c>RemoveService</c>
            /// will promote to the parent container by default, unless <paramref name="promote"/> is
            /// specified as <c>false</c> on those calls.
            /// </summary>
            /// <param name="parentContainer">The parent container to wrap into this container.</param>
            internal ValidationContextServiceContainer(System.ComponentModel.Design.IServiceContainer parentContainer)
            {
                this._parentContainer = parentContainer;
            }

            #endregion

            #region IServiceContainer Members

            public void AddService(Type serviceType, System.ComponentModel.Design.ServiceCreatorCallback callback, bool promote)
            {
                if (promote && this._parentContainer != null)
                {
                    this._parentContainer.AddService(serviceType, callback, promote);
                }
                else
                {
                    lock (this._lock)
                    {
                        if (this._services.ContainsKey(serviceType))
                        {
                            throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Item already exists", serviceType), nameof(serviceType));
                        }

                        this._services.Add(serviceType, callback);
                    }
                }
            }

            public void AddService(Type serviceType, System.ComponentModel.Design.ServiceCreatorCallback callback)
            {
                this.AddService(serviceType, callback, true);
            }

            public void AddService(Type serviceType, object serviceInstance, bool promote)
            {
                if (promote && this._parentContainer != null)
                {
                    this._parentContainer.AddService(serviceType, serviceInstance, promote);
                }
                else
                {
                    lock (this._lock)
                    {
                        if (this._services.ContainsKey(serviceType))
                        {
                            throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Item already exists", serviceType), nameof(serviceType));
                        }

                        this._services.Add(serviceType, serviceInstance);
                    }
                }
            }

            public void AddService(Type serviceType, object serviceInstance)
            {
                this.AddService(serviceType, serviceInstance, true);
            }

            public void RemoveService(Type serviceType, bool promote)
            {
                lock (this._lock)
                {
                    if (this._services.ContainsKey(serviceType))
                    {
                        this._services.Remove(serviceType);
                    }
                }

                if (promote && this._parentContainer != null)
                {
                    this._parentContainer.RemoveService(serviceType);
                }
            }

            public void RemoveService(Type serviceType)
            {
                this.RemoveService(serviceType, true);
            }

            #endregion

            #region IServiceProvider Members

            public object GetService(Type serviceType)
            {
                if (serviceType == null)
                {
                    throw new ArgumentNullException("serviceType");
                }

                this._services.TryGetValue(serviceType, out object service);

                if (service == null && this._parentContainer != null)
                {
                    service = this._parentContainer.GetService(serviceType);
                }


                if (service is System.ComponentModel.Design.ServiceCreatorCallback callback)
                {
                    service = callback(this, serviceType);
                }

                return service;
            }

            #endregion
        }

        #endregion

        #region Private

        Func<Type, object> _serviceProvider;
        string _displayName;
        readonly Dictionary<object, object> _items;

        #endregion
    }
}
