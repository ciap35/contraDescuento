using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.BLL
{
    public class TipoUsuario
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.TipoUsuario tipoUsuarioDAL = new DAL.TipoUsuario();
        #endregion

        public TipoUsuario() { }

        public BE.TipoUsuario Obtener(BE.TipoUsuario tipoUsuario)
        {
            BE.TipoUsuario _tipoUsuario = null;
            try
            {
                if (tipoUsuario != null)
                {
                    _tipoUsuario = tipoUsuario;
                    if (_tipoUsuario.CodTipoUsuario == 0)
                        tipoUsuarioDAL.Obtener(ref _tipoUsuario);
                    else
                        tipoUsuarioDAL.ObtenerPorCodigoTipoUsuario(ref _tipoUsuario);
                    //if (tipoUsuario.Grupo == null)
                    //    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha obtenido el grupo para el tipo de usuario indicado" + tipoUsuario.CodTipoUsuario.ToString()));
                    if (tipoUsuario.Grupo != null)
                    {
                        BLL.Grupo grupoNeg = new BLL.Grupo();

                        List<BE.Grupo> lstGrupos = grupoNeg.Listar();
                        foreach (BE.Grupo grupo in lstGrupos)
                        {
                            if (tipoUsuario.Grupo != null && grupo.CodGrupo == tipoUsuario.Grupo.CodGrupo)
                            {
                                tipoUsuario.Grupo = grupo;
                            }
                        }
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return _tipoUsuario;
        }

        public List<BE.TipoUsuario> Listar()
        {
            List<BE.TipoUsuario> lstTipoUsuario = null;
            try
            {
                lstTipoUsuario= tipoUsuarioDAL.Listar();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstTipoUsuario;
        }
        public void ActualizarMapeo(BE.Grupo grupoUSuario, BE.Grupo grupoComercio, BE.Grupo grupoOtros)
        {
            try
            {
                if (grupoUSuario.CodGrupo == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo USUARIO"));
                if (grupoComercio.CodGrupo == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo COMERCIO"));
                if (grupoOtros.CodGrupo == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo OTROS"));


                tipoUsuarioDAL.ActualizarMapeo(grupoUSuario, grupoComercio, grupoOtros);
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
