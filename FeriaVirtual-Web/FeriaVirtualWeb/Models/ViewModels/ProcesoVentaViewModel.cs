using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeriaVirtualWeb.Models.ViewModels
{
    public class ProcesoVentaViewModel
    {
        public decimal PROCESO { get; set; }
        public decimal? ORDEN { get; set; }
        public string NOMBRECLIENTE { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FECHA { get; set; }
        public bool SELECTED { get; set; } 
    }
}