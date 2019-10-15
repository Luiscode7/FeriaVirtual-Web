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
            var usuario = new USUARIO();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                
                if(!string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(password))
                {
                    usuario = db.USUARIO.FirstOrDefault(p => p.RUTUSUARIO == rut && p.CONTRASENA == password);
                }
                
            }
            return usuario;
        }
    }
}