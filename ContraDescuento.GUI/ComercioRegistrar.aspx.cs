using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class ComercioRegistrar : System.Web.UI.Page
    {
        #region Propiedad Privada
        BLL.Usuario usuarioNeg = new BLL.Usuario();
        BLL.Comercio comercioNeg = new BLL.Comercio();
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        string fileName;
        string logo;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    CargarPreguntas();
                }
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

        private void CargarPreguntas()
        {
            try
            {
                BLL.PreguntaDeSeguridad pregNeg = new BLL.PreguntaDeSeguridad();
                ddlPreguntaDeSeguridad.DataSource = null;
                ddlPreguntaDeSeguridad.DataSource = pregNeg.Listar();
                ddlPreguntaDeSeguridad.DataBind();
                //ddlPreguntaDeSeguridad.Items.Insert(0, new ListItem("-Seleccione-", "-1"));
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar las preguntas: ", "Ocurrió un error al cargar las preguntas: " )+ ex.Message);
            }
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                /*Datos básicos*/
                BE.Usuario usuario = new BE.Usuario();
                usuario.TipoUsuario = new BE.TipoUsuario(BE.EnumTipoUsuario.Comercio);
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Password = txtPassword.Text;
                usuario.FechaDeNacimiento = Convert.ToDateTime(txtFechaDeNacimiento.Text);

                TFL.Fechas fecha = new TFL.Fechas();
                if (fecha.CalcularEdad(usuario.FechaDeNacimiento) < 18)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Debe ser mayor de 18 años.", "Debe ser mayor de 18 años.")));
                }

                if(ddlPreguntaDeSeguridad.SelectedValue == "-1")
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor seleccione una pregunta de seguridad", "Por favor seleccione una pregunta de seguridad")));
                }
                if (txtRespuestaRecuperarPassword.Text == string.Empty)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor ingrese su respuesta de seguridad", "Por favor ingrese su respuesta de seguridad")));
                }

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
                if (rdbtnMujer.Checked)
                    usuario.Sexo = 'M';
                else if (rdbtnHombre.Checked)
                    usuario.Sexo = 'H';
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor elija el sexo", "Por favor elija el sexo")));
                /*Fin datos básicos*/
                /*Domicilio*/
                /*Domicilio Telefono*/

                /*Telefono*/
                usuario.Telefono.Celular = chkCelular.Checked ? true : false;
                usuario.Telefono.NroTelefono = txtNumero.Text;
                usuario.Telefono.Observacion = txtObservacion.Text;
                usuario.Telefono.Caracteristica = txtCaracteristica.Text;
                /*Fin Telefono*/

                /*Datos básicos del comercio*/
                usuario.Comercio = new BE.Comercio();
                usuario.Comercio.NombreComercio = txtComercioNombre.Text;
                usuario.Comercio.Telefono = usuario.Telefono;
                usuario.Comercio.Logo = obtenerArchivoCargado();

                if(usuario.Comercio.Logo == null)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor, suba el logo", "Por favor, suba el logo")));
                }


                /*Fin - Datos básicos del comercio */

                //Alta del usuario administrador del comercio
                usuarioNeg = new BLL.Usuario();
                usuarioNeg.AltaAdminDeComercio(usuario);
                //Alta del comercio
                comercioNeg = new BLL.Comercio();
                usuario.Comercio.Responsable = usuario; //Responsable del comercio;
                comercioNeg.Alta(usuario.Comercio);

                MostrarMensajeOk(TraducirPalabra("El usuario se ha creado éxitosamente", "El usuario se ha creado éxitosamente"));
                this.ClearForm(((System.Web.UI.Control)sender).NamingContainer.Controls);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra(ex.Message,ex.Message));
            }
            catch (FormatException ex)
            {
                MostrarMensajeError(ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra(ex.Message, ex.Message));
            }
        }


        private byte[] obtenerArchivoCargado()
        {
            byte[] logoFile = null;
            try
            {
                if (fLogoUpload.HasFile)
                {
                    fileName = System.IO.Path.GetFileName(fLogoUpload.FileName);
                    logo = System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"] + fileName;
                    if (!System.IO.Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"]))
                        System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"]);
                    fLogoUpload.SaveAs(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"] + fileName);
                    logoFile = System.IO.File.ReadAllBytes(logo);
                    validarArchivoCargado(logoFile);
                    Session["File.Comercio.Logo"] = logoFile;
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            return logoFile;
        }

        private void validarArchivoCargado(byte[] archivo)
        {
            decimal tamanio = archivo.Count() / 1024;
            int tamanioPermitido = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Comercio.Logo.Archivo.TamañoPermitido"])*1024;
            if (tamanio > tamanioPermitido)
                MostrarMensajeError(TraducirPalabra("El tamaño del archivo cargado supera el máximo permitido (", "El tamaño del archivo cargado supera el máximo permitido (") + tamanioPermitido.ToString()+ ")");
  
        }
    }
}