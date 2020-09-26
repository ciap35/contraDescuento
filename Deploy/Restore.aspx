<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Restore.aspx.cs" Inherits="ContraDescuento.GUI.Admin.Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Restore</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="jumbotron">

        <div id="container">
            <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-1s  ">

                <div class="alert alert-danger" role="alert">
                    <h4 class="alert-heading">¡Ups!</h4>
                    <hr />
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                    <br />
                    <img src="Recurso/Imagen/Error.png" width="400px" height="400px" />
                </div>

            </div>
        </div>




        <div id="PanelRestore" runat="server" visible="true">
            <h2 id="restore" runat="server">Restore de BD</h2>
            <div class="form-group">
                <br />
                <img src="Recurso/Ico/db_restore.png" class="animated fadeInDown slower delay-0.1s" />

                <br />
                <br />

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

                <asp:Repeater ID="rptBackups" runat="server" OnItemDataBound="rptBackups_ItemDataBound" OnItemCommand="rptBackups_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-dark">
                            <thead>
                                <tr style="color:#ff6a00">
                                    <th scope="col" style="text-align:center">#</th>
                                    <th scope="col" style="text-align:center" runat="server" id="thFechaDeCreacion">Fecha de creación</th>
                                    <th scope="col" style="text-align:center" runat="server" id="thArchivo">Archivo</th>
                                    </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <tr>
                            <th scope="row" style="text-align:center"><%# Container.ItemIndex +1  %></th>
                            <td style="text-align:center">
                                <asp:Literal ID="litFechaCreacion" runat="server"></asp:Literal></td>
                            <td style="text-align:center">
                                <asp:Literal ID="litBackup" runat="server"></asp:Literal></td>
                            <td>
                                <img src="Recurso/Ico/db_icon_BAK.png" width="30px" height="30px" /></td>
                            <td>
                                <asp:Button ID="btnRestore" UseSubmitBehavior="false" runat="server" CommandName="btnRestore" CommandArgument="<%# ((System.IO.FileInfo)GetDataItem()).FullName %>" Text="REALIZAR RESTORE" CssClass="btn btn-danger" /></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Literal ID="litbtnRestoreMensaje" runat="server"></asp:Literal>
            </div>

            <hr class="bg-light" />
                <asp:ImageButton ID="imgBtnExportExcel" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcel_Click" />
                <asp:ImageButton ID="imgBtnExportPDF" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDF_Click" />
        </div>




    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
