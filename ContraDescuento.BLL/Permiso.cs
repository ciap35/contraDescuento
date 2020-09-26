using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Permiso
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        public bool PeticionValida = false;
        #endregion

        public Permiso() { }

        public void Alta(BE.Permiso permiso, BE.Grupo grupo)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al dar de alta el permiso en el gurpo"));

                permisoDAL.Alta(ref permiso);
                Asignar(grupo, permiso);
            }
            catch(BE.ExcepcionPersonalizada ex)
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
        }
        public void Alta(ref BE.Permiso permiso)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                if (permiso.Descripcion == string.Empty || permiso.URL == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor, complete los datos requeridos para el permiso"));

                permisoDAL.Alta(ref permiso);
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
        }
        public void Baja(BE.Permiso permiso, BE.Grupo grupo)
        {
            try
            {
                DAL.Permiso permisoDAL = new DAL.Permiso();

                if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al dar la baja del permiso en el gurpo"));

                permisoDAL.Baja(permiso);
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
        }
        public void Baja(BE.Permiso permiso)
        {
            try
            {
                DAL.Permiso permisoDAL = new DAL.Permiso();

                if (permiso.CodPermiso <= 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al dar de baja el permiso"));

                permisoDAL.Baja(permiso);
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
        }
        public void Modificar(BE.Permiso permiso)
        {
            try
            {
                DAL.Permiso permisoDAL = new DAL.Permiso();
                if (permiso == null || permiso.Descripcion == string.Empty || permiso.URL == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete los datos del permiso"));
                permisoDAL.Modificar(permiso);
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
        }
        public void Asignar(BE.Grupo grupo, BE.Permiso permiso)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al asignar el permiso al grupo"));

                permisoDAL.Asignar(grupo, permiso);
            }
            catch(BE.ExcepcionPersonalizada ex)
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
        }

        public void Asignar(BE.Grupo grupo, List<BE.Permiso> lstPermiso)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                foreach (BE.Permiso permiso in lstPermiso)
                {
                    if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                        throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al asignar el permiso al grupo"));
                    else
                        permisoDAL.Asignar(grupo, permiso);
                }

            }
            catch(BE.ExcepcionPersonalizada ex)
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
        }

        public void Desasignar(BE.Grupo grupo, BE.Permiso permiso)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al desasignar el permiso al grupo"));

                permisoDAL.Desasignar(grupo, permiso);
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
        }
        public void Desasignar(BE.Grupo grupo, List<BE.Permiso> LstPermisos)
        {
            DAL.Permiso permisoDAL = new DAL.Permiso();
            try
            {
                foreach (BE.Permiso permiso in LstPermisos)
                {
                    if (permiso.CodPermiso <= 0 || grupo.CodGrupo <= 0)
                        throw new BE.ExcepcionPersonalizada(true, new Exception("Ocurrió un error al desasignar el permiso al grupo"));
                    else
                        permisoDAL.Desasignar(grupo, permiso);
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
        }
        public List<BE.GrupoBase> Listar(BE.Grupo grupo)
        {
            try
            {
                DAL.Permiso permiso = new DAL.Permiso();
                if (grupo == null)
                    return permiso.Listar(null);
                else
                    return permiso.Listar(grupo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public List<BE.Permiso> Listar()
        {
            List<BE.Permiso> lstPermisos = null;
            try
            {
                DAL.Permiso permisoDAL = new DAL.Permiso();
                lstPermisos = permisoDAL.Listar();

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstPermisos;
        }
        public BE.Permiso Obtener(BE.Permiso permiso)
        {
            try
            {
                DAL.Permiso permisoDAL = new DAL.Permiso();
                return permisoDAL.Obtener(permiso);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public bool ValidarSolicitud(string URL, BE.Grupo grupo)
        {
            try
            {
                foreach (BE.GrupoBase _grupo in grupo.LstGrupos)
                {
                    if (!_grupo.EsGrupo)
                    {
                        if (((BE.Permiso)_grupo).URL.ToUpper() == URL.ToUpper())
                            PeticionValida = true;
                    }
                    else
                    {
                        ValidarSolicitud(URL, ((BE.Grupo)_grupo));
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return PeticionValida;
        }
    }
}
