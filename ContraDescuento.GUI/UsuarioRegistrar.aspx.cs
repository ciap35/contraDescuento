using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{

    public partial class UsuarioRegistrar : System.Web.UI.Page
    {
        #region Propiedad Privada
        ContraDescuento.BLL.Usuario usuarioNeg = new BLL.Usuario();
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                CargarPreguntas();
            }
            catch (NullReferenceException ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar la página: ", "Ocurrió un error al cargar la página: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar la página: ", "Ocurrió un error al cargar la página: ") + ex.Message);
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


        private void CargarPreguntas()
        {
            try
            {
                BLL.PreguntaDeSeguridad pregNeg = new BLL.PreguntaDeSeguridad();
                ddlPreguntaDeSeguridad.DataSource = null;
                ddlPreguntaDeSeguridad.DataSource= pregNeg.Listar();
                ddlPreguntaDeSeguridad.DataBind();
                //ddlPreguntaDeSeguridad.Items.Insert(0, new ListItem("-Seleccione-", "-1"));
            }
            catch(Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar las preguntas: ", "Ocurrió un error al cargar las preguntas: ") + ex.Message);
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuario = new BE.Usuario();
                usuario.TipoUsuario = new BE.TipoUsuario(BE.EnumTipoUsuario.Usuario);
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Password = txtPassword.Text;
                usuario.FechaDeNacimiento = Convert.ToDateTime(txtFechaDeNacimiento.Text);
                usuario.PreguntaDeSeguridad.CodPreguntaSeguridad = Convert.ToInt32(ddlPreguntaDeSeguridad.SelectedValue);
                usuario.PreguntaDeSeguridad.Respuesta = txtRespuestaRecuperarPassword.Text;
                BE.Idioma idioma = (BE.Idioma)Session["Idioma"];
                if (idioma != null)
                {
                    usuario.Idioma = idioma;
                }
                else
                {
                    usuario.Idioma = new BE.Idioma() { CodIdioma = 1 };
                }
                usuario.Email = txtEmail.Text;

                TFL.Fechas fecha = new TFL.Fechas();
                if(fecha.CalcularEdad(usuario.FechaDeNacimiento) < 18)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Debe ser mayor de 18 años.", "Debe ser mayor de 18 años.")));
                }
                if (rdbtnMujer.Checked)
                    usuario.Sexo = 'M';
                else if (rdbtnHombre.Checked)
                    usuario.Sexo = 'H';
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor elija el sexo", "Por favor elija el sexo")));
                usuarioNeg = new BLL.Usuario();
                
                usuarioNeg.Alta(usuario);
                MostrarMensajeOk(TraducirPalabra("El usuario se ha creado éxitosamente", "El usuario se ha creado éxitosamente"));
                this.ClearForm(((System.Web.UI.Control)sender).NamingContainer.Controls);
            }
            catch(FormatException ex)
            {
                MostrarMensajeError(ex.Message);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
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
                if(ctrl.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                {
                    System.Web.UI.WebControls.CheckBox chk= (System.Web.UI.WebControls.CheckBox)ctrl;
                    chk.Checked = false;
                }
                if(ctrl.GetType() == typeof(System.Web.UI.WebControls.RadioButton))
                {
                    System.Web.UI.WebControls.RadioButton rdbtn = (System.Web.UI.WebControls.RadioButton)ctrl;
                    rdbtn.Checked = false;
                }
               if(ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    System.Web.UI.HtmlControls.HtmlInputRadioButton rdbtn = (System.Web.UI.HtmlControls.HtmlInputRadioButton)ctrl;
                    rdbtn.Checked = false;
                }
               if(ctrl.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    System.Web.UI.WebControls.DropDownList dropDownLst = (System.Web.UI.WebControls.DropDownList)ctrl;
                    if (dropDownLst.Items.Count > 0)
                    {
                        dropDownLst.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}