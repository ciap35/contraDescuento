using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Perfil
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Perfil() { }

        public void Alta(BE.Perfil perfil)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PerfilAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", perfil.Descripcion);
                    if (perfil._Perfil != null && perfil._Perfil.CodPerfil > 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@codPerfilPadre", perfil._Perfil.CodPerfil);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codPerfilPadre", DBNull.Value);
                    conexion.sqlCmd.Parameters.AddWithValue("@admin", perfil.Admin);

                    conexion.sqlCmd.ExecuteNonQuery();
                   
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Perfil perfil)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PerfilBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codPerfil", perfil.CodPerfil);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Perfil perfil)
        {
            try
            {
                try
                {
                    using (conexion = new Acceso.Conexion())
                    {
                        conexion.Abrir();
                        conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        conexion.sqlCmd.CommandText = "PerfilModificar";
                        conexion.sqlCmd.Parameters.AddWithValue("@codPerfil", perfil.CodPerfil);
                        conexion.sqlCmd.Parameters.AddWithValue("@descripcion", perfil.Descripcion);
                        if(perfil._Perfil != null && perfil._Perfil.CodPerfil>0)
                            conexion.sqlCmd.Parameters.AddWithValue("@codPerfilPadre", perfil._Perfil.CodPerfil);
                        else
                            conexion.sqlCmd.Parameters.AddWithValue("@codPerfilPadre", DBNull.Value);

                        conexion.sqlCmd.Parameters.AddWithValue("@admin", perfil.Admin);


                        conexion.sqlCmd.ExecuteNonQuery();
                        conexion.Cerrar();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Método para listar todos los perfiles
        /// </summary>
        /// <param name="soloVigentes">Determina si se obtienen todos los perfiles (incluidos dados de baja) o sólo los vigentes</param>
        /// <returns></returns>
        public List<BE.Perfil> Listar(bool soloVigentes)
        {
            BE.Perfil perfil = null;
            BE.Permiso permiso = null;
            List<BE.Perfil> lstPerfil = new List<BE.Perfil>();
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PerfilListar";
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        perfil = new BE.Perfil()
                        {
                            CodPerfil = (int)conexion.sqlReader["CodPerfil"],
                            Descripcion = conexion.sqlReader["Descripcion"].ToString(),
                            FechaCreacion = conexion.sqlReader["FechaCreacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaCreacion"] : DateTime.MinValue,
                            FechaModificacion = conexion.sqlReader["FechaModificacion"] != DBNull.Value ? (DateTime)conexion.sqlReader["FechaModificacion"] : DateTime.MinValue
                            

                        };
                        if (conexion.sqlReader["Admin"] != DBNull.Value)
                            perfil.Admin = Convert.ToBoolean(conexion.sqlReader["Admin"]);
                        if (conexion.sqlReader["Padre.CodPerfilPadre"] != DBNull.Value)
                        {
                            perfil._Perfil = new BE.Perfil();
                            perfil._Perfil.CodPerfil = (int)conexion.sqlReader["Padre.CodPerfilPadre"];
                            perfil._Perfil.Descripcion = conexion.sqlReader["Padre.Descripcion"].ToString();
                        }
                        lstPerfil.Add(perfil);
                    }
                    conexion.Cerrar();
                };
             
                return lstPerfil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Perfil Obtener(BE.Perfil perfil)
        {
            try
            {
                conexion = new Acceso.Conexion();
                conexion.Abrir();
                conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                conexion.sqlCmd.CommandText = "PerfilObtener";
                conexion.sqlCmd.Parameters.AddWithValue("@CodPerfil", perfil.CodPerfil);
                conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                while (conexion.sqlReader.Read())
                {
                    perfil.CodPerfil = (int)conexion.sqlReader["CodPerfil"];
                    perfil.Descripcion = conexion.sqlReader["Descripcion"].ToString();
                    perfil.FechaCreacion = (DateTime)conexion.sqlReader["FechaCreacion"];
                    if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                        perfil.FechaModificacion = (DateTime)conexion.sqlReader["FechaModificacion"];

                    if (conexion.sqlReader["CodPerfilPadre"] != DBNull.Value)
                    {
                        perfil._Perfil.CodPerfil = (int)conexion.sqlReader["CodPerfilPadre"];
                        if (perfil._Perfil.CodPerfil > 0)
                        {
                            Obtener(perfil._Perfil);
                        }
                    }
                    if (conexion.sqlReader["Admin"] != DBNull.Value)
                    perfil.Admin = Convert.ToBoolean(conexion.sqlReader["Admin"].ToString());

                }
                conexion.Cerrar();
                return perfil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
