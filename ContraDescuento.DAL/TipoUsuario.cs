using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class TipoUsuario
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public TipoUsuario() { }

        public void Obtener(ref BE.TipoUsuario tipoUsuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TipoUsuarioObtener";

                    conexion.sqlCmd.Parameters.AddWithValue("@usuario", tipoUsuario.Usuario); //Usuario 
                    conexion.sqlCmd.Parameters.AddWithValue("@comercio", tipoUsuario.Comercio); //Usuario de Comercio
                    conexion.sqlCmd.Parameters.AddWithValue("@otros", tipoUsuario.Otros); //otros usuarios
                    //Si no tiene asignado tipo de usuario, es administrador.

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        tipoUsuario.Grupo = new BE.Grupo();
                        tipoUsuario.CodTipoUsuario = Convert.ToInt32(conexion.sqlReader["codTipoUsuario"]);
                        tipoUsuario.Grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["codGrupo"]);
                        tipoUsuario.Grupo.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ObtenerPorCodigoTipoUsuario(ref BE.TipoUsuario tipoUsuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TipoUsuarioObtenerPorCodigo";

                    conexion.sqlCmd.Parameters.AddWithValue("@codTipoUsuario", tipoUsuario.CodTipoUsuario); //Usuario 
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        tipoUsuario.CodTipoUsuario = Convert.ToInt32(conexion.sqlReader["codTipoUsuario"]);
                        if (tipoUsuario.Grupo == null)
                        {
                            tipoUsuario.Grupo = new BE.Grupo();
                        }
                        tipoUsuario.Grupo.CodGrupo = Convert.ToInt32(conexion.sqlReader["codGrupo"]);
                        tipoUsuario.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        tipoUsuario.Usuario = Convert.ToBoolean(conexion.sqlReader["usuario"]);
                        tipoUsuario.Comercio = Convert.ToBoolean(conexion.sqlReader["comercio"]);
                        tipoUsuario.Otros = Convert.ToBoolean(conexion.sqlReader["otros"]);
                    }
                    conexion.Cerrar();
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BE.TipoUsuario> Listar()
        {
            List<BE.TipoUsuario> lstTipoUsuario = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TipoUsuarioListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstTipoUsuario = new List<BE.TipoUsuario>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.TipoUsuario tipoUsuario = new BE.TipoUsuario();
                        tipoUsuario.CodTipoUsuario = Convert.ToInt32(conexion.sqlReader["codTipoUsuario"]);
                        tipoUsuario.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        tipoUsuario.Usuario = Convert.ToBoolean(conexion.sqlReader["usuario"]);
                        tipoUsuario.Comercio = Convert.ToBoolean(conexion.sqlReader["comercio"]);
                        tipoUsuario.Otros = Convert.ToBoolean(conexion.sqlReader["otros"]);
                        lstTipoUsuario.Add(tipoUsuario);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTipoUsuario;
        }

        public void ActualizarMapeo(BE.Grupo grupoUSuario, BE.Grupo grupoComercio, BE.Grupo grupoOtros)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TipoUsuarioModificar";

                    conexion.sqlCmd.Parameters.AddWithValue("@usuario", grupoUSuario.CodGrupo); //Usuario 
                    conexion.sqlCmd.Parameters.AddWithValue("@comercio", grupoComercio.CodGrupo); //Usuario de Comercio
                    conexion.sqlCmd.Parameters.AddWithValue("@otros", grupoOtros.CodGrupo); //otros usuarios
                    //Si no tiene asignado tipo de usuario, es administrador.
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