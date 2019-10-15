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
        
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(USUARIO usuario)
        {
            var usuarioManager = new UsuarioManager();
            ActionResult redirect = null;
            var usuarioReturned = new USUARIO();
            usuarioReturned = usuarioManager.GetUsuario(usuario.RUTUSUARIO, usuario.CONTRASENA);

            if(usuarioReturned != null)
            {
                var usuarioPerfil = usuarioReturned.PERFIL_IDPERFIL;
                redirect = RouteAccordingToUser(usuarioPerfil);
                Session["usuario"] = usuarioReturned;
            }
            
            return redirect;
        }

        private ActionResult RouteAccordingToUser(decimal perfil)
        {
            ActionResult output = null;
            switch (perfil)
            {
                case 3:
                    output = RedirectToAction("ChooseProducts", "Cliente");
                    break;
                case 5: output = RedirectToAction("MyListProducts", "Productor");
                    break;
                default:
                    break;
            }
            return output;
        }

    }
}
