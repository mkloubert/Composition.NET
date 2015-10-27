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

namespace MarcelJoachimKloubert.ServiceLocation
{
    /// <summary>
    /// Gives access to a global service locator.
    /// </summary>
    public static class ServiceLocator
    {
        #region Fields 81)

        private static LocatorProvider _provider;

        #endregion Fields 81)

        #region Delegates (1)

        /// <summary>
        /// Describes a function that provides a service locator.
        /// </summary>
        /// <returns>The service locator.</returns>
        public delegate IServiceLocator LocatorProvider();

        #endregion Delegates (1)

        #region Properties (1)

        /// <summary>
        /// Gets the global service locator.
        /// </summary>
        public static IServiceLocator Current
        {
            get { return _provider(); }
        }

        #endregion Properties (1)

        #region Methods (2)

        /// <summary>
        /// Sets the global service locator.
        /// </summary>
        /// <param name="locator">The (new) locator.</param>
        public static void SetLocator(IServiceLocator locator)
        {
            SetProvider(locator != null ? new LocatorProvider(() => locator)
                                        : null);
        }

        /// <summary>
        /// Sets the function that provides the value for the <see cref="ServiceLocator.Current" /> property.
        /// </summary>
        /// <param name="provider">The new provider.</param>
        public static void SetProvider(LocatorProvider provider)
        {
            _provider = provider;
        }

        #endregion Methods (2)
    }
}