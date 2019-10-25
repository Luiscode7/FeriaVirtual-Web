using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.ViewModels
{
    public class SubastaViewModel
    {
        public decimal SUBASTA { get; set; }
        public DateTime FECHA { get; set; }
        public decimal PRECIO { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string PAISCLIENTE { get; set; }
        public List<TRANSPORTISTA> TRANSPORTE { get; set; }
    }
}