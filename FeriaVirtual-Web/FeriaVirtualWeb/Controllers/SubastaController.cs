﻿using System.Web.Mvc;
using FeriaVirtualWeb.Filter;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.ViewModels;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 4)]
    public class SubastaController : Controller
    {
        CollectionManager collection = new CollectionManager();
        // GET: Subasta
        public ActionResult SubastaList()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            return View();
        }

        public ActionResult SubastasList()
        {
            var subasta = collection.GetSubastaExterna();
            return View(subasta);
        }

        public ActionResult SubastasListLocal()
        {
            var subasta = collection.GetSubastaLocal();
            return View(subasta);
        }

        public ActionResult MySubastas()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var listaSu = collection.GetMySubastasList(usuario);
            return View(listaSu);
        }

        public ActionResult GetDetailsMySubastas(decimal id)
        {
            var usuario = (USUARIO)Session["usuario"];
            var listaT = collection.GetTransportistaDetailsBySubasta(id, usuario);
            decimal? proceso = 0;
            var trans = new TRANSPORTISTA
            {
                TIPOTRANSPORTE = listaT.TIPOTRANSPORTE,
                ANCHO = listaT.ANCHO,
                ALTO = listaT.ALTO,
                LARGO = listaT.LARGO,
                CAPACIDADCARGA = listaT.CAPACIDADCARGA,
                REFRIGERACION = listaT.REFRIGERACION == "1" ? "Si" : "No",
                SUBASTAID = listaT.SUBASTAID,
                ESTADOSUBASTA = listaT.ESTADOSUBASTA,
                PRECIO = listaT.PRECIO
            };
            proceso = collection.GetProcesoIdBySubastaId(id);
            var detallesCli = collection.GetDatosClientByProcesoVenta(proceso);
            ViewBag.datoscliente = detallesCli;

            var clientedatos = new ProcesoVentaViewModel();
            if (detallesCli.TIPOPROCESO == "Local")
            {
                var datos = collection.GetDatosClientByProcesoVentaLocal(proceso);
                var cliente = collection.GetDatosClienteByRutOfProducto(datos.RUTCLIENTELOCAL);
                var productosLocal = collection.GetProductosByRutClienteLocalAndProceso(cliente.RUTCLIENTE, proceso);

                clientedatos.IDSUBASTA = datos.IDSUBASTA;
                clientedatos.NOMBRECLIENTE = cliente.NOMBRE;
                clientedatos.DIRECCIONCLINICIAL = cliente.DIRECCION;
                clientedatos.TELEFONOCLI = cliente.TELEFONO;
            }

            ViewBag.datosclienteL = clientedatos;

            return View(trans);
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

        public ActionResult ProductsAccordingToProcesoVentaLocal(decimal id)
        {
            var subastaM = new SubastaManager();
            var idproceso = id;
            var datos = collection.GetDatosClientByProcesoVentaLocal(idproceso);
            var cliente = collection.GetDatosClienteByRutOfProducto(datos.RUTCLIENTELOCAL);
            var productosLocal = collection.GetProductosByRutClienteLocalAndProceso(cliente.RUTCLIENTE, idproceso);
            var clientedatos = new ProcesoVentaViewModel
            {
                IDSUBASTA = datos.IDSUBASTA,
                NOMBRECLIENTE = cliente.NOMBRE,
                DIRECCIONCLINICIAL = cliente.DIRECCION
            };
            ViewBag.productos = productosLocal;
            return View(clientedatos);
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
            var transExist = collection.GetSubastaTransporteIfExist(usuario, trans.SUBASTAID);
            if (trans != null && transExist == null)
            {
                transportista = subastaIn.InsertSubastaAccordingTransportista(usuario, transIns);
            }
            return Json(transportista);
        }

    }
}
