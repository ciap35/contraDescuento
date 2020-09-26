using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI.Admin
{
    public partial class Restore : System.Web.UI.Page
    {

        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PanelRestore.Visible = false;

            try
            {
                rptBackups.DataSource = null;
                if (ValidarPermisos())
                {
                    PanelRestore.Visible = true;
                    MostrarBackups();
                }
                else
                {
                    PanelRestore.Visible = false;
                }

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error, por favor intente nuevamente más tarde.");
            }
        }

        private void MostrarBackups()
        {
            try
            {
                rptBackups.DataSource = ObtenerBackups();
                rptBackups.DataBind();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error, por favor intente nuevamente más tarde.");
            }
        }


        private List<FileInfo> ObtenerBackups()
        {
            List<FileInfo> lstBackups = new List<FileInfo>();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["Path.Backup"]);
                FileInfo[] Files = dir.GetFiles("*.bak");
                foreach (FileInfo file in Files)
                {
                    lstBackups.Add(file);
                }
                Session["Backups"] = lstBackups;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error, por favor intente nuevamente más tarde.");
            }
            return lstBackups.OrderByDescending(x => x.CreationTime).ToList<FileInfo>();
        }
        private void GenerarRestore(string path)
        {
            try
            {
                BLL.Restore restore = new BLL.Restore();
                if (System.IO.File.Exists(path))
                {
                    restore.RealizarRestore(path);
                    MostrarMensajeOK("Se ha realizado el backup de: " + path);
                }
                else
                    throw new Exception("El archivo no éxiste");
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error, por favor intente nuevamente más tarde: " + ex.Message);
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
                            MostrarError("No posee permisos para ingresar a esta página");
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
                else
                {
                    Response.Redirect("Logueese.aspx");
                }
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
            return valido;
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

        protected void rptBackups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.DataItem != null)
                {
                    ((Literal)e.Item.FindControl("litFechaCreacion")).Text = ((FileInfo)e.Item.DataItem).LastWriteTime.ToString();
                    ((Literal)e.Item.FindControl("litBackup")).Text = ((FileInfo)e.Item.DataItem).Name;
                    ((Button)e.Item.FindControl("btnRestore")).CommandArgument = ((FileInfo)e.Item.DataItem).FullName;

                    BE.Idioma idiomaUsuario = null;

                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                    if (idiomaUsuario != null)
                    {

                        object ctllitFechaCreacion = (object)((System.Web.UI.HtmlControls.HtmlTableCell)rptBackups.Controls[0].Controls[0].FindControl("thFechaDeCreacion"));
                        if(ctllitFechaCreacion!=null)
                        Traducir(idiomaUsuario, ref ctllitFechaCreacion);

                        object ctllitBackup = (object)((System.Web.UI.HtmlControls.HtmlTableCell)rptBackups.Controls[0].Controls[0].FindControl("thArchivo"));
                        if (ctllitBackup != null)
                            Traducir(idiomaUsuario, ref ctllitBackup);

                        object ctlbtnRestore = (object)((Button)e.Item.FindControl("btnRestore"));
                        if (ctlbtnRestore != null)
                            Traducir(idiomaUsuario, ref ctlbtnRestore);
                    }
                }

            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }




        protected void rptBackups_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "btnRestore")
                {
                    if (e.CommandArgument != null)
                    {
                        BLL.Restore restore = new BLL.Restore();
                        restore.RealizarRestore(e.CommandArgument.ToString());
                        MostrarMensajeOK("Restore realizado correctamente.");
                    }
                    else
                        throw new Exception("No se puede realizar el restore: " + "Path incompleto.");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        #region Traducir
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
                MostrarError("Ocurrió un error al traducir: " + ex.Message);
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
                        if(ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlTableCell))
                        {
                            System.Web.UI.HtmlControls.HtmlTableCell ctrlPage = (System.Web.UI.HtmlControls.HtmlTableCell)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.InnerText = traduccion.Texto;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
        }


        private string TraducirPalabra(BE.Idioma idioma,string palabra)
        {
            string traduccionResultado = string.Empty;
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
                    foreach(BE.Traduccion traduccion in lstTraduccion)
                    {
                        string traduccionTexto = traduccion.ControlID.ToString().Replace(" ", "").ToUpper();
                        string traduccionTextResultado = palabra.Replace(" ", "").ToUpper();
                        if (traduccionTexto == traduccionTextResultado)
                        {
                            return traduccion.Texto;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
            return traduccionResultado;
        }

        #endregion
        #region Exportar
        protected void imgBtnExportPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    // Le colocamos el título y el autor
                    // **Nota: Esto no será visible en el documento
                    doc.AddTitle("Restore_PDF");
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
                    iTextSharp.text.Font f = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 50.0f, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph parag = new Paragraph("CONTRA DESCUENTO", f);
                    //parag.Alignment = 40;
                    parag.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag);


                    iTextSharp.text.Font fSubtitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 50.0f, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLUE);
                    Paragraph paragSubtitulo = new Paragraph("RESTORE", fSubtitulo);
                    paragSubtitulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(paragSubtitulo);
                    doc.Add(Chunk.NEXTPAGE);


                    List<FileInfo> lstExport = (List<FileInfo>)Session["Backups"];

                    if (lstExport.Count > 0)
                    {
                        int CantCols = 2;


                        PdfPTable tblPrueba = new PdfPTable(CantCols);
                        tblPrueba.WidthPercentage = 100;

                        #region traducir texto
                        string resultado = string.Empty;
                        BE.Idioma idiomaUsuario = null;

                        if ((BE.Idioma)Session["Idioma"] != null)
                            idiomaUsuario = (BE.Idioma)Session["Idioma"];

                        if (idiomaUsuario != null)
                        {
                            resultado = TraducirPalabra(idiomaUsuario, "palabra_FechaCreacion");
                        }
                        #endregion
                        PdfPCell col0 = null;
                        if (resultado!=string.Empty)
                        col0 = new PdfPCell(new Phrase(resultado));
                        else
                        col0 = new PdfPCell(new Phrase("Fecha de creación"));

                        col0.BorderWidth = 1;
                        col0.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                        col0.BorderWidthBottom = 0.95f;
                        tblPrueba.AddCell(col0);


                        PdfPCell col1 = null;
                        if (idiomaUsuario != null)
                        {
                            resultado = TraducirPalabra(idiomaUsuario, "palabra_Archivo");
                        }
                        if (resultado != string.Empty)
                            col1 = new PdfPCell(new Phrase(resultado));
                        else
                            col1 = new PdfPCell(new Phrase("Archivo"));
                        col1.BorderWidth = 1;
                        col1.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                        col1.BorderWidthBottom = 0.95f;
                        tblPrueba.AddCell(col1);



                        foreach (FileInfo ItemExport in lstExport)
                        {
                       
                                tblPrueba.AddCell(ItemExport.CreationTime.ToString());
                                tblPrueba.AddCell(ItemExport.Name.ToString());
                        }

                        doc.Add(tblPrueba);
                        doc.Close();
                        ms.Close();
                        Response.ContentType = "pdf/application";
                        Response.AddHeader("content-disposition",
                        "attachment;filename=ContraDescuento_Restore.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al exportar a PDF: " + ex.Message);
            }
        }

        protected void imgBtnExportExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<FileInfo> lstExport = (List<FileInfo>)Session["Backups"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Restore.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                #region traducir texto
                string resultado = string.Empty;
                BE.Idioma idiomaUsuario = null;

                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];

                if (idiomaUsuario != null)
                {
                    resultado = TraducirPalabra(idiomaUsuario, "palabra_FechaCreacion");
                }
                #endregion
                if(resultado!=string.Empty)
                    Response.Output.Write(resultado); // Nombre de la columna
                else
                Response.Output.Write("Fecha de creación"); // Nombre de la columna
                Response.Output.Write("\t");

                if (idiomaUsuario != null)
                {
                    resultado = TraducirPalabra(idiomaUsuario, "palabra_Archivo");
                }

                if (resultado!=string.Empty)
                Response.Output.Write(resultado); // Nombre de la columna
                else
                    Response.Output.Write("Archivo"); // Nombre de la columna
                Response.Output.Write("\t");

                Response.Output.WriteLine();
                foreach (FileInfo finfo in lstExport)
                {
                    Response.Output.Write(finfo.CreationTime.ToString());
                    Response.Output.Write("\t");
                    Response.Output.Write(finfo.Name.ToString());
                    Response.Output.Write("\t");
                    Response.Output.WriteLine();
                }
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al exportar a excel: " + ex.Message);
            }
        }
        #endregion
    }
}