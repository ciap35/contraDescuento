using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        #region Propiedad Privada
        ContraDescuento.BLL.Usuario usuarioNeg = new BLL.Usuario();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        //BE.Usuario usuario = new BE.Usuario(BE.Enum_Perfil.Usuario);
        BE.Usuario usuario = new BE.Usuario();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    obtenerUsuario();
                    CargarPreguntasDeSeguridad();
                }
            }
            catch (NullReferenceException ex)
            {
                MostrarMensajeError(TraducirPalabra("No se ha podido obtener el perfil del usuario, logueese por favor: ", "No se ha podido obtener el perfil del usuario, logueese por favor: ") + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("No se ha podido obtener el perfil del usuario, logueese por favor: ", "No se ha podido obtener el perfil del usuario, logueese por favor: ") + ex.Message);
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

                    MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
                    //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
                }
            }
            return traduccionResultado;
        }

        private void obtenerUsuario()
        {
            try
            {
                if (Session["usuario"] != null)
                {
                    usuario = (BE.Usuario)Session["usuario"];
                    txtNombre.Text = usuario.Nombre;
                    txtApellido.Text = usuario.Apellido;
                    txtFechaDeNacimiento.Text = usuario.FechaDeNacimiento.ToString("dd/MM/yyyy");
                    ddlIdioma.SelectedValue = usuario.Idioma.CodIdioma.ToString();
                    if(usuario.PreguntaDeSeguridad != null && usuario.PreguntaDeSeguridad.CodPreguntaSeguridad > 0)
                    ddlPreguntaDeSeguridad.SelectedValue = usuario.PreguntaDeSeguridad.CodPreguntaSeguridad.ToString();
                    
                    txtEmail.Text = usuario.Email;
                    if (usuario.Sexo.Value == 'H')
                        rdbtnHombre.Checked = true;
                    if (usuario.Sexo.Value == 'M')
                        rdbtnMujer.Checked = true;
                }
                else
                {
                    MostrarMensajeError(TraducirPalabra("Por favor logueese", "Por favor logueese"));
                    Response.Redirect("Index.aspx");
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Error al mostrar la información del usuario, por favor reintente más tarde.", "Error al mostrar la información del usuario, por favor reintente más tarde."));
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuarioAntiguo = (BE.Usuario)Session["usuario"];
                usuario = new BE.Usuario();
                usuario.CodUsuario = usuarioAntiguo.CodUsuario;
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Password = txtPassword.Text;
                usuario.FechaDeNacimiento = Convert.ToDateTime(txtFechaDeNacimiento.Text);
                usuario.Idioma = new BE.Idioma() { CodIdioma = Convert.ToInt32(ddlIdioma.SelectedValue) };
                usuario.PreguntaDeSeguridad = usuarioAntiguo.PreguntaDeSeguridad;
                usuario.PreguntaDeSeguridad.Respuesta = usuarioAntiguo.PreguntaDeSeguridad.Respuesta;
                usuario.Email = txtEmail.Text;
                usuario.TipoUsuario = usuarioAntiguo.TipoUsuario;
                if (rdbtnMujer.Checked)
                    usuario.Sexo = 'M';
                else if (rdbtnHombre.Checked)
                    usuario.Sexo = 'H';
                else
                    throw new ArgumentNullException(TraducirPalabra("Por favor elija el sexo", "Por favor elija el sexo"));
                usuario.Password = txtPassword.Text;

                if(txtRespuestaRecuperarPassword.Text != string.Empty && Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue) <= 0)
                {
                    throw new ArgumentNullException(TraducirPalabra("Por favor seleccione una pregunta", "Por favor seleccione una pregunta"));
                }
                else if(usuario.PreguntaDeSeguridad.CodPreguntaSeguridad != Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue) && (txtRespuestaRecuperarPassword.Text == string.Empty && Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue) >= 0))
                {
                    throw new ArgumentNullException(TraducirPalabra("Por favor complete la respuesta a la pregunta de seguridad", "Por favor complete la respuesta a la pregunta de seguridad"));
                }
                usuario.PreguntaDeSeguridad.CodPreguntaSeguridad = Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue);
                usuario.PreguntaDeSeguridad.Respuesta = txtRespuestaRecuperarPassword.Text.ToUpper();

                usuarioNeg = new BLL.Usuario();
                usuarioNeg.Modificar(usuario, usuarioAntiguo);
                Session["usuario"] =  usuarioNeg.Obtener(usuario);
                MostrarMensajeOk(TraducirPalabra("Usuario actualizado éxitosamente", "Usuario actualizado éxitosamente"));
                Response.ClearHeaders();
                Response.AddHeader("REFRESH", "3;URL=MiPerfil.aspx");
                
            }
            catch(ArgumentNullException ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex,true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            catch (FormatException ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex,true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
        }

        private void MostrarMensajeError(string error)
        {
            divMensajeOK.Visible = false;
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = error.ToUpper();
        }

        private void MostrarMensajeOk(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                usuarioNeg = new BLL.Usuario();
                usuarioNeg.Baja((BE.Usuario)Session["usuario"]);
                MostrarMensajeOk(TraducirPalabra("Cuenta Eliminada", "Cuenta Eliminada"));

                Session["usuario"] = null;
                Response.Redirect("Index.aspx");
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
        }

        private void CargarPreguntasDeSeguridad()
        {
            try
            {
                BLL.PreguntaDeSeguridad pregSeguridadNeg = new BLL.PreguntaDeSeguridad();
                ddlPreguntaDeSeguridad.DataSource = null;
                ddlPreguntaDeSeguridad.DataSource = pregSeguridadNeg.Listar();
                ddlPreguntaDeSeguridad.DataBind();
                ddlPreguntaDeSeguridad.Items.Insert(0, new ListItem(TraducirPalabra("-Seleccione-", "-Seleccione-"), "-1"));
            }
            catch(Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar preguntas de seguridad: ", "Ocurrió un error al cargar preguntas de seguridad: ") + ex.Message);
            }
        }
    }
}