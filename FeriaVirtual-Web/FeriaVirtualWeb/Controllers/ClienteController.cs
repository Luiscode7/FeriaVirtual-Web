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
