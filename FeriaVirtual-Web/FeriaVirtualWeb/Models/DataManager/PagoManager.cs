using System;
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

                    return pago;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}