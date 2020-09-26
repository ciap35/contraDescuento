using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class ExcepcionPersonalizada : System.Exception
    {
        private BE.Bitacora exception = null;
        public ExcepcionPersonalizada(bool ExControlada, Exception ex) : base(String.Format("{0}", ex.Message))
        {
            exception = new BE.Bitacora(ex);
            exception.ExcepcionControlada = ExControlada;
        }

        public BE.Bitacora obtenerExcepcion()
        {
            return exception;
        }
    }
}
