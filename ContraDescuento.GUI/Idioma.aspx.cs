using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using static iTextSharp.text.Font;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;

namespace ContraDescuento.GUI.Admin
{

    public partial class Idioma : System.Web.UI.Page
    {
        #region Propiedad Privada
        ContraDescuento.BLL.Usuario usuarioNeg = new BLL.Usuario();
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Idioma> lstIdioma = new List<BE.Idioma>();
        List<BE.Traductor> lstTraductor = new List<BE.Traductor>();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        BE.Idioma idiomaSeleccionado = new BE.Idioma();
        BE.Idioma idiomaCached = null;
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            divMensajeError.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    
                    if (ValidarPermisos())
                    {  BindIdiomaGridView();
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        private string TraducirPalabra(BE.Idioma idioma,string palabra, string clave)
        {
            #region traducir texto
            string resultado = string.Empty;
            BE.Idioma idiomaUsuario = null;
            string traduccionResultado = palabra;
            if (idioma != null)
            {
                idiomaUsuario = idioma;
                #endregion



                try
                {
                    BLL.Traductor traduccionNeg = new BLL.Traductor();
                    BE.Traductor traductor = new BE.Traductor();
                    traductor.Idioma = idiomaUsuario;
                   List<BE.Traduccion> lstTradGrilla = new List<BE.Traduccion>();
                    if (idiomaUsuario.Descripcion == string.Empty)
                    {
                        BLL.Idioma IdiomaNeg = new BLL.Idioma();
                        idiomaUsuario = IdiomaNeg.Obtener(idiomaUsuario);
                    }

                    foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                    {

                        lstTradGrilla.Add(traduc.Traduccion);
                    }
                    if (lstTradGrilla.Count > 0)
                    {
                        foreach (BE.Traduccion traduccion in lstTradGrilla)
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

        protected void gvIdioma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIdioma.PageIndex = e.NewPageIndex;
                BindIdiomaGridView();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cambiar de página: ", "Ocurrió un error al cambiar de página: ") + ex.Message);
            }
        }

        protected void gvIdioma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            BLL.Idioma idiomaNeg = null;
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {
                    BE.Idioma idioma = null;
                    switch (e.CommandName)
                    {
                        case "VerTraducciones":
                            if (e.CommandArgument != null)
                            {
                                idiomaNeg = new BLL.Idioma();
                                hdnIdiomaEditar.Value = e.CommandArgument.ToString();
                                idiomaSeleccionado = obtenerIdiomaCache().Where(x => x.CodIdioma == Convert.ToInt32(hdnIdiomaEditar.Value)).First<BE.Idioma>();
                                litIdiomaSeleccionado.Text = idiomaSeleccionado.Descripcion.ToUpper();
                                litIdiomaSeleccionado.Visible = false;
                                establecerIdiomaCache(idiomaSeleccionado);
                                btnCrearTraduccion.Visible = true;
                                divTraducciones.Visible = true;
                                BindTraduccionesGridView();
                                litTituloTraduccion.Text =  idiomaSeleccionado.Descripcion.ToUpper();
                            }

                            break;
                        case "EditarIdioma":

                            if (e.CommandArgument != null)
                            {
                                idioma = new BE.Idioma();
                                idioma.CodIdioma = Convert.ToInt32(e.CommandArgument.ToString());
                                System.Web.UI.WebControls.HiddenField hdnDescripcion = (System.Web.UI.WebControls.HiddenField)(((System.Web.UI.Control)(e.CommandSource)).NamingContainer).FindControl("hdnDescripcion");
                                hdnIdiomaEditar.Value = e.CommandArgument.ToString();
                                MostrarIdiomaFormularioEdicion(true);

                                txtIdiomaEditar.Text = obtenerIdiomaCache().Where(x => x.CodIdioma == Convert.ToInt32(hdnIdiomaEditar.Value)).First<BE.Idioma>().Descripcion;

                                MostrarIdiomaFormularioNuevo(false);
                            }

                            break;
                        case "BorrarIdioma":
                            if (e.CommandArgument != null)
                            {
                                idioma = new BE.Idioma();
                                idioma.CodIdioma = Convert.ToInt32(e.CommandArgument.ToString());
                                idiomaNeg = new BLL.Idioma();
                                idiomaNeg.Baja(idioma);
                                BindIdiomaGridView();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
            }
        }

        private void MostrarIdiomaFormularioEdicion(bool mostrar)
        {
            frmIdiomaEdicion.Visible = mostrar;
            txtIdiomaEditar.Visible = mostrar;
            btnIdiomaEditar.Visible = mostrar;
            if (!mostrar)
                txtIdiomaEditar.Text = string.Empty;
        }

        private void MostrarIdiomaFormularioNuevo(bool mostrar)
        {
            frmIdiomaNuevo.Visible = mostrar;
            txtIdiomaNuevo.Visible = mostrar;
            btnIdiomaCrear.Visible = mostrar;
            if (!mostrar)
                txtIdiomaNuevo.Text = string.Empty;
        }

        protected void gvIdioma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
                #region Traduccion columnas
                BE.Idioma idiomaUsuario = null;
                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion
            try
            {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {

                        if (idiomaUsuario != null)
                        {
                            foreach (System.Web.UI.WebControls.DataControlFieldCell row in e.Row.Cells)
                            {

                                object ctlColumna = (object)row;
                                if (ctlColumna != null)
                                    Traducir(idiomaUsuario, ref ctlColumna);
                            }

                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton lnkVerTraducciones = (LinkButton)e.Row.FindControl("lnkVerTraducciones");
                        if (lnkVerTraducciones != null)
                        {
                            object ctlColumna = (object)lnkVerTraducciones;
                            if (ctlColumna != null)
                                Traducir(idiomaUsuario, ref ctlColumna);
                        }
                        Button btnEditarIdioma = (Button)e.Row.FindControl("btnEditarIdioma");
                        if (btnEditarIdioma != null)
                        {
                            object ctlColumna = (object)btnEditarIdioma;
                            if (ctlColumna != null)
                                Traducir(idiomaUsuario, ref ctlColumna);
                        }
                        Button btnBorrarIdioma = (Button)e.Row.FindControl("btnBorrarIdioma");
                        if (btnBorrarIdioma != null)
                        {
                            object ctlColumna = (object)btnBorrarIdioma;
                            if (ctlColumna != null)
                                Traducir(idiomaUsuario, ref ctlColumna);
                        }
                    }
                }
                catch (Exception ex)
                {
                    BE.Bitacora exception = new BE.Bitacora(ex);
                    exceptionLogger.Grabar(exception);
                    MostrarMensajeError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
                }
         
        }

        private void BindIdiomaGridView()
        {
            try
            {
                BLL.Idioma idiomaNeg = new BLL.Idioma();
                lstIdioma = idiomaNeg.Listar();
                establecerIdiomaCache(lstIdioma);
                Session["Idioma_Export"] = lstIdioma;
                gvIdioma.DataSource = lstIdioma;
                gvIdioma.DataBind();
                MostrarIdiomaFormularioNuevo(false);
                divTraducciones.Visible = false;

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al bindear la grilla de idiomas: ", "Ocurrió un error al bindear la grilla de idiomas: ") + ex.Message);
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
                            MostrarMensajeError(TraducirPalabra("No posee permisos para ingresar a esta página", "No posee permisos para ingresar a esta página"));
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
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            return valido;
        }


        protected void btnIdiomaEditar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Idioma idioma = null;
                idioma = obtenerIdiomaCache().Where(x => x.CodIdioma == Convert.ToInt32(hdnIdiomaEditar.Value)).FirstOrDefault<BE.Idioma>();
                if (idioma == null)
                    throw new Exception("btnIdiomaEditar_Click");
                idioma.Descripcion = txtIdiomaEditar.Text;
                BLL.Idioma idiomaNeg = new BLL.Idioma();
                idiomaNeg.Modificacion(idioma);
                BindIdiomaGridView();
                MostrarIdiomaFormularioEdicion(false);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
        }

        protected void btnIdiomaCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdiomaNuevo.Text == string.Empty)
                {
                    MostrarMensajeError(TraducirPalabra("Por favor verifique los datos ingresados", "Por favor verifique los datos ingresados"));
                }
                else
                {
                    BE.Idioma idioma = new BE.Idioma();
                    idioma.Descripcion = txtIdiomaNuevo.Text;
                    BLL.Idioma idiomaNeg = new BLL.Idioma();
                    idiomaNeg.Alta(idioma);
                    BindIdiomaGridView();
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
        }

        protected void btnIdiomaNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarIdiomaFormularioNuevo(true);
                MostrarIdiomaFormularioEdicion(false);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
        }

        private void establecerIdiomaCache(List<BE.Idioma> lstIdioma)
        {
            if (lstIdioma.Count > 0)
                Session["lstIdiomaBinded"] = lstIdioma;
        }

        private void establecerIdiomaCache(BE.Idioma idioma)
        {
            if (idioma != null )
                Session["IdiomaCached"] = idioma;
        }

        private void establecerTraductorCache(BE.Traductor traductor)
        {
            if(traductor!=null)
                Session["TraductorCached"] = traductor;
        }

        private List<BE.Idioma> obtenerIdiomaCache()
        {

            try
            {
                lstIdioma = (List<BE.Idioma>)Session["lstIdiomaBinded"];

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
            return lstIdioma;
        }

        private BE.Idioma obtenerIdiomaSeleccionado()
        {

            try
            {
                idiomaCached = (BE.Idioma)Session["IdiomaCached"];

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
            return idiomaCached;
        }

        private void establecerTraductorCache(List<BE.Traductor> lstTraductor)
        {
            if (lstTraductor.Count > 0)
                Session["lstTraductorBinded"] = lstTraductor;
        }

        private List<BE.Traductor> obtenerTraductorCache()
        {

            try
            {
                lstTraductor = (List<BE.Traductor>)Session["lstTraductorBinded"];

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
            return lstTraductor;
        }

        private void BindTraduccionesGridView()
        {
            try
            {
                BLL.Traductor TraduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                idiomaSeleccionado = obtenerIdiomaSeleccionado();
                traductor.Idioma = idiomaSeleccionado;
                lstTraductor.Clear();
                lstTraductor = TraduccionNeg.ListarTraducciones(traductor);
                divTraducciones.Visible = lstTraductor.Count > 0;
                if (lstTraductor.Count > 0) { 
                establecerTraductorCache(lstTraductor);
                foreach(BE.Traductor trad in lstTraductor)
                {
                    lstTraduccion.Add(trad.Traduccion);
                }
                
                    gvTraducciones.DataSource = null;
               
                gvTraducciones.DataSource = lstTraduccion;
                    if (gvTraducciones.PageIndex < 0) { 
                        gvTraducciones.PageIndex = 0;
          
                    }
                    gvTraducciones.DataBind();
                }
                else
                {
                    divTraducciones.Visible = true;
                    gvTraducciones.DataSource = null;
                    gvTraducciones.DataBind();
                    MostrarMensajeError(TraducirPalabra("No se han encontrado traducciones para este idioma", "No se han encontrado traducciones para este idioma"));
                }
                MostrarTraduccionEdicion(false);
                MostrarTraduccionNueva(false);

            }
            catch (Exception ex)
            {
                
                BE.Bitacora exception = new BE.Bitacora(ex);
               // exceptionLogger.Grabar(exception);
             //   MostrarMensajeError(TraducirPalabra("Ocurrió un error al bindear la grilla de idiomas: ", "Ocurrió un error al bindear la grilla de idiomas: ") + ex.Message);
            }
        }

        protected void gvTraducciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTraducciones.PageIndex = e.NewPageIndex;
                BindTraduccionesGridView();
                //BindIdiomaGridView();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cambiar de página: ", "Ocurrió un error al cambiar de página: ") + ex.Message);
            }
        }

        private void MostrarTraduccionEdicion(bool mostrar)
        {
            frmTraduccionEditar.Visible = mostrar;
            txtTraduccionEditar.Visible = mostrar;
            txtControlIDEditar.Visible = mostrar;
            txtPaginaEditar.Visible = mostrar;
            btnTraduccionGuardar.Visible = mostrar;
            if (!mostrar)
            {
                txtTraduccionEditar.Text = string.Empty;
                txtPaginaEditar.Text = string.Empty;
                txtControlIDEditar.Text = string.Empty;
            }
        }

        private void MostrarTraduccionNueva(bool mostrar)
        {
            frmTraduccionNueva.Visible = mostrar;
            txtTraduccionNueva.Visible = mostrar;
            litIdiomaSeleccionado.Visible = mostrar;
            txtControlIDNuevo.Visible = mostrar;
            txtPaginaNueva.Visible = mostrar;
            btnTraduccionCrear.Visible = mostrar;
            if (!mostrar)
            {
                txtTraduccionNueva.Text = string.Empty;
                txtControlIDNuevo.Text = string.Empty;
                txtPaginaNueva.Text = string.Empty;
            }
        }

        protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            BLL.Traductor traduccionNeg = null;
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {
                    BE.Traductor traductor = null;
                    switch (e.CommandName)
                    {
                        case "EditarTraduccion":

                            if (e.CommandArgument != null)
                            {
                                divTraducciones.Visible = true;
                                MostrarTraduccionEdicion(true);
                                MostrarTraduccionNueva(false);
                                BE.Traductor trad = new BE.Traductor();
                                traductor = obtenerTraductorCache().Where(x => x.Traduccion.CodTraduccion == Convert.ToInt32(e.CommandArgument)).FirstOrDefault<BE.Traductor>();
                                if (traductor != null && traductor.Traduccion != null)
                                {
                                    txtPaginaEditar.Text = traductor.Traduccion.Pagina;
                                    txtControlIDEditar.Text = traductor.Traduccion.ControlID;
                                    txtTraduccionEditar.Text = traductor.Traduccion.Texto;
                                    if (traductor.Traduccion != null && traductor.Traduccion.CodTraduccion > 0)
                                    {
                                        hdnCodTraduccionEditar.Value = traductor.Traduccion.CodTraduccion.ToString();
                                    }
                                }
                            
                    }

                            break;
                        case "BorrarTraduccion":
                            if (e.CommandArgument != null)
                            {
                                 divTraducciones.Visible = true;
                                MostrarTraduccionEdicion(true);
                                MostrarTraduccionNueva(false);

                                traduccionNeg = new BLL.Traductor();
                                BE.Traductor trad = new BE.Traductor();
                                traductor = obtenerTraductorCache().Where(x => x.Traduccion.CodTraduccion == Convert.ToInt32(e.CommandArgument)).FirstOrDefault<BE.Traductor>();
                                traduccionNeg.Baja(traductor);
                                BindTraduccionesGridView();
                                MostrarTraduccionEdicion(false);
                                MostrarTraduccionNueva(false);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los perfiles y permisos: ", "Ocurrió un error al mostrar los perfiles y permisos: ") + ex.Message);
            }
        }

        protected void gvTraducciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Traduccion columnas
            BE.Idioma idiomaUsuario = null;
            if ((BE.Idioma)Session["Idioma"] != null)
                idiomaUsuario = (BE.Idioma)Session["Idioma"];
            #endregion
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                 
                    if (idiomaUsuario != null)
                    {
                        foreach (System.Web.UI.WebControls.DataControlFieldCell row in e.Row.Cells)
                        {

                            object ctlColumna = (object)row;
                            if (ctlColumna != null)
                                Traducir(idiomaUsuario, ctlColumna);
                        }

                    }
                }
                else if(e.Row.RowType== DataControlRowType.DataRow)
                {
                    //Button btnEditarTraduccion = (Button)e.Row.FindControl("btnEditarTraduccion");
                    //if(btnEditarTraduccion!=null)
                    //{
                    //    btnEditarTraduccion.Text = TraducirPalabra(idiomaUsuario,btnEditarTraduccion.UniqueID, btnEditarTraduccion.Text); 
                    //}
                    //Button btnBorrarTraduccion = (Button)e.Row.FindControl("btnBorrarTraduccion");
                    //if(btnBorrarTraduccion!= null)
                    //{
                    //    btnBorrarTraduccion.Text = TraducirPalabra(idiomaUsuario, btnEditarTraduccion.UniqueID, btnEditarTraduccion.Text);
                    //}


                   Button btnEditarTraduccion = (Button)e.Row.FindControl("btnEditarTraduccion");
                   if (btnEditarTraduccion != null)
                   {
                       object ctlbtnEditarTraduccion = (object)btnEditarTraduccion;
                       if (ctlbtnEditarTraduccion != null)
                        Traducir(idiomaUsuario, ctlbtnEditarTraduccion);
                   }
                   Button btnBorrarTraduccion = (Button)e.Row.FindControl("btnBorrarTraduccion");
                   if (btnBorrarTraduccion != null)
                   {
                       object ctlbtnBorrarTraduccion = (object)btnBorrarTraduccion;
                       if (ctlbtnBorrarTraduccion != null)
                            Traducir(idiomaUsuario, ctlbtnBorrarTraduccion);


                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        private void Traducir(BE.Idioma idioma, object ControlATraducir)
        {
            try
            {
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                traductor.Idioma = idioma;
                //lstTraduccion.Clear();
                List<BE.Traduccion> lstTraducGrilla = new List<BE.Traduccion>();
                if (idioma.Descripcion == string.Empty)
                {
                    BLL.Idioma IdiomaNeg = new BLL.Idioma();
                    idioma = IdiomaNeg.Obtener(idioma);
                }

                foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                {
                    //lstTraduccion.Add(traduc.Traduccion);
                    lstTraducGrilla.Add(traduc.Traduccion);
                }
                //if (lstTraduccion.Count > 0)
                if (lstTraducGrilla.Count > 0)
                {
                    TraducirControles(lstTraducGrilla, ref ControlATraducir);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir: ", "Ocurrió un error al traducir: ") + ex.Message);
            }
        }
        private void Traducir(BE.Idioma idioma, ref object ControlATraducir)
        {
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
                    TraducirControles(lstTraduccion, ref ControlATraducir);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir: ", "Ocurrió un error al traducir: ") + ex.Message);
            }
        }

        private void TraducirControles(List<BE.Traduccion> traducciones, ref object ControlATraducir)
        {
            try
            {
                foreach (BE.Traduccion traduccion in traducciones)
                {
                    if (ControlATraducir != null)
                    {
                        if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                        {
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                            {
                                System.Web.UI.HtmlControls.HtmlAnchor ctrlPage = (System.Web.UI.HtmlControls.HtmlAnchor)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Literal))
                            {
                                System.Web.UI.WebControls.Literal ctrlPage = (System.Web.UI.WebControls.Literal)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                            {
                                System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Button))
                            {
                                System.Web.UI.WebControls.Button ctrlPage = (System.Web.UI.WebControls.Button)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.LinkButton))
                            {
                                System.Web.UI.WebControls.LinkButton ctrlPage = (System.Web.UI.WebControls.LinkButton)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                            {
                                System.Web.UI.WebControls.CheckBox ctrlPage = (System.Web.UI.WebControls.CheckBox)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.DataControlFieldHeaderCell))
                            {
                                System.Web.UI.WebControls.DataControlFieldHeaderCell ctrlPage = (System.Web.UI.WebControls.DataControlFieldHeaderCell)ControlATraducir;
                                if (ctrlPage.UniqueID != null)
                                    if (ctrlPage.UniqueID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.Text = traduccion.Texto;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
            }
        }



        protected void btnTraduccionEditar_Click(object sender, EventArgs e)
        {
            try
            {
                //BLL.Traduccion tradNeg = new BLL.Traduccion();
                //BE.Traductor trad = new BE.Traductor();
                //traductor = obtenerTraductorCache().Where(x => x.Traduccion.CodTraduccion == Convert.ToInt32(hdnCodTraduccionEditar.Value)).FirstOrDefault<BE.Traductor>();
                //traductor.Traduccion.ControlID = txtControlIDEditar.Text;
                //traductor.Traduccion.Pagina = txtPaginaEditar.Text;
                //traductor.Traduccion.Texto = txtTraduccionEditar.Text;
                //traduccionNeg.Modificacion(traductor);


                MostrarTraduccionNueva(false);
                MostrarTraduccionEdicion(true);
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                idiomaCached = obtenerIdiomaSeleccionado();
                if (idiomaCached == null || idiomaCached.CodIdioma == 0)
                    throw new ArgumentNullException(TraducirPalabra("Idioma no seleccionado", "Idioma no seleccionado"));
                traductor.Idioma = idiomaCached;
                traductor.Traduccion.Pagina = txtPaginaEditar.Text;
                traductor.Traduccion.ControlID = txtControlIDEditar.Text;
                traductor.Traduccion.Texto = txtTraduccionEditar.Text;
                if (hdnCodTraduccionEditar.Value != null)
                    traductor.Traduccion.CodTraduccion = Convert.ToInt32(hdnCodTraduccionEditar.Value);
                else
                {
                    throw new ArgumentNullException(TraducirPalabra("Ocurrió un error al guardar la traducción", "Ocurrió un error al guardar la traducción"));
                }
                traduccionNeg.Modificacion(traductor);
                BindTraduccionesGridView();
                MostrarTraduccionEdicion(false);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al crear una traducción: ", "Ocurrió un error al crear una traducción: " + ex.Message));
            }
        }

        protected void btnTraduccionCrear_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                idiomaCached = obtenerIdiomaSeleccionado();
                if (idiomaCached == null || idiomaCached.CodIdioma == 0)
                    throw new ArgumentNullException("Idioma no seleccionado");
                traductor.Idioma = idiomaCached;
                traductor.Traduccion.Pagina = txtPaginaNueva.Text;
                traductor.Traduccion.ControlID = txtControlIDNuevo.Text;
                traductor.Traduccion.Texto = txtTraduccionNueva.Text;
                traduccionNeg.Alta(traductor);
                BindTraduccionesGridView();
                MostrarTraduccionNueva(false);
                MostrarTraduccionEdicion(false);

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al crear una traducción: ", "Ocurrió un error al crear una traducción: ") + ex.Message);
            }
        }

        protected void btnCrearTraduccion_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarTraduccionNueva(true);
                MostrarTraduccionEdicion(false);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al crear una traducción: ", "Ocurrió un error al crear una traducción: ") + ex.Message);
            }
        }


        #region Traducir Grillas

        #endregion

        #region Exportar

 

        protected void imgBtnExportExcel_Idioma_Click(object sender, ImageClickEventArgs e)
        {
            
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<BE.Idioma> lstExport = (List<BE.Idioma>)Session["Idioma_Export"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Idiomas.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                #region traducir texto
                string resultado = string.Empty;
                BE.Idioma idiomaUsuario = null;

                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(BE.Idioma));
                foreach (PropertyDescriptor prop in props)
                {
                    foreach (Attribute atr in prop.Attributes)
                    {
                        if (atr.GetType() == typeof(BE.Exportable))
                        {
                            if (((ContraDescuento.BE.Exportable)atr).Exportar)
                            {
                                if (idiomaUsuario != null)
                                {
                                    resultado = TraducirPalabra(idiomaUsuario, "palabra_" + prop.DisplayName);
                                    if (resultado != string.Empty && resultado.Replace(" ", "").ToUpper() != "LOREMIPSUM")
                                        Response.Output.Write(resultado); // Nombre de la columna
                                    else
                                        Response.Output.Write(prop.DisplayName); // Nombre de la columna
                                }
                                Response.Output.Write("\t");
                            }
                        }
                    }
                }
                Response.Output.WriteLine();
                exportTool.Escribir<BE.Idioma>(lstExport, Response.Output);
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al exportar a excel: ", "Ocurrió un error al exportar a excel: ") + ex.Message);
            }
        }

        protected void imgBtnExportPDF_Idioma_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    // Le colocamos el título y el autor
                    // **Nota: Esto no será visible en el documento
                    doc.AddTitle("Idioma_PDF");
                    doc.AddCreator("Contra Descuento");
                    // Abrimos el archivo
                    doc.Open();
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Escribimos el encabezamiento en el documento
                    doc.Add(Chunk.NEWLINE);

                    byte[] img;
                    img = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/Recurso/Imagen/logo.png");
                    if (img != null)
                    {
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img);
                        //pic.SetAbsolutePosition(100, 300);
                        pic.ScaleToFit(500, 600);
                        doc.Add(pic);
                    }
                    doc.Add(Chunk.NEWLINE);

                    Font f = new Font(FontFamily.TIMES_ROMAN, 50.0f, Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph parag = new Paragraph("CONTRA DESCUENTO", f);
                    //parag.Alignment = 40;
                    parag.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag);

                    Font fSubtitulo = new Font(FontFamily.TIMES_ROMAN, 50.0f, Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph parag2 = new Paragraph("IDIOMAS", f);
                    //parag.Alignment = 40;
                    parag2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag2);

                    doc.Add(Chunk.NEXTPAGE);


                    #region traducir texto
                    string resultado = string.Empty;
                    BE.Idioma idiomaUsuario = null;

                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];
                    #endregion



                    List<BE.Idioma> lstExportPDF= (List<BE.Idioma>)Session["Idioma_Export"];
                    // lstBitacora=  lstBitacora.Take(3).ToList<BE.Bitacora>();
                    if (lstExportPDF.Count > 0)
                    {
                        int CantCols = 0;
                        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(BE.Idioma));
                        foreach (System.ComponentModel.PropertyDescriptor prop in props)
                        {
                            foreach (Attribute atr in prop.Attributes)
                            {
                                if (atr.GetType() == typeof(BE.Exportable))
                                {
                                    if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                    {
                                        CantCols++;

                                    }
                                }
                            }
                        }

                        PdfPTable tblPrueba = new PdfPTable(CantCols);
                        tblPrueba.WidthPercentage = 100;
                        foreach (System.ComponentModel.PropertyDescriptor prop in props)
                        {
                            foreach (Attribute atr in prop.Attributes)
                            {
                                if (atr.GetType() == typeof(BE.Exportable))
                                {
                                    if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                    {
                                        //PdfPCell clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        PdfPCell clNombre = null;

                                        if (idiomaUsuario != null)
                                        {
                                            resultado = TraducirPalabra(idiomaUsuario, "palabra_" + prop.DisplayName);
                                            if (resultado != string.Empty && resultado.Replace(" ", "").ToUpper() != "LOREMIPSUM")
                                            {
                                                clNombre = new PdfPCell(new Phrase(resultado));
                                            }
                                            else
                                                clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        }



                                        clNombre.BorderWidth = 1;
                                        clNombre.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                                        clNombre.BorderWidthBottom = 0.95f;
                                        clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tblPrueba.AddCell(clNombre);
                                    }
                                }
                            }
                        }

                        foreach (BE.Idioma exportItem in lstExportPDF)
                        {
                            foreach (System.ComponentModel.PropertyDescriptor prop in props)
                            {
                                foreach (Attribute atr in prop.Attributes)
                                {
                                    if (atr.GetType() == typeof(BE.Exportable))
                                    {
                                        if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                        {
                                            tblPrueba.AddCell(prop.Converter.ConvertToString(prop.GetValue(exportItem)));
                                        }
                                    }
                                }
                            }
                        }



                        doc.Add(tblPrueba);
                        doc.Close();
                        ms.Close();
                        Response.ContentType = "pdf/application";
                        Response.AddHeader("content-disposition",
                        "attachment;filename=ContraDescuento_Idiomas.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al exportar a PDF: ", "Ocurrió un error al exportar a PDF: ") + ex.Message);
            }
        }

        protected void imgBtnExportPDF_Traducciones_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    // Le colocamos el título y el autor
                    // **Nota: Esto no será visible en el documento
                    doc.AddTitle("Traducciones_PDF");
                    doc.AddCreator("Contra Descuento");
                    // Abrimos el archivo
                    doc.Open();
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Escribimos el encabezamiento en el documento
                    doc.Add(Chunk.NEWLINE);

                    byte[] img;
                    img = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/Recurso/Imagen/logo.png");
                    if (img != null)
                    {
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img);
                        //pic.SetAbsolutePosition(100, 300);
                        pic.ScaleToFit(500, 600);
                        doc.Add(pic);
                    }
                    doc.Add(Chunk.NEWLINE);

                    Font f = new Font(FontFamily.TIMES_ROMAN, 50.0f, Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph parag = new Paragraph("CONTRA DESCUENTO", f);
                    parag.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag);

                    Font fSubtitulo = new Font(FontFamily.TIMES_ROMAN, 50.0f, Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph parag2 = new Paragraph("TRADUCCIONES", f);
                    parag2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag2);

                    doc.Add(Chunk.NEXTPAGE);

                    List<BE.Traductor> lstExportPDF = (List<BE.Traductor>)Session["lstTraductorBinded"];
                    List<BE.Traduccion> lstTraduccionExportPDF = new List<BE.Traduccion>();
                    foreach (BE.Traductor traductor in lstExportPDF)
                    {
                        lstTraduccionExportPDF.Add(traductor.Traduccion);
                    }
                    
                    if (lstTraduccionExportPDF.Count > 0)
                    {
                        int CantCols = 0;
                        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(BE.Traduccion));
                        foreach (System.ComponentModel.PropertyDescriptor prop in props)
                        {
                            foreach (Attribute atr in prop.Attributes)
                            {
                                if (atr.GetType() == typeof(BE.Exportable))
                                {
                                    if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                    {
                                        CantCols++;
                                    }
                                }
                            }
                        }


                        #region traducir texto
                        string resultado = string.Empty;
                        BE.Idioma idiomaUsuario = null;

                        if ((BE.Idioma)Session["Idioma"] != null)
                            idiomaUsuario = (BE.Idioma)Session["Idioma"];
                        #endregion

                        PdfPTable tblPrueba = new PdfPTable(CantCols);
                        tblPrueba.WidthPercentage = 100;
                        foreach (System.ComponentModel.PropertyDescriptor prop in props)
                        {
                            foreach (Attribute atr in prop.Attributes)
                            {
                                if (atr.GetType() == typeof(BE.Exportable))
                                {
                                    if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                    {
                                        //PdfPCell clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        PdfPCell clNombre = null;

                                        if (idiomaUsuario != null)
                                        {
                                            resultado = TraducirPalabra(idiomaUsuario, "palabra_" + prop.DisplayName);
                                            if (resultado != string.Empty && resultado.Replace(" ", "").ToUpper() != "LOREMIPSUM")
                                            {
                                                clNombre = new PdfPCell(new Phrase(resultado));
                                            }
                                            else
                                                clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        }

                                        clNombre.BorderWidth = 1;
                                        clNombre.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                                        clNombre.BorderWidthBottom = 0.95f;
                                        clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tblPrueba.AddCell(clNombre);
                                    }
                                }
                            }
                        }

                        foreach (BE.Traduccion exportItem in lstTraduccionExportPDF)
                        {
                            foreach (System.ComponentModel.PropertyDescriptor prop in props)
                            {
                                foreach (Attribute atr in prop.Attributes)
                                {
                                    if (atr.GetType() == typeof(BE.Exportable))
                                    {
                                        if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                        {
                                            tblPrueba.AddCell(prop.Converter.ConvertToString(prop.GetValue(exportItem)));
                                        }
                                    }
                                }
                            }
                        }
                        doc.Add(tblPrueba);
                        doc.Close();
                        ms.Close();
                        Response.ContentType = "pdf/application";
                        Response.AddHeader("content-disposition",
                        "attachment;filename=ContraDescuento_Traducciones.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al exportar a PDF: ", "Ocurrió un error al exportar a PDF: ") + ex.Message);
            }
        }

        protected void imgBtnExportExcel_Traducciones_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<BE.Traductor> lstExport = (List<BE.Traductor>)Session["lstTraductorBinded"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Traducciones.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                #region traducir texto
                string resultado = string.Empty;
                BE.Idioma idiomaUsuario = null;

                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(BE.Traduccion));
                foreach (PropertyDescriptor prop in props)
                {
                    foreach (Attribute atr in prop.Attributes)
                    {
                        if (atr.GetType() == typeof(BE.Exportable))
                        {
                            if (((ContraDescuento.BE.Exportable)atr).Exportar)
                            {
                                if (idiomaUsuario != null)
                                {
                                    resultado = TraducirPalabra(idiomaUsuario, "palabra_" + prop.DisplayName);
                                    if (resultado != string.Empty && resultado.Replace(" ", "").ToUpper() != "LOREMIPSUM")
                                        Response.Output.Write(resultado); // Nombre de la columna
                                    else
                                        Response.Output.Write(prop.DisplayName); // Nombre de la columna
                                }
                                Response.Output.Write("\t");
                            }
                        }
                    }
                }
                Response.Output.WriteLine();
                List<BE.Traduccion> lstTraduccionesExport = new List<BE.Traduccion>();
                foreach(BE.Traductor trad in lstExport)
                {
                    lstTraduccionesExport.Add(trad.Traduccion);
                }
                exportTool.Escribir<BE.Traduccion>(lstTraduccionesExport, Response.Output);
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al exportar a excel: ", "Ocurrió un error al exportar a excel: ") + ex.Message);
            }
           
        }

        #endregion

        private string TraducirPalabra(BE.Idioma idioma, string palabra)
        {
            string traduccionResultado = string.Empty;
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
                        string traduccionTextResultado = palabra.Replace(" ", "").ToUpper();
                        if (traduccionTexto == traduccionTextResultado)
                        {
                            return traduccion.Texto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Ocurrió un error al traducir la página: " + ex.Message);
            }
            return traduccionResultado;
        }

    }
}