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
                var usuario = new USUARIO();
                if(!string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(password))
                {
                    usuario = db.USUARIO.FirstOrDefault(p => p.RUTUSUARIO == rut && p.CONTRASENA == password);
                }
                return usuario;
            }
        }
    }
}