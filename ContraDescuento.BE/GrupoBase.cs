using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public abstract class GrupoBase
    {
        #region Propiedades Privadas
        private string descripcion = string.Empty;
        private bool esGrupo = false;
        #endregion

        #region Propiedades Públicas
        [Exportable(true)]
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        public bool EsGrupo { get { return esGrupo; } set { esGrupo = value; } }
        #endregion


        public GrupoBase() { }
        public GrupoBase(string descripcion,bool EsGrupo)
        {
            this.Descripcion = descripcion;
            this.EsGrupo = EsGrupo;
        }

      
        public abstract override string ToString();
    }
}
