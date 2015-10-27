/**********************************************************************************************************************
 * Composition.NET (https://github.com/mkloubert/Composition.NET)                                                     *
 *                                                                                                                    *
 * Copyright (c) 2015, Marcel Joachim Kloubert <marcel.kloubert@gmx.net>                                              *
 * All rights reserved.                                                                                               *
 *                                                                                                                    *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the   *
 * following conditions are met:                                                                                      *
 *                                                                                                                    *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the          *
 *    following disclaimer.                                                                                           *
 *                                                                                                                    *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the       *
 *    following disclaimer in the documentation and/or other materials provided with the distribution.                *
 *                                                                                                                    *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote    *
 *    products derived from this software without specific prior written permission.                                  *
 *                                                                                                                    *
 *                                                                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, *
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  *
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, *
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR    *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,  *
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE   *
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                           *
 *                                                                                                                    *
 **********************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.ServiceLocation
{
    /// <summary>
    /// A service locator based on delegates and additionally uses fallbacks (if defined).
    /// </summary>
    public sealed partial class DelegateServiceLocator : ServiceLocatorBase
    {
        #region Fields (2)

        private readonly Dictionary<Type, InstanceProvider> _MULTI_PROVIDERS = new Dictionary<Type, InstanceProvider>();
        private readonly Dictionary<Type, InstanceProvider> _SINGLE_PROVIDERS = new Dictionary<Type, InstanceProvider>();

        #endregion Fields (2)

        #region Constructors (1)

        /// /// <summary>
        /// Initializes a new instance of the <see cref="DelegateServiceLocator" /> class.
        /// </summary>
        /// <param name="baseLocator">The value for <see cref="DelegateServiceLocator.BaseLocator" /> property.</param>
        /// <param name="multiFallback">The value for <see cref="DelegateServiceLocator.MultiInstanceFallback" /> property.</param>
        /// <param name="singleFallback">The value for <see cref="DelegateServiceLocator.SingleInstanceFallback" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="ServiceLocatorBase._SYNC_ROOT" /> field.</param>
        public DelegateServiceLocator(IServiceLocator baseLocator = null,
                                      MultiInstanceFallbackProvider multiFallback = null, SingleInstanceFallbackProvider singleFallback = null,
                                      object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            this.BaseLocator = baseLocator;

            this.MultiInstanceFallback = multiFallback;
            this.SingleInstanceFallback = singleFallback;
        }

        #endregion Constructors (1)

        #region Properties (3)

        /// <summary>
        /// Gets the base service locator, if defined.
        /// </summary>
        public IServiceLocator BaseLocator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the provider that is invoked if that object is not able
        /// to locate instances for a specific service type.
        /// </summary>
        public MultiInstanceFallbackProvider MultiInstanceFallback
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the provider that is invoked if that object is not able
        /// to locate an instance for a specific service type.
        /// </summary>
        public SingleInstanceFallbackProvider SingleInstanceFallback
        {
            get;
            private set;
        }

        #endregion Properties (3)

        #region Delegates and Events (4)

        /// <summary>
        /// Describes a function / method that provides a list of instances of a service.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <param name="locator">The underlying locator.</param>
        /// <param name="key">The used key.</param>
        /// <returns>The list of instances or <see langword="null" /> to throw a <see cref="ServiceActivationException" />.</returns>
        public delegate IEnumerable<T> MultiInstanceProvider<T>(DelegateServiceLocator locator, object key);

        /// <summary>
        /// Describes a function / method that provides a list of instances of a service
        /// if that object itself is not able to locate instances for the defined service type.
        /// </summary>
        /// <param name="locator">The underlying locator.</param>
        /// <param name="key">The used key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of instances or <see langword="null" /> to throw a <see cref="ServiceActivationException" />.</returns>
        public delegate IEnumerable<object> MultiInstanceFallbackProvider(DelegateServiceLocator locator, object key, Type serviceType);

        /// <summary>
        /// Describes a function / method that provides a single instance of a service.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <param name="locator">The underlying locator.</param>
        /// <param name="key">The used key.</param>
        /// <returns>The list of instances or <see langword="null" /> to throw a <see cref="ServiceActivationException" />.</returns>
        public delegate T SingleInstanceProvider<T>(DelegateServiceLocator locator, object key);

        /// <summary>
        /// Describes a fallback function / method that provides a single instance of a service
        /// if that object itself is not able to locate an instance for the defined service type.
        /// </summary>
        /// <param name="locator">The underlying locator.</param>
        /// <param name="key">The used key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of instances or <see langword="null" /> to throw a <see cref="ServiceActivationException" />.</returns>
        public delegate object SingleInstanceFallbackProvider(DelegateServiceLocator locator, object key, Type serviceType);

        #endregion Delegates and Events (4)

        #region Methods (6)

        private static SingleInstanceProvider<T> MultiToSingle<T>(MultiInstanceProvider<T> provider)
        {
            return ((locator, key) =>
                {
                    var serviceType = typeof(T);

                    var result = provider(locator, key);
                    if (result == null)
                    {
                        throw new ServiceActivationException(serviceType, key);
                    }

                    try
                    {
                        return result.Single();
                    }
                    catch (Exception ex)
                    {
                        throw new ServiceActivationException(serviceType, key, ex);
                    }
                });
        }

        /// <inheriteddoc />
        protected override IEnumerable<object> OnGetAllInstances(Type serviceType, object key)
        {
            IEnumerable<object> result = null;

            InstanceProvider provider;
            lock (this._SYNC_ROOT)
            {
                if (this._MULTI_PROVIDERS.TryGetValue(serviceType, out provider))
                {
                    var seq = provider.Invoke<IEnumerable>(this, key);

                    if (seq != null)
                    {
                        result = seq.Cast<object>();
                    }

                    if (result == null)
                    {
                        throw new ServiceActivationException(serviceType, key);
                    }
                }
            }

            var tryFallback = true;

            if (result == null)
            {
                if (this.BaseLocator != null)
                {
                    // use base service locator instead

                    tryFallback = false;
                    result = this.BaseLocator
                                 .GetAllInstances(serviceType, key);
                }
            }

            if (tryFallback)
            {
                // try by fallback, if defined

                if (this.MultiInstanceFallback != null)
                {
                    result = this.MultiInstanceFallback(this, key, serviceType);
                }
            }

            return result;
        }

        /// <inheriteddoc />
        protected override object OnGetInstance(Type serviceType, object key)
        {
            object result = null;

            InstanceProvider provider;
            lock (this._SYNC_ROOT)
            {
                if (this._SINGLE_PROVIDERS.TryGetValue(serviceType, out provider))
                {
                    result = provider.Invoke<object>(this, key);

                    if (result == null)
                    {
                        throw new ServiceActivationException(serviceType, key);
                    }
                }
            }

            var tryFallback = true;

            if (result == null)
            {
                if (this.BaseLocator != null)
                {
                    // use base service locator instead

                    tryFallback = false;
                    result = this.BaseLocator
                                 .GetInstance(serviceType, key);
                }
            }

            if (tryFallback)
            {
                // try by fallback, if defined

                if (this.SingleInstanceFallback != null)
                {
                    result = this.SingleInstanceFallback(this, key, serviceType);
                }
            }

            return result;
        }

        /// <summary>
        /// Registers a provider for resolving a list of instances of a service.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <param name="provider">The provider to register.</param>
        /// <param name="registerDefaultSingleProvider">
        /// Also register a default single instance provider or not.
        /// </param>
        /// <returns>That instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A provider is already registrated for the service type.
        /// </exception>
        public DelegateServiceLocator RegisterMultiProvider<T>(MultiInstanceProvider<T> provider,
                                                               bool registerDefaultSingleProvider = true)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            lock (this._SYNC_ROOT)
            {
                var serviceType = typeof(T);

                if (this._MULTI_PROVIDERS.ContainsKey(serviceType))
                {
                    throw new InvalidOperationException();
                }

                this._MULTI_PROVIDERS
                    .Add(serviceType,
                         new InstanceProvider(serviceType, provider));
            }

            if (registerDefaultSingleProvider)
            {
                this.RegisterSingleProvider<T>(MultiToSingle<T>(provider),
                                               false);
            }

            return this;
        }

        /// <summary>
        /// Registers a provider for resolving a single instance of a service.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <param name="provider">The provider to register.</param>
        /// <param name="registerDefaultMultiProvider">
        /// Also register a default multi instance provider or not.
        /// </param>
        /// <returns>That instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A provider is already registrated for the service type.
        /// </exception>
        public DelegateServiceLocator RegisterSingleProvider<T>(SingleInstanceProvider<T> provider,
                                                                bool registerDefaultMultiProvider = true)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            lock (this._SYNC_ROOT)
            {
                var serviceType = typeof(T);

                if (this._SINGLE_PROVIDERS.ContainsKey(serviceType))
                {
                    throw new InvalidOperationException();
                }

                this._SINGLE_PROVIDERS
                    .Add(serviceType,
                         new InstanceProvider(serviceType, provider));
            }

            if (registerDefaultMultiProvider)
            {
                this.RegisterMultiProvider<T>(SingleToMulti<T>(provider),
                                              false);
            }

            return this;
        }

        private static MultiInstanceProvider<T> SingleToMulti<T>(SingleInstanceProvider<T> provider)
        {
            return (locator, key) =>
                {
                    try
                    {
                        var instance = provider(locator, key);
                        if (instance != null)
                        {
                            return new T[] { instance };
                        }
                    }
                    catch (ServiceActivationException saex)
                    {
                        if (saex.InnerException != null)
                        {
                            throw;
                        }
                    }

                    return Enumerable.Empty<T>();
                };
        }

        #endregion Methods (6)
    }
}