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
        public void InsertNewProducto(PRODUCTO newProducto, USUARIO usuario)
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
    }
}