using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

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
                    trans.REFRIGERACION = uptrans.REFRIGERACION;
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