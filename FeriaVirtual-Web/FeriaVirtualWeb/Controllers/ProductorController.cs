﻿using System.Collections.Generic;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Filter;


namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 5)]
    public class ProductorController : Controller
    {
        CollectionManager collection = new CollectionManager();
        
        public ActionResult MyListProducts()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
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

        public ActionResult GetMyProductsSelledAndProfit(decimal id)
        {
            var idproceso = id;
            var usuario = (USUARIO)Session["usuario"];
            var listaPAceppted = collection.GetMyProductsAccepted(usuario, idproceso);
            var listaPAcceptedtotales = collection.GetProductsAccepted(idproceso);
            var sumaPrecios = collection.TotalSumOfPrecioOfProductorAccordingToOneSell(listaPAceppted);

            var ventaByProceso = collection.GetVentaByProcesoVenta(idproceso);
            var ganancia = collection.GetMyProfit(listaPAcceptedtotales, ventaByProceso, sumaPrecios, usuario);
            return View(ganancia);
        }

        public ActionResult GetMyVentasLocales()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var misPrLocales = collection.GetMyProcesoVentaLocal(usuario);
            
            return View(misPrLocales);
        }

        public ActionResult GetProductComprados(decimal id)
        {
            var usuario = (USUARIO)Session["usuario"];
            var misPlocales = collection.GetMyProductListProcesoLocalComprados(usuario, id);
            return View(misPlocales);
        }

        public ActionResult GetGananciaLocales(decimal id)
        {
            var usuario = (USUARIO)Session["usuario"];
            var misPlocales = (List<PRODUCTO>)collection.GetMyProductListProcesoLocalComprados(usuario, id);
            var pagoNeto = collection.GetPagoLocalNeto(misPlocales);
            var ventaByProceso = collection.GetVentaByProcesoVenta(id);
            var pagoTotal = collection.GetPagoLocalTotal(pagoNeto, ventaByProceso);

            ViewBag.venta = ventaByProceso;
            ViewBag.pago = pagoTotal;
            return View(misPlocales);
        }
    }
}
