using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Idioma
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Idioma() { }

        public void Alta(BE.Idioma idioma)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "IdiomaAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", idioma.Descripcion);
                    if (idioma.Charset != string.Empty)
                        conexion.sqlCmd.Parameters.AddWithValue("@charset", idioma.Charset);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@charset", DBNull.Value);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Idioma idioma)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "IdiomaBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", idioma.CodIdioma);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificacion(BE.Idioma idioma)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "IdiomaModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", idioma.CodIdioma);
                    if (idioma.Charset != string.Empty)
                        conexion.sqlCmd.Parameters.AddWithValue("@charset", idioma.Charset);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@charset", DBNull.Value);

                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", idioma.Descripcion);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Idioma Obtener(BE.Idioma idioma)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "IdiomaObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", idioma.CodIdioma);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        idioma.CodIdioma = (int)conexion.sqlReader["CodIdioma"];
                        idioma.Descripcion = conexion.sqlReader["Descripcion"].ToString();
                        idioma.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        idioma.Charset = conexion.sqlReader["Charset"] != DBNull.Value ? conexion.sqlReader["Charset"].ToString() : string.Empty;
                        idioma.PorDefecto = conexion.sqlReader["PorDefecto"] != DBNull.Value ? Convert.ToBoolean(conexion.sqlReader["PorDefecto"]) : false;
                    }
                    conexion.Cerrar();
                }
                return idioma;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Idioma> Listar()
        {
            List<BE.Idioma> lstIdioma = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "IdiomaListar";
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    lstIdioma = new List<BE.Idioma>();
                    BE.Idioma idioma = null;
                    while (conexion.sqlReader.Read())
                    {
                        idioma = new BE.Idioma()
                        {
                            CodIdioma = (int)conexion.sqlReader["CodIdioma"],
                            Descripcion = conexion.sqlReader["Descripcion"].ToString(),
                            FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]),

                        };
                        idioma.Charset = conexion.sqlReader["Charset"] != DBNull.Value ? conexion.sqlReader["Charset"].ToString() : string.Empty;
                        idioma.PorDefecto = conexion.sqlReader["PorDefecto"] != DBNull.Value ? Convert.ToBoolean(conexion.sqlReader["PorDefecto"]) : false;
                        lstIdioma.Add(idioma);
                    }
                    conexion.Cerrar();
                }

                return lstIdioma;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
