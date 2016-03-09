﻿using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebMediaClient.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[AllowAnonymous]
		public async Task<ActionResult> SuccessfulLogin()
		{
			try
			{
				string token = TempData["token"].ToString();
				string url = "http://localhost:8080/api/User/GetCurrentUser";
				UserModel user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				ViewBag.Message = string.Format("Welcome, {0}. Redirecting you to home page...", user.Username);
				ViewBag.Token = token;
				HttpContext.Session.Add("currentUser", user);
				return View();
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			try
			{
				HttpContext.Session.Remove("currentUser");
				return View();
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public ActionResult SetUser(UserModel user)
		{
			try
			{
				HttpContext.Session.Add("currentUser", user);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}
	}
}