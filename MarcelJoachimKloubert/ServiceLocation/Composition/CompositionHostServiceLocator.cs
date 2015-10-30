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
using System.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MarcelJoachimKloubert.ServiceLocation.Composition
{
    /// <summary>
    /// A service locator based on a <see cref="CompositionHost" />.
    /// </summary>
    public class CompositionHostServiceLocator : ServiceLocatorBase
    {
        #region Fields (1)

        /// <summary>
        /// Stores the underlying <see cref="CompositionHost" />.
        /// </summary>
        protected readonly CompositionHost _HOST;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositionHostServiceLocator" /> class.
        /// </summary>
        /// <param name="host">The underlying <see cref="CompositionHost" />.</param>
        /// <param name="syncRoot">The value for the <see cref="ServiceLocatorBase._SYNC_ROOT" /> field.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="host" /> is <see langword="null" />.
        /// </exception>
        public CompositionHostServiceLocator(CompositionHost host, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }

            this._HOST = host;
        }

        #endregion Constructors (1)

        #region Methods (4)

        private static string AsString(object obj)
        {
            if (obj is string)
            {
                return (string)obj;
            }

            if (DBNull.Value.Equals(obj))
            {
                obj = null;
            }

            if (obj == null)
            {
                return null;
            }

            return obj.ToString();
        }

        private static TResult InvokeGetExportedValueMethod<TResult>(Expression<Func<CompositionHost, TResult>> expr,
                                                                     CompositionHost host,
                                                                     Type serviceType, string key)
        {
            var method = (expr.Body as MethodCallExpression).Method;
            var methodName = method.Name;

            var @params = new object[0];
            if (key != null)
            {
                // with key
                @params = new object[] { key };
            }

            return (TResult)host.GetType()
                                .GetMethods()
                                .First(m =>
                                       {
                                           if (m.Name != methodName)
                                           {
                                               return false;
                                           }

                                           if (!m.IsGenericMethod)
                                           {
                                               return false;
                                           }

                                           return m.GetParameters().Length == @params.Length;
                                       }).MakeGenericMethod(serviceType)
                                         .Invoke(obj: host,
                                                 parameters: @params.Length < 1 ? null : @params);
        }

        /// <inheriteddoc />
        protected override IEnumerable<object> OnGetAllInstances(Type serviceType, object key)
        {
            return InvokeGetExportedValueMethod(h => h.GetExports<object>(),
                                                this._HOST,
                                                serviceType, AsString(key));
        }

        /// <inheriteddoc />
        protected override object OnGetInstance(Type serviceType, object key)
        {
            return InvokeGetExportedValueMethod(h => h.GetExport<object>(),
                                                this._HOST,
                                                serviceType, AsString(key));
        }

        #endregion Methods (4)
    }
}