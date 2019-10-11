﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Filter;
using System.IO;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 5)]
    public class ProductorController : Controller
    {
        CollectionManager productosList = new CollectionManager();
        
        // GET: Productor
        public ActionResult Index()
        {
            var lista = new List<PRODUCTO>();
            lista = (List<PRODUCTO>)productosList.GetProductosList();
            return View(lista);
        }

        // GET: Productor/Details/5
      
        public JsonResult Listar(List<PRODUCTO> productos)
        {
            var productsSeleted = GetProductsSelected(productos);
            var productor = new ProductorManager();
            var usuario = (USUARIO)Session["usuario"];
            foreach (var item in productsSeleted)
            {
                productor.InsertNewProducto(item, usuario);
            }
            return Json(productsSeleted);
        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        private List<PRODUCTO> GetProductsSelected(List<PRODUCTO> products)
        {
            return products.Where(p => p.IsChecked == true).ToList();
        }

        // GET: Productor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Productor/Edit/5
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

        // GET: Productor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Productor/Delete/5
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
