<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UsuarioRegistrar.aspx.cs" Inherits="ContraDescuento.GUI.UsuarioRegistrar" %>




<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Registro</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script src="Lib/JqueryUI/js/jquery-ui.js"></script>
    <link href="Lib/JqueryUI/css/jquery-ui.css" rel="stylesheet" />


    <div class="jumbotron ">
        <h2 class="text-center text-uppercase" runat="server" id="tituloRegistro">Registro</h2>
        <img src="Recurso/Imagen/personas.svg" width="350px" class="ComercioRegistrar_Logo " />
        <div id="container">
            <div id="divMensajeError" runat="server" visible="false">
                <h4 class="alert-heading">¡Ups!</h4>
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>

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

        <br />
        <br />
        <hr style="background: #000; padding: 2px;" />
        <h3 class="card-subtitle" runat="server" id="DatosPersonales">DATOS PERSONALES</h3>
        <br />

        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend  " style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true" id="nombre" runat="server">Nombre</span></span>
                </div>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="..." MaxLength="50" TabIndex="1"></asp:TextBox>

            </div>
            <asp:RegularExpressionValidator ID="Regex_txtNombre" ForeColor="Red" runat="server" ControlToValidate="txtNombre"
                ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RfvTxtNombre" runat="server" ControlToValidate="txtNombre" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true" id="apellido" runat="server">Apellido</span></span>
                </div>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control" placeholder="..." MaxLength="50" TabIndex="2"></asp:TextBox>
            </div>
            <asp:RegularExpressionValidator ID="Regex_txtApellido" ForeColor="Red" runat="server" ControlToValidate="txtApellido"
                ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RfvTxtApellido" runat="server" ControlToValidate="txtApellido" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend " style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true"  id="fechaNacimiento" runat="server">Fecha de nacimiento</span></span>
                </div>
                <asp:TextBox ID="txtFechaDeNacimiento" runat="server" MaxLength="10" placeholder=" __ / __ / ____ " TabIndex="3"></asp:TextBox>

                &nbsp;&nbsp;&nbsp; 
                <label for="rdbtnHombre">
                    <img src="Recurso/Ico/men.png" /></label>
                <input type="radio" id="rdbtnHombre" runat="server" tabindex="4" />
                &nbsp;
                    <label for="rdbtnMujer">
                        <img src="Recurso/Ico/women.png" /></label>
                <input type="radio" id="rdbtnMujer" runat="server" tabindex="5" /><span id="rdbtnMujerMensaje"></span>&nbsp;&nbsp;<span id="rdbtnSexoMensaje"></span>
                <asp:RequiredFieldValidator ID="RfvTxtFechaDeNacimiento" runat="server" ControlToValidate="txtFechaDeNacimiento" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
            <br />

            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-100"  id="email" runat="server">Email</span>
                </div>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="..." type="email" MaxLength="80" TabIndex="6"></asp:TextBox>
            </div>
            <asp:RegularExpressionValidator ID="Regex_txtEmail" ForeColor="Red" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="* Verifique" SetFocusOnError="true" />
            <br />
            <asp:RequiredFieldValidator ID="RfvTxtEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>

            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text  w-100"><span class="align-content-md-center " aria-hidden="true"  id="password" runat="server">Password</span></span>
                </div>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20" TabIndex="7"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RfvTxtPassword" runat="server" ControlToValidate="txtPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPassword" ID="regTxtPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ForeColor="Red" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="true"  id="repitaSuPassword" runat="server">Repita su password</span></span>
                </div>
                <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20" TabIndex="8"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RfvTxtRepetirPassword" runat="server" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtRepetirPassword" ID="regTxtRptPassword" ValidationExpression="^[\s\S]{8,}$" runat="server" ForeColor="Red" ErrorMessage="Caract: min 8 - max 20"></asp:RegularExpressionValidator>
            <asp:CompareValidator ID="cpvPasswordMatch" runat="server" Operator="Equal" ControlToCompare="txtPassword" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Las contraseñas no coinciden :("></asp:CompareValidator>
            <br />
            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-180"><span class="align-content-md-center " aria-hidden="false"  id="preguntaSeguridad" runat="server">Pregunta de seguridad</span></span>
                </div>
                <asp:DropDownList ID="ddlPreguntaDeSeguridad" TabIndex="9" DataValueField="CodPreguntaSeguridad" DataTextField="Pregunta" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvDDLPreguntaDeSeguridad" runat="server" ControlToValidate="ddlPreguntaDeSeguridad" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic" ForeColor="Red" ErrorMessage="Seleccione una pregunta" SetFocusOnError="true"></asp:CompareValidator>
            </div>
            <div class="input-group mb-3">
                <div class="input-group-prepend" style="width: 180px!important">
                    <span class="input-group-text w-100"><span class="align-content-md-center " aria-hidden="false"  id="respuesta" runat="server">Respuesta</span></span>
                </div>
                <asp:TextBox ID="txtRespuestaRecuperarPassword" TabIndex="10" runat="server" CssClass="form-control" placeholder="..." MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtRespuestaRecuperarPassword" runat="server" ControlToValidate="txtRespuestaRecuperarPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>


        </div>
        <br />
        <br />
        <asp:Button ID="btnRegistrar" runat="server" Text="Registrame" class="btn btn-block btn-outline-primary" OnClick="btnRegistrar_Click" TabIndex="11" />
    </div>



    <script type="text/javascript">
        $(function () {
            $("#<%= txtFechaDeNacimiento.ClientID %>").datepicker({
                changeYear: true,
                dateFormat: "dd/mm/yy",
                yearRange: '-80:-18'
            }).mask('99/99/9999');
        });


        $("#<%= btnRegistrar.ClientID %>").click(function () {
            var mujer = $("#<%=rdbtnMujer.ClientID%>").is(':checked');
            var hombre = $("#<%=rdbtnHombre.ClientID%>").is(':checked');
            if (mujer == false && hombre == false) {
                $("#rdbtnSexoMensaje").css("color", "#ff0000").html("* Ingrese");
            }
        });

    </script>

</asp:Content>





<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
