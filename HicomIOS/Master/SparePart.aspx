﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SparePart.aspx.cs"
    Inherits="HicomIOS.Master.SparePart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        #tableSupplierList a i {
            font-size: 16px;
        }

        .swal-modal {
            /*width: 420px !important;
            height: 140px !important;*/
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
         .noclick       {
  pointer-events:none; 
    opacity:0.6;  
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
        &nbsp;New Spare Part
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
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("part_no")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("part_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn Caption="Copy" FieldName="id" CellStyle-HorizontalAlign="Center" Width="25px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnCopy" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="copyItem(<%# Eval("id")%>)" title="Copy">
                            <i class="fa fa-copy" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="part_no" Caption="Part No" Width="50px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="secondary_code" Caption="Secondary Code" Width="50px" />
                <dx:GridViewDataTextColumn FieldName="part_name_tha" Caption="Part Name" Width="110px" />
                <dx:GridViewDataTextColumn FieldName="cat_name_tha" Caption="Category" Width="40px" />
                <dx:GridViewDataTextColumn FieldName="shelf_name" Caption="Shelf" Width="60px" />

                <%--<dx:GridViewDataTextColumn FieldName="unit_code" Caption="Item Unit" Width="30px" />

                <dx:GridViewDataTextColumn FieldName="quantity" Caption="Quantity" Width="30px" CellStyle-HorizontalAlign="Center">
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
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> >
                             <div class="handle"></div>
                        </button>
                        </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                
                
                
                
               <%-- <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <input type="checkbox" name="is_enable" id="item_<%# Eval("id")%>" data-size="mini" onchange="changeStatusItem(<%# Eval("id")%>)" <%# ((bool)Eval("is_enable")) ? "checked=''" : "" %> <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
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
                    <div class="modal-title">Add Spare Part</div>
                </div>
                <div class="modal-body text-center">
                    <ul class="nav nav-tabs" style="margin-top: 0px;">
                        <li class="active"><a data-toggle="tab" href="#productList">Spare Part</a></li>
                        <li><a data-toggle="tab" href="#supplier">Supplier</a></li>
                        <li><a data-toggle="tab" href="#shelf">Shelf</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="productList" class="tab-pane fade in active" style="margin-top: 5px;">
                            <input type="hidden" id="hdfis_edit" runat="server" />
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtPartNo">Part No:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" validate-data id="txtPartNo" required="required" runat="server" />
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
                                        <input type="text" class="form-control" validate-data id="txtPartNameTha" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtPartNameEng">Part Name(ENG):</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" validate-data id="txtPartNameEng" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cboCateID">Categories:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cboCateID" CssClass="form-control" runat="server" ClientInstanceName="cboCateID" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <%-- <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cboShelfID">Shelf:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cboShelfID" CssClass="form-control" runat="server" ClientInstanceName="cboShelfID" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cboBrand">Brand:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cboBrand" CssClass="form-control" runat="server" ClientInstanceName="cboBrand" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="cboUnitType">Unit Type:</label>
                                    <div class="col-xs-9">
                                        <dx:ASPxComboBox ID="cboUnitType" CssClass="form-control" runat="server" ClientInstanceName="cboUnitType" TextField="data_text"
                                            ValueField="data_value">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtSellingPrice">Selling Price:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" id="txtSellingPrice" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtMinSellingPrice">Min Selling Price:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" id="txtMinSellingPrice" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantity">Quantity:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantity" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantityReserve">Reserve:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityReserve" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3" for="txtQuantityBalance">Total:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control numberic" readonly="" id="txtQuantityBalance" runat="server" />
                                    </div>
                                </div>
                            </div>
                             

                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-3 no-padding" for="txtRemark">Remark:</label>
                                    <div class="col-xs-9">
                                        <input type="text" class="form-control" id="txtRemark" runat="server" />
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
                                            <label class="control-label col-xs-2" for="supplier_name">Supplier</label>
                                            <div class="col-xs-5">
                                                <dx:ASPxComboBox ID="cboSupplierSparepart" CssClass="form-control" runat="server"
                                                    OnCallback="cboSupplierSparepart_Callback"
                                                    ClientInstanceName="cboSupplierSparepart" TextField="data_text"
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
                                            <dx:ASPxGridView ID="gridSupplier" SettingsBehavior-AllowSort="false" ClientInstanceName="gridSupplier" runat="server"
                                                Settings-VerticalScrollBarMode="Visible"
                                                Settings-VerticalScrollableHeight="300"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridSupplier_CustomCallback"
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
                                                    <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center">
                                                        <DataItemTemplate>
                                                            <a class="btn btn-mini" onclick="deleteSupplier(<%# Eval("id")%>)" title="Delete">
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                            </a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                    <dx:GridViewDataTextColumn FieldName="supplier_name" Caption="Supplier Name" CellStyle-HorizontalAlign="Left" Width="700" />
                                                </Columns>
                                              

                                                <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                                    CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                            </dx:ASPxGridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <input type="hidden" id="sparepartId" />
                        </div>
                        <div id="shelf" class="tab-pane fade">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">Shelf</legend>
                                        <div class="form-group col-xs-12 no-padding">
                                            <label class="control-label col-xs-2" for="shelf">Shelf</label>
                                            <div class="col-xs-5">
                                                <dx:ASPxComboBox ID="cboShelf" CssClass="form-control" runat="server"
                                                    OnCallback="cboShelf_Callback"
                                                    ClientInstanceName="cboShelf" TextField="data_text"
                                                    ValueField="data_value">
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="col-xs-4 no-padding text-left">
                                                <button type="button" class="btn-addItem" onclick="submitShelf()">
                                                    <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                                </button>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-12 no-padding">
                                            <dx:ASPxGridView ID="gridViewShelf" SettingsBehavior-AllowSort="false" ClientInstanceName="gridViewShelf" runat="server"
                                                Settings-VerticalScrollBarMode="Visible"
                                                Settings-VerticalScrollableHeight="300"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridViewShelf_CustomCallback"
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
                                                    <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center">
                                                        <DataItemTemplate>
                                                            <a class="btn btn-mini" onclick="deleteShelf(<%# Eval("id")%>)" title="Delete">
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                            </a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                    <dx:GridViewDataTextColumn FieldName="shelf_name" Caption="Shelf Name" CellStyle-HorizontalAlign="Left" Width="700" />
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


    <script type="text/javascript">
        var pageURL = "SparePart.aspx";
        $(document).ready(function () {
            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
                $('.nav-tabs li:first-child > a').trigger('click');
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
            });
       

        $("input[validate-data]").keyup(function () {
            if ($(this).val() != "") {
                $(this).parent().parent().removeClass("has-error");
            }

        });
        var h = window.innerHeight;
        gridView.SetHeight(h - 155);
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
            gridView.SetHeight(height);
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

        function newItem() {
            isCopy = false;
            $("#sparepartId").val(0);
            $("#modal_form .modal-title").html("เพิ่ม Spare Part");

            $.ajax({
                type: 'POST',
                url: pageURL + "/NewData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    $("input[validate-data]").each(function () {
                        $(this).css("border-color", "")

                    });
                     cboUnitType.SetEnabled(true);
                    $('#txtPartNo').removeClass("noclick");

                    cboCateID.GetMainElement().style.border = "";

                    cboBrand.GetMainElement().style.border = "";

                    cboUnitType.GetMainElement().style.border = "";

                    $("#modal_form").modal("show");
                    setTimeout(function () {
                        cboSupplierSparepart.PerformCallback();
                        gridSupplier.PerformCallback();

                        gridViewShelf.PerformCallback();
                        cboShelf.PerformCallback();
                    }, 100);

                }
            });

        }

        function editItem(element, id, index) {
            gridView.FocusedRowIndex = index;

            isCopy = false;
            $("#sparepartId").val(id);
            //$.ajax({
            //    type: 'POST',
            //    url: pageURL + "/CleaSession",
            //    data: {},
            //    contentType: 'application/json; charset=utf-8',
            //    dataType: 'json',
            //    success: function () {
              
            //    }
            //});
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditSparePartData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {


                    if (result.d["is_edit"] != undefined && result.d["is_edit"] == "1") {
                        $('#txtPartNo').addClass("noclick");
                        cboUnitType.SetEnabled(false);
                    }
                    else {
                        $('#txtPartNo').removeClass("noclick");
                          cboUnitType.SetEnabled(true);  
                    }

                    $('#txtPartNo').val(result.d["part_no"]);
                    $('#txtSecondaryCode').val(result.d["secondary_code"]);

                    if (result.d["cat_id"] == 0 || result.d["cat_id"] == "") {
                        //cboCateID.AddItem("--โปรดเลือก--", "0");
                        cboCateID.SetValue("0");
                    } else {
                        cboCateID.SetValue(result.d["cat_id"]);
                        if (cboCateID.GetSelectedItem() == null) {
                            cboCateID.SetValue("0");
                        }
                    }

                    //cboShelfID.SetValue(result.d["shelf_id"]);
                    $('#txtPartNameTha').val(result.d["part_name_tha"]);
                    $('#txtPartNameEng').val(result.d["part_name_eng"]);
                    if (result.d["brand_id"] == 0 || result.d["brand_id"] == "") {
                        //cboBrand.AddItem("--โปรดเลือก--", "0");
                        cboBrand.SetValue("0");
                    } else {
                        cboBrand.SetValue(result.d["brand_id"]);
                        if (cboBrand.GetSelectedItem() == null) {
                            cboBrand.SetValue("0");
                        }
                    }

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
                    $('#txtQuantity').val(result.d["quantity"]);
                    $('#txtQuantityReserve').val(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val(result.d["quantity_balance"]);
                    var color = "#555";
			        if (result.d["quantity_balance"] < 0) {
				        color = "red";
			        }
			        $('#txtQuantityBalance').css('color', color);
                    $('#txtRemark').val(result.d["remark"]);
                    $("#modal_form .modal-title").html("แก้ไข Spare Part : " + element.title);
                    $("#modal_form").modal("show");

                    setTimeout(function () {
                        cboSupplierSparepart.PerformCallback();
                        gridSupplier.PerformCallback();

                        gridViewShelf.PerformCallback();
                        cboShelf.PerformCallback();
                    }, 100);

                }
            });
        }

        function copyItem(id) {
            isCopy = true;
            $("#sparepartId").val(0);
            $.ajax({
                type: 'POST',
                url: pageURL + "/CopySparePartData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $('#txtPartNo').val(result.d["part_no"]);
                    $('#txtSecondaryCode').val(result.d["secondary_code"]);

                    if (result.d["cat_id"] == 0 || result.d["cat_id"] == "") {
                        //cboCateID.AddItem("--โปรดเลือก--", "0");
                        cboCateID.SetValue("0");
                    } else {
                        cboCateID.SetValue(result.d["cat_id"]);
                    }

                    //cboShelfID.SetValue(result.d["shelf_id"]);
                    $('#txtPartNameTha').val(result.d["part_name_tha"]);
                    $('#txtPartNameEng').val(result.d["part_name_eng"]);
                    if (result.d["brand_id"] == 0 || result.d["brand_id"] == "") {
                        //cboBrand.AddItem("--โปรดเลือก--", "0");
                        cboBrand.SetValue("0");
                    } else {
                        cboBrand.SetValue(result.d["brand_id"]);
                    }

                    if (result.d["unit_id"] == 0 || result.d["unit_id"] == "") {
                        //cboUnitType.AddItem("--โปรดเลือก--", "0");
                        cboUnitType.SetValue("0");
                    } else {
                        cboUnitType.SetValue(result.d["unit_id"]);
                    }

                    $('#txtSellingPrice').val(result.d["selling_price"]);
                    $('#txtMinSellingPrice').val(result.d["min_selling_price"]);
                    $('#txtQuantity').val("0");//(result.d["quantity"]);
                    $('#txtQuantityReserve').val("0");//(result.d["quantity_reserve"]);
                    $('#txtQuantityBalance').val("0");//(result.d["quantity_balance"]);

                    $("#modal_form .modal-title").html("คัดลอก Spare Part");
                    $("#modal_form").modal("show");

                    setTimeout(function () {
                        cboSupplierSparepart.PerformCallback();
                        gridSupplier.PerformCallback();

                        gridViewShelf.PerformCallback();
                        cboShelf.PerformCallback();
                    }, 100);

                }
            });
        }

        var isCopy = false;
        function validateData() {

            if ($("#txtPartNo").val() == "" ) {
                $("#txtPartNo").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtPartNo").focus();

                return false;

            } else if ($("#txtPartNameTha").val() == "") {
                $("#txtPartNameTha").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtPartNameTha").focus();

                return false;

            } else if ($("#txtPartNameEng").val() == "") {
                $("#txtPartNameEng").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#productList").addClass("active in");
                $('a[href*=productList]').click();
                $("#txtPartNameEng").focus();

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
                        alertWarning(result.d);
                        setTimeout(function () {
                            cboSupplierSparepart.PerformCallback();
                            gridSupplier.PerformCallback();
                            gridViewShelf.PerformCallback();
                            cboShelf.PerformCallback();

                            return false;

                        }, 100);
                        return;
                    }
                    else {
                        submitFrom();
                    }
                }
            });
        }

        function clearValidateCate(s, e) {
            console.log("valid cbo: " + cboCateID.GetValue());
            if (cboCateID.GetValue() != null) {
                $("#cboCateID").parent().parent().removeClass("has-error");
                cboCateID.GetMainElement().style.border = "";
            }

        }
        function submitFrom() {
            var sparepartId = $("#sparepartId").val();

            var parametersAdd = {
                sparePartData: [
                    {
                        part_no: $('#txtPartNo').val(),
                        secondary_code: $('#txtSecondaryCode').val(),
                        cat_id: cboCateID.GetValue() == null ? 0 : parseInt(cboCateID.GetValue()),
                        //shelf_id: cboShelfID.GetValue() == null ? 0 : parseInt(cboShelfID.GetValue()),
                        part_name_tha: $('#txtPartNameTha').val(),
                        part_name_eng: $('#txtPartNameEng').val(),
                        brand_id: cboBrand.GetValue() == null ? 0 : parseInt(cboBrand.GetValue()),
                        unit_code: "",
                        unit_id: cboUnitType.GetValue() == null ? 0 : parseInt(cboUnitType.GetValue()),
                        selling_price: $('#txtSellingPrice').val() == "" ? 0 : parseInt($('#txtSellingPrice').val()),
                        min_selling_price: $('#txtMinSellingPrice').val() == "" ? 0 : parseInt($('#txtMinSellingPrice').val()),
                        quantity: $('#txtQuantity').val() == "" ? 0 : parseInt($('#txtQuantity').val()),
                        quantity_reserve: $('#txtQuantityReserve').val() == "" ? 0 : parseInt($('#txtQuantityReserve').val()),
                        quantity_balance: $('#txtQuantityBalance').val() == "" ? 0 : parseInt($('#txtQuantityBalance').val()),
                        remark: $('#txtRemark').val() == "" ? null : $('#txtRemark').val(),
                        id: parseInt(sparepartId)
                        
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
                        window.location.reload();
                    }
                    else {
                        alertWarning(result.d);
                    }
                }
            });


        }

        function deleteItem(e) {
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
                            url: "SparePart.aspx/DeleteSparePart",
                            data: '{id: ' + id + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d) {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                    .then((value) => {
                                        location.reload()
                                    });
                                } else {
                                    alertError("การลบข้อมูลผิดพลาด!");
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

            var value = cboSupplierSparepart.GetValue();
            var name = cboSupplierSparepart.GetText();
            if (value == "" || value == null) {
                alertWarning("กรุณาเลือกผู้จัดจำหน่าย");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "SparePart.aspx/SelectedSupplier",
                    data: '{id: "' + value + '",name : "' + name + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        cboSupplierSparepart.PerformCallback();
                        gridSupplier.PerformCallback();

                        $("#lbValidateSupplier").hide();
                        $.LoadingOverlay("hide");

                    },
                    failure: function (response) {
                        alertWarning(response.d);
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
                          url: "SparePart.aspx/DeleteSupplier",
                          data: '{id:"' + e + '"}',
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (data) {
                              swal("ลบข้อมูลสำเร็จ!", {
                                  icon: "success"
                              })
                              .then((value) => {
                                  cboSupplierSparepart.PerformCallback();
                                  gridSupplier.PerformCallback();
                              });
                          }
                      });
                  }
              });
        }

        function submitShelf() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var value = cboShelf.GetValue();
            var name = cboShelf.GetText();
            if (value == "" || value == null) {
                alertWarning("กรุณาเลือก Shelf");
                $.LoadingOverlay("hide");
            } else {
                $.ajax({
                    type: "POST",
                    url: "SparePart.aspx/SelectedShelf",
                    data: '{id: "' + value + '",name : "' + name + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        cboShelf.PerformCallback();
                        gridViewShelf.PerformCallback();

                        $.LoadingOverlay("hide");

                    },
                    failure: function (response) {
                        alertWarning(response.d);
                    }
                });
            }
        }

        function deleteShelf(e) {
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
                          url: "SparePart.aspx/DeleteShelf",
                          data: '{id:"' + e + '"}',
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (data) {
                              swal("ลบข้อมูลสำเร็จ!", {
                                  icon: "success"
                              })
                              .then((value) => {
                                  cboShelf.PerformCallback();
                                  gridViewShelf.PerformCallback();
                              });
                          }
                      });
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_product_spare_part"}',
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