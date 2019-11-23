using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class AdministradorManager
    {
        public string UpdateEstadoTransporteToAccept(decimal transportista)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    TRANSPORTISTA trans = db.TRANSPORTISTA.Where(t => t.IDTRANSPORTISTA == transportista).FirstOrDefault();
                    trans.ESTADOSUBASTA = "Aceptado";
                    db.SaveChanges();

                    List<TRANSPORTISTA> lista = db.TRANSPORTISTA.Where(tr => tr.SUBASTAID == trans.SUBASTAID && tr.ESTADOSUBASTA != "Aceptado").ToList();

                    foreach (var item in lista)
                    {
                        TRANSPORTISTA transRechazado = db.TRANSPORTISTA.Where(tr => tr.IDTRANSPORTISTA == item.IDTRANSPORTISTA).FirstOrDefault();
                        transRechazado.ESTADOSUBASTA = "Rechazado";
                        db.SaveChanges();
                    }
                    return trans.ESTADOSUBASTA;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}