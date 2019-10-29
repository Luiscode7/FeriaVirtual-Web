using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Filter;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 3)]
    public class ClienteController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult ChooseProducts()
        {
            return View();
        }

        public ActionResult MyOrdersList()
        {
            var lista = new List<PRODUCTO>();
            var listaOrdenes = new List<ORDEN>();
            var usuario = (USUARIO)Session["usuario"];
            listaOrdenes = collection.GetMyOrderList(usuario);
            ViewBag.ordenList = listaOrdenes;
            lista = (List<PRODUCTO>)collection.GetProductosList();
            return View(listaOrdenes);
        }

        public ActionResult GetProductOfMyOrders(decimal id)
        {
            var detalles = collection.GetMyProductsByOrders(id); 
            return View(detalles);
        }

        public ActionResult RecepcionDetails(decimal id)
        {
            var procesoByorden = collection.GetProcesoDecimalByOrden(id);
            var listadoDetails = collection.GetDatosClientByProcesoVenta(procesoByorden);
            return View(listadoDetails);
        }

        public ActionResult GetListToAddNewOrders()
        {
            var lista = (List<PRODUCTO>)collection.GetProductosList();
            return View(lista);
        }

        public JsonResult AddOrder(List<PRODUCTO> productos)
        {
            var productsSeleted = new List<PRODUCTO>();

            if (ModelState.IsValid)
            {
                productsSeleted = collection.GetProductsSelected(productos);
                var cliente = new ClienteManager();
                var usuario = (USUARIO)Session["usuario"];
                cliente.InsertNewProductoToOrder(productsSeleted, usuario);
            }
            else
            {
                return Json(null);
            }

            return Json(productsSeleted);
        }


        public ActionResult GetRecepcion(decimal id)
        {
            var ordenEstado = new ORDEN();
            if(id != 0)
            {
                ordenEstado = collection.GetEstadoOrden(id);
            }
            var ordenEnvio = new ORDEN
            {
                IDORDEN = ordenEstado.IDORDEN,
                ESTADO = "Recepcionado"
            };
            return View(ordenEnvio);
        }

        public JsonResult EditEstadoOrden(ORDEN orden)
        {
            var estado = new ClienteManager();
            var updateEstado = estado.UpdateEstadoOrden(orden);
            var updateNoDisposable = new ORDEN
            {
                IDORDEN = updateEstado.IDORDEN,
                CANTIDAD = updateEstado.CANTIDAD,
                FECHA = updateEstado.FECHA,
                CLIENTE_RUTCLIENTE = updateEstado.CLIENTE_RUTCLIENTE,
                ESTADO = updateEstado.ESTADO
            };
            return Json(updateNoDisposable);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cliente/Edit/5
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

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
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
