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

namespace MarcelJoachimKloubert.ServiceLocation
{
    /// <summary>
    /// Describes an object for locating service objects.
    /// </summary>
    public interface IServiceLocator : IServiceProvider
    {
        #region Methods (4)

        /// <summary>
        /// Gets all instances of a service.
        /// </summary>
        /// <typeparam name="TService">Type of the service.</typeparam>
        /// <param name="key">
        /// Key of the service.
        /// <see langword="null" /> indicates to get the default service.
        /// </param>
        /// <returns>All instances of the service.</returns>
        IEnumerable<TService> GetAllInstances<TService>(object key = null);

        /// <summary>
        /// Gets all instances of a service.
        /// </summary>
        /// <param name="serviceType">Typ des Dienstes.</param>
        /// <param name="key">
        /// Key of the service.
        /// <see langword="null" /> indicates to get the default service.
        /// </param>
        /// <returns>All instances of the service.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        IEnumerable<object> GetAllInstances(Type serviceType, object key = null);

        /// <summary>
        /// Gets a single instance of a specific service.
        /// </summary>
        /// <typeparam name="TService">Type of the service.</typeparam>
        /// <param name="key">
        /// Key of the service.
        /// <see langword="null" /> indicates to get the default service.
        /// </param>
        /// <returns>The instance of the service.</returns>
        /// <exception cref="ServiceActivationException">
        /// Error while locating service instance, e.g. not found.
        /// </exception>
        TService GetInstance<TService>(object key = null);

        /// <summary>
        /// Gets a single instance of a specific service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">
        /// Key of the service.
        /// <see langword="null" /> indicates to get the default service.
        /// </param>
        /// <returns>The instance of the service.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ServiceActivationException">
        /// Error while locating service instance, e.g. not found.
        /// </exception>
        object GetInstance(Type serviceType, object key = null);

        #endregion Methods (4)
    }
}