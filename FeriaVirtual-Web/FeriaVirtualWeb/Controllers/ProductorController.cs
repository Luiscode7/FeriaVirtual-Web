using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Filter;
using System.IO;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 5)]
    public class ProductorController : Controller
    {
        CollectionManager collection = new CollectionManager();
        
        public ActionResult MyListProducts()
        {
            return View();
        }

        public ActionResult MyProductsListProcesoExterno()
        {
            var usuario = (USUARIO)Session["usuario"];
            var lista = collection.GetMyProductosList(usuario);
            return View(lista);
        }

        public ActionResult MyProductsListProcesoLocal()
        {
            var usuario = (USUARIO)Session["usuario"];
            var listaPLocal = collection.GetMyProductListProcesoLocal(usuario);
            return View(listaPLocal);
        }

        public ActionResult GetListToAddNewProductos()
        {
            var listaVacia = collection.GetProductosList();
            return View(listaVacia);
        }

        public JsonResult AddNewProductos(List<PRODUCTO> productos)
        {
            var productsSeleted = new List<PRODUCTO>();
            var productosList = new List<PRODUCTO>();
            var productosListNoDiposable = new List<PRODUCTO>();
            var newProducto = new PRODUCTO();

            if (ModelState.IsValid)
            {
                productsSeleted = collection.GetProductosListSelected(productos);
                var productor = new ProductorManager();
                var usuario = (USUARIO)Session["usuario"];
                foreach (var item in productsSeleted)
                {
                    if (collection.GetProductosListIfExternoExist(item, usuario))
                    {
                        newProducto = productor.InsertNewProducto(item, usuario);
                        productosList.Add(newProducto);
                    }
                    else
                    {
                        newProducto = productor.UpdateProducto(item, usuario);
                        productosList.Add(newProducto);
                    }
                    
                }
            }
            else
            {
                return Json(null);
            }

            foreach (var item in productosList)
            {
                productosListNoDiposable.Add(new PRODUCTO()
                {
                    IDPRODUCTO = item.IDPRODUCTO,
                    DESCRIPCION = item.DESCRIPCION,
                    PRECIO = item.PRECIO,
                    STOCK = item.STOCK,
                    TIPOVENTA = item.TIPOVENTA
                });
            }
            
            return Json(productosListNoDiposable);
        }

        public ActionResult EditProductos(decimal id)
        {
            var idEdit = collection.GetProductToEdit(id);
            return View(idEdit);
        }

        public JsonResult AddProductoEditado(PRODUCTO producto)
        {
            var productoEdit = new ProductorManager();
            var productoR = productoEdit.UpdateProducto(producto);
            var productoActualizado = new PRODUCTO
            {
                IDPRODUCTO = productoR.IDPRODUCTO,
                DESCRIPCION = productoR.DESCRIPCION,
                PRECIO = productoR.PRECIO,
                STOCK = productoR.STOCK
            };

            return Json(productoActualizado);
        }

        public ActionResult DeleteProduct(decimal id)
        {
            var productoDel = new PRODUCTO();
            if (id != 0)
            {
                productoDel = collection.GetProductByIdProducto(id);
            }
            return View(productoDel);
        }

        public JsonResult DeleteMyProduct(PRODUCTO producto)
        {
            var productorM = new ProductorManager();
            var productoDelete = new PRODUCTO();
            if (producto != null)
            {
                productoDelete = productorM.DeleteProduct(producto);
            }

            return Json(productoDelete);
        }

        // POST: Productor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Productor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Productor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
