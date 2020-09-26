using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Provincia
    {
        #region Propiedades Privadas
        private int codProvincia = 0;
        private string descripcion = string.Empty;
        private List<BE.Localidad> localidad = null;
        #endregion

        #region Propiedades Públicas
        public int CodProvincia { get { return codProvincia; } set { codProvincia = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        /// <summary>
        /// Localidad
        /// </summary>
        public List<BE.Localidad> Local { get { return localidad; } set { localidad = value; } }
        #endregion

        public Provincia() { }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
