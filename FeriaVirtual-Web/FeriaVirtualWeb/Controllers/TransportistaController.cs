using System.Collections.Generic;
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
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
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
                    IDTRANSPORTISTA = item.IDTRANSPORTISTA,
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

        public ActionResult AddNewTransporte()
        {
            var usuario = (USUARIO)Session["usuario"];
            var transporte = collection.GetMyTransporteByToNew(usuario);
            var newtransporte = new TRANSPORTISTA
            {
                IDTRANSPORTISTA = transporte.IDTRANSPORTISTA,
                RUTTRANSPORTISTA = transporte.RUTTRANSPORTISTA,
                NOMBRE = transporte.NOMBRE,
                TELEFONO = transporte.TELEFONO
            };

            return View(newtransporte);
        }

        public JsonResult AddNewTransportes(TRANSPORTISTA transportista)
        {
            var newtransporte = new TransportistaManager();
            return Json(newtransporte.InsertNewTransporte(transportista));
        }

        public ActionResult EditMyTransportes(decimal id)
        {
            var updateTrans = collection.GetMyTransporteById(id);
            return View(updateTrans);
        }

        public JsonResult EditMyTransporte(TRANSPORTISTA transp)
        {
            var updateTrans = new TRANSPORTISTA();
            var transM = new TransportistaManager();
            if(transp != null)
            {
                updateTrans = transM.UpdateTransporte(transp);
            }
            return Json(updateTrans);
        }

    }
}
