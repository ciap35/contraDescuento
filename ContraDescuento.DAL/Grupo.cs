using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Grupo
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Grupo() { }

        public void Alta(ref BE.Grupo grupo)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", grupo.Descripcion);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["CodGrupo"]);
                        grupo.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Baja(BE.Grupo grupo)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupo.CodGrupo);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarBaja(BE.Grupo grupo)
        {
            bool BajaValida = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoValidarBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupo.CodGrupo);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    
                    while(conexion.sqlReader.Read())
                    {
                        BajaValida = Convert.ToBoolean(conexion.sqlReader[0]);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BajaValida;
        }

        public void Modificar(BE.Grupo grupo)
        {

            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupo.CodGrupo);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", grupo.Descripcion);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BE.GrupoBase> Obtener(BE.Grupo grupo)
        {
            List<BE.GrupoBase> _lstGrupo = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoObtenerHijos";
                    conexion.sqlCmd.Parameters.AddWithValue("@CodGrupo", grupo.CodGrupo);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if(conexion.sqlReader.FieldCount>0)
                        _lstGrupo = new List<BE.GrupoBase>(); 
                    while (conexion.sqlReader.Read())
                    {
                       BE.Grupo _grupo = new BE.Grupo();
                        _grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["CodGrupo"]);
                        _grupo.Descripcion = Convert.ToString(conexion.sqlReader["Descripcion"]);
                        _grupo.EsGrupo = true;
                        _grupo.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            _grupo.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);

                        _lstGrupo.Add(_grupo);

                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _lstGrupo;
        }

        public List<BE.Grupo> Listar()
        {
            List<BE.Grupo> lstGrupo = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstGrupo = new List<BE.Grupo>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.Grupo grupo = new BE.Grupo();
                        grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["CodGrupo"]);
                        grupo.Descripcion = Convert.ToString(conexion.sqlReader["Descripcion"]);
                        grupo.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        grupo.EsGrupo = true;
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            grupo.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);
                        if (conexion.sqlReader["CodPerfilPadre"] != DBNull.Value)
                        {
                            BE.Grupo grupoPadre = new BE.Grupo();
                            grupoPadre.CodGrupo = Convert.ToInt32(conexion.sqlReader["CodPerfilPadre"]);
                        }

                        lstGrupo.Add(grupo);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstGrupo;
        }

      

        public void AsignarGrupoHijo(BE.Grupo grupo, BE.Grupo grupoHijo)
        {
            string mensajeError = string.Empty;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoAsignarHijo";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupoHijo.CodGrupo);
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupoPadre", grupo.CodGrupo);

                    conexion.sqlReader =  conexion.sqlCmd.ExecuteReader();
                    bool error = false;
                    while(conexion.sqlReader.Read())
                    {
                        mensajeError = conexion.sqlReader[0].ToString();
                        error = mensajeError != string.Empty;
                    }
                    conexion.Cerrar();
                    if (error)
                        throw new Exception(mensajeError) ;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarGrupoHijo(BE.Grupo grupo, BE.Grupo grupoHijo)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoEliminarHijo";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupoHijo.CodGrupo);
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupoPadre", grupo.CodGrupo);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Grupo> Listar(BE.Grupo grupo)
        {
            List<BE.Grupo> lstGrupo = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "GrupoListarPorGrupo";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", grupo.CodGrupo);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstGrupo = new List<BE.Grupo>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.Grupo _grupo = new BE.Grupo();
                        _grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["CodGrupo"]);
                        _grupo.Descripcion = Convert.ToString(conexion.sqlReader["Descripcion"]);
                        _grupo.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            _grupo.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);

                        lstGrupo.Add(_grupo);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstGrupo;
        }
    }
}
