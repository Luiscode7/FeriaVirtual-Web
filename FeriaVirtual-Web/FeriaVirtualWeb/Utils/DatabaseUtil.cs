using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Utils
{
    public class DatabaseUtil
    {
        public static decimal GetNextID()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.PRODUCTO.Max(p => p.IDPRODUCTO);
                var nextID = lastID + 1;
                return nextID;
            }
        }
    }
}