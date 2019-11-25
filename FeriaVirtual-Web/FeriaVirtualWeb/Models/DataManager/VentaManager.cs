﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.ViewModels;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class VentaManager
    {
        public VENTA InsertNewVenta(VentaViewModel newVenta)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    VENTA venta = new VENTA
                    {
                        IDVENTA = DatabaseUtil.GetNextIDVenta(),
                        FECHA = newVenta.FECHA,
                        IMPUESTOADUANA = newVenta.IMPUESTOADUANA,
                        COSTOTRANSPORTE = newVenta.COSTOTRANSPORTE,
                        COMISIONEMPRESA = newVenta.COMISIONEMPRESA,
                        PROCESOVENTA_IDPROCESOVENTA = newVenta.PROCESOVENTA_IDPROCESOVENTA
                    };
                    db.VENTA.Add(venta);
                    db.SaveChanges();
                    return venta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VENTA GetAndInsertDetalisVenta(VENTA ventaDet)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    var ordenid = GetOrdenIdByProcesoID(ventaDet.PROCESOVENTA_IDPROCESOVENTA);
                    var productosOrden = GetProductByOrden(ordenid);
                    var productos = GetProductsProductorAccordingProductosOrden(productosOrden);
                    decimal? costoTotal = 0;
                    foreach (var item in productos)
                    {
                        costoTotal += item.PRECIO * item.CANTIDAD;
                    }

                    VENTA venta = db.VENTA.Where(v => v.IDVENTA == ventaDet.IDVENTA).FirstOrDefault();
                    venta.COSTOTOTAL = costoTotal;
                    db.SaveChanges();

                    return venta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal? GetOrdenIdByProcesoID(decimal? procesoid)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.IDPROCESOVENTA == procesoid).FirstOrDefault().ORDENID;
            }
        }

        private List<PRODUCTO> GetProductByOrden(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.PRECIO == null).ToList();
            }
        }

        private List<PRODUCTO> GetProductsProductorAccordingProductosOrden(List<PRODUCTO> productos)
        {
            var productosP = new List<PRODUCTO>();
            var newList = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    productosP = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION
                    && p.IDPROCESOVENTA == item.IDPROCESOVENTA && p.PRECIO != null && p.TIPOVENTA == "Externo"
                    && p.ESTADOPROCESO == "Aceptado").ToList();
                    if (productosP.Count() != 0)
                    {
                        foreach (var item2 in productosP)
                        {
                            newList.Add(new PRODUCTO
                            {
                                IDPRODUCTO = item2.IDPRODUCTO,
                                DESCRIPCION = item2.DESCRIPCION,
                                PRECIO = item2.PRECIO,
                                STOCK = item2.STOCK,
                                CANTIDAD = item2.CANTIDAD,
                                PRODUCTOR_RUTPRODUCTOR = item2.PRODUCTOR_RUTPRODUCTOR
                            });
                        }
                        
                    }

                }
                return newList;
            }
        }
    }
}