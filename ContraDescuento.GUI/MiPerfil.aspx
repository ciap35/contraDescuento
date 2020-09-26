<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="ContraDescuento.GUI.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <title>CD - Mi Perfil</title>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script src="Lib/JqueryUI/js/jquery-ui.js"></script>
    <link href="Lib/JqueryUI/css/jquery-ui.css" rel="stylesheet" />


    <div class="jumbotron">
        <h2>Mi perfil</h2>
        <div id="container">
            <div id="divMensajeError" runat="server" visible="false">
                <%--<h4 class="alert-heading">¡Ups!</h4>--%>
                <div class="alert alert-danger" role="alert">
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>

            <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown ">
                <div class="alert alert-success" role="alert">
                   <%-- <h4>Status</h4>
                    <hr />--%>
                    <p class="mb-0">
                        <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>


        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100"><span class="glyphicon glyphicon-search" aria-hidden="true">Nombre</span></span>
                </div>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="Regex_txtNombre" ForeColor="Red" runat="server" ControlToValidate="txtNombre"
                    ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
                <asp:RequiredFieldValidator ID="RfvTxtNombre" runat="server" ControlToValidate="txtNombre" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Apellido</span>
                </div>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="Regex_txtApellido" ForeColor="Red" runat="server" ControlToValidate="txtApellido"
                    ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
                <asp:RequiredFieldValidator ID="RfvTxtApellido" runat="server" ControlToValidate="txtApellido" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Fecha de nacimiento</span>
                </div>
                <asp:TextBox ID="txtFechaDeNacimiento" runat="server" MaxLength="10" placeholder="__/__/____"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RfvTxtFechaDeNacimiento" runat="server" ControlToValidate="txtFechaDeNacimiento" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>


                &nbsp;&nbsp;&nbsp; 
                <label for="rdbtnHombre">
                    <img src="Recurso/Ico/men.png" /></label>
                <input type="radio" id="rdbtnHombre" runat="server" />
                &nbsp;
                    <label for="rdbtnMujer">
                        <img src="Recurso/Ico/women.png" /></label>
                <input type="radio" id="rdbtnMujer" runat="server" /><span id="rdbtnMujerMensaje"></span>&nbsp;&nbsp;<span id="rdbtnSexoMensaje"></span>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Email</span>
                </div>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="..." type="email" MaxLength="80"></asp:TextBox>
                <asp:RegularExpressionValidator ID="Regex_txtEmail" ForeColor="Red" runat="server" ControlToValidate="txtEmail"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="* Verifique" SetFocusOnError="true" />
                <asp:RequiredFieldValidator ID="RfvTxtEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Idioma</span>
                </div>
                <asp:DropDownList ID="ddlIdioma" runat="server">
                    <asp:ListItem Text="Español" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Ingles" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Password</span>
                </div>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20"></asp:TextBox>

            </div>
        </div>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Repita su password</span>
                </div>
                <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" placeholder="..." type="password" MaxLength="20"></asp:TextBox>

            </div>
        </div>
        <asp:CompareValidator ID="cpvPasswordMatch" runat="server" Operator="Equal" ControlToCompare="txtPassword" ControlToValidate="txtRepetirPassword" SetFocusOnError="true" ForeColor="Red" Display="Dynamic" ErrorMessage="Las contraseñas no coinciden :("></asp:CompareValidator>
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-110">Pregunta de seguridad</span>
                </div>
                <asp:DropDownList ID="ddlPreguntaDeSeguridad" style="margin-left:8px;" DataValueField="CodPreguntaSeguridad" DataTextField="Pregunta" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text  w-100">Su respuesta</span>
                    </div>
                    <asp:TextBox ID="txtRespuestaRecuperarPassword" runat="server" CssClass="form-control"  placeholder="..." MaxLength="250"></asp:TextBox>
                    <%--<br /><asp:RequiredFieldValidator ID="rfvRespuestaRecuperarPassword" runat="server" ControlToValidate="txtRespuestaRecuperarPassword" ErrorMessage="Por favor complete la respuesta de seguridad." SetFocusOnError="true" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </div>
            </div>        

        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" class="btn btn-block btn-success" OnClick="btnActualizar_Click" />
        <asp:Button ID="btnEliminar" runat="server" Text="Baja de cuenta" class="btn btn-block btn-danger" OnClick="btnEliminar_Click" />
    </div>



    <%--    </div>--%>

    <script type="text/javascript">
        $(function () {
            $("#<%= txtFechaDeNacimiento.ClientID %>").datepicker({
                changeYear: true,
                dateFormat: "dd/mm/yy",
                yearRange: '1910:2019',
                maxDate: '0D'
            }).mask('99/99/9999');
        });


        $("#<%= btnActualizar.ClientID %>").click(function () {

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
