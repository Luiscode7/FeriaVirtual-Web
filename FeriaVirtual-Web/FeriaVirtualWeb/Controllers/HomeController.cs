using FeriaVirtualWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 1)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var usuario = (USUARIO)Session["usuario"];
            return View(usuario);
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
    }
}