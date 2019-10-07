using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriaVirtualWeb.Utils
{
    public class ResponseModel
    {
        public dynamic Result { get; set; }
        public bool Response { get; set; }
        public string Message { get; set; }
        public string Href { get; set; }
        public string Function { get; set; }

        public ResponseModel()
        {
            Response = false;
            Message = "Ha ocurrido un Error Inesperado";
        }

        public void SetResponse(bool r, string m = "")
        {
            Response = r;
            Message = m;

            if(!r && m == "")
            {
                Message = "Ha Ocurrido un Error Inesperado";
            }
        }
    }
}