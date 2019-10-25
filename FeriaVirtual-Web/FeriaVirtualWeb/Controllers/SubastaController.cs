﻿using System;
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

        
        public ActionResult ProductsAccordingToProcesoVenta(decimal id)
        {
            var subastaM = new SubastaManager();
            var datos = collection.GetDatosClientByProcesoVenta(id);
            var ordenid = subastaM.GetOrderIdByProcesoVentaId(id);
            var listaP = collection.GetProductClientByOrderAndProductorNull(ordenid);
            ViewBag.productos = listaP;
            return View(datos);
        }

        public ActionResult AddTransportAndPrecioToSubasta(decimal id)
        {
            var usuario = (USUARIO)Session["usuario"];
            var tipotransporte = collection.GetTransporte(usuario);
            var transportista = new TRANSPORTISTA();
            transportista = new TRANSPORTISTA
            {
                SUBASTAID = id,
                TRANSPORTELISTA = tipotransporte
            };
            return View(transportista);
        }

        public JsonResult Postular(TRANSPORTISTA trans)
        {
            var usuario = (USUARIO)Session["usuario"];
            var transIns = trans;
            var subastaIn = new SubastaManager();
            var transportista = new TRANSPORTISTA();
            if (trans != null)
            {
                transportista = subastaIn.InsertSubastaAccordingTransportista(usuario, transIns);
            }
            return Json(transportista);
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
