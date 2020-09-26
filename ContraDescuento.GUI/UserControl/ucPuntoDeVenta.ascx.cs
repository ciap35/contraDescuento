using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI.UserControl
{
    public partial class PuntoDeVenta : System.Web.UI.UserControl
    {
        #region Propiedad Privada
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Domicilio> PuntosDeVenta = new List<BE.Domicilio>();
        BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                divMensajeOK.Visible = false;
                divMensajeError.Visible = false;
                ValidarPermisos();
                if (!IsPostBack)
                {
                 
                    ProvinciaCargar();
                    CargarPuntosDeVenta(((BE.Usuario)Session["Usuario"]).Comercio);
                }   Traducir();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener la provincia seleccionada: ", "Ocurrió un error al obtener la provincia seleccionada: ") + ex.Message);
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
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Ocurrió un error al validar  los permisos: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            return valido;
        }
        private void MostrarMensajeError(string error)
        {
            divMensajeOK.Visible = false;
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = error.ToUpper();
        }

        private void OcultarMensajeError()
        {
            divMensajeOK.Visible = false;
            divMensajeError.Visible = false;
            litMensajeError.Visible = false;
            litMensajeError.Text = string.Empty;
            divErrorAltaPuntoDeVenta.Visible = false;

        }

        private void MostrarMensajeOk(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
        }

        private BE.Provincia ProvinciaObtenerSeleccionada(int codProvincia)
        {
            BE.Provincia provinciaSelected = null;
            try
            {
                List<BE.Provincia> lstProvincia = (List<BE.Provincia>)Session["Provincia"];
                provinciaSelected = (from p in lstProvincia where p.CodProvincia == codProvincia select p).FirstOrDefault<BE.Provincia>();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener la provincia seleccionada: ", "Ocurrió un error al obtener la provincia seleccionada: ") + ex.Message);
            }
            return provinciaSelected;


        }
        private BE.Localidad LocalidadOBtenerSeleccionada(int codLocalidad)
        {
            BE.Localidad localidadSelected = null;
            try
            {
                List<BE.Localidad> lstLocalidad = ProvinciaObtenerSeleccionada(Convert.ToInt32(ddlProvincia.SelectedValue)).Local;
                localidadSelected = (from l in lstLocalidad where l.CodLocalidad == codLocalidad select l).FirstOrDefault<BE.Localidad>();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener la provincia seleccionada: ", "Ocurrió un error al obtener la provincia seleccionada: ") + ex.Message);

            }
            return localidadSelected;
        }
        private List<BE.Provincia> ProvinciaListar()
        {
            try
            {
                BLL.Provincia provinciaNeg = new BLL.Provincia();
                Session["Provincia"] = provinciaNeg.Listar();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar las provincias: ", "Ocurrió un error al cargar las provincias: ") + ex.Message);

            }
            return (List<BE.Provincia>)Session["Provincia"];
        }
        private void ProvinciaCargar()
        {
            try
            {
                ddlProvincia.DataSource = ProvinciaListar();
                ddlProvincia.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar las provincias: ", "Ocurrió un error al cargar las provincias: ") + ex.Message);
            }
        }
        private void LocalidadCargar(BE.Provincia provincia, ref DropDownList ddlLocalidad)
        {
            try
            {
                BLL.Localidad localidadNeg = new BLL.Localidad();
                ddlLocalidad.DataSource = localidadNeg.ListarPorProvincia(ref provincia);
                ddlLocalidad.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar las localidades: ", "Ocurrió un error al cargar las localidades: ") + ex.Message);
            }
        }
        /// <summary>
        /// False = Datos correctos
        /// True = Datos incorrectos
        /// </summary>
        /// <returns>[False | True]</returns>
        private bool ValidarIngresoError()
        {
            bool Error = false;
            try
            {
                RfvTxtCalle.Validate();
                RfvTxtNumero.Validate();

                if (!Error)
                    Error = ddlProvincia.SelectedValue == null;
                if (!Error)
                    Error = ddlLocalidad.SelectedValue == null;
                if (!Error)
                    Error = string.IsNullOrEmpty(txtCalle.Text);
                if (!Error)
                    Error = string.IsNullOrEmpty(txtNumero.Text);
                litAltaPuntoDeVentaError.Text = string.Empty;
                litAltaPuntoDeVentaError.Text += TraducirPalabra("Complete los siguientes datos: ", "Complete los siguientes datos: ");

                if (string.IsNullOrEmpty(txtCalle.Text))
                {
                    litAltaPuntoDeVentaError.Text += TraducirPalabra("Calle", "Calle");
                }
                if (string.IsNullOrEmpty(txtNumero.Text))
                {
                    litAltaPuntoDeVentaError.Text += TraducirPalabra(" Número", " Número");
                }
                litAltaPuntoDeVentaError.Visible = Error;
                divErrorAltaPuntoDeVenta.Visible = Error;

                divMensajeError.Visible = false;
                litMensajeError.Visible = false;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al validar el ingreso: ", "Ocurrió un error al validar el ingreso: ") + ex.Message);
            }
            return Error;
        }
        private void CargarPuntosDeVenta(BE.Comercio comercio)
        {
            try
            {
                BLL.PuntoDeVenta PuntoDeVentaNeg = new BLL.PuntoDeVenta();
                List<BE.Domicilio> lstPuntosVenta = PuntoDeVentaNeg.Listar(comercio);
                rptPuntoDeVenta.DataSource = lstPuntosVenta;
                rptPuntoDeVenta.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar los puntos de venta: ", "Ocurrió un error al cargar los puntos de venta: ") + ex.Message);
            }
        }

        private void LimpiarFormularioPuntoVenta()
        {
            ProvinciaCargar();
            LocalidadCargar(ProvinciaObtenerSeleccionada(Convert.ToInt32(ddlProvincia.SelectedItem.Value)), ref ddlLocalidad);
            //ddlProvincia.ClearSelection();
            //ddlLocalidad.DataSource = null;
            //ddlLocalidad.DataBind();
            //ddlLocalidad.ClearSelection();
            txtCalle.Text = string.Empty;
            txtPiso.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtDpto.Text = string.Empty;
        }
        protected void btnCrearPuntoDeVenta_Click(object sender, EventArgs e)
        {
            OcultarMensajeError();
            try
            {
                if (!ValidarIngresoError())
                {
                    BE.Domicilio domicilio = new BE.Domicilio();
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    domicilio.Calle = txtCalle.Text;
                    domicilio.Numero = txtNumero.Text;
                    domicilio.Piso = txtPiso.Text;
                    domicilio.Departamento = txtDpto.Text;
                    domicilio.Provincia = ProvinciaObtenerSeleccionada(Convert.ToInt32(ddlProvincia.SelectedItem.Value));
                    domicilio.Local = LocalidadOBtenerSeleccionada(Convert.ToInt32(ddlLocalidad.SelectedItem.Value));
                    Session["Usuario"] = usuario;
                    BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
                    puntoDeVentaNeg.Alta(domicilio, usuario.Comercio, usuario);
                    CargarPuntosDeVenta(usuario.Comercio);

                    LimpiarFormularioPuntoVenta();
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al crear un punto de venta: ", "Ocurrió un error al crear un punto de venta: ") + ex.Message);
            }
        }

        protected void rptPuntoDeVenta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            BE.Domicilio domicilio = null;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item != null)
                    {
                        if (e.Item.DataItem != null)
                        {
                            domicilio = (BE.Domicilio)e.Item.DataItem;
                        }
                        DropDownList ddlProvincia = (DropDownList)e.Item.FindControl("ddlProvinciaItem");
                        if (ddlProvincia != null)
                        {
                            ddlProvincia.DataSource = ProvinciaListar();
                            ddlProvincia.DataBind();
                            ddlProvincia.SelectedValue = Convert.ToString((domicilio.Provincia.CodProvincia));
                        }
                        DropDownList ddlLocalidad = (DropDownList)e.Item.FindControl("ddlLocalidadItem");
                        LocalidadCargar(domicilio.Provincia, ref ddlLocalidad);
                        ddlLocalidad.SelectedValue = Convert.ToString(domicilio.Local.CodLocalidad);
                        TextBox txtCalleItem = (TextBox)e.Item.FindControl("txtCalleItem");
                        txtCalleItem.Text = domicilio.Calle;
                        TextBox txtPisoItem = (TextBox)e.Item.FindControl("txtPisoItem");
                        txtPisoItem.Text = domicilio.Piso;
                        TextBox txtNumeroItem = (TextBox)e.Item.FindControl("txtNumeroItem");
                        txtNumeroItem.Text = domicilio.Numero;
                        TextBox txtDptoItem = (TextBox)e.Item.FindControl("txtDptoItem");
                        txtDptoItem.Text = domicilio.Departamento;


                        txtCalleItem.Attributes.Add("placeholder", "");
                        txtNumeroItem.Attributes.Add("placeholder", "");
                        txtPisoItem.Attributes.Add("placeholder", "");
                        txtDptoItem.Attributes.Add("placeholder", "");


                        BE.Idioma idiomaUsuario = null;
                            if ((BE.Idioma)Session["Idioma"] != null)
                                idiomaUsuario = (BE.Idioma)Session["Idioma"];

                        if (idiomaUsuario != null)
                        {
                            LinkButton lnkGuardarPuntoDeVenta = (LinkButton)e.Item.FindControl("lnkGuardarPuntoDeVenta");
                            if (lnkGuardarPuntoDeVenta != null)
                            {
                                object ctllnkCancelarEdicion = (object)lnkGuardarPuntoDeVenta;
                                Traducir(idiomaUsuario, ref ctllnkCancelarEdicion);
                            }
                            LinkButton lnkCancelarEdicion = (LinkButton)e.Item.FindControl("lnkCancelarEdicion");
                            if (lnkCancelarEdicion != null)
                            {
                                object ctllnkCancelarEdicion = (object)lnkCancelarEdicion;
                                Traducir(idiomaUsuario, ref ctllnkCancelarEdicion);
                            }
                        }

                    }
                }
                else if (e.Item != null && e.Item.ItemType == ListItemType.Header)
                {
                 
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar los puntos de venta: ", "Ocurrió un error al cargar los puntos de venta: ") + ex.Message);
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
                            if (traduccionTexto == clave)
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



        private void Traducir()
        {
            BE.Idioma idiomaUsuario = null;
            try
            {
                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];

                if (idiomaUsuario != null)
                {
                    object ctllitProvincia = (object)litProvincia;
                    Traducir(idiomaUsuario, ref ctllitProvincia);

                    object ctllitLocalidad = (object)litLocalidad;
                    Traducir(idiomaUsuario, ref ctllitLocalidad);

                    object ctllitCalle = (object)litCalle;
                    Traducir(idiomaUsuario, ref ctllitCalle);

                    object ctllitNumero = (object)litNumero;
                    Traducir(idiomaUsuario, ref ctllitNumero);

                    object ctllitPiso = (object)litPiso;
                    Traducir(idiomaUsuario, ref ctllitPiso);

                    object ctllitDpto = (object)litDpto;
                    Traducir(idiomaUsuario, ref ctllitDpto);

                    object ctlbtnCrearPuntoDeVenta = (object)btnCrearPuntoDeVenta;
                    Traducir(idiomaUsuario, ref ctlbtnCrearPuntoDeVenta);

                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
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
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
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
        protected void rptPuntoDeVenta_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {

                    switch (e.CommandName)
                    {
                        case "BorrarDomicilio":
                            if (e.CommandArgument != null)
                            {
                                BE.Domicilio domicilio = (
                                    from dom
                                    in puntoDeVentaNeg.Listar(getUsuarioCached().Comercio)
                                    where dom.CodDomicilio == Convert.ToInt32(e.CommandArgument)
                                    select dom).FirstOrDefault<BE.Domicilio>();
                                puntoDeVentaNeg.Baja(domicilio, getUsuarioCached().Comercio, getUsuarioCached());
                                CargarPuntosDeVenta(getUsuarioCached().Comercio);
                            }
                            else
                                MostrarMensajeError(TraducirPalabra("No se ha indicado ningún código para borrar.", "No se ha indicado ningún código para borrar."));
                            break;
                        case "EditarDomicilio":
                            if (e.CommandArgument != null)
                            {
                                OcultarMensajeError();
                                BE.Domicilio domicilio = (
                                    from dom
                                    in puntoDeVentaNeg.Listar(getUsuarioCached().Comercio)
                                    where dom.CodDomicilio == Convert.ToInt32(e.CommandArgument)
                                    select dom).FirstOrDefault<BE.Domicilio>();
                                Session["PuntoDeVentaSelected"] = domicilio;
                                DropDownList ddlProvinciaItem = (DropDownList)e.Item.FindControl("ddlProvinciaItem");
                                DropDownList ddlLocalidadItem = (DropDownList)e.Item.FindControl("ddlLocalidadItem");
                                TextBox txtCalleItem = (TextBox)e.Item.FindControl("txtCalleItem");
                                TextBox txtNumeroItem = (TextBox)e.Item.FindControl("txtNumeroItem");
                                TextBox txtPisoItem = (TextBox)e.Item.FindControl("txtPisoItem");
                                TextBox txtDptoItem = (TextBox)e.Item.FindControl("txtDptoItem");
                                ddlProvinciaItem.Enabled = true;
                                ddlLocalidadItem.Enabled = true;
                                txtCalleItem.Enabled = true;
                                txtNumeroItem.Enabled = true;
                                txtPisoItem.Enabled = true;
                                txtDptoItem.Enabled = true;
                                LinkButton lnkGuardarPuntoDeVenta = (LinkButton)e.Item.FindControl("lnkGuardarPuntoDeVenta");
                                lnkGuardarPuntoDeVenta.Visible = true;
                                lnkGuardarPuntoDeVenta.Enabled = true;
                                lnkGuardarPuntoDeVenta.CommandName = "Guardar";
                                lnkGuardarPuntoDeVenta.CommandArgument = domicilio.CodDomicilio.ToString();
                                LinkButton lnkCancelarEdicion = (LinkButton)e.Item.FindControl("lnkCancelarEdicion");
                                lnkCancelarEdicion.Enabled = true;
                                lnkCancelarEdicion.Visible = true;

                                txtCalleItem.Attributes.Add("placeholder", "...");
                                txtNumeroItem.Attributes.Add("placeholder", "...");
                                txtPisoItem.Attributes.Add("placeholder", "...");
                                txtDptoItem.Attributes.Add("placeholder", "...");
                            }
                            else
                                MostrarMensajeError(TraducirPalabra("No se ha indicado ningún Punto de Venta para borrar.", "No se ha indicado ningún Punto de Venta para borrar."));
                            break;

                        case "CancelarEdicion":
                            if (e.CommandArgument != null)
                            {
                                DropDownList ddlProvinciaItem = (DropDownList)e.Item.FindControl("ddlProvinciaItem");
                                DropDownList ddlLocalidadItem = (DropDownList)e.Item.FindControl("ddlLocalidadItem");
                                TextBox txtCalleItem = (TextBox)e.Item.FindControl("txtCalleItem");
                                TextBox txtNumeroItem = (TextBox)e.Item.FindControl("txtNumeroItem");
                                TextBox txtPisoItem = (TextBox)e.Item.FindControl("txtPisoItem");
                                TextBox txtDptoItem = (TextBox)e.Item.FindControl("txtDptoItem");
                                ddlProvinciaItem.Enabled = false;
                                ddlLocalidadItem.Enabled = false;
                                txtCalleItem.Enabled = false;
                                txtNumeroItem.Enabled = false;
                                txtPisoItem.Enabled = false;
                                txtDptoItem.Enabled = false;
                                LinkButton lnkGuardarPuntoDeVenta = (LinkButton)e.Item.FindControl("lnkGuardarPuntoDeVenta");
                                lnkGuardarPuntoDeVenta.Visible = false;
                                lnkGuardarPuntoDeVenta.Enabled = false;
                                LinkButton lnkCancelarEdicion = (LinkButton)e.Item.FindControl("lnkCancelarEdicion");
                                lnkCancelarEdicion.Enabled = false;
                                lnkCancelarEdicion.Visible = false;
                                Panel panelValidarDatos = (Panel)e.Item.FindControl("panelValidarDatos");
                                panelValidarDatos.Visible = false;
                                txtCalleItem.Attributes.Add("placeholder", "");
                                txtNumeroItem.Attributes.Add("placeholder", "");
                                txtPisoItem.Attributes.Add("placeholder", "");
                                txtDptoItem.Attributes.Add("placeholder", "");
                            }
                            else
                                MostrarMensajeError(TraducirPalabra("No se ha indicado ningún Punto de Venta.", "No se ha indicado ningún Punto de Venta."));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al borrar un punto de venta: ", "Ocurrió un error al borrar un punto de venta: ") + ex.Message);
            }
        }

        private BE.Usuario getUsuarioCached()
        {
            return ((BE.Usuario)Session["Usuario"]);
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlProvincia.SelectedItem != null)
                {
                    LocalidadCargar(ProvinciaObtenerSeleccionada(Convert.ToInt32(ddlProvincia.SelectedItem.Value)), ref ddlLocalidad);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Ocurrió un error al cargar las provincias: " + ex.Message);
            }
        }

        protected void ddlLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlProvinciaItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.Domicilio puntoDeVenta = (BE.Domicilio)Session["PuntoDeVentaSelected"];
            try
            {
                foreach (RepeaterItem item in rptPuntoDeVenta.Items)
                {
                    var hdnPuntoDeVenta = (HiddenField)item.FindControl("hdnPuntoDeVenta");
                    if (hdnPuntoDeVenta.Value == puntoDeVenta.CodDomicilio.ToString())
                    {
                        DropDownList ddlProvinciaItem = (DropDownList)item.FindControl("ddlProvinciaItem");
                        DropDownList ddlLocalidadItem = (DropDownList)item.FindControl("ddlLocalidadItem");
                        BE.Provincia provincia = new BE.Provincia() { CodProvincia = Convert.ToInt32(ddlProvinciaItem.SelectedItem.Value) };
                        LocalidadCargar(provincia, ref ddlLocalidadItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al cargar las localidades: " + ex.Message);
            }
        }

        protected void lnkGuardarPuntoDeVenta_Command(object sender, CommandEventArgs e)
        {
            try
            {
                OcultarMensajeError();
                divErrorAltaPuntoDeVenta.Visible = false;
                BE.Domicilio domicilio = obtenerPuntoDeVentaEditar();
                if (domicilio != null)
                {
                    puntoDeVentaNeg.Modificar(domicilio);

                    CargarPuntosDeVenta(((BE.Usuario)Session["Usuario"]).Comercio);
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al guardar el punto de venta: " + ex.Message);
            }

        }

        private BE.Domicilio obtenerPuntoDeVentaEditar()
        {
            BE.Domicilio puntoDeVentaEditar = null;
            BE.Domicilio puntoDeVenta = (BE.Domicilio)Session["PuntoDeVentaSelected"];
            bool valido = true;
            try
            {
                foreach (RepeaterItem item in rptPuntoDeVenta.Items)
                {
                    var hdnPuntoDeVenta = (HiddenField)item.FindControl("hdnPuntoDeVenta");
                    if (hdnPuntoDeVenta.Value == puntoDeVenta.CodDomicilio.ToString())
                    {


                        DropDownList ddlProvinciaItem = (DropDownList)item.FindControl("ddlProvinciaItem");
                        DropDownList ddlLocalidadItem = (DropDownList)item.FindControl("ddlLocalidadItem");
                        TextBox txtCalleItem = (TextBox)item.FindControl("txtCalleItem");
                        TextBox txtNumeroItem = (TextBox)item.FindControl("txtNumeroItem");
                        TextBox txtPisoItem = (TextBox)item.FindControl("txtPisoItem");
                        TextBox txtDptoItem = (TextBox)item.FindControl("txtDptoItem");

                        valido = !string.IsNullOrEmpty(txtCalleItem.Text) && !string.IsNullOrEmpty(txtNumeroItem.Text);


                        puntoDeVentaEditar = new BE.Domicilio();
                        puntoDeVentaEditar = puntoDeVenta;


                        puntoDeVentaEditar.Piso = txtPisoItem.Text;
                        puntoDeVentaEditar.Departamento = txtDptoItem.Text;
                        puntoDeVentaEditar.Provincia.CodProvincia = Convert.ToInt32(ddlProvinciaItem.SelectedItem.Value);
                        puntoDeVentaEditar.Local.CodLocalidad = Convert.ToInt32(ddlLocalidadItem.SelectedItem.Value);



                        Panel panelValidarDatos = (Panel)item.FindControl("panelValidarDatos");
                        Literal litErrorDatos = (Literal)item.FindControl("litErrorDatos");
                        if (panelValidarDatos != null)
                        {
                            litErrorDatos.Text = string.Empty;
                            litErrorDatos.Text = "Los siguientes campos no pueden ser vacíos: ";
                            if (string.IsNullOrEmpty(txtCalleItem.Text))
                            {
                                litErrorDatos.Text += " Calle ";
                            }
                            if (string.IsNullOrEmpty(txtNumeroItem.Text))
                            {
                                litErrorDatos.Text += " Número  ";
                            }
                        }
                        if (!valido)
                            puntoDeVentaEditar = null;
                        panelValidarDatos.Visible = !valido;
                        litErrorDatos.Visible = !valido;
                        HtmlGenericControl divValidarDatos = (HtmlGenericControl)item.FindControl("divValidarDatos");
                        if (divValidarDatos != null)
                            divValidarDatos.Visible = true;

                        if (txtNumeroItem.Text != string.Empty && txtCalleItem.Text != string.Empty)
                        {
                            puntoDeVentaEditar.Calle = txtCalleItem.Text;
                            puntoDeVentaEditar.Numero = txtNumeroItem.Text;
                        }
                        else
                        {
                            txtCalleItem.Text = puntoDeVenta.Calle;
                            txtNumeroItem.Text = puntoDeVenta.Numero;
                        }

                        LinkButton lnkGuardarPuntoDeVenta = (LinkButton)item.FindControl("lnkGuardarPuntoDeVenta");
                        LinkButton lnkCancelarEdicion = (LinkButton)item.FindControl("lnkCancelarEdicion");


                        ddlProvinciaItem.Enabled = puntoDeVentaEditar == null;
                        ddlLocalidadItem.Enabled = puntoDeVentaEditar == null;
                        txtCalleItem.Enabled = puntoDeVentaEditar == null;
                        txtNumeroItem.Enabled = puntoDeVentaEditar == null;
                        txtPisoItem.Enabled = puntoDeVentaEditar == null;
                        txtDptoItem.Enabled = puntoDeVentaEditar == null;

                        lnkGuardarPuntoDeVenta.Enabled = puntoDeVentaEditar == null;
                        lnkGuardarPuntoDeVenta.Visible = puntoDeVentaEditar == null;

                        lnkCancelarEdicion.Enabled = puntoDeVentaEditar == null;
                        lnkCancelarEdicion.Visible = puntoDeVentaEditar == null;


                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar el punto de venta a editar: ", "Ocurrió un error al mostrar el punto de venta a editar: ") + ex.Message);
            }
            return puntoDeVentaEditar;
        }



    }
}