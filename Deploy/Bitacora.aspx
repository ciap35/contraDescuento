<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="ContraDescuento.GUI.Admin.Bitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    
    <link href="Lib/Style/Main.css" rel="stylesheet" />
    <div class="jumbotron">

        <div id="container">
            <div id="divMensajeError" runat="server" visible="false">
                <h4 class="alert-heading">¡Ups!</h4>
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>
        </div>
           

        <div id="PanelBitacora" runat="server" visible="false">
        <h2><asp:literal id="litTituloBitacoraDeSistema" runat="server">Bitacora de Sistema</asp:literal></h2>
        
            <div class="container-fluid">
                <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s ">
                    <div class="alert alert-success" role="alert">
                       <%-- <h4>Status</h4>
                        <hr />--%>
                        <p class="mb-0">
                            <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                        </p>
                    </div>
                </div>
            </div>
            <div class="form-group">

            <asp:GridView ID="gvBitacora" runat="server"  
                AllowPaging="true" AutoGenerateColumns="False" 
                PagerStyle-HorizontalAlign="Center"  
                PagerSettings-Mode="NumericFirstLast" 
                PagerSettings-Visible="true" PagerSettings-Position="TopAndBottom" 
                OnPageIndexChanging="gvBitacora_PageIndexChanging" class="table table-dark pagination-ys"
                OnRowDataBound="gvBitacora_RowDataBound"
                >
            <Columns>
                <asp:BoundField  DataField="CodMensaje" HeaderText="#"  InsertVisible="False" ReadOnly="True" SortExpression="codMensaje" />
                <asp:BoundField DataField="ExcepcionControlada" HeaderText="¿Excepción controlada?" ReadOnly="True" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="True" />
                <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" ReadOnly="True" />
                <asp:BoundField DataField="Stack" HeaderText="Stack" ReadOnly="True" />
                
            </Columns>
                 <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
        </asp:GridView>
                <hr class="bg-light" />
                <asp:ImageButton ID="imgBtnExportExcel" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcel_Click" />
                <asp:ImageButton ID="imgBtnExportPDF" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDF_Click" />
        </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
