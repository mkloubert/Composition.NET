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
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MarcelJoachimKloubert.ServiceLocation
{
    /// <summary>
    /// Is thrown if a service could be be located by a <see cref="IServiceLocator" /> object.
    /// </summary>
    [DebuggerDisplay("ServiceActivationException = {ServiceType}; {Key}")]
    public class ServiceActivationException : Exception
    {
        #region Fields (1)

        /// <summary>
        /// Stores the default message of that exception.
        /// </summary>
        public const string DEFAULT_EXCEPTION_MESSAGE = "Could be locate service by given type and key!";

        #endregion Fields (1)

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceActivationException" /> class.
        /// </summary>
        /// <param name="serviceType">Value for <see cref="ServiceActivationException.ServiceType" /> property.</param>
        /// <param name="key">Value for <see cref="ServiceActivationException.Key" /> property.</param>
        /// <param name="innerException">The optional inner exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        public ServiceActivationException(Type serviceType, object key,
                                          Exception innerException)
            : base(DEFAULT_EXCEPTION_MESSAGE,
                   innerException)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.ServiceType = serviceType;
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceActivationException" /> class.
        /// </summary>
        /// <param name="serviceType">Value for <see cref="ServiceActivationException.ServiceType" /> property.</param>
        /// <param name="key">Value for <see cref="ServiceActivationException.Key" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        public ServiceActivationException(Type serviceType, object key)
            : base(DEFAULT_EXCEPTION_MESSAGE)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.ServiceType = serviceType;
            this.Key = key;
        }

        /// <inheriteddoc />
        protected ServiceActivationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors (2)

        #region Properties (2)

        /// <summary>
        /// Gets the service key.
        /// </summary>
        public object Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the underlying service.
        /// </summary>
        public Type ServiceType
        {
            get;
            private set;
        }

        #endregion Properties (2)
    }
}