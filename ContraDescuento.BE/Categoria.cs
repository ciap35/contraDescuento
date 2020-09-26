using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Categoria
    {
        #region Propiedades Privadas
        private int codCategoria = 0;
        private string descripcion = string.Empty;
        #endregion

        #region Propiedades Públicas
        public int CodCategoria { get { return codCategoria; } set { codCategoria = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        #endregion

        public Categoria() { }
    }
}
