using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI.UserControl
{
    /// <summary>
    /// Descripción breve de LoginHandler
    /// </summary>
    public class LoginHandler : IHttpHandler
    {
        #region Propiedades Privadas
        string Mensaje = "";
        BLL.Usuario usuarioNeg = null;
        Page Pagina = (Page)HttpContext.Current.Handler;
        BE.Usuario usuario = null;
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        #endregion


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request["metodo"] == null)
                    throw new ApplicationException("Ubicación no disponible temporalmente, intente nuevamente más tarde");

                switch (context.Request["metodo"])
                {
                    case "Login":
                        Login(context.Request.QueryString["email"], context.Request.QueryString["pwd"]); break;
                    defult: break;
                }
            }
            catch (Exception ex)
            {
                string Mensaje = javaScriptSerializer.Serialize(ex.Message);
                context.Response.ContentType = "text/html";
                context.Response.Write(Mensaje);
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void Login(string email, string pwd)
        {
            string Mensaje;
            try
            {
                string password = pwd;
                int codUsuario = 0;
                usuarioNeg = new BLL.Usuario();
                if (usuarioNeg.Login(email, ref pwd, ref codUsuario) == BLL.Enum_Status_Login.Exitoso && codUsuario > 0)
                {
                    usuario = usuarioNeg.Obtener(codUsuario);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}