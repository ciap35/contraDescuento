using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class PreguntaDeSeguridad
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public PreguntaDeSeguridad() { }

        //public void Alta(BE.PreguntaDeSeguridad preguntaDeSeguridad)
        //{
        //    try
        //    {

        //    }
        //    catch (BE.ExcepcionPersonalizada ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(true, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(false, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //}
        //public void Modificar(BE.PreguntaDeSeguridad preguntaDeSeguridad)
        //{
        //    try
        //    {

        //    }
        //    catch (BE.ExcepcionPersonalizada ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(true, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(false, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //}
        //public void Baja(BE.PreguntaDeSeguridad preguntaDeSeguridad)
        //{
        //    try
        //    {

        //    }
        //    catch (BE.ExcepcionPersonalizada ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(true, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BE.ExcepcionPersonalizada Error = new BE.ExcepcionPersonalizada(false, ex);
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //}

        public BE.PreguntaDeSeguridad Obtener(BE.PreguntaDeSeguridad preguntaDeSeguridad)
        {
            BE.PreguntaDeSeguridad pregSeguridad = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PreguntaDeSeguridadObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codPregunta", preguntaDeSeguridad.CodPreguntaSeguridad);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    while (conexion.sqlReader.Read())
                    {
                        pregSeguridad = new BE.PreguntaDeSeguridad();
                        pregSeguridad.CodPreguntaSeguridad= Convert.ToInt32(conexion.sqlReader["CodPregunta"]);
                        pregSeguridad.Pregunta = Convert.ToString(conexion.sqlReader["Pregunta"]);
                        pregSeguridad.FechaAlta = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if(conexion.sqlReader["FechaBaja"] != DBNull.Value)
                        pregSeguridad.FechaBaja = Convert.ToDateTime(conexion.sqlReader["FechaBaja"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            pregSeguridad.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
            return pregSeguridad;
        }

        public List<BE.PreguntaDeSeguridad> Listar()
        {
            List<BE.PreguntaDeSeguridad> lstPreguntaDeSeguridad = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PreguntaDeSeguridadListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.HasRows)
                        lstPreguntaDeSeguridad = new List<BE.PreguntaDeSeguridad>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.PreguntaDeSeguridad pregSeguridad = new BE.PreguntaDeSeguridad();
                        pregSeguridad.CodPreguntaSeguridad = Convert.ToInt32(conexion.sqlReader["CodPregunta"]);
                        pregSeguridad.Pregunta = Convert.ToString(conexion.sqlReader["Pregunta"]);
                        pregSeguridad.FechaAlta = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if(conexion.sqlReader["FechaBaja"] != DBNull.Value)
                        pregSeguridad.FechaBaja = Convert.ToDateTime(conexion.sqlReader["FechaBaja"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            pregSeguridad.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);

                        lstPreguntaDeSeguridad.Add(pregSeguridad);
                    }
                    conexion.Cerrar();
                };
                return lstPreguntaDeSeguridad;
            }
           
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
