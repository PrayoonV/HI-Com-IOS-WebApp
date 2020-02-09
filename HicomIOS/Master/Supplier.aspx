﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="HicomIOS.Master.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        .btn-labelEdit {
            cursor: pointer;
        }

        a:link {
            text-decoration: none !important;
        }

        a:active {
            text-decoration: underline !important;
        }

        a:hover {
            text-decoration: underline !important;
        }

        .swal-modal {
            width: 420px !important;
            height: 280px !important;
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
        input:read-only {
        background-color:white !important;
        }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script>
        $('#gridCell').css({
            'height': '550px;'
        });
        function OnSupplierGroupChanged() {
            $("#cboSupplierGroup").parent().parent().removeClass("has-error");
        }

        function OnSupplierBrandChanged() {
            $("#cboSupplierBrand").parent().parent().removeClass("has-error");
        }

        //Call Back Geo to Province
        var lastProvince = null;
        function OnGeoChanged(cboGeo) {
            setTimeout(0.8);
            if (cboProvince.InCallback()) {
                lastProvince = cboGeo.GetValue().toString();
            } else {
                cboProvince.PerformCallback(cboGeo.GetValue().toString());
            }

            cboAmphur.PerformCallback("0"); // Clear selected cboDistrict value
            cboDistrict.PerformCallback("0"); // Clear selected cboDistrict value
            $("#txtZipcode").val("");
            $("#cboGEO").parent().parent().removeClass("has-error");
        }
        function OnEndCallback3(s, e) {
            if (lastProvince) {
                cboProvince.PerformCallback(lastProvince);
                lastProvince = null;
            }
        }

        // Call Back Province to Amphur
        var lastCountry = null;
        function OnCountryChanged(cboProvince) {
            if (cboAmphur.InCallback()) {
                lastCountry = cboProvince.GetValue().toString();
            } else {
                cboAmphur.PerformCallback(cboProvince.GetValue().toString());
            }
            cboDistrict.PerformCallback("0"); // Clear selected cboDistrict value
            $("#txtZipcode").val("");
            $("#cboProvince").parent().parent().removeClass("has-error");
        }
        function OnEndCallback(s, e) {
            if (lastCountry) {
                cboAmphur.PerformCallback(lastCountry);
                lastCountry = null;
            }
        }

        // Call Back Amphur to District
        var lastCity = null;
        function OnAmphurChanged(cboAmphur) {
            if (cboDistrict.InCallback())
                lastCity = cboAmphur.GetValue().toString();
            else
                cboDistrict.PerformCallback(cboAmphur.GetValue().toString());
            $("#txtZipcode").val("");
            $("#cboAmphur").parent().parent().removeClass("has-error");
        }
        function OnEndCallback2(s, e) {
            if (lastCity) {
                cboDistrict.PerformCallback(lastCity);
                lastCity = null;
            }
        }

        function OnDistrictChanged(cboDistrict) {
            $("#cboDistrict").parent().parent().removeClass("has-error");
            var districtID = cboDistrict.GetValue().toString();
            $.ajax({
                type: 'POST',
                url: "Config.aspx/GetZipcode",
                data: '{districtID: "' + districtID + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        $("#txtZipcode").val(result.d["zipcode"]);
                        $("#txtZipcode").parent().parent().removeClass("has-error");
                    }
                }
            });
        }
    </script>
    <button type="button" onclick="NewItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มผู้จัดจำหน่าย
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="supplierGrid"
            Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridView_CustomCallback" OnPageIndexChanged="gridView_PageIndexChanged" OnBeforeColumnSortingGrouping="gridView_BeforeColumnSortingGrouping">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
            <Columns>
                <dx:GridViewDataTextColumn Caption=" " FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px" Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("supplier_code") + " - " + Eval("supplier_name_tha")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("supplier_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="supplier_code" Caption="Supplier Code" Width="40px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="Edit">
                            <%# Eval("supplier_code")%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="supplier_group_name_tha" Caption="Supplier Group" Width="50px" />
                <dx:GridViewDataTextColumn FieldName="supplier_name_tha" Caption="Supplier Name" Width="80px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="address_tha" Caption="Address" Width="80px" />
                <dx:GridViewDataTextColumn FieldName="tel" Caption="Telephone" Width="50px" />
                <dx:GridViewDataTextColumn FieldName="email" Caption="E-Mail" Width="50px" />
                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="55px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                            data-toggle="button" aria-pressed="true" onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                            <div class="handle"></div>
                        </button>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>

        <div class="modal fade" id="modal_form" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <div class="modal-title">เพิ่มผู้จัดจำหน่าย</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtSupplierCode">Supplier Code:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtSupplierCode" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtSupplierNameTHA">Supplier Name(THA):</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtSupplierNameTHA" runat="server" validate-data />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtSupplierNameENG">Supplier Name(ENG):</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtSupplierNameENG" runat="server" validate-data />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtSupplierNameOther">Supplier Name Other:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtSupplierNameOther" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboSupplierGroup">Supplier Group:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboSupplierGroup" CssClass="form-control" runat="server" ClientInstanceName="cboSupplierGroup" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnSupplierGroupChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboSupplierBrand">Supplier Brand:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboSupplierBrand" CssClass="form-control" runat="server" ClientInstanceName="cboSupplierBrand" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnSupplierBrandChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressTHA">Address(THA):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressTHA" runat="server" validate-data />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressENG">Address(ENG):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressENG" runat="server" validate-data />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtTaxId">Tax ID:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control numberic" id="txtTaxId" runat="server" maxlength="13" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBranchNo">Branch No</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control numberic" id="txtBranchNo" runat="server" maxlength="5" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboGEO">GEO:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboGEO" CssClass="form-control" runat="server" ClientInstanceName="cboGEO" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnGeoChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboProvince">Province:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboProvince" CssClass="form-control" runat="server" ClientInstanceName="cboProvince"
                                        TextField="data_text" ValueField="data_value" OnCallback="cboProvince_Callback">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }" EndCallback=" OnEndCallback3" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboAmphur">Amphur:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboAmphur" CssClass="form-control" runat="server" ClientInstanceName="cboAmphur" TextField="data_text"
                                        ValueField="data_value" OnCallback="cboAmphur_Callback">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnAmphurChanged(s); }" EndCallback=" OnEndCallback2" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboDistrict">District:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboDistrict" CssClass="form-control" runat="server" ClientInstanceName="cboDistrict" TextField="data_text"
                                        ValueField="data_value" OnCallback="cboDistrict_Callback">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnDistrictChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtZipcode">Zipcode:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtZipcode" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtContactName">Contact Name:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtContactName" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtTelephone">Telephone:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control isValidTelephone" maxlength="10" id="txtTelephone" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtFax">Fax:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control isValidTelephone" maxlength="10" id="txtFax" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtMobile">Mobile:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control isValidMobile" id="txtMobile" runat="server" maxlength="11" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEmail">Email:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEmail" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtFirstContact">First Contact:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtFirstContact" readonly="true" autocomplete="off" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtCreditLimit">Credit Limit:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtCreditLimit" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="form-group col-xs-6 no-padding hidden">
                                <label class="control-label col-xs-4 no-padding" for="txtFinancialLimit">Financial Limit:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtFinancialLimit" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtVAT">VAT:</label>
                                <div class="col-xs-8">
                                    <!--<input type="text" class="form-control" id="txtVAT_bak" runat="server" />-->
                                    <select class="form-control" id="txtVAT" runat="server">
                                        <option value="0">0 %</option>
                                        <option value="7">7 %</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="form-group col-xs-6 no-padding hidden">
                                <label class="control-label col-xs-4 no-padding" for="txtTragetSale">Target Sale:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtTragetSale" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding hidden">
                                <label class="control-label col-xs-4 no-padding" for="txtAccountCode">Account Code:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtAccountCode" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtRemark">Remark:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                        <input type="hidden" id="supplierId" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn-addItem" onclick="submitFrom()">
                            <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                        </button>
                        <button type="button" class="btn-addItem" data-dismiss="modal">
                            <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                        </button>
                    </div>
                </div>
            </div>
        </div>


    </div>
    <script type="text/javascript">
        var pageURL = "Supplier.aspx";
        $(document).ready(function () {
            $("#txtFirstContact").datepicker({ dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true });

            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
                $("#modal_form input[type=text]").each(function () {
                    $(this).parent().parent().removeClass("has-error");
                });
            });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });
            var h = window.innerHeight;
            supplierGrid.SetHeight(h - 155);

             $(".isValidTelephone").keypress(function (e) {
                //if ((this.value.length < 11)) {
                //    alert(1)
                //    if (this.value.length == 3)
                //        $("#" + this.id).val(data) + '-';
                //    $("#" + this.id).val(data);
                //    //$("#" + this.id).focus();
                //}
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
                else if (this.value.length == 2) {
                    $("#" + this.id).val($("#" + this.id).val() + '-');

                }
            });


            $(".isValidMobile").keypress(function (e) {
                //if ((this.value.length < 11)) {
                //    alert(1)
                //    if (this.value.length == 3)
                //        $("#" + this.id).val(data) + '-';
                //    $("#" + this.id).val(data);
                //    //$("#" + this.id).focus();
                //}
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
                else if (this.value.length == 3) {
                    $("#" + this.id).val($("#" + this.id).val() + '-');

                }
            });

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
            supplierGrid.SetHeight(height);
        });
        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                supplierGrid.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function NewItem() {
            $("#supplierId").val("");
            $("#modal_form .modal-title").html("เพิ่มผู้จัดจำหน่าย");
            $("#modal_form").modal("show");
        }

        function editItem(element, id, index) {
            gridView.FocusedRowIndex = index;

            $("#supplierId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditSupplierData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    if (result.d["supplier_group_id"] == 0 || result.d["supplier_group_id"] == "") {
                        cboSupplierGroup.SetValue("0");
                    } else {
                        cboSupplierGroup.SetValue(result.d["supplier_group_id"]);
                        if (cboSupplierGroup.GetSelectedItem() == null) {
                            cboSupplierGroup.SetValue("0");
                        }
                    }

                    if (result.d["supplier_brand_id"] == 0 || result.d["supplier_brand_id"] == "") {
                        cboSupplierBrand.SetValue("0");
                    } else {
                        cboSupplierBrand.SetValue(result.d["supplier_brand_id"]);
                        if (cboSupplierBrand.GetSelectedItem() == null) {
                            cboSupplierBrand.SetValue("0");
                        }
                    }

                    if (result.d["geo_id"] == 0 || result.d["geo_id"] == "") {
                        cboGEO.SetValue("0");
                    } else {
                        cboGEO.SetValue(result.d["geo_id"]);
                    }

                    cboProvince.ClearItems();
                    cboProvince.AddItem("--โปรดเลือก--", "0");
                    if (result.d.provinceList.length == 0) {
                        cboProvince.SetValue("0");
                    } else {
                        for (var i = 0; i < result.d.provinceList.length; i++) {
                            var rowProvince = result.d.provinceList[i];
                            cboProvince.AddItem(rowProvince["name"], rowProvince["id"]);
                        }
                    }

                    cboAmphur.ClearItems();
                    cboAmphur.AddItem("--โปรดเลือก--", "0");
                    if (result.d.amphurList.length == 0) {
                        cboAmphur.SetValue("0");
                    } else {
                        for (var i = 0; i < result.d.amphurList.length; i++) {
                            var rowAmphur = result.d.amphurList[i];
                            cboAmphur.AddItem(rowAmphur["name"], rowAmphur["id"]);
                        }
                    }

                    cboDistrict.ClearItems();
                    cboDistrict.AddItem("--โปรดเลือก--", "0");
                    if (result.d.districtList.length == 0) {
                        cboDistrict.SetValue("0");
                    } else {
                        for (var i = 0; i < result.d.districtList.length; i++) {
                            var rowDistrict = result.d.districtList[i];
                            cboDistrict.AddItem(rowDistrict["name"], rowDistrict["id"]);
                        }
                    }

                    $('#txtSupplierCode').val(result.d["supplier_code"]);
                    $('#txtSupplierNameTHA').val(result.d["supplier_name_tha"]);
                    $('#txtSupplierNameENG').val(result.d["supplier_name_eng"]);
                    $('#txtSupplierNameOther').val(result.d["supplier_name_other"]);
                    //cboSupplierGroup.SetValue(result.d["supplier_group_id"]);
                    //cboSupplierBrand.SetValue(result.d["supplier_brand_id"]);
                    $('#txtAddressTHA').val(result.d["address_tha"]);
                    $('#txtAddressENG').val(result.d["address_eng"]);
                    $('#txtTaxId').val(result.d["tax_id"]);
                    $('#txtBranchNo').val(result.d["branch_no"]);

                    cboGEO.SetValue(result.d["geo_id"]);
                    if (cboGEO.GetSelectedItem() == null) {
                        cboGEO.SetValue("0");
                    }
                    cboProvince.SetValue(result.d["province_id"]);
                    if (cboProvince.GetSelectedItem() == null) {
                        cboProvince.SetValue("0");
                    }
                    cboAmphur.SetValue(result.d["amphur_id"]);
                    if (cboAmphur.GetSelectedItem() == null) {
                        cboAmphur.SetValue("0");
                    }
                    cboDistrict.SetValue(result.d["district_id"]);
                    if (cboDistrict.GetSelectedItem() == null) {
                        cboDistrict.SetValue("0");
                    }
                    first_contact = "";
                    if (result.d["first_contact"] != "01/01/1900" && result.d["first_contact"] != "01-01-1900") {
                        first_contact = result.d["first_contact"];
                    }

                    $('#txtZipcode').val(result.d["zipcode"]);
                    $('#txtContactName').val(result.d["contact_name"]);
                    $('#txtTelephone').val(result.d["tel"]);
                    $('#txtEmail').val(result.d["email"]);
                    $('#txtFax').val(result.d["fax"]);
                    $('#txtMobile').val(result.d["mobile"]);
                    $('#txtFirstContact').val(first_contact);
                    $('#txtCreditLimit').val(result.d["credit_limit"]);
                    $('#txtFinancialLimit').val(result.d["financial_limit"]);
                    $('#txtVAT').val(result.d["vat"]);
                    $('#txtTragetSale').val(result.d["target_sale"]);
                    $('#txtAccountCode').val(result.d["account_code_id"]);
                    $('#txtRemark').val(result.d["remark"]);

                    $("#modal_form .modal-title").html("แก้ไขผู้จัดจำหน่าย : " + element.title);
                    $("#modal_form").modal("show");
                }
            });
        }

        function isValidEmailAddress(emailAddress) {
            var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
            return pattern.test(emailAddress);
        }

        function isValidNumberPhome(NumberPhone) {
            var pattern = /^[0-9]{10}/i;
            return pattern.test(NumberPhone);
        }

        function submitFrom() {

            if ($("#txtSupplierNameTHA").val() == "") {
                $("#txtSupplierNameTHA").parent().parent().addClass("has-error");
                $("#txtSupplierNameTHA").focus();
                return false;
            } else if ($("#txtSupplierNameENG").val() == "") {
                $("#txtSupplierNameENG").parent().parent().addClass("has-error");
                $("#txtSupplierNameENG").focus();
                return false;
            }
            else if ($("#txtAddressTHA").val() == "") {
                $("#txtAddressTHA").parent().parent().addClass("has-error");
                $("#txtAddressTHA").focus();
                return false;
            }
            else if ($("#txtAddressENG").val() == "") {
                $("#txtAddressENG").parent().parent().addClass("has-error");
                $("#txtAddressENG").focus();
                return false;
            }
            else if ($("#txtTaxId").val() != "" && $("#txtTaxId").val().length < 13) {
                alertError('Please input Tax id minumum 13 digit');
                $("#txtTaxId").focus();

                return false;
            }
            else if ($("#txtBranchNo").val() != "" && $("#txtBranchNo").val().length < 5) {
                alertError('Please input Branch no minumum 5 digit');
                $("#txtBranchNo").focus();

                return false;
            }

            if (!isValidEmailAddress($("#txtEmail").val()) && $("#txtEmail").val() != "") {
                $("#txtEmail").focus();
                return false;
            }

            //if (!isValidNumberPhome($("#txtTelephone").val()) && $("#txtTelephone").val() != "") {
            //    $("#txtTelephone").focus();
            //    return false;
            //}

            var supplierId = $("#supplierId").val();
            if (supplierId == "") {
                var parametersAdd = {
                    supplierAddData: [
                        {
                            supplier_code: $('#txtSupplierCode').val(),
                            supplier_name_tha: $('#txtSupplierNameTHA').val(),
                            supplier_name_eng: $('#txtSupplierNameENG').val(),
                            supplier_name_other: $('#txtSupplierNameOther').val(),
                            supplier_group_id: (cboSupplierGroup.GetValue() ? cboSupplierGroup.GetValue() : "0"),
                            supplier_group_name_eng: (cboSupplierGroup.GetText() ? cboSupplierGroup.GetText() : ""),
                            supplier_group_name_tha: (cboSupplierGroup.GetText() ? cboSupplierGroup.GetText() : ""),
                            supplier_brand_id: (cboSupplierBrand.GetValue() ? cboSupplierBrand.GetValue() : "0"),
                            supplier_brand_name_eng: (cboSupplierBrand.GetText() ? cboSupplierBrand.GetText() : ""),
                            supplier_brand_name_tha: (cboSupplierBrand.GetText() ? cboSupplierBrand.GetText() : ""),
                            address_tha: $('#txtAddressTHA').val(),
                            address_eng: $('#txtAddressENG').val(),
                            branch_no: $('#txtBranchNo').val(),
                            tax_id: $('#txtTaxId').val(),
                            geo_id: (cboGEO.GetValue() ? cboGEO.GetValue() : "0"),
                            province_id: (cboProvince.GetValue() ? cboProvince.GetValue() : "0"),
                            amphur_id: (cboAmphur.GetValue() ? cboAmphur.GetValue() : "0"),
                            district_id: (cboDistrict.GetValue() ? cboDistrict.GetValue() : "0"),
                            zipcode: $('#txtZipcode').val(),
                            contact_name: $("#txtContactName").val(),
                            tel: $("#txtTelephone").val(),
                            email: $("#txtEmail").val(),
                            fax: $("#txtFax").val(),
                            mobile : $("#txtMobile").val(),
                            first_contact: $("#txtFirstContact").val(),
                            credit_limit: ($('#txtCreditLimit').val() ? $("#txtCreditLimit").val() : "0"),
                            financial_limit: ($('#txtFinancialLimit').val() ? $("#txtFinancialLimit").val() : "0"),
                            vat: ($('#txtVAT').val() ? $("#txtVAT").val() : "0"),
                            target_sale: ($('#txtTragetSale').val() ? $("#txtTragetSale").val() : "0"),
                            account_code_id: $('#txtAccountCode').val(),
                            remark: $("#txtRemark").val()
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/InsertSupplier",
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

            } else if (supplierId != "") {
                var parametersEdit = {
                    supplierUpdateData: [
                        {
                            id: supplierId,
                            supplier_code: $('#txtSupplierCode').val(),
                            supplier_name_tha: $('#txtSupplierNameTHA').val(),
                            supplier_name_eng: $('#txtSupplierNameENG').val(),
                            supplier_name_other: $('#txtSupplierNameOther').val(),
                            supplier_group_id: (cboSupplierGroup.GetValue() ? cboSupplierGroup.GetValue() : "0"),
                            supplier_brand_id: (cboSupplierBrand.GetValue() ? cboSupplierBrand.GetValue() : "0"),
                            address_tha: $('#txtAddressTHA').val(),
                            address_eng: $('#txtAddressENG').val(),
                            branch_no: $('#txtBranchNo').val(),
                            tax_id: $('#txtTaxId').val(),
                            geo_id: (cboGEO.GetValue() ? cboGEO.GetValue() : "0"),
                            province_id: (cboProvince.GetValue() ? cboProvince.GetValue() : "0"),
                            amphur_id: (cboAmphur.GetValue() ? cboAmphur.GetValue() : "0"),
                            district_id: (cboDistrict.GetValue() ? cboDistrict.GetValue() : "0"),
                            zipcode: $('#txtZipcode').val(),
                            contact_name: $("#txtContactName").val(),
                            tel: $("#txtTelephone").val(),
                            email: $("#txtEmail").val(),
                            fax: $("#txtFax").val(),
                            mobile : $("#txtMobile").val(),
                            first_contact: $("#txtFirstContact").val(),
                            credit_limit: ($('#txtCreditLimit').val() ? $("#txtCreditLimit").val() : "0"),
                            financial_limit: ($('#txtFinancialLimit').val() ? $("#txtFinancialLimit").val() : "0"),
                            vat: ($('#txtVAT').val() ? $("#txtVAT").val() : "0"),
                            target_sale: ($('#txtTragetSale').val() ? $("#txtTragetSale").val() : "0"),
                            account_code_id: $('#txtAccountCode').val(),
                            remark: $("#txtRemark").val()
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/UpdateSupplier",
                    data: JSON.stringify(parametersEdit),
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
                            url: pageURL + "/DeleteSupplier",
                            data: '{id: "' + id + '"}',
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_supplier"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                        var txtSearch = $("#txtSearchBoxData").val();
                        supplierGrid.PerformCallback(txtSearch.toString());
                    }
                }
            });
        }


    </script>
</asp:Content>
