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
            return View(lista);
        }

        public ActionResult ProductosListAccordingProcesoVenta(decimal id)
        {
            var productos = new List<PRODUCTO>();
            productos = collection.GetProductClientByOrder(id);
            return View(productos);
        }
   

        public JsonResult Postular(decimal orden)
        {
            var procesoManager = new ProcesoVentaManager();
            var productos = new List<PRODUCTO>();
            var pPostulacion = new List<PRODUCTO>();
            var newOrden = orden;
            var procesoOr = orden;
            var proceso = new PROCESOVENTA();
   
            productos = collection.GetProductClientByOrder(newOrden);
            proceso = collection.GetProcesoByOrden(procesoOr);
            var usuario = (USUARIO)Session["usuario"];
            pPostulacion = collection.GetProductsProductorAccordingToProcesoVenta(productos, usuario);
            if (pPostulacion.Count() != 0)
            {
                var procesoestado = procesoManager.InsertProcesoVentaAccordingToUsuario(pPostulacion, proceso.IDPROCESOVENTA);
                procesoManager.InsertOrderToProceso(productos, proceso.IDPROCESOVENTA);
            }
          
            return Json(pPostulacion);
        }

        public ActionResult MyPostulaciones()
        {
            var usuario = (USUARIO)Session["usuario"];
            var listaP = collection.GetMyPostulaciones(usuario);
            return View(listaP);
        }

        public ActionResult MyPostulacionesDetails(decimal? id)
        {
            var idp = id;
            var detalle = new ProcesoVentaViewModel();
            var listaP = new List<PRODUCTO>();
            if (id != 0)
            {
                detalle = collection.GetMyPostulacionDetails(id);
            }

            listaP = collection.GetProductClientByOrder(idp);
            ViewBag.productos = listaP;
            return View(detalle);
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
