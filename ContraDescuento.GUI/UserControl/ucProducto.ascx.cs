using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI.UserControl
{
    public partial class Producto : System.Web.UI.UserControl
    {
        #region Propiedad Privada
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Domicilio> PuntosDeVenta = new List<BE.Domicilio>();
        BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
        BLL.Producto productoNeg = new BLL.Producto();
        bool PostFromMenuProducto = false;
        string fileName;
        string logo;
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //divMensajeError.Visible = false;
                divMensajeOK.Visible = false;
                ValidarPermisos();
                if (Session["MiProducto.IsPostBack"] != null && Convert.ToBoolean(Session["MiProducto.IsPostBack"]) == false)
                    CargarPuntosDeVenta();
                
                if (!IsPostBack)
                {
                    BindProductoCategoria();
                    CargarPuntosDeVenta();
                    Session["Producto.Modificar"] = null;
                    LimpiarFormularioProductoNuevo();
                  
                }
                  Traducir();
                CargarProductos();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al cargar el producto y sus puntos de venta: ", "Ocurrió un error al cargar el producto y sus puntos de venta: ") + ex.Message);
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

        private void CargarProductos()
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null && usuario.Comercio != null)
                {
                    List<BE.Producto> lstProductos = productoNeg.Listar(usuario.Comercio);
                    rptProducto.DataSource = null;
                    rptProducto.DataSource = lstProductos;
                    rptProducto.DataBind();
                    Session["Productos"] = lstProductos;
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener el producto: ", "Ocurrió un error al obtener el producto: ") + ex.Message);
            }
        }


        private void BindProductoCategoria()
        {
            try
            {
                ddlProductoCategoria.DataSource = null;
                Session["Categoria"] = productoNeg.ListarCategoria();
                ddlProductoCategoria.DataSource = (List<BE.Categoria>)Session["Categoria"];
                ddlProductoCategoria.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener el punto de venta : ", "Ocurrió un error al obtener el punto de venta : ") + ex.Message);
            }
        }
        private List<BE.Domicilio> CargarPuntosDeVenta()
        {

            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null && usuario.Comercio != null)
                {
                    PuntosDeVenta = puntoDeVentaNeg.Listar(usuario.Comercio);
                    Session["Comercio.PuntosDeVenta"] = PuntosDeVenta;
                    rptPuntoDeVentaStock.DataSource = null;
                    rptPuntoDeVentaStock.DataSource = PuntosDeVenta;
                    rptPuntoDeVentaStock.DataBind();
                    //chkLstPuntoDeVenta.DataSource = null;
                    //chkLstPuntoDeVenta.DataSource = PuntosDeVenta;
                    //chkLstPuntoDeVenta.DataBind();
                }
                else
                {
                    Response.Redirect("index.aspx");
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Ocurrió un error al obtener el punto de venta : ", "Ocurrió un error al obtener el punto de venta : ") + ex.Message);
            }
            return PuntosDeVenta;
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



        private void LimpiarFormularioProductoNuevo()
        {
            try
            {
                //OcultarMensajeError();
                if (Session["Producto.Modificar"] == null)
                {
                    btnActualizarProducto.Visible = false;
                    btnBorrarProducto.Visible = false;
                    btnCrearProducto.Visible = true;
                    txtProductoTitulo.Text = string.Empty;
                    ChkMantenerLogo.Checked = false;
                    txtProductoDescripcion.Text = string.Empty;
                    txtProductoPrecio.Text = string.Empty;
                    txtProductoDescuento.Text = string.Empty;
                    //txtProductoCantidad.Text = string.Empty;
                    txtFechaVigenciaDesde.Text = string.Empty;
                    txtFechaVigenciaHasta.Text = string.Empty;
                    //CargarPuntosDeVenta();
                    Session["File.Comercio.Producto"] = null;
                    imgProducto.ImageUrl = @"~/Recurso/Imagen/foto.png";
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Error al crear el producto: ", "Error al crear el producto: ") + ex.Message);
            }
        }

        protected void rptProducto_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.Item != null && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    switch (e.CommandName)
                    {
                        case "Editar":
                            divMensajeOK.Visible = false;
                            CargarProductos(Convert.ToInt32(e.CommandArgument));
                            break;
                        case "Borrar":
                            HiddenField hdnPuntoDeVenta = (HiddenField)e.Item.FindControl("hdnPuntoDeVenta");
                            if (hdnPuntoDeVenta != null) {
                                BorrarProducto(Convert.ToInt32(e.CommandArgument), Convert.ToInt32(hdnPuntoDeVenta.Value));
                            }
                            Session["Producto.Modificar"] = null;
                            Session["Producto.Editar"] = null;
                            LimpiarFormularioProductoNuevo();
                            CargarProductos();
                            CargarPuntosDeVenta();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError(TraducirPalabra("Ha ocurrido un error al operar sobre el producto: ", "Ha ocurrido un error al operar sobre el producto: ") + ex.Message);
            }
        }

        private void CargarProductos(int codProducto)
        {
            try
            {
                Session["Producto.Selected"] = codProducto;
                List<BE.Producto> lstProductos = (List<BE.Producto>)Session["Productos"];
                if (lstProductos.Count > 0)
                {
                    BE.Producto prod = (from _prod in lstProductos where _prod.CodProducto == codProducto select _prod).FirstOrDefault<BE.Producto>();
                    Session["Producto.Editar"] = prod;
                    if (prod != null)
                    {
                        panelSubCategoria.Visible = false;

                        txtProductoTitulo.Text = prod.Titulo;
                        ChkMantenerLogo.Checked = true;
                        txtProductoPrecio.Text = prod.Precio.ToString("0.##");
                        //txtProductoCantidad.Text = Convert.ToString(prod.Cantidad);
                        txtProductoDescuento.Text = Convert.ToString(Convert.ToInt32(prod.Descuento));
                        if (prod.Categoria != null && prod.Categoria.CodCategoria > 0)
                            ddlProductoCategoria.SelectedValue = prod.Categoria.CodCategoria.ToString();
                        if (prod.SubCategoria != null && prod.SubCategoria.CodSubCategoria > 0)
                        {

                            Session["SubCategoria"] = productoNeg.ListarSubCategoria(prod.Categoria);
                            ddlProductoSubCategoria.DataSource = null;
                            ddlProductoSubCategoria.DataSource = (List<BE.SubCategoria>)Session["SubCategoria"];
                            ddlProductoSubCategoria.DataBind();
                            ddlProductoSubCategoria.SelectedValue = prod.SubCategoria.CodSubCategoria.ToString();
                            panelSubCategoria.Visible = true;
                        }
                        txtProductoDescripcion.Text = prod.Descripcion;
                        txtFechaVigenciaDesde.Text = prod.FechaVigenciaDesde.ToString("dd/MM/yyyy");
                        txtFechaVigenciaHasta.Text = prod.FechaVigenciaHasta.ToString("dd/MM/yyyy");

                        if (prod.Foto != null)
                            imgProducto.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(prod.Foto);
                        else
                            imgProducto.ImageUrl = @"~/Recurso/Imagen/foto.png";


                        CargarPuntosDeVenta(); //NUEVO
                        //foreach (ListItem item in chkLstPuntoDeVenta.Items)
                        //{
                        //    foreach (BE.Domicilio dom in prod.PuntosDeVenta)
                        //    {
                        //        if (item.Value == dom.CodDomicilio.ToString())
                        //        {
                        //            item.Selected = true;
                        //        }
                        //    }

                        //}
                    }
                    Session["Producto.Modificar"] = prod;
                    btnActualizarProducto.Visible = true;
                    btnBorrarProducto.Visible = true;
                    btnCrearProducto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al cargar los productos: ", "Hubo un error al cargar los productos: ") + ex.Message);
            }

        }

        protected void rptProducto_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item != null && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    Image prodImage = (Image)e.Item.FindControl("thumbnailProducto");
                    System.Web.UI.HtmlControls.HtmlControl RowItem = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("RowItem");
                    Literal txtProductoTituloItem = (Literal)e.Item.FindControl("litProductoTituloItem");
                    Literal txtProductoPrecioItem = (Literal)e.Item.FindControl("litProductoPrecioItem");
                    Literal txtProductoDescuentoItem = (Literal)e.Item.FindControl("litProductoDescuentoItem");
                    Literal txtProductoCantidadItem = (Literal)e.Item.FindControl("litProductoCantidadItem");
                    Literal txtProductoCategoriaItem = (Literal)e.Item.FindControl("litProductoCategoriaItem");
                    Literal txtProductoSubCategoriaItem = (Literal)e.Item.FindControl("litProductoSubCategoriaItem");
                    Literal txtProductoVigenciaDesdeItem = (Literal)e.Item.FindControl("litProductoVigenciaDesdeItem");
                    Literal txtProductoVigenciaHastaItem = (Literal)e.Item.FindControl("litProductoVigenciaHastaItem");
                    Literal txtProductoPuntoDeVentaItem = (Literal)e.Item.FindControl("litPuntosDeVentaItem");
                    HiddenField hdnPuntoDeVenta = (HiddenField)e.Item.FindControl("hdnPuntoDeVenta");

                    BE.Producto prod = (BE.Producto)e.Item.DataItem;
                    if (prod != null)
                    {

                        if (prodImage != null)
                        {
                            if (prod.Foto == null)
                                prodImage.ImageUrl = @"~/Recurso/Imagen/foto.png";
                            else
                                prodImage.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(prod.Foto);
                        }


                        if (txtProductoTituloItem != null)
                        {
                            txtProductoTituloItem.Text = prod.Titulo;
                        }

                        if (txtProductoPrecioItem != null)
                        {
                            txtProductoPrecioItem.Text = prod.Precio.ToString("0.##");
                        }
                        if (txtProductoDescuentoItem != null)
                        {
                            txtProductoDescuentoItem.Text = prod.Descuento.ToString("0.##");
                        }
                       
                        if (txtProductoCategoriaItem != null)
                        {
                            txtProductoCategoriaItem.Text = prod.Categoria.Descripcion;
                        }
                        if (txtProductoSubCategoriaItem != null)
                        {
                            txtProductoSubCategoriaItem.Text = prod.SubCategoria.Descripcion;
                        }
                        if (txtProductoVigenciaDesdeItem != null)
                        {
                            txtProductoVigenciaDesdeItem.Text = prod.FechaVigenciaDesde.ToString("dd/MM/yyyy");
                        }

                        if (txtProductoVigenciaHastaItem != null)
                        {
                            txtProductoVigenciaHastaItem.Text = prod.FechaVigenciaHasta.ToString("dd/MM/yyyy");
                        }

                        if (txtProductoPuntoDeVentaItem != null)
                        {
                            foreach (BE.Domicilio dom in prod.PuntosDeVenta)
                            {
                                txtProductoPuntoDeVentaItem.Text += "<li class='list-group-item'>" + dom + "</li>";
                                if(txtProductoCantidadItem!=null)
                                {
                                    int cantidad = 0;
                                    prod.PuntosDeVentaStock.TryGetValue(dom, out cantidad);
                                    if (cantidad > 0)
                                        txtProductoCantidadItem.Text = Convert.ToString(cantidad);
                                    else
                                    {
                                        if (RowItem != null) { 
                                            RowItem.Attributes.Add("class", "row bg-danger");
                                            txtProductoCantidadItem.Text = "SIN STOCK";
                                        }
                                    }
                                        //NO HAY STOCK!!!
                                }
                                if (hdnPuntoDeVenta != null)
                                {
                                    hdnPuntoDeVenta.Value = Convert.ToString(dom.CodDomicilio);
                                }
                            }
                        }

                     

                    }
                    
                    }
                else if (e.Item != null && e.Item.ItemType == ListItemType.Header)
                {
                    #region Traduccion columnas
                    BE.Idioma idiomaUsuario = null;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {
                        Literal litFotoRepeater = (Literal)e.Item.FindControl("litFotoRepeater");
                        if (litFotoRepeater != null)
                        {
                            object ctllitFotoRepeaterTitulo = (object)litFotoRepeater;
                            Traducir(idiomaUsuario, ref ctllitFotoRepeaterTitulo);
                        }
                        Literal litTituloRepeater = (Literal)e.Item.FindControl("litTituloRepeater");
                        if (litTituloRepeater != null)
                        {
                            object ctlitTituloRepeater = (object)litTituloRepeater;
                            Traducir(idiomaUsuario, ref ctlitTituloRepeater);
                        }
                        Literal litPrecioRepeater = (Literal)e.Item.FindControl("litPrecioRepeater");
                        if (litPrecioRepeater != null)
                        {
                            object ctllitPrecioRepeater = (object)litPrecioRepeater;
                            Traducir(idiomaUsuario, ref ctllitPrecioRepeater);
                        }
                        Literal litDescuentoRepeater = (Literal)e.Item.FindControl("litDescuentoRepeater");
                        if (litDescuentoRepeater != null)
                        {
                            object ctllitDescuentoRepeater = (object)litDescuentoRepeater;
                            Traducir(idiomaUsuario, ref ctllitDescuentoRepeater);
                        }

                        Literal litCantidadRepeater = (Literal)e.Item.FindControl("litCantidadRepeater");
                        if (litCantidadRepeater != null)
                        {
                            object ctllitCantidadRepeater = (object)litCantidadRepeater;
                            Traducir(idiomaUsuario, ref ctllitCantidadRepeater);
                        }
                        Literal litProductoCategoriaRepeater = (Literal)e.Item.FindControl("litProductoCategoriaRepeater");
                        if (litProductoCategoriaRepeater != null)
                        {
                            object ctllitProductoCategoriaRepeater = (object)litProductoCategoriaRepeater;
                            Traducir(idiomaUsuario, ref ctllitProductoCategoriaRepeater);
                        }
                        Literal litProductoSubCategoriaRepeater = (Literal)e.Item.FindControl("litProductoSubCategoriaRepeater");
                        if (litProductoSubCategoriaRepeater != null)
                        {
                            object ctllitProductoSubCategoriaRepeater = (object)litProductoSubCategoriaRepeater;
                            Traducir(idiomaUsuario, ref ctllitProductoSubCategoriaRepeater);
                        }
                        Literal litFechaVigenciaDesdeRepeater = (Literal)e.Item.FindControl("litFechaVigenciaDesdeRepeater");
                        if (litFechaVigenciaDesdeRepeater != null)
                        {
                            object ctllitFechaVigenciaDesdeRepeater = (object)litFechaVigenciaDesdeRepeater;
                            Traducir(idiomaUsuario, ref ctllitFechaVigenciaDesdeRepeater);
                        }
                        Literal litFechaVigenciaHastaRepeater = (Literal)e.Item.FindControl("litFechaVigenciaHastaRepeater");
                        if (litFechaVigenciaHastaRepeater != null)
                        {
                            object ctllitFechaVigenciaHastaRepeater = (object)litFechaVigenciaHastaRepeater;
                            Traducir(idiomaUsuario, ref ctllitFechaVigenciaHastaRepeater);
                        }
                        Literal litPuntosDeVentaRepeater = (Literal)e.Item.FindControl("litPuntosDeVentaRepeater");
                        if (litPuntosDeVentaRepeater != null)
                        {
                            object ctllitPuntosDeVentaRepeater = (object)litPuntosDeVentaRepeater;
                            Traducir(idiomaUsuario, ref ctllitPuntosDeVentaRepeater);
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al cargar los productos: ", "Hubo un error al cargar los productos: ") + ex.Message);
            }
        }

        protected void btnCargarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] imagen = obtenerArchivoCargado();
                if (imagen != null)
                    imgProducto.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagen);
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor cargue la imagen", "Por favor cargue la imagen")));
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al cargar los productos: ", "Hubo un error al cargar los productos: ") + ex.Message);
            }
        }


        private byte[] obtenerArchivoCargado()
        {
            byte[] productoFile = null;
            try
            {
                if (!ChkMantenerLogo.Checked && fpProductoFoto.HasFile)
                {
                    fileName = System.IO.Path.GetFileName(fpProductoFoto.FileName);
                    logo = System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Producto"] + fileName;
                    if (!System.IO.Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Producto"]))
                        System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Producto"]);
                    fpProductoFoto.SaveAs(System.Configuration.ConfigurationSettings.AppSettings["Path.Comercio.Producto"] + fileName);
                    productoFile = System.IO.File.ReadAllBytes(logo);
                    validarArchivoCargado(productoFile);
                    Session["File.Comercio.Producto"] = productoFile;
                }
                else
                {
                    if (Session["File.Comercio.Producto"] != null)
                        return (byte[])Session["File.Comercio.Producto"];
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            return productoFile;
        }

        private List<BE.Domicilio> obtenerPuntosDeVenta()
        {
            return (List<BE.Domicilio>)Session["Comercio.PuntosDeVenta"];
        }

        private Dictionary<BE.Domicilio, int> obtenerPuntosDeVentaSeleccionados()
        {
            Dictionary<BE.Domicilio, int> lstPuntosDeVentaSelccionados = new Dictionary<BE.Domicilio, int>();
            try
            {
                BE.Comercio comercio = ((BE.Usuario)Session["Usuario"]).Comercio;
                List<BE.Domicilio> lstPuntosDeVenta = new List<BE.Domicilio>();
                lstPuntosDeVenta = puntoDeVentaNeg.Listar(comercio);

                //foreach (ListItem item in chkLstPuntoDeVenta.Items)
                //{
                //    if (item.Selected)
                //    {
                //        lstPuntosDeVentaSelccionados.Add(
                //            (from pto in lstPuntosDeVenta where pto.CodDomicilio == Convert.ToInt32(item.Value) select pto).FirstOrDefault<BE.Domicilio>()
                //            );
                //    }
                //}

                foreach (RepeaterItem puntoDeVentaStock in rptPuntoDeVentaStock.Items)
                {
                    System.Web.UI.WebControls.HiddenField hdnPuntoDeVenta = (System.Web.UI.WebControls.HiddenField)puntoDeVentaStock.FindControl("hdnPuntoDeVenta");
                    System.Web.UI.WebControls.CheckBox chkPuntoDeVenta = (System.Web.UI.WebControls.CheckBox)puntoDeVentaStock.FindControl("chkSeleccion");
                    System.Web.UI.WebControls.TextBox txtProductoCantidad = (System.Web.UI.WebControls.TextBox)puntoDeVentaStock.FindControl("txtProductoCantidad");
                    foreach (BE.Domicilio puntoDeVenta in lstPuntosDeVenta)
                    {
                        if (hdnPuntoDeVenta != null && hdnPuntoDeVenta.Value != string.Empty)
                        {
                            if (puntoDeVenta.CodDomicilio == Convert.ToInt32(hdnPuntoDeVenta.Value))

                                if (chkPuntoDeVenta != null)
                                {
                                    if (chkPuntoDeVenta.Checked)
                                    {
                                        if (txtProductoCantidad != null)
                                            lstPuntosDeVentaSelccionados.Add(puntoDeVenta, Convert.ToInt32(txtProductoCantidad.Text));
                                    }
                                }
                        }
                    }
                }

                Session["Comercio.PuntoDeVenta.Selected"] = lstPuntosDeVentaSelccionados;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(ex.Message);
            }
            return lstPuntosDeVentaSelccionados;
        }
        private void validarArchivoCargado(byte[] archivo)
        {
            decimal tamanio = archivo.Count() / 1024;
            int tamanioPermitido = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Comercio.Producto.Archivo.TamañoPermitido"]) * 1024;
            if (tamanio > tamanioPermitido)
                MostrarMensajeError("El tamaño del archivo cargado supera el máximo permitido (" + tamanioPermitido.ToString() + ")");

        }

        //protected void chkLstPuntoDeVenta_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    List<BE.Domicilio> lstPuntosDeVentaSelccionados = new List<BE.Domicilio>();
        //    PuntosDeVenta = obtenerPuntosDeVenta();
        //    try
        //    {
        //        foreach (ListItem chkPtoVenta in chkLstPuntoDeVenta.Items)
        //        {
        //            if (chkPtoVenta.Selected == true)
        //            {
        //                lstPuntosDeVentaSelccionados.Add(
        //                    (from ptoVenta in PuntosDeVenta where ptoVenta.CodDomicilio == Convert.ToInt32(chkPtoVenta.Value) select ptoVenta).First<BE.Domicilio>()
        //                    );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        MostrarMensajeError("Hubo un error al cargar la imagen del producto: " + ex.Message);
        //    }
        //    Session["Producto.PuntoDeVenta.Selected"] = lstPuntosDeVentaSelccionados;
        //}


        /// <summary>
        /// Solo borra el producto para el punto de venta seleccionado
        /// </summary>
        /// <param name="codProducto"></param>
        private void BorrarProducto(int codProducto,int codPuntoVenta)
        {
            try
            {
                Session["Producto.Selected"] = 0;
                List<BE.Producto> lstProductos = (List<BE.Producto>)Session["Productos"];
                if (lstProductos.Count > 0)
                {
                    BE.Producto prod = 
                        (from _prod 
                         in lstProductos
                         where _prod.CodProducto == codProducto && 
                         (from ptoVenta in _prod.PuntosDeVenta where ptoVenta.CodDomicilio == codPuntoVenta select ptoVenta)
                         .FirstOrDefault<BE.Domicilio>() != null select _prod)
                         .FirstOrDefault<BE.Producto>();
                    //BE.Domicilio dom = (from ptoVenta in prod.PuntosDeVenta where ptoVenta.CodDomicilio == codPuntoVenta select ptoVenta).FirstOrDefault<BE.Domicilio>();
                    BE.Domicilio dom = (from ptoVenta in prod.PuntosDeVenta where ptoVenta.CodDomicilio == codPuntoVenta select ptoVenta).FirstOrDefault<BE.Domicilio>();
                    if (prod != null)
                    {
                        productoNeg.BajaProductoPuntoDeVenta(prod, ((BE.Usuario)Session["Usuario"]).Comercio, dom);

                        //foreach (BE.Domicilio puntoDeVenta in prod.PuntosDeVenta)
                        //{
                            //productoNeg.BajaProductoPuntoDeVenta(prod, ((BE.Usuario)Session["Usuario"]).Comercio, puntoDeVenta);
                        //}

                    }
                }
            }
            catch (Exception ex)
            {

                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al borrar el producto: ", "Hubo un error al borrar el producto: ") + ex.Message);
            }
        }

        protected void btnActualizarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null && usuario.Comercio != null)
                {
                    BE.Producto producto = (BE.Producto)Session["Producto.Modificar"];
                    producto.Descripcion = txtProductoDescripcion.Text;
                    producto.Titulo = txtProductoTitulo.Text;
                    producto.Precio = Convert.ToDecimal(txtProductoPrecio.Text);
                    producto.Descuento = Convert.ToDecimal(txtProductoDescuento.Text);
                    //producto.Cantidad = Convert.ToInt32(txtProductoCantidad.Text);
                    producto.MantenerFoto = ChkMantenerLogo.Checked;
                    if (!ChkMantenerLogo.Checked)
                    {

                        producto.Foto = obtenerArchivoCargado();
                    }
                    

                        producto.PuntosDeVentaStock = obtenerPuntosDeVentaSeleccionados();

                    if (Convert.ToDateTime(txtFechaVigenciaDesde.Text) > Convert.ToDateTime(txtFechaVigenciaHasta.Text))
                        throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("La fecha de vigencia DESDE no puede ser mayor que la de HASTA", "La fecha de vigencia DESDE no puede ser mayor que la de HASTA")));

                    producto.FechaVigenciaDesde = Convert.ToDateTime(txtFechaVigenciaDesde.Text);
                    producto.FechaVigenciaHasta = Convert.ToDateTime(txtFechaVigenciaHasta.Text);

                    if (producto.Categoria == null)
                        producto.Categoria = new BE.Categoria();
                    producto.Categoria.CodCategoria = Convert.ToInt32(ddlProductoCategoria.SelectedValue);
                    if (producto.SubCategoria == null)
                        producto.SubCategoria = new BE.SubCategoria();
                    producto.SubCategoria.CodSubCategoria = Convert.ToInt32(ddlProductoSubCategoria.SelectedValue);

                    productoNeg.Modificar(producto, usuario.Comercio);
                    Session["Producto.Editar"] = null;
                    LimpiarFormularioProductoNuevo();
                    MostrarMensajeOk(TraducirPalabra("Se ha actualizado éxitosamente el producto", "Se ha actualizado éxitosamente el producto"));
                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al actualizar el producto: ", "Hubo un error al actualizar el producto: ") + ex.Message);
            }
        }

        protected void btnCrearProducto_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null && usuario.Comercio != null)
                {
                    BE.Producto producto = new BE.Producto();
                    producto.Descripcion = txtProductoDescripcion.Text;
                    producto.Titulo = txtProductoTitulo.Text;
                    producto.Precio = Convert.ToDecimal(txtProductoPrecio.Text);
                    producto.Descuento = Convert.ToDecimal(txtProductoDescuento.Text);
                    //producto.Cantidad = Convert.ToInt32(txtProductoCantidad.Text);
                    producto.Foto = obtenerArchivoCargado();
                    if (producto.Foto == null)
                        throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor Cargue la foto del producto", "Por favor Cargue la foto del producto")));
                    producto.PuntosDeVentaStock = obtenerPuntosDeVentaSeleccionados();
                    //foreach (BE.Domicilio dom in producto.PuntosDeVenta)
                    //{
                    //    producto.PuntosDeVentaStock.Add(dom, Convert.ToInt32(txtProductoCantidad.Text));
                    //}
                    if (txtFechaVigenciaDesde.Text != string.Empty)
                        producto.FechaVigenciaDesde = Convert.ToDateTime(txtFechaVigenciaDesde.Text);
                    if (txtFechaVigenciaHasta.Text != string.Empty)
                        producto.FechaVigenciaHasta = Convert.ToDateTime(txtFechaVigenciaHasta.Text);

                    if (Convert.ToDateTime(txtFechaVigenciaDesde.Text) > Convert.ToDateTime(txtFechaVigenciaHasta.Text))
                        throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("La fecha de vigencia DESDE no puede ser mayor que la de HASTA", "La fecha de vigencia DESDE no puede ser mayor que la de HASTA")));


                    producto.Categoria = new BE.Categoria();
                    producto.Categoria.CodCategoria = Convert.ToInt32(ddlProductoCategoria.SelectedValue);
                    producto.SubCategoria = new BE.SubCategoria();
                    producto.SubCategoria.CodSubCategoria = Convert.ToInt32(ddlProductoSubCategoria.SelectedValue);
                    productoNeg.Alta(ref producto, usuario.Comercio);
                    Session["Producto.Modificar"] = null;

                    LimpiarFormularioProductoNuevo();
                    CargarPuntosDeVenta();
                    MostrarMensajeOk(TraducirPalabra("Se ha creado éxitosamente el producto", "Se ha creado éxitosamente el producto"));
                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Error al crear el producto: ", "Error al crear el producto: ") + ex.Message);
            }
        }

        protected void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null && usuario.Comercio != null)
                {
                    BE.Producto producto = (BE.Producto)Session["Producto.Modificar"];
                    BLL.Producto productoNeg = new BLL.Producto();
                    productoNeg.Baja(producto, usuario.Comercio);


                    Session["Producto.Modificar"] = null;
                    LimpiarFormularioProductoNuevo();
                    CargarProductos();
                    MostrarMensajeOk(TraducirPalabra("Se ha borrado éxitosamente el producto", "Se ha borrado éxitosamente el producto"));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al actualizar el producto: ", "Hubo un error al actualizar el producto: ") + ex.Message);
            }
        }

        protected void ddlProductoCategoria_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BE.Categoria categoria = null;
                List<BE.Categoria> lstCategoria = (List<BE.Categoria>)Session["Categoria"];

                categoria = (from cat in lstCategoria where Convert.ToInt32(cat.CodCategoria) == Convert.ToInt32(ddlProductoCategoria.SelectedValue) select cat).FirstOrDefault<BE.Categoria>();
                if (lstCategoria != null && categoria != null)
                {
                    panelSubCategoria.Visible = true;
                    Session["SubCategoria"] = productoNeg.ListarSubCategoria(categoria);
                    ddlProductoSubCategoria.DataSource = null;
                    ddlProductoSubCategoria.DataSource = (List<BE.SubCategoria>)Session["SubCategoria"];
                    ddlProductoSubCategoria.DataBind();
                }
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("No se ha podido recuperar la lista de categoria de la BD", "No se ha podido recuperar la lista de categoria de la BD")));
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al mostrar formulario de creación de producto: ", "Hubo un error al mostrar formulario de creación de producto: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al mostrar formulario de creación de producto: ", "Hubo un error al mostrar formulario de creación de producto: ") + ex.Message);
            }
        }
        protected void btnProductoNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Producto.Modificar"] = null;
                Session["Producto.Editar"] = null;
                LimpiarFormularioProductoNuevo();
                CargarProductos();
                CargarPuntosDeVenta();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al mostrar formulario de creación de producto: ", "Hubo un error al mostrar formulario de creación de producto: ") + ex.Message);
            }
        }

        protected void rptPuntoDeVentaStock_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            BE.Idioma idiomaUsuario = null;
            try
            {
                if (e.Item.DataItem != null)
                {
                    System.Web.UI.WebControls.HiddenField hdnPuntoDeVenta = (System.Web.UI.WebControls.HiddenField)e.Item.FindControl("hdnPuntoDeVenta");
                    System.Web.UI.WebControls.CheckBox chkSeleccion = (System.Web.UI.WebControls.CheckBox)e.Item.FindControl("chkSeleccion");
                    System.Web.UI.WebControls.TextBox txtProductoCantidad = (System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtProductoCantidad");

                    BE.Domicilio dom = (BE.Domicilio)e.Item.DataItem;

                    if (hdnPuntoDeVenta != null)
                    {
                        hdnPuntoDeVenta.Value = Convert.ToString(dom.CodDomicilio);
                    }
                    if (chkSeleccion != null)
                    {
                        chkSeleccion.Text = dom.ToString();
                    }


                    BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
                    BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                    BE.Producto productoEdicion = (BE.Producto)Session["Producto.Editar"];
                    Dictionary<BE.Domicilio, int> lstPuntoDeVentaStock = null;
                    if (usuario!=null)
                    {  
                        if(usuario.Comercio!=null)
                        {
                            if(productoEdicion!=null)
                            {
                               lstPuntoDeVentaStock =  puntoDeVentaNeg.ListarStockPorPuntoDeVenta(usuario.Comercio, dom, ref productoEdicion);

                            }

                        }
                    }
                    if(lstPuntoDeVentaStock != null && lstPuntoDeVentaStock.Count > 0)
                    {
                        foreach(KeyValuePair<BE.Domicilio,Int32> item in lstPuntoDeVentaStock)
                        {
                            if(item.Key.CodDomicilio == Convert.ToInt32(hdnPuntoDeVenta.Value))
                            {
                                chkSeleccion.Checked = true;
                                if (txtProductoCantidad != null)
                                {
                                    txtProductoCantidad.Text = Convert.ToString(item.Value);
                                }
                            }
                            
                        }
                    }


                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {
                        Literal litABMCantidad = (Literal)e.Item.FindControl("litABMCantidad");
                        object ctllitABMCantidad = (object)litABMCantidad;
                        Traducir(idiomaUsuario, ref ctllitABMCantidad);
                    }

                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al mostrar los puntos de venta: ", "Hubo un error al mostrar los puntos de venta: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError(TraducirPalabra("Hubo un error al mostrar los puntos de venta: ", "Hubo un error al mostrar los puntos de venta: ") + ex.Message);
            }
        }

        protected void rptPuntoDeVentaStock_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName != string.Empty && e.CommandArgument != null)
                //{
                //    switch (e.CommandName)
                //    {
                //        case "Seleccionar":
                //            break;
                //        default:
                //            break;
                //    }
                //}
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Hubo un error al mostrar los puntos de venta: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarMensajeError("Hubo un error al mostrar los puntos de venta: " + ex.Message);
            }
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
                    object ctllitTituloFelicidades = (object)litTituloFelicidades;
                    Traducir(idiomaUsuario, ref ctllitTituloFelicidades);
                    object ctlChkMantenerLogo = (object)ChkMantenerLogo;
                    Traducir(idiomaUsuario, ref ctlChkMantenerLogo);
                    object ctlbtnCargarImagen = (object)btnCargarImagen;
                    Traducir(idiomaUsuario, ref ctlbtnCargarImagen);
                    object ctllitABMProductoTitulo = (object)litABMProductoTitulo;
                    Traducir(idiomaUsuario, ref ctllitABMProductoTitulo);
                    object ctllitABMProductoCategoria = (object)litABMProductoCategoria;
                    Traducir(idiomaUsuario, ref ctllitABMProductoCategoria);
                    object ctllitABMProductoSubCategoria = (object)litABMProductoSubCategoria;
                    Traducir(idiomaUsuario, ref ctllitABMProductoSubCategoria);
                    object ctllitABMProductoPrecio = (object)litABMProductoPrecio;
                    Traducir(idiomaUsuario, ref ctllitABMProductoPrecio);
                    object ctllitABMDescuento = (object)litABMDescuento;
                    Traducir(idiomaUsuario, ref ctllitABMDescuento);
                    object ctllitProductoDescripcion = (object)litProductoDescripcion;
                    Traducir(idiomaUsuario, ref ctllitProductoDescripcion);
                    object ctllitLimpiarDescripcion = (object)litLimpiarDescripcion;
                    Traducir(idiomaUsuario, ref ctllitLimpiarDescripcion);
                    object ctllitABMFechaVigenciaDesde = (object)litABMFechaVigenciaDesde;
                    Traducir(idiomaUsuario, ref ctllitABMFechaVigenciaDesde);
                    object ctllitABMFechaVigenciaHasta = (object)litABMFechaVigenciaHasta;
                    Traducir(idiomaUsuario, ref ctllitABMFechaVigenciaHasta);
                    object ctllitProductoPuntosDeVenta = (object)litProductoPuntosDeVenta;
                    Traducir(idiomaUsuario, ref ctllitProductoPuntosDeVenta);
                    object ctlbtnCrearProducto = (object)btnCrearProducto;
                    Traducir(idiomaUsuario, ref ctlbtnCrearProducto);
                    object ctlbtnActualizarProducto = (object)btnActualizarProducto;
                    Traducir(idiomaUsuario, ref ctlbtnActualizarProducto);
                    object ctlbtnBorrarProducto = (object)btnBorrarProducto;
                    Traducir(idiomaUsuario, ref ctlbtnBorrarProducto);
                    object ctlbtnProductoNuevo = (object)btnProductoNuevo;
                    Traducir(idiomaUsuario, ref ctlbtnProductoNuevo);
                    object clbtnCargarImagen = (object)btnCargarImagen;
                    Traducir(idiomaUsuario, ref ctlbtnCargarImagen);
                    object ctlucProducto_litTituloProducto = (object)ucProducto_litTituloProducto;
                    Traducir(idiomaUsuario, ref ctlucProducto_litTituloProducto);


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
    }
}