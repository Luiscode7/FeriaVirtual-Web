using FeriaVirtualWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
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

        public ActionResult CotizacionDetalleProcesoVenta(decimal id)
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

        public JsonResult EnviarCotizacion(decimal id)
        {
            var ventaM = new VentaManager();
            var ordenM = new ClienteManager();
            var ordenid = collection.GetOrdenIdByProcedoId(id);
            var productosOr = ventaM.GetProductByOrden(ordenid);
            var listaProducto = ventaM.GetProductsWithCantidadAndPrecioToResumenVenta(productosOr);
            var cliente = collection.GetclienteByOrdenId(ordenid);
            var costoTotal = ventaM.GetCostoTotalProductsOrdesToCotizacion(ordenid);
            var estado = ordenM.ChangeEstadoCotizacionSent(ordenid);

            string productos = string.Empty;
            string paraPortal = "Favor de ACEPTAR o RECHAZAR esta cotizacion por medio de su portal en nuestra pagina";

            foreach (var item in listaProducto)
            {
                productos = productos + "<br/><hr/>" + "<table><tr><td>Producto:</td><td>" + "&nbsp;" + item.DESCRIPCION + "</td></tr>" +
                    "<tr><td>Cantidad Solicitada:</td><td>" + "&nbsp;" + item.CANTIDAD.ToString() + "</td></tr>" +
                    "<tr><td>Precio:</td><td>" + "&nbsp;" + "$" + item.PRECIO.ToString() + "</td></tr></table>";
            }

            string body = "<p>Estimado(a)" + " " + cliente.NOMBRE + " " + "hacemos envio de la cotizacion de su orden de compra numero" + " " + ordenid.ToString() + ":" +" </p>"
                    + productos + " " + "<br/>" + "<table><tr><td><strong>Costo Total:</strong></td><td>" + "&nbsp;" + "<strong>$</strong>" + "<strong>" + costoTotal.ToString() + "</strong>" + "</td></tr></table>" +
                    "<br/><p><strong>"+ paraPortal +"</strong></p>";

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("maipogrande77@gmail.com");
            correo.To.Add(cliente.CORREO);
            correo.Subject = "Cotizacion de Orden de Compra";
            correo.Body = body;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            string mycorreo = "maipogrande77@gmail.com";
            string password = "maipo123";
            smtp.Credentials = new System.Net.NetworkCredential(mycorreo, password);

            smtp.Send(correo);

            return Json(id);
        }

        public ActionResult IngresarValoresDeVenta(decimal id)
        {
            var venta = new VentaViewModel();
            venta.PROCESOVENTA_IDPROCESOVENTA = id;
            venta.FECHA = DateTime.Now;
            var subastaid = collection.GetSubastaByProcesoVenta(id);
            var costoT = collection.GetCostoTranporteToVenta(subastaid);
            venta.COSTOTRANSPORTE = costoT;
            return View(venta);
        }

        [HttpPost]
        public ActionResult ResumenVenta(VentaViewModel venta)
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var ventaM = new VentaManager();
            var insertVenta = new VENTA();
            if (venta != null)
            {
                insertVenta = ventaM.InsertNewVenta(venta);
            }
            var ventaDetalle = ventaM.GetAndInsertDetalisVenta(insertVenta);
            var ordenid = ventaM.GetOrdenIdByProcesoID(venta.PROCESOVENTA_IDPROCESOVENTA);
            var productosOr = ventaM.GetProductByOrden(ordenid);
            ViewBag.productos = ventaM.GetProductsWithCantidadAndPrecioToResumenVenta(productosOr);
            return View(ventaDetalle);
        }

        public JsonResult EnviarResumen(decimal id)
        {
            var ventaM = new VentaManager();
            var venta = collection.GetVentaByProcesoVenta(id);
            var ordenid = collection.GetOrdenIdByProcedoId(id);
            var productosOr = ventaM.GetProductByOrden(ordenid);
            var listaProducto = ventaM.GetProductsWithCantidadAndPrecioToResumenVenta(productosOr);
            var cliente = collection.GetclienteByOrdenId(ordenid);

            string productos = string.Empty;

            foreach (var item in listaProducto)
            {
                productos = productos + "<br/><hr/>" + "<table><tr><td>Producto:</td><td>" + "&nbsp;" + item.DESCRIPCION + "</td></tr>" +
                    "<tr><td>Cantidad Solicitada:</td><td>" + "&nbsp;" + item.CANTIDAD.ToString() + "</td></tr>" +
                    "<tr><td>Precio:</td><td>" + "&nbsp;" + "$" + item.PRECIO.ToString() + "</td></tr></table>";
            }

            string body = "<p>Estimado(a)" + " " + cliente.NOMBRE + " " + "los costos correspondientes a su orden numero" + " " + ordenid.ToString() + " " + "son: </p>"
                    + productos +" "+"<br/>"+ "<table><tr><td><strong>Costo Total:</strong></td><td>" + "&nbsp;" + "<strong>$</strong>" + "<strong>"+ venta.COSTOTOTAL.ToString() +"</strong>"+ "</td></tr></table>";

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("maipogrande77@gmail.com");
            correo.To.Add(cliente.CORREO);
            correo.Subject = "Resumen de venta de Orden de Compra";
            correo.Body = body;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            string mycorreo = "maipogrande77@gmail.com";
            string password = "maipo123";
            smtp.Credentials = new System.Net.NetworkCredential(mycorreo, password);

            smtp.Send(correo);

            return Json(id);
        }

        public ActionResult HistorialVentas()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var ventas = collection.GetVentas();
            return View(ventas);
        }

        public JsonResult RepartirGanancias(decimal id)
        {
            var idproceso = id;
            var listaPAceppted = new List<PRODUCTO>();
            var ganancia = new VENTA();
            var usuarios = collection.GetProductoresByProcesoID(id);
            foreach (var item in usuarios)
            {
                listaPAceppted = collection.GetMyProductsAcceptedByProductorList(item.RUTPRODUCTOR, idproceso);
                var listaPAcceptedtotales = collection.GetProductsAccepted(idproceso);
                var sumaPrecios = collection.TotalSumOfPrecioOfProductorAccordingToOneSell(listaPAceppted);

                var ventaByProceso = collection.GetVentaByProcesoVenta(idproceso);
                ganancia = collection.GetMyProfitToEmail(listaPAcceptedtotales, ventaByProceso, sumaPrecios, item.RUTPRODUCTOR);

                string body = "<p>Estimado(a)"+" "+ item.NOMBRE +" "+"las ganancias correspondientes a la venta numero"+" "+ ganancia.IDVENTA.ToString()+" "+"son: </p>"
                    +"</br>"+ "<table><tr><td>Costo Transporte</td><td>"+"$" + ganancia.COSTOTRANSPORTE.ToString() + "</td></tr>" +
                    "<tr><td>Comision Empresa</td><td>" + ganancia.COMISIONEMPRESA.ToString() +"%"+ "</td></tr>" +
                    "<tr><td>Impuesto Aduana</td><td>" + "$" + ganancia.IMPUESTOADUANA.ToString() + "</td></tr>" +
                    "<tr><td>Ganancia Neta</td><td>" + "$" + ganancia.GANANCIAPRODUCTORNETA.ToString() + "</td></tr>" +
                    "<tr><td>Ganancia Total</td><td>" + "$" + ganancia.GANANCIATOTAL.ToString() + "</td></tr></table>";

                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("maipogrande77@gmail.com");
                correo.To.Add(item.CORREO);
                correo.Subject = "Ganancias de Venta Realizada";
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                string mycorreo = "maipogrande77@gmail.com";
                string password = "maipo123";
                smtp.Credentials = new System.Net.NetworkCredential(mycorreo, password);

                smtp.Send(correo);

            }
            
            return Json(ganancia);
        }

        public ActionResult GananciasOfProductores(List<PRODUCTOR> productor)
        {
            var listaPaceptados = new List<PRODUCTO>();
            var listaPAceppted = new List<PRODUCTO>();
            var listaPAcceptedtotales = new List<PRODUCTO>();
            decimal? sumaPrecios = 0;
            var ventaByProceso = new VENTA();
            var ganancia = new List<VENTA>(); 
            foreach (var item in productor)
            {
                listaPAceppted = collection.GetMyProductsAcceptedByListProductor(item.RUTPRODUCTOR, item.PROCESOID);
                listaPAcceptedtotales = collection.GetProductsAccepted(item.PROCESOID);
                sumaPrecios = collection.TotalSumOfPrecioOfProductorAccordingToOneSell(listaPAceppted);
                ventaByProceso = collection.GetVentaByProcesoVenta(item.PROCESOID);
                ganancia = collection.GetMyProfitListToAdmin(listaPAcceptedtotales, ventaByProceso, sumaPrecios, item.RUTPRODUCTOR);
            }
            return View(ganancia);
        }

        public ActionResult ProcesosVentaLocal()
        {
            var pVental = collection.GetProcesoVentaLocalListToAdmin();
            return View(pVental);
        }

        public ActionResult DetalleProcesoVentaLocal(decimal id)
        {
            var productores = collection.GetProductorDatosbyProcesoId(id);
            return View(productores);
        }

        public JsonResult GenerarVentaLocal(List<ProcesoVentaViewModel> productventa)
        {
            var ventaM = new VentaManager();
            var generarv = ventaM.InsertNewVentaLocal(productventa);
            return Json(productventa);
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
