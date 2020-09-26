using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class DigitoVerificador : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PanelDigitoVerificador.Visible = true;
            
            try
            {
                if (ValidarPermisos()) {
                    PanelDigitoVerificador.Visible = true;
                    btnValidarDigitoVerificador.Visible = true;
                }
                else
                {
                    PanelDigitoVerificador.Visible = false;
                    btnValidarDigitoVerificador.Visible = false;
                }
                if (!IsPostBack)
                {

                    rptRegCorrupto.DataSource = null;
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error, por favor intente nuevamente más tarde.", "Ocurrió un error, por favor intente nuevamente más tarde."));
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

                    MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
                    //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
                }
            }
            return traduccionResultado;
        }


        protected void btnValidarDigitoVerificador_Click(object sender, EventArgs e)
        {
            try
            {
                
                BLL.DigitoVerificador digVerif = new BLL.DigitoVerificador();
                List<BE.IAuditable> lstRegCorruptos = digVerif.ValidarDigitosVerificadores();


                if (lstRegCorruptos.Count > 0)
                {
                    //Mostrar registros corruptos.
                    MostrarError(TraducirPalabra("Se han encontrado registros corruptos.\n\r Total: ", "Se han encontrado registros corruptos.\n\r Total: ") + lstRegCorruptos.Count);
                    BindRegistrosCorruptos(lstRegCorruptos);
                }
                else
                {
                    rptRegCorrupto.Visible = false;
                    MostrarMensajeOK(TraducirPalabra("No se han identificado registros corruptos", "No se han identificado registros corruptos"));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar los dígitos verificadores, por favor intente nuevamente más tarde.", "Ocurrió un error al validar los dígitos verificadores, por favor intente nuevamente más tarde."));
            }
        }

        private bool ValidarPermisos()
        {
            bool valido = false;
            try
            {
                string URL = this.Context.Request.CurrentExecutionFilePath.Replace("/", "");
                if (System.Web.HttpContext.Current.Session != null && HttpContext.Current.Session["Usuario"] != null)
                {
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    if (usuario.TipoUsuario != null && usuario.TipoUsuario.Grupo != null && usuario.TipoUsuario.Grupo.LstGrupos.Count > 0)
                    {
                        BLL.Permiso permisoNeg = new BLL.Permiso();
                        permisoNeg.ValidarSolicitud(URL, usuario.TipoUsuario.Grupo);
                        if (!permisoNeg.PeticionValida)
                        {
                            MostrarError(TraducirPalabra("No posee permisos para ingresar a esta página", "No posee permisos para ingresar a esta página"));
                            Response.ClearHeaders();
                            Response.AddHeader("REFRESH", "5;URL=Index.aspx");
                        }
                        else
                        {
                            valido = true;
                        }
                    }
                    else
                    {
                        if (usuario.TipoUsuario == null) //Es administrador
                        {
                            valido = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Logueese.aspx");
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            return valido;
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
        }

        private void BindRegistrosCorruptos(List<BE.IAuditable> LstRegCorruptos)
        {
            try
            {
                rptRegCorrupto.DataSource = LstRegCorruptos;
                rptRegCorrupto.DataBind();
                rptRegCorrupto.Visible = true;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al realizar bind de registros corruptos: ", "Ocurrió un error al realizar bind de registros corruptos: ") + ex.Message);
            }
        }

        protected void rptRegCorrupto_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.DataItem != null)
                {
                    string CodRegistro = "Cod" + (e.Item.DataItem).GetType().Name.ToString();
                    System.Reflection.PropertyInfo propInf = e.Item.DataItem.GetType().GetProperty(CodRegistro);
                    if (propInf != null)
                    {
                        //Resuelvo obtener la propiedad pública que representa la PK del objeto. Se hace de esta manera en caso de tener que mostrar objetos de distinto tipo.
                        ((Literal)e.Item.FindControl("litCodigoRegistro")).Text  = e.Item.DataItem.GetType().GetProperty(CodRegistro).GetValue(e.Item.DataItem).ToString();
                        ((Literal)e.Item.FindControl("litInfoRegistro")).Text = e.Item.DataItem.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar Perfil: ", "Ocurrió un error al validar Perfil: ") + ex.Message);
            }
        }
    }
}