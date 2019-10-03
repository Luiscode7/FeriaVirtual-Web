using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;

namespace FeriaVirtualWeb.Controllers
{
  
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {   
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(USUARIO usuario)
        {
            var usuarioManager = new UsuarioManager();
            var usuarioReturned = usuarioManager.GetUsuario(usuario.RUTUSUARIO, usuario.CONTRASENA);
            Session["usuario"] = usuarioReturned;

            return RedirectToAction("Index","Home");
        }

    }
}
