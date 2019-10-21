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
    [UserAuthorization(Rol = 4)]
    public class SubastaController : Controller
    {
        CollectionManager collection = new CollectionManager();
        // GET: Subasta
        public ActionResult SubastaList()
        {
            var subasta = collection.GetSubasta();
            return View(subasta);
        }

        
        public ActionResult ProductsAccordingToOrders(decimal id)
        {
            var productosList = collection.GetProductClientByOrder(id);
            return View(productosList);
        }

        public JsonResult Postular(decimal subasta)
        {
            var usuario = (USUARIO)Session["usuario"];
            var idsubasta = subasta;
            var subastaIn = new SubastaManager();
            var transportista = new TRANSPORTISTA();
            if (subasta != 0)
            {
                transportista = subastaIn.InsertSubastaAccordingTransportista(usuario, subasta);
            }
            return Json(idsubasta);
        }

        // POST: Subasta/Create
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

        // GET: Subasta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Subasta/Edit/5
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

        // GET: Subasta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Subasta/Delete/5
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
