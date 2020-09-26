using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using static iTextSharp.text.Font;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ContraDescuento.GUI
{
    public partial class Grupos : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        BLL.Grupo grupoNeg = new BLL.Grupo();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        BLL.TipoUsuario tipoUsuarioNeg = new BLL.TipoUsuario();

        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                divMensajeError.Visible = false;
                divMensajeOK.Visible = false;
                panelGrupos.Visible = false;
                if (ValidarPermisos())
                {
                    panelGrupos.Visible = true;
                    if (!IsPostBack)
                    {
                        CargarTreeView();
                        GrupoBind();
                        PermisoBind();
                        CargarGruposAsignados();
                        BindUsuarios();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al cargar grupos y permisos: " + ex.Message);
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
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: " )+ ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
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

                    MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
                    //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
                }
            }
            return traduccionResultado;
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


        #region Grupos
        protected void btnGrupoNuevo_Click(object sender, EventArgs e)
        {
            MostrarFrmGrupoNuevo(true);
            btnEditarGrupo.Visible = true;
        }



        private void MostrarFrmGrupoNuevo(bool mostrar)
        {
            frmGrupoNuevoEditar.Visible = mostrar;
            txtGrupoNuevoEditarNombre.Text = string.Empty;

        }

        private void MostrarFrmGrupoEditar(bool mostrar, BE.Grupo grupo)
        {
            frmGrupoNuevoEditar.Visible = mostrar;
            if (mostrar && grupo != null)
            {
                txtGrupoNuevoEditarNombre.Text = grupo.Descripcion;
            }
            if (!mostrar)
                txtGrupoNuevoEditarNombre.Text = string.Empty;

            btnEditarGrupo.Visible = mostrar;
        }

        private void MostrarFrmPermisosPorGrupo(bool mostrar)
        {
            frmPermisosPorGrupo.Visible = mostrar;

        }

        private void MostrarFrmGrupoAsignar(bool mostrar)
        {
            frmGrupoAsignar.Visible = mostrar;
        }

        private void GrupoBind()
        {
            try
            {
                List<BE.Grupo> lstGrupos = grupoNeg.Listar();
                gvGrupos.DataSource = null;
                Session["Grupos"] = lstGrupos;
                gvGrupos.DataSource = lstGrupos;
                gvGrupos.DataBind();
                if (lstGrupos.Count == 0)
                    MostrarFrmGrupoNuevo(true);
                frmGrupoNuevoEditar.Visible = false;
                btnEditarGrupo.Visible = false;
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al listar los grupos: ", "Ocurrió un error al listar los grupos: ") + ex.Message);
            }
        }

        private void BindGrupos(BE.Grupo grupo)
        {
            try
            {

                List<BE.Grupo> lstGrupo = new List<BE.Grupo>();
                foreach (BE.Grupo grupoItem in grupoNeg.Listar())
                {
                    if (grupoItem.CodGrupo != grupo.CodGrupo)
                    {
                        lstGrupo.Add(grupoItem);
                    }
                }

                chkGruposPadre.DataSource = null;
                chkGruposPadre.DataSource = lstGrupo;
                chkGruposPadre.DataBind();

                foreach (System.Web.UI.WebControls.ListItem chkGrupoItem in chkGruposPadre.Items)
                {
                    foreach (BE.GrupoBase grupoChk in grupo.LstGrupos)
                    {
                        BE.Grupo grupoItem = null;
                        if (grupoChk.EsGrupo)
                            grupoItem = ((BE.Grupo)grupoChk);
                        if (grupoItem != null)
                        {
                            if (grupoItem.CodGrupo.ToString() == chkGrupoItem.Value)
                                chkGrupoItem.Selected = true;
                        }
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al cargar grupos: ", "Ocurrió un error al cargar grupos: ") + ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        private BE.Grupo GrupoObtenerDeGrilla(int codGrupo)
        {
            BE.Grupo grupo = null;
            try
            {
                List<BE.Grupo> grupoLst = ((List<BE.Grupo>)Session["Grupos"]);
                grupo = (from grupoCached in grupoLst where grupoCached.CodGrupo == codGrupo select grupoCached).FirstOrDefault<BE.Grupo>();

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al listar los grupos: ", "Ocurrió un error al listar los grupos: " + ex.Message));
            }
            return grupo;
        }

        protected void gvGrupos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int codGrupo = 0;
            try
            {
                if (!string.IsNullOrEmpty(e.CommandName))
                {
                    if (e.CommandArgument != null)
                    {
                        switch (e.CommandName)
                        {
                            case "GrupoEditar":
                                codGrupo = Convert.ToInt32(e.CommandArgument);
                                if (codGrupo > 0)
                                {
                                    PermisoBind();

                                    BE.Grupo grupo = GrupoObtenerDeGrilla(codGrupo);
                                    if (grupo != null)
                                    {
                                        Session["GrupoEditar"] = grupo;

                                        MostrarFrmGrupoEditar(true, grupo);
                                        MostrarFrmPermisosPorGrupo(false);
                                        MostrarFrmGrupoAsignar(true);

                                        BindGrupos(grupo); //Muestra los grupos en el checklist para asignarlo/desasignarlo  al grupo a editar.
                                    }
                                    CargarTreeView(); //Actualizar treeview.
                                }
                                else
                                {
                                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido editar el grupo"));
                                }
                                break;
                            case "GrupoGestionPermisos":
                                codGrupo = Convert.ToInt32(e.CommandArgument);
                                if (codGrupo > 0)
                                {
                                    BE.Grupo grupo = GrupoObtenerDeGrilla(codGrupo);
                                    if (grupo != null)
                                    {
                                        Session["GrupoEditar"] = grupo;
                                        MostrarFrmGrupoEditar(true, grupo);
                                        MostrarFrmPermisosPorGrupo(true);
                                        MostrarFrmGrupoAsignar(false);
                                        BindPermisos(grupo); //Muestra los grupos en el checklist para asignarlo/desasignarlo  al grupo a editar.
                                    }
                                }
                                break;
                            case "GrupoBorrar":

                                codGrupo = Convert.ToInt32(e.CommandArgument);
                                if (codGrupo > 0)
                                {
                                    BE.Grupo grupo = GrupoObtenerDeGrilla(codGrupo);
                                    if (grupo != null)
                                        grupoNeg.Baja(grupo);

                                    GrupoBind(); //Actualizar grilla de grupos
                                    PermisoBind(); //Actualizar grilla de permisos
                                    CargarTreeView(); //Actualizar treeview
                                }
                                else
                                {
                                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido editar el grupo"));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al editar los grupos: ", "Ocurrió un error al editar los grupos: ") + ex.Message);
            }
        }

        protected void btnEditarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGrupoNuevoEditarNombre.Text == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception(TraducirPalabra("Por favor complete el nombre del grupo", "Por favor complete el nombre del grupo")));


                BE.Grupo grupoEditar = (BE.Grupo)Session["GrupoEditar"];
                if (grupoEditar != null && grupoEditar.CodGrupo > 0)
                {
                    grupoEditar.Descripcion = txtGrupoNuevoEditarNombre.Text;
                    if (grupoEditar.Descripcion != txtGrupoNuevoEditarNombre.Text)
                    {
                        grupoNeg.Modificar(grupoEditar);


                    }

                    List<BE.Grupo> lstGrupoAsignar = new List<BE.Grupo>();
                    List<BE.Grupo> lstGrupoDesasignar = new List<BE.Grupo>();
                    List<BE.Permiso> lstPermisoAsignar = new List<BE.Permiso>();
                    List<BE.Permiso> lstPermisoDesignar = new List<BE.Permiso>();

                    if (frmGrupoAsignar.Visible)
                    {
                        grupoNeg.Modificar(grupoEditar);
                    }
                    if (frmPermisosPorGrupo.Visible)
                    {
                        foreach (System.Web.UI.WebControls.ListItem Item in chkListPermisos.Items)
                        {
                            BE.Permiso permiso = null;
                            foreach (BE.GrupoBase permisoItem in grupoEditar.LstGrupos)
                            {
                                if (!permisoItem.EsGrupo)
                                {
                                    if (((BE.Permiso)permisoItem).CodPermiso.ToString() == Item.Value.ToString())
                                    {
                                        permiso = ((BE.Permiso)permisoItem);
                                    }
                                }
                            }
                            if (Item.Selected && permiso == null)
                                lstPermisoAsignar.Add(new BE.Permiso() { CodPermiso = Convert.ToInt32(Item.Value), Descripcion = Item.Text });
                            else if (!Item.Selected && permiso != null)
                                lstPermisoDesignar.Add(new BE.Permiso() { CodPermiso = Convert.ToInt32(Item.Value), Descripcion = Item.Text });
                        }
                    }
                    if (frmGrupoAsignar.Visible)
                    {
                        foreach (System.Web.UI.WebControls.ListItem Item in chkGruposPadre.Items)
                        {
                            BE.Grupo grupoHijo = null;
                            foreach (BE.GrupoBase grupoHijoItem in grupoEditar.LstGrupos)
                            {
                                if (grupoHijoItem.EsGrupo)
                                {
                                    if (((BE.Grupo)grupoHijoItem).CodGrupo.ToString() == Item.Value.ToString())
                                    {
                                        grupoHijo = (BE.Grupo)grupoHijoItem;
                                    }

                                }
                            }
                            if (Item.Selected && grupoHijo == null)
                                lstGrupoAsignar.Add(new BE.Grupo() { CodGrupo = Convert.ToInt32(Item.Value), Descripcion = Item.Text });
                            else if (!Item.Selected && grupoHijo != null)
                                lstGrupoDesasignar.Add(new BE.Grupo() { CodGrupo = Convert.ToInt32(Item.Value), Descripcion = Item.Text });
                        }
                    }


                    if (lstGrupoAsignar.Count > 0)
                    {
                        grupoNeg.AsignarGrupoHijo(grupoEditar, lstGrupoAsignar);
                    }
                    if (lstGrupoDesasignar.Count > 0)
                    {
                        grupoNeg.EliminarGrupoHijo(grupoEditar, lstGrupoDesasignar);
                    }
                    if (lstPermisoAsignar.Count > 0)
                    {
                        permisoNeg.Asignar(grupoEditar, lstPermisoAsignar);
                    }
                    if (lstPermisoDesignar.Count > 0)
                    {
                        permisoNeg.Desasignar(grupoEditar, lstPermisoDesignar);
                    }



                    MostrarMensajeOK(TraducirPalabra("Grupo modificado exitosamente", "Grupo modificado exitosamente"));


                }
                else
                {
                    BE.Grupo grupoNuevo = new BE.Grupo();
                    grupoNuevo.Descripcion = txtGrupoNuevoEditarNombre.Text;
                    grupoNeg.Alta(ref grupoNuevo);
                    MostrarMensajeOK(TraducirPalabra("Grupo creado exitosamente", "Grupo creado exitosamente"));
                }
                GrupoBind();
                CargarTreeView();
                CargarGruposAsignados();
                Session["GrupoEditar"] = null;
                MostrarFrmGrupoNuevo(false);
                MostrarFrmGrupoEditar(false, grupoEditar);
                MostrarFrmPermisosPorGrupo(false);
                MostrarFrmGrupoAsignar(false);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al editar los grupos: ", "Ocurrió un error al editar los grupos: ") + ex.Message);
            }
        }

        protected void gvGrupos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGrupos.PageIndex = e.NewPageIndex;
                GrupoBind();
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al paginar grupos: ", "Ocurrió un error al paginar grupos: ") + ex.Message);
            }
        }

        #endregion

        #region Permisos
        private void MostrarFrmPermisoNuevo(bool mostrar)
        {
            frmPermisoNuevo.Visible = mostrar;
            frmPermisoEditar.Visible = !mostrar;
            txtPermisoNuevoNombre.Text = string.Empty;
            txtPermisoNuevaPagina.Text = string.Empty;

        }

        private void MostrarFrmPermisoEditar(bool mostrar, BE.Permiso permiso)
        {
            frmPermisoNuevo.Visible = !mostrar;
            frmPermisoEditar.Visible = mostrar;
            if (permiso != null)
            {
                txtPermisoEditarNombre.Text = permiso.Descripcion;
                txtPermisoEditarPagina.Text = permiso.URL;
            }
        }

        private void PermisoBind()
        {
            try
            {
                List<BE.Permiso> lstPermisos = permisoNeg.Listar();
                gvPermisos.DataSource = null;
                Session["Permisos"] = lstPermisos;
                gvPermisos.DataSource = lstPermisos;
                gvPermisos.DataBind();
                if (lstPermisos.Count == 0)
                    MostrarFrmPermisoNuevo(true);
                frmPermisoEditar.Visible = false;
                frmPermisoNuevo.Visible = false;
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al listar los grupos: ", "Ocurrió un error al listar los grupos: ") + ex.Message);
            }
        }

        private void BindPermisos(BE.Grupo grupo)
        {
            try
            {
                chkListPermisos.DataSource = null;
                chkListPermisos.DataSource = permisoNeg.Listar();
                chkListPermisos.DataBind();

                if (grupo != null)
                {
                    foreach (BE.GrupoBase permisoItem in grupo.LstGrupos)
                    {
                        BE.Permiso permiso = null;
                        if (!permisoItem.EsGrupo)
                        {
                            permiso = (BE.Permiso)permisoItem;
                            foreach (System.Web.UI.WebControls.ListItem lstITem in chkListPermisos.Items)
                            {
                                if (lstITem.Value.ToString() == permiso.CodPermiso.ToString())
                                {
                                    lstITem.Selected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al cargar grupos: " + ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }
        protected void btnGuardarNuevoPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPermisoNuevaPagina.Text == string.Empty || txtPermisoNuevoNombre.Text == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete los datos del permiso"));

                BE.Permiso permiso = new BE.Permiso(txtPermisoNuevoNombre.Text, txtPermisoNuevaPagina.Text, true);
                permisoNeg.Alta(ref permiso);
                Session["Permisos"] = permiso;
                MostrarFrmPermisoNuevo(false);
                GrupoBind();
                MostrarFrmGrupoEditar(false, null);
                MostrarFrmPermisosPorGrupo(false);
                MostrarFrmGrupoNuevo(false);

                PermisoBind();
                CargarTreeView();

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los datos de Grupos: " + ex.Message);
            }
        }

        protected void btnEditarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPermisoEditarNombre.Text == string.Empty || txtPermisoEditarPagina.Text == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete los datos del permiso"));

                BE.Permiso permisoEditar = (BE.Permiso)Session["PermisoEditar"];
                permisoEditar.Descripcion = txtPermisoEditarNombre.Text;
                permisoEditar.URL = txtPermisoEditarPagina.Text;
                permisoNeg.Modificar(permisoEditar);
                PermisoBind();
                CargarTreeView();
                //GrupoPermisoBind();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al editar los grupos: " + ex.Message);
            }
        }

        private BE.Permiso PermisoObtenerDeGrilla(int codPermiso)
        {
            BE.Permiso permiso = null;
            try
            {
                List<BE.Permiso> permisoLst = ((List<BE.Permiso>)Session["Permisos"]);
                permiso = (from permisoCached in permisoLst where permisoCached.CodPermiso == codPermiso select permisoCached).FirstOrDefault<BE.Permiso>();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al listar los grupos: ", "Ocurrió un error al listar los grupos: ") + ex.Message);
            }
            return permiso;
        }

        protected void gvPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int codPermiso = 0;
            try
            {
                if (!string.IsNullOrEmpty(e.CommandName))
                {
                    if (e.CommandArgument != null)
                    {
                        switch (e.CommandName)
                        {
                            case "PermisoEditar":
                                codPermiso = Convert.ToInt32(e.CommandArgument);
                                if (codPermiso > 0)
                                {
                                    BE.Permiso permiso = PermisoObtenerDeGrilla(codPermiso);
                                    Session["PermisoEditar"] = permiso;
                                    MostrarFrmPermisoEditar(true, permiso);


                                    GrupoBind(); //Actualizar grilla de grupos
                                    CargarTreeView(); //Actualizar treeview
                                }
                                else
                                {
                                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido editar el permiso"));
                                }
                                break;
                            case "PermisoBorrar":
                                codPermiso = Convert.ToInt32(e.CommandArgument);
                                if (codPermiso > 0)
                                {
                                    BE.Permiso permiso = PermisoObtenerDeGrilla(codPermiso);
                                    permisoNeg.Baja(permiso);

                                    Session["PermisoEditar"] = null;
                                    GrupoBind(); //Actualizar grilla de grupos
                                    PermisoBind(); //Actualizar grilla de permisos
                                    CargarTreeView(); //Actualizar treeview

                                }
                                else
                                {
                                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido editar el permiso"));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al editar los grupos: " + ex.Message);
            }
        }


        protected void btnPermisoNuevo_Click(object sender, EventArgs e)
        {
            MostrarFrmPermisoNuevo(true);
        }

        protected void gvPermisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPermisos.PageIndex = e.NewPageIndex;
                PermisoBind();
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al paginar permisos: " + ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }


        #endregion

        #region TreeView
        /*TreeView*/
        public void CargarTreeView()
        {
            try
            {
                tvGrupoPermiso.Nodes.Clear();
                List<BE.Grupo> lstPermiso = grupoNeg.Listar();
                foreach (BE.GrupoBase gb in lstPermiso)
                {
                    TreeNode tn = null;
                    tn = new TreeNode();

                    if (gb.EsGrupo)
                    {

                        BE.Grupo grupoNodo = (BE.Grupo)gb;
                        tn.Text = gb.Descripcion + " <span class='badge badge-primary'>grupo</span>";
                        tn.Value = grupoNodo.CodGrupo.ToString();
                    }
                    else
                    {
                        BE.Permiso permisoNodo = (BE.Permiso)gb;
                        tn.Text = permisoNodo.Descripcion + " - " + permisoNodo.URL + " <span class='badge badge-info'>permiso</span>";
                        tn.Value = permisoNodo.CodPermiso.ToString();
                    }
                    tvGrupoPermiso.Nodes.Add(tn);
                    crearNodo(tn, null, gb);

                }
                tvGrupoPermiso.CollapseAll();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los datos de Grupos: " + ex.Message);
            }
        }

        public void crearNodo(TreeNode trNode, TreeNode trNodeChild, BE.GrupoBase grupoBaseNodo)
        {
            TreeNode tn = null;
            try
            {

                List<BE.GrupoBase> lstGruposHijos = new List<BE.GrupoBase>();
                lstGruposHijos = grupoNeg.Obtener((BE.Grupo)grupoBaseNodo);
                ((BE.Grupo)grupoBaseNodo).LstGrupos = lstGruposHijos;
                foreach (BE.GrupoBase _grupo in ((BE.Grupo)grupoBaseNodo).LstGrupos)
                {
                    tn = new TreeNode();

                    if (grupoBaseNodo.EsGrupo)
                    {
                        tn.Text = ((BE.Grupo)_grupo).Descripcion + " <span class='badge badge-primary'>grupo</span>";
                        tn.Value = ((BE.Grupo)_grupo).CodGrupo.ToString();
                    }
                    else
                    {
                        tn.Value = ((BE.Permiso)_grupo).CodPermiso.ToString();
                        tn.Text = ((BE.Permiso)_grupo).Descripcion + " - " + ((BE.Permiso)_grupo).URL + " <span class='badge badge-info'>permiso</span>";
                    }
                    crearNodo(trNode, tn, _grupo);
                }

                List<BE.GrupoBase> lstHijos = permisoNeg.Listar(((BE.Grupo)grupoBaseNodo));

                foreach (BE.GrupoBase grupoHijo in lstHijos)
                {
                    if (grupoHijo.EsGrupo)
                    {
                        BE.Grupo _grupoHijo = (BE.Grupo)grupoHijo;
                        tn = new TreeNode();
                        tn.Text = _grupoHijo.Descripcion + " <span class='badge badge-primary'>grupo</span>";
                        tn.Value = _grupoHijo.CodGrupo.ToString();
                        if (trNodeChild != null)
                            trNodeChild.ChildNodes.Add(tn);
                        else
                            trNode.ChildNodes.Add(tn);

                        foreach (BE.GrupoBase PermisoHijo in _grupoHijo.LstGrupos)
                        {
                            if (!PermisoHijo.EsGrupo)
                            {
                                TreeNode tnp = new TreeNode();
                                tnp.Text = ((BE.Permiso)PermisoHijo).Descripcion + " - " + ((BE.Permiso)PermisoHijo).URL + " <span class='badge badge-info'>permiso</span>";
                                tnp.Value = ((BE.Permiso)PermisoHijo).CodPermiso.ToString();

                                if (tn != null)
                                    tn.ChildNodes.Add(tnp);

                            }
                            else
                            {
                                TreeNode tnG = new TreeNode();
                                tnG.Text = ((BE.Grupo)PermisoHijo).Descripcion + " <span class='badge badge-primary'>grupo</span>";
                                tnG.Value = ((BE.Grupo)PermisoHijo).CodGrupo.ToString();
                                tn.ChildNodes.Add(tnG);
                                crearNodo(tn, tnG, PermisoHijo);

                            }
                        }

                    }
                    else
                    {
                        BE.Permiso _permisoHijo = (BE.Permiso)grupoHijo;
                        TreeNode tnPermiso = new TreeNode();
                        tnPermiso.Text = _permisoHijo.Descripcion + " - " + _permisoHijo.URL + " <span class='badge badge-info'>permiso</span>";
                        tnPermiso.Value = _permisoHijo.CodPermiso.ToString();
                        if (trNodeChild != null)
                            trNodeChild.ChildNodes.Add(tnPermiso);
                        else
                            trNode.ChildNodes.Add(tnPermiso);
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la jerarquía de permisos: " + ex.Message);
            }
        }

        protected void btnCollapseTreeViewGrupoPermiso_Click(object sender, EventArgs e)
        {
            tvGrupoPermiso.CollapseAll();
            btnExpandTreeViewGrupoPermiso.Visible = true;
            btnCollapseTreeViewGrupoPermiso.Visible = false;
        }

        protected void btnExpandTreeViewGrupoPermiso_Click(object sender, EventArgs e)
        {
            tvGrupoPermiso.ExpandAll();
            btnCollapseTreeViewGrupoPermiso.Visible = true;
            btnExpandTreeViewGrupoPermiso.Visible = false;
        }
        #endregion

        #region Mapeo Grupos - Tipo usuarios
        private void CargarGruposAsignados()
        {
            try
            {
                ddlGrupoUsuarios.DataSource = null;
                ddlGrupoComercio.DataSource = null;
                ddlGrupoOtros.DataSource = null;

                ddlGrupoUsuarios.DataSource = grupoNeg.Listar();
                ddlGrupoComercio.DataSource = grupoNeg.Listar();
                ddlGrupoOtros.DataSource = grupoNeg.Listar();

                BE.TipoUsuario usuarioConfiguracion = new BE.TipoUsuario(BE.EnumTipoUsuario.Usuario);
                BE.TipoUsuario comercioConfiguracion = new BE.TipoUsuario(BE.EnumTipoUsuario.Comercio);
                BE.TipoUsuario otrosConfiguracion = new BE.TipoUsuario(BE.EnumTipoUsuario.Otros);

                usuarioConfiguracion = tipoUsuarioNeg.Obtener(usuarioConfiguracion);
                comercioConfiguracion = tipoUsuarioNeg.Obtener(comercioConfiguracion);
                otrosConfiguracion = tipoUsuarioNeg.Obtener(otrosConfiguracion);

                ddlGrupoUsuarios.DataBind();
                ddlGrupoComercio.DataBind();
                ddlGrupoOtros.DataBind();

                if (usuarioConfiguracion.Grupo != null && usuarioConfiguracion.Grupo.CodGrupo > 0)
                {
                    ddlGrupoUsuarios.SelectedValue = usuarioConfiguracion.Grupo.CodGrupo.ToString();
                }
                if (comercioConfiguracion.Grupo != null && comercioConfiguracion.Grupo.CodGrupo > 0)
                {
                    ddlGrupoComercio.SelectedValue = comercioConfiguracion.Grupo.CodGrupo.ToString();
                }

                if (otrosConfiguracion.Grupo != null && otrosConfiguracion.Grupo.CodGrupo > 0)
                {
                    ddlGrupoOtros.SelectedValue = otrosConfiguracion.Grupo.CodGrupo.ToString();
                }

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                MostrarError(ex.Message);
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);

            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al listar los grupos: ", "Ocurrió un error al listar los grupos: ") + ex.Message);
            }
        }


        protected void btnGuardarMapeoUsuarios_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Grupo grupoUSuario = new BE.Grupo();
                BE.Grupo grupoComercio = new BE.Grupo();
                BE.Grupo grupoOtros = new BE.Grupo();

                if (ddlGrupoUsuarios.SelectedValue == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo USUARIO"));
                if (ddlGrupoComercio.SelectedValue == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo COMERCIO"));
                if (ddlGrupoOtros.SelectedValue == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un grupo para el tipo OTROS"));

                grupoUSuario.CodGrupo = Convert.ToInt32(ddlGrupoUsuarios.SelectedValue);
                grupoComercio.CodGrupo = Convert.ToInt32(ddlGrupoComercio.SelectedValue);
                grupoOtros.CodGrupo = Convert.ToInt32(ddlGrupoOtros.SelectedValue);

                tipoUsuarioNeg.ActualizarMapeo(grupoUSuario, grupoComercio, grupoOtros);
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al guardar la configuración de los grupos: ", "Ocurrió un error al guardar la configuración de los grupos: ") + ex.Message);
            }
        }

        #endregion


        #region Usuarios

        private void BindUsuarios()
        {
            try
            {
                BLL.Usuario usuarioNeg = new BLL.Usuario();
                gvUsuarios.DataSource = null;
                gvUsuarios.DataSource = usuarioNeg.Listar(true);
                gvUsuarios.DataBind();

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los usuarios: ", "Ocurrió un error al mostrar los usuarios: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los usuarios: ", "Ocurrió un error al mostrar los usuarios: ") + ex.Message);
            }
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUsuarios.PageIndex = e.NewPageIndex;
                BindUsuarios();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al cambiar de página  al mostrar los usuarios: ", "Ocurrió un error al cambiar de página al mostrar los usuarios: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al cambiar de página al mostrar los usuarios: ", "Ocurrió un error al cambiar de página al mostrar los usuarios: ") + ex.Message);
            }
        }
        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItem != null && e.Row.RowType == DataControlRowType.DataRow)
                {
                    BE.Usuario usuario = (BE.Usuario)e.Row.DataItem;
                    if (usuario != null && usuario.TipoUsuario != null)
                    {
                        System.Web.UI.WebControls.DropDownList ddlTipoUsuarioAntiguo = (System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlTipoUsuarioAntiguo");
                        System.Web.UI.WebControls.DropDownList ddlTipoUsuarioActual = (System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlTipoUsuarioActual");
                        if (ddlTipoUsuarioAntiguo != null)
                        {
                            ddlTipoUsuarioAntiguo.DataSource = null;
                            ddlTipoUsuarioAntiguo.DataSource = tipoUsuarioNeg.Listar();
                            ddlTipoUsuarioAntiguo.DataBind();

                            ddlTipoUsuarioActual.DataSource = null;
                            ddlTipoUsuarioActual.DataSource = tipoUsuarioNeg.Listar();
                            ddlTipoUsuarioActual.DataBind();

                            List<System.Web.UI.WebControls.ListItem> lstItemsABorrar = new List<System.Web.UI.WebControls.ListItem>();
                            if (ddlTipoUsuarioAntiguo.Items.Count > 0)
                            {
                                ddlTipoUsuarioAntiguo.SelectedValue = Convert.ToString(usuario.TipoUsuario.CodTipoUsuario);
                                ddlTipoUsuarioAntiguo.Enabled = false;
                                string codTipoUsuarioActual = Convert.ToString(usuario.TipoUsuario.CodTipoUsuario);
                                foreach (System.Web.UI.WebControls.ListItem tipoUsuarioItem in ddlTipoUsuarioActual.Items)
                                {

                                    if (tipoUsuarioItem.Value == codTipoUsuarioActual)
                                        lstItemsABorrar.Add(tipoUsuarioItem);
                                }
                                foreach(System.Web.UI.WebControls.ListItem itemBorrar in lstItemsABorrar)
                                {
                                    ddlTipoUsuarioActual.Items.Remove(itemBorrar);
                                }
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al mostrar los usuarios: ", "Ocurrió un error al mostrar los usuarios: ") + ex.Message);
            }
        }


        #endregion
  
    
        protected void imgBtnExportExcelGrupo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<BE.Grupo> lstGrupo = (List<BE.Grupo>)Session["Grupos"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Grupos.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                exportTool.Escribir<BE.Grupo>(lstGrupo, Response.Output);
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a excel: ", "Ocurrió un error al exportar a excel: ") + ex.Message);
            }
        }

        protected void imgBtnExportPDFGrupo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    // Le colocamos el título y el autor
                    // **Nota: Esto no será visible en el documento
                    doc.AddTitle("Grupos_PDF");
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
                    Paragraph parag2 = new Paragraph("GRUPOS", f);
                    //parag.Alignment = 40;
                    parag2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag2);

                    doc.Add(Chunk.NEXTPAGE);



                    List<BE.Grupo> lstGrupo = (List<BE.Grupo>)Session["Grupos"];
                    if (lstGrupo.Count > 0)
                    {
                        int CantCols = 0;
                        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(BE.Grupo));
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
                                        PdfPCell clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        clNombre.BorderWidth = 1;
                                        clNombre.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                                        clNombre.BorderWidthBottom = 0.95f;
                                        tblPrueba.AddCell(clNombre);
                                    }
                                }
                            }
                        }

                        foreach (BE.Grupo grupoItem in lstGrupo)
                        {
                            foreach (System.ComponentModel.PropertyDescriptor prop in props)
                            {
                                foreach (Attribute atr in prop.Attributes)
                                {
                                    if (atr.GetType() == typeof(BE.Exportable))
                                    {
                                        if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                        {
                                            tblPrueba.AddCell(prop.Converter.ConvertToString(prop.GetValue(grupoItem)));
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
                        "attachment;filename=ContraDescuento_Grupos.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a PDF: ", "Ocurrió un error al exportar a PDF: ") + ex.Message);

            }
        }

        protected void imgBtnExportExcelPermiso_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<BE.Permiso> lstPermisos = (List<BE.Permiso>)Session["Permisos"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Permisos.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                exportTool.Escribir<BE.Permiso>(lstPermisos, Response.Output);
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a excel: ", "Ocurrió un error al exportar a excel: ") + ex.Message);
            }
        }

        protected void imgBtnExportPDFPermiso_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    // Le colocamos el título y el autor
                    // **Nota: Esto no será visible en el documento
                    doc.AddTitle("Permisos_PDF");
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
                    Paragraph parag2 = new Paragraph("PERMISOS", f);
                    //parag.Alignment = 40;
                    parag2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag2);

                    doc.Add(Chunk.NEXTPAGE);



                    List<BE.Permiso> lstPermiso = (List<BE.Permiso>)Session["Permisos"];
                    if (lstPermiso.Count > 0)
                    {
                        int CantCols = 0;
                        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(BE.Permiso));
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
                                        PdfPCell clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        clNombre.BorderWidth = 1;
                                        clNombre.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                                        clNombre.BorderWidthBottom = 0.95f;
                                        tblPrueba.AddCell(clNombre);
                                    }
                                }
                            }
                        }

                        foreach (BE.Permiso permisoItem in lstPermiso)
                        {
                            foreach (System.ComponentModel.PropertyDescriptor prop in props)
                            {
                                foreach (Attribute atr in prop.Attributes)
                                {
                                    if (atr.GetType() == typeof(BE.Exportable))
                                    {
                                        if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                        {
                                            tblPrueba.AddCell(prop.Converter.ConvertToString(prop.GetValue(permisoItem)));
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
                        "attachment;filename=ContraDescuento_Permisos.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a PDF: ", "Ocurrió un error al exportar a PDF: ") + ex.Message);
            }
        }

        protected void gvPermisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    #region Traduccion columnas
                    BE.Idioma idiomaUsuario = null;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {
                        foreach (System.Web.UI.WebControls.DataControlFieldCell row in e.Row.Cells)
                        {

                            object clitColumna = (object)row;
                            if (clitColumna != null)
                                Traducir(idiomaUsuario, ref clitColumna);
                        }

                    }
                    #endregion
                }
            }
            catch(Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
            }
        }

        protected void gvGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    #region Traduccion columnas
                    BE.Idioma idiomaUsuario = null;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {
                        foreach (System.Web.UI.WebControls.DataControlFieldCell row in e.Row.Cells)
                        {

                            object clitColumna = (object)row;
                            if (clitColumna != null)
                                Traducir(idiomaUsuario, ref clitColumna);
                        }

                    }
                    #endregion
                }

            }
            catch(Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error, reintente nuevamente más tarde: ", "Ocurrió un error, reintente nuevamente más tarde: ") + ex.Message);
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
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
            }
        }
    }
}