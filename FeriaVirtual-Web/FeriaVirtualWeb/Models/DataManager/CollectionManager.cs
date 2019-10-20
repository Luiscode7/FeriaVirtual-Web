using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).OrderBy(p => p.IDPRODUCTO).ToList();
            }

        }

        public List<PRODUCTO> GetProductsSelected(List<PRODUCTO> products)
        {
            return products.Where(p => p.IsChecked == true).ToList();
        }

        public IEnumerable<PRODUCTO> GetProductosListSelected(int id)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPRODUCTO == id).ToList();
            }
        }

        public List<PRODUCTO> GetProductosListToOrders()
        {
            var nuevaLista = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in GetProductosList())
                {
                    nuevaLista.Add(new PRODUCTO()
                    {
                        IDPRODUCTO = item.IDPRODUCTO,
                        DESCRIPCION = item.DESCRIPCION
                    });
                }

                return nuevaLista;
            }
        }

        public List<ORDEN> GetMyOrderList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.ORDEN.Where(p => p.CLIENTE_RUTCLIENTE == usuario.RUTUSUARIO).OrderBy(p => p.IDORDEN).ToList();
            }
        }

        public CLIENTE GetClienteToProcesoVenta(decimal orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from or in db.ORDEN
                             join cl in db.CLIENTE
                             on or.CLIENTE_RUTCLIENTE equals cl.RUTCLIENTE
                             where or.IDORDEN == orden
                             select cl.NOMBRE);

                return query as CLIENTE;
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
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pv.IDPROCESOVENTA,
                                 ORDEN = pv.ORDENID,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 FECHA = pv.FECHA,
                                 ESTADO = pv.ESTADO
                                 
                             }).OrderBy(p => p.ORDEN).ToList();

                return query as IEnumerable<ProcesoVentaViewModel>;
            }
        }

        public List<PRODUCTO> GetProductClientByOrder(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden).ToList();
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
                    productosP = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.TIPOVENTA == "Externo").FirstOrDefault();
                    if(productosP != null)
                    {
                        newList.Add(new PRODUCTO
                        {
                            IDPRODUCTO = productosP.IDPRODUCTO,
                            DESCRIPCION = productosP.DESCRIPCION,
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
                newProducto = db.PRODUCTO.Where(p => p.DESCRIPCION == producto.DESCRIPCION && p.TIPOVENTA == producto.TIPOVENTA && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).FirstOrDefault();
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

        public List<TRANSPORTISTA> GetTransporte(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO).ToList();
            }
        }

        public List<SUBASTA> GetSubasta()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.SUBASTA.ToList();
            }
        }
    }
}