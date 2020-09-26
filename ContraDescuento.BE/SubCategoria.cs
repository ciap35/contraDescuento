using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class SubCategoria
    {
        #region Propiedades Privadas
        private int codSubCategoria = 0;
        private string descripcion = string.Empty;
        #endregion

        #region Propiedades Públicas
        public int CodSubCategoria { get { return codSubCategoria; } set { codSubCategoria = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        #endregion

        public SubCategoria() { }
    }
}
