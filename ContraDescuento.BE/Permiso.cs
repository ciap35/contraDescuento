using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Permiso : GrupoBase
    {
        #region Propiedades Privadas
        private int codPermiso = 0;
        private string descripcion = string.Empty;
        private string url = string.Empty;
        private DateTime fechaCreacion;
        private DateTime fechaModificacion;
        #endregion

        #region Propiedades Públicas
        [Exportable(true)]
        public int CodPermiso { get { return codPermiso; } set { codPermiso = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        [Exportable(true)]
        public string URL { get { return url; } set { url = value; } }
        [Exportable(true)]
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        #endregion

        public Permiso() { }

        public Permiso(string descripcion, bool esGrupo) : base(descripcion, esGrupo)
        {
            this.Descripcion = descripcion;
            this.EsGrupo = EsGrupo;
        }

        public Permiso(string descripcion,string url,bool esGrupo) : base(descripcion,esGrupo) {
            this.Descripcion = descripcion;
            this.EsGrupo = EsGrupo;
            this.URL = url;
        }

       

        public override string ToString()
        {
            return CodPermiso+ " - " + Descripcion + URL;
        }
    }
}
