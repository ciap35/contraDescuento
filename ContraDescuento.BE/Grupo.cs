using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Grupo : GrupoBase
    {
        #region Propiedades Privadas
        private int codGrupo = 0;
        private DateTime fechaCreacion;
        private DateTime fechaModificacion;
        private List<GrupoBase> lstGrupos = new List<GrupoBase>();

        #endregion

        #region Propiedades Públicas
        [Exportable(true)]
        public int CodGrupo { get { return codGrupo; } set { codGrupo = value; } }
        
        [Exportable(true)]
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        public List<GrupoBase> LstGrupos { get { return lstGrupos; } set { lstGrupos = value; } }
        #endregion

        public Grupo() { }
        public Grupo(string descripcion,bool esGrupo): base(descripcion,esGrupo)
        {
            this.Descripcion = descripcion;
            this.EsGrupo = esGrupo;
        }

        public override string ToString()
        {
            return CodGrupo + " - " + Descripcion;
        }
    }
}
