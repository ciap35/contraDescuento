using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using static iTextSharp.text.Font;
using System.ComponentModel;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ContraDescuento.GUI.Admin
{
    public partial class Bitacora : System.Web.UI.Page
    {

        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            gvBitacora.DataSource = null;
            try
            {
                if (!IsPostBack)
                {
                    if (ValidarPermisos())
                    {
                        PanelBitacora.Visible = true;
                        BitacoraListar();
                    }
                    else
                    {
                        PanelBitacora.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
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
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
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
                }
            }
            return traduccionResultado;
        }



        private void BitacoraListar()
        {
            try
            {
                if (exceptionLogger == null)
                    exceptionLogger = new BLL.Bitacora();
                List<BE.Bitacora> lstBitacora = exceptionLogger.Listar();
                Session["lstBitacora"] = lstBitacora;
                if (lstBitacora.Count > 0)
                {
                    gvBitacora.DataSource = lstBitacora;
                    gvBitacora.DataBind();
                }
                else
                {
                    MostrarMensajeOK(TraducirPalabra("NO SE HAN ENCONTRADO ERRORES EN BITACORA", "NO SE HAN ENCONTRADO ERRORES EN BITACORA"));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra( "Ocurrió un error al mostrar los datos de bitacora: ", "Ocurrió un error al mostrar los datos de bitacora: ") + ex.Message);
                //MostrarError("Ocurrió un error al mostrar los datos de bitacora: " + ex.Message);
                PanelBitacora.Visible = false;
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

        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBitacora.PageIndex = e.NewPageIndex;
            BitacoraListar();
        }

        protected void imgBtnExportExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TFL.Export exportTool = new TFL.Export();
                List<BE.Bitacora> lstBitacora = (List<BE.Bitacora>)Session["lstBitacora"];
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ContraDescuento_Bitacora.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");


                #region traducir texto
                string resultado = string.Empty;
                BE.Idioma idiomaUsuario = null;

                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(BE.Bitacora));
                foreach (PropertyDescriptor prop in props)
                {
                    foreach (Attribute atr in prop.Attributes)
                    {
                        if (atr.GetType() == typeof(BE.Exportable))
                        {
                           if(((ContraDescuento.BE.Exportable)atr).Exportar)
                            {
                                if (idiomaUsuario != null)
                                {
                                    resultado = TraducirPalabra(idiomaUsuario, "palabra_" + prop.DisplayName);
                                    if (resultado != string.Empty && resultado.Replace(" ", "").ToUpper() != "LOREMIPSUM")
                                        Response.Output.Write(resultado); // Nombre de la columna
                                    else
                                        Response.Output.Write(prop.DisplayName); // Nombre de la columna
                                }
                                Response.Output.Write("\t");
                            }
                        }
                    }
                }
                Response.Output.WriteLine();

                exportTool.Escribir<BE.Bitacora>(lstBitacora, Response.Output);
                Response.End();
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a excel: ", "Ocurrió un error al exportar a excel: ") + ex.Message);
                //MostrarError("Ocurrió un error al exportar a excel: " + ex.Message);
            }
        }

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
                    doc.AddTitle("Bitacora_PDF");
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
                    Paragraph parag2 = new Paragraph("BITACORA", f);
                    //parag.Alignment = 40;
                    parag2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(parag2);

                    doc.Add(Chunk.NEXTPAGE);


                    #region traducir texto
                    string resultado = string.Empty;
                    BE.Idioma idiomaUsuario = null;

                    if ((BE.Idioma)Session["Idioma"] != null)
                        idiomaUsuario = (BE.Idioma)Session["Idioma"];

                 
                    #endregion


                    List<BE.Bitacora> lstBitacora = (List<BE.Bitacora>)Session["lstBitacora"];
                    // lstBitacora=  lstBitacora.Take(3).ToList<BE.Bitacora>();
                    if (lstBitacora.Count > 0)
                    {
                        int CantCols = 0;
                        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(BE.Bitacora));
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

                                        PdfPCell clNombre = null;

                                        if (idiomaUsuario != null)
                                        {
                                            resultado = TraducirPalabra(idiomaUsuario, "palabra_"+prop.DisplayName);
                                            if(resultado!=string.Empty && resultado.Replace(" ","").ToUpper() != "LOREMIPSUM")
                                            {
                                                clNombre = new PdfPCell(new Phrase(resultado));
                                            }
                                            else
                                                clNombre = new PdfPCell(new Phrase(prop.DisplayName));
                                        }
                                        

                                        clNombre.BorderWidth = 1;
                                        clNombre.BackgroundColor = new BaseColor(System.Drawing.Color.LightGray);
                                        clNombre.BorderWidthBottom = 0.95f;
                                        clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tblPrueba.AddCell(clNombre);
                                    }
                                }
                            }
                        }

                        foreach (BE.Bitacora bitacoraItem in lstBitacora)
                        {
                            foreach (System.ComponentModel.PropertyDescriptor prop in props)
                            {
                                foreach (Attribute atr in prop.Attributes)
                                {
                                    if (atr.GetType() == typeof(BE.Exportable))
                                    {
                                        if (((ContraDescuento.BE.Exportable)atr).Exportar)
                                        {
                                            tblPrueba.AddCell(prop.Converter.ConvertToString(prop.GetValue(bitacoraItem)));
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
                        "attachment;filename=ContraDescuento_Bitacora.pdf");
                        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al exportar a PDF: ", "Ocurrió un error al exportar a PDF: ") + ex.Message);
            }
        }

        

        protected void gvBitacora_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        foreach(System.Web.UI.WebControls.DataControlFieldCell row in e.Row.Cells)
                        {
                            
                            object clitColumna = (object)row;
                            if(clitColumna != null)
                            Traducir(idiomaUsuario, ref clitColumna);
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(TraducirPalabra("Ocurrió un error al traducir la grilla: ", "Ocurrió un error al traducir la grilla: ") + ex.Message);
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
                        if(ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.DataControlFieldHeaderCell))
                        {
                            System.Web.UI.WebControls.DataControlFieldHeaderCell ctrlPage = (System.Web.UI.WebControls.DataControlFieldHeaderCell)ControlATraducir;
                            if(ctrlPage.UniqueID!=null)
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

        private string TraducirPalabra(BE.Idioma idioma, string palabra)
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
                    foreach (BE.Traduccion traduccion in lstTraduccion)
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
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
            return traduccionResultado;
        }
        #endregion
    }
}