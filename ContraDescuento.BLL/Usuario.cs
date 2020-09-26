using System;
using System.Collections.Generic;
using ContraDescuento.BE;

namespace ContraDescuento.BLL
{
    public class Usuario
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Usuario usuarioDAL = new DAL.Usuario();
        TFL.SymetricCryptool SymCrypto = null;
        BLL.Telefono telefonoNeg = new BLL.Telefono();
        BLL.PreguntaDeSeguridad preguntaDeSeguridadNeg = new BLL.PreguntaDeSeguridad();
        BLL.TipoUsuario tipoUsuarioNeg = new BLL.TipoUsuario();
        #endregion

        #region Alta,Baja,Modificar,Obtener
        public void Alta(BE.Usuario usuario)
        {
            usuario.TipoUsuario = new BE.TipoUsuario(EnumTipoUsuario.Usuario);
            SymCrypto = new TFL.SymetricCryptool();
            try
            {
                string pwd = usuario.Password;
                if (usuario != null)
                {
                    usuario.Password = SymCrypto.Encrypt(ref pwd); //Encriptado con SHA1
                    //validar si existe email
                    if (!usuarioDAL.ValidarExistencia(usuario.Email))
                    {
                        if (usuario.TipoUsuario != null)
                        {
                            usuario.TipoUsuario = tipoUsuarioNeg.Obtener(usuario.TipoUsuario);
                        }

                        if (usuario.PreguntaDeSeguridad.Respuesta == string.Empty || usuario.PreguntaDeSeguridad.Respuesta == null ||
                            usuario.PreguntaDeSeguridad.CodPreguntaSeguridad == 0)
                            throw new BE.ExcepcionPersonalizada(true, new Exception("Debe ingresar la respuesta y/o pregunta de seguridad"));

                        string respuestaSeguridad = usuario.PreguntaDeSeguridad.Respuesta.ToUpper();
                        usuario.PreguntaDeSeguridad.Respuesta = SymCrypto.Encrypt(ref respuestaSeguridad);
                        //1- Calculo el DVH antes del alta.
                        usuario.DVH = usuario.CalcularDVH();
                        //2- Doy el alta del usuario en BD.
                        usuarioDAL.Alta(ref usuario);
                    }
                    else
                    {
                        throw new BE.ExcepcionPersonalizada(true, new Exception("El e-mail proporcionado ya se encuentra registrado, por favor elija otro"));
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                exceptionLogger.Grabar(ex.obtenerExcepcion());
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void AltaAdminDeComercio(BE.Usuario usuario)
        {
            usuario.TipoUsuario = new BE.TipoUsuario(EnumTipoUsuario.Comercio);
            SymCrypto = new TFL.SymetricCryptool();
            try
            {
                string pwd = usuario.Password;
                if (usuario != null)
                {
                    usuario.Password = SymCrypto.Encrypt(ref pwd); //Encriptado con SHA1

                    //validar si existe email
                    if (!usuarioDAL.ValidarExistencia(usuario.Email))
                    {
                        //1- Calculo el DVH antes del alta.
                        usuario.DVH = usuario.CalcularDVH();
                        //2- Doy el alta del usuario en BD.
                        if (usuario.TipoUsuario != null && usuario.TipoUsuario.Comercio == true)
                        {
                            usuario.TipoUsuario = tipoUsuarioNeg.Obtener(usuario.TipoUsuario);
                        }

                        if (usuario.PreguntaDeSeguridad.Respuesta == string.Empty || usuario.PreguntaDeSeguridad.Respuesta == null ||
                            usuario.PreguntaDeSeguridad.CodPreguntaSeguridad == 0)
                            throw new BE.ExcepcionPersonalizada(true, new Exception("Debe ingresar la respuesta y/o pregunta de seguridad"));

                        string respuestaSeguridad = usuario.PreguntaDeSeguridad.Respuesta.ToUpper();
                        usuario.PreguntaDeSeguridad.Respuesta = SymCrypto.Encrypt(ref respuestaSeguridad);

                        usuarioDAL.Alta(ref usuario);

                        BLL.Telefono telefonoNeg = new BLL.Telefono();
                        telefonoNeg.Alta(ref usuario);
                    }
                    else
                    {
                        throw new BE.ExcepcionPersonalizada(true, new Exception("El e-mail proporcionado ya se encuentra registrado, por favor elija otro"));
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                exceptionLogger.Grabar(ex.obtenerExcepcion());
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void Baja(BE.Usuario usuario)
        {
            try
            {
                usuario.Habilitado = false;
                usuarioDAL.Baja(usuario);
                if(usuario.Comercio != null && usuario.Comercio.CodComercio > 0)
                {
                    BLL.Comercio comercioNeg = new Comercio();
                    comercioNeg.Baja(usuario.Comercio);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        public void RehabilitarUsuario(BE.Usuario usuario)
        {
            try
            {
                usuario.Habilitado = true;
                usuarioDAL.Baja(usuario);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        public void Modificar(BE.Usuario usuario, BE.Usuario usuarioAntiguo)
        {
            SymCrypto = new TFL.SymetricCryptool();
            try
            {
                string pwd = usuario.Password;
                if (usuario != null)
                {
                    if (usuario.Password != string.Empty)
                        usuario.Password = SymCrypto.Encrypt(ref pwd); //Encriptado con SHA1


                    if (usuario.PreguntaDeSeguridad == null)
                        throw new ArgumentNullException("No se ha podido cargar la pregunta de seguridad, intente nuevamete más tarde");

                    if (usuario.PreguntaDeSeguridad.CodPreguntaSeguridad != usuarioAntiguo.PreguntaDeSeguridad.CodPreguntaSeguridad)
                    {
                        if (usuario.PreguntaDeSeguridad.Respuesta == string.Empty)
                        {
                            throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor ingrese la respuesta a la pregunta de seguridad."));
                        }
                    }
                    else if (usuario.PreguntaDeSeguridad.CodPreguntaSeguridad == usuarioAntiguo.PreguntaDeSeguridad.CodPreguntaSeguridad)
                    {
                        if (usuario.PreguntaDeSeguridad.Respuesta != string.Empty)
                        {
                            string respuestaSeguridad = usuario.PreguntaDeSeguridad.Respuesta.ToUpper();
                            usuario.PreguntaDeSeguridad.Respuesta = SymCrypto.Encrypt(ref respuestaSeguridad);
                        }

                    }

                    if (usuario.Email != usuarioAntiguo.Email)
                    {
                        if (usuarioDAL.ValidarExistencia(usuario.Email))
                            throw new Exception("El e-mail proporcionado ya se encuentra registrado, por favor elija otro");
                    }
                    //1- Calculo el DVH antes del alta.
                    usuario.DVH = usuario.CalcularDVH();
                    //2- Modifico el usuario en BD.
                    usuarioDAL.Modificar(usuario);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public BE.Usuario Obtener(BE.Usuario usuario)
        {
            try
            {
                usuario = usuarioDAL.Obtener(usuario);
                usuario.Telefono = telefonoNeg.Obtener(usuario.Telefono);
                usuario.PreguntaDeSeguridad = preguntaDeSeguridadNeg.Obtener(usuario.PreguntaDeSeguridad);
                if (usuario.TipoUsuario != null) //Si es nulo, es administrador.
                {
                    usuario.TipoUsuario = tipoUsuarioNeg.Obtener(usuario.TipoUsuario);
                    if (usuario.TipoUsuario.Comercio)
                    {
                        BLL.Comercio comercioNeg = new Comercio();
                        usuario.Comercio = comercioNeg.Obtener(usuario.Comercio);
                    }
                    //else { 
                    //usuario.TipoUsuario = tipoUsuarioNeg.Obtener(usuario.TipoUsuario);
                    //}
                }

                

                if (usuario == null)
                    throw new Exception("Ocurrió un error al obtener el usuario");

                return usuario;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Usuario BuscarPorEmail(BE.Usuario usuario)
        {
            try
            {
                usuario = usuarioDAL.BuscarPorEmail(usuario);

                if (usuario == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha encontrado el usuario un error al obtener el usuario"));

                return usuario;
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void CambiarIdioma(BE.Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor logueese"));
                if (usuario.Idioma == null)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha seleccionado un idioma para el usuario"));
                }

                usuarioDAL.ModificarIdioma(usuario);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void CambiarPassword(BE.Usuario usuario)
        {
            try
            {
                if (usuario == null || usuario.CodUsuario == 0 || usuario.Password == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido cambiar el password"));
                if (usuario.Password.Length < 8)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("La password debe ser igual o mayor a 8 carácteres"));
                SymCrypto = new TFL.SymetricCryptool();
                string pass = usuario.Password;
                usuario.Password = SymCrypto.Encrypt(ref pass);
                usuarioDAL.CambiarPassword(usuario);

                usuarioDAL.DesbloquearCuenta(usuario);
                if (usuario.FechaBaja == DateTime.MinValue)
                    this.RehabilitarUsuario(usuario);
                usuario = this.Obtener(usuario);
                //1- Calculo el DVH antes del alta.
                usuario.DVH = usuario.CalcularDVH();
                //2- Modifico el usuario en BD.
                usuarioDAL.Modificar(usuario);

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.Usuario> Listar(bool soloHabilitados)
        {
            List<BE.Usuario> lstUsuarios = null;
            try
            {
                lstUsuarios = usuarioDAL.Listar(soloHabilitados);
                List<BE.Usuario> lstAdmin = new List<BE.Usuario>();                
                foreach (BE.Usuario admin in lstUsuarios)
                {
                    if (admin.TipoUsuario == null)
                        lstAdmin.Add(admin);
                }
                foreach(BE.Usuario adm in lstAdmin)
                { 
                    lstUsuarios.Remove(adm);
                }

                //foreach (BE.Usuario usuario in lstUsuarios)
                //{
                //    if (usuario.TipoUsuario != null)
                //    {
                //        BE.TipoUsuario tipoUsuario = null;
                //        if(usuario.TipoUsuario.Usuario)
                //            tipoUsuario = new BE.TipoUsuario(true,false,false);
                //        else if(usuario.TipoUsuario.Comercio)
                //            tipoUsuario = new BE.TipoUsuario(false, true, false);
                //        else if(usuario.TipoUsuario.Otros)
                //            tipoUsuario = new BE.TipoUsuario(false, false, true);

                //        usuario.TipoUsuario = tipoUsuarioNeg.Obtener(tipoUsuario);
                //    }
                //}
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstUsuarios;
        }
        #endregion

        #region Negocio
        /// <summary>
        /// Devuelve el estado al intentar loguearse en el sistema a partir del par Email / Password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="usuario">Objeto usuario, al ser el login éxitoso se devolverá en primera instancia el código de usuario para luego obtener el resto de la información</param>
        /// <returns></returns>
        public Enum_Status_Login Login(string email, ref string password, ref BE.Usuario usuario)
        {
            SymCrypto = new TFL.SymetricCryptool();
            try
            {
                password = SymCrypto.Encrypt(ref password);

                if (usuario == null)
                {
                    usuario = new BE.Usuario();
                }

                Enum_Status_Login status = (Enum_Status_Login)usuarioDAL.Login(email, password, ref usuario);

                return status;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void DesbloquearCuenta(BE.Usuario usuario)
        {
            try
            {
                DAL.Usuario usuarioDAL = new DAL.Usuario();

                usuarioDAL.DesbloquearCuenta(usuario);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public bool DesbloquearCuenta(string token)
        {
            try
            {
                DAL.Usuario usuarioDAL = new DAL.Usuario();

                //Validar si éxiste token
                ValidarToken(token);
                //token Válido, desbloqueo cuenta
                usuarioDAL.DesbloquearCuenta(token);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return false;
        }
        private bool ValidarToken(string token)
        {
            try
            {
                DAL.Usuario usuario = new DAL.Usuario();
                return usuario.ValidarToken(token);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public bool EnviarEmailDesbloqueo(BE.Usuario usuario)
        {
            try
            {
                BLL.Email emailNeg = new Email();
                emailNeg.EnviarEmail(usuario, Enum_Tipo_Email.Desbloqueo);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                return false;
            }
            return true;
        }
        public bool RecuperarPassword(string email)
        {
            try
            {
                throw new NotImplementedException("No implementado");
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return false;
        }
        #endregion 
    }

    //public class UsuarioNegocioException : System.Exception
    //{
    //    private BE.Bitacora exception = null;
    //    public UsuarioNegocioException(bool ExControlada, Exception ex) : base(String.Format("Error: {0}", ex.Message))
    //    {
    //        exception = new BE.Bitacora(ex);
    //        exception.ExcepcionControlada = ExControlada;
    //    }

    //    public BE.Bitacora obtenerExcepcion()
    //    {
    //        return exception;
    //    }

    //}
}
