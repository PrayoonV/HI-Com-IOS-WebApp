﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ServicePartList.aspx.cs" Inherits="HicomIOS.Master.ServicePartList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .swal-modal {
            width: 420px !important;
            height: 250px !important;
        }

        .swal-title {
            font-size: 18px !important;
        }

        .swal-overlay:before {
            height: 40% !important;
        }

        .swal-button {
            padding: 5px 15px;
        }

        .disabled {
            pointer-events: none;
        }

        .btn-addItem {
            margin-left: 5px;
            margin-right: 5px;
        }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <button type="button" onclick="addNewItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;New Model
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>

    <div id="gridCell">
        <dx:ASPxGridView ID="gridViewModel" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewModel"
            Width="100%" KeyFieldName="id"
            OnCustomCallback="gridViewModel_CustomCallback" OnPageIndexChanged="gridViewModel_PageIndexChanged" OnBeforeColumnSortingGrouping="gridViewModel_BeforeColumnSortingGrouping">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <dx:GridViewDataTextColumn Caption=" " FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editModelItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("model_name")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>|
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteModelItem(<%# Eval("id")%>)" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Copy" FieldName="id" CellStyle-HorizontalAlign="Center" Width="20px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnCopy" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="copyItem(<%# Eval("id")%>)" title="Copy">
                            <i class="fa fa-copy" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Package" FieldName="count_package" CellStyle-HorizontalAlign="Center" Width="25px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnPackage" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="viewPackage(this, <%# Eval("id")%>)" title="<%# Eval("model_name")%>">
                            <i class="fa fa-list" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Print" FieldName="id" CellStyle-HorizontalAlign="Center" Width="25px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnPrint" class="btn btn-mini" onclick="printModel(<%# Eval("id")%>)" title="Print">
                            <i class="fa fa-print" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="model_name" Caption="Model Name" Width="80px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="description_tha" Caption="Description" Width="110px" />

                <dx:GridViewDataTextColumn FieldName="count_package" Caption="จำนวน Package" Width="30px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="count_partlist" Caption="จำนวน PartList" Width="30px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="55px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                            data-toggle="button" aria-pressed="true" onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                            <div class="handle"></div>
                        </button>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="350" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>
    </div>

    <div class="modal fade" id="modal_add_model" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Maintenance Model
                </div>
                <div class="modal-body">
                    <ul class="nav nav-tabs">
                        <li class="active"><a data-toggle="tab" href="#detail">รายละเอียด</a></li>
                        <li><a data-toggle="tab" href="#remark">หมายเหตุ</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="detail" class="tab-pane fade in active">
                            <div class="row">&nbsp;</div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">Model Name :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" class="form-control" id="txtModelName" runat="server" />

                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">Description :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" class="form-control" id="txtModelDescription" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-4 form-group" style="margin-left: -30px;">
                                        <button type="button" runat="server" id="btnAddPartList" onclick="addPartListModel()"
                                            class="btn-addItem" style="margin-left: 15px;">
                                            เพิ่ม PartList</button>
                                    </div>
                                    <div class="col-xs-8 form-group" style="margin-left: -32px; padding-left: 7px">
                                        <input type="text" id="txtSearchPartListModel" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                            onkeypress="searchSeletedPartListModel(event.keyCode)" style="width: 270px;" />
                                        <button type="button" class="btn-addItem" id="btnSearchPartListModel" onclick="searchSeletedPartListModel(13);">
                                            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                        </button>
                                    </div>
                                </div>
                                <div class="col-xs-12" id="dvGridPartListModelView">
                                    <dx:ASPxGridView ID="gridViewPartListModel" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPartListModel"
                                        Width="100%" KeyFieldName="id"
                                        OnCustomCallback="gridViewPartListModel_CustomCallback" SettingsBehavior-AllowSort="false">
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                        <Paddings Padding="0px" />
                                        <Border BorderWidth="0px" />
                                        <BorderBottom BorderWidth="1px" />
                                        <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                            PageSizeItemSettings-Visible="true">
                                            <PageSizeItemSettings Items="10, 20, 50" />
                                        </SettingsPager>
                                        <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100px">
                                                <DataItemTemplate>
                                                    <a id="btnMoveUp" class="btn btn-mini" onclick="moveUpPartListModel(<%# Eval("id")%>)" title="MoveUp" <%# Container.ItemIndex == 0 ? " disabled=\"disabled\"" : "" %>>
                                                        <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                                    </a>|
                                                    <a id="btnMoveDown" class="btn btn-mini" onclick="moveDownPartListModel(<%# Eval("id")%>)" title="MoveDown">
                                                        <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                                    </a>|
                                                    <a id="btnEdit" class="btn btn-mini" onclick="editPartListModel(<%# Eval("id")%>)" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>|
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deletePartListModel(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i></a>

                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="item" Caption="Item" Width="50px" CellStyle-HorizontalAlign="Center" Settings-AllowSort="True" SortOrder="Ascending">
                                                <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <%--<dx:GridViewDataTextColumn FieldName="sort_no" Caption="Sort No" Width="50px" Visible="false" Settings-AllowSort="True" SortOrder="Ascending" />--%>
                                            <dx:GridViewDataTextColumn FieldName="no" Caption="No" Width="50px" CellStyle-HorizontalAlign="Center">
                                                <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="part_no" Caption="Part No" Width="100px" />


                                            <dx:GridViewDataTextColumn FieldName="part_name" Caption="Part Name" Width="200px" />
                                            <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" Width="30px" CellStyle-HorizontalAlign="Center">
                                                <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>

                                        </Columns>
                                        <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="250" />

                                        <SettingsPopup>
                                            <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                        </SettingsPopup>
                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
                                </div>

                            </div>
                        </div>
                        <div id="remark" class="tab-pane fade">
                            <div class="row">&nbsp;</div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">Remark :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" class="form-control" id="txtModelRemark" runat="server" />
                                        </div>
                                        <button type="button" class="btn-addItem" onclick="addModelRemark()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                        </button>
                                    </div>
                                    <div class="row form-group">
                                        <dx:ASPxGridView ID="gridModelRemark" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridModelRemark"
                                            Width="100%" KeyFieldName="id"
                                            OnCustomCallback="gridModelRemark_CustomCallback">
                                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                            <Paddings Padding="0px" />
                                            <Border BorderWidth="0px" />
                                            <BorderBottom BorderWidth="1px" />
                                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                PageSizeItemSettings-Visible="true">
                                                <PageSizeItemSettings Items="10, 20, 50" />
                                            </SettingsPager>
                                            <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="80px">
                                                    <DataItemTemplate>
                                                        <a id="btnEdit" class="btn btn-mini" onclick="editModelRemark(<%# Eval("id")%>)" title="Edit">
                                                            <i class="fa fa-pencil" aria-hidden="true"></i>
                                                        </a>|
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deleteModelRemark(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i></a>

                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="remark" Caption="Remark" />
                                            </Columns>
                                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="300" />
                                            <SettingsPopup>
                                                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                            </SettingsPopup>
                                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <div class="row form-group">
                        <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior="false" ID="btnSaveModel" Style="visibility: hidden" Text="Confirm" OnClick="btnSaveModel_Click"></dx:ASPxButton>
                        <button type="button" runat="server" id="btnSubmitPartListModel" onclick="submitModel()" class="btn-app btn-addItem">ยืนยัน</button>
                        <button type="button" runat="server" id="btnCancelPartListModel" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_select_part_list_model" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Select Part List
                </div>
                <div class="modal-body">
                    <div class="row">

                        <div class="gridCell">
                            <div class="col-xs-4 text-left">
                                <input id="chkselectAll" type="checkbox" class="" onclick="selectAll()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8 text-left">
                                <input type="text" id="txtSearchPartList" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedPartList(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchPartList" onclick="searchSeletedPartList(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewPartListSelected" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPartListSelected"
                                    Width="100%" KeyFieldName="id"
                                    OnHtmlDataCellPrepared="gridViewPartListSelected_HtmlDataCellPrepared"
                                    EnableCallBacks="true"
                                    OnCustomCallback="gridViewPartListSelected_CustomCallback">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkBox"
                                                    ProductId='<%# Eval("id")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkGetPartListModel(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="item_no" Caption="Item no" Width="100px" />--%>
                                        <dx:GridViewDataTextColumn FieldName="part_no" Caption="Part No" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="part_name_tha" Caption="Part Name" Width="400px" />


                                    </Columns>
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="320" />

                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                    </SettingsPopup>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-xs-12">
                        <div class="row form-group">

                            <button type="button" runat="server" id="Button7" onclick="submitPartListModel()" class="btn-app btn-addItem">ยืนยัน</button>
                            <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_edit_part_list_model" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Edit PartList Model
                </div>
                <div class="modal-body">

                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Item :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control numberic" id="txtPartModelItem" validate-data runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Part Name :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" id="txtPartModelName" runat="server" />

                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">No :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" id="txtPartModelNo" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Type :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" id="txtPartModelType" maxlength="10" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control numberic" id="txtPartModelQty" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row form-group">
                        <button type="button" runat="server" id="Button11" onclick="submitEditPartListModel()" class="btn-app btn-addItem">ยืนยัน</button>
                        <button type="button" runat="server" id="Button12" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_package" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Package List
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Model :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" readonly="" id="txtPackageModel" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <button type="button" runat="server" id="Button1" onclick="addPackage()"
                                    class="btn-addItem" style="margin-left: 15px;">
                                    เพิ่ม Package</button>
                            </div>
                            <div class="row form-group" style="margin-left: 0px;">
                                <input type="text" id="txtSearchViewPackage" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedViewPackage(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchViewPackage" onclick="searchSeletedViewPackage(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                        </div>
                        <div class="gridCell">
                            <dx:ASPxGridView ID="gridViewPackage" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPackage"
                                Width="100%" KeyFieldName="id"
                                OnCustomCallback="gridViewPackage_CustomCallback">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                <Paddings Padding="0px" />
                                <Border BorderWidth="0px" />
                                <BorderBottom BorderWidth="1px" />
                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                    PageSizeItemSettings-Visible="true">
                                    <PageSizeItemSettings Items="10, 20, 50" />
                                </SettingsPager>
                                <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="60px">
                                        <DataItemTemplate>
                                            <a id="btnEdit" class="btn btn-mini" onclick="editPackageItem(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                            </a>|
                                            <a id="btnDelete" class="btn btn-mini" onclick="deletePackageItem(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="count_year" Caption="Count Year" Width="50px" />
                                    <dx:GridViewDataTextColumn FieldName="item_of_part" Caption="Item of Part" Width="80px" />
                                    <dx:GridViewDataTextColumn FieldName="running_hours" Caption="Running Hours" Width="60px" />

                                    <dx:GridViewDataTextColumn FieldName="count_part_list" Caption="Count Part List" Width="70px" />
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="total_price" Caption="Total Price" Width="60px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="70px">
                                        <DataItemTemplate>
                                            <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                                                data-toggle="button" aria-pressed="true" onclick="changeStatusItemPackage(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                                                <div class="handle"></div>
                                            </button>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="320" />

                                <SettingsPopup>
                                    <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                </SettingsPopup>
                                <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                    CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-xs-12">
                        <div class="row form-group">
                            <button type="button" runat="server" id="Button2" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_add_package" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Package
                </div>
                <div class="modal-body">
                    <fieldset>
                        <legend>Header</legend>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Model :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" readonly="" id="txtPackageModelName" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Count Year :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtPackageCountYear" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Item of Part :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control " id="txtPackageItemOfPart" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Running Hours :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtPackageRunningHours" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Total Price :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" readonly="" id="txtPackageTotalPrice" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Service Charge :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtPackageServiceChange" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Overhaul :</label>
                                    <div class="col-xs-7 no-padding">
                                        <select class="form-control" runat="server" id="cbbOverhaul">
                                             <option value="0">Please Select</option>
                                            <option value="1">Overhaul Motor</option>
                                            <option value="2">Overhaul Motor + Fan Motor</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Overhaul Rate :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtOverhaulRate" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Description :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtDescriptionAdd" runat="server" />
                                    </div>
                                    <button type="button" class="btn-addItem" onclick="addModelDescription()">
                                        <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                    </button>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewDescriptionListPackage" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewDescriptionListPackage"
                                    Width="100%" KeyFieldName="id"
                                    OnCustomCallback="gridViewDescriptionListPackage_CustomCallback">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                                            <DataItemTemplate>
                                                <a id="btnEdit" class="btn btn-mini" title="Edit" onclick="editModelDescription(<%# Eval("id")%>)">
                                                    <i class="fa fa-pencil" aria-hidden="true"></i>
                                                </a>|
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deleteDescription(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                                <%-- <a id="btnMoveUp" class="btn btn-mini hidden" onclick="moveUpPartListPackage(<%# Eval("id")%>)" title="MoveUp">
                                                        <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                                    </a>
                                                    <a id="btnMoveDown" class="btn btn-mini hidden" onclick="moveDownPartListPackage(<%# Eval("id")%>)" title="MoveDown">
                                                        <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                                    </a>--%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="200px" />
                                    </Columns>
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="100" />
                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="50px" />
                                    </SettingsPopup>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </fieldset>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <button type="button" runat="server" id="Button3" onclick="addPartListPackage()"
                                    class="btn-addItem" style="margin-left: 15px;">
                                    เพิ่ม PartList</button>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <dx:ASPxGridView ID="gridViewPartListPackage" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPartListPackage"
                                Width="100%" KeyFieldName="id"
                                OnCustomCallback="gridViewPartListPackage_CustomCallback">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                <Paddings Padding="0px" />
                                <Border BorderWidth="0px" />
                                <BorderBottom BorderWidth="1px" />
                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                    PageSizeItemSettings-Visible="true">
                                    <PageSizeItemSettings Items="10, 20, 50" />
                                </SettingsPager>
                                <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                                        <DataItemTemplate>
                                            <a id="btnDelete" class="btn btn-mini" onclick="deletePartListPackage(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                            <a id="btnMoveUp" class="btn btn-mini hidden" onclick="moveUpPartListPackage(<%# Eval("id")%>)" title="MoveUp">
                                                <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                            </a>
                                            <a id="btnMoveDown" class="btn btn-mini hidden" onclick="moveDownPartListPackage(<%# Eval("id")%>)" title="MoveDown">
                                                <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="item_no" Caption="Item no" Width="50px" Visible="false" />
                                    <dx:GridViewDataTextColumn FieldName="sort_no" Caption="Sort no" Width="50px" />
                                    <dx:GridViewDataTextColumn FieldName="part_no" Caption="Part No" Width="80px" />
                                    <dx:GridViewDataTextColumn FieldName="part_name" Caption="Part Name" Width="150px" />


                                </Columns>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="200" />

                            </dx:ASPxGridView>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-xs-12">

                        <%--<dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnSavePackage" Style="visibility: hidden" Text="Confirm" OnClick="btnSavePackage_Click"></dx:ASPxButton>--%>
                        <button type="button" runat="server" id="Button4" onclick="submitPackage()" class="btn-app btn-addItem">ยืนยัน</button>

                        <button type="button" runat="server" id="Button5" class="btn-app btn-addItem" onclick="backToPackageList()">ยกเลิก</button>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_select_part_list_package" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Select Part List
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-4 text-left">
                            <input id="chkselectAllPartListPackage" type="checkbox" class="" onclick="SelectAllPartListPackage()" />&nbsp;เลือกทั้งหมด
                        </div>
                        <div class="col-xs-8 text-left">
                            <input type="text" id="txtSearchPartListPackage" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                onkeypress="searchSeletedPartListPackage(event.keyCode)" />
                            <button type="button" class="btn-addItem" id="btnSearchPartListPackage" onclick="searchSeletedPartListPackage(13);">
                                <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                            </button>
                        </div>
                        <div class="col-xs-12">
                            <dx:ASPxGridView ID="gridViewPartListPackageSelect" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPartListPackageSelect"
                                Width="100%" KeyFieldName="id"
                                OnHtmlDataCellPrepared="gridViewPartListPackageSelect_HtmlDataCellPrepared"
                                OnCustomCallback="gridViewPartListPackageSelect_CustomCallback">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                <Paddings Padding="0px" />
                                <Border BorderWidth="0px" />
                                <BorderBottom BorderWidth="1px" />
                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                    PageSizeItemSettings-Visible="true">
                                    <PageSizeItemSettings Items="10, 20, 50" />
                                </SettingsPager>
                                <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                        <DataItemTemplate>
                                            <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                TextField="Name" ValueField="is_selected" ClientInstanceName="chkBox"
                                                ProductId='<%# Eval("id")%>'>
                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkGetPartListPackage(s, e); 
                                                                    }" />
                                            </dx:ASPxCheckBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="item_no" Caption="Item no" Width="50px" />
                                    <dx:GridViewDataTextColumn FieldName="part_no" Caption="Part No" Width="50px" />
                                    <dx:GridViewDataTextColumn FieldName="part_name_tha" Caption="Part Name" Width="150px" />
                                    <%--<dx:GridViewDataTextColumn FieldName="description_tha" Caption="Description" Width="200px" />--%>
                                </Columns>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="320" />

                                <SettingsPopup>
                                    <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                </SettingsPopup>
                                <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                    CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                            </dx:ASPxGridView>
                        </div>

                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth"></label>
                                <div class="col-xs-7 no-padding text-right">
                                    <button type="button" runat="server" id="Button6" onclick="submitPartListPackage()" class="btn-app btn-addItem">ยืนยัน</button>
                                    <button type="button" runat="server" id="Button9" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_edit_model_remark_model" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Edit Remark
                </div>
                <div class="modal-body">

                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Remark :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" id="txtEditModelRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="hdEditModelRemarkId" />
                </div>

                <div class="modal-footer">
                    <div class="row form-group">
                        <button type="button" runat="server" id="Button10" onclick="submitModelRemark()" class="btn-app btn-addItem">ยืนยัน</button>
                        <button type="button" runat="server" id="Button13" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_edit_model_description_model" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Edit Description
                </div>
                <div class="modal-body">

                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Description :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" id="txtDescription" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="hdfDescriptionid" />
                </div>

                <div class="modal-footer">
                    <div class="row form-group">
                        <button type="button" runat="server" id="Button15" onclick="submitDescription()" class="btn-app btn-addItem">ยืนยัน</button>
                        <button type="button" runat="server" id="Button16" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdModelId" />
    <asp:HiddenField runat="server" ID="hdPackageId" />
    <asp:HiddenField runat="server" ID="hdPartListModelId" />
    <script type="text/javascript">
        $(document).ready(function () {
            var h = window.innerHeight;
            gridViewModel.SetHeight(h - 155);
        });

        $('#modal_add_model').on('shown.bs.modal', function () {
            $('.nav-tabs li:first-child > a').trigger('click');
        });
        $('#modal_package').on('hidden.bs.modal', function () {
            var txtSearch = $("#txtSearchBoxData").val();
            gridViewModel.PerformCallback(txtSearch.toString());
        });

        $("input[validate-data]").keyup(function () {
            if ($(this).val() != "") {
                $(this).parent().parent().removeClass("has-error");
            }
        });

        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {

            $("[name='is_enable']").bootstrapToggle();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };

            var height = dimensions.height - 152;

            console.log(height);
            gridViewModel.SetHeight(height);
        });

        function searchSeletedPartList(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchPartList").val();
                gridViewPartListSelected.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchSeletedPartListPackage(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchPartListPackage").val();
                gridViewPartListPackageSelect.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchSeletedPartListModel(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchPartListModel").val();
                gridViewPartListModel.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function searchSeletedViewPackage(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchViewPackage").val();
                gridViewPackage.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function addNewItem() {
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/NewModel",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#txtModelName').val("");
                    $('#txtModelDescription').val("");
                    $('#hdModelId').val(0);
                    $('#hdPackageId').val(0);
                    gridViewPartListModel.PerformCallback();
                    gridModelRemark.PerformCallback();

                    $('#modal_add_model').modal('show');
                },
                failure: function (response) {

                }
            });

        }

        function addPartListModel() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/GetPartListModel",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    gridViewPartListSelected.PerformCallback();
                    $('#modal_select_part_list_model').modal('show');
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });


            $.LoadingOverlay("hide");
        }

        function SelectAllPartListPackage() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkselectAllPartListPackage').is(':checked');
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SelectAllPartListPackage",
                data: '{selected : ' + selected + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListPackageSelect.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                }
            });
        }
        function selectAll() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkselectAll').is(':checked');
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SelectAll",
                data: '{selected : ' + selected + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListSelected.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                }
            });
        }

        function chkGetPartListModel(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SelectPartListModel",
                data: '{id: "' + key + '" , isSelected: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitPartListModel() {
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SubmitPartListModel",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //var data = response.d;
                    gridViewPartListModel.PerformCallback();
                    $('#modal_select_part_list_model').modal('hide');
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitModel() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            if ($('#txtModelName').val() == "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณาระบุชื่อ Model");
                return;
            }

            $('#btnSaveModel').click();

            $.LoadingOverlay("hide");
        }

        function editModelItem(element, e, index) {
            gridViewModel.FocusedRowIndex = index;

            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/GetModelDataEdit",
                data: "{id : '" + e + "', index: " + index + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtModelName').val(data.model_name);
                    $('#txtModelDescription').val(data.description_tha);
                    $('#hdModelId').val(data.id);
                    $('#modal_add_model .modal-header').html('Edit model : ' + element.title);
                    $('#modal_add_model').modal('show');
                    gridViewPartListModel.PerformCallback();
                    gridModelRemark.PerformCallback();
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function deleteModelItem(e) {
            var id = e;

            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeleteModel",
                            data: '{id:"' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                console.log(data.d);
                                if (data.d == 'success') {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                            location.reload()
                                        });
                                } else {
                                    swal("การลบข้อมูลผิดพลาด!", {
                                        icon: "error"
                                    });
                                }
                            }
                        });
                    }
                });
        }

        function editPartListModel(e) {
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/GetPartListModelDataEdit",
                data: "{id : '" + e + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtPartModelItem').val(data.item);
                    $('#txtPartModelNo').val(data.no);
                    $('#txtPartModelName').val(data.part_name);
                    $('#txtPartModelQty').val(data.qty);
                    $('#txtPartModelType').val(data.type);
                    $('#hdPartListModelId').val(e);

                    $('#modal_edit_part_list_model').modal('show');

                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitEditPartListModel() {
            if ($("#txtPartModelItem").val() == "") {
                $("#txtPartModelItem").parent().parent().addClass("has-error");
                $("#txtPartModelItem").focus();
                return false;
            }
            var item = $('#txtPartModelItem').val();
            var no = $('#txtPartModelNo').val();
            var qty = $('#txtPartModelQty').val();
            var type = $('#txtPartModelType').val();
            var id = $('#hdPartListModelId').val();
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SubmitPartListModelDataEdit",
                data: "{id : '" + id + "', item : '" + item + "', no : '" + no + "',  qty : '" + qty + "' , type : '" + type + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#txtPartModelItem').val("");
                    $('#txtPartModelNo').val("");
                    $('#txtPartModelName').val("");
                    $('#txtPartModelQty').val(0);
                    $('#txtPartModelType').val("");
                    $('#hdPartListModelId').val(0);

                    $('#modal_edit_part_list_model').modal('hide');

                    gridViewPartListModel.PerformCallback();
                },
                failure: function (response) {

                }
            });
        }

        function deletePartListModel(e) {
            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeletePartListModel",
                            data: "{id : '" + e + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                console.log(data.d);
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridViewPartListModel.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }
        // END MODEL FUNCTION

        function viewPackage(element, e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;

            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/ViewPackageList",
                data: "{model_id : '" + key + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //var data = response.d;
                    $('#hdModelId').val(key);
                    gridViewPackage.PerformCallback();
                    $('#txtPackageModel').val(response.d);
                    $('#txtPackageModelName').val(response.d);
                    $('#modal_package .modal-header').html("Package list : Model " + element.title);
                    $('#modal_package').modal("show");

                },
                failure: function (response) {
                    //alert(response.d
                    $.LoadingOverlay("hide");
                }
            });

            $.LoadingOverlay("hide");
        }

        function addPackage() {
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/NewPackage",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtPackageCountYear').val("");
                    $('#txtPackageItemOfPart').val("");
                    $('#txtPackageRunningHours').val("");
                    $('#txtPackageTotalPrice').val("");
                    $('#txtPackageServiceChange').val("");
                    $('#cbbOverhaul').val("0");
                    $('#txtOverhaulRate').val("");
                    $('#hdPackageId').val(0);
                    $('#modal_package').modal("hide");
                    $('#modal_add_package').modal("show");

                    gridViewPartListPackage.PerformCallback();
                    gridViewDescriptionListPackage.PerformCallback();
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function addPartListPackage() {
            $.LoadingOverlay("show", {
                zIndex: 9999

            });

            var model_id = $('#hdModelId').val();
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/ViewPartListPackage",
                data: "{model_id : '" + model_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    gridViewPartListPackageSelect.PerformCallback();
                    $('#modal_select_part_list_package').modal('show');
                },
                failure: function (response) {
                    //alert(response.d
                    $.LoadingOverlay("hide");
                }
            });

            $.LoadingOverlay("hide");
        }

        function chkGetPartListPackage(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SelectPartListPackage",
                data: '{id: "' + key + '" , isSelected: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitPartListPackage() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SubmitPartListPackage",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    gridViewPartListPackage.PerformCallback();
                    $('#modal_select_part_list_package').modal('hide');
                    $('#txtPackageTotalPrice').val(data);


                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response.d);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function editPackageItem(e) {
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/GetPackageDataEdit",
                data: "{id : '" + e + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtPackageCountYear').val(data.count_year);
                    $('#txtPackageItemOfPart').val(data.item_of_part);
                    $('#txtPackageRunningHours').val(data.running_hours);
                    $('#txtPackageTotalPrice').val(data.total_price);
                    $('#txtPackageServiceChange').val(data.service_charge);
                    $("#cbbOverhaul").val(data.overhaul_service);
                    $("#txtOverhaulRate").val(data.overhaul_rate);
                    $('#hdModelId').val(data.model_id);
                    $('#hdPackageId').val(e);
                    $('#modal_package').modal("hide");
                    $('#modal_add_package').modal("show");
                    gridViewPartListPackage.PerformCallback();
                    gridViewDescriptionListPackage.PerformCallback();
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitPackage() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            if ($('#txtPackageItemOfPart').val() == "") {
                alertWarning("กรุณาระบุ Item of Part");
                $.LoadingOverlay("hide");
            }
            else if ($('#txtPackageRunningHours').val() == "") {
                alertWarning("กรุณาระบุ Running Hours");
                $.LoadingOverlay("hide");
            }
            else {
                //$('#btnSavePackage').click();
                savePackage();
                $.LoadingOverlay("hide");
            }
        }
        function savePackage(e) {
            var parametersSavePackage = {
                Data: [
                    {

                        ModelId: $('#hdModelId').val(),
                        PackageId: $('#hdPackageId').val(),
                        PackageCountYear: $('#txtPackageCountYear').val(),
                        PackageItemOfPart: $('#txtPackageItemOfPart').val(),
                        PackageRunningHours: $('#txtPackageRunningHours').val(),
                        PackageServiceChange: $('#txtPackageServiceChange').val(),
                        Packageoverhaul_service: $("#cbbOverhaul").val(),
                        Packageoverhaul_servicedes: $('#cbbOverhaul option:selected').text(),
                        Packageoverhaul_rate: $('#txtOverhaulRate').val(),

                    }
                ]
            };
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SavePackage",
                data: JSON.stringify(parametersSavePackage),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "success") {

                        $('#modal_add_package').modal("hide");
                        gridViewPackage.PerformCallback();
                        $('#modal_package').modal("show");
                    }
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function deletePackageItem(e) {
            var id = e;
            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeletePackageItem",
                            data: "{id : '" + id + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridViewPackage.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }

        function deletePartListPackage(e) {
            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeletePartListPackage",
                            data: "{id : '" + e + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#txtPackageTotalPrice').val(data.d);
                                        gridViewPartListPackage.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }

        function backToPackageList() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = $('#hdModelId').val();

            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/ViewPackageList",
                data: "{model_id : '" + key + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //var data = response.d;
                    gridViewPackage.PerformCallback();

                    $('#modal_package').modal("show");
                    $('#modal_add_package').modal("hide");
                },
                failure: function (response) {
                    //alert(response.d
                    $.LoadingOverlay("hide");
                }
            });

            $.LoadingOverlay("hide");
        }

        function moveUpPartListModel(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/MoveUpPartListModel",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListModel.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function moveDownPartListModel(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/MoveDownPartListModel",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListModel.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function moveUpPartListPackage(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/MoveUpPartListPackage",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListPackage.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function moveDownPartListPackage(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/MoveDownPartListPackage",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartListPackage.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function addModelRemark() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var remark = $('#txtModelRemark').val();
            if (remark == "") {
                alertWarning("กรุณาระบุหมายเหตุ");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "ServicePartList.aspx/AddModelRemark",
                    data: '{remark: "' + remark + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#hdEditModelRemarkId').val(0);
                        $('#txtModelRemark').val("");

                        gridModelRemark.PerformCallback();

                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertWarning(response.d);

                        $.LoadingOverlay("hide");
                    }
                });
            }
        }
        function editModelRemark(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/EditModelRemark",
                data: '{id: ' + e + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdEditModelRemarkId').val(e);
                    $('#txtEditModelRemark').val(data.remark);
                    $('#modal_edit_model_remark_model').modal("show");

                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alertWarning(response.d);

                    $.LoadingOverlay("hide");
                }
            });

        }
        function deleteModelRemark(e) {
            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeleteModelRemark",
                            data: '{id: ' + e + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#hdEditModelRemarkId').val(0);
                                        gridModelRemark.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }
        function submitModelRemark() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var remark = $('#txtEditModelRemark').val();
            var key = $('#hdEditModelRemarkId').val();
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SubmitModelRemark",
                data: '{id: ' + key + ' , remark : "' + remark + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#hdEditModelRemarkId').val(0);
                    gridModelRemark.PerformCallback();
                    $('#modal_edit_model_remark_model').modal("hide");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alertWarning(response.d);

                    $.LoadingOverlay("hide");
                }
            });
        }
        function printModel(e) {
            var id = e;
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Part_List|" + id, "_blank");
        }

        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                txtSearch = $.trim(txtSearch);
                console.log(txtSearch);
                gridViewModel.PerformCallback(txtSearch.toString());
                $("#txtSearchBoxData").val(txtSearch);

                $.LoadingOverlay("hide");
            }
        }
        function changeStatusItemPackage(id, e) {
            var model_id = $('#hdModelId').val();
            var enable = e;
            var is_enable = 0;
            if (enable == "False") {
                is_enable = 1;
            }
            console.log("changestatusPackage==" + e);
            console.log("changestatusPackage==" + is_enable);
            $.ajax({
                type: 'POST',
                url: "Config.aspx/ChangeStatus",
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_maintenance_package"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        gridViewPackage.PerformCallback("model_id|" + model_id);

                    }
                }
            });
        }
        function changeStatusItem(id, e) {
            var enable = e;
            var is_enable = 0;
            if (enable == "False") {
                is_enable = 1;
            }
            console.log("changestatus==" + e);
            console.log("changestatus==" + is_enable);
            $.ajax({
                type: 'POST',
                url: "Config.aspx/ChangeStatus",
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_maintenance_model"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                        var txtSearch = $("#txtSearchBoxData").val();
                        gridViewModel.PerformCallback(txtSearch.toString());

                    }
                }
            });
        }

        function addModelDescription() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var desc = $('#txtDescriptionAdd').val();
            if (desc == "") {
                alertWarning("กรุณาระบุรายละเอียด");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "ServicePartList.aspx/AddDescription",
                    data: '{description: "' + desc + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#hdfDescriptionid').val(0);
                        $('#txtDescriptionAdd').val("");

                        gridViewDescriptionListPackage.PerformCallback();

                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertWarning(response.d);

                        $.LoadingOverlay("hide");
                    }
                });
            }
        }
        function editModelDescription(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/EditDescription",
                data: '{id: ' + e + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdfDescriptionid').val(e);
                    $('#txtDescription').val(data.description);
                    $('#modal_edit_model_description_model').modal("show");

                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alertWarning(response.d);

                    $.LoadingOverlay("hide");
                }
            });

        }
        function deleteDescription(e) {
            swal({
                title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "ServicePartList.aspx/DeleteDescription",
                            data: '{id: ' + e + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#hdfDescriptionid').val(0);
                                        gridViewDescriptionListPackage.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }
        function submitDescription() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var remark = $('#txtDescription').val();
            var key = $('#hdfDescriptionid').val();
            $.ajax({
                type: "POST",
                url: "ServicePartList.aspx/SubmitDescription",
                data: '{id: ' + key + ' , description : "' + remark + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#hdfDescriptionid').val(0);
                    gridViewDescriptionListPackage.PerformCallback();
                    $('#modal_edit_model_description_model').modal("hide");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alertWarning(response.d);

                    $.LoadingOverlay("hide");
                }
            });
        }

        function copyItem(id) {
             $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#hdModelId').val(0);
            $('#hdPackageId').val(0);

            $.ajax({
                type: 'POST',
                url: "ServicePartList.aspx/CopyModelData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    var data = result.d;
                    
                      $('#txtModelName').val(data.model_name);
                    $('#txtModelDescription').val(data.description_tha);

                    gridViewPartListModel.PerformCallback();
                    gridModelRemark.PerformCallback();

                    $("#modal_add_model .modal-header").html("คัดลอก Model");
                    $('#modal_add_model').modal('show');
                     $.LoadingOverlay("hide");
                }
            });
        }

    </script>
</asp:Content>
