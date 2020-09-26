using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class MisDescuentos : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            divMensajeError.Visible = false;
            divMensajeOK.Visible = false;
            try
            {
                ValidarPermisos();
                if (!IsPostBack)
                {
                    VerMisDescuentos();
                    
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
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


        private void ValidarPermisos()
        {
            try
            {
                string URL = this.Context.Request.CurrentExecutionFilePath.Replace("/", "");
                if (System.Web.HttpContext.Current.Session != null && HttpContext.Current.Session["Usuario"] != null)
                {
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    if (usuario.TipoUsuario != null && usuario.TipoUsuario.Grupo != null && usuario.TipoUsuario.Grupo.LstGrupos.Count > 0)
                    {
                        permisoNeg.ValidarSolicitud(URL, usuario.TipoUsuario.Grupo);
                        if (!permisoNeg.PeticionValida && URL.ToUpper() != "INDEX.ASPX")
                        {
                            Response.ClearHeaders();
                            Response.AddHeader("REFRESH", "1;URL=Index.aspx");
                        }
                    }
                }
                else { 
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
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar la oferta: ", "Ocurrió un error al mostrar la oferta: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar la oferta: ", "Ocurrió un error al mostrar la oferta: ") + ex.Message);
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

        private void BindMisCuponesDescuentos()
        {
            try
            {
                BLL.Descuento descuentoNeg = new BLL.Descuento();
                BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                if (usuario != null)
                {
                    LvMisDescuentos.DataSource = null;
                    LvMisDescuentos.DataSource = descuentoNeg.ObtenerDescuentos(usuario);
                    LvMisDescuentos.DataBind();
                    if (LvMisDescuentos.Items.Count == 0)
                    {
                        NoposeeDescuentos.Visible = true;
                        Traducir();
                    }
                    else
                        NoposeeDescuentos.Visible = false;
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
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
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
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
                        System.Web.UI.WebControls.Literal litPrecioAnterior = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPrecioAnterior");
                        System.Web.UI.WebControls.Literal litProductoPrecio = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoPrecio");
                        System.Web.UI.WebControls.Literal litTituloProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litTituloProducto");
                        System.Web.UI.WebControls.Literal litProductoCantidad = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoCantidad");
                        System.Web.UI.WebControls.Literal litProductoDescuento = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoDescuento");
                        System.Web.UI.WebControls.Literal litProductoAhorroTotal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoAhorroTotal");
                        System.Web.UI.WebControls.Literal litProductoPrecioFinal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoPrecioFinal");
                        System.Web.UI.WebControls.Literal litPuntoDeVenta = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPuntoDeVenta");
                        System.Web.UI.WebControls.Literal litProductoFechaCupon = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoFechaCupon");
                        System.Web.UI.WebControls.Literal litProductoFechaCanje = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoFechaCanje");
                        System.Web.UI.WebControls.Literal litProductoCupon = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoCupon");

                        System.Web.UI.HtmlControls.HtmlImage imgComercio = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComercio");
                        System.Web.UI.WebControls.Literal litComercioNombre = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litComercioNombre");

                        if (descuento.Producto != null && descuento.PuntoDeVenta != null && descuento.Comercio != null)
                        {
                            if (img != null) { if (descuento.Producto != null) { img.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(descuento.Producto.Foto); } else { img.ImageUrl = @"~/Recurso/Imagen/empty.png"; } }
                            if (litProductoPrecio != null) { litProductoPrecio.Text = Convert.ToString(descuento.Producto.Precio); }
                            if (litTituloProducto != null) { litTituloProducto.Text = descuento.Producto.Titulo; }
                            if (litProductoCantidad != null) { litProductoCantidad.Text = Convert.ToString(descuento.Cantidad); }
                            if (litProductoDescuento != null) { litProductoDescuento.Text = Convert.ToString(descuento._Descuento); }
                            if (litProductoAhorroTotal != null) { litProductoAhorroTotal.Text = Convert.ToString(descuento.AhorroTotal); }
                            if (litProductoPrecioFinal != null) { litProductoPrecioFinal.Text = Convert.ToString(descuento.Producto.Precio - descuento.AhorroTotal); }
                            if (litPuntoDeVenta != null) { litPuntoDeVenta.Text = Convert.ToString(descuento.PuntoDeVenta); }
                            if (litProductoFechaCupon != null) { litProductoFechaCupon.Text = descuento.FechaCupon.ToString(); }
                            if (litProductoFechaCanje != null) { if (descuento.FechaCanje == DateTime.MinValue) { litProductoFechaCanje.Visible = false; } else { litProductoFechaCanje.Text = descuento.FechaCanje.ToString(); } }
                            if (litProductoCupon != null) { litProductoCupon.Text = descuento.Cupon; }
                            if (imgComercio != null) { if (descuento.Comercio.Logo != null) { imgComercio.Src = "data:image/png;base64," + Convert.ToBase64String(descuento.Comercio.Logo); } else { imgComercio.Src = @"~/Recurso/Imagen/empty.png"; } }
                            if (litComercioNombre != null) { if (descuento.Comercio != null) { litComercioNombre.Text = descuento.Comercio.NombreComercio; } }
                        }
                    }

                    //Traducir literales de listView
                    #region Traducción de controles
                    BE.Idioma idiomaUsuario = null;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {
                        System.Web.UI.WebControls.Literal litCantidad = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litCantidad");
                        object ctllitCantidad = (object)litCantidad;
                        Traducir(idiomaUsuario, ref ctllitCantidad);


                        System.Web.UI.WebControls.Literal litPrecioAnterior = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPrecioAnterior");
                        object ctllitPrecioAnterior = (object)litPrecioAnterior;
                        Traducir(idiomaUsuario, ref ctllitPrecioAnterior);


                        System.Web.UI.WebControls.Literal litDescuento = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litDescuento");
                        object ctllitDescuento = (object)litDescuento;
                        Traducir(idiomaUsuario, ref ctllitDescuento);

                        System.Web.UI.WebControls.Literal litAhorro = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litAhorro");
                        object ctllitAhorro = (object)litAhorro;
                        Traducir(idiomaUsuario, ref ctllitAhorro);

                        System.Web.UI.WebControls.Literal litPrecioFinal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPrecioFinal");
                        object ctllitPrecioFinal = (object)litPrecioFinal;
                        Traducir(idiomaUsuario, ref ctllitPrecioFinal);

                        System.Web.UI.WebControls.Literal litRetiraPor = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litRetiraPor");
                        object ctllitRetiraPor = (object)litRetiraPor;
                        Traducir(idiomaUsuario, ref ctllitRetiraPor);


                        System.Web.UI.WebControls.Literal litFechaCupon = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litFechaCupon");
                        object ctllitFechaCupon = (object)litFechaCupon;
                        Traducir(idiomaUsuario, ref ctllitFechaCupon);



                        System.Web.UI.WebControls.Literal litProductoFechaCanje = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litProductoFechaCanje");
                        object ctllitProductoFechaCanje = (object)litProductoFechaCanje;
                        Traducir(idiomaUsuario, ref ctllitProductoFechaCanje);
                    }
                    #endregion
                }
                //else if (e.Item.ItemType == ListViewItemType.EmptyItem)
                //{
                //    //Traducir literales de listView
                //    #region Traducción de controles
                //    BE.Idioma idiomaUsuario = null;
                //    if ((BE.Idioma)Session["Idioma"] != null)
                //        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                //    if (idiomaUsuario != null)
                //    {
                //        System.Web.UI.WebControls.Literal NoposeeDescuentos = (System.Web.UI.WebControls.Literal)e.Item.FindControl("NoposeeDescuentos");
                //        object ctlNoposeeDescuentos = (object)NoposeeDescuentos;
                //        Traducir(idiomaUsuario, ref ctlNoposeeDescuentos);
                //    }
                //    #endregion
                //}
            }
            
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los cupones de descuento: ", "Ocurrió un error al mostrar los cupones de descuento: ") + ex.Message);
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
                    object ctlNoposeeDescuentos = (object)NoposeeDescuentos;
                    Traducir(idiomaUsuario, ref ctlNoposeeDescuentos);

                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
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
                MostrarError(TraducirPalabra("Ocurrió un error al traducir: ", "Ocurrió un error al traducir: ") + ex.Message);
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
                            if(traduccion.CodTraduccion == 284)
                            {

                            }
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

                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
            }
        }
    }
}