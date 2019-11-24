using System;
using System.Collections.Generic;
using System.Linq;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ProcesoVentaManager
    {
        public List<PRODUCTO> InsertProcesoVentaAccordingToUsuario(List<PRODUCTO> productos, decimal procesoventa, decimal ordenid)
        {
            try
            {
                var producto = new PRODUCTO();
                var newProducto = new PRODUCTO();
                var newProductoList = new List<PRODUCTO>();
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        producto = db.PRODUCTO.Where(p => p.IDPRODUCTO == item.IDPRODUCTO).FirstOrDefault();
                        newProducto = new PRODUCTO
                        {
                            IDPRODUCTO = DatabaseUtil.GetNextIDProducto(),
                            DESCRIPCION = producto.DESCRIPCION,
                            PRECIO = producto.PRECIO,
                            STOCK = producto.STOCK,
                            PRODUCTOR_RUTPRODUCTOR = producto.PRODUCTOR_RUTPRODUCTOR,
                            TIPOVENTA = producto.TIPOVENTA,
                            IDPROCESOVENTA = procesoventa,
                            ORDEN_IDORDEN = ordenid,
                            ESTADOPROCESO = "Pendiente"
                        };
                        db.PRODUCTO.Add(newProducto);
                        newProductoList.Add(newProducto);
                        db.SaveChanges();
                    }
                    return newProductoList;
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

        public List<PRODUCTO> UpdateCantidadProductsToProductsPostulados(List<PRODUCTO> productosOrden, USUARIO usuario)
        {
            var productos = new PRODUCTO();
            var productosLis = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productosOrden)
                {
                    productos = db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == item.ORDEN_IDORDEN && p.DESCRIPCION == item.DESCRIPCION && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).FirstOrDefault();
                    if(productos != null)
                    {
                        productos.CANTIDAD = item.CANTIDAD;
                        productosLis.Add(productos);
                        db.SaveChanges();
                    }
                  
                }
                return productosLis;
            }
        }

        public void UpdateStockProductsAfterPostular(List<PRODUCTO> listaProductos, USUARIO usuario)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in listaProductos)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                        && p.DESCRIPCION == item.DESCRIPCION && p.IDPROCESOVENTA == null && p.TIPOVENTA == "Externo" ).FirstOrDefault();
                        if(producto != null)
                        {
                            if (item.CANTIDAD >= producto.STOCK)
                            {
                                producto.STOCK = 0;
                            }
                            else
                            {
                                producto.STOCK = producto.STOCK - item.CANTIDAD;
                            }
                            db.SaveChanges();
                        }

                        PRODUCTO productoPostulado = db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                        && p.DESCRIPCION == item.DESCRIPCION && p.IDPROCESOVENTA != null && p.ESTADOPROCESO == "Pendiente" && p.TIPOVENTA == "Externo").FirstOrDefault();
                        if(item.CANTIDAD < productoPostulado.STOCK)
                        {
                            productoPostulado.STOCK = item.CANTIDAD;
                            db.SaveChanges();
                        }
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