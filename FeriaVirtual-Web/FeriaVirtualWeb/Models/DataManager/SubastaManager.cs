using System;
using System.Linq;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class SubastaManager
    {
        public TRANSPORTISTA InsertSubastaAccordingTransportista(USUARIO usuario, TRANSPORTISTA transportista)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    TRANSPORTISTA trans = db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO && t.IDTRANSPORTISTA == transportista.IDTRANSPORTISTA).FirstOrDefault();
                    TRANSPORTISTA newTrans = new TRANSPORTISTA
                    {
                        IDTRANSPORTISTA = DatabaseUtil.GetNextIDTransportista(),
                        RUTTRANSPORTISTA = trans.RUTTRANSPORTISTA,
                        NOMBRE = trans.NOMBRE,
                        TELEFONO = trans.TELEFONO,
                        TIPOTRANSPORTE = trans.TIPOTRANSPORTE,
                        ANCHO = trans.ANCHO,
                        ALTO = trans.ALTO,
                        LARGO = trans.LARGO,
                        CAPACIDADCARGA = trans.CAPACIDADCARGA,
                        REFRIGERACION = trans.REFRIGERACION,
                        SUBASTAID = transportista.SUBASTAID,
                        PRECIO = transportista.PRECIO,
                        ESTADOSUBASTA = "Pendiente"
                    };
                    db.TRANSPORTISTA.Add(newTrans);
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