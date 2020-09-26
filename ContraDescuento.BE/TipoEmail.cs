using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
   public class TipoEmail
    {
        private int codTipoEmail;
        private string descripcion;

        public int CodTipoEmail { get { return codTipoEmail; } set { codTipoEmail = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }

        public TipoEmail() { }
    }
}
