using System;
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
        CollectionManager collection = new CollectionManager();
        
        public ActionResult MyListProducts()
        {
            var usuario = (USUARIO)Session["usuario"];
            var lista = collection.GetMyProductosList(usuario);
            var listaVacia = collection.GetProductosList();
            ViewBag.misproductos = lista;
            return View(listaVacia);
        }

        public JsonResult Listar(List<PRODUCTO> productos)
        {
            var productsSeleted = new List<PRODUCTO>();

            if (ModelState.IsValid)
            {
                productsSeleted = collection.GetProductsSelected(productos);
                var productor = new ProductorManager();
                var usuario = (USUARIO)Session["usuario"];
                foreach (var item in productsSeleted)
                {
                    productor.InsertNewProducto(item, usuario);
                }
            }
            else
            {
                return Json(null);
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
