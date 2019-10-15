using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Utils
{
    public class DatabaseUtil
    {
        public static decimal GetNextIDProducto()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.PRODUCTO.Max(p => p.IDPRODUCTO);
                var nextID = lastID + 1;
                return nextID;
            }
        }

        public static decimal GetNextIDOrder()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.ORDEN.Max(p => p.IDORDEN);
                var nextID = lastID + 1;
                return nextID;            
            }
        }
    }
}