using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Perfil
    {
        #region Propiedades Privadas
        DAL.Bitacora exceptionLogger = new DAL.Bitacora();
        #endregion

        public Perfil() { }

        public void Alta(BE.Perfil perfil)
        {
            try {
                DAL.Perfil perfilDAL = new DAL.Perfil();
                perfilDAL.Alta(perfil);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Baja(BE.Perfil perfil)
        {
            try {
                DAL.Perfil perfilDAL = new DAL.Perfil();
                perfilDAL.Baja(perfil);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Modificar(BE.Perfil perfil)
        {
            try {
                DAL.Perfil perfilDAL = new DAL.Perfil();
                perfilDAL.Modificar(perfil);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.Perfil> Listar()
        {
            try {
                DAL.Perfil perfilDAL = new DAL.Perfil();
                DAL.Permiso permiso = new DAL.Permiso();
                List<BE.Perfil> lstPerfil = new List<BE.Perfil>();
                lstPerfil = perfilDAL.Listar(true);
                foreach(BE.Perfil perfilItem in lstPerfil)
                {
                    perfilItem.Permisos.AddRange(permiso.Listar(perfilItem));
                }
                return lstPerfil;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        private List<BE.Perfil> ObtenerPerfiles(BE.Perfil perfil)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }
}
