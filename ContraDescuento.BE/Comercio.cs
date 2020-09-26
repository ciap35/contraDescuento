using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Comercio
    {
        #region Propiedades Privadas
        private int codComercio = 0;
        private string nombreComercio = string.Empty;
        private string descripcion = string.Empty;
        private byte[] logo = null;
        private bool mantenerLogo = false;
        private BE.Usuario responsable = new BE.Usuario();
        private BE.Telefono telefono = new BE.Telefono();
        private List<BE.Domicilio> puntoDeVenta = new List<BE.Domicilio>();
        private List<BE.Producto> lstProducto = new List<Producto>();
        private DateTime fechaCreacion = DateTime.MinValue;
        private DateTime fechaBaja = DateTime.MinValue;
        private DateTime fechaModificacion = DateTime.MinValue;
        #endregion

        #region Propiedades Públicas
        public int CodComercio { get { return codComercio; } set { codComercio = value; } }
        public string NombreComercio { get { return nombreComercio; } set { nombreComercio = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        public byte[] Logo { get { return logo; } set { logo = value; } }
        public bool MantenerLogo { get { return mantenerLogo; } set { mantenerLogo = value; } }
        public BE.Usuario Responsable { get { return responsable; } set { responsable = value; } }
        public BE.Telefono Telefono { get { return telefono; } set { telefono = value; } }
        public List<BE.Domicilio> PuntoDeVenta { get { return puntoDeVenta; } set { puntoDeVenta = value; } }
        public List<BE.Producto> LstProducto { get { return lstProducto; } set { lstProducto = value; } }
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaBaja { get { return fechaBaja; } set { fechaBaja = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        #endregion

        public Comercio() { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
