using System.Linq;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Utils
{
    public class DatabaseUtil
    {
        public static decimal GetNextIDProducto()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.PRODUCTO.DefaultIfEmpty().Max(p => p == null ? 0 : p.IDPRODUCTO);
                var nextID = lastID + 1;
                return nextID;
            }
        }

        public static decimal GetNextIDOrder()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.ORDEN.DefaultIfEmpty().Max(p => p ==null ? 0 : p.IDORDEN);
                var nextID = lastID + 1;
                return nextID;            
            }
        }

        public static decimal GetNextIDContacto()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.CONTACTO.DefaultIfEmpty().Max(p => p == null ? 0: p.IDCONTACTO); 
                var nextID = lastID + 1;
                return nextID;
            }
        }

        public static decimal GetNextIDTransportista()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lastID = db.TRANSPORTISTA.DefaultIfEmpty().Max(p => p == null ? 0 : p.IDTRANSPORTISTA);
                var nextID = lastID + 1;
                return nextID;
            }
        }
    }
}