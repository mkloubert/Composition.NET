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

using MarcelJoachimKloubert.ServiceLocation.Composition;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MarcelJoachimKloubert.Composition.Tests
{
    internal interface ITest
    {
    }

    [Export(typeof(ITest))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class Test1 : ITest
    {
        private static int _instances;

        public Test1()
        {
            this.A = _instances++;
        }

        public int A
        {
            get;
            private set;
        }
    }

    [Export(typeof(ITest))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class Test2 : ITest
    {
        private static int _instances;

        public Test2()
        {
            this.A = _instances++;
        }

        public int A
        {
            get;
            private set;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            var container = new CompositionContainer(asmCatalog, isThreadSafe: true);

            var sl = new ExportProviderServiceLocator(container);

            foreach (var t in sl.GetAllInstances<ITest>())
            {
            }

            foreach (var t in sl.GetAllInstances<ITest>())
            {
            }

            Console.ReadLine();
        }
    }
}