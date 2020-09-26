using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class PreguntaDeSeguridad
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.PreguntaDeSeguridad preguntaDeSeguridadDAL = new DAL.PreguntaDeSeguridad();
        TFL.SymetricCryptool SymCrypto = null;
        
        #endregion

        public PreguntaDeSeguridad() { }


        public BE.PreguntaDeSeguridad Obtener(BE.PreguntaDeSeguridad preguntaDeSeguridad)
        {
            BE.PreguntaDeSeguridad preguntaDeSeg = null;
            try
            {
                preguntaDeSeg = preguntaDeSeguridadDAL.Obtener(preguntaDeSeguridad);
                if (preguntaDeSeg == null)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se encontró pregunta de seguridad."));
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            return preguntaDeSeg;
        }

        public List<BE.PreguntaDeSeguridad> Listar()
        {
            List<BE.PreguntaDeSeguridad> lstPreguntaDeSeguridad = null;
            try
            {
                lstPreguntaDeSeguridad = preguntaDeSeguridadDAL.Listar();
                if (lstPreguntaDeSeguridad == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se han encontrado preguntas de seguridad."));
            }
            catch(BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            return lstPreguntaDeSeguridad;
        }
        public string Encriptar(ref string Entrada)
        {
            try
            {
                if (Entrada == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Debe contestar la pregunta de seguridad"));

                SymCrypto = new TFL.SymetricCryptool();
                return SymCrypto.Encrypt(ref Entrada);
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

        /// <summary>
        /// La Respuesta del usuario ya debe estar cargada en memoria al momento de comparar.
        /// </summary>
        /// <param name="preguntaDeSeguridad">Objeto que representa la pregunta y respuesta del usuario</param>
        /// <returns></returns>
        public bool ValidarEntrada(ref BE.Usuario usuario)
        {
            try
            {
                if (usuario.Email == string.Empty || usuario.PreguntaDeSeguridad.EntradaRespuestaUsuario == string.Empty || usuario.PreguntaDeSeguridad.EntradaCodPreguntaUsuario == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Debe contestar la pregunta de seguridad"));

                BLL.Usuario usuarioNeg = new BLL.Usuario();
                usuario =  usuarioNeg.BuscarPorEmail(usuario);
                if (usuario != null && usuario.CodUsuario > 0)
                {
                    string EntradaRespuestaUsuario = usuario.PreguntaDeSeguridad.EntradaRespuestaUsuario.ToUpper();
                    EntradaRespuestaUsuario = Encriptar(ref EntradaRespuestaUsuario);
                    string UsuarioRespuesta = usuario.PreguntaDeSeguridad.Respuesta;
                   
                    //usuario.PreguntaDeSeguridad.Respuesta = Encriptar(ref UsuarioRespuesta);
                    TFL.SymetricCryptool has = new TFL.SymetricCryptool();
                    var resultado =  has.Decrypt(usuario.PreguntaDeSeguridad.Respuesta);

                    return (usuario.PreguntaDeSeguridad.EntradaCodPreguntaUsuario == usuario.PreguntaDeSeguridad.CodPreguntaSeguridad && EntradaRespuestaUsuario == usuario.PreguntaDeSeguridad.Respuesta);
                }
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Usuario inexistente"));
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
            return false;
        }
    }
    //public class PreguntaDeSeguridadNegocioException : System.Exception
    //{
    //    private BE.Bitacora exception = null;
    //    public PreguntaDeSeguridadNegocioException(bool ExControlada, Exception ex) : base(String.Format("Error: {0}", ex.Message))
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
