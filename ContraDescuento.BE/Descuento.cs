using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Descuento
    {
        #region Propiedades Privadas
        private int codCupon;
        private BE.Comercio comercio;
        private BE.Producto producto;
        private BE.Domicilio puntoDeVenta;
        private BE.Usuario usuario;
        private string cupon;
        private Int32 cantidad;
        private decimal ahorroTotal;
        private decimal descuento;
        private DateTime fechaCupon;
        private DateTime fechaCanje;

        #endregion

        #region Propiedades Públicas
        public Int32 CodCupon { get { return codCupon; } set { codCupon = value; } }
        public Int32 Cantidad { get { return cantidad; } set { cantidad = value; } }
        public BE.Comercio Comercio { get { return comercio; } set { comercio = value; } }
        public BE.Producto Producto { get { return producto; } set { producto = value; } }
        public BE.Domicilio PuntoDeVenta { get { return puntoDeVenta; } set { puntoDeVenta = value; } }
        public decimal _Descuento { get { return descuento; } set { descuento = value; } }
        public string Cupon { get { return cupon; } set { cupon = value; } }
        public decimal AhorroTotal { get { return ahorroTotal; } set { ahorroTotal = value; } }
        public BE.Usuario Usuario { get { return usuario; } set { usuario = value; } }
        public DateTime FechaCupon { get { return fechaCupon; } set { fechaCupon = value; } }
        public DateTime FechaCanje { get { return fechaCanje; } set { fechaCanje = value; } }
        #endregion

        public Descuento() { }

    }
}
