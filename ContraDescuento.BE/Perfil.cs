using System;
using System.Collections.Generic;
using System.Text;

namespace ContraDescuento.BE
{
    public class Perfil
    {
        #region Propiedades Privadas
        private int codPerfil = 0;
        private string descripcion = string.Empty;
        private bool admin = false;
        private DateTime fechaCreacion;
        private DateTime fechaModificacion;
        private Perfil _perfil = null;
        private List<Permiso> permisos = new List<Permiso>();

        #endregion

        #region Propiedades Públicas
        public int CodPerfil { get { return codPerfil; } set { codPerfil = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        public bool Admin { get { return admin; } set { admin = value; } }
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value; } }
        public Perfil _Perfil { get { return _perfil; } set { _perfil = value; } }
        public List<Permiso> Permisos { get { return permisos; } set { permisos = value; } }
        #endregion
        public Perfil() { }

        public Perfil(int _codPerfil)
        {
            if (this._Perfil != null)
                this._Perfil.codPerfil = _codPerfil;
        }
        public override string ToString()
        {
            return CodPerfil + " - " + Descripcion;
        }
    }
}
