using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Domicilio
    {
        #region Propiedades Privadas
        private int codDomicilio = 0;
        private string calle = string.Empty;
        private string numero = string.Empty;
        private string piso = string.Empty;
        private string departamento = string.Empty;
        private Provincia provincia = new Provincia();
        private Localidad localidad = new Localidad();
        private DateTime fechaCreacion = DateTime.MinValue;
        private DateTime fechaBaja = DateTime.MinValue;
        private DateTime fechaModificacion = DateTime.MinValue;

        #endregion

        #region Propiedades Públicas
        public int CodDomicilio { get { return codDomicilio; } set { codDomicilio = value; } }
        public string Calle { get { return calle; } set { calle = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        public string Piso { get { return piso; } set { piso = value; } }
        public string Departamento { get { return departamento; } set { departamento = value; } }
        public Provincia Provincia { get { return provincia; } set { provincia = value; } }
        public Localidad Local { get { return localidad; } set { localidad = value; } }

        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaBaja { get { return fechaBaja; } set { fechaBaja = value; } }
        public DateTime? FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value.Value; } }
        
        public string Descripcion { get { return Provincia + " - " + Local + " - " + Calle + "  " + Numero + " - " + Piso + " " + Departamento; } }
        #endregion

        public Domicilio() { }

        public override string ToString()
        {
            return Provincia + " - " + Local + " - " + Calle + "  " + Numero + "  " + Piso + " " + Departamento;
        }
    }
}
