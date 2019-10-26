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
    public class TransportistaController : Controller
    {
        CollectionManager collection = new CollectionManager();
        // GET: Transportista
        public ActionResult Transporte()
        {
            return View();
        }

        public ActionResult MyTransportes()
        {
            var newLista = new List<TRANSPORTISTA>();
            var usuario = (USUARIO)Session["usuario"];
            var transporte = collection.GetTransporte(usuario);
            foreach (var item in transporte)
            {
                newLista.Add(new TRANSPORTISTA()
                {
                    TIPOTRANSPORTE = item.TIPOTRANSPORTE,
                    ANCHO = item.ANCHO,
                    ALTO = item.ALTO,
                    LARGO = item.LARGO,
                    CAPACIDADCARGA = item.CAPACIDADCARGA,
                    REFRIGERACION = item.REFRIGERACION == "1" ? "Si" : "No",
                });
            }
            return View(newLista);
        }

        // GET: Transportista/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transportista/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transportista/Create
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

        // GET: Transportista/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transportista/Edit/5
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

        // GET: Transportista/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transportista/Delete/5
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
