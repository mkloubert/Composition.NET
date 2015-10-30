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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MarcelJoachimKloubert.Composition
{
    /// <summary>
    /// A MEF helper class for composing a single instance.
    /// </summary>
    /// <typeparam name="T">Type of the object to compose.</typeparam>
    public sealed class SingleInstanceComposer<T>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleInstanceComposer{T}" /> class.
        /// </summary>
        /// <param name="container">The value for the <see cref="SingleInstanceComposer{T}.Container" /> property.</param>
        /// <param name="doRefresh">
        /// Do an initial call of <see cref="SingleInstanceComposer{T}.Refresh()" /> method or not.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> is <see langword="null" />.
        /// </exception>
        public SingleInstanceComposer(CompositionContainer container,
                                      bool doRefresh = true)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;

            if (doRefresh)
            {
                this.Refresh();
            }
        }

        #endregion Constructors (1)

        #region Properties (2)

        /// <summary>
        /// Gets the underlying <see cref="CompositionContainer" /> instance.
        /// </summary>
        public CompositionContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last created instance by <see cref="SingleInstanceComposer{T}.Container" />.
        /// </summary>
        [Import(AllowRecomposition = true)]
        public T Instance
        {
            get;
            private set;
        }

        #endregion Properties (2)

        #region Methods (1)

        /// <summary>
        /// Refreshes the instance in <see cref="SingleInstanceComposer{T}.Instance" /> property.
        /// </summary>
        public void Refresh()
        {
            this.Container
                .ComposeParts(this);
        }

        #endregion Methods (1)
    }
}