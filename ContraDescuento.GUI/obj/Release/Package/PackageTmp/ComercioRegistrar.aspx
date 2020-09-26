<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComercioRegistrar.aspx.cs" Inherits="ContraDescuento.GUI.ComercioRegistrar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Registro Comercio</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script src="Lib/JqueryUI/js/jquery-ui.js"></script>
    <link href="Lib/JqueryUI/css/jquery-ui.css" rel="stylesheet" />
   


    <div class="jumbotron" style="background-color:#5e5e5e;color: #ffffff"> <!--#353A85-->
        <h2 class="text-center" ID="TituloComercioRegistrar" runat="server">REGISTRO - COMERCIO</h2>
        <img src="Recurso/Imagen/personas.svg" width="350px" class="ComercioRegistrar_Logo " />
        <br />
        <div id="container">
            <div id="divMensajeError" runat="server" visible="false">
                <h4 class="alert-heading">¡Ups!</h4>
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>

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


        <br />
        <br />

        <hr style="background-color: #fff" />
        <h3 class="card-subtitle  text-uppercase" runat="server" id="MiComercio">MI COMERCIO <img src="Recurso/Ico/store.png" width="32" /></h3>
        <br />
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend  ">
                    <span class="input-group-text w-100 "><span class="align-content-md-center " aria-hidden="true" id="NombreComercio" runat="server">Nombre de comercio</span></span>
                </div>
                <asp:TextBox ID="txtComercioNombre" runat="server" CssClass="form-control" placeholder="..." MaxLength="50" TabIndex="1"></asp:TextBox>

            </div>

             <div class="input-group mb-3">
                <div class="input-group-prepend  ">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true" id="Logo" runat="server">Logo</span></span>
                </div>
                 <asp:FileUpload ID="fLogoUpload" runat="server" CssClass="btn btn-primary" accept="image/*"/>

            </div>
        </div>


        <hr style="background-color: #fff" />
        <h3 class="card-subtitle  text-uppercase" runat="server" id="DatosPersonalesUsuario">DATOS PERSONALES DEL ADMINISTRADOR DE NEGOCIO  <img src="Recurso/Ico/personal-card.png" width="38" /></h3>
        <br />
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true" id="nombreUsuario" runat="server">Nombre</span></span>
                </div>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="..." MaxLength="50" TabIndex="1"></asp:TextBox>

            </div>
            <asp:RegularExpressionValidator ID="Regex_txtNombre" ForeColor="Red" runat="server" ControlToValidate="txtNombre"
                ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RfvTxtNombre" runat="server" ControlToValidate="txtNombre" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text w-100" runat="server" id="Apellido">Apellido</span>
                </div>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control" placeholder="..." MaxLength="50" TabIndex="2"></asp:TextBox>
            </div>
            <asp:RegularExpressionValidator ID="Regex_txtApellido" ForeColor="Red" runat="server" ControlToValidate="txtApellido"
                ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RfvTxtApellido" runat="server" ControlToValidate="txtApellido" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend ">
                    <span class="input-group-text w-100" runat="server" id="Cumpleanios">Fecha de nacimiento</span>
                </div>
                <asp:TextBox ID="txtFechaDeNacimiento" runat="server" MaxLength="10" placeholder=" __ / __ / ____ " TabIndex="3"></asp:TextBox>

                &nbsp;&nbsp;&nbsp; 
                <label for="rdbtnHombre">
                    <img src="Recurso/Ico/men.png" /></label>
                <input type="radio" id="rdbtnHombre" runat="server" TabIndex="4" />
                &nbsp;
                    <label for="rdbtnMujer">
                        <img src="Recurso/Ico/women.png" /></label>
                <input type="radio" id="rdbtnMujer" runat="server" TabIndex="5"/><span id="rdbtnMujerMensaje"></span>&nbsp;&nbsp;<span id="rdbtnSexoMensaje"></span>
                <asp:RequiredFieldValidator ID="RfvTxtFechaDeNacimiento" runat="server" ControlToValidate="txtFechaDeNacimiento" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
            <br />

            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text w-100" runat="server" id="email">Email</span>
                </div>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="..." type="email" MaxLength="80" TabIndex="6"></asp:TextBox>
            </div>
            <asp:RegularExpressionValidator ID="Regex_txtEmail" ForeColor="Red" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <br />
            <asp:RequiredFieldValidator ID="RfvTxtEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100" id="password" runat="server">Password</span>
                </div>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20" TabIndex="7"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RfvTxtPassword" runat="server" ControlToValidate="txtPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPassword" ID="regTxtPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ForeColor="Red" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text w-100" runat="server" id="repetirPassword">Repita su password</span>
                </div>
                <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20"  TabIndex="8"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RfvTxtRepetirPassword" runat="server" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRepetirPassword" ID="regTxtRptPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
            <asp:CompareValidator ID="cpvPasswordMatch" runat="server" Operator="Equal" ControlToCompare="txtPassword" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Las contrasñeas no coinciden :("></asp:CompareValidator>
        </div>

        <br />
        <br />
        <hr style="background-color: #fff" />
        <br />
        <h3 class="card-subtitle text-uppercase" id="telefono" runat="server">Teléfono</h3>
        <br />
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text w-100" runat="server" id="Caracteristica">Característica</span>
            </div>
            <asp:TextBox ID="txtCaracteristica" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="10"  TabIndex="9"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regex_Caracteristica" runat="server" Enabled="true" ControlToValidate="txtCaracteristica" Display="Dynamic"  ValidationExpression="([+]?\d{1,2})" ForeColor="Red" SetFocusOnError="true" error="Código de país inválida"></asp:RegularExpressionValidator>
        </div>
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text w-100" id="Celular">
                  &nbsp;&nbsp; 
                    <asp:CheckBox ID="chkCelular" runat="server" Text="Celular" TextAlign="Left"  TabIndex="10" /></span>
            </div>
        </div>
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text w-100" id="numero" runat="server">Número</span>
            </div>
            <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="20" TabIndex="11"></asp:TextBox>
        </div>
         <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text w-100" id="observacion" runat="server">Observación</span>
            </div>
            <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control" placeholder="..." type="text" MaxLength="50" TabIndex="12"></asp:TextBox>
        </div>
         <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-180"><span class="align-content-md-center " aria-hidden="false" id="PreguntaSeguridad" runat="server">Pregunta de seguridad</span></span>
                </div>
                <asp:DropDownList ID="ddlPreguntaDeSeguridad" TabIndex="9" DataValueField="CodPreguntaSeguridad" DataTextField="Pregunta" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvDDLPreguntaDeSeguridad" runat="server" ControlToValidate="ddlPreguntaDeSeguridad" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic" ForeColor="Red" ErrorMessage="Seleccione una pregunta" SetFocusOnError="true"></asp:CompareValidator>
            </div>
            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="false" id="respuesta" runat="server">Respuesta</span></span>
                </div>
                <asp:TextBox ID="txtRespuestaRecuperarPassword" TabIndex="10" runat="server" CssClass="form-control" placeholder="..." MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtRespuestaRecuperarPassword" runat="server" ControlToValidate="txtRespuestaRecuperarPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
    <asp:Button ID="btnRegistrar" runat="server" Text="Registrarme" class="btn btn-block btn-primary" OnClick="btnRegistrar_Click" TabIndex="13" />
    </div>



    <script type="text/javascript">
        $(function () {
            $("#<%= txtFechaDeNacimiento.ClientID %>").datepicker({
                changeYear: true,
                dateFormat: "dd/mm/yy",
                yearRange: '1910:2019',
                maxDate: '0D'
            }).mask('99/99/9999');
        });


        $("#<%= btnRegistrar.ClientID %>").click(function () {
            var mujer = $("#<%=rdbtnMujer.ClientID%>").is(':checked');
            var hombre = $("#<%=rdbtnHombre.ClientID%>").is(':checked');
            if (mujer == false && hombre == false) {
                $("#rdbtnSexoMensaje").css("color", "#ff0000").html("* Ingrese");
            }
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


    </script>

</asp:Content>





<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
