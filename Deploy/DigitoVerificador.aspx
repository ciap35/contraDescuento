<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DigitoVerificador.aspx.cs" Inherits="ContraDescuento.GUI.DigitoVerificador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
<title>CD - Dígito Verificador</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="jumbotron">

        <div id="container-fluid">
            <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-1s ">
                <div class="alert alert-danger" role="alert">
                    <%--<h4 class="alert-heading">¡Ups!</h4>--%>
                    <hr />
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                    <br />
                    <img src="Recurso/Imagen/Error.png" width="400px" height="400px" />
                </div>
            </div>
        </div>

        <div class="container-fluid">
            <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s fadeOut delay-5s ">
                <div class="alert alert-success" role="alert">
                   <%-- <h4>Status</h4>
                    <hr />--%>
                    <p class="mb-0">
                        <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>

        <div id="PanelDigitoVerificador" runat="server" visible="false">
            <h2><asp:literal ID="litDigitoVerificadorTitulo" runat="server">Dígito Verificador</asp:literal></h2>
            <div class="form-group">
                <br />
                
                <img src="Recurso/Ico/locked-page_256.png" class="animated fadeInDown slower delay-0.1s" width="32px" />
                <asp:Button ID="btnValidarDigitoVerificador" runat="server" Text="Verificar integridad BD" CssClass="btn  btn-primary btn-danger" OnClick="btnValidarDigitoVerificador_Click" />
            </div>
        </div>


          <asp:Repeater ID="rptRegCorrupto" runat="server" OnItemDataBound="rptRegCorrupto_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-dark">
                            <thead>
                                <tr style="color:#ff6a00">
                                    <th scope="col" style="text-align:center">codRegistro</th>
                                    <th scope="col" style="text-align:center" runat="server" id="thFechaDeCreacion">Registro</th>
                                     <th scope="col" style="text-align:center"></th>
                                    </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <tr>
                            <th scope="row" style="text-align:center"><asp:Literal ID="litCodigoRegistro" runat="server"></asp:Literal></th>
                            <td style="text-align:center">
                                <asp:Literal ID="litInfoRegistro" runat="server"></asp:Literal></td>                                
                            <td>
                                <img src="Recurso/Ico/error.png" width="30px" height="30px" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>




    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
