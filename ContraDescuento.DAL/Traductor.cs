using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Traductor
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        private BE.Traductor _traductor = null;
        private List<BE.Traductor> lstTraductor = null;
        #endregion

        public Traductor() { }

        public void Alta(BE.Traductor traductor)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TraduccionAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", traductor.Idioma.CodIdioma);
                    conexion.sqlCmd.Parameters.AddWithValue("@pagina", traductor.Traduccion.Pagina);
                    conexion.sqlCmd.Parameters.AddWithValue("@controlID", traductor.Traduccion.ControlID);
                    conexion.sqlCmd.Parameters.AddWithValue("@texto", traductor.Traduccion.Texto);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Traductor traductor)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TraduccionBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codTraduccion", traductor.Traduccion.CodTraduccion);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificacion(BE.Traductor traductor)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TraduccionModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codTraduccion", traductor.Traduccion.CodTraduccion);
                    conexion.sqlCmd.Parameters.AddWithValue("@pagina", traductor.Traduccion.Pagina);
                    conexion.sqlCmd.Parameters.AddWithValue("@controlID", traductor.Traduccion.ControlID);
                    conexion.sqlCmd.Parameters.AddWithValue("@texto", traductor.Traduccion.Texto);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Traductor Obtener(BE.Traductor traductor)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TraduccionObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codTraduccion", traductor.Traduccion.CodTraduccion);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    _traductor = new BE.Traductor();
                    while (conexion.sqlReader.Read())
                    {
                        _traductor.Traduccion.CodTraduccion = (int)conexion.sqlReader["codTraduccion"];
                        _traductor.Idioma.CodIdioma = (int)conexion.sqlReader["codIdioma"];
                        _traductor.Traduccion.Pagina = conexion.sqlReader["pagina"].ToString();
                        _traductor.Traduccion.ControlID = conexion.sqlReader["controlID"].ToString();
                        _traductor.Traduccion.Texto = conexion.sqlReader["texto"].ToString();
                        lstTraductor.Add(_traductor);
                    }
                    conexion.Cerrar();
                }
                return traductor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Traductor> Listar(BE.Traductor traductor)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TraduccionListar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", traductor.Idioma.CodIdioma);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                   
                    lstTraductor = new List<BE.Traductor>();
                    while (conexion.sqlReader.Read())
                    {
                        _traductor = new BE.Traductor();
                        _traductor.Traduccion.CodTraduccion = (int)conexion.sqlReader["codTraduccion"];
                        _traductor.Idioma.CodIdioma = (int)conexion.sqlReader["codIdioma"];
                        _traductor.Traduccion.Pagina = conexion.sqlReader["pagina"].ToString();
                        _traductor.Traduccion.ControlID = conexion.sqlReader["controlID"].ToString();
                        _traductor.Traduccion.Texto = conexion.sqlReader["texto"].ToString();
                        lstTraductor.Add(_traductor);
                    }
                    conexion.Cerrar();
                }
                return lstTraductor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
