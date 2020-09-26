using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.Acceso;
namespace ContraDescuento.DAL
{
    public class Bitacora
    {
        #region Propiedades Privadas
        List<BE.Bitacora> lstexceptionLogger = null;
        #endregion
        public void Grabar(BE.Bitacora exception)
        {
            try
            {
                using (Acceso.Conexion Conexion = new Acceso.Conexion())
                {
                    Conexion.Abrir();
                    Conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Conexion.sqlCmd.CommandText = "LogAlta"; //LogErrorAlta
                    Conexion.sqlCmd.Parameters.AddWithValue("@message", exception.Mensaje == null ? DBNull.Value.ToString() : exception.Mensaje);
                    Conexion.sqlCmd.Parameters.AddWithValue("@informativo", exception.ExcepcionControlada);
                    Conexion.sqlCmd.Parameters.AddWithValue("@stackTrace", exception.Stack == null ? DBNull.Value.ToString() : exception.Stack);
                    Conexion.sqlCmd.Parameters.AddWithValue("@mensajePersonalizado", exception.MensajePersonalizado == null ? DBNull.Value.ToString() : exception.MensajePersonalizado);
                    Conexion.sqlCmd.Parameters.AddWithValue("@proyecto", exception.Proyecto == null ? DBNull.Value.ToString() : exception.Proyecto);
                    Conexion.sqlCmd.Parameters.AddWithValue("@clase", exception.Clase == null ? DBNull.Value.ToString() : exception.Clase);
                    Conexion.sqlCmd.Parameters.AddWithValue("@metodo", exception.Metodo == null ? DBNull.Value.ToString() : exception.Metodo);
                    Conexion.sqlCmd.Parameters.AddWithValue("@contexto", exception.Contexto == null ? DBNull.Value.ToString() : exception.Metodo);
                    Conexion.sqlCmd.ExecuteNonQuery();
                    Conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                GrabarEnArchivo(ex);
            }
        }

        public List<BE.Bitacora> Listar()
        {
            try
            {
                using (Acceso.Conexion Conexion = new Acceso.Conexion())
                {
                    Conexion.Abrir();
                    Conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Conexion.sqlCmd.CommandText = "BitacoraListar";
                    Conexion.sqlReader = Conexion.sqlCmd.ExecuteReader();
                    lstexceptionLogger = new List<BE.Bitacora>();
                    while (Conexion.sqlReader.Read())
                    {
                        lstexceptionLogger.Add(new BE.Bitacora()
                        {
                            CodMensaje = (int)Conexion.sqlReader["codMensaje"],
                            Mensaje = Conexion.sqlReader["message"].ToString(),
                            Stack = Conexion.sqlReader["stackTrace"].ToString(), //error
                            Fecha = Convert.ToDateTime(Conexion.sqlReader["fecha"]),
                            ExcepcionControlada = Conexion.sqlReader["informativo"] != DBNull.Value ? Convert.ToBoolean(Conexion.sqlReader["informativo"]) : false,
                            Proyecto = Convert.ToString(Conexion.sqlReader["proyecto"]),
                            Clase = Convert.ToString(Conexion.sqlReader["clase"]),
                            Contexto = Convert.ToString(Conexion.sqlReader["contexto"]),
                            Metodo = Convert.ToString(Conexion.sqlReader["metodo"])
                        });
                    }
                    Conexion.Cerrar();
                };
                return lstexceptionLogger;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GrabarEnArchivo(Exception exception)
        {
            try
            {
                string pathLog = System.Configuration.ConfigurationSettings.AppSettings["Log"];
                if (!Directory.Exists(pathLog))
                    Directory.CreateDirectory(pathLog);

                using (StreamWriter sw = new StreamWriter(pathLog + "ContraDescuento_Log_" + DateTime.Now.ToString("dd--MM--yyyy") + ".txt", true))
                {
                    sw.WriteLine("==================================================================================================================");
                    sw.WriteLine("[" + DateTime.Now + "]: ");
                    BE.Bitacora exceptionLogger = new BE.Bitacora(exception);
                    sw.WriteLine(exceptionLogger.ToString());
                    sw.Flush();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
