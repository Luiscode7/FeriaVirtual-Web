using System;
using System.ComponentModel.DataAnnotations;

namespace FeriaVirtualWeb.Models.ViewModels
{
    public class ProcesoVentaViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name ="N° PROCESO")]
        public decimal? PROCESO { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public decimal? ORDEN { get; set; }
        [Display(Name = "CLIENTE")]
        public string NOMBRECLIENTE { get; set; }
        [Display(Name = "PRODUCTOR")]
        public string NOMBREPRODUCTOR { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FECHA { get; set; }
        public string RUTCLIENTELOCAL { get; set; }
        public string ESTADO { get; set; }
        [Display(Name = "PAIS DESTINO")]
        public string PAISCLIENTE { get; set; }
        [Display(Name = "PROCESO")]
        public string TIPOPROCESO { get; set; }
        public string CLIENTEINICIAL { get; set; }
        [Display(Name = "DIRECCION")]
        public string CLIENTEFINAL { get; set; }
        [Display(Name = "DIRECCION")]
        public string DIRECCIONCLINICIAL { get; set; }
        public string DIRECCIONCLIFINAL { get; set; }
        public string CIUDADCLIENTE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "N° SUBASTA")]
        public decimal IDSUBASTA { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "FECHA")]
        public DateTime FECHASUBASTA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C0}")]
        public decimal? PRECIO { get; set; }
        public string TIPOTRANSPORTE { get; set; }
        [Display(Name = "ESTADO")]
        public string ESTADOSUBASTA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "TELEFONO")]
        public string TELEFONOCLI { get; set; }

    }
}