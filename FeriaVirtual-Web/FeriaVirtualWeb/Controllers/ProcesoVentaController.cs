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
            return View();
        }

        public ActionResult ProcesosVentaList()
        {
            var lista = collection.GetClientListProcesoVenta();
            return View(lista);
        }

        public ActionResult ProductosListAccordingProcesoVenta(decimal id)
        {
            var productos = new List<PRODUCTO>();
            productos = collection.GetProductClientByOrderAndProductorNull(id);
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
   
            productos = collection.GetProductClientByOrderAndProductorNull(newOrden);
            proceso = collection.GetProcesoByOrden(procesoOr);
            var usuario = (USUARIO)Session["usuario"];
            pPostulacion = collection.GetProductsProductorAccordingToProcesoVenta(productos, usuario);
            if (pPostulacion.Count() != 0)
            {
                var productsInserted = procesoManager.InsertProcesoVentaAccordingToUsuario(pPostulacion, proceso.IDPROCESOVENTA, newOrden);
                var productosfull = procesoManager.UpdateCantidadProductsToProductsPostulados(productos, usuario);
                foreach (var item in productos)
                {
                    if (item.IDPROCESOVENTA == null)
                    {
                        procesoManager.InsertOrderToProceso(productos, proceso.IDPROCESOVENTA);
                    }
                }
                
                procesoManager.UpdateStockProductsAfterPostular(productosfull);
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
            var usuario = (USUARIO)Session["usuario"];
            if (id != 0)
            {
                detalle = collection.GetMyPostulacionDetails(id);
            }

            listaP = collection.GetProductsListAccordingToPostulacion(idp, usuario);
            ViewBag.productos = listaP;
            return View(detalle);
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
