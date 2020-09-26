<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountManager.aspx.cs" Inherits="ContraDescuento.GUI.AccountManager" %>

<%@ Register Src="~/UserControl/ucLogin.ascx" TagPrefix="uc1" TagName="ucLogin" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <br />
    <br />
    <br />
    
    <div id="container">
        <div id="divMensajeError" runat="server" visible="false">
            <h4 class="alert-heading">¡Ups!</h4>
            <div class="alert alert-danger" role="alert">
                <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>
    </div>

      <div class="container-fluid">
                    <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s ">
                        <div class="alert alert-success" role="alert">
                         <%--   <h4>Status</h4>
                            <hr />--%>
                            <p class="mb-0">
                                <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                            </p>
                        </div>
                    </div>
                <uc1:ucLogin runat="server" ID="ucLogin" />
                </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
