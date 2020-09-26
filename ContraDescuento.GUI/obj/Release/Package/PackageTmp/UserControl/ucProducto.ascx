<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProducto.ascx.cs" Inherits="ContraDescuento.GUI.UserControl.Producto" %>


<script src="Lib/JqueryUI/js/jquery-ui.js"></script>
<link href="Lib/JqueryUI/css/jquery-ui.css" rel="stylesheet" />



<div class="jumbotron">
  
    
        
  <!-- MENSAJES DE ERROR U OK-->      
    <div class="container-fluid justify-content-center" id="Mensajes">
    <div id="divMensajeError" runat="server" visible="false" class="alert alert-danger animated fadeInDown fadeOut slower delay-5s ">
        <h4 class="alert-heading">¡Ups! <button type="button" class="close" data-dismiss="alert">&times;</button></h4>
        <div class="alert alert-danger" role="alert">
            <asp:Literal ID="litMensajeError" runat="server" Visible="true"></asp:Literal>
        </div>
    </div>

    <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown fadeOut slower delay-5s ">
        <div class="alert alert-success" role="alert">
            <h4><asp:literal ID="litTituloFelicidades" runat="server">¡FELICIDADES!</asp:literal> <button type="button" class="close" data-dismiss="alert">&times;</button></h4>
            <hr />
            <p class="mb-0">
                <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
            </p>
        </div>
    </div>
        </div>
    
<!-- FIN MENSAJES DE ERROR U OK-->


    <div class="container">





        <div id="divErrorAltaPuntoDeVenta" runat="server" visible="false" class="alert alert-danger animated fadeInDown slower delay-1s ">
            <h4 class="alert-heading">¡Ups!</h4>
            <div class="alert alert-danger" role="alert">
                <hr />
                <p class="mb-0">
                    <asp:Literal ID="litProductoError" runat="server" Visible="false"></asp:Literal>
                </p>
            </div>
        </div>

        <!--ABM de producto -->
        <div class="row">

            <!--Producto Card-->
            <div class="card" style="width: 100%">
                <%--style="width: 27rem;"--%>
                <%--<asp:UpdatePanel ID="updImagenProducto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                <p class="text-center"><asp:Image ID="imgProducto" runat="server" ImageUrl="../Recurso/Imagen/foto.png" alt="Foto de su producto" Width="200" Height="200" CssClass="card-img-top" />
                <asp:FileUpload ID="fpProductoFoto" runat="server" CssClass="btn btn-ligth text-center form-control btn btn-dark " />
                    </p>
                <span class="input-group-text w-350" id="MantenerLogo">&nbsp;&nbsp;
                        <asp:CheckBox ID="ChkMantenerLogo" name="MantenerLogo" runat="server" Text="MANTENER MI FOTO ACTUAL&nbsp;&nbsp;" TextAlign="Left" TabIndex="4" /></span>
                
                <asp:Button ID="btnCargarImagen" runat="server" Text="Cargar" CssClass="btn btn-primary btn-block" CausesValidation="false" OnClick="btnCargarImagen_Click" />

                <%--   </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCargarImagen" EventName="click" />
                            </Triggers>
                                </asp:UpdatePanel>--%>
                <div class="card-body">
                    <h5 class="card-title">
                        <asp:Literal ID="litABMProductoTitulo" runat="server">TÍTULO</asp:Literal></h5>
                    <asp:TextBox ID="txtProductoTitulo" runat="server" CssClass="form-control" placeholder="..."></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductoTitulo" runat="server" ControlToValidate="txtProductoTitulo" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>
                </div>
                <ul class="list-group list-group-flush">

                    <li class="list-group-item">
                        <h5 class="card-title">
                            <asp:Literal ID="litABMProductoCategoria" runat="server">CATEGORIA</asp:Literal></h5>
                        <asp:DropDownList ID="ddlProductoCategoria" runat="server" DataValueField="CodCategoria" CssClass="form-control" DataTextField="Descripcion" OnTextChanged="ddlProductoCategoria_TextChanged" AutoPostBack="true"></asp:DropDownList>
                    </li>

                    <asp:Panel ID="panelSubCategoria" runat="server" Visible="false">
                        <li class="list-group-item">
                            <h5 class="card-title">
                                <asp:Literal ID="litABMProductoSubCategoria" runat="server">SUB CATEGORIA</asp:Literal></h5>
                            <asp:DropDownList ID="ddlProductoSubCategoria" runat="server" DataValueField="CodSubCategoria" DataTextField="Descripcion" CssClass="form-control"></asp:DropDownList>
                        </li>
                    </asp:Panel>
                    <li class="list-group-item">
                        <h5 class="card-title">
                            <asp:Literal ID="litABMProductoPrecio" runat="server">PRECIO</asp:Literal></h5>
                        <asp:TextBox ID="txtProductoPrecio" runat="server" CssClass="form-control" placeholder="..." MaxLength="6"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductoPrecio" runat="server" ControlToValidate="txtProductoPrecio" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>
                    </li>

                    <li class="list-group-item">
                        <h5 class="card-title">
                            <asp:Literal ID="litABMDescuento" runat="server">DESCUENTO</asp:Literal></h5>
                        <asp:TextBox ID="txtProductoDescuento" runat="server" CssClass="form-control" placeholder="..." type="number" min="0" max="100" TextMode="Number" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductoDescuento" runat="server" ControlToValidate="txtProductoDescuento" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>
                        <asp:RangeValidator runat="server" ID="rngValidator" ControlToValidate="txtProductoDescuento" ErrorMessage="El descuento debe estar entre 0 y 100" ForeColor="Red" Display="Dynamic" MinimumValue="0" MaximumValue="100" Type="Double"></asp:RangeValidator>
                    </li>

                    <%--                    <li class="list-group-item">
                        <h5 class="card-title">
                            <asp:Literal ID="litABMCantidad" runat="server">CANTIDAD</asp:Literal></h5>
                        <asp:TextBox ID="txtProductoCantidad" runat="server" CssClass="form-control" placeholder="Cantidad" type="number" min="0" max="9999" MaxLength="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductoCantidad" runat="server" ControlToValidate="txtProductoCantidad" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>
                    </li>--%>
                </ul>
            </div>
            <!--Fin Producto Card-->



            <!--Punto de venta del  producto-->
            <div class="card" style="width: 100%">
                <!--Descripcion del  producto-->
                <div class="card-body">
                    <div class="card-body">
                        <h5 class="card-title text-upper">
                            <asp:Literal ID="litProductoDescripcion" runat="server">Descripción</asp:Literal></h5>
                        <p class="card-text">
                            <asp:TextBox ID="txtProductoDescripcion" runat="server" TextMode="MultiLine" Rows="6" MaxLength="500" Style="width: 100%; resize: none"></asp:TextBox>
                        </p>
                    </div>
                    <!--Fin Descripcion del  producto-->
                    <div class="card-body">
                        <a href="#" class="card-link  btn btn-block btn-primary" onclick="BorrarDescripcion()"><asp:literal ID="litLimpiarDescripcion" runat="server">LIMPIAR DESCRIPCION</asp:literal></a>
                    </div>
                    <ul class="list-group list-group-flush">
                        <li class="list-group-item">
                            <h5 class="card-title">
                                <asp:Literal ID="litABMFechaVigenciaDesde" runat="server">VIGENCIA DESDE</asp:Literal></h5>
                            <div class="input-group mb-3">
                                <div class="input-group-prepend ">
                                    <span class="input-group-text w-110">Fecha de vigencia desde</span>
                                    <asp:TextBox ID="txtFechaVigenciaDesde" runat="server" MaxLength="10" placeholder=" __ / __ / ____ "></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li class="list-group-item">
                            <h5 class="card-title">
                                <asp:Literal ID="litABMFechaVigenciaHasta" runat="server">VIGENCIA HASTA</asp:Literal></h5>
                            <div class="input-group mb-3">
                                <div class="input-group-prepend ">
                                    <span class="input-group-text w-110">Fecha de vigencia hasta</span>

                                    <asp:TextBox ID="txtFechaVigenciaHasta" runat="server" MaxLength="10" placeholder=" __ / __ / ____ "></asp:TextBox>

                                </div>
                            </div>
                        </li>
                    </ul>

                    <h5 class="card-title text-uppercase text-center">
                        <asp:Literal ID="litProductoPuntosDeVenta" runat="server">Puntos de Venta</asp:Literal></h5>
                    <p class="card-text">

                        <asp:UpdatePanel ID="updCheckPuntoDeVenta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <%--<asp:CheckBoxList ID="chkLstPuntoDeVenta" AutoPostBack="true" runat="server" DataValueField="codDomicilio" DataTextField="Descripcion" TextAlign="Right" OnSelectedIndexChanged="chkLstPuntoDeVenta_SelectedIndexChanged">
                                </asp:CheckBoxList>--%>

                                <asp:Repeater ID="rptPuntoDeVentaStock" runat="server" OnItemDataBound="rptPuntoDeVentaStock_ItemDataBound" OnItemCommand="rptPuntoDeVentaStock_ItemCommand">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnPuntoDeVenta" runat="server" />
                                      
                                        <li class="list-group-item">
                                            <h5 class="card-title">
                                                <asp:Literal ID="litABMCantidad" runat="server">CANTIDAD</asp:Literal></h5>
                                              <asp:CheckBox ID="chkSeleccion" runat="server" TextAlign="Right"  />
                                            <asp:TextBox ID="txtProductoCantidad" runat="server" Text="0" TextMode="Number" CssClass="form-control" placeholder="Cantidad" type="number" min="0" max="9999" MaxLength="4"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvProductoCantidad" runat="server" ControlToValidate="txtProductoCantidad" Display="static" ForeColor="DarkRed" ErrorMessage="*Requerido"></asp:RequiredFieldValidator>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rptPuntoDeVentaStock" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </p>
                </div>
               <%-- <div class="card-body">
                    <a href="#" class="card-link  btn btn-block btn-outline-info" onclick="Deseleccionar()">NINGUNO</a>
                    <br />
                    <a href="#" class="card-link  btn btn-block btn-info" onclick="SeleccionarTodos()">TODOS</a>
                </div>--%>

            </div>
            <!--Fin Punto de venta del  producto-->
            <br />
            <br />
            <br />
            <br />
            <div class="card-body">
                <asp:Button ID="btnCrearProducto" runat="server" Text="CREAR" class="btn btn-primary btn-block" OnClick="btnCrearProducto_Click" />
                <asp:Button ID="btnActualizarProducto" runat="server" Text="ACTUALIZAR" class="btn btn-success btn-block" OnClick="btnActualizarProducto_Click" />
                <asp:Button ID="btnBorrarProducto" runat="server" Text="BORRAR" class="btn btn-danger btn-block" OnClick="btnBorrarProducto_Click" />

                <asp:Button ID="btnProductoNuevo" runat="server" Text="CREAR NUEVO PRODUCTO" CssClass="btn btn-info btn-block" OnClick="btnProductoNuevo_Click" CausesValidation="false" />
                <%--Style="height: 500px; width: 80px"--%>
            </div>
        </div>
        <!--Fin ABM de producto -->
    </div>
    <!--Container-->
    <br />
    <br />
</div>

<div class="jumbotron">





    <h1 class="text-center">
        <asp:Literal ID="ucProducto_litTituloProducto" runat="server">PRODUCTOS</asp:Literal></h1>
    <br />
    <br />
    <hr class="bg-dark" />
    <asp:UpdatePanel ID="updProducto" runat="server">
        <ContentTemplate>
            <div class="">

                <asp:Repeater ID="rptProducto" runat="server" OnItemDataBound="rptProducto_ItemDataBound" OnItemCommand="rptProducto_ItemCommand">
                    <HeaderTemplate>
                        
                        <div class="row" >
                            <div class="col-1  text-center">
                                <asp:Literal ID="litFotoRepeater" runat="server">FOTO</asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litTituloRepeater" runat="server">TÍTULO</asp:Literal>
                            </div>

                            <div class="col-1 text-center">
                                <asp:Literal ID="litPrecioRepeater" runat="server">PRECIO</asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litDescuentoRepeater" runat="server">DESCUENTO</asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litCantidadRepeater" runat="server">CANT.</asp:Literal>
                            </div>

                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoCategoriaRepeater" runat="server">CATEGORIA</asp:Literal>
                            </div>

                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoSubCategoriaRepeater" runat="server">SUBCATEGORIA</asp:Literal>
                            </div>


                            <div class="col-1 text-center">
                                <asp:Literal ID="litFechaVigenciaDesdeRepeater" runat="server">VIG. DESDE</asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litFechaVigenciaHastaRepeater" runat="server">VIG. HASTA</asp:Literal>
                            </div>
                            <div class="col-2 text-center">
                                <asp:Literal ID="litPuntosDeVentaRepeater" runat="server">PTOS DE VENTA</asp:Literal>
                            </div>
                            <div class="col-1 text-center"></div>

                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnPuntoDeVenta" runat="server" Value='<%# Eval("CodProducto") %>' />
                        
                        <div class="row" ID="RowItem" runat="server">
                            <div class="col-1 text-center">
                                <asp:Image ID="thumbnailProducto" runat="server" Width="50" Height="50" />
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoTituloItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                $&nbsp;<asp:Literal ID="litProductoPrecioItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoDescuentoItem" runat="server"></asp:Literal>%
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoCantidadItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoCategoriaItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoSubCategoriaItem" runat="server"></asp:Literal>
                            </div>

                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoVigenciaDesdeItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-1 text-center">
                                <asp:Literal ID="litProductoVigenciaHastaItem" runat="server"></asp:Literal>
                            </div>
                            <div class="col-2 text-center">
                                <ul>
                                    <asp:Literal ID="litPuntosDeVentaItem" runat="server"></asp:Literal>
                                </ul>
                            </div>
                            <div class="col-1">
                                <asp:LinkButton ID="lnkSeleccionar" CausesValidation="false" runat="server" Text="" CommandName="Editar" CommandArgument='<%# Eval("CodProducto") %>'><img src="../Recurso/Ico/edit.png" width="24" /></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkBorrarProducto" CausesValidation="false" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("CodProducto") %>'> <img src="../Recurso/Ico/delete.png" width="24" /></asp:LinkButton>
                            </div>

                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <hr />
                    </SeparatorTemplate>
                </asp:Repeater>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rptProducto" EventName="DataBinding" />
        </Triggers>
    </asp:UpdatePanel>
</div>


<script type="text/javascript">

    
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
        $("#<%= txtProductoDescuento.ClientID %>").attr('maxlength', '4');
    });


    $("#<%= txtProductoDescuento.ClientID %>").keypress(function () {
        var text = $("#<%= txtProductoDescuento.ClientID %>").val();
        if (text.length > 3) {
            text = text.substring(0, 2);
            $("#<%= txtProductoDescuento.ClientID %>").val(text);
        }
    });


<%--    $("#<%= txtProductoCantidad.ClientID %>").keypress(function () {
        var text = $("#<%= txtProductoCantidad.ClientID %>").val();
        if (text.length > 4) {
            text = text.substring(0, 3);
            $("#<%= txtProductoCantidad.ClientID %>").val(text);
        }
    });--%>

    $(function () {
        $("#<%= txtFechaVigenciaDesde.ClientID %>").datepicker({
            changeYear: true,
            dateFormat: "dd/mm/yy",
            yearRange: '+0: +1',
            maxDate: '1Y',
            minDate: '0D'
        });


        $("#<%= txtFechaVigenciaHasta.ClientID %>").datepicker({
            changeYear: true,
            dateFormat: "dd/mm/yy",
            yearRange: '+0:+1',
            maxDate: '1Y',
            minDate: '0D'
        });
    });


    function BorrarDescripcion() {
        $("#<%= txtProductoDescripcion.ClientID%>").val("");
    }

    function Deseleccionar() {
        $('[id *= chkLstPuntoDeVenta]').find('input[type="checkbox"]').each(function () {
            $(this).prop("checked", false);
        });
    }

    function SeleccionarTodos() {
        $('[id *= chkLstPuntoDeVenta]').find('input[type="checkbox"]').each(function () {
            $(this).prop("checked", true);
        });
    }

    $("#<%=ChkMantenerLogo.ClientID %>").on('click', function () {
        if ($(this).is(':checked')) {
            $("#MantenerLogo").css("background", "#25c475");
            $("#MantenerLogo").css("color", "#ffffff");
            $("#<%= btnCargarImagen.ClientID%>").css("display", "none");
        }
        else {
            $("#MantenerLogo").css("background", "");
            $("#MantenerLogo").css("color", "#000000");
            $("#<%= btnCargarImagen.ClientID%>").css("display", "");
        }
    });

</script>

