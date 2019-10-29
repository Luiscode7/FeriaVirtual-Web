using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ClienteManager
    {
        public void InsertNewProductoToOrder(List<PRODUCTO> ListProducto, USUARIO usuario)
        {
            try
            {
                
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    ORDEN orden = new ORDEN
                    {
                        IDORDEN = DatabaseUtil.GetNextIDOrder(),
                        FECHA = DateTime.Now,
                        CLIENTE_RUTCLIENTE = usuario.RUTUSUARIO,
                        ESTADO = "Pendiente"
                    };
                    db.ORDEN.Add(orden);
                    db.SaveChanges();

                    foreach (var item in ListProducto)
                    {
                        PRODUCTO producto = new PRODUCTO
                        {
                            IDPRODUCTO = DatabaseUtil.GetNextIDProducto(),
                            DESCRIPCION = item.DESCRIPCION,
                            CANTIDAD = item.CANTIDAD,
                            ORDEN_IDORDEN = GetLastOrder()
                        };
                        db.PRODUCTO.Add(producto);
                        db.SaveChanges();
                    }
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal GetLastOrder()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.ORDEN.Max(p => p.IDORDEN);
                return lastID;
            }
        }

        public ORDEN UpdateEstadoOrden(ORDEN estadoOr)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    ORDEN orden = db.ORDEN.Where(or => or.IDORDEN == estadoOr.IDORDEN).FirstOrDefault();
                    orden.ESTADO = estadoOr.ESTADO;
                    db.SaveChanges();
                    return orden;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}