using FeriaVirtualWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeriaVirtualWeb.Filter
{
    public class CheckSession: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usuario = HttpContext.Current.Session["usuario"];

            if(usuario == null)
            {
                if(filterContext.Controller is LoginController == false)
                {
                    filterContext.HttpContext.Response.Redirect("~/Usuario/Login");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}