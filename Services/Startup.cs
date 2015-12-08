using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Ninject;
using System.Reflection;
using System.Web.Http;

//[assembly: OwinStartup(typeof(Services.Startup))]

namespace Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

		//	var webApiConfiguration = new HttpConfiguration();
		//	webApiConfiguration.Routes.MapHttpRoute(
		//		name: "DefaultApi",
		//		routeTemplate: "api/{controller}/{action}/{id}",
		//		defaults: new { id = RouteParameter.Optional });

		//	app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(webApiConfiguration);
		//}

		//private static StandardKernel CreateKernel()
		//{
		//	var kernel = new StandardKernel();
		//	kernel.Load(Assembly.GetExecutingAssembly());
		//	return kernel;
		}
    }
}
