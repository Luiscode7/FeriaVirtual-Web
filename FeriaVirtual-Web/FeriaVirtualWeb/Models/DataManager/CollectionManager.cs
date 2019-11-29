using System;
using System.Collections.Generic;
using System.Linq;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.ViewModels;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class CollectionManager
    {
        public IEnumerable<PRODUCTO> GetProductosList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRECIO == null && p.CANTIDAD == null).ToList();
            }
            
        }

        public IEnumerable<PRODUCTO> GetMyProductosList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var listaRechazados = GetProductsRejected(usuario);
                var producto = new List<PRODUCTO>();
                var alocal = new ProductorManager();
                if (GetProductProcesoLocalEqualsToRejected(listaRechazados).Count() == 0)
                {
                    alocal.InsertProductosWhenHasBeedRejectedToLocal(listaRechazados);
                    ChangeRechazadosToMovidos(listaRechazados);
                }
                else
                {
                    alocal.UpdateProductosWhenHasBeedRejectedToLocal(listaRechazados);
                    ChangeRechazadosToMovidos(listaRechazados);
                }

                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == null && p.TIPOVENTA == "Externo").OrderBy(p => p.IDPRODUCTO).ToList();
            }

        }

        public VENTA GetMyVenta(decimal idprocesoventa)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.VENTA.Where(v => v.PROCESOVENTA_IDPROCESOVENTA == idprocesoventa).FirstOrDefault();
            }
        }

        public PRODUCTO GetProductByIdProducto(decimal id)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPRODUCTO == id).FirstOrDefault();
            }
        }

        public List<ProcesoVentaViewModel> GetProcesoVentaLocalList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pd in db.PRODUCTOR join pr in db.PRODUCTO
                            on pd.RUTPRODUCTOR equals pr.PRODUCTOR_RUTPRODUCTOR
                            join pv in db.PROCESOVENTA on pr.IDPROCESOVENTA equals
                            pv.IDPROCESOVENTA where pv.TIPOPROCESO == "Local"
                            select new ProcesoVentaViewModel
                            {
                                PROCESO = pv.IDPROCESOVENTA,
                                NOMBREPRODUCTOR = pd.NOMBRE,
                                CLIENTEINICIAL = pr.PRODUCTOR_RUTPRODUCTOR,
                                FECHA = pv.FECHA

                            }).Distinct().ToList();

                return query;
            }
        }

        public List<PRODUCTO> GetProcesoLocalProductsListOfProductor(decimal proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPROCESOVENTA == proceso && p.CANTIDAD == null && p.CLIENTEINTERNO == null).ToList();
            }
        }

        public List<PRODUCTO> GetProcesoLocalProductsListFilterByCantidad(List<PRODUCTO> productos)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return  productos.Where(p => p.CANTIDAD != null).ToList(); 
            }
        }

        public IEnumerable<PRODUCTO> GetMyProductListProcesoLocal(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.TIPOVENTA == "Local" && p.CLIENTEINTERNO == null).OrderBy(p => p.IDPRODUCTO).ToList();
            }
        }

        public IEnumerable<PRODUCTO> GetMyProductListProcesoLocalComprados(USUARIO usuario, decimal procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO &&
                p.TIPOVENTA == "Local" && p.IDPROCESOVENTA == procesoid && p.CLIENTEINTERNO != null).ToList();
            }
        }

        public List<PROCESOVENTA> GetMyProcesoVentaLocal(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pr in db.PRODUCTO join pv in db.PROCESOVENTA
                             on pr.IDPROCESOVENTA equals pv.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             && pr.TIPOVENTA == "Local"
                             select pv).GroupBy(p => p.IDPROCESOVENTA).Select(p => p.FirstOrDefault()).ToList();

                return query;
            }
        }

        private List<PRODUCTO> GetProductsRejected(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.ESTADOPROCESO == "Rechazado").ToList();
            }
        }

        private List<PRODUCTO> GetProductProcesoLocalEqualsToRejected(List<PRODUCTO> productos)
        {
            var lista = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    lista = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.TIPOVENTA == "Local" &&
                    p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR && p.IDPROCESOVENTA == null).ToList();
                }
                return lista;
            }
        }

        private void ChangeRechazadosToMovidos(List<PRODUCTO> rechazados)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in rechazados)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR && p.ESTADOPROCESO == item.ESTADOPROCESO).FirstOrDefault();
                        producto.ESTADOPROCESO = "Movido";
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PRODUCTO> GetProductsSelected(List<PRODUCTO> products)
        {
            return products.Where(p => p.CANTIDAD != null).ToList();
        }

        public List<PRODUCTO> GetProductosListSelected(List<PRODUCTO> products)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return products.Where(p => p.PRECIO != null && p.STOCK != null).ToList();
            }
        }

        public List<ORDEN> GetMyOrderList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.ORDEN.Where(p => p.CLIENTE_RUTCLIENTE == usuario.RUTUSUARIO).OrderBy(p => p.IDORDEN).ToList();
            }
        }

        public ORDEN GetEstadoOrden(decimal orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.ORDEN.Where(or => or.IDORDEN == orden).FirstOrDefault();
            }
        }

        public decimal? GetOrdenIdByProcedoId(decimal? procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.IDPROCESOVENTA == procesoid).FirstOrDefault().ORDENID;
            }
        }

        public List<PRODUCTO> GetMyProductsByOrders(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.CANTIDAD != null && p.STOCK == null).ToList();
            }
        }

        public IEnumerable<ProcesoVentaViewModel> GetClientListProcesoVenta()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join or in db.ORDEN
                             on pv.ORDENID equals or.IDORDEN
                             join cl in db.CLIENTE
                             on or.CLIENTE_RUTCLIENTE equals cl.RUTCLIENTE
                             select new ProcesoVentaViewModel()
                             {
                                 PROCESO = pv.IDPROCESOVENTA,
                                 ORDEN = pv.ORDENID,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 FECHA = pv.FECHA
                                 
                             }).OrderBy(p => p.ORDEN).ToList();

                return query as IEnumerable<ProcesoVentaViewModel>;
            }
        }

        public List<PRODUCTO> GetProductClientByOrderAndProductorNull(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.PRODUCTOR_RUTPRODUCTOR == null).ToList();
            }
        }

        public List<PRODUCTO> GetProductsListAccordingToPostulacion(decimal? orden, USUARIO usuario)
        {
            var listaP = new List<PRODUCTO>();
            var listaOr = new List<PRODUCTO>();
            var ordenid = orden;
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                listaP = db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == ordenid && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).ToList();
                listaOr = GetProductsListClientOrdenAccordingPostulacionProducts(ordenid, listaP);
                return listaOr;
            }
        }

        private List<PRODUCTO> GetProductsListClientOrdenAccordingPostulacionProducts(decimal? orden, List<PRODUCTO> productos)
        {
            var producto = new PRODUCTO();
            var lista = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    producto = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.ORDEN_IDORDEN == orden && p.PRECIO == null).FirstOrDefault();
                    lista.Add(new PRODUCTO()
                    {
                        DESCRIPCION = item.DESCRIPCION,
                        CANTIDAD = producto.CANTIDAD,
                        STOCK = item.STOCK,
                        ESTADOPROCESO = item.ESTADOPROCESO,
                        CANTIDADACEPTADA = item.CANTIDAD
                    });
                }
                return lista;
            }
        }

        public List<PRODUCTO> GetProductsProductorAccordingToProcesoVenta(List<PRODUCTO> productos,USUARIO usuario)
        {
            var productosP = new PRODUCTO();
            var newList = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    productosP = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                    && p.IDPROCESOVENTA == null && p.TIPOVENTA == "Externo").FirstOrDefault();
                    if(productosP != null && productosP.STOCK >= 1)
                    {
                        newList.Add(new PRODUCTO
                        {
                            IDPRODUCTO = productosP.IDPRODUCTO,
                            DESCRIPCION = productosP.DESCRIPCION,
                            PRECIO = productosP.PRECIO,
                            STOCK = productosP.STOCK,
                            PRODUCTOR_RUTPRODUCTOR = productosP.PRODUCTOR_RUTPRODUCTOR
                        });
                    }
                    
                }
                return newList;
            }
        }

        public PROCESOVENTA GetProcesoByOrden(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.ORDENID == orden).FirstOrDefault();
            }
        }

        public decimal? GetProcesoDecimalByOrden(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.ORDENID == orden).FirstOrDefault().IDPROCESOVENTA;
            }
        }

        public PRODUCTO GetProductToEdit(decimal id)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPRODUCTO == id).FirstOrDefault();
            }
        }

        public bool GetProductosListIfExternoExist(PRODUCTO producto, USUARIO usuario)
        {
            var newProducto = new PRODUCTO();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                newProducto = db.PRODUCTO.Where(p => p.DESCRIPCION == producto.DESCRIPCION && p.TIPOVENTA == producto.TIPOVENTA
                && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == null).FirstOrDefault();
                if(newProducto != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public TRANSPORTISTA GetTransportistaByLowPrice(decimal? lowprice, decimal subastaid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(tr => tr.PRECIO == lowprice && tr.SUBASTAID == subastaid).FirstOrDefault();
            }
        }

        public decimal? GetTransportistaLowPrice(decimal subastaid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(tr => tr.SUBASTAID == subastaid).Min(tr => tr.PRECIO);
            }
        }

        public List<TRANSPORTISTA> GetTransportistasOfertas(decimal subasta)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.SUBASTAID == subasta && t.ESTADOSUBASTA == "Pendiente").ToList();
            }
        }

        public List<TRANSPORTISTA> GetTransportistas()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.ESTADOSUBASTA != null).ToList();
            }
        }

        public List<TRANSPORTISTA> GetTransporte(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO && t.SUBASTAID == null).ToList();
            }
        }

        public TRANSPORTISTA GetSubastaTransporteIfExist(USUARIO usuario, decimal? idsubasta)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO && t.SUBASTAID == idsubasta).FirstOrDefault();
            }
        }

        public TRANSPORTISTA GetMyTransporteById(decimal idtrans)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.IDTRANSPORTISTA == idtrans).FirstOrDefault();
            }
        }

        public TRANSPORTISTA GetMyTransporteByToNew(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO).FirstOrDefault();
            }
        }

        public IEnumerable<string> GetRefrigeracionList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.ToList().Select(t => t.REFRIGERACION).Distinct();
            }
        }

        public List<SUBASTA> GetSubastaExternaToAdministrador()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from tr in db.TRANSPORTISTA join sb in db.SUBASTA
                             on tr.SUBASTAID equals sb.IDSUBASTA
                             join pv in db.PROCESOVENTA on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             where pv.TIPOPROCESO == "Externo" && tr.ESTADOSUBASTA != "Aceptado"
                             select sb).GroupBy(sb => sb.IDSUBASTA).Select(sb => sb.FirstOrDefault()).ToList();
                return query;
            }
        }

        public List<SUBASTA> GetSubastaLocalToAdministrador()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from tr in db.TRANSPORTISTA join sb in db.SUBASTA
                             on tr.SUBASTAID equals sb.IDSUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             where pv.TIPOPROCESO == "Local" && tr.ESTADOSUBASTA != "Aceptado"
                             select sb).GroupBy(sb => sb.IDSUBASTA).Select(sb => sb.FirstOrDefault()).ToList();
                return query;
            }
        }

        public List<SUBASTA> GetSubastaExterna()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             where pv.TIPOPROCESO == "Externo"
                             select sb).ToList();
                return query;
            }
        }

        public List<SUBASTA> GetSubastaLocal()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             where pv.TIPOPROCESO == "Local"
                             select sb).ToList();
                return query;
            }
        }

        public IEnumerable<ProcesoVentaViewModel> GetMySubastasList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from t in db.TRANSPORTISTA join s in db.SUBASTA
                             on t.SUBASTAID equals s.IDSUBASTA
                             where t.RUTTRANSPORTISTA == usuario.RUTUSUARIO
                             && t.ESTADOSUBASTA != null
                             select new ProcesoVentaViewModel
                             {
                                 IDSUBASTA = s.IDSUBASTA,
                                 ESTADOSUBASTA = t.ESTADOSUBASTA,
                                 FECHASUBASTA = s.FECHA

                             }).ToList();

                return query;
            }
        }

        public TRANSPORTISTA GetTransportistaDetailsBySubasta(decimal subasta, USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.SUBASTAID == subasta && t.RUTTRANSPORTISTA == usuario.RUTUSUARIO).FirstOrDefault();
            }
        }

        public List<ProcesoVentaViewModel> GetMyPostulaciones(USUARIO usuario)
        {
            var repetido = new ProcesoVentaViewModel();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var listadoAceptados = GetMyPostulacionesAceptadas(usuario);
                var listadoPendientes = GetMyPostulacionesPendientes(usuario);
                var listadoMovidos = GetMyPostulacionesMovidas(usuario);
                var listadoRechazados = GetMyPostulacionesRechazadas(usuario);

                foreach (var item in listadoPendientes)
                {
                    repetido = listadoAceptados.Where(p => p.PROCESO == item.PROCESO).FirstOrDefault();
                    if (repetido == null)
                    {
                        listadoAceptados.Add(new ProcesoVentaViewModel
                        {
                            PROCESO = item.PROCESO,
                            ESTADO = item.ESTADO,
                            FECHA = item.FECHA,
                            ORDEN = item.ORDEN,
                            TIPOPROCESO = item.TIPOPROCESO
                        });
                    }
                }

                foreach (var item in listadoMovidos)
                {
                    repetido = listadoAceptados.Where(p => p.PROCESO == item.PROCESO).FirstOrDefault();
                    if (repetido == null)
                    {
                        listadoAceptados.Add(new ProcesoVentaViewModel
                        {
                            PROCESO = item.PROCESO,
                            ESTADO = item.ESTADO,
                            FECHA = item.FECHA,
                            ORDEN = item.ORDEN,
                            TIPOPROCESO = item.TIPOPROCESO
                        });
                    }
                }

                foreach (var item in listadoRechazados)
                {
                    repetido = listadoAceptados.Where(p => p.PROCESO == item.PROCESO).FirstOrDefault();
                    if (repetido == null)
                    {
                        listadoAceptados.Add(new ProcesoVentaViewModel
                        {
                            PROCESO = item.PROCESO,
                            ESTADO = item.ESTADO,
                            FECHA = item.FECHA,
                            ORDEN = item.ORDEN,
                            TIPOPROCESO = item.TIPOPROCESO
                        });
                    }
                }

                return listadoAceptados;
            }
        }

        private List<ProcesoVentaViewModel> GetMyPostulacionesAceptadas(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA
                             equals pr.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             && pr.TIPOVENTA == "Externo" && pr.ESTADOPROCESO == "Aceptado"
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 FECHA = pv.FECHA,
                                 ORDEN = pv.ORDENID,
                                 TIPOPROCESO = pv.TIPOPROCESO

                             }).GroupBy(p => p.PROCESO).Select(p => p.FirstOrDefault()).ToList();

                return query;
            }
        }

        
        private List<ProcesoVentaViewModel> GetMyPostulacionesPendientes(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA
                             equals pr.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             && pr.TIPOVENTA == "Externo" && pr.ESTADOPROCESO == "Pendiente"
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 FECHA = pv.FECHA,
                                 ORDEN = pv.ORDENID,
                                 TIPOPROCESO = pv.TIPOPROCESO

                             }).GroupBy(p => p.PROCESO).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        private List<ProcesoVentaViewModel> GetMyPostulacionesMovidas(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA
                             equals pr.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             && pr.TIPOVENTA == "Externo" && pr.ESTADOPROCESO == "Movido"
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 FECHA = pv.FECHA,
                                 ORDEN = pv.ORDENID,
                                 TIPOPROCESO = pv.TIPOPROCESO

                             }).GroupBy(p => p.PROCESO).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        private List<ProcesoVentaViewModel> GetMyPostulacionesRechazadas(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA
                             equals pr.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             && pr.TIPOVENTA == "Externo" && pr.ESTADOPROCESO == "Rechazado"
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 FECHA = pv.FECHA,
                                 ORDEN = pv.ORDENID,
                                 TIPOPROCESO = pv.TIPOPROCESO

                             }).GroupBy(p => p.PROCESO).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }


        public ProcesoVentaViewModel GetMyPostulacionDetails(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lista = new List<PRODUCTO>();
                var query = (from pr in db.PRODUCTO
                             join pv in db.PROCESOVENTA on pr.IDPROCESOVENTA
                             equals pv.IDPROCESOVENTA join or in db.ORDEN
                             on pv.ORDENID equals or.IDORDEN join cl in db.CLIENTE
                             on or.CLIENTE_RUTCLIENTE equals cl.RUTCLIENTE
                             where pr.ORDEN_IDORDEN == orden
                             select new ProcesoVentaViewModel()
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 ORDEN = pr.ORDEN_IDORDEN,
                                 FECHA = pv.FECHA,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 PAISCLIENTE = cl.PAIS,
                                 TIPOPROCESO = pv.TIPOPROCESO
                                
                             }).FirstOrDefault();

                return query as ProcesoVentaViewModel;
            }
        }

        public decimal? GetProcesoIdBySubastaId(decimal subasta)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.SUBASTA.Where(s => s.IDSUBASTA == subasta).FirstOrDefault().PROCESOVENTAID;
            }
        }

        public List<ProcesoVentaViewModel> GetDatosClientByProcesoVenta(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA equals
                             pr.IDPROCESOVENTA join pd in db.PRODUCTOR on
                             pr.PRODUCTOR_RUTPRODUCTOR equals pd.RUTPRODUCTOR
                             where pv.IDPROCESOVENTA == proceso && pr.ESTADOPROCESO == "Aceptado"
                             select new ProcesoVentaViewModel()
                             {
                                 IDSUBASTA = sb.IDSUBASTA,
                                 NOMBREPRODUCTOR = pd.NOMBRE,
                                 DIRECCIONCLINICIAL = pd.DIRECCION,
                                 TELEFONOCLI = pd.TELEFONO,
                                 FECHA = pv.FECHA,
                                 TIPOPROCESO = pr.TIPOVENTA

                             }).GroupBy(p => p.NOMBREPRODUCTOR).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        public List<ProcesoVentaViewModel> GetDatosClientByProcesoVentaL(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA equals
                             pr.IDPROCESOVENTA join pd in db.PRODUCTOR on
                             pr.PRODUCTOR_RUTPRODUCTOR equals pd.RUTPRODUCTOR
                             where pv.IDPROCESOVENTA == proceso && pr.TIPOVENTA == "Local"
                             select new ProcesoVentaViewModel()
                             {
                                 IDSUBASTA = sb.IDSUBASTA,
                                 NOMBREPRODUCTOR = pd.NOMBRE,
                                 DIRECCIONCLINICIAL = pd.DIRECCION,
                                 TELEFONOCLI = pd.TELEFONO,
                                 FECHA = pv.FECHA,
                                 TIPOPROCESO = pr.TIPOVENTA

                             }).GroupBy(p => p.NOMBREPRODUCTOR).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        public List<ProcesoVentaViewModel> GetDatosClientByProcesoVentaL(string rut, decimal? procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA equals
                             pr.IDPROCESOVENTA join pd in db.PRODUCTOR on
                             pr.PRODUCTOR_RUTPRODUCTOR equals pd.RUTPRODUCTOR
                             where pr.CLIENTEINTERNO == rut && pr.IDPROCESOVENTA == procesoid
                             && pr.TIPOVENTA == "Local"
                             select new ProcesoVentaViewModel()
                             {
                                 IDSUBASTA = sb.IDSUBASTA,
                                 NOMBREPRODUCTOR = pd.NOMBRE,
                                 DIRECCIONCLINICIAL = pd.DIRECCION,
                                 TELEFONOCLI = pd.TELEFONO,
                                 FECHA = pv.FECHA,
                                 TIPOPROCESO = pr.TIPOVENTA

                             }).GroupBy(p => p.NOMBREPRODUCTOR).Select(p => p.FirstOrDefault()).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }
        public ProcesoVentaViewModel GetDatosClientByProcesoVentaLocal(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA equals
                             pr.IDPROCESOVENTA
                             where pv.IDPROCESOVENTA == proceso && pr.CLIENTEINTERNO != null
                             select new ProcesoVentaViewModel()
                             {
                                 IDSUBASTA = sb.IDSUBASTA,
                                 FECHA = pv.FECHA,
                                 RUTCLIENTELOCAL = pr.CLIENTEINTERNO

                             }).FirstOrDefault();

                return query as ProcesoVentaViewModel;
            }
        }

        public CLIENTE GetDatosClienteByRutOfProducto(string rut)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.CLIENTE.Where(cl => cl.RUTCLIENTE == rut).FirstOrDefault();
            }
        }

        public List<PRODUCTO> GetProductosByRutClienteLocalAndProceso(string rutclienteL, decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.CLIENTEINTERNO == rutclienteL && p.IDPROCESOVENTA == proceso).ToList();
            }
        }

        public List<PRODUCTO> GetMyProductsAccepted(USUARIO usuario, decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == proceso && p.ESTADOPROCESO == "Aceptado").ToList();
            }
        }

        public List<PRODUCTO> GetMyProductsAcceptedByListProductor(string rut, decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == rut && p.IDPROCESOVENTA == proceso && p.ESTADOPROCESO == "Aceptado").ToList();
            }
        }

        public List<PRODUCTO> GetProductsAccepted(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPROCESOVENTA == proceso && p.ESTADOPROCESO == "Aceptado").ToList();
            }
        }

        private List<PRODUCTO> GetProductoresByProductsAccepteed(List<PRODUCTO> productos)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return productos.GroupBy(p => p.PRODUCTOR_RUTPRODUCTOR).Select(p => p.FirstOrDefault()).ToList();
            }
        }

        public decimal? TotalSumOfPrecioOfProductorAccordingToOneSell(List<PRODUCTO> productos)
        {
            decimal? total = 0;
            foreach (var item in productos)
            {
                total += item.PRECIO * item.CANTIDAD;
            }

            return total;
        }

        public VENTA GetVentaByProcesoVenta(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.VENTA.Where(v => v.PROCESOVENTA_IDPROCESOVENTA == proceso).FirstOrDefault();
            }
        }

        public VENTA GetMyProfit(List<PRODUCTO> productsAceptados, VENTA venta, decimal? preciTotalP, USUARIO usuario)
        {
            var gananciaTotal = new VENTA();
            decimal? ventaGanancia = 0;
            decimal? costoTranporte = 0;
            decimal? gananciaEmp = 0;
            decimal? aduana = venta.IMPUESTOADUANA / 100;
            decimal? costoAduana = 0;

            var listaByProductor = GetProductoresByProductsAccepteed(productsAceptados);
            if (listaByProductor.Count() > 1)
            {
                costoTranporte = venta.COSTOTRANSPORTE / listaByProductor.Count();
                gananciaEmp = venta.GANANCIA / listaByProductor.Count();
                costoAduana = venta.COSTOTOTAL * aduana / listaByProductor.Count();

                foreach (var item in listaByProductor)
                {
                    if(item.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO)
                    {
                        ventaGanancia = preciTotalP - costoTranporte - gananciaEmp - costoAduana;

                        gananciaTotal.GANANCIAPRODUCTORNETA = preciTotalP;
                        gananciaTotal.COSTOTRANSPORTE = costoTranporte;
                        gananciaTotal.COMISIONEMPRESA = venta.COMISIONEMPRESA;
                        gananciaTotal.GANANCIA = gananciaEmp;
                        gananciaTotal.IMPUESTOADUANA = costoAduana;
                        gananciaTotal.GANANCIATOTAL = ventaGanancia;

                    }
                }
            }
            else
            {
                ventaGanancia = preciTotalP - venta.COSTOTRANSPORTE - venta.GANANCIA - (aduana * preciTotalP);

                gananciaTotal.GANANCIAPRODUCTORNETA = preciTotalP;
                gananciaTotal.COSTOTRANSPORTE = venta.COSTOTRANSPORTE;
                gananciaTotal.GANANCIA = venta.GANANCIA;
                gananciaTotal.IMPUESTOADUANA = venta.IMPUESTOADUANA;
                gananciaTotal.GANANCIATOTAL = ventaGanancia;

            }
            
            return gananciaTotal;
        }

        public List<VENTA> GetMyProfitListToAdmin(List<PRODUCTO> productsAceptados, VENTA venta, decimal? preciTotalP, string rut)
        {
            var gananciaTotal = new List<VENTA>();
            decimal? ventaGanancia = 0;
            decimal? costoTranporte = 0;
            decimal? gananciaEmp = 0;
            decimal? aduana = venta.IMPUESTOADUANA / 100;
            decimal? costoAduana = 0;

            var listaByProductor = GetProductoresByProductsAccepteed(productsAceptados);
            if (listaByProductor.Count() > 1)
            {
                costoTranporte = venta.COSTOTRANSPORTE / listaByProductor.Count();
                gananciaEmp = venta.GANANCIA / listaByProductor.Count();
                costoAduana = venta.COSTOTOTAL * aduana / listaByProductor.Count();

                foreach (var item in listaByProductor)
                {
                    if (item.PRODUCTOR_RUTPRODUCTOR == rut)
                    {
                        ventaGanancia = preciTotalP - costoTranporte - gananciaEmp - costoAduana;

                        gananciaTotal.Add(new VENTA
                        {
                            GANANCIAPRODUCTORNETA = preciTotalP,
                            COSTOTRANSPORTE = costoTranporte,
                            COMISIONEMPRESA = venta.COMISIONEMPRESA,
                            GANANCIA = gananciaEmp,
                            IMPUESTOADUANA = costoAduana,
                            GANANCIATOTAL = ventaGanancia

                        });

                    }
                }
            }
            else
            {
                ventaGanancia = preciTotalP - venta.COSTOTRANSPORTE - venta.GANANCIA - (aduana * preciTotalP);

                gananciaTotal.Add(new VENTA
                {
                    GANANCIAPRODUCTORNETA = preciTotalP,
                    COSTOTRANSPORTE = venta.COSTOTRANSPORTE,
                    GANANCIA = venta.GANANCIA,
                    IMPUESTOADUANA = venta.IMPUESTOADUANA,
                    GANANCIATOTAL = ventaGanancia

                });
               
            }

            return gananciaTotal;
        }

        public IEnumerable<PRODUCTO> GetProductsListMyCompras(USUARIO usuario)
        {
            var listaP = new List<PRODUCTO>();
            var listaConmonto = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                listaP = db.PRODUCTO.Where(p => p.CLIENTEINTERNO == usuario.RUTUSUARIO).ToList();

                foreach (var item in listaP)
                {
                    listaConmonto.Add(new PRODUCTO()
                    {
                        DESCRIPCION = item.DESCRIPCION,
                        PRECIO = item.PRECIO,
                        STOCK = item.STOCK,
                        IDPROCESOVENTA = item.IDPROCESOVENTA,
                        CANTIDAD = item.CANTIDAD,
                        MONTOTOTALPRECIO = item.PRECIO * item.CANTIDAD
                    });
                }
                return listaConmonto.OrderBy(p => p.IDPROCESOVENTA);
            }
        }

        public List<PROCESOVENTA> GetProcesoVentaExternaList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(pv => pv.TIPOPROCESO == "Externo").ToList();
            }
        }

        public List<PROCESOVENTA> GetProcesoVentaLocalListToAdmin()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(pv => pv.TIPOPROCESO == "Local").ToList();
            }
        }

        public List<ProcesoVentaViewModel> GetProductorDatosbyOrdenId(decimal ordenid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pd in db.PRODUCTOR join pr in db.PRODUCTO
                             on pd.RUTPRODUCTOR equals pr.PRODUCTOR_RUTPRODUCTOR
                             where pr.ORDEN_IDORDEN == ordenid
                             && pr.ESTADOPROCESO == "Aceptado"
                             select new ProcesoVentaViewModel
                             {
                                 NOMBREPRODUCTOR = pd.NOMBRE,
                                 DESCRIPCIONP = pr.DESCRIPCION,
                                 PRECIOP = pr.PRECIO,
                                 STOCKP = pr.STOCK,
                                 CANTIDAD = pr.CANTIDAD

                             }).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        public List<ProcesoVentaViewModel> GetProductorDatosbyProcesoId(decimal procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pd in db.PRODUCTOR join pr in db.PRODUCTO
                             on pd.RUTPRODUCTOR equals pr.PRODUCTOR_RUTPRODUCTOR
                             where pr.IDPROCESOVENTA == procesoid
                             && pr.TIPOVENTA == "Local" && pr.CLIENTEINTERNO != null
                             select new ProcesoVentaViewModel
                             {
                                 NOMBREPRODUCTOR = pd.NOMBRE,
                                 NOMBRECLIENTE = pr.CLIENTEINTERNO,
                                 DESCRIPCIONP = pr.DESCRIPCION,
                                 PRECIOP = pr.PRECIO,
                                 STOCKP = pr.STOCK,
                                 CANTIDAD = pr.CANTIDAD,
                                 PROCESO = pr.IDPROCESOVENTA

                             }).ToList();

                return query as List<ProcesoVentaViewModel>;
            }
        }

        public decimal? GetSubastaByProcesoVenta(decimal procesoventaid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.SUBASTA.Where(s => s.PROCESOVENTAID == procesoventaid).FirstOrDefault().IDSUBASTA;
            }
        }

        public decimal? GetCostoTranporteToVenta(decimal? subastaid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.SUBASTAID == subastaid && t.ESTADOSUBASTA == "Aceptado").FirstOrDefault().PRECIO;
            }
        }

        public List<VENTA> GetVentas()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.VENTA.ToList();
            }
        }

        public List<PRODUCTOR> GetProductorByProcesoVentaToGananciaVenta(decimal procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pd in db.PRODUCTOR join pr in
                             db.PRODUCTO on pd.RUTPRODUCTOR equals
                             pr.PRODUCTOR_RUTPRODUCTOR where
                             pr.IDPROCESOVENTA == procesoid &&
                             pr.ESTADOPROCESO == "Aceptado"
                             select pd).ToList();

                return query;
            }
        }

        public PAGO GetPago(decimal? ordenid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PAGO.Where(p => p.ORDEN_IDORDEN == ordenid).FirstOrDefault();
            }
        }
    }
}