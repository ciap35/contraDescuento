using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class Perfil : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Perfil PerfilNeg = new BLL.Perfil();
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        private BE.Perfil perfilEditar = new BE.Perfil();
        private BE.Permiso permisoEditar = new BE.Permiso();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            gvPerfil.DataSource = null;
            divMensajeOK.Visible = false;
            try
            {
                if (!IsPostBack)
                    if (ValidarPerfilAdmin())
                    {
                        PerfilListar();
                    }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
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

        private bool ValidarPerfilAdmin()
        {
            try
            {
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario == null || usuario.Perfil.CodPerfil != 1)
                {
                    MostrarError("Perfil inválido para la operación que intenta llevar a cabo.");
                    return false; //No es administrador
                }
                PanelPerfil.Visible = true;
                return true;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los datos de bitacora: " + ex.Message);
            }
            return false;
        }

        private void PerfilListar()
        {
            try
            {
                if (PerfilNeg == null)
                    PerfilNeg = new BLL.Perfil();
                List<BE.Perfil> lstPerfil = PerfilNeg.Listar();
                if (lstPerfil.Count > 0)
                {

                    gvPerfil.DataSource = lstPerfil;
                    gvPerfil.DataBind();
                    BindDropDownListPerfiles(null);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        private void BindTreeView(List<BE.Perfil> lstPerfil)
        {
            tvPerfiles.DataSource = null;
            TreeNode tNode = null;
            BLL.Permiso permisoNeg = new BLL.Permiso();
            List<BE.Permiso> lstPermisos = null;
            try
            {
                int index = 0;
                foreach (BE.Perfil nodePerfil in lstPerfil)
                {

                    tvPerfiles.Nodes.Add(new TreeNode { Text = nodePerfil.Descripcion, Value = nodePerfil.CodPerfil.ToString() });
                    lstPermisos = new List<BE.Permiso>();
                    lstPermisos = permisoNeg.Listar(nodePerfil);

                    foreach (BE.Permiso nodePermiso in lstPermisos)
                    {
                        tvPerfiles.Nodes[index].ChildNodes.Add(new TreeNode { Text = nodePermiso.Descripcion, Value = nodePermiso.CodPermiso.ToString() });
                    }
                    index += 1;
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        protected void gvPerfil_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPerfil.PageIndex = e.NewPageIndex;
                PerfilListar();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al cambiar de página: " + ex.Message);

            }
        }

        protected void gvPerfil_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divTreeView.Visible = false;
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {
                    BE.Perfil perfil = null;
                    switch (e.CommandName)
                    {
                        case "VerPermisos":
                            if (e.CommandArgument != null)
                            {
                                btnAgregarPermiso.Visible = true;
                                divPermisos.Visible = true;
                                perfil = new BE.Perfil();
                                perfil.CodPerfil = Convert.ToInt32(e.CommandArgument.ToString());
                                PermisoListar(perfil);
                                hdnCodPerfilPermiso.Value = perfil.CodPerfil.ToString();
                            }

                            break;
                        case "EditarPerfil":
                            gvPermisos.DataSource = null;
                            gvPermisos.DataBind();
                            
                            

                            if (e.CommandArgument != null)
                            {
                                perfilEditar.CodPerfil = Convert.ToInt32(e.CommandArgument.ToString());
                                System.Web.UI.WebControls.HiddenField hdnDescripcion = (System.Web.UI.WebControls.HiddenField)(((System.Web.UI.Control)(e.CommandSource)).NamingContainer).FindControl("hdnDescripcion");
                                System.Web.UI.WebControls.HiddenField hdnPerfilPadre = (System.Web.UI.WebControls.HiddenField)(((System.Web.UI.Control)(e.CommandSource)).NamingContainer).FindControl("hdnPerfilPadre");

                                perfilEditar.Descripcion = hdnDescripcion.Value;
                                Session["perfilEditar"] = perfilEditar;
                                hdnCodPerfilPermiso.Value = perfilEditar.CodPerfil.ToString();
                                PerfilEditar(perfilEditar);
                                BindDropDownListPerfiles(perfilEditar);
                                if (hdnPerfilPadre != null)
                                {
                                    perfilEditar._Perfil = new BE.Perfil();
                                    if (hdnPerfilPadre.Value != string.Empty)
                                    {
                                        perfilEditar._Perfil.CodPerfil = Convert.ToInt32(hdnPerfilPadre.Value);
                                        ddlPerfiles.SelectedValue = perfilEditar._Perfil.CodPerfil.ToString();
                                    }
                                }

                                if (e.CommandArgument != null)
                                {
                                    perfil = new BE.Perfil();
                                    perfil.CodPerfil = Convert.ToInt32(e.CommandArgument.ToString());
                                    PermisoListar(perfil);
                                }
                            }

                            break;
                        case "BorrarPerfil":
                            if (e.CommandArgument != null)
                            {
                                perfilEditar.CodPerfil = Convert.ToInt32(e.CommandArgument.ToString());
                                Session["perfilEditar"] = perfilEditar;
                                PerfilBaja(perfilEditar);
                                PermisoListar(null);
                                divPermisos.Visible = false;
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
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        private void PermisoListar(BE.Perfil perfil)
        {
            try
            {
                PerfilListar();
                BLL.Permiso permisoNeg = new BLL.Permiso();
                List<BE.Permiso> lstPermisos = null;
                if (perfil != null)
                    lstPermisos = permisoNeg.Listar(perfil);
                gvPermisos.DataSource = null;
                if (lstPermisos != null && lstPermisos.Count > 0)
                {
                    gvPermisos.DataSource = lstPermisos;
                }
                gvPermisos.DataBind();
                //divPermisos.Visible = lstPermisos.Count > 0;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error, por favor reintente más tarde: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }
        private void PerfilEditar(BE.Perfil perfil)
        {
            try
            {
                frmPerfil.Visible = true;

                txtPerfil.Visible = true;
                btnGuardarPerfil.Visible = true;

                txtPerfilNuevo.Visible = false;
                btnGuardarPerfilNuevo.Visible = false;

                txtPerfil.Text = perfilEditar.Descripcion;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }
        private void PerfilBaja(BE.Perfil perfil)
        {
            try
            {
                BLL.Perfil perfilNeg = new BLL.Perfil();
                PerfilNeg.Baja(perfil);
                PerfilListar();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }
        protected void btnVerTreeView_Click(object sender, EventArgs e)
        {

            divTreeView.Visible = true;
            try
            {
                BLL.Perfil perfilNeg = new BLL.Perfil();
                BindTreeView(perfilNeg.Listar());
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la jerarquía de perfiles y permisos");
            }
        }

        protected void btnOcultarTreeView_Click(object sender, EventArgs e)
        {
            divTreeView.Visible = false;
        }

        protected void btnAgregarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                divMensajeOK.Visible = false;

                frmPerfil.Visible = true;

                txtPerfilNuevo.Visible = true;
                btnGuardarPerfilNuevo.Visible = true;

                txtPerfil.Visible = false;
                btnGuardarPerfil.Visible = false;
                BindDropDownListPerfiles(null);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        protected void btnAgregarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                frmPermiso.Visible = true;
                btnAgregarPermiso.Visible = true;
                divPermisos.Visible = true;
                frmPermisoNUevo.Visible = true;
                frmPermisoEditar.Visible = false;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        protected void btnGuardarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Perfil perfilNeg = new BLL.Perfil();
                perfilEditar = (BE.Perfil)Session["perfilEditar"];
                if (txtPerfil.Text != string.Empty)
                {
                    perfilEditar.Descripcion = txtPerfil.Text;
                    if (ddlPerfiles.SelectedValue != null)
                    {
                        perfilEditar._Perfil = new BE.Perfil();
                        perfilEditar._Perfil.CodPerfil = Convert.ToInt32(ddlPerfiles.SelectedItem.Value);
                    }
                    PerfilNeg.Modificar(perfilEditar);
                    VaciarFormPerfil();
                    PerfilListar();
                    PerfilOcultarAmbosFormularios();
                }
                else
                {
                    MostrarError("Error al guardar nuevo perfil");
                }
                PerfilListar();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        protected void btnNuevoPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Permiso permisoNeg = new BLL.Permiso();
                BE.Perfil perfil = new BE.Perfil();
                perfil.CodPerfil = Convert.ToInt32(hdnCodPerfilPermiso.Value);
                BE.Permiso permiso = new BE.Permiso();
                if (txtPermisoNuevo.Text != string.Empty)
                {
                    permiso.Descripcion = txtPermisoNuevo.Text;
                    permiso.URL = txtURLNueva.Text;
                    perfil.Permisos.Add(permiso);
                    permisoNeg.Alta(perfil);
                    VaciarFormPermiso();
                    PermisoListar(perfil);
                }
                else
                {
                    MostrarError("Por favor ingrese la descripción del perfil");
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar los perfiles y permisos: " + ex.Message);
                PanelPerfil.Visible = false;
            }
        }

        private void VaciarFormPerfil()
        {
            txtPerfil.Text = string.Empty;
        }

        private void VaciarFormPermiso()
        {
            txtPermisoEditar.Text = string.Empty;
            txtURL.Text = string.Empty;
            txtPermisoNuevo.Text = string.Empty;
            txtURLNueva.Text = string.Empty;
        }

        private void BindDropDownListPerfiles(BE.Perfil perfilExcluyente)
        {
            try
            {
                ddlPerfiles.DataSource = null;

                BLL.Perfil perfilNeg = new BLL.Perfil();
                List<BE.Perfil> lstPerfil = new List<BE.Perfil>();
                lstPerfil.Add(new BE.Perfil { CodPerfil = 0, Descripcion = "-NULL-" });
                if (perfilExcluyente == null)
                    lstPerfil.AddRange(perfilNeg.Listar());
                else
                {
                    foreach (BE.Perfil perfil in PerfilNeg.Listar())
                    {
                        if (perfil.CodPerfil != perfilExcluyente.CodPerfil)
                            lstPerfil.Add(perfil);
                    }
                }


                ddlPerfiles.DataSource = lstPerfil;
                ddlPerfiles.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Error al recuperar los perfiles: " + ex.Message);
            }
        }

        protected void gvPerfil_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItem != null)
                {
                    HiddenField hdnDescripcion = (HiddenField)e.Row.FindControl("hdnDescripcion");
                    HiddenField hdnPerfilPadre = (HiddenField)e.Row.FindControl("hdnPerfilPadre");
                    if (hdnDescripcion != null)
                    {
                        hdnDescripcion.Value = ((BE.Perfil)e.Row.DataItem).Descripcion;
                    }
                    if (hdnPerfilPadre != null && ((BE.Perfil)e.Row.DataItem)._Perfil != null)
                    {
                        hdnPerfilPadre.Value = ((BE.Perfil)e.Row.DataItem)._Perfil.CodPerfil.ToString();
                    }
                    Button btnVerPermisos = (Button)e.Row.FindControl("btnVerPermisos");
                    if (btnVerPermisos != null)
                        btnVerPermisos.Visible = ((BE.Perfil)e.Row.DataItem).Permisos.Count > 0;
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Error al recuperar los perfiles" + ex.Message);
            }
        }
        protected void btnGuardarPerfilNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Perfil perfilNeg = new BLL.Perfil();
                BE.Perfil perfil = new BE.Perfil();
                if (ddlPerfiles.SelectedItem != null)
                    perfil._Perfil = new BE.Perfil();
                perfil._Perfil.CodPerfil = Convert.ToInt32(ddlPerfiles.SelectedItem.Value);
                perfil.Descripcion = txtPerfilNuevo.Text;
                perfilNeg.Alta(perfil);
                MostrarMensajeOK("Se ha dado de alta éxitosamente el perfil");
                PerfilOcultarAmbosFormularios();
                PermisoOcultarAmbosFormularios();
                PerfilListar();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Error al recuperar los perfiles" + ex.Message);
            }
        }

        private void PerfilOcultarFormularioEdicion()
        {
            txtPerfilNuevo.Visible = true;
            btnGuardarPerfilNuevo.Visible = true;

            txtPerfil.Visible = false;
            btnGuardarPerfil.Visible = false;
        }

        private void PerfilOcultarFormularioNuevo()
        {
            txtPerfilNuevo.Visible = false;
            btnGuardarPerfilNuevo.Visible = false;

            txtPerfil.Visible = true;
            btnGuardarPerfil.Visible = true;
        }

        private void PerfilOcultarAmbosFormularios()
        {
            frmPerfil.Visible = false;
            txtPerfilNuevo.Visible = false;
            btnGuardarPerfilNuevo.Visible = false;

            txtPerfil.Visible = false;
            btnGuardarPerfil.Visible = false;
        }

        protected void btnPermisoGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Permiso permisoNeg = new BLL.Permiso();
                BE.Permiso permiso = new BE.Permiso();
                BE.Perfil perfil = new BE.Perfil();
                permiso.CodPermiso = Convert.ToInt32(hdnCodPermiso.Value);
                permiso = permisoNeg.Obtener(permiso);
                permiso.Descripcion = txtPermisoEditar.Text;
                permiso.URL = txtURL.Text;
                perfil.CodPerfil = Convert.ToInt32(hdnCodPerfilPermiso.Value);
                permisoNeg.Modificar(permiso, perfil);
                PermisoListar(perfil);
                VaciarFormPermiso();
                PermisoOcultarAmbosFormularios();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un errror al guardar el permiso: " + ex.Message);
            }
        }


        protected void gvPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != null && e.CommandArgument != null)
                {
                    BE.Permiso permiso = new BE.Permiso();
                    BLL.Permiso permisoNeg = new BLL.Permiso();

                    switch (e.CommandName)
                    {
                        case "Editar":

                            permiso.CodPermiso = Convert.ToInt32(e.CommandArgument);
                            hdnCodPermiso.Value = permiso.CodPermiso.ToString();
                            permiso = permisoNeg.Obtener(permiso);
                            frmPermisoEditar.Visible = true;
                            txtPermisoEditar.Text = permiso.Descripcion;
                            txtURL.Text = permiso.URL;
                            divPermisos.Visible = true;
                            frmPermisoEditar.Visible = true;
                            txtPermisoEditar.Visible = true;
                            frmPermisoNUevo.Visible = false;
                            txtURL.Visible = true;
                            break;
                        case "Borrar":
                            permiso.CodPermiso = Convert.ToInt32(e.CommandArgument);
                            permisoNeg.Baja(permiso);
                            PermisoListar(new BE.Perfil() { CodPerfil = Convert.ToInt32(hdnCodPerfilPermiso.Value) });
                            PerfilListar();
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
                MostrarError("Ocurrió un errror al editar el permiso: " + ex.Message);
            }
        }

        private void PermisoBorrar()
        {
            try
            {

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        private void PermisoEditar(BE.Permiso permiso)
        {
            try
            {

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
            }
        }

        private void PermisoOcultarAmbosFormularios()
        {
            frmPermisoEditar.Visible = false;
            frmPermisoNUevo.Visible = false;
            divPermisos.Visible = false;
        }
    }
}