﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductSet.aspx.cs" Inherits="HicomIOS.Master.ProductSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
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
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
        function changedProductNoID() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbProductNoID.GetValue();
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/GetProductName",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onChangedSuccess,
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");
        }
        function onChangedSuccess(response) {
            $('#txtProductNameENG').val(response.d.name_eng);
            $('#txtProductNameTHA').val(response.d.name_tha);
            $('#txtQuantityStock').val(response.d.quantity);
            $('#txtQuantityReserve').val(response.d.quantity_reserve);
            $('#txtQuantityBalance').val(response.d.quantity_balance);
        }
        function submitForm() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            var productId = $("#hdProductSetId").val();
            var id = cbbProductNoID.GetValue();

            var parametersAdd = {
                productData: [
                    {
                        id: id,
                        product_no: cbbProductNoID.GetText(),
                        product_model: $('#txtProductModel').val(),
                        product_name_tha: $('#txtProductNameTHA').val(),
                        product_name_eng: $('#txtProductNameENG').val(),
                        quantityStock: $('#quantityStock').val(),
                        quantity: $('#txtQuantity').val() == "" ? 0 : parseInt($('#txtQuantity').val()),
                        quantity_reserve: $('#quantity_reserve').val(),
                        quantity_balance: $('#quantity_balance').val(),
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
                        $.LoadingOverlay("hide");
                        window.location.reload();
                    }
                    else {
                        $.LoadingOverlay("hide");
                        alertMessage(result.d, "E");
                    }
                }
            });
        }
    </script>
    <button type="button" onclick="newProductSet()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มจัดกลุ่มสินค้า
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridViewProductSet" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewProductSet"
            Width="100%" KeyFieldName="product_product_set_id" EnableCallBacks="true" OnCustomCallback="gridViewProductSet_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <%--<dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editProductSet(<%# Eval("product_set_id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteProductSet(<%# Eval("product_set_id")%>, '<%# Eval("product_no")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn Caption="Print" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <a id="btnPrint" class="btn btn-mini" onclick="printProductSet(<%# Eval("product_set_id")%>)" title="Print">
                            <i class="fa fa-print" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" Width="80px" />
                <dx:GridViewDataTextColumn FieldName="product_name_tha" Caption="Product Name" Width="300px" />
                <dx:GridViewDataTextColumn FieldName="count_product_id" Caption="Number of Products" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="qty" Caption="Quantity" Width="50px" />
                <%--<dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <input type="checkbox" name="is_enable" id="item_<%# Eval("product_set_id")%>" data-size="mini" onchange="changeStatusItem(<%# Eval("product_set_id")%>)" <%# ((bool)Eval("is_enable")) ? "checked=''" : "" %> <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %> />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <%--<dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("product_set_id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("product_set_id")%>, '<%# Eval("is_enable")%>')"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> >
                             <div class="handle"></div>
                        </button>
                        </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="500" />
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
                        <%--<li><a data-toggle="tab" href="#quotationLine">Quotation Line</a></li>--%>
                        <li><a data-toggle="tab" href="#productMappaing">Product Mapping</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="productList" class="tab-pane fade in active">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">ข้อมูลสินค้าจัดประกอบ</legend>
                                        <div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4" for="cbbProductNoID">Product No:</label>
                                                <div class="col-xs-8">
                                                    <dx:ASPxComboBox ID="cbbProductNoID" CssClass="form-control" runat="server"
                                                        ClientInstanceName="cbbProductNoID" TextField="data_text"
                                                        ClientSideEvents-ValueChanged="changedProductNoID"
                                                        ValueField="data_value">
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Product Name(THA):</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" readonly="" id="txtProductNameTHA" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtProductNameENG">Product Name(ENG):</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" readonly="" id="txtProductNameENG" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row">
                                            <div class="form-group col-xs-12 no-padding">
                                                <label class="control-label col-xs-2 no-padding" for="txtDescriptionTHA">Description(THA):</label>
                                                <div class="col-xs-10">
                                                    <input type="text" class="form-control" id="txtDescriptionTHA" runat="server" />
                                                </div>
                                            </div>
                                        </div>--%>
                                        <%--<div class="row">
                                            <div class="form-group col-xs-12 no-padding">
                                                <label class="control-label col-xs-2 no-padding" for="txtDescriptionENG">Description(ENG):</label>
                                                <div class="col-xs-10">
                                                    <input type="text" class="form-control" id="txtDescriptionENG" runat="server" />
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4" for="txtProductModel">Product Model:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="txtProductModel" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4" for="cboBrand">Brand:</label>
                                                <div class="col-xs-8">
                                                    <dx:ASPxComboBox ID="cboBrand" CssClass="form-control" runat="server" ClientInstanceName="cboBrand" TextField="data_text"
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
                                        </div>--%>
                                        <div class="row">

                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtQuantity">Quantity:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control numberic" validate-data id="txtQuantity" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4" for="txtQuantityStock">Quantity Stock:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control numberic" readonly="" id="txtQuantityStock" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtQuantityReserve">Quantity Reserve:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" readonly="" id="txtQuantityReserve" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtQuantityBalance">QuantityBalance:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" readonly="" id="txtQuantityBalance" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtSellingPrice">Selling Price :</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control numberic" id="txtSellingPrice" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtMinSellingPrice">Min Selling Price :</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control numberic" id="txtMinSellingPrice" runat="server" />
                                                </div>
                                            </div>

                                        </div>--%>
                                        <%--<div class="row">
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
                                        </div>--%>
                                        <%--<div class="row">
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
                                        </div>--%>

                                        <%--     <div class="form-group col-xs-6 no-padding">
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

                                        <%-- <div class="row">
                                            <div class="form-group col-xs-6 no-padding">
                                                <label class="control-label col-xs-4 no-padding" for="txtRemark">Remark :</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="txtRemark" runat="server" />
                                                </div>
                                            </div>

                                        </div>--%>
                                    </fieldset>
                                </div>
                            </div>

                        </div>
                        <div id="productMappaing" class="tab-pane fade">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">จัดกลุ่มสินค้า</legend>
                                        <div class="form-group col-xs-12">
                                            <div class="row text-left">
                                                <button type="button" onclick="addProduct()" class="btn-addItem">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>
                                                    &nbsp;เพิ่มสินค้า
                                                </button>
                                                <button type="button" onclick="addSparePart()" class="btn-addItem">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>
                                                    &nbsp;เพิ่มสินค้าอะไหล่
                                                </button>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 text-left  no-padding" style="margin-bottom: 5px;">
                                            <input type="text" id="TextGridViewProduct" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." onkeypress="searchDataGridViewProduct(event.keyCode)">
                                            <button type="button" style="margin-left: 5px;" class="btn-addItem" id="btnSubmitSearchGridViewProduct" onclick="searchDataGridViewProduct(13);">
                                                <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                            </button>
                                        </div>
                                        <div class="form-group col-xs-12">

                                            <div class="row">

                                                <dx:ASPxGridView ID="gridViewProduct" ClientInstanceName="gridViewProduct" runat="server"
                                                    Settings-VerticalScrollBarMode="Visible"
                                                    Settings-VerticalScrollableHeight="200"
                                                    OnCustomCallback="gridViewProduct_CustomCallback"
                                                    KeyFieldName="id" Width="100%">
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                                    <Paddings Padding="0px" />
                                                    <Border BorderWidth="0px" />
                                                    <BorderBottom BorderWidth="1px" />
                                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                        PageSizeItemSettings-Visible="true">
                                                        <PageSizeItemSettings Items="10, 20, 50" />
                                                    </SettingsPager>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100px">
                                                            <DataItemTemplate>
                                                                <a id="btnEdit" class="btn btn-mini" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                                                                    <i class="fa fa-pencil" aria-hidden="true"></i>
                                                                </a>|
                                                                <a class="btn btn-mini" onclick="deleteItem(<%# Eval("id")%>)" title="Delete">
                                                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                                </a>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataColumn FieldName="product_no" VisibleIndex="1" CellStyle-HorizontalAlign="Left" Width="100px" />
                                                        <dx:GridViewDataColumn FieldName="product_name" VisibleIndex="2" CellStyle-HorizontalAlign="Left" Width="400px" />
                                                        <dx:GridViewDataColumn FieldName="unit_code" VisibleIndex="3" CellStyle-HorizontalAlign="Left" Width="100px" />
                                                        <dx:GridViewDataColumn FieldName="qty" VisibleIndex="4" CellStyle-HorizontalAlign="Right" Width="50px" />
                                                        <dx:GridViewDataColumn FieldName="productType" VisibleIndex="5" CellStyle-HorizontalAlign="Left" Width="100px" />

                                                    </Columns>

                                                    <%--   <ClientSideEvents SelectionChanged="grid_SelectionChanged" />--%>
                                                </dx:ASPxGridView>

                                            </div>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdProductSetId" />
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <%-- <div id="quotationLine" class="tab-pane fade">
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtQuotationDescLine1">Quotation Desc Line1:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtQuotationDescLine1" runat="server" />
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

                        </div>--%>
                    </div>
                </div>
                <div class="modal-footer">
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

    <%--///////////--%>
    <div class="modal fade" id="modal_select_product" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeSelectProductModal()" aria-hidden="true">&times;</button>
                    <div class="modal-title">Product</div>
                </div>
                <div class="modal-body">
                    <div class="row">
                         
                         <div class="col-xs-12 text-left" style="margin-bottom: 5px;">
                             <div class="col-xs-6 text-left no-padding">
                                <input type="text" id="txtsearchDataGridSelectProduct" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGridSelectProduct(event.keyCode)" />
                                   
                            </div>
                             <div class="col-xs-2 text-left no-padding" style="margin-left: 5px;">
                                
                                    <button type="button" class="btn-addItem" id="btnSubmitSearchSelectProduct" onclick="searchDataGridSelectProduct(13);">
                                        <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                    </button>
                            </div>
                             </div>
                        <div class="col-xs-12">
                            <dx:ASPxGridView ID="gridViewSelectProduct" 
                                runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewSelectProduct"
                                Width="100%" KeyFieldName="id"
                                OnHtmlDataCellPrepared="gridViewSelectProduct_HtmlDataCellPrepared"
                                OnCustomCallback="gridViewSelectProduct_CustomCallback">
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
                                                TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                                ProductId='<%# Eval("id")%>' ProductType="P">
                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                                        GetCheckBoxValue(s, e); 
                                                                                    }" />
                                            </dx:ASPxCheckBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn FieldName="product_no" VisibleIndex="1" Width="100px" />
                                    <dx:GridViewDataColumn FieldName="product_name" VisibleIndex="2" Width="300px" />
                                    <dx:GridViewDataColumn FieldName="unit_code" VisibleIndex="4" Width="100px" />
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
                    <button type="button"
                        class="btn-addItem"
                        id="btnSubmitProductSelect" onclick="submitSelectProduct()">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;ตกลง
                    </button>
                    <button type="button"
                        class="btn-addItem"
                        onclick="closeSelectProductModal()">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก               
                    </button>
                </div>
            </div>
        </div>
    </div>

   <%-- //////////--%>
 

    <div class="modal fade" id="modal_select_spare_part_product" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeSelectProductModal()" aria-hidden="true">&times;</button>
                    <div class="modal-title">Product</div>
                </div>
                <div class="modal-body">
                    <div class="form-group col-xs-12 no-padding">
                        <div class="div-search-box">
                            <input type="text" id="TextSelectSparePart" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGridSelectSparePart(event.keyCode)" />
                            <button type="button" class="btn-addItem" id="btnSubmitSearchSelectSparePart" onclick="searchDataGridSelectSparePart(13);">
                                <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                            </button>
                        </div>
                        <dx:ASPxGridView ID="gridViewSelectSparePart" ClientInstanceName="gridViewSelectSparePart" runat="server" Settings-VerticalScrollBarMode="Visible"
                            Settings-VerticalScrollableHeight="220"
                            KeyFieldName="id" Width="100%"
                            OnCustomCallback="gridViewSelectSparePart_CustomCallback"
                            OnHtmlDataCellPrepared="gridViewSelectSparePart_HtmlDataCellPrepared">
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                    <DataItemTemplate>
                                        <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                            TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                            ProductId='<%# Eval("id")%>' ProductType="S">
                                            <ClientSideEvents ValueChanged="function(s, e) { 
                                                                                        GetCheckBoxValue(s, e); 
                                                                                    }" />
                                        </dx:ASPxCheckBox>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataColumn FieldName="product_no" VisibleIndex="1" Width="100px" />
                                <dx:GridViewDataColumn FieldName="product_name" VisibleIndex="2" Width="300px" />
                                <dx:GridViewDataColumn FieldName="unit_code" VisibleIndex="4" Width="100px" />

                            </Columns>
                            <%--   <ClientSideEvents SelectionChanged="grid_SelectionChanged" />--%>
                            <SettingsSearchPanel Visible="true"></SettingsSearchPanel>
                        </dx:ASPxGridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn-addItem"
                        id="btomSubmitSparepart" onclick="submitSelectSparepart()">
                        <%-- ใช้ function เดียวกับ Product --%>
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;ตกลง
                    </button>
                    <button type="button"
                        class="btn-addItem"
                        onclick="closeSelectPartModal()">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก               
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_edit_product" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Edit Product Detail
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Product No :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" readonly="" id="txtEditProductNo" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Product Name :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control" readonly="" id="txtEditProductName" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                <div class="col-xs-7 no-padding">
                                    <input type="text" class="form-control numberic" id="txtEditProductQty" runat="server" />
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdEditProductId" />
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth"></label>
                                <div class="col-xs-7 no-padding text-right">
                                    <button type="button" runat="server" id="Button7" onclick="submitEditProduct()" class="btn-app btn-addItem">ยืนยัน</button>
                                    <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modal_confirm_save" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">ลบข้อมูล</div>
                </div>
                <div class="modal-body text-center">
                    <input type="hidden" name="delete_link" id="delete_link" class="hide" value="">
                    ยืนยันการบันทึกข้อมูล &nbsp;
                    <label id="lbShowProductNo"></label>
                    &nbsp; ?         
                </div>
                <div class="modal-footer">

                    <button type="button"
                        class="btn-addItem"
                        id="btnConfirmSave" onclick="submitForm()">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;ตกลง
                    </button>
                    <button type="button"
                        class="btn-addItem"
                        data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก               
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var pageURL = "ProductSet.aspx";
        $(document).ready(function () {
            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
            });
            $('#count_product').html("0");
            var h = window.innerHeight;
            gridViewProductSet.SetHeight(h - 155);
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
            gridViewProductSet.SetHeight(height);
        });
        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                gridViewProductSet.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchDataGridSelectProduct(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtsearchDataGridSelectProduct").val();
                gridViewSelectProduct.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchDataGridSelectSparePart(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#TextSelectSparePart").val();
                gridViewSelectSparePart.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function searchDataGridViewProduct(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#TextGridViewProduct").val();
                gridViewProduct.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function newProductSet() {
            $("#hdProductSetId").val(0);
            $.ajax({
                type: 'POST',
                url: "ProductSet.aspx/NewProductSetData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();

                    gridViewProduct.PerformCallback();
                    $('#txtProductNo').prop("disabled", false);
                    $("#modal_form .modal-title").html("เพิ่มจัดกลุ่มสินค้า");
                    $("#modal_form").modal("show");
                }
            });

        }

        function printProductSet(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/GetSaleProductSetData",
                data: '{set_id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;

                    if (data) {
                        window.open("../Report/DocumentViewer.aspx?ReportArgs=Product_Set|" + data.product_set_id, "_blank");
                    }
                    $.LoadingOverlay("hide");

                }

            });


        }

        function editProductSet(id) {
            $("#hdProductSetId").val(id);
            $.ajax({
                type: 'POST',
                url: "ProductSet.aspx/GetEditProductSetData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $('#txtProductNo').val(result.d["product_no"]);
                    $('#txtProductNo').prop("disabled", "disabled");
                    //$('#txtSerialNo').val(result.d["serial_no"]);
                    cbbProductNoID.SetValue(result.d["product_no"]);
                    //cboBrand.SetValue(result.d["brand_id"]);
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
                    $('#txtQuantity').val(result.d["quantity"]);
                    $('#txtQuantityReserve').val(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val(result.d["quantity_balance"]);
                    //cboUnitType.SetValue(result.d["unit_id"]);
                    $('#txtSellingPrice').val(result.d["selling_price"]);
                    $('#txtMinSellingPrice').val(result.d["min_selling_price"]);
                    $('#txtRemark').val(result.d["remark"]);
                    $('#txtPressure').val(result.d["pressure"]);
                    $('#txtPowerSupply').val(result.d["power_supply"]);
                    $('#txtPhase').val(result.d["phase"]);
                    $('#txtHz').val(result.d["hz"]);
                    //$('#txtUnitWarranty').val(result.d["unit_warranty"]);
                    //$('#txtAirEndWarranty').val(result.d["air_end_warranty"]);

                    gridViewProduct.PerformCallback();

                    $("#modal_form .modal-title").html("แก้ไขสินค้า");
                    $("#modal_form").modal("show");
                }
            });
        }

        function deleteProductSet(id, dataText) {
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
                      url: pageURL + "/DeleteProductSet",
                      data: '{id: ' + id + '}',
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (data) {

                          swal("ลบข้อมูลสำเร็จ!", {
                              icon: "success"
                          })
                          .then((value) => {
                              window.location.reload();
                          });

                      }
                  });
              }
          });
        }
        function validateData() {

            if (cbbProductNoID.GetValue() == "" || cbbProductNoID.GetValue() == null) {
                alert("กรุณาเลือกรหัส Product No");
                return;
            }
            else {
                $("input[validate-data]").each(function () {
                    //$(this).attr("readonly", true);
                    if ($(this).val() == "") {

                        $(this).css("border-color", "red")
                    }
                    else {
                        $.ajax({
                            type: 'POST',
                            url: pageURL + "/ValidateData",
                            data: {},
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (result) {
                                if (result.d != "") {
                                    alertMessage(result.d, "E");
                                    return;
                                }
                                else {
                                    var product_set_no = cbbProductNoID.GetText();
                                    confirmSave(product_set_no);
                                }
                            }
                        });
                    }
                });
            }
        }

        function confirmSave(product_no) {

            $("#modal_confirm_save .modal-title").html("ยืนยันการบันทึก");
            $("#modal_confirm_save").modal("show");

            $('#lbShowProductNo').html(product_no)
        }

        function addProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/LoadProductList",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    $("#modal_select_product .modal-title").html("เพิ่มสินค้า");
                    $("#modal_select_product").modal("show");

                    gridViewSelectProduct.PerformCallback();


                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function GetCheckBoxValue(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            var type = $(input).attr("ProductType");
            console.log("key:" + key);
            console.log("value:" + value);
            console.log("type:" + type);
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/SelectedProduct",
                data: '{id: "' + key + '" , isSelected: "' + value + '" , product_type : "' + type + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitSelectProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/SubmitSelectProduct",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    gridViewProduct.PerformCallback();

                    $("#modal_select_product").modal("hide");
                    $('#count_product').html(response.d);
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitSelectSparepart() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/SubmitSelectProduct",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    gridViewProduct.PerformCallback();

                    $("#modal_select_spare_part_product").modal("hide");
                    $('#count_product').html(response.d);
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function deleteItem(e) {
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
                           url: pageURL + "/DeleteProduct",
                           data: '{id:"' + e + '"}',
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           success: function (response) {
                               swal("ลบข้อมูลสำเร็จ!", {
                                   icon: "success"
                               })
                                  .then((value) => {
                                      gridViewProduct.PerformCallback();
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

        function addSparePart() {

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "ProductSet.aspx/LoadSparePartList",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    $("#modal_select_spare_part_product .modal-title").html("เพิ่มสินค้าอะไหล่");
                    $("#modal_select_spare_part_product").modal("show");

                    gridViewSelectSparePart.PerformCallback();


                },
                failure: function (response) {
                    //alert(response.d);
                }
            });

        }

        function closeSelectProductModal() {
            $("#modal_select_product").modal("hide");
        }
        function closeSelectPartModal() {
            $("#modal_select_spare_part_product").modal("hide");
        }
        function editItem(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: pageURL + "/EditProduct",
                data: '{id:"' + e + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdEditProductId').val(e);
                    $('#txtEditProductNo').val(data.product_no);
                    $('#txtEditProductName').val(data.product_name);
                    $('#txtEditProductQty').val(data.qty);

                    $("#modal_edit_product").modal("show");
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submitEditProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = $('#hdEditProductId').val();
            var qty = $('#txtEditProductQty').val();
            $.ajax({
                type: "POST",
                url: pageURL + "/SubmitEditProduct",
                data: '{id:"' + key + '" , qty : "' + qty + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#hdEditProductId').val("");
                    $('#txtEditProductNo').val("");
                    $('#txtEditProductName').val("");
                    $('#txtEditProductQty').val(0);

                    $("#modal_edit_product").modal("hide");
                    gridViewProduct.PerformCallback();

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
                url: pageURL + "/EditEnable",
                data: '{id: "' + id + '", is_enable: "' + is_enable + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        gridViewProductSet.PerformCallback();

                    }
                }
            });
        }
    </script>
</asp:Content>

