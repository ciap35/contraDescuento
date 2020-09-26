using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Telefono
    {
        #region Propiedades Públicas
        private int codTelefono = 0;
        private string caracteristica = string.Empty;
        private bool celular = false;
        private string nroTelefono = string.Empty;
        private string observacion = string.Empty;
        private DateTime fechaCreacion = DateTime.MinValue;
        private DateTime fechaBaja = DateTime.MinValue;
        private DateTime fechaModificacion = DateTime.MinValue;
        #endregion

        #region Propiedades Privadas
        public int CodTelefono { get { return codTelefono; } set { codTelefono = value; } }
        public string Caracteristica { get { return caracteristica; } set { caracteristica = value; } }
        public string NroTelefono { get { return nroTelefono; } set { nroTelefono = value; } }
        public bool Celular { get { return celular; } set { celular = value; } }
        public string Observacion { get { return observacion; } set { observacion = value; } }
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaBaja { get { return fechaBaja; } set { fechaBaja = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        #endregion

        public Telefono() { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
