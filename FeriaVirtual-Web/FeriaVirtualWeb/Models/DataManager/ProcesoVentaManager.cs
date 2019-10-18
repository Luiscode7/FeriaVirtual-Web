using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ProcesoVentaManager
    {
        public void InsertProcesoVentaAccordingToUsuario(List<PRODUCTO> productos, decimal procesoventa)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.IDPRODUCTO == item.IDPRODUCTO).FirstOrDefault();
                        producto.IDPROCESOVENTA = procesoventa;
                        db.SaveChanges();
                    }
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}