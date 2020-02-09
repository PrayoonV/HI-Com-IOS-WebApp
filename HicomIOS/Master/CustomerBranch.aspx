﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CustomerBranch.aspx.cs" Inherits="HicomIOS.Master.CustomerBranch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        .modal .modal-dialog {
            top: -20px;
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
    </style>
    <script>
        function OnCustomerIdChanged() {
            $("#cboCustomerId").parent().parent().removeClass("has-error");
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
            var districtID = cboDistrict.GetValue().toString();
            $("#cboDistrict").parent().parent().removeClass("has-error");
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
        &nbsp;เพิ่มสาขาลูกค้า
    </button>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="customerBranchGrid"
            Width="100%" KeyFieldName="id">
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
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btn
                            " class="btn btn-mini " onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("branch_name")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i><%--<%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>--%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="branch_no" Caption="Branch No" Width="70px" />
                <dx:GridViewDataTextColumn FieldName="branch_name" Caption="Branch Name" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="company_name_tha" Caption="Company Name" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="address_branch" Caption="Address" Width="200px" />
                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <input type="checkbox" name="is_enable" id="item_<%# Eval("id")%>" data-size="mini" onchange="changeStatusItem(<%# Eval("id")%>)" <%# ((bool)Eval("is_enable")) ? "checked=''" : "" %> <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %> />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
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
                        <div class="modal-title">เพิ่มสาขาลูกค้า</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBranchNo">Branch No:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtBranchNo" title="กรุณากรอกเป็นตัวเลขเท่านั้น" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBranchName">Branch Name:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtBranchName" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="cboCustomerId">Customer Name:</label>
                                <div class="col-xs-10">
                                    <dx:ASPxComboBox ID="cboCustomerId" CssClass="form-control" runat="server" ClientInstanceName="cboCustomerId" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCustomerIdChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressBillTHA">Address Bill(ENG):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressBillTHA" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressBillENG">Address Bill(ENG):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressBillENG" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressInstallTHA">Address Install(THA):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressInstallTHA" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddressInstallENG">Address Install(ENG):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddressInstallENG" runat="server" />
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
                                    <input type="text" class="form-control numberic" id="txtZipcode" runat="server" />
                                </div>
                            </div>
                        </div>
                        <input type="hidden" id="customerBranchId" />
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
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        var pageURL = "CustomerBranch.aspx";
        $(document).ready(function () {

            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
                $("#modal_form input[type=text]").each(function () {
                    $(this).parent().parent().removeClass("has-error");
                });
                $("#cboCustomerId").parent().parent().removeClass("has-error");
                $("#cboGEO").parent().parent().removeClass("has-error");
                $("#cboProvince").parent().parent().removeClass("has-error");
                $("#cboAmphur").parent().parent().removeClass("has-error");
                $("#cboDistrict").parent().parent().removeClass("has-error");
            });

            clearHasError();
        });

        function NewItem() {
            $("#customerBranchId").val("");
            $("#modal_form .modal-title").html("เพิ่มสาขาลูกค้า");
            $("#modal_form").modal("show");
        }

        function editItem(id) {
            $("#customerBranchId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditCustomerBranchData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    for (var i = 0; i < result.d.provinceList.length; i++) {
                        var rowProvince = result.d.provinceList[i];
                        cboProvince.AddItem(rowProvince["name"], rowProvince["id"]);
                    }

                    for (var i = 0; i < result.d.amphurList.length; i++) {
                        var rowAmphur = result.d.amphurList[i];
                        cboAmphur.AddItem(rowAmphur["name"], rowAmphur["id"]);
                    }

                    for (var i = 0; i < result.d.districtList.length; i++) {
                        var rowDistrict = result.d.districtList[i];
                        cboDistrict.AddItem(rowDistrict["name"], rowDistrict["id"]);
                    }

                    $('#txtBranchNo').val(result.d["branch_no"]);
                    $('#txtBranchName').val(result.d["branch_name"]);
                    cboCustomerId.SetValue(result.d["customer_id"]);
                    $('#txtAddressBillTHA').val(result.d["address_bill_tha"]);
                    $('#txtAddressBillENG').val(result.d["address_bill_eng"]);
                    $('#txtAddressInstallTHA').val(result.d["address_install_tha"]);
                    $('#txtAddressInstallENG').val(result.d["address_install_eng"]);
                    cboGEO.SetValue(result.d["geo_id"]);
                    cboProvince.SetValue(result.d["province_id"]);
                    cboAmphur.SetValue(result.d["amphur_id"]);
                    cboDistrict.SetValue(result.d["district_id"]);
                    $('#txtZipcode').val(result.d["zipcode"]);

                    $("#modal_form .modal-title").html("แก้ไขสาขาลูกค้า");
                    $("#modal_form").modal("show");
                }
            });
        }

        function submitFrom() {

            var isValid = false;
            
            var isValidCombo = new Array();
            isValidCombo.push(validateCombobox("cboCustomerId", cboCustomerId.GetValue()));
            isValidCombo.push(validateCombobox("cboGEO", cboGEO.GetValue()));
            isValidCombo.push(validateCombobox("cboProvince", cboProvince.GetValue()));
            isValidCombo.push(validateCombobox("cboAmphur", cboAmphur.GetValue()));
            isValidCombo.push(validateCombobox("cboDistrict", cboDistrict.GetValue()));

            if (isValidCombo.reduce(getSum) > 0) {
                isValid = true;
            }

            $("#modal_form input[type=text]").each(function () {
                if ($(this).val() == "") {
                    $(this).parent().parent().addClass("has-error");
                    isValid = true;
                } else {
                    $(this).parent().parent().removeClass("has-error");
                }
            });

            if (!isValid) {
                var customerBranchId = $("#customerBranchId").val();
                if (customerBranchId == "") {
                    var parametersAdd = {
                        customerBranchAddData: [
                            {
                                branch_no: $('#txtBranchNo').val(),
                                branch_name: $('#txtBranchName').val(),
                                customer_id: parseInt(cboCustomerId.GetValue()),
                                address_bill_tha: $('#txtAddressBillTHA').val(),
                                address_bill_eng: $('#txtAddressBillENG').val(),
                                address_install_tha: $('#txtAddressInstallTHA').val(),
                                address_install_eng: $('#txtAddressInstallENG').val(),
                                geo_id: parseInt(cboGEO.GetValue()),
                                province_id: parseInt(cboProvince.GetValue()),
                                amphur_id: parseInt(cboAmphur.GetValue()),
                                district_id: parseInt(cboDistrict.GetValue()),
                                zipcode: $('#txtZipcode').val()
                            }
                        ]
                    };

                    $.ajax({
                        type: 'POST',
                        url: pageURL + "/InsertCustomerBranch",
                        data: JSON.stringify(parametersAdd),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (result) {
                            if (result.d) {
                                window.location.reload();
                            }
                        }
                    });

                } else if (customerBranchId != "") {
                    var parametersEdit = {
                        customerBranchUpdateData: [
                            {
                                id: customerBranchId,
                                branch_no: $('#txtBranchNo').val(),
                                branch_name: $('#txtBranchName').val(),
                                customer_id: cboCustomerId.GetValue(),
                                address_bill_tha: $('#txtAddressBillTHA').val(),
                                address_bill_eng: $('#txtAddressBillENG').val(),
                                address_install_tha: $('#txtAddressInstallTHA').val(),
                                address_install_eng: $('#txtAddressInstallENG').val(),
                                geo_id: parseInt(cboGEO.GetValue()),
                                province_id: parseInt(cboProvince.GetValue()),
                                amphur_id: parseInt(cboAmphur.GetValue()),
                                district_id: parseInt(cboDistrict.GetValue()),
                                zipcode: $('#txtZipcode').val()
                            }
                        ]
                    };

                    $.ajax({
                        type: 'POST',
                        url: pageURL + "/UpdateCustomerBranch",
                        data: JSON.stringify(parametersEdit),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (result) {
                            if (result.d) {
                                window.location.reload();
                            }
                        }
                    });
                }
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
                           type: 'POST',
                           url: pageURL + "/DeleteCustomerBranch",
                           data: '{id: "' + id + '"}',
                           contentType: 'application/json; charset=utf-8',
                           dataType: 'json',
                           success: function (result) {
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

        function changeStatusItem(id) {
            var is_enable = 0;
            if ($('#item_' + id).is(':checked')) {
                is_enable = 1;
            }

            $.ajax({
                type: 'POST',
                url: "Config.aspx/ChangeStatus",
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_customer_branch"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                    }
                }
            });
        }
    </script>
</asp:Content>
