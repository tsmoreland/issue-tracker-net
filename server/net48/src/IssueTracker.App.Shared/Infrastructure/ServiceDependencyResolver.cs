//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.App.Shared.Infrastructure
{
    public sealed class ServiceDependencyResolver : IDependencyResolver, Microsoft.Practices.ServiceLocation.IServiceLocator
    {

        /// <inheritdoc cref="IDependencyResolver.GetService(Type)" />
        public object GetService(Type serviceType)
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            return serviceProvider.GetService(serviceType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType) 
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            return serviceProvider.GetServices(serviceType);
        }

        /// <inheritdoc />
        public IDependencyScope BeginScope() 
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            IServiceScope scope = serviceProvider.CreateScope();
            return new DepedencyScopeFacade(scope);
        }


        /// <inheritdoc />
        public object GetInstance(Type serviceType)
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            object @object = serviceProvider.GetRequiredService(serviceType);
            return @object;
        }

        /// <inheritdoc />
        public object GetInstance(Type serviceType, string key)
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            // ignoring key until we need it, they'll need to work something else out
            return serviceProvider.GetRequiredService(serviceType); 
        }

        /// <inheritdoc />
        public TService GetInstance<TService>()
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            return serviceProvider.GetRequiredService<TService>(); 
        }

        /// <inheritdoc />
        public TService GetInstance<TService>(string key)
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            // ignoring key until we need it, they'll need to work something else out
            return serviceProvider.GetRequiredService<TService>(); 
        }


        /// <inheritdoc />
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            return serviceProvider.GetServices(serviceType).Cast<object>();
        }

        /// <inheritdoc />
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            return serviceProvider.GetServices<TService>();
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceScope scope = (IServiceScope)HttpContext.Current?.Items[typeof(IServiceScope)];
            return scope?.ServiceProvider;
        }

        private sealed class DepedencyScopeFacade : IDependencyScope
        {
            private readonly IServiceScope _scope;

            public DepedencyScopeFacade(IServiceScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            /// <inheritdoc />
            public object GetService(Type serviceType) => _scope.ServiceProvider.GetService(serviceType);

            /// <inheritdoc />
            public IEnumerable<object> GetServices(Type serviceType) => _scope.ServiceProvider.GetServices(serviceType);

            /// <inheritdoc/>
            public void Dispose() => _scope.Dispose();

        }


        /// <inheritdoc />
        public void Dispose()
        {
            // no-op
        }

    }
}
