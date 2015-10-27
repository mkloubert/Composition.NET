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
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.ServiceLocation
{
    /// <summary>
    /// A basic object that locates service instances.
    /// </summary>
    public abstract partial class ServiceLocatorBase : IServiceLocator
    {
        #region Fields (1)

        /// <summary>
        /// Stores the object for thread safe operations.
        /// </summary>
        protected readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="ServiceLocatorBase._SYNC_ROOT" /> field.</param>
        protected ServiceLocatorBase(object syncRoot = null)
        {
            this._SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Methods (6)

        /// <inheriteddoc />
        public IEnumerable<TService> GetAllInstances<TService>(object key = null)
        {
            return this.GetAllInstances(typeof(TService), key)
                       .Cast<TService>();
        }

        /// <inheriteddoc />
        public IEnumerable<object> GetAllInstances(Type serviceType, object key = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            IEnumerable<object> result = null;

            ServiceActivationException exceptionToThrow = null;
            try
            {
                result = this.OnGetAllInstances(serviceType, key);

                if (result == null)
                {
                    exceptionToThrow = new ServiceActivationException(serviceType,
                                                                      key);
                }
            }
            catch (Exception ex)
            {
                exceptionToThrow = new ServiceActivationException(serviceType,
                                                                  key,
                                                                  ex);
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }

            using (var e = result.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    var obj = e.Current;
                    if (obj != null)
                    {
                        yield return obj;
                    }
                }
            }
        }

        /// <inheriteddoc />
        public TService GetInstance<TService>(object key = null)
        {
            return (TService)this.GetInstance(typeof(TService),
                                              key);
        }

        /// <inheriteddoc />
        public object GetInstance(Type serviceType, object key = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            object result = null;

            ServiceActivationException exceptionToThrow = null;
            try
            {
                result = this.OnGetInstance(serviceType, key);

                if (result == null)
                {
                    exceptionToThrow = new ServiceActivationException(serviceType, key);
                }
            }
            catch (Exception ex)
            {
                exceptionToThrow = ex as ServiceActivationException;

                if (exceptionToThrow == null)
                {
                    exceptionToThrow = new ServiceActivationException(serviceType, key,
                                                                      ex);
                }
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }

            return result;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            try
            {
                return this.GetInstance(serviceType);
            }
            catch (ServiceActivationException sae)
            {
                var innerEx = sae.InnerException;
                if (innerEx != null)
                {
                    throw innerEx;
                }

                return null;
            }
        }

        /// <summary>
        /// Stores the logic for the <see cref="ServiceLocatorBase.GetAllInstances(Type, object)" /> method.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">
        /// The key of the service.
        /// <see langword="null" /> indicates to locate the default service.
        /// </param>
        /// <returns>The list of service instances.</returns>
        /// <exception cref="ServiceActivationException">
        /// Error while locating service instance, e.g. not found.
        /// </exception>
        protected abstract IEnumerable<object> OnGetAllInstances(Type serviceType, object key);

        /// <summary>
        /// Stores the logic for the <see cref="ServiceLocatorBase.GetInstance(Type, object)" /> method.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">
        /// The key of the service.
        /// <see langword="null" /> indicates to locate the default service.
        /// </param>
        /// <returns>The located instance.</returns>
        /// <exception cref="ServiceActivationException">
        /// Error while locating service instance, e.g. not found.
        /// </exception>
        protected abstract object OnGetInstance(Type serviceType, object key);

        #endregion Methods (6)
    }
}