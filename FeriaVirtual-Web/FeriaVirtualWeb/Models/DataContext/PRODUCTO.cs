//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FeriaVirtualWeb.Models.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUCTO
    {
        public PRODUCTO()
        {
            this.ORDEN = new HashSet<ORDEN>();
        }
    
        public decimal IDPRODUCTO { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<decimal> PRECIO { get; set; }
        public Nullable<decimal> STOCK { get; set; }
        public string PRODUCTOR_RUTPRODUCTOR { get; set; }
    
        public virtual ICollection<ORDEN> ORDEN { get; set; }
        public virtual PRODUCTOR PRODUCTOR { get; set; }
    }
}
