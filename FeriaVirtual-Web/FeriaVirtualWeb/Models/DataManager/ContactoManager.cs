using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ContactoManager
    {
        public CONTACTO InsertNewContacto(CONTACTO newContacto)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    CONTACTO contacto = new CONTACTO
                    {
                        IDCONTACTO = DatabaseUtil.GetNextIDContacto(),
                        EMPRESA = newContacto.EMPRESA,
                        CORREO = newContacto.CORREO,
                        TELEFONO = newContacto.TELEFONO
                    };
                    db.CONTACTO.Add(contacto);
                    db.SaveChanges();
                    return contacto;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}