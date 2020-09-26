<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComercioAdministracion.aspx.cs" Inherits="ContraDescuento.GUI.ComercioAdministracion" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControl/ucPuntoDeVenta.ascx" TagPrefix="uc1" TagName="PuntoDeVenta" %>
<%@ Register Src="~/UserControl/ucProducto.ascx" TagPrefix="uc1" TagName="ucProducto" %>




<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="Lib/Style/simple-sidebar.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="d-flex" id="wrapper" style="margin-left: -20px !important;">







        <!-- Sidebar -->
        <div class="bg-light border-right" id="sidebar-wrapper">
            <div class="sidebar-heading text-center text-capitalize bg-dark text-light">
                <div>
                    <asp:Image runat="server" ID="ComercioLogo" CssClass="img-thumbnailbg-transparent" Width="120" Height="120" />
                </div>
                <br />
                <div id="MiComercioNombre" style="font-size: 18px">
                    <asp:Literal ID="litComercioSideBarNombreComercio" runat="server">MI COMERCIO</asp:Literal>
                </div>
            </div>
            <div class="list-group list-group-flush flex-fill" style="width: auto!important;">
                <asp:LinkButton class="list-group-item list-group-item-action bg-info text-light text-center" runat="server" ID="navMisDatos" CausesValidation="false" OnClick="navMisDatos_Click"><img src="Recurso/Ico/store_white.png" width="24" />&nbsp;<asp:Literal ID="litSideMisDatos" runat="server">Mis Datos &nbsp;</asp:Literal></asp:LinkButton>
                <asp:LinkButton class="list-group-item list-group-item-action bg-info text-light text-center" runat="server" ID="navMisProductos" CausesValidation="false" OnClick="navMisProductos_Click"><img src="Recurso/Ico/shopping-bag_white.png" width="24" />&nbsp;<asp:Literal ID="litSideMisProductos" runat="server">Mis Productos</asp:Literal></asp:LinkButton>
                <asp:LinkButton Visible="true" class="list-group-item list-group-item-action bg-info text-light text-center" runat="server" CausesValidation="false" ID="navMisDescuentos" OnClick="navMisDescuentos_Click"><img src="Recurso/Ico/shopping-bag_white.png" width="24" />&nbsp;<asp:Literal ID="litSideMisDescuentos" runat="server">Mis Descuentos</asp:Literal></asp:LinkButton>
                <%--                <a href="#" class="list-group-item list-group-item-action bg-info text-light text-center">Mis productos</a>
                <a href="#" class="list-group-item list-group-item-action bg-info text-light text-center">Mis descuentos</a>--%>
                <%--<hr class="dropdown-toggle-split bg-dark text-light w-75" />--%>
                <div class="sidebar-heading text-center text-capitalize bg-dark text-light">
                    <asp:Literal ID="litComercioSideBarMenuReporte" runat="server">MIS ESTADÍSTICAS<br /><img src="Recurso/Ico/diagram.png" width="32" />&nbsp;</asp:Literal>
                </div>
                <asp:LinkButton class="list-group-item list-group-item-action bg-info text-light text-center" runat="server" ID="navEstadisticasComercio" CausesValidation="false" OnClick="navEstadisticasComercio_Click"><img src="Recurso/Ico/store_white.png" width="24" />&nbsp;<asp:literal ID="litSideMiComercio" runat="server">MI COMERCIO</asp:literal>&nbsp;</asp:LinkButton>
                <%--<a href="#" class="list-group-item list-group-item-action bg-info text-light text-center">Desempeño de descuentos</a>
                <a href="#" class="list-group-item list-group-item-action bg-info text-light text-center">Desempeño de mis artículos</a>--%>
            </div>
        </div>

        <!-- /#sidebar-wrapper -->

        <!-- Page Content -->
        <div id="page-content-wrapper">

            <nav class="navbar w-auto navbar-expand-lg navbar-light bg-light border-bottom bg-dark" style="width: 101% !important">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <button class="btn btn-info animated flipInX slow infinite" id="menu-toggle"><<</button>






                <!-- MENSAJES DE ERROR U OK-->


                <div class="container-fluid justify-content-center" id="Mensajes">
                    <div id="divMensajeError" runat="server" visible="false" role="alert" class=" alert alert-danger animated fadeIn fadeOut slow delay-5s">
                        <h4 class="alert-heading">¡Ups! 
                            <button type="button" class="close" data-dismiss="alert">&times;</button></h4>
                        <div class="alert alert-danger" role="alert">
                            <asp:Literal ID="litMensajeError" runat="server" Visible="true"></asp:Literal>

                        </div>
                    </div>

                    <div id="divMensajeOK" runat="server" visible="false" role="alert" class=" alert alert-success animated fadeIn fadeOut slow delay-5s">
                        <h4><asp:Literal ID="litFelicidadesTitulo" runat="server"> FELICIDADES </asp:Literal>
                            <button type="button" class="close" data-dismiss="alert">&times;</button></h4>
                        <hr />
                        <p class="mb-0">
                            <asp:Literal ID="litMensajeOk" runat="server" Visible="true"></asp:Literal>
                        </p>
                    </div>
                </div>


                <!-- FIN MENSAJES DE ERROR U OK-->

            </nav>

            <asp:Panel ID="pnMisDatos" runat="server">
                <div class="container-fluid bg-light">
                    <!--Accordion wrapper-->
                    <div class="accordion md-accordion" id="accordionEx" role="tablist" aria-multiselectable="true">
                        <!-- Accordion Mis Datos-->
                        <div class="card" id="cardMisDatos">
                            <!-- Card header Mis Datos -->
                            <div class="card-header bg-dark text-light" role="tab" id="headingOne0">
                                <a data-toggle="collapse" data-parent="#accordionEx" href="#MisDatos" aria-expanded="true"
                                    aria-controls="collapseOne1">
                                    <h5 class="mb-0">
                                        <h3 class="card-subtitle text-uppercase text-center "><asp:literal id="litMisDatosMenuToogle" runat="server">Mis Datos</asp:literal>
                                        <img id="MisDatosArrowUp" src="Recurso/Ico/up_arrow.png" width="32" hidden /><img id="MisDatosArrowDown" src="Recurso/Ico/down_arrow.png" width="32" hidden /></h3>
                                        <i class="fas fa-angle-down rotate-icon"></i>
                                    </h5>
                                </a>
                            </div>
                            <div id="MisDatos" class="collapse show" role="tabpanel" aria-labelledby="headingOne0"
                                data-parent="#accordionEx">
                                <div class="card-body">
                                    <div runat="server" id="MisDatos" visible="true">

                                        <br />
                                        <div class="form-group">
                                            <div class="input-group mb-3">
                                                <div class="input-group-prepend  ">
                                                    <span class="input-group-text w-100 "><span class="align-content-md-center " aria-hidden="true"><asp:literal ID="litNombreComercioToggle" runat="server">Nombre de comercio</asp:literal></span></span>
                                                </div>
                                                <asp:TextBox ID="txtComercio" runat="server" CssClass="form-control" placeholder="..." MaxLength="50" TabIndex="1"></asp:TextBox>
                                            </div>

                                            <div class="input-group mb-3">
                                                <div class="input-group-prepend  ">
                                                    <span class="input-group-text w-100 "><span class="align-content-md-center " aria-hidden="true"><asp:literal ID="litDescripcionComercio" runat="server">Descripción</asp:literal></span></span>
                                                </div>
                                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="..." TextMode="MultiLine" MaxLength="200" Rows="8" Style="resize: none" TabIndex="2"></asp:TextBox>
                                            </div>

                                            <div class="input-group mb-3">
                                                <div class="input-group-prepend  ">
                                                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true"><asp:literal ID="litLogo" runat="server">Logo</asp:literal></span></span>
                                                    <asp:Button ID="btnCargarLogo" runat="server" Text="Cargar" CssClass="btn btn-primary btn-block" OnClick="btnCargarLogo_Click" />
                                                </div>
                                                <asp:FileUpload ID="fLogoUpload" runat="server" CssClass="btn btn-dark text-light" TabIndex="3" />

                                                <span class="input-group-text w-350" id="MantenerLogo">&nbsp;&nbsp;
                                                 <asp:CheckBox ID="ChkMantenerLogo" name="MantenerLogo" runat="server" Text="MANTENER MI LOGO ACTUAL&nbsp;&nbsp;" TextAlign="Left" TabIndex="4" />
                                                </span>


                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Accordion Teléfono-->
                        <div class="card" id="cardMiTelefono">
                            <!-- Card header Teléfono -->
                            <div class="card-header bg-dark text-light" role="tab" id="headingOne1">
                                <a data-toggle="collapse" data-parent="#accordionEx" href="#telefono" aria-expanded="true"
                                    aria-controls="collapseOne1">
                                    <h5 class="mb-0">
                                        <h3 class="card-subtitle text-uppercase text-center "><asp:literal ID="litTelefono" runat="server">Teléfono</asp:literal>
                                          <img id="MiTelefonoArrowUp" src="Recurso/Ico/up_arrow.png" width="32" hidden /><img id="MiTelefonoArrowDown" src="Recurso/Ico/down_arrow.png" width="32" hidden />

                                        </h3>
                                        <i class="fas fa-angle-down rotate-icon"></i>
                                    </h5>
                                </a>
                            </div>
                            <!-- Card Teléfono -->
                            <div id="telefono" class="collapse show" role="tabpanel" aria-labelledby="headingOne1"
                                data-parent="#accordionEx">
                                <div class="card-body">
                                    <br />
                                    <div class="input-group mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text w-100"><asp:literal ID="litCaracteristica" runat="server">Característica</asp:literal></span>
                                        </div>
                                        <asp:TextBox ID="txtCaracteristica" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="10" TabIndex="5"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regex_Caracteristica" runat="server" Enabled="true" ControlToValidate="txtCaracteristica" Display="Dynamic" ValidationExpression="([+]?\d{1,2})" ForeColor="Red" SetFocusOnError="true" error="Código de país inválida"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="input-group mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text w-100" id="Celular">&nbsp;&nbsp; 
                                        <asp:CheckBox ID="chkCelular" runat="server" Text="CELULAR" TextAlign="Left" TabIndex="6" /></span>
                                        </div>
                                    </div>
                                    <div class="input-group mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text w-100"><asp:literal ID="litNumero" runat="server">Número</asp:literal></span>
                                        </div>
                                        <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="20" TabIndex="7"></asp:TextBox>
                                    </div>
                                    <div class="input-group mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text w-100"><asp:literal ID="litTelefonoObservacion" runat="server">Teléfono - Observación</asp:literal></span>
                                        </div>
                                        <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="50" TabIndex="8"></asp:TextBox>
                                    </div>


                                </div>
                            </div>
                            <!-- Card Teléfono -->

                        </div>
                        <!-- Fin Accordion Teléfono-->
                        <!-- Accordion Puntos de venta -->

                        <div class="card" id="cardMiPuntoDeVenta">
                            <!-- Card header Punto de teléfono -->
                            <div class="card-header bg-dark text-light" role="tab" id="heading2">
                                <a data-toggle="collapse" data-parent="#accordionEx" href="#PuntoDeVenta" aria-expanded="true"
                                    aria-controls="collapseOne2">
                                    <h5 class="mb-0">
                                        <h3 class="card-subtitle text-uppercase text-center "><asp:literal ID="litPuntosDeVentaToogle" runat="server">Puntos de venta</asp:literal>
                                        <img id="MiPuntoDeVentaArrowUp" src="Recurso/Ico/up_arrow.png" width="32" hidden /><img id="MiPuntoDeVentaArrowDown" src="Recurso/Ico/down_arrow.png" width="32" hidden /></h3>
                                        <i class="fas fa-angle-down rotate-icon"></i>
                                    </h5>
                                </a>
                            </div>
                            <!-- Card Puntos de Venta-->
                            <div id="PuntoDeVenta" class="collapse show" role="tabpanel" aria-labelledby="heading2"
                                data-parent="#accordionEx">
                                <div class="card-body">
                                    <br />
                                    <uc1:PuntoDeVenta runat="server" ID="PuntoDeVenta1" />
                                </div>
                            </div>
                        </div>
                        <!-- Accordion Puntos de venta -->
                        <br />
                        <br />
                        <br />
                    </div>
                    <!--Fin Accordion wrapper-->
                    <asp:Button ID="btnModificarDatos" runat="server" Text="ACTUALIZAR MIS DATOS" class="btn btn-block btn-success" TabIndex="9" OnClick="btnModificarDatos_Click" />
                    <%--<asp:Button ID="btnCancelarSuscripcion" runat="server" Text="Cancelar suscripción" class="btn btn-block btn-danger" TabIndex="13" OnClick="btnCancelarSuscripcion_Click" />--%>
                </div>
                <!--Fin - Mis Datos !-->
            </asp:Panel>
            <asp:Panel ID="pnMisProductos" runat="server" Visible="false">
                <uc1:ucProducto runat="server" ID="ucProducto" />
            </asp:Panel>
            <asp:Panel ID="pnMisDescuentos" runat="server" Visible="false">
                <div class="jumbotron-fluid">
                    <br />
                    <h1 class="text-center font-italic"><asp:literal ID="litAcreditacionDescuentos" runat="server">ACREDITACIÓN DE DESCUENTOS</asp:literal>
                        <img src="Recurso/Ico/discount_64.png" width="64" /></h1>

                    <!--DESCUENTOS A CONFIRMAR-->
                    <hr class="bg-light" />

                    <div style="font-size: 25px" class="text-center">
                        <asp:DataPager ID="dtPagerPrincipal" runat="server" PageSize="3" PagedControlID="LvMisDescuentos">
                            <Fields>
                                <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="badge badge-primary text-xl-center" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                    <br />
                    <hr class="bg-dark" />
                    <div class="row justify-content-md-center ">
                        <%--<div class="jumbotron justify-content-center">--%>
                        <asp:ListView ID="LvMisDescuentos" Visible="true" runat="server" OnItemCommand="LvMisDescuentos_ItemCommand" OnPagePropertiesChanging="LvMisDescuentos_PagePropertiesChanging" OnItemDataBound="LvMisDescuentos_ItemDataBound">
                            <ItemTemplate>
                                <div class="card">
                                    <div class="card-header">
                                        <div class="text-center">
                                            <h1 class="font-italic">
                                                <asp:Literal ID="litTituloProducto" runat="server"></asp:Literal></h1>
                                            <br />
                                            <asp:Image ID="imgProducto" runat="server" Width="150" />
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="card-body">

                                        <div class="text-center badge-info"><asp:literal ID="litDatosDeUsuario" runat="server">DATOS DE USUARIO</asp:literal></div>

                                        <div class="text-center">
                                          <asp:literal ID="litUsuarioNombreDescuento" runat="server">  NOMBRE:</asp:literal>
                                            <asp:Literal ID="litUsuarioNombre" runat="server"></asp:Literal><br />
                                            <asp:literal ID="litUsuarioApellidoDescuento" runat="server">APELLIDO:</asp:literal>
                                            <asp:Literal ID="litUsuarioApellido" runat="server"></asp:Literal><br />
                                            <asp:literal ID="litEmailDescuento" runat="server">EMAIL:</asp:literal>
                                            <asp:Literal ID="litUsuarioEmail" runat="server"></asp:Literal><br />
                                            <asp:literal ID="litTelefono" runat="server">TELÉFONO:</asp:literal>
                                            <asp:Literal ID="litUsuarioTelefono" runat="server"></asp:Literal><br />
                                        </div>
                                        <hr class="bg-info" />
                                        <br />
                                        <div class="text-center badge-warning"><asp:literal ID="litDetalleCupon" runat="server">DETALLE DE CUPÓN</asp:literal></div>
                                        <div class="text-center">
                                            <asp:literal ID="litDetalleCantidad" runat="server">CANTIDAD:</asp:literal>
                                            <asp:Literal ID="litProductoCantidad" runat="server"></asp:Literal><br />
                                        </div>
                                        <div class="text-center">
                                            <asp:literal ID="litDetalleDescuento" runat="server">DESCUENTO:</asp:literal>
                                            <asp:Literal ID="litProductoDescuento" runat="server"></asp:Literal>%<br />
                                        </div>
                                        <div class="text-center">
                                           <asp:literal ID="litDetalleAhorro" runat="server"> AHORRO:</asp:literal>
                                            <asp:Literal ID="litProductoAhorroTotal" runat="server"></asp:Literal>$<br />
                                        </div>
                                        <div class="text-center">
                                            <asp:literal ID="litDetallePrecioFinal" runat="server">PRECIO FINAL:</asp:literal>
                                            <asp:Literal ID="litProductoPrecioFinal" runat="server"></asp:Literal>$<br />
                                        </div>
                                        <hr class="bg-warning" />
                                        <br />
                                        <div class="text-center badge-primary"><asp:literal ID="litDetalleRetiraPor" runat="server">RETIRA POR: </asp:literal></div>
                                        <div class="text-center">
                                            <br />
                                            <asp:Literal ID="litPuntoDeVenta" runat="server"></asp:Literal><br />
                                        </div>
                                        <hr class="bg-primary" />
                                        <br />
                                        <div class="text-center badge-danger"><asp:literal ID="litCuponInformacion" runat="server">CUPÓN - INFORMACIÓN</asp:literal></div>

                                        <div class="text-center">
                                            <br />
                                           <asp:literal ID="litCuponFechaGeneracion" runat="server"> CUPÓN - FECHA GENERACIÓN: </asp:literal>
                                               <br />
                                            <asp:Literal ID="litProductoFechaCupon" runat="server"></asp:Literal><br />
                                        </div>
                                        <div class="text-center">

                                            <asp:Literal ID="litProductoFechaCanje" runat="server">FECHA CANJE: </asp:Literal><br />
                                        </div>
                                        <%--<div class="text-center">Cupón:
                                            <asp:Literal ID="litProductoCupon" runat="server"></asp:Literal><br />
                                        </div>--%>
                                        <hr class="bg-danger" />
                                        <asp:Button ID="btnConfirmar" runat="server" CommandName="confirmar" CommandArgument='<%# Eval("CodCupon") %>' CssClass="btn btn-block btn-success animated infinite slow pulse text-light" Text="CONFIRMAR" />
                                        <br />
                                        <asp:Button ID="btnCancelar" runat="server" CommandName="cancelar" CommandArgument='<%# Eval("CodCupon") %>' CssClass="btn btn-block btn-danger text-light" Text="CANCELAR" />
                                    </div>
                                </div>

                            </ItemTemplate>
                        </asp:ListView>

                        <%--</div>--%>
                    </div>
                    <br />
                    <hr class="bg-dark" />
                    <div style="font-size: 25px" class="text-center">
                        <asp:DataPager ID="dpLvMisDescuentos" runat="server" PageSize="3" PagedControlID="LvMisDescuentos">
                            <Fields>
                                <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="badge badge-primary text-xl-center" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </div>
            </asp:Panel>
            <!--FIN DESCUENTOS A CONFIRMAR-->
            <asp:Panel ID="pnEstadisticasMiComercio" runat="server" Visible="false">
                <div class="container-fluid">
                    <div class="justify-content-md-center">
                        <br />
                        <h1 class="text-center font-italic">
                            <asp:Literal ID="EstadisticasTituloMiComercio" runat="server">MI COMERCIO- ESTADÍSTICAS</asp:Literal>
                            <img src="Recurso/Ico/diagram.png" width="64" /></h1>

                        <div class="container">
                            <div class="card-body">
                                <div class="text-center" style="background-color: #73c3ff">
                                    <img src="Recurso/Ico/store_white.png" width="40" class="animated fadeInLeft delay-1s" />
                                    <p class="text-center font-weight-bold text-light text-uppercase animated fadeIn delay-1s"><asp:Literal ID="litPuntosDeVenta" runat="server"></asp:Literal></p>
                                </div>
                                <div class="text-center" style="background-color: #50d39a">
                                    <img src="Recurso/Ico/shopping-bag_white.png" width="40" class="animated fadeInRight delay-2s" />
                                    <p class="text-center font-weight-bold text-light text-uppercase animated fadeIn delay-2s"><asp:literal Id="litProductos" runat="server"></asp:literal></p>
                                </div>
                                <div class="text-center" style="background-color: #6f42c1">
                                    <img src="Recurso/Ico/sale_white.png" width="40" class="animated fadeInUp delay-3s" />
                                    <p class="text-center font-weight-bold text-light text-uppercase animated fadeIn delay-3s"><asp:Literal ID="litCuponesPendientes" runat="server"></asp:Literal></p>
                                </div>
                            </div>
                        </div>

                        <!--DESCUENTOS A CONFIRMAR-->
                        <hr class="bg-light" />
                        <div class="jumbotron-fluid">
                            <h1 id="tituloDescuentosPorProducto" runat="server"></h1>
                            <div id="chartDescuentosPorProducto" style="height: 370px; max-width: 920px; margin: 0px auto;"></div>
                            <hr class="bg-dark" />
                            <br />
                            <h1 id="tituloCantidadProductosPorPuntoVenta" runat="server"></h1>
                            <div id="chartCantidadProductosPorPuntoVenta" style="height: 370px; max-width: 920px; margin: 0px auto;"></div>
                            <hr class="bg-dark" />
                            <br />
                            <h1 id="tituloEstadisticaCantidadCuponesCanjeadosPorFecha" runat="server"></h1>
                            <div id="chartEstadisticaCantidadCuponesCanjeadosPorFecha" style="height: 370px; max-width: 920px; margin: 0px auto;"></div>
                        </div>
                    </div>

                </div>
            </asp:Panel>

        </div>
    </div>




    <%--    </div>
    </div>--%>


    <%--  </div>
    <!-- /#page-content-wrapper -->

    </div>--%>
    <script src="Lib/CanvasJs/js/canvasjs.min.js"></script>

    <!-- Menu Toggle Script -->
    <script type="text/javascript">
        $(document).ready(function () {
            Estadisticas_DescuentosPorProducto();
            Estadisticas_CantProductosPorPuntoDeVenta();
            Estadistica_CantidadCuponesCanjeadosPorFecha();
        });

        function Estadisticas_DescuentosPorProducto() {
            $.ajax({
                type: "POST",                                              // tipo de request que estamos generando
                url: 'ComercioEstadisticas.aspx/DescuentosPorProducto',                    // URL al que vamos a hacer el pedido
                data: null,                                                // data es un arreglo JSON que contiene los parámetros que 
                // van a ser recibidos por la función del servidor
                contentType: "application/json",            // tipo de contenido
                dataType: "json",                                          // formato de transmición de datos
                async: true,                                               // si es asincrónico o no
                success: function (r) {

                    var data = eval(r.d);
                    var coordenadas = [];
                    for (var i = 0; i < data.length; i++) {
                        debugger;
                        coordenadas.push({
                            label: data[i].etiqueta,
                            y: parseInt(data[i].valor)
                        }); debugger;
                    }

                    var chart = new CanvasJS.Chart("chartDescuentosPorProducto", {
                        animationEnabled: true,
                        theme: "light2", // "light1", "light2", "dark1", "dark2"
                        exportEnabled: true,
                        animationEnabled: true,
                        //title: {
                        //    text: "DISTRIBUCIÓN DE PRODUCTOS CANJEADOS (POR UNIDAD)"
                        //},
                        
                        data: [{
                            type: "pie",
                            startAngle: 25,
                            //indexLabelFontColor: "yellow",
                            toolTipContent: "<b>{label}</b>: {y}",
                            showInLegend: "true",
                            legendText: "{label}",
                            indexLabelFontSize: 16,
                            indexLabel: "{label} - {y}",
                            dataPoints: coordenadas
                        }]
                    });
                    chart.render();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) { // función que va a ejecutar si hubo algún tipo de error en el pedido
                    var error = eval("(" + XMLHttpRequest.responseText + ")");
                    console.log(error.Message);
                }
            });
        }

        function Estadisticas_CantProductosPorPuntoDeVenta() {
            $.ajax({
                type: "POST",                                              // tipo de request que estamos generando
                url: 'ComercioEstadisticas.aspx/CantidadProductosPorPuntoVenta',                    // URL al que vamos a hacer el pedido
                data: null,                                                // data es un arreglo JSON que contiene los parámetros que 
                // van a ser recibidos por la función del servidor
                contentType: "application/json",            // tipo de contenido
                dataType: "json",                                          // formato de transmición de datos
                async: true,                                               // si es asincrónico o no
                success: function (r) {

                    var data = eval(r.d);
                    var coordenadas = [];
                    for (var i = 0; i < data.length; i++) {
                        debugger;
                        coordenadas.push({
                            label: data[i].etiqueta,
                            y: parseInt(data[i].valor)
                        }); debugger;
                    }

                    var chart = new CanvasJS.Chart("chartCantidadProductosPorPuntoVenta", {
                        animationEnabled: true,
                        exportEnabled: true,
                        //title: {
                        //    text: "CANTIDAD DE PRODUCTOS CANJEADOS POR PUNTO DE VENTA"
                        //},
                        axisX: {
                            interval: 1
                        },
                        axisY2: {
                            interlacedColor: "rgba(1,77,101,.2)",
                            gridColor: "rgba(1,77,101,.1)"
                        },
                        data: [{
                            type: "bar",
                            axisYType: "secondary",
                            color: "#014D65",
                            dataPoints: coordenadas
                        }]
                    });
                    chart.render();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) { // función que va a ejecutar si hubo algún tipo de error en el pedido
                    var error = eval("(" + XMLHttpRequest.responseText + ")");
                    console.log(error.Message);
                }
            });
        }

        function Estadistica_CantidadCuponesCanjeadosPorFecha() {
            $.ajax({
                type: "POST",                                              // tipo de request que estamos generando
                url: 'ComercioEstadisticas.aspx/EstadisticaCantidadCuponesCanjeadosPorFecha',                    // URL al que vamos a hacer el pedido
                data: null,                                                // data es un arreglo JSON que contiene los parámetros que 
                // van a ser recibidos por la función del servidor
                contentType: "application/json",            // tipo de contenido
                dataType: "json",                                          // formato de transmición de datos
                async: true,                                               // si es asincrónico o no
                success: function (r) {

                    var data = eval(r.d);
                    var coordenadas = [];
                    for (var i = 0; i < data.length; i++) {
                        debugger;
                        coordenadas.push({
                            label: data[i].etiqueta,
                            y: parseInt(data[i].valor)
                        }); debugger;
                    }

                    var chart = new CanvasJS.Chart("chartEstadisticaCantidadCuponesCanjeadosPorFecha", {
                        animationEnabled: true,
                        exportEnabled: true,
                        //title: {
                        //    text: "CANTIDAD DE CUPONES CANJEADOS POR FECHA"
                        //},
                        axisX: {
                            title: "CUPONES",
                            valueFormatString: "#0",
                            suffix: "",
                            prefix: ""
                        },
                        data: [{
                            type: "splineArea",
                            axisYType: "secondary",
                            color: "#014D65",
                            dataPoints: coordenadas
                        }]
                    });
                    chart.render();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) { // función que va a ejecutar si hubo algún tipo de error en el pedido
                    var error = eval("(" + XMLHttpRequest.responseText + ")");
                    console.log(error.Message);
                }
            });
        }


        var pfx = ["webkit", "moz", "MS", "o", ""];
        var x = document.getElementById("Mensajes");
        PrefixedEvent(x, "AnimationEnd", AnimationListener);

        function PrefixedEvent(element, type, callback) {
            for (var p = 0; p < pfx.length; p++) {
                if (!pfx[p]) type = type.toLowerCase();
                element.addEventListener(pfx[p] + type, callback, false);
            }
        }

        function AnimationListener() {
            $("#Mensajes").hide();
        }



        $(document).ready(function () {
            var nombreComercio = $("#<%= txtComercio.ClientID %>").val();
            if (nombreComercio != "MI COMERCIO") {
                $("#MiComercioNombre").text(nombreComercio);
                $("#MiComercioNombre").css("font-size", fontSizeDefault);
            }
            else {
                   $("#MiComercioNombre").text('');
            }


            $("#MisDatosArrowUp").removeAttr("hidden");
            $("#MisDatosArrowDown").attr("hidden", "hidden");

            $("#MiTelefonoArrowUp").removeAttr("hidden");
            $("#MiTelefonoArrowDown").attr("hidden", "hidden");

            $("#MiPuntoDeVentaArrowUp").removeAttr("hidden");
            $("#MiPuntoDeVentaArrowDown").attr("hidden", "hidden");
        });

        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            if ($("#wrapper").hasClass("toggled")) { $("#menu-toggle").html("<<"); }
            else { $("#menu-toggle").html(">>"); }
            $("#wrapper").toggleClass("toggled");
        });

        $("#<%=chkCelular.ClientID %>").on('click', function () {
            if ($(this).is(':checked')) {
                $("#Celular").css("background", "#25c475");
                $("#Celular").css("color", "#ffffff");

            }
            else {
                $("#Celular").css("background", "");
                $("#Celular").css("color", "#000000");
            }
        });

        $("#<%=ChkMantenerLogo.ClientID %>").on('click', function () {
            if ($(this).is(':checked')) {
                $("#MantenerLogo").css("background", "#25c475");
                $("#MantenerLogo").css("color", "#ffffff");

            }
            else {
                $("#MantenerLogo").css("background", "");
                $("#MantenerLogo").css("color", "#000000");
            }
        });

        $("#cardMisDatos").click(function (e) {
            if ($("#MisDatosArrowUp").is(":hidden")) {
                $("#MisDatosArrowUp").removeAttr("hidden");
                $("#MisDatosArrowDown").attr("hidden", "hidden");
            }
            else {
                $("#MisDatosArrowDown").removeAttr("hidden");
                $("#MisDatosArrowUp").attr("hidden", "hidden");
            }
        });

        $("#cardMiTelefono").click(function (e) {
            if ($("#MiTelefonoArrowUp").is(":hidden")) {
                $("#MiTelefonoArrowUp").removeAttr("hidden");
                $("#MiTelefonoArrowDown").attr("hidden", "hidden");
            }
            else {
                $("#MiTelefonoArrowDown").removeAttr("hidden");
                $("#MiTelefonoArrowUp").attr("hidden", "hidden");
            }
        });

        $("#cardMiPuntoDeVenta").click(function (e) {
            if ($("#MiPuntoDeVentaArrowUp").is(":hidden")) {
                $("#MiPuntoDeVentaArrowUp").removeAttr("hidden");
                $("#MiPuntoDeVentaArrowDown").attr("hidden", "hidden");
            }
            else {
                $("#MiPuntoDeVentaArrowDown").removeAttr("hidden");
                $("#MiPuntoDeVentaArrowUp").attr("hidden", "hidden");
            }
        });

        $("#<%= txtComercio.ClientID %>").on('input', function (e) {
            var nombre = $("#<%= txtComercio.ClientID %>").val();
            var fontSizeDefault = 18;
            if (nombre.length > 10 && nombre.length < 16) {
                fontSizeDefault = 14;
            }
            else if (nombre.length >= 16 && nombre.length < 30) {
                fontSizeDefault = 12;
            }
            else if (nombre.length >= 30 && nombre.length < 51) {
                fontSizeDefault = 10;
            }
            
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
