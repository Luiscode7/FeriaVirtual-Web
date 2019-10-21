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
                        producto.ESTADOPROCESO = "Pendiente";
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

        //inserta el id proceso de acuerdo al id orden que posee en los productos solicitados por el cliente, en tabla productos
        public void InsertOrderToProceso(List<PRODUCTO> productos, decimal proceso)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        PRODUCTO productoOr = db.PRODUCTO.Where(p => p.IDPRODUCTO == item.IDPRODUCTO).FirstOrDefault();
                        productoOr.IDPROCESOVENTA = proceso;
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