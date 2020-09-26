using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class AccountManager : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string tipoOperacion = Request.QueryString["tipoOperacion"].ToString();
                    if (tipoOperacion != string.Empty)
                        GestionarOperacion(tipoOperacion);

                }

            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private void GestionarOperacion(string tipoOperacion)
        {
            try
            {
                string token = Request.QueryString["token"];
                if (tipoOperacion == ((int)BE.Enum_Tipo_Email.Desbloqueo).ToString()) {
                    DesbloquearCuenta(token);
                    ucLogin.MostrarPreguntasDeSeguridad();
                 //MostrarMensajeOK("Cuenta desbloqueada éxitosamente");
                }
                else if (tipoOperacion == ((int)BE.Enum_Tipo_Email.RehabilitarCuenta).ToString()) { 
                    RehabilitarCuenta(token);

                    //MostrarMensajeOK("Cuenta rehabilitada éxitosamente");


                    #region traducir texto
                    string resultado = string.Empty;
                    BE.Idioma idiomaUsuario = null;

                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];
                    #endregion
                    MostrarMensajeOK(TraducirPalabra(idiomaUsuario, "Cuenta rehabilitada éxitosamente","Cuenta rehabilitada éxitosamente"));
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(ex.Message);
            }
        }

        private string TraducirPalabra(BE.Idioma idioma, string palabra,string clave)
        {
            string traduccionResultado = palabra;
            try
            {
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                traductor.Idioma = idioma;
                lstTraduccion.Clear();
                if (idioma.Descripcion == string.Empty)
                {
                    BLL.Idioma IdiomaNeg = new BLL.Idioma();
                    idioma = IdiomaNeg.Obtener(idioma);
                }

                foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                {
                    lstTraduccion.Add(traduc.Traduccion);
                }
                if (lstTraduccion.Count > 0)
                {
                    foreach (BE.Traduccion traduccion in lstTraduccion)
                    {
                        string traduccionTexto = traduccion.ControlID.ToString().Replace(" ", "").ToUpper();
                        clave = clave.Replace(" ", "").ToUpper();
                        if (traduccionTexto == clave && traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                        {
                            traduccionResultado = traduccion.Texto;
                        }
                    }
                }
                return traduccionResultado;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);



                #region traducir texto
                string resultado = string.Empty;
                BE.Idioma idiomaUsuario = null;

                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion
                MostrarError(TraducirPalabra(idiomaUsuario, "Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ")+ex.Message);
                //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
            return traduccionResultado;
        }


        private void RehabilitarCuenta(string token)
        {
            try
            {

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(ex.Message);
            }
        }
        private void MostrarError(string mensaje)
        {
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = mensaje.ToUpper();
        }

        private void MostrarMensajeOK(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
        }


        private void DesbloquearCuenta(string token)
        {
            try
            {
                if (token != string.Empty)
                {
                    BLL.Usuario usuarioNeg = new BLL.Usuario();
                    usuarioNeg.DesbloquearCuenta(token);
                }
                else {
                    #region traducir texto
                    string resultado = string.Empty;
                    BE.Idioma idiomaUsuario = null;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];
                    #endregion
                    TraducirPalabra(idiomaUsuario, "Token inválido", "Token inválido");

                    throw new Exception(TraducirPalabra(idiomaUsuario, "Token inválido", "Token inválido"));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(ex.Message);
            }
        }
    }
}