<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ContraDescuento.GUI.Index" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>Contra Descuento</title>
    <style>
        .card-top {
            background-image: url('Recurso/Imagen/supermercado.jpg');
            background-position: center;
            background-size: cover;
        }

        .card-header {
            background-color: #BDBDBD;
        }

        .card_item {
            margin: 3px;
            width: 18rem;
            height: 38rem;
            overflow: hidden;
        }

            .card_item > img {
                width: 270px;
                height: 280px;
            }

        .carousel-item {
            overflow: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">


    <div class="container-fluid">
        <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s ">
            <div class="alert alert-success" role="alert">
                <%--<h4>Status</h4>
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


        <div class="jumbotron bg-dark border-dark " style="margin-top: 35px">
            <h1 class="display-4 text-light">
                <asp:Literal ID="litTitulo1" runat="server">¡Último momento! </asp:Literal><img src="Recurso/Ico/sale_64.png" /></h1>
            <div class="card text-center">
                <div class="card-header text-uppercase bg-light ">
                    <img src="Recurso/Ico/shopping-cart_64.png" width="32px" />&nbsp;<asp:Literal ID="litTitulo2" runat="server">Descuentos del día</asp:Literal>&nbsp;<%--<img src="Recurso/Imagen/qr.png" width="32px" />--%>
                </div>
                <div class="card-body card-top">
                    <div id="carouselExampleSlidesOnly" class="carousel slide" data-ride="carousel" data-interval="3000">
                        <div class="carousel-inner">
                            <asp:Repeater ID="rptOfertaDelDia" runat="server" OnItemDataBound="rptOfertaDelDia_ItemDataBound" OnItemCommand="rptOfertaDelDia_ItemCommand">
                                <ItemTemplate>
                                    <div class="carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                        <h4 class="card-title text-xl-center text-uppercase bg-secondary" style="padding: 5px; color: #FFF">
                                            <asp:Literal ID="litDescuentoDelDiaTitulo" runat="server"></asp:Literal></h4>
                                        <p class="card-text text-danger text-xl-center">
                                            OFF
                                            <asp:Literal ID="litDescuentoDelDiaPorcentajeDescuento" runat="server"></asp:Literal>%
                                        </p>
                                        <img src="Recurso/Imagen/empty.png" width="200" height="220" id="imgDescuentoDelDia" runat="server" />
                                        <%--class="img-thumbnail img-fluid"--%>
                                        <div class="row justify-content-center">
                                        <p class="card-text  text-danger" style="text-decoration: line-through" runat="server" id="litPrecioAnterior">ANTES: $
                                            </p><p class="card-text  text-danger" style="text-decoration: line-through" ><asp:Literal ID="litDescuentoDelDiaPrecioAnterior" runat="server"></asp:Literal></p>
                                            </div>
                                        <div class="row justify-content-center">
                                        <p class="card-text text-success text-center" style="font-size: 24px" runat="server" id="litPrecioFinal">AHORA: $</p>
                                            <p class="card-text text-success text-center" style="font-size: 24px"  ><asp:Literal ID="litDescuentoDelDiaPrecioActual" runat="server"></asp:Literal></p>
                                            </div>
                                        <%--<a href="#" class="btn btn-block btn-danger" id="btnVerMasOfertaDelDia" runat="server">[+] VER MÁS</a>--%>
                                        <asp:LinkButton ID="btnVerMasOfertaDelDia" runat="server" CssClass="btn btn-block btn-danger" Text="[+] VER MÁS" CommandName="VerMas" CommandArgument='<%# Eval("LstProducto[0].CodProducto") %>' />
                                    </div>
                                </ItemTemplate>

                            </asp:Repeater>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <link href="Lib/Style/simple-sidebar.css" rel="stylesheet" />
        <div class="d-flex" id="wrapper" style="margin-left: -30px !important;">





            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <asp:UpdatePanel ID="IDPanelBusqueda" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
            <!-- Sidebar -->
            <div class="bg-light border-right" id="sidebar-wrapper">
                <div class="sidebar-heading text-center text-capitalize bg-info text-light">
                    <h3 runat="server" id="tituloBuscarPor" class="text-center font-italic">BUSCAR POR:</h3>
                </div>
                <div class="list-group list-group-flush flex-fill" style="width: auto!important;">
                    <br />
                    <h5 class="text-center text-capitalize" runat="server" id="tituloCategoria">CATEGORIA</h5>
                    <asp:DropDownList ID="ddlCategoria" runat="server" DataValueField="CodCategoria" DataTextField="Descripcion" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                    <br />
                    <div id="pnSubCategoria" runat="server" visible="false">
                        <hr class="bg-dark" />
                        <h5 class="text-center text-capitalize" runat="server" id="tituloSubCategoria">SUBCATEGORIA</h5>
                        <asp:DropDownList ID="ddlSubCategoria" runat="server" DataValueField="CodSubCategoria" DataTextField="Descripcion" CssClass="form-control"></asp:DropDownList>
                        <br />
                        <hr class="bg-dark" />
                    </div>
                    <h5 class="text-center text-capitalize" runat="server" id="tituloDescuento">% DESCUENTO</h5>
                    <asp:TextBox ID="txtProductoDescuento" runat="server" CssClass="form-control" placeholder="" type="number" min="0" max="100" TextMode="Number" MaxLength="3"></asp:TextBox>
                    <asp:RangeValidator runat="server" ID="rngValidator" ControlToValidate="txtProductoDescuento" ErrorMessage="El descuento debe estar entre 0 y 100" ForeColor="Red" Display="Dynamic" MinimumValue="0" MaximumValue="100" Type="Double"></asp:RangeValidator>
                    <br />
                    <hr class="bg-dark" />
                    <asp:Button ID="btnBuscarDescuentos" runat="server" CssClass="btn btn-block btn-primary" Text="BUSCAR" OnClick="btnBuscarDescuentos_Click" />
                    <br />
                    <asp:Button ID="btnVerTodo" runat="server" CssClass="btn btn-block btn-warning" Text="VER TODOS" OnClick="btnVerTodo_Click" />
                </div>
            </div>
                   </ContentTemplate>
             
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlCategoria" EventName="DataBinding" />
                    <asp:AsyncPostBackTrigger ControlID="ddlSubCategoria" EventName="DataBinding" />
         
                </Triggers>
            </asp:UpdatePanel>


            <!-- /#sidebar-wrapper -->
            <nav class="navbar w-auto navbar-expand-lg navbar-light bg-light border-bottom " style="width: 100% !important;">
                <button class="btn btn-info animated flipInX slow infinite" id="menu-toggle"><<</button>


                    <asp:UpdatePanel ID="updDescuentos" runat="server" UpdateMode="always" >
                <ContentTemplate>

                <div class="container-fluid">
                    <div class="row justify-content-md-center">
                        <asp:Repeater ID="rptProductos" runat="server" OnItemDataBound="rptProductos_ItemDataBound" OnItemCommand="rptProductos_ItemCommand">
                            <ItemTemplate>
                                <div class="card card_item">
                                    <img src="~/Recurso/Imagen/empty.png" runat="server" id="imgProducto" class="card-img-top">
                                     <div class="card-text bg-ligth text-uppercase font-italic font-weight-bolder text-center">
                                         
                                         <img src="~/Recurso/Imagen/empty.png" runat="server" width="50" height="45" id="imgComercio" /><br />
                                          <p class="text-black-50 text-uppercase font-italic"><asp:literal ID="litComercioNombre" runat="server"></asp:literal> </p>
                                         <hr class="bg-light" />
                                     </div>
                                    <div class="card-body">
                                        <h5 class="card-title text-center">
                                            <asp:Literal ID="litTitulProducto" runat="server"> </asp:Literal>
                                            <br />
                                            <p class="badge badge-danger">
                                                <asp:Literal ID="litPorcentajeDescuentoProducto" runat="server"></asp:Literal>% OFF
                                            </p>
                                        </h5>
                                        <div class="row justify-content-center">
                                            <p id="pPrecioAnterior" class="text-danger text-center" style="text-decoration: line-through" runat="server">PRECIO ANTERIOR: $
                                            </p><p  class="text-danger text-center" style="text-decoration: line-through"> <asp:Literal ID="litPrecioAnteriorProducto" runat="server"></asp:Literal></p>
                                            </div>
                                        <div class="row justify-content-center">
                                        <p class="card-text text-success text-center" id="pPrecioActual" runat="server">PRECIO FINAL: $</p><p class="text-success text-center" > <asp:Literal ID="litPrecioActualProducto" runat="server"></asp:Literal></p>
                                            </div>
                                        <%--<a href="#" class="btn text-center btn-primary btn-block text-upperCase" runat="server" id="btnVerDetalleProducto">[+] VER MÁS</a>--%>
                                        <asp:LinkButton ID="btnVerDetalleProducto" runat="server" CssClass="btn text-center btn-primary btn-block text-upperCase" Text="[+] VER MÁS" CommandName="VerMas" CommandArgument='<%# Eval("LstProducto[0].CodProducto") %>'></asp:LinkButton>
                                    </div>
                                </div>
                            </ItemTemplate>

                        </asp:Repeater>
                    </div>
                    

                    <div id="PnResultadoSinDescuentos" runat="server" visible="false" class="container justify-content-center">
                        <div class="container">
                            <h1 class="text-center font-italic">¡Ops, no se han encontrado descuentos pero te podemos mostrar otras ofertas!</h1>
                            <div class="row">
                                <img src="Recurso/Imagen/empty.png" width="500" class="justify-content-center"  />
                            </div>
                            <asp:Button ID="btnVerMasOfertas" runat="server" CssClass="btn btn-success btn-block animated pulse slow" Text="[VER MÁS OFERTAS]" OnClick="btnVerMasOfertas_Click" />
                        </div>
                    </div>

                </div>
                    </ContentTemplate>
                        <Triggers>
                                       <asp:AsyncPostBackTrigger ControlID="rptProductos" EventName="DataBinding" />
                        </Triggers>
                        </asp:UpdatePanel>

            </nav>

                 
            <script type="text/javascript">


                $(document).ready(function () {
                    $("#<%= txtProductoDescuento.ClientID %>").attr('maxlength', '4');
                });

                $(".card_item > img").mouseover(function () {
                    this.className = "zoom animated flipInX slow";

                    $(this).css("opacity", "1");
                });
                $(".card_item > img").each(function () {
                    $(".card_item > img").css("opacity", "0.6");
                });

                $("#menu-toggle").click(function (e) {
                    e.preventDefault();
                    if ($("#wrapper").hasClass("toggled")) { $("#menu-toggle").html("<<"); }
                    else { $("#menu-toggle").html(">>"); }
                    $("#wrapper").toggleClass("toggled");
                });

                $("#<%= txtProductoDescuento.ClientID %>").keypress(function () {
                    var text = $("#<%= txtProductoDescuento.ClientID %>").val();
                    if (text.length > 3) {
                        text = text.substring(0, 2);
                        $("#<%= txtProductoDescuento.ClientID %>").val(text);
                    }
                });


    //$(".card item >img").mouseout().each(function () {
    //    this.className = "";
    //});

    //$(".card_item").mouseover().addClass("zoom");
            </script>
</asp:Content>
