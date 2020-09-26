using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI.UserControl
{
    public partial class ucLogin : System.Web.UI.UserControl
    {
        #region "Propiedades privadas"
        BLL.Usuario usuarioNeg = new BLL.Usuario();
        BE.Usuario usuario = null;
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            
            LimpiarMensaje();
            try
            {
                Traducir(((BE.Idioma)Session["Idioma"]));
            }
            catch(Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la pantalla de login: ", "Ocurrió un error al traducir la pantalla de login: " )+ ex.Message);
            }
        }


        private void Traducir(BE.Idioma idioma)
        {
            try
            {
                if (idioma != null) { 
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                traductor.Idioma = idioma;
                lstTraduccion.Clear();
               
                    foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                    {
                        lstTraduccion.Add(traduc.Traduccion);
                    }
                    if (lstTraduccion.Count > 0)
                    {
                        foreach(BE.Traduccion traduccion in lstTraduccion)
                        {
                            var control = this.FindControl(traduccion.ControlID);

                           
                            if(control!=null )
                            {
                                if (control.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                {
                                    System.Web.UI.WebControls.TextBox ctrlPage = (System.Web.UI.WebControls.TextBox)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.WebControls.LinkButton))
                                {
                                    System.Web.UI.WebControls.LinkButton ctrlPage = (System.Web.UI.WebControls.LinkButton)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                                {
                                    System.Web.UI.HtmlControls.HtmlAnchor ctrlPage = (System.Web.UI.HtmlControls.HtmlAnchor)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.InnerText = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.WebControls.Literal))
                                {
                                    System.Web.UI.WebControls.Literal ctrlPage = (System.Web.UI.WebControls.Literal)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                                {
                                    System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.InnerText = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.WebControls.Button))
                                {
                                    System.Web.UI.WebControls.Button ctrlPage = (System.Web.UI.WebControls.Button)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                                }
                                if (control.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                {
                                    System.Web.UI.WebControls.CheckBox ctrlPage = (System.Web.UI.WebControls.CheckBox)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al obtener el idioma: ", "Ocurrió un error al obtener el idioma: " )+ ex.Message);
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string pwd = txtPassword.Text;
                BE.Usuario usuario = null;
                BE.Enum_Status_Login status = usuarioNeg.Login(txtEmail.Text, ref pwd, ref usuario);
                if (status == BE.Enum_Status_Login.Exitoso && usuario.CodUsuario > 0)
                {
                    usuario = usuarioNeg.Obtener(usuario);
                    Session["usuario"] = usuario;
                    Session["Idioma"] = usuario.Idioma;
                    if (usuario.TipoUsuario != null && usuario.TipoUsuario.Comercio)
                    {
                        System.Web.HttpContext.Current.Session["Usuario"] = usuario;
                        Response.Redirect("ComercioAdministracion.aspx");
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
                else
                {
                    switch (status)
                    {
                        case BE.Enum_Status_Login.UsuarioPwdInvalido:
                            MostrarError(TraducirPalabra("Usuario y/o password inválidos", "Usuario y/o password inválidos")); break;
                        case BE.Enum_Status_Login.Bloqueado:
                            btnReestablecerCuenta.Visible = true;
                            btnReestablecerCuenta.Text = TraducirPalabra("Reactivar mi cuenta", "Reactivar mi cuenta");
                            MostrarError(TraducirPalabra("Cuenta bloqueada. Haga click sobre el siguiente botón", "Cuenta bloqueada. Haga click sobre el siguiente botón"));
                            break;
                        case BE.Enum_Status_Login.Inexistente:
                            MostrarError(TraducirPalabra("Cuenta inhabilitada o inexistente", "Cuenta inhabilitada o inexistente")); break;
                        case BE.Enum_Status_Login.Baja:
                            btnReestablecerCuenta.Visible = true;
                            MostrarError(TraducirPalabra("La cuenta se ha dado de baja anteriormente.\n\rPor favor reestablezca su cuenta desde aquí: ", "La cuenta se ha dado de baja anteriormente.\n\rPor favor reestablezca su cuenta desde aquí: "));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }


        private string TraducirPalabra(string palabra, string clave)
        {
            #region traducir texto
            string resultado = string.Empty;
            BE.Idioma idiomaUsuario = null;
            string traduccionResultado = palabra;
            if ((BE.Idioma)Session["Idioma"] != null)
            {
                idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion



                try
                {
                    BLL.Traductor traduccionNeg = new BLL.Traductor();
                    BE.Traductor traductor = new BE.Traductor();
                    traductor.Idioma = idiomaUsuario;
                    lstTraduccion.Clear();
                    if (idiomaUsuario.Descripcion == string.Empty)
                    {
                        BLL.Idioma IdiomaNeg = new BLL.Idioma();
                        idiomaUsuario = IdiomaNeg.Obtener(idiomaUsuario);
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
                            if (traduccionTexto == clave && traduccion.Texto.ToUpper().Replace(" ","") != "LOREMIPSUM")
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

                    MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
                    //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
                }
            }
            return traduccionResultado;
        }


        private void MostrarMensajeOK(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
        }

        private void MostrarError(string mensaje)
        {
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = mensaje.ToUpper();
            divMensajeOK.Visible = false;
            //btnReestablecerCuenta.Visible = false;
        }


        private void LimpiarMensaje()
        {
            divMensajeError.Visible = false;
            litMensajeError.Visible = false;
            litMensajeError.Text = "";
        }

        protected void btnReestablecerCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                btnReestablecerCuenta.Visible = false;
                LimpiarMensaje();
                BLL.Usuario usuarioNeg = new BLL.Usuario();
                BE.Usuario usuario = new BE.Usuario()
                {
                    Email = txtEmailRecuperarPassword.Text
                };
                if (usuarioNeg.EnviarEmailDesbloqueo(usuario))
                {
                    LimpiarMensaje();
                    MostrarMensajeOK(TraducirPalabra("Se ha enviado un e-mail a su cuenta, verifique", "Se ha enviado un e-mail a su cuenta, verifique"));
                    ClearForm(((System.Web.UI.Control)sender).NamingContainer.Controls);
                }
                else
                {
                    MostrarError(TraducirPalabra("No se ha podido enviar el e-mail, por favor responda su pregunta secreta", "No se ha podido enviar el e-mail, por favor responda su pregunta secreta"));
                    txtRespuestaEmailRecuperarPassword.Text = txtEmailRecuperarPassword.Text;
                    //Mostrar el formulario de preguntas ya que el e-mail no se pudo enviar.
                    MostrarPreguntasDeSeguridad();
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        public void MostrarPreguntasDeSeguridad()
        {
            try
            {
                this.Visible = true;
                frmLogin.Visible = frmRecuperarPassword.Visible = false;
                frmRecuperarPasswordPreguntas.Visible = true;
                BLL.PreguntaDeSeguridad preguntaDeSeguridad = new BLL.PreguntaDeSeguridad();
                ddlPreguntaDeSeguridad.DataSource = null;
                ddlPreguntaDeSeguridad.DataSource =  preguntaDeSeguridad.Listar();
                ddlPreguntaDeSeguridad.DataBind();
                //ddlPreguntaDeSeguridad.Items.Insert(0, new ListItem("-Seleccione-", "-1"));
            }
            catch(BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                bitacora.ExcepcionControlada = true;
                exceptionLogger.Grabar(bitacora);
            }
            catch(Exception ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
            }
        }

        public void ClearForm(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    System.Web.UI.WebControls.TextBox txt = (System.Web.UI.WebControls.TextBox)ctrl;
                    txt.Text = String.Empty;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)ctrl;
                    chk.Checked = false;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.WebControls.RadioButton))
                {
                    System.Web.UI.WebControls.RadioButton rdbtn = (System.Web.UI.WebControls.RadioButton)ctrl;
                    rdbtn.Checked = false;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    System.Web.UI.HtmlControls.HtmlInputRadioButton rdbtn = (System.Web.UI.HtmlControls.HtmlInputRadioButton)ctrl;
                    rdbtn.Checked = false;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputText))
                {
                    System.Web.UI.HtmlControls.HtmlInputText inputTxt = (System.Web.UI.HtmlControls.HtmlInputText)ctrl;
                    inputTxt.Value = string.Empty;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputPassword))
                {
                    System.Web.UI.HtmlControls.HtmlInputPassword inputTxtPwd = (System.Web.UI.HtmlControls.HtmlInputPassword)ctrl;
                    inputTxtPwd.Value = string.Empty;
                }
                if (ctrl.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    System.Web.UI.WebControls.DropDownList dropDownLst = (System.Web.UI.WebControls.DropDownList)ctrl;
                    if (dropDownLst.Items.Count > 0)
                    {
                        dropDownLst.SelectedIndex = 0;
                    }
                }
            }
        }

        protected void litRecordarPassword_Click(object sender, EventArgs e)
        {
            //Traducir texto
            //litTitleLoginModal.Text = "Recuperar mi password";
            mostrarFrmEnviarEmail();
        }

        private void mostrarFrmEnviarEmail()
        {
            this.Visible = true;
            litTitleRecuperarPasswordModal.Visible = true;
            litTitleLoginModal.Visible = false;
            frmRecuperarPassword.Visible = true;
            frmLogin.Visible = false;
            frmRecuperarPasswordPreguntas.Visible = false;
        }

        protected void btnValidarPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.PreguntaDeSeguridad preguntaDeSeguridadNeg = new BLL.PreguntaDeSeguridad();
                usuario = new BE.Usuario();
                usuario.Email = txtRespuestaEmailRecuperarPassword.Text;
                usuario.PreguntaDeSeguridad.EntradaRespuestaUsuario = txtRespuestaRecuperarPassword.Text;
                usuario.PreguntaDeSeguridad.EntradaCodPreguntaUsuario = Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue);
                if(preguntaDeSeguridadNeg.ValidarEntrada(ref usuario))
                {
                    //Mostrar controles para blanquear password.
                    frmRecuperarPasswordPreguntas.Visible = false;
                    frmChangePassword.Visible = true;
                    Session["UsuarioTemp"] = usuario;
                }
                else
                {
                    txtRespuestaRecuperarPassword.Text = string.Empty;
                    ddlPreguntaDeSeguridad.SelectedIndex = 0;
                    MostrarError(TraducirPalabra("La pregunta y/o respuesta son incorrectas", "La pregunta y/o respuesta son incorrectas"));
                }
            }
            catch(BE.ExcepcionPersonalizada ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al validar la pregunta:", "Ocurrió un error al validar la pregunta: ") + TraducirPalabra(ex.Message, ex.Message));
            }
            catch(Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al validar la pregunta:", "Ocurrió un error al validar la pregunta: " )+ ex.Message);
            }
        }

        protected void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            try
            {
                
                BLL.Usuario usuarioNeg = new BLL.Usuario();
                usuario = (BE.Usuario)Session["UsuarioTemp"];
                usuario.Password = txtPasswordNueva.Text;
             
                usuarioNeg.CambiarPassword(usuario);
                usuario = usuarioNeg.Obtener(usuario);
                if (usuario.FechaBaja == DateTime.MinValue)
                {
                    MostrarMensajeOK(TraducirPalabra("¡Bienvenido nuevamente!", "¡Bienvenido nuevamente!"));
                }
                else
                {
                    MostrarMensajeOK(TraducirPalabra("Se ha cambiado la password correctamente", "Se ha cambiado la password correctamente"));
                }
                
                frmChangePassword.Visible = false;
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al validar la pregunta: ", "Ocurrió un error al validar la pregunta: " )+ ex.Message);
            }
        }
    }
}