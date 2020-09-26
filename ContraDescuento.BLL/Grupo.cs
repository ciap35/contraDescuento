using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.BLL
{
    public class Grupo
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Grupo grupoDAL = new DAL.Grupo();
        BLL.Permiso permisoNeg = new Permiso();
        List<BE.Grupo> lstGrupo = null;
        List<BE.Grupo> lstGrupoPermiso = new List<BE.Grupo>();
        #endregion

        public Grupo() { }

        public void Alta(ref BE.Grupo grupo)
        {
            try
            {
                grupoDAL.Alta(ref grupo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void Modificar(BE.Grupo grupo)
        {
            try
            {
                grupoDAL.Modificar(grupo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void Baja(BE.Grupo grupo)
        {
            try
            {
                if (ValidarBaja(grupo))
                {
                    grupoDAL.Baja(grupo);
                }
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido realizar la baja del grupo, pues ya hay usuarios con la asignación de este grupo, por favor realice el cambio de grupo por default para el tipo de usuario que posee el grupo actual"));
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public bool ValidarBaja(BE.Grupo grupo)
        {
            try
            {
                return grupoDAL.ValidarBaja(grupo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.Grupo> Listar()
        {
            try
            {
                lstGrupoPermiso = grupoDAL.Listar();
                foreach (BE.Grupo grupo in lstGrupoPermiso)
                {
                    AgregarGruposHijos(grupo);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstGrupoPermiso;
        }

        private void AgregarGruposHijos(BE.Grupo grupo)
        {
            try
            {
                List<BE.GrupoBase> lstGruposHijos = new List<BE.GrupoBase>();
                lstGruposHijos = grupoDAL.Obtener(grupo);
                grupo.LstGrupos = lstGruposHijos;
                foreach (BE.Grupo _grupo in grupo.LstGrupos)
                {
                    AgregarGruposHijos(_grupo);
                }//grupo.LstGrupos.AddRange(permisoNeg.Listar(grupo));
                List<BE.GrupoBase> lstHijos = permisoNeg.Listar(grupo);
                //Evitar duplicados.
                foreach (BE.GrupoBase grupoHijo in lstHijos)
                {
                    if (grupoHijo.EsGrupo)
                    {
                        BE.Grupo _grupoHijo = (BE.Grupo)grupoHijo;
                        bool existeGrupo = false;
                        foreach (BE.GrupoBase grupoItem in lstHijos)
                        {
                            BE.Grupo gr = null;
                            if (grupoItem.EsGrupo)
                                gr = (BE.Grupo)grupoItem;
                            if (gr != null && gr.CodGrupo == _grupoHijo.CodGrupo)
                            {
                                existeGrupo = true;
                            }
                        }
                        if (!existeGrupo)
                        {
                            BE.GrupoBase grupoBaseHijo = (BE.Grupo)grupoHijo;
                            grupo.LstGrupos.Add(grupoBaseHijo);
                        }
                    }
                    else
                    {
                        BE.Permiso _permisoHijo = (BE.Permiso)grupoHijo;
                        bool existePermiso = false;
                        foreach (BE.GrupoBase PermisoItem in lstHijos)
                        {
                            BE.Permiso pr = null;
                            if (!PermisoItem.EsGrupo)
                                pr = (BE.Permiso)PermisoItem;
                            if (pr != null && pr.CodPermiso == _permisoHijo.CodPermiso)
                            {
                                existePermiso = true;
                            }
                        }
                        if (!existePermiso)
                        {
                            BE.GrupoBase permisoBaseHijo = (BE.Permiso)grupoHijo;
                            grupo.LstGrupos.Add(permisoBaseHijo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.GrupoBase> Obtener(BE.Grupo grupo)
        {
            List<BE.GrupoBase> lstGrupo = null;
            try
            {
                lstGrupo = grupoDAL.Obtener(grupo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstGrupo;
        }

        public void AsignarGrupoHijo(BE.Grupo grupo,List<BE.Grupo> lstSubgrupo)
        {
            try
            {
                foreach(BE.Grupo grupoHijo in lstSubgrupo)
                {
                    grupoDAL.AsignarGrupoHijo(grupo, grupoHijo);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void EliminarGrupoHijo(BE.Grupo grupo, List<BE.Grupo> lstSubgrupo)
        {
            try
            {
                foreach (BE.Grupo grupoHijo in lstSubgrupo)
                {
                    grupoDAL.EliminarGrupoHijo(grupo, grupoHijo);
                }
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
