using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Localidad
    {
        #region Propiedades Privadas
        private int codLocalidad = 0;
        private string descripcion = string.Empty;
        #endregion


        #region Propiedades Públicas
        public int CodLocalidad { get { return codLocalidad; } set { codLocalidad = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        #endregion

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
