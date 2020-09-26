<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="ContraDescuento.GUI.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="container">
       <br />
        <br />
        <h1 class="text-center">¡Ops, página no encontrada!</h1>
        <h2 class="text-center animated fadeInDownBig slow delay-1s">Haga click y lo llevaremos al inicio por usted.<br />
           
        </h2>
        <br />
        <br />
        <p class="text-center badge-pill animated fadeInDownBig slow delay-1s"><a style="text-decoration:underline;color:cornflowerblue;font-size:35px" href="Index.aspx">HOME</a></p>
        <br />
        <img src="Recurso/Imagen/404.png" width="100%" height="100%" class="animated fadeInDownBig slow delay-2s"/>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
