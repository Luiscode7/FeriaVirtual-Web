using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class ProductorManager
    {
        public PRODUCTO InsertNewProducto(PRODUCTO newProducto, USUARIO usuario)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    PRODUCTO producto = new PRODUCTO
                    {
                        IDPRODUCTO = DatabaseUtil.GetNextIDProducto(),
                        DESCRIPCION = newProducto.DESCRIPCION,
                        PRECIO = newProducto.PRECIO,
                        STOCK = newProducto.STOCK,
                        PRODUCTOR_RUTPRODUCTOR = usuario.RUTUSUARIO,
                        TIPOVENTA = newProducto.TIPOVENTA
                    };
                    db.PRODUCTO.Add(producto);
                    db.SaveChanges();

                    return producto;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PRODUCTO UpdateProducto(PRODUCTO productoEdit)
        {
            try
            {
               
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    PRODUCTO producto = db.PRODUCTO.Where(p => p.IDPRODUCTO == productoEdit.IDPRODUCTO).FirstOrDefault();
                    producto.PRECIO = productoEdit.PRECIO;
                    producto.STOCK = productoEdit.STOCK;
                    db.SaveChanges();
                    return producto;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PRODUCTO UpdateProducto(PRODUCTO productoEdit, USUARIO usuario)
        {
            try
            {

                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    PRODUCTO producto = db.PRODUCTO.Where(p => p.DESCRIPCION == productoEdit.DESCRIPCION && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).FirstOrDefault();
                    producto.PRECIO = productoEdit.PRECIO;
                    producto.STOCK = productoEdit.STOCK;
                    db.SaveChanges();
                    return producto;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertProductosWhenHasBeedRejectedToLocal(List<PRODUCTO> productos)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        PRODUCTO producto = new PRODUCTO
                        {
                            IDPRODUCTO = DatabaseUtil.GetNextIDProducto(),
                            DESCRIPCION = item.DESCRIPCION,
                            PRECIO = item.PRECIO,
                            STOCK = item.STOCK,
                            PRODUCTOR_RUTPRODUCTOR = item.PRODUCTOR_RUTPRODUCTOR,
                            TIPOVENTA = "Local"
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

        public void UpdateProductosWhenHasBeedRejectedToLocal(List<PRODUCTO> productos)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION &&
                        p.IDPROCESOVENTA == null && p.TIPOVENTA == "Local" && p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR).FirstOrDefault();
                        producto.STOCK = producto.STOCK + item.STOCK;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateStockProcesoLocalProductsIfCompra(List<PRODUCTO> productos)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in productos)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.TIPOVENTA == item.TIPOVENTA
                                            && p.IDPROCESOVENTA == item.IDPROCESOVENTA &&
                                            p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR && p.CANTIDAD == null && p.CLIENTEINTERNO == null).FirstOrDefault();
                        producto.STOCK = producto.STOCK - item.CANTIDAD;
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