<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ofertas.aspx.cs" Inherits="ContraDescuento.GUI.Ofertas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">


    <div class="container-fluid">
        <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s ">
            <div class="alert alert-success" role="alert">
              <%--  <h4>Status</h4>
                <hr />--%>
                <p class="mb-0">
                    <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                </p>
            </div>
        </div>
    </div>
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

    <div class="container-fluid" id="OfertasPanel" runat="server">
       

        <%--<div class="jumbotron bg-dark border-dark " style="margin-top: 35px">--%>

        <div id="panelOferta" runat="server" visible="false">
        <h1 class="display-4 text-light">
            <asp:Literal ID="litTituloOfertas" runat="server">¡ESTAS A UN PASO DE OBTENER TU DESCUENTO!</asp:Literal></h1>
        <div class="row justify-content-md-center">
            
            <div class="card">
                <div class="card card_item ">
                    
                    <img src="~/Recurso/Imagen/empty.png" runat="server" id="imgProducto" class="animated flipInX slow img-thumbnail" style="height: 500px !important;">
                    <div class="card-body">
                        <div class="card-header text-center">
                        <img src="~/Recurso/Imagen/empty.png" runat="server" id="imgComercio" class="animated fadeInLeft slow img-thumbnail figure-img" style="height: 150px !important;">
                            <p class="animated fadeInRight slow text-black-50 text-uppercase font-italic"><asp:Literal ID="litComercioNombre" runat="server"></asp:Literal></p>
                        
                            </div>
                        <h5 class="card-title text-center">

                            <asp:Literal ID="litTitulProducto" runat="server"> </asp:Literal>
                            <p class="badge badge-danger">
                                <asp:Literal ID="litPorcentajeDescuentoProducto" runat="server"></asp:Literal> <asp:Literal ID="litPorcentaje" runat="server">% OFF</asp:Literal>
                            </p>
                        </h5>
                        
                        
                        <p class="text-danger text-center" style="text-decoration: line-through"><asp:literal id="litPrecioAnterior" runat="server">PRECIO ANTERIOR: </asp:literal><asp:Literal ID="litPrecioAnteriorProducto" runat="server"></asp:Literal>$</p>
                        <p class="card-text text-success text-center"><asp:literal ID="litPrecioFinal" runat="server">PRECIO FINAL: </asp:literal><asp:Literal ID="litPrecioActualProducto" runat="server"></asp:Literal>$</p>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">
                                <h5 class="card-title">
                                    <asp:Literal ID="litPuntoDeVentaDisponibleTitulo" runat="server">LO RETIRO POR: </asp:Literal></h5>
                                <asp:RadioButtonList ID="chkLstPuntosDeVentaDisponible" runat="server"></asp:RadioButtonList>
                                <%--<asp:Literal ID="litPuntoDeVentaDisponible" runat="server"></asp:Literal>--%>

                            </li>
                        </ul>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">
                                <h5 class="card-title">
                                    <asp:Literal ID="litABMCantidadTitulo" runat="server">CANTIDAD</asp:Literal></h5>
                                <asp:TextBox ID="txtProductoCantidad" runat="server" CssClass="form-control" placeholder="Cantidad" type="number" Text="1" TextMode="Number" min="0" max="9999" MaxLength="4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvProductoCantidad" runat="server" ControlToValidate="txtProductoCantidad" InitialValue="0" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>

                            </li>
                        </ul>
                        <%--<asp:ImageButton ID="btnCanjearDescuento" runat="server" AlternateText="OBTENER MI DESCUENTO" ImageUrl="Recurso/Ico/shopping-cart_64.png"
                            OnClick="btnCanjearDescuento_Click" CssClass="btn text-center btn-outline-primary btn-block text-upperCase" />--%>



                        <div id="divMensajeInformativo" runat="server" visible="false" style="width: 420px" class=" animated fadeInDown delay-1s ">
                            <div class="alert alert-info" role="alert">
                                <h4 class="alert alert-heading">¡Ups!</h4>
                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                <hr />
                                <asp:Literal ID="litMensajeInformativoOferta" runat="server" Visible="false"></asp:Literal>

                                <br />
                            </div>
                        </div>





                        <div class="btn text-center btn-outline-success btn-block text-upperCase">
                            <img src="Recurso/Ico/shopping-cart_64.png" width="24" />
                            <asp:Button ID="btnCanjear" runat="server" CssClass="btn btn-block text-upperCase" OnClick="btnCanjear_Click" Text="CANJEAR" />
                        </div>
                        <%--<a href="#" class="btn text-center btn-outline-primary btn-block text-upperCase" runat="server" id="btnCanjearProducto">
                            OBTENER MI DESCUENTO</a>--%>
                    </div>
                </div>
            </div>
        </div>
            </div>
        <br />
        <br />
        <div id="panelSinStock" runat="server" visible="false">

            <div class="jumbotron bg-dark text-light text-center">
                <br />
                <br />
                <h1>
                    <br /><asp:literal ID="SinStock" runat="server">NOS ENCONTRAMOS SIN STOCK EN ESTE MOMENTO PARA EL PRODUCTO SELECCIONADO.</asp:literal>
                    
                </h1>
                <br />
                <hr class="bg-light" />
                <h2 class="text-center">
                    <asp:Literal ID="QueEsperas" runat="server">¿QUE ESPERAS PARA OBTENER LOS MEJORES DESCUENTOS?.</asp:Literal>
                    <br />
                    <br />
                    <a href="Index.aspx"><img src="Recurso/Ico/home.png" class="animated infinite flipOutY slow" width="64px" /></a>
                </h2>
            </div>
        </div>




        </div>

        <script type="text/javascript">


            $(document).ready(function () {
                $("#<%= txtProductoCantidad.ClientID %>").attr('maxlength', '4');
            });


            /*Evitar ingresar más de 4 carácteres*/
            $("#<%= txtProductoCantidad.ClientID %>").keypress(function () {
                var text = $("#<%= txtProductoCantidad.ClientID %>").val();
                if (text.length > 4) {
                    text = text.substring(0, 3);
                    $("#<%= txtProductoCantidad.ClientID %>").val(text);
                }

            });
            /*Evitar ingresar letras*/
            $("#<%= txtProductoCantidad.ClientID %>").keyup(function () {
                $("#<%= txtProductoCantidad.ClientID %>").val(this.value.match(/[0-9]*/));
            });
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
