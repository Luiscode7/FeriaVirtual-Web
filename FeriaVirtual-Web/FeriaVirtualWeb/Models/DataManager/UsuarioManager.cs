using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class UsuarioManager
    {
        public USUARIO GetUsuario(string rut, string password)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return !string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(password) ?
                    db.USUARIO.Where(u => u.RUTUSUARIO == rut && u.CONTRASENA == password).FirstOrDefault() 
                    : null;
            }
        }
    }
}