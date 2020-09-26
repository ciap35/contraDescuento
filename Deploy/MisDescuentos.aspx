<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisDescuentos.aspx.cs" Inherits="ContraDescuento.GUI.MisDescuentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <link href="Lib/Style/Main.css" rel="stylesheet" />
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

    <div class="container-fluid">
        <div id="container">
            <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-1s ">

                <div class="alert alert-danger" role="alert">
                    <h4 class="alert-heading">¡Ups!</h4>
                    <hr />
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                    <br />
                    <img src="Recurso/Imagen/Error.png" width="400px" height="400px" />
                </div>

            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="jumbotron justify-content-center">
            <h1 class="text-center font-italic" id="MisCuponesDeDescuento" runat="server">MIS CUPONES DE DESCUENTO
                <img src="Recurso/Ico/sale_64.png" width="64" /></h1>
            <br />
            <hr class="bg-light" />

            <div style="font-size: 25px" class="text-center">
                <asp:DataPager ID="dtPagerPrincipal" runat="server" PageSize="5" PagedControlID="LvMisDescuentos">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="badge badge-primary text-xl-center" />
                    </Fields>
                </asp:DataPager>
            </div>
            <br />
            <hr class="bg-dark" />
            <div class="row justify-content-md-center">
                <%--<div class="jumbotron justify-content-center">--%>
                <asp:ListView ID="LvMisDescuentos" Visible="true" runat="server" OnPagePropertiesChanging="LvMisDescuentos_PagePropertiesChanging" OnItemDataBound="LvMisDescuentos_ItemDataBound">
                    <ItemTemplate>
                        <div class="card">
                            <div class="card-header">
                                <div class="text-center">
                                    <h1 class="font-italic">
                                        <asp:Literal ID="litTituloProducto" runat="server"></asp:Literal></h1>
                                    <br />
                                    <asp:Image ID="imgProducto" runat="server" Width="300" />
                                </div>
                            </div>
                            <div class="card-header text-center">
                        <img src="~/Recurso/Imagen/empty.png" runat="server" id="imgComercio" class="animated fadeInLeft slow img-thumbnail figure-img" style="height: 150px !important;">
                            <p class="animated fadeInRight slow text-uppercase text-black-50 font-italic"><asp:Literal ID="litComercioNombre" runat="server"></asp:Literal></p>
                        
                            </div>
                            <div class="card-body">
                                <div class="text-center text-uppercase"><asp:literal id="litCantidad" runat="server">CANTIDAD:</asp:literal>
                                    <asp:Literal ID="litProductoCantidad" runat="server"></asp:Literal><br />
                                </div>
                                <div class="card-text text-center text-uppercase  text-danger"style="text-decoration: line-through"><asp:literal id="litPrecioAnterior" runat="server">PRECIO ANTERIOR:</asp:literal>
                                <asp:Literal ID="litProductoPrecio" runat="server"></asp:Literal>$<br />
                                </div>

                                <div class="text-center text-uppercase text-success"><asp:literal id="litDescuento" runat="server">DESCUENTO:</asp:literal>
                                    <asp:Literal ID="litProductoDescuento" runat="server"></asp:Literal>%<br />
                                </div>
                                <div class="text-center text-uppercase text-success"><asp:literal id="litAhorro" runat="server">AHORRO:</asp:literal>
                                    <asp:Literal ID="litProductoAhorroTotal" runat="server"></asp:Literal>$<br />
                                </div>
                                
                                <div class="text-center text-uppercase text-success"><asp:literal id="litPrecioFinal" runat="server">PRECIO FINAL:</asp:literal>
                                    <asp:Literal ID="litProductoPrecioFinal" runat="server"></asp:Literal>$<br />
                                </div>
                                <div class="text-center text-uppercase"><asp:literal id="litRetiraPor" runat="server">RETIRA POR: </asp:literal>
                                    <asp:Literal ID="litPuntoDeVenta" runat="server"></asp:Literal><br />
                                </div>
                                <div class="text-center text-uppercase"><asp:literal id="litFechaCupon" runat="server">FECHA CUPÓN: </asp:literal>
                                    <asp:Literal ID="litProductoFechaCupon" runat="server"></asp:Literal><br />
                                </div>
                                <br />
                                <div class="text-center text-uppercase">
                                    <asp:Literal ID="litProductoFechaCanje" runat="server">FECHA CANJE: </asp:Literal><br />
                                </div>
                                <%--  <div class="text-center">Cupón: <asp:Literal ID="litProductoCupon" runat="server"></asp:Literal><br /></div>--%>
                            </div>
                        </div>

                    </ItemTemplate>
                    <EmptyDataTemplate>
                          <%--  <div class="jumbotron">
                            <h2 ><asp:literal id="NoposeeDescuentos" runat="server">AÚN NO POSEE DESCUENTOS POR ACREDITAR</asp:literal></h2>
                        </div>--%>
                    </EmptyDataTemplate>

                </asp:ListView>
                   <div class="jumbotron">
                   <h2 ><asp:literal id="NoposeeDescuentos" runat="server" Visible="false">AÚN NO POSEE DESCUENTOS POR ACREDITAR</asp:literal></h2>
                       </div>
                <%--</div>--%>
            </div>
            <br />
            <hr class="bg-dark" />
            <div style="font-size: 25px" class="text-center">
                <asp:DataPager ID="dpLvMisDescuentos" runat="server" PageSize="5" PagedControlID="LvMisDescuentos">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="badge badge-primary text-xl-center" />
                    </Fields>
                </asp:DataPager>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
