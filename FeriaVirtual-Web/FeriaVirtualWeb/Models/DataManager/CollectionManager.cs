﻿using System;
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
                                 FECHA = pv.FECHA
                                 
                             }).ToList();

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

        public ProcesoVentaViewModel GetProcesoByOrden(decimal orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from p in db.PROCESOVENTA
                             where p.ORDENID == orden
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = p.IDPROCESOVENTA,
                                 ORDEN = p.ORDENID
                             });
                return query as ProcesoVentaViewModel;
            }
        }
    }
}