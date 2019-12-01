using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeriaVirtualWeb.Models.ViewModels
{
    public class VentaViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "N° VENTA")]
        public decimal IDVENTA { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FECHA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C0}")]
        [Display(Name = "COSTO ADUANA (%)")]
        public decimal? IMPUESTOADUANA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "COSTO TRANSPORTE")]
        public decimal? COSTOTRANSPORTE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}%")]
        [Display(Name = "COMISION EMPRESA (%)")]
        public decimal? COMISIONEMPRESA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C0}")]
        [Display(Name = "COSTO TOTAL")]
        public decimal? COSTOTOTAL { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C0}")]
        [Display(Name = "GANANCIA TOTAL")]
        public decimal? GANANCIA { get; set; }
        public string ESTADO { get; set; }
        public string EMPRESA_RUTEMPRESA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "N° PROCESO VENTA")]
        public decimal? PROCESOVENTA_IDPROCESOVENTA { get; set; }

    }
}