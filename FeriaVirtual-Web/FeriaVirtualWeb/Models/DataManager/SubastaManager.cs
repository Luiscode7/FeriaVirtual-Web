using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class SubastaManager
    {
        public TRANSPORTISTA InsertSubastaAccordingTransportista(USUARIO usuario, decimal subasta)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    TRANSPORTISTA trans = db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO).FirstOrDefault();
                    trans.SUBASTAID = subasta;
                    db.SaveChanges();
                    return trans;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal? GetOrderIdByProcesoVentaId(decimal proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.FirstOrDefault(p => p.IDPROCESOVENTA == proceso).ORDENID;
            }
        }
    }
}