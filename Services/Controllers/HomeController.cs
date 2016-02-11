using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Services.Controllers
{
	public class HomeController : ApiController
	{
		[System.Web.Http.HttpPost]
		public IHttpActionResult Index()
		{
			return Ok();
		}
	}
}
