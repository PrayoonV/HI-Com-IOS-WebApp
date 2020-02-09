﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SparePartSet.aspx.cs" Inherits="HicomIOS.Master.SparePartSet" %>

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

        .btn-confirm {
            width: 80px;
        }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
        function changedPartNoID() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbPartNoID.GetValue();
            $.ajax({
                type: "POST",
                url: pageURL + "/GetSparePartName",
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
            $('#txtPartNameEng').val(response.d.name_eng);
            $('#txtPartNameTha').val(response.d.name_tha);
            $('#txtQuantityStock').val(response.d.quantity);
            $('#txtQuantityReserve').val(response.d.quantity_reserve);
            $('#txtQuantityBalance').val(response.d.quantity_balance);
        }

        function submitForm() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //validateData();
            var sparepartId = $("#hdSparePartSetId").val();

            var id = cbbPartNoID.GetValue();

            var parametersAdd = {
                sparePartData: [
                    {
                        id: id,
                        part_no: cbbPartNoID.GetText(),
                        secondary_code: $('#txtSecondaryCode').val(),
                        part_name_tha: $('#txtPartNameTha').val(),
                        part_name_eng: $('#txtPartNameEng').val(),
                        quantityStock: $('#quantityStock').val(),
                        quantity: $('#txtQuantity').val() == "" ? 0 : parseInt($('#txtQuantity').val()),
                        quantity_reserve: $('#quantity_reserve').val(),
                        quantity_balance: $('#quantity_balance').val(), //$('#txtQuantityBalance').val(),

                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: pageURL + "/SaveSparePartData",
                data: JSON.stringify(parametersAdd),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d == "success") {
                        $.LoadingOverlay("hide");
                        window.location.reload();
                    }
                    else {
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
        <dx:ASPxGridView ID="gridViewSparePartSet" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewSparePartSet"
            Width="100%" KeyFieldName="spare_part_set_id" EnableCallBacks="true" OnCustomCallback="gridViewSparePartSet_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
               
                <dx:GridViewDataTextColumn Caption="Print" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <a id="btnPrint" class="btn btn-mini" onclick="printSparePartSet(<%# Eval("spare_part_set_id")%>)" title="Print">
                            <i class="fa fa-print" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="spare_part_set_no" Caption="Part No" Width="80px" />
                <dx:GridViewDataTextColumn FieldName="spare_part_set_name" Caption="Part Name" Width="300px" />
                <dx:GridViewDataTextColumn FieldName="count_product_id" Caption="Number of Parts" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="qty" Caption="Quantity" Width="50px" />
                 <%--<dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                       <button type="button" id="item_<%# Eval("spare_part_set_id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("spare_part_set_id")%>, '<%# Eval("is_enable")%>')"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> >
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
                    <div class="modal-title">Add Spare Part</div>
                </div>
                <div class="modal-body text-center">
                    <ul class="nav nav-tabs" style="margin-top: 0px;">
                        <li class="active"><a data-toggle="tab" href="#productList">Spare Part</a></li>
                        <li><a data-toggle="tab" href="#productMappaing">Spare Part Mapping</a></li>
                    </ul>
                    <div class="tab-content" style="padding-top: 5px;">
                        <div id="productList" class="tab-pane fade in active">
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cbbPartNoID">Part No:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cbbPartNoID" CssClass="form-control" runat="server"
                                            ClientInstanceName="cbbPartNoID" TextField="data_text"
                                            ClientSideEvents-ValueChanged="changedPartNoID"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>

                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtSecondaryCode">Secondary Code:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" id="txtSecondaryCode" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtPartNameTha">Part Name(THA):</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" readonly="" id="txtPartNameTha" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtPartNameEng">Part Name(ENG):</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" readonly="" id="txtPartNameEng" runat="server" />
                                    </div>
                                </div>
                            </div>



                            <div class="row">
                                <%--<div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cboUnitType">Unit Type:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cboUnitType" CssClass="form-control" ClientSideEvents-ValueChanged="function(s, e) { validateValue(s, e); }" runat="server" ClientInstanceName="cboUnitType" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>--%>
                                <%-- <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtSellingPrice">Selling Price:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" validate-data id="txtSellingPrice" runat="server" />
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row">

                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantity">Quantity:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" validate-data id="txtQuantity" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantityStock">Quantity Stock:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityStock" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantityReserve">Quantity Reserve:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityReserve" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantityBalance">Quantity Balance:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityBalance" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <%-- <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-1 no-padding" for="txtRemark">Remark:</label>
                                    <div class="col-xs-11">
                                        <input type="text" class="form-control" id="txtRemark" runat="server" />
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                        <div id="productMappaing" class="tab-pane fade">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">จัดกลุ่มสินค้า</legend>
                                        <div class="form-group col-xs-12">
                                            <div class="row text-left">
                                                <button type="button" onclick="addSparePart()" class="btn-addItem">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>
                                                    &nbsp;เพิ่มสินค้าอะไหล่
                                                </button>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-12">

                                            <div class="row">
                                                <dx:ASPxGridView ID="gridViewSparePart" ClientInstanceName="gridViewSparePart" runat="server"
                                                    Settings-VerticalScrollBarMode="Visible"
                                                    Settings-VerticalScrollableHeight="200"
                                                    OnCustomCallback="gridViewSparePart_CustomCallback"
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
                                                        <dx:GridViewDataColumn FieldName="part_no" VisibleIndex="1" CellStyle-HorizontalAlign="Left" Width="200px" />
                                                        <dx:GridViewDataColumn FieldName="part_name" VisibleIndex="2" CellStyle-HorizontalAlign="Left" Width="400px" />
                                                        <dx:GridViewDataColumn FieldName="unit_code" VisibleIndex="3" CellStyle-HorizontalAlign="Left" Width="100px" />
                                                        <dx:GridViewDataColumn FieldName="qty" VisibleIndex="4" CellStyle-HorizontalAlign="Right" Width="50px" />

                                                    </Columns>
                                                    <SettingsSearchPanel Visible="true"></SettingsSearchPanel>
                                                    <%--   <ClientSideEvents SelectionChanged="grid_SelectionChanged" />--%>
                                                </dx:ASPxGridView>

                                            </div>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdSparePartSetId" />
                                    </fieldset>
                                </div>
                            </div>
                        </div>
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
    <div class="modal fade" id="modal_select_spare_part_product" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeSelectProductModal()" aria-hidden="true">&times;</button>
                    <div class="modal-title">Product</div>
                </div>
                <div class="modal-body">
                    <div class="form-group col-xs-12 no-padding">
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
                                <dx:GridViewDataTextColumn FieldName="quantity" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
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
                        onclick="closeSelectSparepartModal()">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก               
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modal_delete" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">ลบข้อมูล</div>
                </div>
                <div class="modal-body text-center">
                    คุณต้องการลบข้อมูลใช่หรือไม่ ? 
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn-addItem"
                        id="btn_delete" onclick="confirmDelete()">
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
                        class="btn-confirm btn-addItem"
                        id="btnConfirmSave" onclick="submitForm()">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;ตกลง
                    </button>
                    <button type="button"
                        class="btn-confirm btn-addItem"
                        data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก               
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var pageURL = "SparePartSet.aspx";
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
            gridViewSparePartSet.SetHeight(h - 155);
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
            gridViewSparePartSet.SetHeight(height);
        });
        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                 

                var txtSearch = $("#txtSearchBoxData").val();
                txtSearch = $.trim(txtSearch);
                gridViewSparePartSet.PerformCallback(txtSearch.toString());
                $("#txtSearchBoxData").val(txtSearch);

                $.LoadingOverlay("hide");
            }
        }

        function newProductSet() {
            $("#hdSparePartSetId").val(0);
            $.ajax({
                type: 'POST',
                url: pageURL + "/NewProductSetData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();

                    gridViewSparePart.PerformCallback();

                    $("#modal_form .modal-title").html("เพิ่มจัดกลุ่มสินค้า");
                    $("#modal_form").modal("show");
                }
            });

        }

        function GetCheckBoxValue(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            var type = $(input).attr("ProductType");
            console.log(s);
            console.log(e);
            console.log(key);
            $.ajax({
                type: "POST",
                url: pageURL + "/SelectedProduct",
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


        function printSparePartSet(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;

            $.ajax({
                type: "POST",
                url: pageURL + "/SelectedPrintSparePartSet",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    console.log(data.product_set_id);
                    if (data) {
                        window.open("../Report/DocumentViewer.aspx?ReportArgs=SparePart_Set|" + data.product_set_id, "_blank");
                    }
                    $.LoadingOverlay("hide");

                }

            });


        }

        function editProductSet(id) {
            $("#hdSparePartSetId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditSparePartData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    cbbPartNoID.SetValue(result.d["part_no"]);
                    $('#txtSecondaryCode').val(result.d["secondary_code"]);

                    //cboCateID.SetValue(result.d["cat_id"]);

                    //cboShelfID.SetValue(result.d["shelf_id"]);
                    $('#txtPartNameTha').val(result.d["part_name_tha"]);
                    $('#txtPartNameEng').val(result.d["part_name_eng"]);
                    //$('#txtSellingPrice').val(result.d["selling_price"]);
                    $('#quantityStock').val(result.d["quantity"]);
                    $('#txtQuantityReserve').val(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val(result.d["quantity_balance"]);

                    gridViewSparePart.PerformCallback();

                    $("#modal_form .modal-title").html("แก้ไข Spare Part Set");
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
                          success: function (result) {
                              if (result.d) {
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

        function validateData() {

            if (cbbPartNoID.GetValue() == "" || cbbPartNoID.GetValue() == null) {
                alert("กรุณาเลือกรหัส Part No");
                return;
            } else {
                if ($("#txtQuantity").val() == "") {
                    $(".nav-tabs li").removeClass("active");
                    $("a[href$='productList']").parent().addClass("active");

                    $(".tab-content .tab-pane").removeClass("active in");
                    $("#productList").addClass("active in");

                    $("#txtQuantity").css("border-color", "red");
                    $("#txtQuantity").focus();
                    return;
                } else {
                    $.ajax({
                        type: 'POST',
                        url: pageURL + "/ValidateData",
                        data: {},
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (result) {
                            if (result.d != "") {
                                setTimeout(function () {
                                    cboSupplierSparepart.PerformCallback();
                                    gridSupplier.PerformCallback();

                                    gridViewShelf.PerformCallback();
                                    cboShelf.PerformCallback();
                                }, 100);

                                $(".nav-tabs li").removeClass("active");
                                $("a[href$='productMappaing']").parent().addClass("active");

                                $(".tab-content .tab-pane").removeClass("active in");
                                $("#productMappaing").addClass("active in");
                                alertMessage(result.d, "E");
                                return;
                            } else {
                                var partNo = cbbPartNoID.GetText();

                                confirmSave(partNo);
                            }
                        }
                    });
                }

            }

        }

        function validateValue(s, e) {
            var control = s.GetMainElement();
            if (s.GetValue != null) {
                s.GetMainElement().style.border = "";
            }

        }
        $("input[validate-data]").on('change', function () {
            //$(this).attr("readonly", true);
            if ($(this).val() != "") {
                $(this).css("border-color", "")
            }
        });

        function confirmSave(partNo) {

            $("#modal_confirm_save .modal-title").html("ยืนยันการบันทึก");
            $("#modal_confirm_save").modal("show");

            $('#lbShowProductNo').html(partNo)
        }



        function submitSelectSparepart() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: pageURL + "/SubmitSelectProduct",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    gridViewSparePart.PerformCallback();

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
                       success: function (data) {
                           swal("ลบข้อมูลสำเร็จ!", {
                               icon: "success"
                           })
                           .then((value) => {
                               gridViewSparePart.PerformCallback();
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
                url: pageURL + "/LoadSparePartList",
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

        function closeSelectSparepartModal() {
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
                    $('#txtEditProductNo').val(data.part_no);
                    $('#txtEditProductName').val(data.part_name);
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
                    gridViewSparePart.PerformCallback();

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
                        gridViewSparePartSet.PerformCallback();

                    }
                }
            });
        }
    </script>
</asp:Content>

