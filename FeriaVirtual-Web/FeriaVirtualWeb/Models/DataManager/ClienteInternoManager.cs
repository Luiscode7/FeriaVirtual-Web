using System;
using System.Collections.Generic;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ClienteInternoManager
    {
        public List<PRODUCTO> InsertCompra(List<PRODUCTO> listaproductos, USUARIO usuario)
        {
            try
            {
                var lista = new List<PRODUCTO>();
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
                        lista.Add(producto);
                        db.SaveChanges();
                    }
                    return lista;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}