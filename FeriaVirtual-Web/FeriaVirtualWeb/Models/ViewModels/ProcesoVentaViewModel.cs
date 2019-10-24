using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.ViewModels
{
    public class ProcesoVentaViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public decimal? PROCESO { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public decimal? ORDEN { get; set; }
        public string NOMBRECLIENTE { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string NOMBREPRODUCTOR { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string PAISCLIENTE { get; set; }
        public string TIPOPROCESO { get; set; }
        public string CLIENTEINICIAL { get; set; }
        public string CLIENTEFINAL { get; set; }
        public string DIRECCIONCLINICIAL { get; set; }
        public string DIRECCIONCLIFINAL { get; set; }
        public string CIUDADCLIENTE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public decimal IDSUBASTA { get; set; }

    }
}