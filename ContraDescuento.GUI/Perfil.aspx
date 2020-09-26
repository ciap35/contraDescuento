<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="ContraDescuento.GUI.Perfil" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="jumbotron">
        <div id="container">
            <div id="divMensajeError" runat="server" visible="false" class="animated fadeInDown slower delay-1s ">

                <div class="alert alert-danger" role="alert">
                    <h4 class="alert-heading">¡Ups!</h4>
                    <hr />
                    <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
                </div>

            </div>
        </div>



        <div class="container-fluid">
            <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeInDown slower delay-1s fadeOut delay-2s ">
                <div class="alert alert-success" role="alert">
                    <h4>Status</h4>
                    <hr />
                    <p class="mb-0">
                        <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>

        <div id="PanelPerfil" runat="server" visible="false">

            <div class="form-group">
                <h1 class="h1">PERFILES 
                    <asp:Button ID="btnAgregarPerfil" CssClass="btn btn-outline-primary" runat="server" OnClick="btnAgregarPerfil_Click" Text="AGREGAR NUEVO PERFIL" /></h1>
                <br />
                <br />
                <div class="form-group" id="frmPerfil" runat="server" visible="false">
                    <div class="input-group mb-3" runat="server">
                        <div class="input-group-prepend">
                            <span class="input-group-text">PERFIL</span>
                        </div>
                        &nbsp;&nbsp;
                            <asp:TextBox ID="txtPerfilNuevo" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="50"></asp:TextBox><asp:Button ID="btnGuardarPerfilNuevo" Visible="false" runat="server" CssClass="btn btn-primary" Text="CREAR" OnClick="btnGuardarPerfilNuevo_Click" />
                        <asp:TextBox ID="txtPerfil" runat="server" CssClass="form-control" Visible="false" placeholder="..." MaxLength="50"></asp:TextBox><asp:Button ID="btnGuardarPerfil" runat="server" Visible="false" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnGuardarPerfil_Click" />
                        <div class="input-group-prepend">
                            <span class="input-group-text"><span class="glyphicon glyphicon-search" aria-hidden="true">PERFIL PADRE</span></span>
                        </div>
                        <asp:DropDownList ID="ddlPerfiles" runat="server" DataValueField="CodPerfil" DataTextField="Descripcion">
                            <asp:ListItem Value='<%= Eval("CodPerfil") %>' Text='<%= Eval("Descripcion") %>'></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RegularExpressionValidator ID="Regex_txtPerfil" ForeColor="Red" runat="server" ControlToValidate="txtPerfilNuevo"
                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
                    </div>
                </div>


                <asp:GridView ID="gvPerfil" runat="server" AllowPaging="true" AutoGenerateColumns="False" OnPageIndexChanging="gvPerfil_PageIndexChanging" class="table table-dark" OnRowCommand="gvPerfil_RowCommand" OnRowDataBound="gvPerfil_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="CodPerfil" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="codPerfil" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de creación" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnVerPermisos" runat="server" CommandName="VerPermisos" CssClass="btn btn-outline-primary" Text="Ver Permisos" CommandArgument='<%# Eval("CodPerfil") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnEditarPerfil" runat="server" CommandName="EditarPerfil" CssClass="btn btn-outline-success" Text="Editar" CommandArgument='<%#Eval("CodPerfil") %>' />
                                <asp:HiddenField ID="hdnDescripcion" runat="server" Value='<%#Eval("Descripcion") %>' />
                                <asp:HiddenField ID="hdnPerfilPadre" runat="server" Value='<%#Eval("_Perfil.CodPerfil") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnBorrarPerfil" runat="server" CommandName="BorrarPerfil" CssClass="btn btn-outline-danger" Text="Borrar" CommandArgument='<%# Eval("CodPerfil") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
                </asp:GridView>
            </div>
        </div>





        <div class="jumbotron">

            <div id="divPermisos" runat="server" visible="false">
                <h1 class="h1">PERMISOS 
                <asp:Button ID="btnAgregarPermiso" runat="server" class="btn btn-outline-primary" OnClick="btnAgregarPermiso_Click" Text="AGREGAR NUEVO PERMISO" Visible="false" />
                </h1>

                <div class="form-group" id="frmPermiso" runat="server" visible="false">
                     <asp:HiddenField ID="hdnCodPermiso" runat="server" />
                    <asp:HiddenField ID="hdnCodPerfilPermiso" runat="server" />
                    <br />
                    <br />
                    <div class="input-group mb-3" runat="server" id="frmPermisoNUevo" visible="false">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><span class="glyphicon glyphicon-search" aria-hidden="true">NOMBRE PERMISO</span></span>
                        </div>
                        <asp:TextBox ID="txtPermisoNuevo" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>

                          <div class="input-group-prepend">
                            <span class="input-group-text"><span class="glyphicon glyphicon-search" aria-hidden="true">URL</span></span>
                        </div>
                        <asp:TextBox ID="txtURLNueva" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>


                        <asp:Button ID="btnNuevoPermiso" runat="server" CssClass="btn btn-primary" Text="CREAR" OnClick="btnNuevoPermiso_Click" />
                        <asp:RegularExpressionValidator ID="Regex_txtPermisoNuevo" ForeColor="Red" runat="server" ControlToValidate="txtPermisoNuevo"
                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
                    </div>
                    </div>

                    <div class="input-group mb-3" runat="server" id="frmPermisoEditar" visible="false">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><span class="glyphicon glyphicon-search" aria-hidden="true">PERMISO</span></span>
                        </div>
                        <asp:TextBox ID="txtPermisoEditar" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>

                          <div class="input-group-prepend">
                            <span class="input-group-text"><span class="glyphicon glyphicon-search" aria-hidden="true">URL</span></span>
                        </div>
                        <asp:TextBox ID="txtURL" runat="server" CssClass="form-control" placeholder="..." MaxLength="50"></asp:TextBox>

                        <asp:Button ID="btnPermisoGuardar" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnPermisoGuardar_Click" />
                        <asp:RegularExpressionValidator ID="Regex_txtPermisoGuardar" ForeColor="Red" runat="server" ControlToValidate="txtPermisoEditar"
                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="* Verifique" SetFocusOnError="true" />
                    </div>

    </div>

    <div class="form-group">
                    <asp:GridView ID="gvPermisos" runat="server" AllowPaging="true" AutoGenerateColumns="False" class="table table-dark" OnRowCommand="gvPermisos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodPermiso" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="CodPermiso" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="URL" HeaderText="URL" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de creación" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnActualizar" runat="server" CommandName="Editar" CssClass="btn btn-outline-success" Text="Editar" CommandArgument='<%# Eval("CodPermiso") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnBorrar" runat="server" CommandName="Borrar" CssClass="btn btn-outline-danger" Text="Borrar" CommandArgument='<%# Eval("CodPermiso") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>

            <%--<asp:Button ID="btnVerTreeView" runat="server" OnClick="btnVerTreeView_Click" CssClass="btn btn-info" Text="VER MODO JERARQUICO" />
            <asp:Button ID="btnOcultarTreeView" runat="server" OnClick="btnOcultarTreeView_Click" CssClass="btn btn-info" Text="OCULTAR MODO JERARQUICO" />--%>
            <div id="divTreeView" runat="server" visible="false">
                <asp:TreeView ID="tvPerfiles" runat="server"></asp:TreeView>
            </div>

        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
