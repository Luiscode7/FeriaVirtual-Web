using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ProcesoVentaManager
    {
        public PRODUCTO InsertProcesoVentaAccordingToUsuario(List<PRODUCTO> productos, decimal procesoventa)
        {
            try
            {
                var producto = new PRODUCTO();
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        producto = db.PRODUCTO.Where(p => p.IDPRODUCTO == item.IDPRODUCTO).FirstOrDefault();
                        producto.IDPROCESOVENTA = procesoventa;
                        db.SaveChanges();
                    }
                    return producto;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateEstadoProcesoVenta(decimal? procesov)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    PROCESOVENTA proceso = db.PROCESOVENTA.Where(p => p.IDPROCESOVENTA == procesov).FirstOrDefault();
                    proceso.ESTADO = "Pendiente";
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}