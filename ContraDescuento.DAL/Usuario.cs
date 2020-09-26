using System;
using System.Collections.Generic;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Usuario
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        private BE.Usuario usuario = null;
        #endregion
        public Usuario()
        {
        }

        public void Alta(ref BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioRegistrar";
                    conexion.sqlCmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    conexion.sqlCmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    conexion.sqlCmd.Parameters.AddWithValue("@fechaDeNacimiento", Convert.ToDateTime(usuario.FechaDeNacimiento.ToString("dd/MM/yyyy")));
                    conexion.sqlCmd.Parameters.AddWithValue("@email", usuario.Email);
                    conexion.sqlCmd.Parameters.AddWithValue("@sexo", usuario.Sexo.Value);
                    conexion.sqlCmd.Parameters.AddWithValue("@password", usuario.Password);
                    conexion.sqlCmd.Parameters.AddWithValue("@dvh", usuario.DVH);
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", usuario.Idioma.CodIdioma);
                    conexion.sqlCmd.Parameters.AddWithValue("@codTipoUsuario", usuario.TipoUsuario.CodTipoUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@codPregunta", usuario.PreguntaDeSeguridad.CodPreguntaSeguridad);
                    conexion.sqlCmd.Parameters.AddWithValue("@respuesta", usuario.PreguntaDeSeguridad.Respuesta);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    while (conexion.sqlReader.Read())
                    {
                        usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["CodUsuario"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BE.Usuario Obtener(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    usuario = new BE.Usuario();

                    while (conexion.sqlReader.Read())
                    {
                        usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["codUsuario"]);
                        usuario.Nombre = conexion.sqlReader["nombre"].ToString();
                        usuario.Apellido = conexion.sqlReader["apellido"].ToString();
                        usuario.Email = conexion.sqlReader["email"].ToString();
                        usuario.DVH = Convert.ToInt32(conexion.sqlReader["dvh"]);
                        usuario.Sexo = Convert.ToChar(conexion.sqlReader["sexo"]);
                        usuario.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["fechaCreacion"]);
                        usuario.FechaDeNacimiento = Convert.ToDateTime(conexion.sqlReader["fechaDeNacimiento"]);
                        if (conexion.sqlReader["codPregunta"] != System.DBNull.Value)
                        {
                            usuario.PreguntaDeSeguridad.CodPreguntaSeguridad = Convert.ToInt32(conexion.sqlReader["codPregunta"]);
                            if (conexion.sqlReader["respuesta"] != System.DBNull.Value)
                            {
                                usuario.PreguntaDeSeguridad.Respuesta = Convert.ToString(conexion.sqlReader["respuesta"]);
                            }
                        }
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

                        if (conexion.sqlReader["codTelefono"] != System.DBNull.Value)
                        {
                            usuario.Telefono.CodTelefono = Convert.ToInt32(conexion.sqlReader["codTelefono"]);
                        }

                        if (conexion.sqlReader["codComercio"] != System.DBNull.Value)
                        {
                            BE.Comercio comercio = new BE.Comercio() { CodComercio = Convert.ToInt32(conexion.sqlReader["codComercio"]) };
                            usuario.Comercio = comercio;
                        }
                        
                        if(conexion.sqlReader["codTipoUsuario"] != System.DBNull.Value)
                        {
                            usuario.TipoUsuario = new BE.TipoUsuario() { CodTipoUsuario = Convert.ToInt32(conexion.sqlReader["codTipoUsuario"]) };
                        }
                        else
                        {
                            usuario.TipoUsuario = null;
                        }
                        if(conexion.sqlReader["habilitado"] != System.DBNull.Value)
                        {
                            usuario.Habilitado = Convert.ToBoolean(conexion.sqlReader["habilitado"]);
                        }
                        if (conexion.sqlReader["bloqueado"] != System.DBNull.Value)
                        {
                            usuario.Bloqueado = Convert.ToBoolean(conexion.sqlReader["bloqueado"]);
                        }
                        if(conexion.sqlReader["password"] != System.DBNull.Value)
                        {
                            usuario.Password = Convert.ToString(conexion.sqlReader["password"]);
                        }
                    }
                    conexion.Cerrar();
                };
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Usuario> Listar(bool soloHabilitados)
        {
            List<BE.Usuario> lstUsuario = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioListar";
                    conexion.sqlCmd.Parameters.AddWithValue("@soloHabilitados", soloHabilitados);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstUsuario = new List<BE.Usuario>();

                    

                    while (conexion.sqlReader.Read())
                    {
                        usuario = new BE.Usuario();
                        usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["codUsuario"]);
                        usuario.Nombre = conexion.sqlReader["nombre"].ToString();
                        usuario.Apellido = conexion.sqlReader["apellido"].ToString();
                        if (conexion.sqlReader["codTipoUsuario"] != System.DBNull.Value)
                        {
                            usuario.TipoUsuario = new BE.TipoUsuario() { CodTipoUsuario = Convert.ToInt32(conexion.sqlReader["codTipoUsuario"]) };
                            usuario.TipoUsuario.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                            usuario.TipoUsuario.Usuario = Convert.ToBoolean(conexion.sqlReader["usuario"]);
                            usuario.TipoUsuario.Comercio = Convert.ToBoolean(conexion.sqlReader["comercio"]);
                            usuario.TipoUsuario.Otros = Convert.ToBoolean(conexion.sqlReader["otros"]);
                        }
                        else
                        {
                            usuario.TipoUsuario = null;
                        }
                        lstUsuario.Add(usuario);
                    }
                    conexion.Cerrar();
                };
                return lstUsuario;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public BE.Usuario BuscarPorEmail(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioBuscarPorEmail";
                    conexion.sqlCmd.Parameters.AddWithValue("@email", usuario.Email);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    
                    while (conexion.sqlReader.Read())
                    {
                        usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["codUsuario"]);
                        usuario.Email = Convert.ToString(conexion.sqlReader["email"]);
                        if(conexion.sqlReader["codPregunta"] != DBNull.Value)
                        usuario.PreguntaDeSeguridad.CodPreguntaSeguridad = Convert.ToInt32(conexion.sqlReader["codPregunta"]);
                        if (conexion.sqlReader["respuesta"] != DBNull.Value)
                            usuario.PreguntaDeSeguridad.Respuesta = Convert.ToString(conexion.sqlReader["respuesta"]);
                    }
                    conexion.Cerrar();
                };
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CambiarPassword(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioCambiarPassword";
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@password", usuario.Password);

                    conexion.sqlCmd.ExecuteNonQuery();                   
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarExistencia(string email)
        {
            bool Existe = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioValidarExistencia";
                    conexion.sqlCmd.Parameters.AddWithValue("@email", email);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        Existe = Convert.ToBoolean(conexion.sqlReader["USUARIO_EXISTENTE"]);
                    }


                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Existe;
        }
        public void Baja(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandText = "UsuarioHabilitarDeshabilitar";
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@habilitado", usuario.Habilitado);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ModificarIdioma(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandText = "UsuarioCambiarIdioma";
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", usuario.Idioma.CodIdioma);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    conexion.sqlCmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    conexion.sqlCmd.Parameters.AddWithValue("@fechaDeNacimiento", Convert.ToDateTime(usuario.FechaDeNacimiento.ToString("dd/MM/yyyy")));
                    conexion.sqlCmd.Parameters.AddWithValue("@email", usuario.Email);
                    conexion.sqlCmd.Parameters.AddWithValue("@sexo", usuario.Sexo.Value);
                    conexion.sqlCmd.Parameters.AddWithValue("@password", usuario.Password);
                    conexion.sqlCmd.Parameters.AddWithValue("@dvh", usuario.DVH);
                    conexion.sqlCmd.Parameters.AddWithValue("@codIdioma", usuario.Idioma.CodIdioma);
                    if (usuario.TipoUsuario != null)
                    {
                        conexion.sqlCmd.Parameters.AddWithValue("@codTipoUsuario", usuario.TipoUsuario.CodTipoUsuario);
                    }
                    else
                    {
                        conexion.sqlCmd.Parameters.AddWithValue("@codTipoUsuario", DBNull.Value);
                    }
                    //conexion.sqlCmd.Parameters.AddWithValue("@codGrupo", usuario.Grupo.CodGrupo);
                    conexion.sqlCmd.Parameters.AddWithValue("@codPregunta", usuario.PreguntaDeSeguridad.CodPreguntaSeguridad);
                    if(usuario.PreguntaDeSeguridad.Respuesta == string.Empty || usuario.PreguntaDeSeguridad.Respuesta == null)
                        conexion.sqlCmd.Parameters.AddWithValue("@respuesta", DBNull.Value);
                    else
                    conexion.sqlCmd.Parameters.AddWithValue("@respuesta", usuario.PreguntaDeSeguridad.Respuesta);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Login(string email, string password, ref BE.Usuario _usuario)
        {
            int Status = 1;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioLogin";
                    conexion.sqlCmd.Parameters.AddWithValue("@email", email);
                    conexion.sqlCmd.Parameters.AddWithValue("@password", password);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        Status = Convert.ToInt32(conexion.sqlReader["STATUS_LOGIN"]);
                        _usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["CodUsuario"]);
                    }

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Status;
        }

        public void DesbloquearCuenta(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioDesbloquear";
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@token", DBNull.Value);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DesbloquearCuenta(string token)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioDesbloquear";
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", DBNull.Value);
                    conexion.sqlCmd.Parameters.AddWithValue("@token", token);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidarToken(string token)
        {
            bool tokenValido = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "UsuarioValidarToken";
                    conexion.sqlCmd.Parameters.AddWithValue("@token", token);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        tokenValido = (bool)conexion.sqlReader["TOKEN_VALIDO"];
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tokenValido;
        }
    }
}