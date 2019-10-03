using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Filter
{
    public class UserAuthorization:AuthorizeAttribute
    {
        private USUARIO usuario;
        public int Rol { get; set; }
        public UserAuthorization(int rol = 0)
        {
            Rol = rol;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            usuario = (USUARIO)HttpContext.Current.Session["usuario"];

            if(usuario.PERFIL_IDPERFIL != Rol)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
        }
    }
}