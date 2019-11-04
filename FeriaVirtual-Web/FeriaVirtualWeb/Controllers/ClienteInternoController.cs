using FeriaVirtualWeb.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 2)]
    public class ClienteInternoController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult ListsProcesoVentaLocal()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var lista = collection.GetProcesoVentaLocalList();
            return View(lista);
        }

        public ActionResult PutCompra(decimal id)
        {
            var lista = collection.GetProcesoLocalProductsListOfProductor(id);
            return View(lista);
        }

        public JsonResult Comprar(List<PRODUCTO> productos)
        {
            var usuario = (USUARIO)Session["usuario"];
            var insertCompra = new ClienteInternoManager();
            var updateLocal = new ProductorManager();
            var listap = new List<PRODUCTO>();
            var listaInsertar = collection.GetProcesoLocalProductsListFilterByCantidad(productos);
            if(listaInsertar.Count() != 0)
            {
                listap = insertCompra.InsertCompra(listaInsertar, usuario);
                updateLocal.UpdateStockProcesoLocalProductsIfCompra(listaInsertar);
            }

            return Json(listap);
        }

        public ActionResult GetMyCompras()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var listaP = collection.GetProductsListMyCompras(usuario);
            return View(listaP);
        }
    }
}
