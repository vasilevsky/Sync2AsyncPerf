using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using API.Utils;
using Domain;
using Microsoft.Owin;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Syntax;
using Owin;

namespace API
{
    public class Startup
    {
        public static IKernel Container { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            SetupContainer(config);

            config.Services.Add(typeof(IExceptionLogger), new EventLogExceptionLogger());

            app.UseWebApi(config);
        }

        private void SetupContainer(HttpConfiguration config)
        {
            Container = new StandardKernel();
            Container.Bind(m => m.FromAssemblyContaining<Startup>().SelectAllClasses().BindAllInterfaces());
            Container.Bind(m => m.FromAssemblyContaining<Customer>().SelectAllClasses().BindAllInterfaces());

            config.DependencyResolver = new NinjectDependencyResolver(Container);
        }

        /// <summary>
        /// Ninject as DependencyResolver.
        /// </summary>
        private class NinjectDependencyResolver : IDependencyResolver
        {
            private IResolutionRoot Container;

            public NinjectDependencyResolver(IKernel container)
            {
                Container = container;
            }

            private NinjectDependencyResolver(IResolutionRoot container)
            {
                Container = container;
            }

            public object GetService(Type serviceType)
            {
                return Container.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Container.GetAll(serviceType);
            }

            public IDependencyScope BeginScope()
            {
                if (!(Container is IKernel))
                {
                    throw new NotImplementedException("Can't begin a scope on a scope");
                }

                return new NinjectDependencyResolver(((IKernel)Container).BeginBlock());
            }

            public void Dispose()
            {
            }
        }
    }
}
