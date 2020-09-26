using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Email
    {
        #region Propiedades Privadas
        //List<BE.Bitacora> lstexceptionLogger = null;
        Acceso.Conexion conexion;
        #endregion

        public Email() { }

        public BE.Email ObtenerTemplate(BE.TipoEmail tipoEmail)
        {
            BE.Email emailEnvio;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EmailObtenerTemplate";
                    conexion.sqlCmd.Parameters.AddWithValue("@codTipoEmail", tipoEmail.CodTipoEmail);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    emailEnvio = new BE.Email();
                    while(conexion.sqlReader.Read())
                    {
                        emailEnvio.Asunto = conexion.sqlReader["asunto"].ToString();
                        emailEnvio.TipoEmail.Descripcion = conexion.sqlReader["descripcion"].ToString();
                    }
                    conexion.Cerrar();
                };
                return emailEnvio;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public void RegistrarEnvio(BE.Email email)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EmailRegistrarEnvio";
                    conexion.sqlCmd.Parameters.AddWithValue("@token", email.Token);
                    conexion.sqlCmd.Parameters.AddWithValue("@email", email.Destinatario);
                    conexion.sqlCmd.Parameters.AddWithValue("@tipoEmail", email.TipoEmail.CodTipoEmail);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
