using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class ComercioAdministracion : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BE.Comercio comercio = null;
        BLL.Comercio comercioNeg = null;
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        string fileName;
        string logo;
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
                    //divMensajeError.Visible = false;
                    //divMensajeOK.Visible = false;
                    if (Session["Usuario"] != null)
                    {
                        CargarComercio();
                    }

                    Session["MiProducto.IsPostBack"] = false;
                    
                }

                
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
                    else if (usuario.TipoUsuario == null)//Es administrador
                    {
                        valido = true;
                    }
                    else
                        Response.Redirect("Logueese.aspx");
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
                }
            }
            return traduccionResultado;
        }

        private void CargarComercio(BE.Usuario usuario)
        {
            try
            {
                BE.Comercio comercio = usuario.Comercio;
                if (comercio != null)
                {
                    if (comercio.Logo != null)
                        ComercioLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(comercio.Logo);
                    else
                        ComercioLogo.ImageUrl = @"~/Recurso/Imagen/foto.png";
                    litComercioSideBarNombreComercio.Text = usuario.Comercio.NombreComercio.ToUpper();
                    CargarComercio();
                }
                else
                    throw new Exception(TraducirPalabra("El usuario no posee comercio asignado", "El usuario no posee comercio asignado"));
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Ocurrió un error al mostrar el comercio: " + ex.Message);
            }
        }

        private void CargarComercio()
        {
            MisDatos.Visible = true;
            try
            {
                comercio = ((BE.Usuario)Session["Usuario"]).Comercio;
                if (comercio != null)
                {
                    if (comercio.NombreComercio == string.Empty)
                    {
                        comercioNeg = new BLL.Comercio();
                        comercio = comercioNeg.Obtener(comercio);
                        BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                        usuario.Comercio = comercio;
                    }
                    litComercioSideBarNombreComercio.Text = comercio.NombreComercio;
                    txtComercio.Text = comercio.NombreComercio;
                    txtDescripcion.Text = comercio.Descripcion;
                    /*Teléfono*/
                    txtCaracteristica.Text = comercio.Telefono.Caracteristica;
                    txtNumero.Text = comercio.Telefono.NroTelefono;
                    txtObservacion.Text = comercio.Telefono.Observacion;
                    chkCelular.Checked = comercio.Telefono.Celular;
                    if (comercio.Logo != null)
                        ComercioLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(comercio.Logo);
                }
                else
                {
                    MostrarMensajeError(TraducirPalabra("El usuario no posee comercio", "El usuario no posee comercio"));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar el comercio: ", "Ocurrió un error al mostrar el comercio: ") + ex.Message);
            }
        }

        protected void navMisDatos_Click(object sender, EventArgs e)
        {
            try
            {
                Session["menuMisProductos"] = false;
                Session["MiProducto.IsPostBack"] = false;
                pnMisDatos.Visible = true;
                pnMisDescuentos.Visible = false;
                pnMisProductos.Visible = false;
                pnEstadisticasMiComercio.Visible = false;
                
                CargarComercio();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar datos del comercio: ", "Ocurrió un error al cargar datos del comercio: ") + ex.Message);
            }
        }

        protected void navMisProductos_Click(object sender, EventArgs e)
        {
            try
            {
                Session["menuMisProductos"] = true;
                Session["MiProducto.IsPostBack"] = true;
                pnMisDatos.Visible = false;
                pnMisProductos.Visible = true;
                pnMisDescuentos.Visible = false;
                ucProducto.Visible = true;
                pnEstadisticasMiComercio.Visible = false;
                Session["Producto.Selected"] = 0;
                //pnMisDescuentos.Visible = false;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar los productos: ", "Ocurrió un error al cargar los productos: " )+ ex.Message);
            }
        }

        protected void navMisDescuentos_Click(object sender, EventArgs e)
        {
            try
            {
                Session["menuMisProductos"] = false;
                Session["MiProducto.IsPostBack"] = false;
                pnMisDatos.Visible = false;
                pnMisProductos.Visible = false; ucProducto.Visible = false;
                pnMisDescuentos.Visible = true;
                pnEstadisticasMiComercio.Visible = false;
                VerMisDescuentos();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar los descuentos: ", "Ocurrió un error al cargar los descuentos: ") + ex.Message);
            }
        }


        protected void btnModificarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Comercio comercioNeg = new BLL.Comercio();
                BE.Usuario usuario = (BE.Usuario)Session["Usuario"];

                BE.Idioma idioma = (BE.Idioma)Session["Idioma"];
                if (idioma != null)
                {
                    if (usuario.Idioma.CodIdioma != idioma.CodIdioma)
                    {
                        usuario.Idioma = idioma;
                        BLL.Usuario usuarioNeg = new BLL.Usuario();
                        usuarioNeg.Modificar(usuario, (BE.Usuario)Session["Usuario"]);
                    }
                }
                else
                {
                    usuario.Idioma = new BE.Idioma() { CodIdioma = 1 };
                }

                /*Telefono*/
                usuario.Telefono.Celular = chkCelular.Checked ? true : false;
                usuario.Telefono.NroTelefono = txtNumero.Text;
                usuario.Telefono.Observacion = txtObservacion.Text;
                usuario.Telefono.Caracteristica = txtCaracteristica.Text;
                /*Fin Telefono*/

                /*Datos básicos del comercio*/
                if (usuario.Comercio != null)
                {
                    usuario.Comercio.NombreComercio = txtComercio.Text;
                    txtComercio.Text = usuario.Comercio.NombreComercio;
                    litComercioSideBarNombreComercio.Text = txtComercio.Text;
                    usuario.Comercio.Descripcion = txtDescripcion.Text;
                    usuario.Comercio.Telefono = usuario.Telefono;
                    usuario.Comercio.Logo = obtenerArchivoCargado();
                    usuario.Comercio.MantenerLogo = ChkMantenerLogo.Checked;
                }
                else
                {
                    throw new BE.ExcepcionPersonalizada(false, new Exception(TraducirPalabra("El usuario no posee comercio asignado", "El usuario no posee comercio asignado")));
                }
                /*Fin - Datos básicos del comercio */
                comercioNeg.Modificar(usuario.Comercio);
                Session["usuario"] = usuario;
                MostrarMensajeOk(TraducirPalabra("Los datos se han actualizado éxitosamente", "Los datos se han actualizado éxitosamente"));
                Response.Redirect("ComercioAdministracion.aspx");
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            catch (FormatException ex)
            {
                MostrarMensajeError(ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }

        }

        private byte[] obtenerArchivoCargado()
        {
            byte[] logoFile = null;
            try
            {
                if (!ChkMantenerLogo.Checked && fLogoUpload.HasFile)
                {


                    fileName = System.IO.Path.GetFileName(fLogoUpload.FileName);
                    logo = System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"] + fileName;
                    if(!System.IO.Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"]))
                    {
                        System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"]);
                    }
                    fLogoUpload.SaveAs(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Logo"] + fileName); logoFile = System.IO.File.ReadAllBytes(logo);
                    validarArchivoCargado(logoFile);
                    Session["File.Comercio.Logo"] = logoFile;
                }
                else
                {
                    return (byte[])Session["File.Comercio.Logo"];
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return logoFile;
        }

        private void validarArchivoCargado(byte[] archivo)
        {
            try
            {
                decimal tamanio = archivo.Count() / 1024;
                int tamanioPermitido = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Comercio.Logo.Archivo.TamañoPermitido"]) * 1024;
                if (tamanio > tamanioPermitido)
                {
                    MostrarMensajeError(TraducirPalabra("El tamaño del archivo cargado supera el máximo permitido (", "El tamaño del archivo cargado supera el máximo permitido (") + tamanioPermitido.ToString() + ")KB");
                    throw new BE.ExcepcionPersonalizada(false, new Exception(TraducirPalabra("El tamaño del archivo cargado supera el máximo permitido (", "El tamaño del archivo cargado supera el máximo permitido (") + tamanioPermitido.ToString() + ")KB"));
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCargarLogo_Click(object sender, EventArgs e)
        {
            try
            {
                ComercioLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(obtenerArchivoCargado());
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Hubo un error al cargar el logo del comercio: ", "Hubo un error al cargar el logo del comercio: ") + ex.Message);
            }
        }


        private void VerMisDescuentos()
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null)
                {
                    BindMisCuponesDescuentos();
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
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar el descuento: ", "Ocurrió un error al mostrar el descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar el descuento: ", "Ocurrió un error al mostrar el descuento: ") + ex.Message);
            }
        }

        private void BindMisCuponesDescuentos()
        {
            try
            {
                BLL.Descuento descuentoNeg = new BLL.Descuento();
                BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                if (usuario != null)
                {
                    LvMisDescuentos.DataSource = null;
                    Session["MisCupones"] = descuentoNeg.ObtenerDescuentos(usuario.Comercio);
                    LvMisDescuentos.DataSource = (List<BE.Descuento>)Session["MisCupones"];
                    LvMisDescuentos.DataBind();
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
        }

        protected void LvMisDescuentos_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            try
            {
                dpLvMisDescuentos.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                BindMisCuponesDescuentos();

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
        }

        protected void LvMisDescuentos_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.DataItem != null && e.Item.ItemType == ListViewItemType.DataItem)
                {
                    BE.Descuento descuento = (BE.Descuento)e.Item.DataItem;
                    if (descuento != null)
                    {
                        System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgProducto");
                        System.Web.UI.WebControls.Literal litTituloProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litTituloProducto");
                        System.Web.UI.WebControls.Literal litProductoCantidad = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoCantidad");

                        System.Web.UI.WebControls.Literal litUsuarioNombre = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litUsuarioNombre");
                        System.Web.UI.WebControls.Literal litUsuarioApellido = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litUsuarioApellido");
                        System.Web.UI.WebControls.Literal litUsuarioEmail = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litUsuarioEmail");
                        System.Web.UI.WebControls.Literal litUsuarioTelefono = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litUsuarioTelefono");

                        System.Web.UI.WebControls.Literal litProductoDescuento = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoDescuento");
                        System.Web.UI.WebControls.Literal litProductoAhorroTotal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoAhorroTotal");
                        System.Web.UI.WebControls.Literal litProductoPrecioFinal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoPrecioFinal");
                        System.Web.UI.WebControls.Literal litPuntoDeVenta = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPuntoDeVenta");
                        System.Web.UI.WebControls.Literal litProductoFechaCupon = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoFechaCupon");
                        System.Web.UI.WebControls.Literal litProductoFechaCanje = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoFechaCanje");
                        System.Web.UI.WebControls.Literal litProductoCupon = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoCupon");

                        if (descuento.Producto != null && descuento.PuntoDeVenta != null && descuento.Comercio != null)
                        {
                            if (img != null) { if (descuento.Producto != null) { img.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(descuento.Producto.Foto); } else { img.ImageUrl = @"~/Recurso/Imagen/empty.png"; } }
                            if (litTituloProducto != null) { litTituloProducto.Text = descuento.Producto.Titulo; }

                            if (litUsuarioNombre != null) { litUsuarioNombre.Text = descuento.Usuario.Nombre; }
                            if (litUsuarioApellido != null) { litUsuarioApellido.Text = descuento.Usuario.Apellido; }
                            if (litUsuarioEmail != null) { litUsuarioEmail.Text = descuento.Usuario.Email; }
                            if (litUsuarioTelefono != null)
                            {
                                if (descuento.Usuario.Telefono != null)
                                {
                                    litUsuarioTelefono.Text = descuento.Usuario.Telefono.Caracteristica + " " + descuento.Usuario.Telefono.NroTelefono;
                                    if (descuento.Usuario.Telefono.Celular)
                                    {
                                        litUsuarioTelefono.Text += "(CELULAR) " + descuento.Usuario.Telefono.Observacion;
                                    }
                                }
                                else
                                    litUsuarioTelefono.Text = "N/A";
                            }


                            if (litProductoCantidad != null) { litProductoCantidad.Text = Convert.ToString(descuento.Cantidad); }
                            if (litProductoDescuento != null) { litProductoDescuento.Text = Convert.ToString(descuento._Descuento); }
                            if (litProductoAhorroTotal != null) { litProductoAhorroTotal.Text = Convert.ToString(descuento.AhorroTotal); }
                            if (litProductoPrecioFinal != null) { litProductoPrecioFinal.Text = Convert.ToString(descuento.Producto.Precio - descuento.AhorroTotal); }
                            if (litPuntoDeVenta != null) { litPuntoDeVenta.Text = Convert.ToString(descuento.PuntoDeVenta); }
                            if (litProductoFechaCupon != null) { litProductoFechaCupon.Text = descuento.FechaCupon.ToString(); }
                            if (litProductoFechaCanje != null) { if (descuento.FechaCanje == DateTime.MinValue) { litProductoFechaCanje.Visible = false; } else { litProductoFechaCanje.Text = descuento.FechaCanje.ToString(); } }
                            if (litProductoCupon != null) { litProductoCupon.Text = descuento.Cupon; }
                        }


                        #region Traducir texto

                        BE.Idioma idiomaUsuario = null;
                        if ((BE.Idioma)Session["Idioma"] != null)
                            idiomaUsuario = (BE.Idioma)Session["Idioma"];

                        if (idiomaUsuario != null)
                        {
                        
                            Literal litDatosDeUsuario = (Literal)e.Item.FindControl("litDatosDeUsuario");
                        if(litDatosDeUsuario!=null)
                        {
                                object ctllitDatosDeUsuario = (object)litDatosDeUsuario;
                                Traducir(idiomaUsuario, ref ctllitDatosDeUsuario);
                        }
                        Literal litUsuarioNombreDescuento = (Literal)e.Item.FindControl("litUsuarioNombreDescuento");
                            if (litUsuarioNombreDescuento != null)
                            {
                                object ctllitUsuarioNombreDescuento = (object)litUsuarioNombreDescuento;
                                Traducir(idiomaUsuario, ref ctllitUsuarioNombreDescuento);
                            }
                            Literal litUsuarioApellidoDescuento = (Literal)e.Item.FindControl("litUsuarioApellidoDescuento");
                            if (litUsuarioApellidoDescuento != null)
                            {
                                object ctllitUsuarioApellidoDescuento = (object)litUsuarioApellidoDescuento;
                                Traducir(idiomaUsuario, ref ctllitUsuarioApellidoDescuento);
                            }
                            Literal litEmailDescuento = (Literal)e.Item.FindControl("litEmailDescuento");
                            if (litEmailDescuento != null)
                            {
                                object ctllitEmailDescuento = (object)litEmailDescuento;
                                Traducir(idiomaUsuario, ref ctllitEmailDescuento);
                            }
                            Literal litTelefono = (Literal)e.Item.FindControl("litTelefono");
                            if (litTelefono != null)
                            {
                                object ctllitTelefono = (object)litTelefono;
                                Traducir(idiomaUsuario, ref ctllitTelefono);
                            }
                            Literal litDetalleCupon = (Literal)e.Item.FindControl("litDetalleCupon");
                            if (litDetalleCupon != null)
                            {
                                object ctllitDetalleCupon = (object)litDetalleCupon;
                                Traducir(idiomaUsuario, ref ctllitDetalleCupon);
                            }
                            Literal litDetalleCantidad = (Literal)e.Item.FindControl("litDetalleCantidad");
                            if (litDetalleCantidad != null)
                            {
                                object ctllitDetalleCupon = (object)litDetalleCantidad;
                                Traducir(idiomaUsuario, ref ctllitDetalleCupon);
                            }
                            Literal litDetalleDescuento = (Literal)e.Item.FindControl("litDetalleDescuento");
                            if (litDetalleCantidad != null)
                            {
                                object ctlllitDetalleDescuento = (object)litDetalleDescuento;
                                Traducir(idiomaUsuario, ref ctlllitDetalleDescuento);
                            }
                            Literal litDetalleAhorro = (Literal)e.Item.FindControl("litDetalleAhorro");
                            if (litDetalleCantidad != null)
                            {
                                object ctlllitDetalleAhorro = (object)litDetalleAhorro;
                                Traducir(idiomaUsuario, ref ctlllitDetalleAhorro);
                            }
                            Literal litDetallePrecioFinal = (Literal)e.Item.FindControl("litDetallePrecioFinal");
                            if (litDetallePrecioFinal != null)
                            {
                                object ctllitDetallePrecioFinal = (object)litDetallePrecioFinal;
                                Traducir(idiomaUsuario, ref ctllitDetallePrecioFinal);
                            }
                            Literal litDetalleRetiraPor = (Literal)e.Item.FindControl("litDetalleRetiraPor");
                            if (litDetalleRetiraPor != null)
                            {
                                object ctllitDetalleRetiraPor = (object)litDetalleRetiraPor;
                                Traducir(idiomaUsuario, ref ctllitDetalleRetiraPor);
                            }


                            Literal litCuponInformacion = (Literal)e.Item.FindControl("litCuponInformacion");
                            if (litCuponInformacion != null)
                            {
                                object ctllitCuponInformacion = (object)litCuponInformacion;
                                Traducir(idiomaUsuario, ref ctllitCuponInformacion);
                            }
                            Literal litCuponFechaGeneracion = (Literal)e.Item.FindControl("litCuponFechaGeneracion");
                            if (litCuponFechaGeneracion != null)
                            {
                                object ctllitCuponInformacion = (object)litCuponFechaGeneracion;
                                Traducir(idiomaUsuario, ref ctllitCuponInformacion);
                            }
                            //Literal litProductoFechaCanje = (Literal)e.Item.FindControl("litProductoFechaCanje");

                            Button btnConfirmar = (Button)e.Item.FindControl("btnConfirmar");
                            if (btnConfirmar != null)
                            {
                                object ctlbtnConfirmar = (object)btnConfirmar;
                                Traducir(idiomaUsuario, ref ctlbtnConfirmar);
                            }
                            Button btnCancelar = (Button)e.Item.FindControl("btnCancelar");
                            if (btnCancelar != null)
                            {
                                object ctlbtnCancelar = (object)btnCancelar;
                                Traducir(idiomaUsuario, ref ctlbtnCancelar);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
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
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al traducir: ", "Ocurrió un error al traducir: ") + ex.Message);
            }
        }

        protected void LvMisDescuentos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            List<BE.Descuento> lstDescuentos = null;
            try
            {
                if (e.CommandName != string.Empty && e.CommandArgument != null)
                {
                    switch (e.CommandName)
                    {
                        case "confirmar":
                            lstDescuentos = (List<BE.Descuento>)Session["MisCupones"];
                            BE.Descuento descuento = (from d in lstDescuentos where Convert.ToInt32(d.CodCupon) == Convert.ToInt32(e.CommandArgument) select d).FirstOrDefault<BE.Descuento>();
                            if (descuento != null)
                            {
                                BLL.Descuento descuentoNeg = new BLL.Descuento();
                                descuentoNeg.Confirmar(descuento);
                                MostrarMensajeOk(TraducirPalabra("EL CUPÓN SE HA ACREDITADO ÉXITOSAMENTE", "EL CUPÓN SE HA ACREDITADO ÉXITOSAMENTE"));
                                BindMisCuponesDescuentos();
                                //Listo para canjear:
                            }
                            break;
                        case "cancelar":
                             lstDescuentos = (List<BE.Descuento>)Session["MisCupones"];
                            BE.Descuento descuentoCancelar = (from d in lstDescuentos where Convert.ToInt32(d.CodCupon) == Convert.ToInt32(e.CommandArgument) select d).FirstOrDefault<BE.Descuento>();
                            if (descuentoCancelar != null)
                            {
                                BLL.Descuento descuentoNeg = new BLL.Descuento();
                                descuentoNeg.Cancelar(descuentoCancelar);
                                MostrarMensajeOk(TraducirPalabra("EL CUPÓN SE HA CANCELADO ÉXITOSAMENTE", "EL CUPÓN SE HA CANCELADO ÉXITOSAMENTE"));
                                BindMisCuponesDescuentos();
                                //Listo para canjear:
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
        }

        private void ArmarDashboard()
        {
            try
            {
                tituloDescuentosPorProducto.InnerText = TraducirPalabra("DISTRIBUCIÓN DE PRODUCTOS CANJEADOS (POR UNIDAD)", "DISTRIBUCIÓN DE PRODUCTOS CANJEADOS (POR UNIDAD)");
                tituloCantidadProductosPorPuntoVenta.InnerText = TraducirPalabra("CANTIDAD DE PRODUCTOS CANJEADOS POR PUNTO DE VENTA", "CANTIDAD DE PRODUCTOS CANJEADOS POR PUNTO DE VENTA");
                tituloEstadisticaCantidadCuponesCanjeadosPorFecha.InnerText = TraducirPalabra("CANTIDAD DE CUPONES CANJEADOS POR FECHA", "CANTIDAD DE CUPONES CANJEADOS POR FECHA");

                if (Session["Usuario"] != null)
                { 
                    BE.Comercio comercio = ((BE.Usuario)Session["Usuario"]).Comercio;
                    if(comercio != null) {
                        comercioNeg = new BLL.Comercio(); 
                        int cantidadPuntosVenta = comercioNeg.EstadisticaCantidadPuntosDeVenta(comercio);
                        int cantidadProductos = comercioNeg.EstadisticaCantidadProductos(comercio);
                        int cantidadTicketsPendientes = comercioNeg.EstadisticaCantidadTicketsPendientes(comercio);

                        if (cantidadPuntosVenta <= 1)
                            litPuntosDeVenta.Text = Convert.ToInt32(cantidadPuntosVenta)+TraducirPalabra(" PUNTO DE VENTA", " PUNTO DE VENTA");
                        if(cantidadPuntosVenta > 1)
                            litPuntosDeVenta.Text = Convert.ToInt32(cantidadPuntosVenta) + " PUNTOS DE VENTA";

                        if (cantidadProductos <= 1)
                            litProductos.Text = Convert.ToInt32(cantidadProductos) + " PRODUCTO";
                        if (cantidadProductos > 1)
                            litProductos.Text = Convert.ToInt32(cantidadProductos) + " PRODUCTOS";

                        if (cantidadTicketsPendientes <= 1)
                            litCuponesPendientes.Text = Convert.ToInt32(cantidadTicketsPendientes) + " TICKET PENDIENTES ";
                        if (cantidadTicketsPendientes > 1)
                            litCuponesPendientes.Text = Convert.ToInt32(cantidadTicketsPendientes) + "  TICKETS PENDIENTES";

                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al armar el reporte: ", "Ocurrió un error al armar el reporte: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al armar el reporte: ", "Ocurrió un error al armar el reporte: ") + ex.Message);
            }
        }
        

        protected void navEstadisticasComercio_Click(object sender, EventArgs e)
        {
            try
            {
                pnMisDatos.Visible = false;
                pnMisDescuentos.Visible = false;
                pnMisProductos.Visible = false;
                pnEstadisticasMiComercio.Visible = true;
                ArmarDashboard();

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al armar el reporte: ", "Ocurrió un error al armar el reporte: ") + ex.Message);
            }
        }

       



    }
}