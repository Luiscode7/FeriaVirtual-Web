using System;
using System.Linq;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Utils;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class PagoManager
    {
        public PAGO InsertNewPago(PAGO newPago)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    PAGO pago = new PAGO
                    {
                        IDPAGO = DatabaseUtil.GetNextIDPago(),
                        FECHA = DateTime.Now,
                        TOTAL = newPago.TOTAL,
                        ORDEN_IDORDEN = newPago.ORDEN_IDORDEN
                    };
                    db.PAGO.Add(pago);
                    db.SaveChanges();

                    UpdateRecepcionadoToPagado(pago.ORDEN_IDORDEN);

                    return pago;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void UpdateRecepcionadoToPagado(decimal? ordenid)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    ORDEN orden = db.ORDEN.Where(or => or.IDORDEN == ordenid).FirstOrDefault();
                    orden.ESTADO = "Pagado";
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}