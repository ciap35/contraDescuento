using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ContraDescuento.GUI
{
    public class Global : System.Web.HttpApplication
    {

        #region Propiedades Privadas
        private BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        private bool PeticionValida = false;
        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                //BLL.Sistema system = new BLL.Sistema();
                //if (!system.ValidarExistenciaBaseDeDatos())
                //{
                //    string path = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+ @"\BAK\ContraDescuento\BD\" + System.Configuration.ConfigurationSettings.AppSettings["BD_RestoreFile"];
                //    if (!System.IO.Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["BD_Path_Restore"]))
                //    {
                //        string pathRestore = System.Configuration.ConfigurationSettings.AppSettings["BD_Path_Restore"];
                //        string fileBak = System.Configuration.ConfigurationSettings.AppSettings["BD_RestoreFile"];

                //        System.IO.Directory.CreateDirectory(pathRestore);
                //        System.IO.File.Copy(path, pathRestore+fileBak);
                //    }

                //    system.CrearBaseDeDatos();
                //   // system.CargarParametria();
                    
                //}

             
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                bitacora.ExcepcionControlada = true;
                exceptionLogger.Grabar(bitacora);
            }
            catch (Exception ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        
        }


      

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
            }
            catch(Exception ex)
            {

            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}