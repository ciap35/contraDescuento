<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLogin.ascx.cs" Inherits="ContraDescuento.GUI.UserControl.ucLogin" %>


<div id="Formulario" class="modal-dialog" role="document">
    <div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title" id="LoginLabel">
                <asp:Literal ID="litTitleLoginModal" runat="server">Login</asp:Literal>
                   <asp:Literal ID="litTitleRecuperarPasswordModal" Visible="false" runat="server">Reestablecer mi password</asp:Literal>
            </h5>
        </div>
        <div class="modal-body">

            <div id="container-fluid">
                <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-2s ">

                    <div class="alert alert-danger" role="alert">
                        <%--<h4 class="alert-heading">¡Ups!</h4>--%>
                        <hr />
                        <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                        <asp:Button ID="btnReestablecerCuenta" CssClass="btn btn-success" Visible="false" runat="server" Text="Reactivar mi cuenta" OnClick="btnReestablecerCuenta_Click" />
                    </div>

                </div>
            </div>



            <div class="container-fluid">
                <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown  slower delay-2s ">
                    <div class="alert alert-success" role="alert">
                        <%--<h4>Status</h4>
                        <hr />--%>
                        <p class="mb-0">
                            <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                        </p>
                    </div>
                </div>
            </div>

            <div id="frmLogin" runat="server">


                <div class="form-group">
                    <label id="lblEmail" for="txtEmail" runat="server">Correo electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="..." type="email" required></asp:TextBox>
                </div>
                <div class="form-group">
                    <label id="lblPassword" for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="..." type="password" required></asp:TextBox>
                </div>
                <div class="form-group form-check">
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-xs-7">
                            <img width="24" height="24" src="../Recurso/Ico/padlock.png" />
                        </div>
                        <div class="col-xs-6">
                            <p class="text-muted form-text">

                                <asp:LinkButton ID="litRecordarPassword" runat="server" class="form-text text-muted" Style="font-size: 14px;" OnClick="litRecordarPassword_Click" OnClientClick="VerfrmRecuperarPassword();" ToolTip="¿Ha olvidado su contraseña?" Text="¿Ha olvidado su contraseña?">
                                </asp:LinkButton>
                            </p>
                        </div>

                    </div>
                </div>





                <br />
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" class="btn btn-primary" OnClick="btnIngresar_Click" />
            </div>

            <div id="frmRecuperarPassword" visible="false" runat="server">
                <div class="form-group">
                    <label id="lblEmailRecuperarPassword" for="txtEmailRecuperarPassword" runat="server">Correo electrónico</label>
                    <asp:TextBox ID="txtEmailRecuperarPassword" runat="server" CssClass="form-control" placeholder="..."></asp:TextBox>
                </div>
                <asp:Button ID="btnEmailRecuperarPassword" runat="server" Text="Envíame email" class="btn btn-primary" OnClick="btnReestablecerCuenta_Click" />
                <br />
            </div>

            <div id="frmRecuperarPasswordPreguntas" visible="false" runat="server">
                <div class="form-group">
                    <br />
                    <label id="lblPreguntaDeSeguridad" for="ddlPreguntaDeSeguridad" runat="server" class="text-uppercase">Pregunta de seguridad:</label>
                    <asp:DropDownList ID="ddlPreguntaDeSeguridad" DataValueField="CodPreguntaSeguridad" DataTextField="Pregunta" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <hr />
                    <label id="lblRecuperarPassword" for="txtEmailRecuperarPassword" runat="server" class="text-uppercase">Su email:</label>
                    <asp:TextBox ID="txtRespuestaEmailRecuperarPassword" runat="server" CssClass="form-control" TextMode="Email" placeholder="..." MaxLength="80"></asp:TextBox>
                    <hr />
                    <label id="lblRespuestaRecuperarPassword" for="txtRespuestaRecuperarPassword" runat="server" class="text-uppercase">Su respuesta:</label>
                    <asp:TextBox ID="txtRespuestaRecuperarPassword" runat="server" CssClass="form-control" placeholder="..." MaxLength="250"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnValidarPregunta" runat="server" Text="Validar" OnClick="btnValidarPregunta_Click" class="btn btn-primary text-uppercase btn-block" />
                    <br />
                </div>
            </div>
            <div id="frmChangePassword" runat="server" visible="false">
                <div class="form-group">
                    <br />
                    <hr />
                    <label id="lblPasswordNueva" for="txtPasswordNueva" runat="server" class="text-uppercase">Su nueva password:</label>
                    <asp:TextBox ID="txtPasswordNueva" runat="server" CssClass="form-control" placeholder="..." MaxLength="20" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RfvTxtPassword" runat="server" ControlToValidate="txtPasswordNueva" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPasswordNueva" ID="regTxtPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ForeColor="Red" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
                    <hr />
                    <label id="lbRepetirPassword" for="txtRepetirPassword" runat="server" class="text-uppercase">Confirme su password:</label>
                    <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" placeholder="..." MaxLength="20" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RfvTxtRepetirPassword" runat="server" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRepetirPassword" ID="regTxtRptPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ForeColor="Red" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cpvPasswordMatch" runat="server" Operator="Equal" ControlToCompare="txtPasswordNueva" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Las contraseñas no coinciden :("></asp:CompareValidator>
                    <br />
                    <br />
                    <asp:Button ID="btnCambiarPassword" runat="server" CssClass="btn btn-info text-uppercase btn-block" OnClick="btnCambiarPassword_Click" Text="Cambiar password" />
                </div>
            </div>
        </div>

    </div>
</div>



<script type="text/javascript">
    var pfx = ["webkit", "moz", "MS", "o", ""];
    var x = document.getElementById("Formulario");
    PrefixedEvent(x, "AnimationEnd", AnimationListener);

    function PrefixedEvent(element, type, callback) {
        for (var p = 0; p < pfx.length; p++) {
            if (!pfx[p]) type = type.toLowerCase();
            element.addEventListener(pfx[p] + type, callback, false);
        }
    }

    function AnimationListener() {
        //$("#container-fluid").hide();
    }

    $("#modal-dialog").modal({ "backdrop": "static" });

    $("#btnCerrar").click(function () {
        VerfrmLogin();
    });

    function VerfrmLogin() {
        $("#<%= frmRecuperarPassword.ClientID %>").hide();
        $("#<%= frmRecuperarPasswordPreguntas.ClientID%>").hide();
        $("#<%= frmLogin.ClientID%>").show();
    }

    function VerfrmRecuperarPassword() {
        $("#<%=frmRecuperarPasswordPreguntas.ClientID%>").hide();
        $("#<%=frmRecuperarPassword.ClientID%>").show();
        $("#<%=frmLogin.ClientID%>").hide();
    }
</script>

