<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grupos.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="ContraDescuento.GUI.Grupos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .Permiso {
            margin-left: 50px;
            /*display: inline;
    float: left;
    color: #000;
    cursor: pointer;
    text-indent: 20px;
    white-space: nowrap;*/
        }

        .Grupo {
            margin-left: 50px;
            /*display: inline;
    float: left;
    color: #000;
    cursor: pointer;
    text-indent: 20px;
    white-space: nowrap;*/
        }
    </style>

    <link href="Lib/Style/Main.css" rel="stylesheet" />
    <div class="jumbotron">

        <div id="divMensajeError" runat="server" visible="false" class="alert alert-error animated fadeInOut slow delay-10s">
            <%--<h4 class="alert-heading">¡Ups!</h4>--%>
            <div class="alert alert-danger" role="alert">
                <asp:Literal ID="litMensajeError" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>

        <div id="divMensajeOK" runat="server" visible="false" class="alert alert-success animated fadeIn delay-3s fadeOut delay-10s">
            <div class="alert alert-success" role="alert">
                <%--<h4>Status</h4>
                <hr />--%>
                <p class="mb-0">
                    <asp:Literal ID="litMensajeOk" runat="server" Visible="false"></asp:Literal>
                </p>
            </div>
        </div>
    </div>

    <div id="panelGrupos" runat="server">
        <div class="jumbotron">
            <h1 class="h1 text-center">
                <asp:Literal ID="litGrupoPermisoTitulo" runat="server">GRUPOS Y PERMISOS</asp:Literal>
            </h1>

            <asp:TreeView ID="tvGrupoPermiso" runat="server">
            </asp:TreeView>
            <br />
            <asp:Button ID="btnCollapseTreeViewGrupoPermiso" runat="server" Text="Contraer [-]" CssClass="btn btn-outline-primary btn-block" OnClick="btnCollapseTreeViewGrupoPermiso_Click" Visible="false" />
            <asp:Button ID="btnExpandTreeViewGrupoPermiso" runat="server" Text="Expandir [+]" CssClass="btn btn-outline-primary btn-block" OnClick="btnExpandTreeViewGrupoPermiso_Click" />



        </div>

        <div class="jumbotron">
            <h1 class="h1 text-center">
                <asp:Literal ID="litGrupoTitulo" runat="server">GRUPOS </asp:Literal>
                <asp:Button ID="btnGrupoNuevo" CssClass="btn btn-outline-primary" runat="server" OnClick="btnGrupoNuevo_Click" Text="AGREGAR NUEVO GRUPO" /></h1>
            <br />
            <br />
            <br />
            <div id="container">

                <div id="frmGrupoNuevoEditar" runat="server" visible="false">
                    <h1 class="h1">
                        <asp:Literal ID="litGrupoNuevoEditarTitulo" runat="server">NUEVO GRUPO</asp:Literal></h1>
                    <br />

                    <div class="form-group">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text w-100" id="nombreGrupo" runat="server">NOMBRE</span>
                            </div>
                            <asp:TextBox ID="txtGrupoNuevoEditarNombre" runat="server" CssClass="form-control" Visible="true" placeholder="..." MaxLength="50"></asp:TextBox>

                            <%--<asp:Button ID="btnGuardarNuevoGrupo" Visible="true" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnGuardarNuevoGrupo_Click" />--%>
                        </div>
                    </div>
                </div>

                <%-- <div class="input-group mb-3">
                        <div class="input-group-prepend">--%>

                <%--<asp:DropDownList ID="ddlGrupoSeleccionarPadre" runat="server" DataMember="CodGrupo" DataValueField="Descripcion"></asp:DropDownList>--%>
                <div id="frmPermisosPorGrupo" runat="server" visible="false">
                    <div class="col-sm text-uppercase badge badge-info" id="PermisosTitulo" runat="server">PERMISOS</div>
                    <div class="col-sm">
                        <div class="form-check">
                            <br />
                            <asp:CheckBoxList ID="chkListPermisos" DataValueField="CodPermiso" DataTextField="URL" runat="server" CssClass="form-check" Font-Size="Small" CellPadding="2" CellSpacing="2" RepeatColumns="4" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>

                <div id="frmGrupoAsignar" runat="server" visible="false">
                    <div class="col-sm text-uppercase badge badge-primary" runat="server" id="GruposHijo">GRUPOS (HIJO)</div>
                    <div class="col-sm-10">
                        <div class="form-check">
                            <div class="form-check">
                                <br />
                                <asp:CheckBoxList ID="chkGruposPadre" DataValueField="CodGrupo" DataTextField="Descripcion" runat="server" CssClass="form-check" Font-Size="Small" CellPadding="2" CellSpacing="2" RepeatColumns="5" RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>


                <asp:Button ID="btnEditarGrupo" Visible="true" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnEditarGrupo_Click" />
                <br />
                <hr />


                <asp:GridView ID="gvGrupos" runat="server"
                    OnPageIndexChanging="gvGrupos_PageIndexChanging" AllowPaging="true" PageSize="5" PagerStyle-HorizontalAlign="Center" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" PagerSettings-Position="TopAndBottom"
                    OnRowDataBound="gvGrupos_RowDataBound" OnRowCommand="gvGrupos_RowCommand" AutoGenerateColumns="false" class="table table-dark pagination-ys" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="CodGrupo" HeaderText="#" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de creación" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnEditarGrupo" runat="server" CommandName="GrupoEditar" CommandArgument='<%# Eval("CodGrupo") %>' CssClass="btn btn-primary" Text="Gestión de grupos" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnAdministrarPermisos" runat="server" CommandName="GrupoGestionPermisos" CssClass="btn btn-info" Text="Gestión de permisos" CommandArgument='<%# Eval("CodGrupo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnBorrarGrupo" runat="server" CommandName="GrupoBorrar" CssClass="btn btn-danger" Text="Borrar" CommandArgument='<%# Eval("CodGrupo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No existen datos cargados.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <hr />
            <asp:ImageButton ID="imgBtnExportExcelGrupo" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcelGrupo_Click" />
            <asp:ImageButton ID="imgBtnExportPDFGrupo" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDFGrupo_Click" />
            <br />
            <hr />

            <!-- PERMISOS-->
            <div class="jumbotron">


                <h1 class="h1 text-center">
                    <asp:Literal ID="litPermisoTitulo" runat="server">PERMISOS </asp:Literal>
                    <asp:Button ID="btnPermisoNuevo" CssClass="btn btn-outline-primary" runat="server" OnClick="btnPermisoNuevo_Click" Text="AGREGAR NUEVO PERMISO" /></h1>
                <br />
                <br />



                <div id="container">
                    <div id="frmPermisoNuevo" runat="server" visible="false">
                        <h1 class="h1">
                            <asp:Literal ID="litPermisoNuevoTitulo" runat="server">NUEVO PERMISO</asp:Literal></h1>
                        <br />

                        <div class="form-group">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text w-100" id="litNombrePermisoTitulo" runat="server">NOMBRE</span>
                                </div>
                                <asp:TextBox ID="txtPermisoNuevoNombre" runat="server" CssClass="form-control" Visible="true" placeholder="..." MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text w-100" id="litPaginaPermiso" runat="server">PÁGINA</span>
                                </div>
                                <asp:TextBox ID="txtPermisoNuevaPagina" runat="server" CssClass="form-control" Visible="true" placeholder="..." MaxLength="50"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnGuardarNuevoPermiso" Visible="true" runat="server" CssClass="btn btn-block btn-primary" Text="GUARDAR" OnClick="btnGuardarNuevoPermiso_Click" />
                            <br />
                            <hr />
                        </div>
                    </div>

                    <div id="frmPermisoEditar" runat="server" visible="false">
                        <h1 class="h1">
                            <asp:Literal ID="litPermisoEditarTitulo" runat="server">EDITAR PERMISO</asp:Literal></h1>
                        <br />

                        <div class="form-group">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text w-100" id="litNombrePermisoEditar" runat="server">NOMBRE</span>
                                </div>
                                <asp:TextBox ID="txtPermisoEditarNombre" runat="server" CssClass="form-control" Visible="true" placeholder="..." MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text w-100" id="litNombreEditar" runat="server">PÁGINA</span>
                                </div>
                                <asp:TextBox ID="txtPermisoEditarPagina" runat="server" CssClass="form-control" Visible="true" placeholder="..." MaxLength="50"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnEditarPermiso" Visible="true" runat="server" CssClass="btn btn-block btn-primary" Text="GUARDAR" OnClick="btnEditarPermiso_Click" />
                        </div>
                    </div>

                    <asp:GridView ID="gvPermisos" runat="server"
                        OnPageIndexChanging="gvPermisos_PageIndexChanging" AllowPaging="true" PageSize="5" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" PagerStyle-HorizontalAlign="Center" PagerSettings-Position="TopAndBottom"
                        OnRowDataBound="gvPermisos_RowDataBound" OnRowCommand="gvPermisos_RowCommand" AutoGenerateColumns="false" class="table table-dark pagination-ys" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="CodPermiso" HeaderText="#" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="URL" HeaderText="Pagina" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de creación" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnEditarGrupo" runat="server" CommandName="PermisoEditar" CommandArgument='<%# Eval("CodPermiso") %>' CssClass="btn btn-success" Text="Editar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnBorrarGrupo" runat="server" CommandName="PermisoBorrar" CssClass="btn btn-danger" Text="Borrar" CommandArgument='<%# Eval("CodPermiso") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No existen datos cargados.
                        </EmptyDataTemplate>
                    </asp:GridView>
                                <hr />
            <asp:ImageButton ID="imgBtnExportExcelPermiso" runat="server" ImageUrl="~/Recurso/Ico/excel.png" Width="24" OnClick="imgBtnExportExcelPermiso_Click" />
            <asp:ImageButton ID="imgBtnExportPDFPermiso" runat="server" ImageUrl="~/Recurso/Ico/pdf.png" Width="24" OnClick="imgBtnExportPDFPermiso_Click" />
                </div>
            </div>

        </div>
        <div class="jumbotron">
            <h1 class="h1 text-center">
                <asp:Literal ID="litAsignacionDeGruposAUsuarios" runat="server">ASIGNACIÓN DE GRUPOS A TIPOS DE USUARIOS</asp:Literal>
            </h1>

            <div class="form-group align-content-center">
                <div class="input-group mb-3">
                    <div class="input-group-prepend  " style="width: 180px!important">
                        <span class="input-group-text w-100 badge badge-primary"><span class="align-content-md-center " aria-hidden="true" id="litUsuariosAsignacion" runat="server">USUARIOS</span></span>
                    </div>
                    <asp:DropDownList ID="ddlGrupoUsuarios" runat="server" DataValueField="CodGrupo" DataTextField="Descripcion"></asp:DropDownList>

                </div>

                <div class="input-group mb-3">
                    <div class="input-group-prepend  " style="width: 180px!important">
                        <span class="input-group-text w-100 badge badge-primary"><span class="align-content-md-center " aria-hidden="true" id="litComerciosAsignacion" runat="server">COMERCIOS</span></span>
                    </div>
                    <asp:DropDownList ID="ddlGrupoComercio" runat="server" DataValueField="CodGrupo" DataTextField="Descripcion"></asp:DropDownList>

                </div>

                <div class="input-group mb-3">
                    <div class="input-group-prepend  " style="width: 180px!important">
                        <span class="input-group-text w-100 badge badge-primary"><span class="align-content-md-center " aria-hidden="true" id="litOtrosAsignacion" runat="server">OTROS</span></span>
                    </div>
                    <asp:DropDownList ID="ddlGrupoOtros" runat="server" DataValueField="CodGrupo" DataTextField="Descripcion"></asp:DropDownList>

                </div>
                <br />
                <asp:Button ID="btnGuardarMapeoUsuarios" runat="server" CssClass="btn btn-primary text-uppercase" Text="Guardar configuración" OnClick="btnGuardarMapeoUsuarios_Click" />
            </div>
        </div>
          <div style="display:none">
        <div class="jumbotron">
            <h1 class="h1 text-center">
                <asp:Literal ID="litADeGruposAUsuariosEsp" runat="server">ASIGNACIÓN DE GRUPOS A USUARIOS</asp:Literal> <%--litAsignacionDeGruposAUsuariosEspecifico--%>
            </h1>

          
            <asp:GridView ID="gvUsuarios" runat="server" 
                AllowPaging="true" AutoGenerateColumns="False" PagerStyle-HorizontalAlign="Center"
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" PagerSettings-Position="TopAndBottom"
                OnPageIndexChanging="gvUsuarios_PageIndexChanging" OnRowDataBound="gvUsuarios_RowDataBound" class="table table-dark pagination-ys" >
                <Columns>
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Nombre" DataField="Apellido" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"/>
                    <asp:TemplateField HeaderText="Tipo Usuario (ACTUAL)" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTipoUsuarioAntiguo" CssClass="btn btn-primary dropdown-toggle" runat="server" DataValueField="CodTipoUsuario" DataTextField="Descripcion"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Usuario (NUEVO)" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTipoUsuarioActual" CssClass="btn btn-primary dropdown-toggle" runat="server" DataValueField="CodTipoUsuario" DataTextField="Descripcion"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle ForeColor="Orange" HorizontalAlign="Center" />
                
            </asp:GridView>
            <br />
            <%--<asp:Button ID="btnGuardarCambios" runat="server" CssClass="btn btn-primary text-uppercase" Text="Guardar configuración" OnClick="" />--%>
        </div>
    </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
