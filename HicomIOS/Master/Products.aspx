﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="HicomIOS.Master.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            deleteSupplier padding: 0;
            text-align: right;
        }

        .modal .modal-dialog {
            top: -25px;
        }

        #tableSupplierList a i {
            font-size: 16px;
        }

        .swal-modal {
            width: 420px !important;
            height: 250px !important;
        }

        .swal-title {
            font-size: 15px !important;
        }

        .swal-overlay:before {
            height: 40% !important;
        }

        .swal-button {
            padding: 5px 15px;
        }

        .display-none {
            display: none;
        }

        .display-block {
            display: block;
        }

        .noclick {
            pointer-events: none;
            opacity: 0.6;
        }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
    </script>
    <button type="button" onclick="newItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มสินค้า
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
            Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridView_CustomCallback" OnPageIndexChanged="gridView_PageIndexChanged" OnBeforeColumnSortingGrouping="gridView_BeforeColumnSortingGrouping">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <dx:GridViewDataTextColumn Caption=" " FieldName="id" CellStyle-HorizontalAlign="Center" Width="25px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("product_no")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>

                        <%-- | <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("product_no")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>--%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Copy" FieldName="id" CellStyle-HorizontalAlign="Center" Width="20px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnCopy" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="copyItem(<%# Eval("id")%>)" title="Copy">
                            <i class="fa fa-copy" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" Width="50px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="product_name_tha" Caption="Product Name" Width="110px" />
                <dx:GridViewDataTextColumn FieldName="pressure" Caption="Pressure" Width="30px" />
                <dx:GridViewDataTextColumn FieldName="power_supply" Caption="Power Supply" Width="40px" />
                <dx:GridViewDataTextColumn FieldName="phase" Caption="Phase" Width="30px" />
                <dx:GridViewDataTextColumn FieldName="cat_name_tha" Caption="Category" Width="40px" />
                <dx:GridViewDataTextColumn FieldName="product_model" Caption="Model" Width="50px" />

                <%--<dx:GridViewDataTextColumn FieldName="quantity" Caption="Quantity" Width="30px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                 <dx:GridViewDataTextColumn FieldName="quantity_reserve" Caption="Reserve" Width="30px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                 <dx:GridViewDataTextColumn FieldName="quantity_balance" Caption="Total" Width="30px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>--%>

                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="55px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                            data-toggle="button" aria-pressed="true" onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                            <div class="handle"></div>
                        </button>
                        <%--<input type="checkbox" class="btn btn-left btn-toggle active  <%# (bool)Eval("is_enable") == false ? "" : "disabled" %>" name="is_enable" id="item_<%# Eval("id")%>" data-toggle="toggle" data-size="mini" data-onstyle="success" 
                            onchange="changeStatusItem(<%# Eval("id")%>)"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> />--%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>
    </div>
    <div class="modal fade" id="modal_form" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">เพิ่มสินค้า</div>
                </div>
                <div class="modal-body text-center">

                    <ul class="nav nav-tabs" style="margin-top: 0px;">
                        <li class="active"><a data-toggle="tab" href="#productList">Product</a></li>
                        <li><a data-toggle="tab" href="#quotationLine">Quotation Line</a></li>
                        <li><a data-toggle="tab" href="#supplier">Supplier</a></li>
                        <li><a data-toggle="tab" href="#mfg">MFG</a></li>

                    </ul>
                    <div class="tab-content" style="padding: 10px 0;">
                        <div id="productList" class="tab-pane fade in active">
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="txtProductNo">Product No:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control " id="txtProductNo" validate-data runat="server" />
                                    </div>
                                </div>
                                <%-- <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="txtSerialNo">Serial No:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtSerialNo" runat="server" />
                                    </div>
                                </div>--%>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Product Name(THA):</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtProductNameTHA" validate-data runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtProductNameENG">Product Name(ENG):</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtProductNameENG" validate-data runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="txtProductModel">Product Model:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtProductModel" validate-data runat="server" />
                                    </div>
                                </div>
                            </div>
                            <%--                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2 no-padding" for="txtDescriptionTHA">Description(THA):</label>
                                    <div class="col-xs-10">
                                        <input type="text" class="form-control" id="txtDescriptionTHA" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2 no-padding" for="txtDescriptionENG">Description(ENG):</label>
                                    <div class="col-xs-10">
                                        <input type="text" class="form-control" id="txtDescriptionENG" runat="server" />
                                    </div>
                                </div>
                            </div>--%>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="cboCateID">Categories:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboCateID" CssClass="form-control" runat="server" ClientInstanceName="cboCateID" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="cboUnitType">Unit Type:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboUnitType" CssClass="form-control" runat="server" ClientInstanceName="cboUnitType" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4" for="cboBrand">Brand:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboBrand" CssClass="form-control" runat="server" ClientInstanceName="cboBrand" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuantity">Quantity:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtQuantity" runat="server" disabled="disabled" value="0" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuantityReserve">Reserve:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityReserve" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuantityBalance">Total:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtQuantityBalance" runat="server" />
                                    </div>
                                </div>

                            </div>
                            <div class="row">

                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtMinSellingPrice">Min Selling Price :</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtMinSellingPrice" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtSellingPrice">Selling Price :</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtSellingPrice" runat="server" />
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtPressure">Pressure(MPa):</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtPressure" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtHz">Hz :</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtHz" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtPowerSupply">Power Supply:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtPowerSupply" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtPhase">Phase:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtPhase" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <%--   <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtUnitWarranty">Unit Warranty:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtUnitWarranty" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtAirEndWarranty">Air End Warranty:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtAirEndWarranty" runat="server" />
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtRemark">Remark :</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtRemark" runat="server" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div id="quotationLine" class="tab-pane fade">
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine1">Quotation Desc Line1:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine1" validate-data runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine2">Quotation Desc Line2:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine2" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine3">Quotation Desc Line3:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine3" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine4">Quotation Desc Line4:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine4" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine5">Quotation Desc Line5:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine5" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine6">Quotation Desc Line6:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine6" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine7">Quotation Desc Line7:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine7" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine8">Quotation Desc Line8:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine8" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine9">Quotation Desc Line9:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine9" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine10">Quotation Desc Line10:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine10" runat="server" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div id="supplier" class="tab-pane fade">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">Supplier</legend>
                                        <div class="form-group col-xs-12 no-padding">
                                            <label class="control-label col-xs-2" for="cboSupplier">Supplier</label>
                                            <div class="col-xs-5">
                                                <dx:ASPxComboBox ID="cboSupplier" CssClass="form-control" runat="server"
                                                    OnCallback="cboSupplier_Callback"
                                                    ClientInstanceName="cboSupplier" TextField="data_text"
                                                    ValueField="data_value">
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="col-xs-4 no-padding text-left">
                                                <button type="button" class="btn-addItem" onclick="submitSupplier()">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                                </button>
                                                <span id="lbValidateSupplier" style="color: #a94442; font-weight: 400; display: none;" runat="server">*กรุณาเลือก Supplier อย่างน้อย 1 รายการ</span>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-12 no-padding">
                                            <dx:ASPxGridView ID="supplierGrid" SettingsBehavior-AllowSort="false" ClientInstanceName="supplierGrid" runat="server"
                                                Settings-VerticalScrollBarMode="Visible"
                                                Settings-VerticalScrollableHeight="300"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="supplierGrid_CustomCallback"
                                                Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                                <Paddings Padding="0px" />
                                                <Border BorderWidth="0px" />
                                                <BorderBottom BorderWidth="1px" />
                                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                    PageSizeItemSettings-Visible="true">
                                                    <PageSizeItemSettings Items="10, 20, 50" />
                                                </SettingsPager>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="80">
                                                        <DataItemTemplate>
                                                            <a class="btn btn-mini" onclick="deleteSupplier(<%# Eval("id")%>)" title="Delete">
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                            </a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                    <dx:GridViewDataTextColumn FieldName="supplier_name" Caption="Supplier Name" CellStyle-HorizontalAlign="Left" Width="659" />
                                                    <dx:GridViewDataTextColumn FieldName="isactive" Caption="Status" CellStyle-HorizontalAlign="Left" Width="100" />
                                                </Columns>
                                                <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                                <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                                    CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                            </dx:ASPxGridView>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdProductId" />
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div id="mfg" class="tab-pane fade">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">MFG</legend>
                                        <div class="form-group col-xs-12 no-padding" style="display: none;">
                                            <label class="control-label col-xs-2" for="cboSupplier">MFG No</label>
                                            <div class="col-xs-3  no-padding-right">
                                                <input type="text" class="form-control" id="txtMFGNo" runat="server" />
                                            </div>
                                            <label class="control-label col-xs-2 no-padding" for="cboSupplier">Receive Date</label>
                                            <div class="col-xs-3">
                                                <input type="text" class="form-control" id="txtReceiveDate" runat="server" />
                                            </div>
                                            <div class="col-xs-2 no-padding text-left">
                                                <button type="button" class="btn-addItem" onclick="submitMFG()">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                                </button>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-12 no-padding">
                                            <dx:ASPxGridView ID="gridMFG" SettingsBehavior-AllowSort="false" ClientInstanceName="gridMFG" runat="server"
                                                Settings-VerticalScrollBarMode="Visible"
                                                Settings-VerticalScrollableHeight="300"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridMFG_CustomCallback"
                                                Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                                <Paddings Padding="0px" />
                                                <Border BorderWidth="0px" />
                                                <BorderBottom BorderWidth="1px" />
                                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                    PageSizeItemSettings-Visible="true">
                                                    <PageSizeItemSettings Items="10, 20, 50" />
                                                </SettingsPager>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="80">
                                                        <DataItemTemplate>
                                                            <a class="btn btn-mini" onclick="editMFG(<%# Eval("id")%>)" title="Edit">
                                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                                            </a>
                                                            <%--| <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteMFG(<%# Eval("id")%>, '<%# Eval("mfg_no")%>')" title="Delete">
                                                                 <i class="fa fa-trash-o" aria-hidden="true"></i></a>--%>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                    <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" CellStyle-HorizontalAlign="Left" Width="220" />
                                                    <dx:GridViewDataTextColumn FieldName="display_receive_date" Caption="Receive Date" CellStyle-HorizontalAlign="Left" Width="100" />
                                                    <dx:GridViewDataTextColumn FieldName="purchase_request_no" Caption="Reference Document ( IN )" CellStyle-HorizontalAlign="Left" Width="165">
                                                        <DataItemTemplate>
                                                            <a href="PurchaseRequest.aspx?dataId=<%# Eval("purchase_request_id")%>" target="_blank" title="<%# Eval("purchase_request_no")%>"><%# Eval("purchase_request_no")%></a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="display_issue_date" Caption="Issue Date" CellStyle-HorizontalAlign="Left" Width="90">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="issue_no" Caption="Reference Document(OUT)" CellStyle-HorizontalAlign="Left" Width="170">
                                                        <DataItemTemplate>
                                                            <a href="Issue.aspx?dataId=<%# Eval("issue_id")%>" target="_blank" title="<%# Eval("issue_no")%>"><%# Eval("issue_no")%></a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataTextColumn FieldName="id" Caption="Service Schedule" CellStyle-HorizontalAlign="Left" Width="120">
                                                        <DataItemTemplate>
                                                            <button type="button" id="btnSetting" class="btn-addItem btn-setting">
                                                                <i class="fas fa-calendar-check"></i>&nbsp;Setting
                                                            </button>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>--%>
                                                </Columns>
                                                <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                                <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                                    CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                            </dx:ASPxGridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn-addItem" onclick="createDummy()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;สร้างยอดยกมา
                    </button>--%>
                    <button type="button" class="btn-addItem" onclick="validateData()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="modal_edit_mfg" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">Edit MFG</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-6 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtEditMFGMFGNo">MFG No :</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control" id="txtEditMFGMFGNo" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-6 no-padding" id="divmfgdate" runat="server">
                            <label class="control-label col-xs-4 no-padding" for="txtEditMFGReceiveDate">Receive Date :</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control" id="txtEditMFGReceiveDate" runat="server" />
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdEditMFGID" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitEditMFG()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>



    <script type="text/javascript">
        var pageURL = "Products.aspx";

        $(document).ready(function () {
            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                //$(this).find('input').first().focus();
                $('.nav-tabs li:first-child > a').trigger('click');
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
                $("input[validate-data]").parent().parent().removeClass("has-error");
            });

            $('#txtReceiveDate').datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });
            var h = window.innerHeight;
            gridView.SetHeight(h - 155);
        });

        function searchDataGrid(e) {

            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                txtSearch = $.trim(txtSearch);
                gridView.PerformCallback(txtSearch.toString());
                $("#txtSearchBoxData").val(txtSearch);

                $.LoadingOverlay("hide");
            }
        }

        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            //console.log("asdasd");
            $("[name='is_enable']").bootstrapToggle();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };
            //console.log(dimensions);
            //var a = $('#gridView').length;
            var height = dimensions.height - 152;
            //document.getElementById('gridCell').setAttribute("style", "height:" + height + "px");
            console.log(height);
            gridView.SetHeight(height);
        });

        function newItem() {
            $("#hdProductId").val(0);
            $.ajax({
                type: 'POST',
                url: pageURL + "/NewProductData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    $('#txtProductNo').removeClass("noclick");
                    $("#txtProductNo").focus();
                    $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
                    cboUnitType.SetEnabled(true);
                    cboSupplier.PerformCallback();
                    supplierGrid.PerformCallback();
                    gridMFG.PerformCallback();

                    $("#modal_form .modal-title").html("เพิ่มสินค้า");
                    $("#modal_form").modal("show");

                    $("input[validate-data]").each(function () {
                        $(this).css("border-color", "")

                    });
                    cboCateID.GetMainElement().style.border = "";

                    cboBrand.GetMainElement().style.border = "";

                    cboUnitType.GetMainElement().style.border = "";
                }
            });

        }

        function editItem(element, id, index) {
            gridView.FocusedRowIndex = index;

            $("#hdProductId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditProductData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    $("#txtProductNameTHA").focus();

                    if (result.d["product_no"] != undefined && result.d["product_no"] != "") {
                        $('#txtProductNo').addClass("noclick");
                    }
                    else
                        $('#txtProductNo').removeClass("noclick");

                    $('#txtProductNo').val(result.d["product_no"]);
                    //$('#txtSerialNo').val(result.d["serial_no"]);


                    if (result.d["is_edit"] != undefined && result.d["is_edit"] == "1") {
                        cboUnitType.SetEnabled(false);
                    }
                    else {
                        cboUnitType.SetEnabled(true);
                    }

                    console.log("cat_id : " + result.d["cat_id"]);

                    if (result.d["cat_id"] == 0 || result.d["cat_id"] == "") {
                        //cboCateID.AddItem("--โปรดเลือก--", "0");
                        cboCateID.SetValue("0");
                    } else {
                        cboCateID.SetValue(result.d["cat_id"]);
                        if (cboCateID.GetSelectedItem() == null) {
                            cboCateID.SetValue("0");
                        }
                    }

                    if (result.d["brand_id"] == 0 || result.d["brand_id"] == "") {
                        //cboBrand.AddItem("--โปรดเลือก--", "0");
                        cboBrand.SetValue("0");
                    } else {
                        cboBrand.SetValue(result.d["brand_id"]);
                        if (cboBrand.GetSelectedItem() == null) {
                            cboBrand.SetValue("0");
                        }
                    }

                    $('#txtProductModel').val(result.d["product_model"]);
                    $('#txtProductNameTHA').val(result.d["product_name_tha"]);
                    $('#txtProductNameENG').val(result.d["product_name_eng"]);
                    $('#txtDescriptionTHA').val(result.d["description_tha"]);
                    $('#txtDescriptionENG').val(result.d["description_eng"]);
                    $('#txtQuotationDescLine1').val(result.d["quotation_desc_line1"]);
                    $('#txtQuotationDescLine2').val(result.d["quotation_desc_line2"]);
                    $('#txtQuotationDescLine3').val(result.d["quotation_desc_line3"]);
                    $('#txtQuotationDescLine4').val(result.d["quotation_desc_line4"]);
                    $('#txtQuotationDescLine5').val(result.d["quotation_desc_line5"]);
                    $('#txtQuotationDescLine6').val(result.d["quotation_desc_line6"]);
                    $('#txtQuotationDescLine7').val(result.d["quotation_desc_line7"]);
                    $('#txtQuotationDescLine8').val(result.d["quotation_desc_line8"]);
                    $('#txtQuotationDescLine9').val(result.d["quotation_desc_line9"]);
                    $('#txtQuotationDescLine10').val(result.d["quotation_desc_line10"]);
                    //$('#txtProductNo').val(result.d["stock_count_type"]);
                    $('#txtQuantity').val(result.d["quantity"]);
                    $('#txtQuantityReserve').val(result.d["quantity_reserve"]);
                    var total = parseFloat(result.d["quantity"]) - parseFloat(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val(total);
                    var color = "#555";
                    if (result.d["quantity_balance"] < 0) {
                        color = "red";
                    }
                    $('#txtQuantityBalance').css('color', color);
                    //$('#txtProductNo').val(result.d["min_qty"]);
                    if (result.d["unit_id"] == 0 || result.d["unit_id"] == "") {
                        //cboUnitType.AddItem("--โปรดเลือก--", "0");
                        cboUnitType.SetValue("0");
                    } else {
                        cboUnitType.SetValue(result.d["unit_id"]);
                        if (cboUnitType.GetSelectedItem() == null) {
                            cboUnitType.SetValue("0");
                        }
                    }
                    $('#txtSellingPrice').val(result.d["selling_price"]);
                    $('#txtMinSellingPrice').val(result.d["min_selling_price"]);
                    $('#txtRemark').val(result.d["remark"]);
                    $('#txtPressure').val(result.d["pressure"]);
                    $('#txtPowerSupply').val(result.d["power_supply"]);
                    $('#txtPhase').val(result.d["phase"]);
                    $('#txtHz').val(result.d["hz"]);
                    //$('#txtUnitWarranty').val(result.d["unit_warranty"]);
                    //$('#txtAirEndWarranty').val(result.d["air_end_warranty"]);

                    cboSupplier.PerformCallback();
                    supplierGrid.PerformCallback();
                    gridMFG.PerformCallback();



                    $("#modal_form .modal-title").html("แก้ไขสินค้า : " + element.title);

                    $("#modal_form").modal("show");

                }
            });
        }

        function deleteMFG(id, dataText) {
            swal({
                title: 'คุณต้องการลบข้อมูล "' + dataText + '" ใช่หรือไม่ ?',
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: pageURL + "/DeleteMFG",
                            data: "{id : " + id + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                $('#hdSelectedBorrowDetailId').val(0);
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridMFG.PerformCallback();

                                        $.LoadingOverlay("hide");
                                    });
                            },
                            failure: function (response) {
                            }
                        });
                    }
                });

        }
        function copyItem(id) {
            $("#hdProductId").val(0);
            $.ajax({
                type: 'POST',
                url: pageURL + "/CopyProductData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $('#txtProductNo').val(result.d["product_no"]);
                    //$('#txtSerialNo').val(result.d["serial_no"]);

                    console.log("cat_id : " + result.d["cat_id"]);

                    if (result.d["cat_id"] == 0 || result.d["cat_id"] == "") {
                        //cboCateID.AddItem("--โปรดเลือก--", "0");
                        cboCateID.SetValue("0");
                    } else {
                        cboCateID.SetValue(result.d["cat_id"]);
                    }

                    if (result.d["brand_id"] == 0 || result.d["brand_id"] == "") {
                        //cboBrand.AddItem("--โปรดเลือก--", "0");
                        cboBrand.SetValue("0");
                    } else {
                        cboBrand.SetValue(result.d["brand_id"]);
                    }

                    $('#txtProductModel').val(result.d["product_model"]);
                    $('#txtProductNameTHA').val(result.d["product_name_tha"]);
                    $('#txtProductNameENG').val(result.d["product_name_eng"]);
                    $('#txtDescriptionTHA').val(result.d["description_tha"]);
                    $('#txtDescriptionENG').val(result.d["description_eng"]);
                    $('#txtQuotationDescLine1').val(result.d["quotation_desc_line1"]);
                    $('#txtQuotationDescLine2').val(result.d["quotation_desc_line2"]);
                    $('#txtQuotationDescLine3').val(result.d["quotation_desc_line3"]);
                    $('#txtQuotationDescLine4').val(result.d["quotation_desc_line4"]);
                    $('#txtQuotationDescLine5').val(result.d["quotation_desc_line5"]);
                    $('#txtQuotationDescLine6').val(result.d["quotation_desc_line6"]);
                    $('#txtQuotationDescLine7').val(result.d["quotation_desc_line7"]);
                    $('#txtQuotationDescLine8').val(result.d["quotation_desc_line8"]);
                    $('#txtQuotationDescLine9').val(result.d["quotation_desc_line9"]);
                    $('#txtQuotationDescLine10').val(result.d["quotation_desc_line10"]);
                    //$('#txtProductNo').val(result.d["stock_count_type"]);
                    $('#txtQuantity').val("0");//(result.d["quantity"]);
                    $('#txtQuantityReserve').val("0");//(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val("0");//(result.d["quantity_balance"]);
                    //$('#txtProductNo').val(result.d["min_qty"]);
                    if (result.d["unit_id"] == 0 || result.d["unit_id"] == "") {
                        //cboUnitType.AddItem("--โปรดเลือก--", "0");
                        cboUnitType.SetValue("0");
                    } else {
                        cboUnitType.SetValue(result.d["unit_id"]);
                    }
                    $('#txtSellingPrice').val(result.d["selling_price"]);
                    $('#txtMinSellingPrice').val(result.d["min_selling_price"]);
                    $('#txtRemark').val(result.d["remark"]);
                    $('#txtPressure').val(result.d["pressure"]);
                    $('#txtPowerSupply').val(result.d["power_supply"]);
                    $('#txtPhase').val(result.d["phase"]);
                    $('#txtHz').val(result.d["hz"]);
                    //$('#txtUnitWarranty').val(result.d["unit_warranty"]);
                    //$('#txtAirEndWarranty').val(result.d["air_end_warranty"]);

                    cboSupplier.PerformCallback();
                    supplierGrid.PerformCallback();
                    gridMFG.PerformCallback();

                    $("#modal_form .modal-title").html("คัดลอกสินค้า");
                    $("#modal_form").modal("show");
                }
            });
        }

        function validateData() {
            if ($("#txtProductNameTHA").val() == "") {
                $("#txtProductNameTHA").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtProductNameTHA").focus();

                return false;
            } else if ($("#txtProductNameENG").val() == "") {
                $("#txtProductNameENG").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtProductNameENG").focus();

                return false;
            }
            else if ($("#txtProductModel").val() == "") {
                $("#txtProductModel").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtProductModel").focus();

                return false;
            }
            else if ($("#txtQuotationDescLine1").val() == "") {
                $("#txtQuotationDescLine1").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#quotationLine").addClass("active in");
                $('a[href*=quotationLine]').click();
                $("#txtQuotationDescLine1").focus();

                return false;
            }

            $.ajax({
                type: 'POST',
                url: pageURL + "/ValidateData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d != "") {
                        setTimeout(function () {
                            //cboSupplierSparepart.PerformCallback();
                            supplierGrid.PerformCallback();

                            //gridViewShelf.PerformCallback();
                            //cboShelf.PerformCallback();
                            $("#lbValidateSupplier").show();
                            $(".tab-pane.fade").removeClass("active in");
                            $("#supplier").addClass("active in");
                            $('a[href*=supplier]').click();
                            return false;
                        }, 100);
                    }
                    else {
                        submitForm();
                    }
                }
            });
        }

        function createDummy() {
            var productId = $("#hdProductId").val();
            var qty = $('#txtQuantity').val() == "" ? 0 : parseInt($('#txtQuantity').val());
            if (qty == 0) {
                alert('กรุณากรอกจำนวน');
            }
            $.ajax({
                type: 'POST',
                url: pageURL + "/SaveProductDataDummy",
                data: '{product_id: "' + productId + '", qty: ' + qty + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d == "success") {
                        alert("สร้างยอดสำเร็จ")
                    }
                }
            });
        }

        function submitForm() {
            var productId = $("#hdProductId").val();
            var parametersAdd = {
                productData: [
                    {
                        id: parseInt(productId),
                        product_no: $('#txtProductNo').val(),
                        //serial_no: $('#txtSerialNo').val(),
                        cat_id: (cboCateID.GetValue() ? parseInt(cboCateID.GetValue()) : 0),
                        brand_id: (cboBrand.GetValue() ? parseInt(cboBrand.GetValue()) : 0),
                        product_model: $('#txtProductModel').val(),
                        product_name_tha: $('#txtProductNameTHA').val(),
                        product_name_eng: $('#txtProductNameENG').val(),
                        description_tha: $('#txtDescriptionTHA').val(),
                        description_eng: $('#txtDescriptionENG').val(),
                        quotation_desc_line1: $('#txtQuotationDescLine1').val(),
                        quotation_desc_line2: $('#txtQuotationDescLine2').val(),
                        quotation_desc_line3: $('#txtQuotationDescLine3').val(),
                        quotation_desc_line4: $('#txtQuotationDescLine4').val(),
                        quotation_desc_line5: $('#txtQuotationDescLine5').val(),
                        quotation_desc_line6: $('#txtQuotationDescLine6').val(),
                        quotation_desc_line7: $('#txtQuotationDescLine7').val(),
                        quotation_desc_line8: $('#txtQuotationDescLine8').val(),
                        quotation_desc_line9: $('#txtQuotationDescLine9').val(),
                        quotation_desc_line10: $('#txtQuotationDescLine10').val(),
                        stock_count_type: "",
                        //$('#txtProductNo').val(result.d["stock_count_type"]);
                        quantity: $('#txtQuantity').val() == "" ? 0 : parseInt($('#txtQuantity').val()),
                        quantity_reserve: $('#txtQuantityReserve').val() == "" ? 0 : parseInt($('#txtQuantityReserve').val()),
                        quantity_balance: $('#txtQuantityBalance').val() == "" ? 0 : parseInt($('#txtQuantityBalance').val()),
                        //$('#txtProductNo').val(result.d["min_qty"]);
                        unit_id: (cboUnitType.GetValue() ? parseInt(cboUnitType.GetValue()) : 0),
                        selling_price: ($('#txtSellingPrice').val() == "" ? 0 : parseInt($('#txtSellingPrice').val())),
                        min_selling_price: ($('#txtMinSellingPrice').val() == "" ? 0 : parseInt($('#txtMinSellingPrice').val())),
                        remark: $('#txtRemark').val(),
                        is_enable: true,
                        is_delete: false,
                        pressure: $('#txtPressure').val(),
                        power_supply: $('#txtPowerSupply').val(),
                        phase: $('#txtPhase').val() == "" ? 0 : parseInt($('#txtPhase').val()),
                        hz: $('#txtHz').val() == "" ? 0 : parseInt($('#txtHz').val()),
                        //unit_warranty: $('#txtUnitWarranty').val(),
                        //air_end_warranty: $('#txtAirEndWarranty').val(),
                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: pageURL + "/SaveProductData",
                data: JSON.stringify(parametersAdd),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d == "success") {
                        window.location.reload();
                    }
                    else {
                        alertError(result.d);
                    }
                }
            });
        }

        function deleteItem(id, dataText) {
            swal({
                title: 'คุณต้องการลบข้อมูล "' + dataText + '" ใช่หรือไม่ ?',
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: pageURL + "/DeleteProduct",
                            data: '{id: ' + id + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d) {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                            window.location.reload();
                                        });
                                }
                            }
                        });
                    }
                });
        }


        function submitSupplier() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var value = cboSupplier.GetValue();
            var name = cboSupplier.GetText();
            if (value == "" || value == null) {
                alertWarning("กรุณาเลือกผู้จัดจำหน่าย");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "Products.aspx/SelectedSupplier",
                    data: '{id: "' + value + '",name : "' + name + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "") {
                            $("#lbValidateSupplier").hide();
                            cboSupplier.PerformCallback();
                            supplierGrid.PerformCallback();
                        } else {
                            alertWarning("เลือกผู้จัดจำหน่าย " + name + " แล้ว");
                        }

                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertError(response.d);
                    }
                });
            }
        }

        function deleteSupplier(e) {
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
                            url: "Products.aspx/DeleteSupplier",
                            data: '{id:"' + e + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        cboSupplier.PerformCallback();
                                        supplierGrid.PerformCallback();
                                    });

                            }
                        });
                    }
                });
        }

        function submitSupplier() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var value = cboSupplier.GetValue();
            var name = cboSupplier.GetText();
            if (value == "" || value == null) {
                alertWarning("กรุณาเลือกผู้จัดจำหน่าย");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "Products.aspx/SelectedSupplier",
                    data: '{id: "' + value + '",name : "' + name + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "") {
                            $("#lbValidateSupplier").hide();
                            cboSupplier.PerformCallback();
                            supplierGrid.PerformCallback();
                        } else {
                            alertWarning("เลือกผู้จัดจำหน่าย " + name + " แล้ว");
                        }

                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertError(response.d);
                    }
                });
            }

        }

        function submitMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            var mfg_no = $('#txtMFGNo').val();
            var receive_date = $('#txtReceiveDate').val();

            if (mfg_no == "" || mfg_no == undefined) {
                $.LoadingOverlay("hide");
                alertWarning("กรุณากรอก MFG");
                return;
            }

            $.ajax({
                type: "POST",
                url: "Products.aspx/AddMFG",
                data: '{mfg_no : "' + mfg_no + '",receive_date : "' + receive_date + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "-1") {
                        alertWarning("MFG ซ้ำ, กรุณากรอกตรวจสอบอีกครั้ง");
                    } else {
                        gridMFG.PerformCallback();

                        $('#txtMFGNo').val("");
                        $('#txtReceiveDate').val("");
                    }

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertError(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function editMFG(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#hdEditMFGID').val(e);
            $.ajax({
                type: "POST",
                url: "Products.aspx/EditMFG",
                data: '{id : ' + e + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtEditMFGMFGNo').val(data.mfg_no);
                    $('#txtEditMFGReceiveDate').val(data.display_receive_date);
                    $('#divmfgdate').addClass("display-none");
                    $('#txtEditMFGReceiveDate').addClass("noclick");
                    //
                    $('#modal_edit_mfg').modal("show");

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertError(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function submitEditMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = $('#hdEditMFGID').val();
            var mfg_no = $('#txtEditMFGMFGNo').val();
            var receive_date = $('#txtEditMFGReceiveDate').val();
            $.ajax({
                type: "POST",
                url: "Products.aspx/SubmitEditMFG",
                data: '{id : ' + key + ' , mfg_no : "' + mfg_no + '" , receive_date : "' + receive_date + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d == "success") {
                        window.location.reload();
                    }
                    else {
                        alertError(response.d);

                    }
                    $('#hdEditMFGID').val(0);
                    $('#txtEditMFGMFGNo').val("");
                    $('#txtEditMFGReceiveDate').val("");

                    gridMFG.PerformCallback();

                    $('#modal_edit_mfg').modal("hide");

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertError(response.d);
                    $.LoadingOverlay("hide");
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_product"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        var txtSearch = $("#txtSearchBoxData").val();
                        gridView.PerformCallback(txtSearch.toString());


                    }
                }
            });
        }
    </script>
</asp:Content>
