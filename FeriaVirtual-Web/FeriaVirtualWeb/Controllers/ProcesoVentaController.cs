using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Filter;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.ViewModels;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 5)]
    public class ProcesoVentaController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult ProcesoVentaList()
        {
            var productos = new List<PRODUCTO>();
            var lista = collection.GetClientListProcesoVenta();
            foreach (var item in lista)
            {
                productos = collection.GetProductClientByOrder(item.ORDEN);
            }
            ViewBag.productos = productos;
            return View(lista);
        }
   

        [HttpPost]
        public ActionResult Postular(List<ProcesoVentaViewModel> proceso)
        {
            var procesoManager = new ProcesoVentaManager();
            var productos = new List<PRODUCTO>();
            var pPostulacion = new List<PRODUCTO>();
            foreach (var item in proceso)
            {
                productos = collection.GetProductClientByOrder(item.ORDEN);
                var usuario = (USUARIO)Session["usuario"];
                pPostulacion = collection.GetProductsProductorAccordingToProcesoVenta(productos, usuario);
                if (pPostulacion != null)
                {
                    procesoManager.InsertProcesoVentaAccordingToUsuario(pPostulacion, item.PROCESO);
                }
            }
            return View(pPostulacion);
        }

        // GET: ProcesoVenta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProcesoVenta/Create
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

        // GET: ProcesoVenta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProcesoVenta/Edit/5
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

        // GET: ProcesoVenta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProcesoVenta/Delete/5
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
