using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{

    public partial class Site : System.Web.UI.MasterPage
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            //Page.Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            menuAdministrador.Visible = false;
            rptIdioma.DataSource = null;
            BindDDLIdioma();
            divMensajeError.Visible = false;
            divMensajeOK.Visible = false;
            try
            {
                ConfigPantallaBienvenida();
                if (Session["Usuario"] != null)
                {
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    if (usuario.Idioma != null)
                        Traducir(usuario.Idioma);
                }
                else
                {
                    if(Session["Idioma"]!=null)
                    {
                        Traducir(((BE.Idioma)Session["Idioma"]));
                    }
                }
                ValidarPermisos();
                if (!IsPostBack)
                {
    
                }

            }
            catch(HttpException ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                divMensajeError.Visible = false;    
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un errror: " + ex.Message);
            }
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
                        if (!permisoNeg.PeticionValida)
                        {
                            if (URL.ToUpper() == "MISDESCUENTOS.ASPX")
                            {
                                DivMisdescuentos.Visible = false;
                                Response.Redirect("Index.aspx");
                            }
                            //MostrarError("No posee permisos para ingresar a esta página");
                            Response.ClearHeaders();
                            Response.AddHeader("REFRESH", "5;URL=Index.aspx");
                          


                        }

                        if (usuario.TipoUsuario != null && usuario.TipoUsuario.Comercio)
                        {
                            DivMisdescuentos.Visible = false;
                            DivRegistrarComercio.Visible = false;
                        }
                        else
                        {
                            DivMisdescuentos.Visible = true;
                            DivRegistrarComercio.Visible = true;
                        }
                    }
                    if (usuario != null && usuario.TipoUsuario != null && usuario.TipoUsuario.Comercio)
                        menuComercio.Visible = true;
                }
                //else if (URL.ToUpper() == "INDEX.ASPX" || URL.ToUpper() == "LOGIN.ASPX" ) 
                //    Response.Redirect("Logueese.aspx");

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al validar  los permisos: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al validar  los permisos: " + ex.Message);
            }
        }

        //protected void Page_LoadComplete(object sender,EventArgs e)
        //{
        //    Traducir((BE.Idioma)Session["Idioma"]);
        //}

        private void MostrarMensajeOK(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
        }

        private void ConfigPantallaBienvenida()
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null)
                {


                    btnLogin.Visible = false;
                    litbtnLogout.Visible = true;
                    btnLogout.Visible = true;
                    btnRegistrarUsuario.Visible = false;
                    litUsuarioSaludo.Visible = true;
                    lnkPerfilUsuario.InnerText = usuario.ToString().ToUpper();
                    lnkPerfilUsuario.Visible = true;
                    //menuUsuario.Visible = usuario.Perfil.CodPerfil == (int)BE.Enum_Perfil.Usuario;
                    menuAdministrador.Visible = usuario.TipoUsuario == null;
                    //menuComercio.Visible = (usuario.Perfil.CodPerfil == (int)BE.Enum_Perfil.Comercio);
                }
                else
                {

                    btnLogin.Visible = true;
                    litbtnLogout.Visible = true;
                    btnLogout.Visible = false;
                    btnRegistrarUsuario.Visible = true;
                    litUsuarioSaludo.Visible = false;
                    lnkPerfilUsuario.InnerText = string.Empty;
                    lnkPerfilUsuario.Visible = false;
                    menuUsuario.Visible = true;
                    menuAdministrador.Visible = false;
                    menuComercio.Visible = false;
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error configurar la pantalla de bienvenida: " + ex.Message);
            }

        }
        private void MostrarError(string mensaje)
        {
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = mensaje.ToUpper();
        }

        private void BindDDLIdioma()
        {
            try
            {
                BLL.Idioma idiomaNeg = new BLL.Idioma();
                List<BE.Idioma> lstIdioma = new List<BE.Idioma>();
                lstIdioma = idiomaNeg.Listar();
                foreach (BE.Idioma idioma in lstIdioma)
                {
                    if (Session["Idioma"] == null && idioma.PorDefecto)
                        Session["Idioma"] = idioma;
                }
                rptIdioma.DataSource = lstIdioma;
                rptIdioma.DataBind();

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al cargar el idioma: " + ex.Message);
            }
        }



        protected void rptIdioma_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        BE.Idioma idioma = (BE.Idioma)e.Item.DataItem;
                        LinkButton lnkBtnIdiomaItem = (LinkButton)e.Item.FindControl("lnkBtnIdiomaItem");
                        if (lnkBtnIdiomaItem != null)
                        {
                            lnkBtnIdiomaItem.Text = idioma.Descripcion;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al obtener el idioma: " + ex.Message);
            }
        }

        protected void rptIdioma_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {
                    switch (e.CommandName)
                    {
                        case "SeleccionarIdioma":
                            if (e.CommandArgument != null)
                            {
                                BE.Idioma idioma = new BE.Idioma();
                                idioma.CodIdioma = Convert.ToInt32(e.CommandArgument);


                                litDDLIdioma.Text = ((LinkButton)e.CommandSource).Text;


                                BLL.Idioma IdiomaNeg = new BLL.Idioma();
                                idioma.CodIdioma = Convert.ToInt32(e.CommandArgument);
                                idioma = IdiomaNeg.Obtener(idioma);
                                Session["Idioma"] = idioma;
                                BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                                if (usuario != null && usuario.Idioma != null)
                                {
                                    usuario.Idioma = idioma;
                                    Session["Usuario"] = usuario;
                                    BLL.Usuario usuarioNeg = new BLL.Usuario();
                                    usuarioNeg.CambiarIdioma(usuario);
                                }
                                Traducir(idioma);
                                if(this.Request.CurrentExecutionFilePath != string.Empty)
                                {
                                    Response.Redirect(this.Request.CurrentExecutionFilePath);
                                }

                                //Response.Redirect(this.Request.AppRelativeCurrentExecutionFilePath);
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
                MostrarError("Ocurrió un error al obtener el idioma: " + ex.Message);
            }
        }

        private void Traducir(BE.Idioma idioma)
        {
            try
            {
                
                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                traductor.Idioma = idioma;
                lstTraduccion.Clear();
                if(idioma.Descripcion == string.Empty)
                {
                    BLL.Idioma IdiomaNeg = new BLL.Idioma();
                    idioma = IdiomaNeg.Obtener(idioma);
                }
                litDDLIdioma.Text = idioma.Descripcion;
                foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                {
                    lstTraduccion.Add(traduc.Traduccion);
                }
                if (lstTraduccion.Count > 0)
                {
                    TraducirControles(lstTraduccion);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al obtener el idioma: " + ex.Message);
            }
        }

        private void TraducirControles(List<BE.Traduccion> traducciones)
        {
            try
            {
                foreach (BE.Traduccion traduccion in traducciones)
                {

                    //foreach(object o in this.Page.Master.Controls)
                    //{
                    //    this.Page.Master.FindControl(traduccion.ControlID);
                    //}

                    ContentPlaceHolder cphBody = (ContentPlaceHolder)Page.Form.FindControl("cphBody");
                    ContentPlaceHolder cphHeader = (ContentPlaceHolder)Page.Form.FindControl("cphHeader");
                    ContentPlaceHolder SiteMaster = (ContentPlaceHolder)Page.Form.FindControl("siteMaster");
                     
                    if (cphBody != null)
                    {
                        var ControlATraducir = cphBody.FindControl(traduccion.ControlID);
                        if (ControlATraducir != null)
                        {
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                            {
                                System.Web.UI.HtmlControls.HtmlAnchor ctrlPage = (System.Web.UI.HtmlControls.HtmlAnchor)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Literal))
                            {
                                System.Web.UI.WebControls.Literal ctrlPage = (System.Web.UI.WebControls.Literal)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                            {
                                System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Button))
                            {
                                System.Web.UI.WebControls.Button ctrlPage = (System.Web.UI.WebControls.Button)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if(traduccion.Texto.ToUpper().Replace(" ","")!= "LOREMIPSUM")
                                    ctrlPage.Text = traduccion.Texto;
                            }
                            if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                            {
                                System.Web.UI.WebControls.CheckBox ctrlPage = (System.Web.UI.WebControls.CheckBox)ControlATraducir;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.Text = traduccion.Texto;
                            }
                        }
                        else
                        {
                           
                            var control = this.Page.Master.FindControl(traduccion.ControlID);

                            //if (control == null)
                            //{
                            //    var controlInSite = this.Page.Master.FindControl(traduccion.ControlID);
                            //    if (controlInSite != null)
                            //    {
                            //        control = controlInSite;
                            //    }
                            //}

                            if (control !=null)
                            {
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
                                        ctrlPage.Text = traduccion.Texto ;
                                }
                                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                                {
                                    System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)control;
                                    if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                        ctrlPage.InnerText = traduccion.Texto ;
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
                        foreach (System.Web.UI.Control ctrl in cphBody.Controls)
                        {
                            if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                            {
                                System.Web.UI.HtmlControls.HtmlAnchor ctrlPage = (System.Web.UI.HtmlControls.HtmlAnchor)ctrl;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ctrl.GetType() == typeof(System.Web.UI.WebControls.Literal))
                            {
                                System.Web.UI.WebControls.Literal ctrlPage = (System.Web.UI.WebControls.Literal)ctrl;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.Text = traduccion.Texto ;
                            }
                            if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                            {
                                System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)ctrl;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.InnerText = traduccion.Texto;
                            }
                            if (ctrl.GetType() == typeof(System.Web.UI.WebControls.Button))
                            {
                                System.Web.UI.WebControls.Button ctrlPage = (System.Web.UI.WebControls.Button)ctrl;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.Text = traduccion.Texto;
                            }
                            if (ctrl.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                            {
                                System.Web.UI.WebControls.CheckBox ctrlPage = (System.Web.UI.WebControls.CheckBox)ctrl;
                                if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                    if (traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                                        ctrlPage.Text = traduccion.Texto;
                            }
                        }

                    }

                   

                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex,true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
        }


        
    }
}