using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ClienteInternoManager
    {
        public void InsertCompra(List<PRODUCTO> listaproductos, USUARIO usuario)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in listaproductos)
                    {
                        PRODUCTO producto = new PRODUCTO
                        {
                            IDPRODUCTO = DatabaseUtil.GetNextIDProducto(),
                            DESCRIPCION = item.DESCRIPCION,
                            PRECIO = item.PRECIO,
                            STOCK = item.STOCK,
                            PRODUCTOR_RUTPRODUCTOR = item.PRODUCTOR_RUTPRODUCTOR,
                            IDPROCESOVENTA = item.IDPROCESOVENTA,
                            CANTIDAD = item.CANTIDAD,
                            TIPOVENTA = item.TIPOVENTA,
                            CLIENTEINTERNO = usuario.RUTUSUARIO
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
    }
}