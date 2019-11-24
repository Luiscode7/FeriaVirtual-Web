using FeriaVirtualWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.ViewModels;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 1)]
    public class AdministradorController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult OfertasTranportistas()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            return View();
        }

        public ActionResult SubastasExternaList()
        {
            var subasta = collection.GetSubastaExternaToAdministrador();
            return View(subasta);
        }

        public ActionResult SubastasLocalList()
        {
            var subasta = collection.GetSubastaLocalToAdministrador();
            return View(subasta);
        }

        public ActionResult Transportistas(decimal id)
        {
            var oferta = collection.GetTransportistasOfertas(id);
            return View(oferta);
        }

        public ActionResult AceptarTransporte(decimal id)
        {
            var lowprice = collection.GetTransportistaLowPrice(id);
            var accept = collection.GetTransportistaByLowPrice(lowprice, id);
            return View(accept);
        }

        public JsonResult AceptarTransportePost(decimal transportista)
        {
            var adminM = new AdministradorManager();
            var accept = adminM.UpdateEstadoTransporteToAccept(transportista);
            return Json(accept);
        }

        public ActionResult ProcesosVentaExterna()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var listaP = collection.GetProcesoVentaExternaList();
            return View(listaP);
        }

        public ActionResult DetalleProcesoVenta(decimal id)
        {
            var detalle = new ProcesoVentaViewModel();
            var listaProductores = new List<ProcesoVentaViewModel>();
            var listaPordenes = new List<PRODUCTO>();
            if (id != 0)
            {
                detalle = collection.GetMyPostulacionDetails(id);
                listaProductores = collection.GetProductorDatosbyOrdenId(id);
                listaPordenes = collection.GetMyProductsByOrders(id);
            }
            ViewBag.productores = listaProductores;
            ViewBag.productosOr = listaPordenes;
            return View(detalle);
        }

        public ActionResult IngresarValoresDeVenta(decimal id)
        {
            var venta = new VENTA();
            venta.PROCESOVENTA_IDPROCESOVENTA = id;
            venta.FECHA = DateTime.Now;
            return View(venta);
        }

        public ActionResult Reportes()
        {
            return View();
        }

        // GET: Administrador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrador/Create
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

        // GET: Administrador/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administrador/Edit/5
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

        // GET: Administrador/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administrador/Delete/5
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
