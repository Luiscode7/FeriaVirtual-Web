using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Filter;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.ViewModels;
using FeriaVirtualWeb.Reports;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 6)]
    public class ConsultorController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult GetReportes()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            return View();
        }

        public ActionResult ExportPDFListaVentas()
        {
            var listadoVentas = collection.GetVentas();
            var listado = new ListadoVentas();
            byte[] bytesG = listado.PdfReport(listadoVentas);
            return File(bytesG, "application/pdf", "ListadoVentas.pdf");
        }

        public ActionResult ExportPDFListaOrdenes()
        {
            var listadoOrdenes = collection.GetOrdenesList();
            var listado = new ListadoOrdenes();
            byte[] bytesG = listado.PdfReport(listadoOrdenes);
            return File(bytesG, "application/pdf", "ListadoOrdenes.pdf");
        }

        public ActionResult ExportPDFSubastaExterna()
        {
            var listadoSubastaExterna = collection.GetSubastaExternaList();
            var listado = new ListadoSubastasPexterna();
            byte[] bytesG = listado.PdfReport(listadoSubastaExterna);
            return File(bytesG, "application/pdf", "ListadoSubastaExterna.pdf");
        }

        public ActionResult ExportPDFSubastaLocal()
        {
            var listadoSubastaLocal = collection.GetSubastaLocalList();
            var listado = new ListadoSubastasPLocal();
            byte[] bytesG = listado.PdfReport(listadoSubastaLocal);
            return File(bytesG, "application/pdf", "ListadoSubastaLocal.pdf");
        }

        // GET: Consultor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultor/Create
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

        // GET: Consultor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Consultor/Edit/5
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

        // GET: Consultor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Consultor/Delete/5
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
