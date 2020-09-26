using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Permiso
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Permiso() { }


        public void Alta(ref BE.Permiso permiso)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", DBNull.Value);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", permiso.Descripcion);
                    conexion.sqlCmd.Parameters.AddWithValue("@pagina", permiso.URL);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        permiso.CodPermiso = (int)conexion.sqlReader["codPermiso"];
                        permiso.FechaCreacion = (DateTime)conexion.sqlReader["fechaCreacion"];
                    };
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Asignar(BE.Grupo grupo, BE.Permiso permiso)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoAsignar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codPermiso", permiso.CodPermiso);
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

        public void Desasignar(BE.Grupo grupo, BE.Permiso permiso)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoDesasignar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codPermiso", permiso.CodPermiso);
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

        public void Baja(BE.Permiso permiso)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@CodPermiso", permiso.CodPermiso);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Permiso permiso)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codPermiso", permiso.CodPermiso);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", permiso.Descripcion);
                    conexion.sqlCmd.Parameters.AddWithValue("@pagina", permiso.URL);
                    
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public List<BE.Permiso> Listar()
        {
            BE.Permiso permiso = null;
            List<BE.Permiso> lstPermisos = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoListar";
                    conexion.sqlCmd.Parameters.AddWithValue("@CodGrupo", DBNull.Value);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstPermisos = new List<BE.Permiso>();

                    while (conexion.sqlReader.Read())
                    {
                        permiso = new BE.Permiso()
                        {
                            CodPermiso = (int)conexion.sqlReader["CodPermiso"],
                            Descripcion = conexion.sqlReader["Descripcion"].ToString(),
                            URL = conexion.sqlReader["URL"].ToString(),
                            FechaCreacion = conexion.sqlReader["FechaCreacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaCreacion"] : DateTime.MinValue,
                            FechaModificacion = conexion.sqlReader["FechaModificacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaModificacion"] : DateTime.MinValue

                        };
                        lstPermisos.Add(permiso);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstPermisos;
        }
        /// <summary>
        /// Método para listar todos los perfiles
        /// </summary>
        /// <returns></returns>
        public List<BE.GrupoBase> Listar(BE.Grupo grupo)
        {
            BE.Permiso permiso = null;  
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PermisoListar";
                    if (grupo == null)
                    {
                        grupo = new BE.Grupo();
                        conexion.sqlCmd.Parameters.AddWithValue("@CodGrupo", DBNull.Value);
                    }
                    else { 
                    conexion.sqlCmd.Parameters.AddWithValue("@CodGrupo", grupo.CodGrupo);
                    }
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        permiso = new BE.Permiso()
                        {
                            CodPermiso = (int)conexion.sqlReader["CodPermiso"],
                            Descripcion = conexion.sqlReader["Descripcion"].ToString(),
                            URL = conexion.sqlReader["URL"].ToString(),
                            FechaCreacion = conexion.sqlReader["FechaCreacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaCreacion"] : DateTime.MinValue,
                            FechaModificacion = conexion.sqlReader["FechaModificacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaModificacion"] : DateTime.MinValue

                        };
                        grupo.LstGrupos.Add(permiso);
                    }
                    conexion.Cerrar();
                };
                return grupo.LstGrupos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Permiso Obtener(BE.Permiso permiso)
        {
            try
            {
                conexion = new Acceso.Conexion();
                conexion.Abrir();
                conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                conexion.sqlCmd.CommandText = "PermisoObtener";
                conexion.sqlCmd.Parameters.AddWithValue("@CodPermiso", permiso.CodPermiso);
                conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                while (conexion.sqlReader.Read())
                {
                    permiso = new BE.Permiso()
                    {
                        CodPermiso = (int)conexion.sqlReader["CodPermiso"],
                        Descripcion = conexion.sqlReader["Descripcion"].ToString(),
                        URL = conexion.sqlReader["URL"].ToString(),
                        FechaCreacion = conexion.sqlReader["FechaCreacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaCreacion"] : DateTime.MinValue,
                        FechaModificacion = conexion.sqlReader["FechaModificacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaModificacion"] : DateTime.MinValue
                    };
                }
                conexion.Cerrar();
                return permiso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
