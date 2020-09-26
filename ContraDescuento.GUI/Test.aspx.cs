using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BE.GrupoBase perfilAdministrador = new BE.Grupo("Usuario",true);
            //perfilAdministrador.Descripcion = "Usuario Administrador";
            //perfilAdministrador.Agregar(new BE.Permiso("DVV.aspx",false));
            //perfilAdministrador.Agregar(new BE.Permiso("Bitacora.aspx", false));
            //perfilAdministrador.Agregar(new BE.Permiso("Backup.aspx", false));
            //perfilAdministrador.Agregar(new BE.Permiso("Restore.aspx", false));
            //perfilAdministrador.Agregar(new BE.Permiso("Idioma.aspx", false));

            //BE.Grupo perfilUsuario = new BE.Grupo("Usuario",true);

            //perfilUsuario.Agregar(new BE.Permiso("Login.aspx", false));
            //perfilUsuario.Agregar(new BE.Permiso("Registrar.aspx", false));
            //perfilUsuario.Agregar(new BE.Permiso("BuscarDescuentos.aspx", false));
            //perfilUsuario.Agregar(new BE.Permiso("CanjearDescuentos.aspx", false));

            //BE.Grupo perfilComercio = new BE.Grupo("Comercio",true);

            //perfilComercio.Agregar(new BE.Permiso("Login.aspx",false));
            //perfilComercio.Agregar(new BE.Permiso("Registrar.aspx",false));
            //perfilComercio.Agregar(new BE.Permiso("ComercioAdministrar.aspx",false));

            //perfilAdministrador.Agregar(perfilUsuario);
            //perfilAdministrador.Agregar(perfilComercio);

            //perfilAdministrador.RecorrerPermisos(0);
            //RecorrerPermisos((BE.Grupo)perfilAdministrador);
            /*Fin - Composite*/
        }
        public void RecorrerPermisos(BE.Grupo grupo)
        {
         
            // Recursively display child nodes
            if(grupo.EsGrupo)
            foreach (BE.GrupoBase component in grupo.LstGrupos)
            {
                    if(component.EsGrupo)
                this.RecorrerPermisos((BE.Grupo)component);
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetVentasPorProducto()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return "Producto 1;Producto2";
        }
    }
}