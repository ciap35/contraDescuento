<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Idioma.aspx.cs" Inherits="ContraDescuento.GUI.Admin.Idioma" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Idioma</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

        <link href="Lib/Style/Main.css" rel="stylesheet" />
    <div class="jumbotron">
        <h1 class="h1 text-center">
            <asp:Literal ID="litIdiomaTitulo" runat="server">IDIOMA </asp:Literal>
            <asp:Button ID="btnIdiomaNuevo" CssClass="btn btn-outline-primary" runat="server" OnClick="btnIdiomaNuevo_Click" Text="AGREGAR NUEVO IDIOMA" /></h1>

        <br />
        <br />
        <br />


        <div id="container">

            <div id="divMensajeError" runat="server" visible="false" class="animated">
                <h4 class="alert-heading">¡Ups!</h4>
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>

            <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s fadeOut delay-5s">
                <div class="alert alert-success" role="alert">
                    <%--<h4>Status</h4>
                    <hr />--%>
                    <p class="mb-0">
                        <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
            </div>





            <!--Formulario edición de idioma --->
            <div id="frmIdiomaEdicion" runat="server" visible="false">

                <h1 class="h1">
                    <asp:Literal ID="litTituloIdiomaEditar" runat="server">IDIOMA EDITAR</asp:Literal>
                    <br />
                    <br />
                    <div class="form-group">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                 <span class="input-group-text w-100" id="litIdioma" runat="server" >IDIOMA</span>
                            </div>
                            <asp:TextBox ID="txtIdiomaEditar" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="20"><br /><br /></asp:TextBox><asp:Button ID="btnIdiomaEditar" Visible="false" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnIdiomaEditar_Click" />
                        </div>
                    </div>
            </div>
            <asp:HiddenField ID="hdnIdiomaEditar" runat="server" />
            <!--Formulario nuevo  idioma --->
            <div id="frmIdiomaNuevo" runat="server" visible="false">
                <div class="form-group">
                    <h1 class="h1">
                        <asp:Literal ID="litTituloIdiomaNuevo" runat="server">IDIOMA NUEVO</asp:Literal>
                        <br />
                        <br />

                        <div class="input-group mb-3" runat="server">
                            <div class="input-group-prepend">
                                <span class="input-group-text w-100" id="litNuevoIdioma" runat="server" >IDIOMA</span>
                            </div>

                            <asp:TextBox ID="txtIdiomaNuevo" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="20"></asp:TextBox><asp:Button ID="btnIdiomaCrear" Visible="false" runat="server" CssClass="btn btn-primary" Text="CREAR" OnClick="btnIdiomaCrear_Click" />
                        </div>
                </div>
            </div>

            <asp:GridView ID="gvIdioma" runat="server" AllowPaging="true" AutoGenerateColumns="False" 
                OnPageIndexChanging="gvIdioma_PageIndexChanging" class="table table-dark pagination-ys" OnRowCommand="gvIdioma_RowCommand" OnRowDataBound="gvIdioma_RowDataBound" 
                PagerSettings-Position="TopAndBottom">
                <Columns>
                    <asp:BoundField DataField="CodIdioma" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="CodIdioma" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de creación" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                <%--    <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnVerTraducciones" runat="server" CommandName="VerTraducciones" CssClass="btn btn-primary" Text="Administrar traducciones" CommandArgument='<%# Eval("CodIdioma") %>' />
                            
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVerTraducciones" runat="server" CommandName="VerTraducciones" CssClass="btn btn-primary" Text="Administrar traducciones" CommandArgument='<%# Eval("CodIdioma") %>' PostBackUrl="Idioma.aspx#divTraducciones"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnEditarIdioma" runat="server" CommandName="EditarIdioma" CssClass="btn btn-success" Text="Editar" CommandArgument='<%#Eval("CodIdioma") %>' />
                            <asp:HiddenField ID="hdnDescripcion" runat="server" Value='<%#Eval("CodIdioma") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnBorrarIdioma" runat="server" CommandName="BorrarIdioma" CssClass="btn btn-danger" Text="Borrar" CommandArgument='<%# Eval("CodIdioma") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
            </asp:GridView>
            <hr />
            <asp:ImageButton ID="imgBtnExportExcel_Idioma" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcel_Idioma_Click" />
            <asp:ImageButton ID="imgBtnExportPDF_Idioma" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDF_Idioma_Click" />
        </div>
    </div>
    <br />
    <br />
    
     <div class="jumbotron" id="divTraducciones" name="divTraducciones" runat="server" visible="false">
         <a href="#" name="divTraducciones" ></a>
    <h1 class="h1 text-center">
        <asp:Literal ID="litTituloTraduccion" runat="server">TRADUCCIONES </asp:Literal>
        <asp:Literal ID="litIdiomaSeleccionado" runat="server"></asp:Literal>
        <asp:Button ID="btnCrearTraduccion" runat="server" CssClass="btn btn-info" Visible="false" Text="CREAR TRADUCCIÓN" OnClick="btnCrearTraduccion_Click" />
    </h1>
    <br />
    <br />
    <br />




    <!--Formulario edición de traduccion --->
    <div id="frmTraduccionEditar" runat="server" visible="false">

        <h1 class="h1">
            <asp:Literal ID="litTituloTraduccionEditar" runat="server">TRADUCCION EDITAR</asp:Literal>
            <br />
            <br />
            <div class="form-group">
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                         <span class="input-group-text w-100" id="litPaginaTraduccionEdit" runat="server">PAGINA</span>
                    </div>
                    <asp:TextBox ID="txtPaginaEditar" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="50"></asp:TextBox>
                </div>

                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                         <span class="input-group-text w-100"  id="litControlIDTraduccionEdit" runat="server">CONTROL ID</span>
                    </div>
                    <asp:TextBox ID="txtControlIDEditar" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="30"></asp:TextBox><asp:Button ID="btnTraduccionEditar" Visible="false" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnTraduccionEditar_Click" />
                </div>

                    <div class="input-group mb-3">
                    <div class="input-group-prepend">
                         <span class="input-group-text w-100"  id="litTextoTraduccionEdit" runat="server" >TEXTO</span>
                    </div>
                    <asp:TextBox ID="txtTraduccionEditar" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="256">

                    </asp:TextBox>
                </div>
                <br />

                <asp:Button ID="btnTraduccionGuardar" Visible="false" runat="server" CssClass="btn btn-primary btn-block" Text="GUARDAR" OnClick="btnTraduccionEditar_Click" />
            </div>
    </div>
    <!--Formulario nueva traduccion --->
    <div id="frmTraduccionNueva" runat="server" visible="false">
        <div class="form-group">
            <h1 class="h1">
                <asp:Literal ID="litTituloTraduccionNueva" runat="server">TRADUCCIÓN NUEVA</asp:Literal></h1>
                <br />
                <br />

                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                       <span class="input-group-text w-100" id="litPaginaTraduccion" runat="server" >PÁGINA</span>
                    </div>
                    <asp:TextBox ID="txtPaginaNueva" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="50"></asp:TextBox>
                    
                    </div>
                 <div class="input-group mb-3">
                      <div class="input-group-prepend">
                        <span class="input-group-text w-100" id="litControlID" runat="server">CONTROL ID</span>
                    </div>
                    <asp:TextBox ID="txtControlIDNuevo" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="300"></asp:TextBox>
                </div>

              <div class="input-group mb-3">
                      <div class="input-group-prepend">
                        <span class="input-group-text w-100" id="litTexto" runat="server" >TEXTO</span>
                    </div>
                    <asp:TextBox ID="txtTraduccionNueva" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="256"></asp:TextBox>
                </div>
                  <br />    
                  <asp:Button ID="btnTraduccionCrear" Visible="false" runat="server" CssClass="btn btn-primary btn-block" Text="CREAR" OnClick="btnTraduccionCrear_Click" />
                </div>
        </div>



    <asp:HiddenField ID="hdnCodTraduccionEditar" runat="server" />
   
        <asp:GridView ID="gvTraducciones" runat="server" AllowPaging="true" AutoGenerateColumns="False" 
            OnPageIndexChanging="gvTraducciones_PageIndexChanging" class="table table-dark pagination-ys" OnRowCommand="gvTraducciones_RowCommand" 
            OnRowDataBound="gvTraducciones_RowDataBound" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="TopAndBottom" PagerStyle-HorizontalAlign="Center"
            PagerStyle-BackColor="#2e2e2e" PagerStyle-BorderStyle="Solid">
            <Columns>
                <asp:BoundField DataField="CodTraduccion" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="CodIdioma" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Pagina" HeaderText="Página" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ControlID" HeaderText="ControlID" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Texto" HeaderText="Texto" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnEditarTraduccion" runat="server" CommandName="EditarTraduccion" CssClass="btn btn-success" Text="EDITAR" CommandArgument='<%#Eval("CodTraduccion") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnBorrarTraduccion" runat="server" CommandName="BorrarTraduccion" CssClass="btn btn-danger" Text="BORRAR" CommandArgument='<%# Eval("CodTraduccion") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
        </asp:GridView>
         <hr />
            <asp:ImageButton ID="imgBtnExportExcel_Traducciones" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcel_Traducciones_Click" />
            <asp:ImageButton ID="imgBtnExportPDF_Traducciones" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDF_Traducciones_Click" />
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
