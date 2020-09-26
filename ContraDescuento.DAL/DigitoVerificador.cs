using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;
using ContraDescuento.Acceso;

namespace ContraDescuento.DAL
{
    public class DigitoVerificador
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public DigitoVerificador() { }

        /// <summary>
        ///Se obtienen todos los registros de la tabla Usuario para el calculo de los Digitos Verificadores
        /// </summary>
        /// <returns></returns>
        public List<BE.IAuditable> ObtenerRegistrosUsuario()
        {
            List<BE.IAuditable> lstUsuarios = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DVH_Registros_Listar";
                    conexion.sqlCmd.Parameters.AddWithValue("@tabla", "usuario");
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    lstUsuarios = new List<BE.IAuditable>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Usuario usuario = new BE.Usuario();
                        usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["codUsuario"]);
                        usuario.Nombre = conexion.sqlReader["nombre"].ToString();
                        usuario.Apellido = conexion.sqlReader["apellido"].ToString();
                        usuario.Sexo =   Convert.ToChar(conexion.sqlReader["sexo"]);
                        usuario.Password = conexion.sqlReader["password"].ToString();
                        usuario.Email = conexion.sqlReader["email"].ToString();
                        usuario.DVH = Convert.ToInt32(conexion.sqlReader["dvh"]);
                        usuario.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["fechaCreacion"]);
                        usuario.FechaDeNacimiento = Convert.ToDateTime(conexion.sqlReader["fechaDeNacimiento"]);

                        if (conexion.sqlReader["fechaModificacion"] != System.DBNull.Value)
                            usuario.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["fechaModificacion"]);

                        if (conexion.sqlReader["fechaBaja"] != System.DBNull.Value)
                            usuario.FechaBaja = Convert.ToDateTime(conexion.sqlReader["fechaBaja"]);

                        usuario.Bloqueado = Convert.ToBoolean(conexion.sqlReader["bloqueado"]);
                        usuario.CantIntentos = Convert.ToInt32(conexion.sqlReader["cantIntentos"]);

                        if (conexion.sqlReader["codIdioma"] != System.DBNull.Value)
                        {
                            usuario.Idioma = new BE.Idioma();
                            usuario.Idioma.CodIdioma = Convert.ToInt32(conexion.sqlReader["codIdioma"]);
                        }

                        if (conexion.sqlReader["codComercio"] != System.DBNull.Value)
                        {
                            BE.Comercio comercio = new BE.Comercio() { CodComercio = Convert.ToInt32(conexion.sqlReader["codComercio"]) };
                            usuario.Comercio = comercio;
                        }
                        //usuario.Grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["codGrupo"]);
                        lstUsuarios.Add(usuario);
                    }
                    conexion.Cerrar();
                };
                return lstUsuarios;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarDVV(DateTime fechaCalculo,int resultado)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DVH_Actualizar";
                    conexion.sqlCmd.Parameters.AddWithValue("@resultado", resultado);
                    conexion.sqlCmd.Parameters.AddWithValue("@fechaCalculo", resultado);

                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool ObtenerDVV(string tabla)
        {
            try
            {
                throw new NotImplementedException("No implementado");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
