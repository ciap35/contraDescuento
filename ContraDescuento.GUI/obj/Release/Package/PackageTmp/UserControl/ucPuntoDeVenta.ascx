<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntoDeVenta.ascx.cs" Inherits="ContraDescuento.GUI.UserControl.PuntoDeVenta" %>





<div class="jumbotron">
     <!-- MENSAJES DE ERROR U OK-->
       <div class="container-fluid justify-content-center" id="Mensajes">
    <div id="divMensajeError" runat="server" visible="false" >
    <h4 class="alert-heading">¡Ups!</h4>
    <div class="alert alert-danger" role="alert">
        <asp:Literal ID="litMensajeError" runat="server" Visible="true"></asp:Literal>
    </div>
</div>

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
    
<!-- FIN MENSAJES DE ERROR U OK-->

    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="updPuntoDeVenta" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="container">
                   <div id="divErrorAltaPuntoDeVenta" runat="server" visible="false" class="alert alert-danger animated fadeInDown slower delay-1s ">
                        <h4 class="alert-heading">¡Ups!</h4>
                        <div class="alert alert-danger" role="alert">
                            <hr />
                            <p class="mb-0">
                                <asp:Literal ID="litAltaPuntoDeVentaError" runat="server" Visible="false"></asp:Literal>
                            </p>
                        </div>
                    </div>
                <div class="row">
                 
                    <div class="col-sm">
                        <h3 class="subtitle text-center"><asp:literal ID="litProvincia" runat="server">Provincia</asp:literal></h3>
                        <asp:DropDownList  ID="ddlProvincia" CausesValidation="false" AutoPostBack="true" DataTextField="Descripcion" DataValueField="CodProvincia" runat="server" CssClass="form-control" TabIndex="1" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm">
                        <h3 class="subtitle text-center"><asp:literal ID="litLocalidad" runat="server">Localidad</asp:literal></h3>
                        <asp:DropDownList ID="ddlLocalidad" CausesValidation="false" DataTextField="Descripcion" DataValueField="CodLocalidad" runat="server" CssClass="form-control" TabIndex="2" OnSelectedIndexChanged="ddlLocalidad_SelectedIndexChanged">
                          
                        </asp:DropDownList> 
                        <%--<asp:ListItem Text="-Seleccione una provincia-" Value="-1">                                
                            </asp:ListItem>--%>
                    </div>
                    <div class="col-sm">

                        <h3 class="subtitle text-center"><asp:literal ID="litCalle" runat="server">Calle</asp:literal></h3>
                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control" placeholder="..." MaxLength="50" TabIndex="3"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator ID="Regex_txtCalle" ForeColor="Red" runat="server" ControlToValidate="txtCalle"
                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" Enabled="false" />
                        <asp:RequiredFieldValidator ID="RfvTxtCalle" runat="server" ControlToValidate="txtCalle" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm">
                        <h3 class="subtitle text-center"><asp:literal ID="litNumero" runat="server">Nro</asp:literal></h3>
                        <div class="input-group mb-3">

                            <asp:TextBox ID="txtNumero" type="number" min="0" max="99999" runat="server" CssClass="form-control" placeholder="..." MaxLength="5" TabIndex="4"></asp:TextBox>

                        </div>
                        <asp:RegularExpressionValidator ID="Regex_txtNumero" ForeColor="Red" runat="server" ControlToValidate="txtNumero"
                            ValidationExpression="[0-9]*$" ErrorMessage="* Verifique" SetFocusOnError="true" ValidateRequestMode="Disabled" Enabled="false" />
                        <asp:RequiredFieldValidator ID="RfvTxtNumero" runat="server" ControlToValidate="txtNumero" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" ValidateRequestMode="Disabled" Enabled="false"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm">
                        <h3 class="subtitle text-center"><asp:literal ID="litPiso" runat="server">Piso</asp:literal></h3>
                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtPiso" type="number" min="0" max="999" runat="server" CssClass="form-control" placeholder="..." MaxLength="3" TabIndex="5"></asp:TextBox>

                        </div>
                        <asp:RegularExpressionValidator ID="Regex_txtPiso" ForeColor="Red" runat="server" ControlToValidate="txtPiso"
                            ValidationExpression="[0-9]*$" ErrorMessage="* Verifique" SetFocusOnError="true" ValidateRequestMode="Disabled" Enabled="false" />
                        <asp:RequiredFieldValidator ID="RfvTxtPiso" ValidateRequestMode="Disabled" runat="server" ControlToValidate="txtPiso" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" Enabled="false"></asp:RequiredFieldValidator>

                    </div>
                    <div class="col-sm">
                        <h3 class="subtitle text-center"><asp:literal ID="litDpto" runat="server">Dpto</asp:literal></h3>
                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtDpto" runat="server" CssClass="form-control" placeholder="..." MaxLength="10" TabIndex="6"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm">
                        <h3 class="subtitle text-center">&nbsp;</h3>
                        <div class="input-group mb-3">
                            <asp:Button ID="btnCrearPuntoDeVenta" runat="server" OnClick="btnCrearPuntoDeVenta_Click" Text="Crear" CssClass="btn btn-primary btn-block" TabIndex="7"/>
                        </div>
                    </div>
                </div>
                <!--Row -->
            </div>
            <!--Container-->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCrearPuntoDeVenta" EventName="click" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<!-- Jumbotron -->

<asp:UpdatePanel ID="updPuntoDeVentaRepeater" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Repeater ID="rptPuntoDeVenta" runat="server" OnItemCommand="rptPuntoDeVenta_ItemCommand" OnItemDataBound="rptPuntoDeVenta_ItemDataBound" Visible="true">


            <ItemTemplate>
                <div class="container">
                    <asp:Panel ID="panelValidarDatos" runat="server" Visible="false">
                        <div id="divValidarDatos" runat="server" visible="false">
                            <h4 class="alert-heading">¡Ups!</h4>
                            <div class="alert alert-danger" role="alert">
                                <asp:Literal ID="litErrorDatos" runat="server">Error</asp:Literal>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <asp:HiddenField ID="hdnPuntoDeVenta" runat="server" Value='<%# Eval("CodDomicilio") %>' />
                        <div class="col-sm">
                            <%--Provincia--%>
                            <asp:DropDownList AutoPostBack="true" ID="ddlProvinciaItem" Enabled="false" runat="server" DataTextField="Descripcion" DataValueField="CodProvincia" CssClass="form-control" TabIndex="8" OnSelectedIndexChanged="ddlProvinciaItem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm">
                            <asp:DropDownList ID="ddlLocalidadItem" Enabled="false" CausesValidation="false" DataTextField="Descripcion" DataValueField="CodLocalidad" runat="server" CssClass="form-control" TabIndex="9">
                             
                            </asp:DropDownList>
                               <%--<asp:ListItem Text="-Seleccione una provincia-" Value="-1">                                
                                </asp:ListItem>--%>
                        </div>

                        <div class="col-sm">
                            <%--Calle--%>
                            <div class="input-group mb-3">
                                <asp:TextBox Enabled="false" ID="txtCalleItem" runat="server" CssClass="form-control" placeholder="CALLE" MaxLength="50" TabIndex="10"></asp:TextBox>
                            </div>
                            <asp:RegularExpressionValidator ID="Regex_txtCalleItem" ForeColor="Red" runat="server" ControlToValidate="txtCalleItem"
                                ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" Enabled="false" />
                            <asp:RequiredFieldValidator ID="RfvTxtCalleItem" runat="server" ControlToValidate="txtCalleItem"
                                SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm">
                            <%--Nro--%>
                            <div class="input-group mb-3">

                                <asp:TextBox Enabled="false" ID="txtNumeroItem" type="number" min="0" max="99999" runat="server" CssClass="form-control" placeholder="NRO" MaxLength="5" TabIndex="11"></asp:TextBox>

                            </div>
                            <asp:RegularExpressionValidator ID="Regex_txtNumeroItem" ForeColor="Red" runat="server" ControlToValidate="txtNumeroItem"
                                ValidationExpression="[0-9]*$" ErrorMessage="* Verifique" SetFocusOnError="true" Enabled="false" />
                            <asp:RequiredFieldValidator ID="RfvTxtNumeroItem" runat="server" ControlToValidate="txtNumeroItem"
                                SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm">
                            <%--Piso--%>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtPisoItem" Enabled="false" type="number" min="0" max="999" runat="server" CssClass="form-control" placeholder="PISO" MaxLength="3" TabIndex="12"></asp:TextBox>

                            </div>
                            <asp:RegularExpressionValidator ID="Regex_txtPisoItem" ForeColor="Red" runat="server" ControlToValidate="txtPisoItem"
                                ValidationExpression="[0-9]*$" ErrorMessage="* Verifique" SetFocusOnError="true" Enabled="false" />
                            <asp:RequiredFieldValidator ID="RfvTxtPiso" runat="server" ControlToValidate="txtPisoItem"
                                SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm">
                            <%--Dpto--%>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtDptoItem" Enabled="false" runat="server" CssClass="form-control" placeholder="DPTO" MaxLength="10" TabIndex="13"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm1">
                            <%--Borrar Punto de Venta--%>
                            <div class="input-group mb-3">
                                <asp:LinkButton ID="lnkBorrarPuntoVenta" runat="server" CommandArgument='<%#Eval("CodDomicilio") %>' CommandName="BorrarDomicilio" Text="BORRAR"><img src="../Recurso/Ico/delete.png" width="32" /> </asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkEditarPuntoDeVenta" runat="server" CommandArgument='<%#Eval("CodDomicilio") %>' CommandName="EditarDomicilio" Text="EDITAR"><img src="../Recurso/Ico/edit.png" width="32" /> </asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkGuardarPuntoDeVenta" BorderStyle="Solid" BorderColor="#2e2e2e" runat="server" Visible="false" Enabled="false" OnCommand="lnkGuardarPuntoDeVenta_Command" CommandArgument='<%#Eval("CodDomicilio") %>' CommandName="EditarDomicilio" Text="Guardar"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkCancelarEdicion" BorderStyle="Solid" Font-Bold="true" ForeColor="DarkRed" BorderColor="#cc0000" runat="server" Visible="false" Enabled="false" Text="Cancelar" CommandArgument='<%#Eval("CodDomicilio") %>' CommandName="CancelarEdicion"></asp:LinkButton>
                            </div>
                        </div>

                    </div>
                    <!--Container-->
                </div>

                <!-- row -->
            </ItemTemplate>

        </asp:Repeater>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rptPuntoDeVenta" EventName="DataBinding" />
    </Triggers>
</asp:UpdatePanel>


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

</script>




