using System;
using System.Linq;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class TransportistaManager
    {
        public TRANSPORTISTA UpdateTransporte(TRANSPORTISTA uptrans)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    TRANSPORTISTA trans = db.TRANSPORTISTA.Where(t => t.IDTRANSPORTISTA == uptrans.IDTRANSPORTISTA).FirstOrDefault();
                    trans.TIPOTRANSPORTE = uptrans.TIPOTRANSPORTE;
                    trans.ANCHO = uptrans.ANCHO;
                    trans.ALTO = uptrans.ALTO;
                    trans.LARGO = uptrans.LARGO;
                    trans.CAPACIDADCARGA = uptrans.CAPACIDADCARGA;
                    db.SaveChanges();
                    return trans;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TRANSPORTISTA InsertNewTransporte(TRANSPORTISTA newTrans)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    TRANSPORTISTA trans = new TRANSPORTISTA
                    {
                        IDTRANSPORTISTA = DatabaseUtil.GetNextIDTransportista(),
                        RUTTRANSPORTISTA = newTrans.RUTTRANSPORTISTA,
                        NOMBRE = newTrans.NOMBRE,
                        TELEFONO = newTrans.TELEFONO,
                        TIPOTRANSPORTE = newTrans.TIPOTRANSPORTE,
                        ANCHO = newTrans.ANCHO,
                        ALTO = newTrans.ALTO,
                        LARGO = newTrans.LARGO,
                        CAPACIDADCARGA = newTrans.CAPACIDADCARGA,
                        REFRIGERACION = newTrans.REFRIGERACION
                    };
                    db.TRANSPORTISTA.Add(trans);
                    db.SaveChanges();
                    return trans;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}