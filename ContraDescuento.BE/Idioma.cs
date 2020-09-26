using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Idioma
    {
        #region Propiedades Privadas
        private int codIdioma = 0;
        private string descripcion = string.Empty;
        private string charset = string.Empty;
        private bool porDefecto = false;
        private DateTime fechaCreacion = DateTime.MinValue;
        private DateTime fechaModificacion = DateTime.MinValue;
        #endregion

        #region Propiedades Públicas
        [Exportable(true)]
        public int CodIdioma { get { return codIdioma; } set { codIdioma = value; } }
        public string Charset { get { return charset; } set { charset = value; } }
        [Exportable(true)]
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        public bool PorDefecto { get { return porDefecto; } set { porDefecto = value; } }
        [Exportable(true)]
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        #endregion

        public Idioma() { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
