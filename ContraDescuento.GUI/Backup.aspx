<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="ContraDescuento.GUI.Admin.Backup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Backup</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="jumbotron">

        <div id="container-fluid">
            <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-1s ">
            
                <div class="alert alert-danger" role="alert">
                        <h4 class="alert-heading">¡Ups!</h4>
                    <hr />
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                     <br /><img src="Recurso/Imagen/Error.png" width="400px" height="400px" />
                </div>
               
            </div>
        </div>

        <div class="container-fluid">
            <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s fadeOut delay-5s ">
                <div class="alert alert-success" role="alert">
               <%--     <h4>Status</h4>
                    <hr />--%>
                    <p class="mb-0">
                        <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>


        

        <div id="PanelBackup" runat="server" visible="false">
            <h2 id="generacionBackup" runat="server">Generación de Backups</h2>
            <div class="form-group">
                <br />
                <img src="Recurso/Ico/db_backup.png" class="animated fadeInDown slower delay-0.1s" />
                <asp:Button ID="btnBackup" runat="server" Text="Generar Backup" CssClass="btn btn-primary" OnClick="btnBackup_Click" />
                <asp:Literal ID="litbtnBackupMensaje" runat="server"></asp:Literal>
                
            </div>
        </div>


    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
