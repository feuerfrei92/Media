using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMediaClient.Models;

namespace WebMediaClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

		private void Application_BeginRequest(Object source, EventArgs e)
		{
			HttpApplication application = (HttpApplication)source;
			HttpContext context = application.Context;

			string culture = null;
			if (context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
			{
				culture = Request.UserLanguages[0];
				Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
				Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
			}
		}
    }
}
