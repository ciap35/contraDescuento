using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Producto
    {

        #region Propiedades Privadas
        private int codProducto;
        private string titulo;
        private string descripcion;
        private decimal precio;
        private decimal descuento;
        //private int cantidad;
        private byte[] foto;
        private bool mantenerFoto = false;
        private DateTime fechaVigenciaDesde;
        private DateTime fechaVigenciaHasta;
        private DateTime fechaCreacion;
        private DateTime fechaBaja;
        private DateTime fechaModificacion;
        private Dictionary<BE.Domicilio, int> puntosDeVentaStock = new Dictionary<Domicilio, int>();
        private List<BE.Domicilio> puntosDeVenta = new List<Domicilio>();
        private BE.Categoria categoria = null;
        private BE.SubCategoria subCategoria = null;
        #endregion
        
        #region Propiedades Públicas
        public int CodProducto { get { return codProducto; } set { codProducto = value;} }
        public string Titulo { get { return titulo; } set { titulo= value;} }
        public string Descripcion { get { return descripcion; } set { descripcion= value;} }
        public decimal Precio { get { return precio; } set { precio = value; } }
        public decimal Descuento { get { return descuento; } set { descuento = value; } }
        //public int Cantidad { get { return cantidad; } set { cantidad = value; } }
        public byte[] Foto { get { return foto; } set { foto = value; } }
        public bool MantenerFoto { get { return mantenerFoto; } set { mantenerFoto = value; } }
        public DateTime FechaVigenciaDesde { get { return fechaVigenciaDesde; } set { fechaVigenciaDesde = value; } }
        public DateTime FechaVigenciaHasta { get { return fechaVigenciaHasta; } set { fechaVigenciaHasta = value; } }
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaBaja { get { return fechaBaja; } set { fechaBaja = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        public List<BE.Domicilio> PuntosDeVenta { get { return puntosDeVenta; } set { puntosDeVenta = value; } }
        public Dictionary<BE.Domicilio, int> PuntosDeVentaStock { get { return puntosDeVentaStock; } set { puntosDeVentaStock = value; } }
        public BE.Categoria Categoria { get { return categoria; } set { categoria = value; } }
        public BE.SubCategoria SubCategoria { get { return subCategoria; } set { subCategoria = value; } }
        #endregion
        public Producto() { }

        public override string ToString()
        {
            return CodProducto + " " + Descripcion +" "+ FechaCreacion;
        }
    }
}
