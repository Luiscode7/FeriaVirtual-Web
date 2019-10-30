using FeriaVirtualWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 2)]
    public class ClienteInternoController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult ListsProcesoVentaLocal()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var lista = collection.GetProcesoVentaLocalList();
            return View(lista);
        }

        public ActionResult PutCompra(decimal id)
        {
            var lista = collection.GetProcesoLocalProductsListOfProductor(id);
            return View(lista);
        }

        public JsonResult Comprar(List<PRODUCTO> productos)
        {
            var usuario = (USUARIO)Session["usuario"];
            var insertCompra = new ClienteInternoManager();
            var updateLocal = new ProductorManager();
            var listap = new List<PRODUCTO>();
            var listaInsertar = collection.GetProcesoLocalProductsListFilterByCantidad(productos);
            if(listaInsertar.Count() != 0)
            {
                listap = insertCompra.InsertCompra(listaInsertar, usuario);
                updateLocal.UpdateStockProcesoLocalProductsIfCompra(listaInsertar);
            }

            return Json(listap);
        }

        // GET: ClienteInterno/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteInterno/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteInterno/Create
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

        // GET: ClienteInterno/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClienteInterno/Edit/5
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

        // GET: ClienteInterno/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClienteInterno/Delete/5
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
