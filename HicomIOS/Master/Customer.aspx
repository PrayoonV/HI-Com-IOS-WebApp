﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="HicomIOS.Master.Customer" %>

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

        s

        .swal-button {
            padding: 5px 15px;
        }
         input:read-only {
        background-color:white !important;
        }
    </style>
    <script>
        function AddMFG() {
            $('#txtProject').val("");
            $('#txtModel').val("");
            $('#txt_MFG').val("");
            $("#txt_MFG").removeAttr("disabled");
            $("#chkMFG").prop("checked", false);
            $("#chkMFG").removeAttr("disabled");
            $(".product-detail").show();
            $('#txtPressure').val("");
            $('#txtPowerSupply').val("");
            $('#txtHz').val("");
            $('#txtPhase').val("");
            $('#txtUnitwarraty').val("");
            $('#txtAirendwarraty').val("");
            $('#txtservice_fee').val("");
            $('#modal_add_customerMFG').modal("show");
            $("#modal_add_customerMFG .modal-title").html("เพื่ม MFG");
        }

        function onClickMFG() {

            if (!$('#chkMFG').is(':checked')) {
                $("#txt_MFG").removeAttr("disabled");
            } else {
                $("#txt_MFG").attr("disabled", "disabled");
                $("#txt_MFG").val("");
            }
        }



        function OnCustomerGroupChanged() {
            $("#cboCustomerGroup").parent().parent().removeClass("has-error");
        }

        function OnIndustryChanged() {
            $("#cboIndustry").parent().parent().removeClass("has-error");
        }

        function OnCustomerBusinessChanged() {
            $("#cboCustomerBusiness").parent().parent().removeClass("has-error");
        }

        //Call Back Geo to Province
        var lastProvince = null;
        var sess_call_province;
        function OnGeoChanged(cboGeo) {

            clearTimeout(sess_call_province);
            sess_call_province = setTimeout(function validate() {

                if (cboProvince.InCallback()) {
                    lastProvince = cboGeo.GetValue().toString();
                } else {
                    cboProvince.PerformCallback(cboGeo.GetValue().toString());
                }

                cboamphur.performcallback("0"); // clear selected cbodistrict value
                cboDistrict.PerformCallback("0"); // Clear selected cboDistrict value
                $("#txtZipcode").val("");
                $("#cboGEO").parent().parent().removeClass("has-error");

            }, 500);


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
    <button type="button" onclick="newItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มลูกค้า 
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server"
            ClientInstanceName="customerGrid" CssClass="gridResize"
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
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("customer_code") + " - " + Eval("company_name_tha")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("company_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer_code" Caption="Customer Code" Width="50px" Settings-AllowSort="True" SortOrder="Ascending">
                    <DataItemTemplate>
                        <div style="width: 100px; word-wrap: break-word;"><%# Eval("customer_code")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="group_name_tha" Caption="Customer Group" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 100px; word-wrap: break-word;"><%# Eval("group_name_tha")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="company_name_tha" Caption="Customer Name" Width="75px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("company_name_tha")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="industry_name_tha" Caption="Industry Name" Visible="false" />--%>
                <dx:GridViewDataTextColumn FieldName="address_bill_tha" Caption="Address" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("address_bill_tha")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="bill_date" Caption="Invoice Closing Date" Width="50px">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="tax_id" Caption="Tax ID" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("tax_id")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="fax" Caption="Fax" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("fax")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="tel" Caption="Telephone" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("tel")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="mobile" Caption="Mobile" Width="50px">
                    <DataItemTemplate>
                        <div style="width: 120px; word-wrap: break-word;"><%# Eval("mobile")%></div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" Width="50px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                            data-toggle="button" aria-pressed="true" onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                            <div class="handle"></div>
                        </button>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>

            <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="0" />
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
                    <div class="modal-title">เพิ่มลูกค้า</div>
                </div>
                <div class="modal-body text-center">
                    <ul class="nav nav-tabs" style="margin-top: 0px;">
                        <li class="active"><a data-toggle="tab" href="#customerList">ข้อมูลลูกค้า</a></li>
                        <li><a data-toggle="tab" href="#customerAttention">ข้อมูลผู้ติดต่อ</a></li>
                        <li><a data-toggle="tab" href="#customerMFG">Customer MFG</a></li>
                    </ul>
                    <div class="tab-content" style="padding: 10px 0;">
                        <div id="customerList" class="tab-pane fade in active">
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtCustomerCode">Customer Code:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtCustomerCode" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtCompanyNameTHA">Company Name(THA):</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtCompanyNameTHA" runat="server" validate-data />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtCompanyNameENG">Company Name(ENG):</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtCompanyNameENG" runat="server" validate-data />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtCompanyNameOther">Company Name Other:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtCompanyNameOther" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2 no-padding" for="txtAddressBillTHA">Address Bill(THA):</label>
                                    <div class="col-xs-10">
                                        <input type="text" class="form-control" id="txtAddressBillTHA" runat="server" validate-data />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2 no-padding" for="txtAddressBillENG">Address Bill(ENG):</label>
                                    <div class="col-xs-10">
                                        <input type="text" class="form-control" id="txtAddressBillENG" runat="server" validate-data />
                                    </div>
                                </div>
                            </div>
                            <%--<div class="row">
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
                            </div>--%>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="cboCustomerGroup">Customer Group:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboCustomerGroup" CssClass="form-control" runat="server" ClientInstanceName="cboCustomerGroup" TextField="data_text"
                                            ValueField="data_value">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCustomerGroupChanged(s); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="cboCustomerBusiness">Customer Business:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboCustomerBusiness" CssClass="form-control" runat="server" ClientInstanceName="cboCustomerBusiness" TextField="data_text"
                                            ValueField="data_value">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCustomerBusinessChanged(s); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="cboIndustry">Industrial Estate:</label>
                                    <div class="col-xs-8">
                                        <dx:ASPxComboBox ID="cboIndustry" CssClass="form-control" runat="server" ClientInstanceName="cboIndustry" TextField="data_text"
                                            ValueField="data_value">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnIndustryChanged(s); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtTaxId">Tax Id:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtTaxId" runat="server" maxlength="13" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtBranchName">Branch Name:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtBranchName" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtBranchCode">Branch No:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" id="txtBranchCode" runat="server" maxlength="5" />
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
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtFax">Fax:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control isValidTelephone" id="txtFax" runat="server" maxlength="10" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtTelephone">Telephone:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control isValidTelephone" id="txtTelephone" runat="server" maxlength="10" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtMobile">Mobile:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control isValidMobile" id="txtMobile" runat="server" maxlength="11" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" aria-autocomplete="none" for="txtFirstContact">First Contact:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control numberic" autocomplete="off" readonly="true" id="txtFirstContact" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtVAT">VAT:</label>
                                    <div class="col-xs-8">
                                        <!--<input type="text" class="form-control numberic" id="txtVAT_bak" runat="server" />-->
                                        <select class="form-control" id="txtVAT" runat="server">
                                            <option value="0">0 %</option>
                                            <option value="7">7 %</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtCreditTerm">Credit Term:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtCreditTerm" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 no-padding">
                                    <label class="control-label col-xs-4 no-padding" for="txtBillDate">Invoice Closing Date:</label>
                                    <div class="col-xs-8">
                                        <input type="text" class="form-control" id="txtBillDate" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2 no-padding" for="txtRemark">Remark:</label>
                                    <div class="col-xs-10">
                                        <input type="text" class="form-control" id="txtRemark" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <input type="hidden" id="customerId" />
                        </div>
                        <div id="customerAttention" class="tab-pane fade">
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <div class="col-xs-2 text-left">
                                        <button type="button" class="btn-addItem" onclick="addAttention()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่มผู้ติดต่อ
                                        </button>
                                    </div>
                                </div>
                                <div class="form-group col-xs-12 no-padding">
                                    <dx:ASPxGridView ID="gridCustomerAttention" SettingsBehavior-AllowSort="false" ClientInstanceName="gridCustomerAttention" runat="server"
                                        Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="300"
                                        EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridCustomerAttention_CustomCallback"
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
                                                    <a class="btn btn-mini" onclick="editAttention(<%# Eval("id")%>)" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>|
                                                    <a class="btn btn-mini" onclick="deleteAttention(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="attention_name" Caption="Attention Name" CellStyle-HorizontalAlign="Left" Width="350" />
                                            <dx:GridViewDataTextColumn FieldName="attention_tel" Caption="Attention Tel." CellStyle-HorizontalAlign="Left" Width="200" />
                                            <dx:GridViewDataTextColumn FieldName="attention_email" Caption="Attention Email" CellStyle-HorizontalAlign="Left" Width="220" />
                                        </Columns>
                                        <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
                                </div>
                            </div>
                        </div>
                        <div id="customerMFG" class="tab-pane fade">
                            <div class="row">
                                <div class="form-group col-xs-12 no-padding">
                                    <div class="col-xs-2 text-left">
                                        <button type="button" class="btn-addItem" onclick="AddMFG()" style="display:none;">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม MFG
                                        </button>
                                    </div>
                                    <div class="col-xs-10 text-right">
                                            <input type="text" id="txtSearchBoxMFGData" class="form-control" placeholder="กรอกคำค้นหา..." style="display:inline-block; width:50%;" runat="server" onkeypress="searchDataGridMFG(event.keyCode)" />
                                             <button type="button" class="btn-addItem" id="btnSubmitMFGSearch" onclick="searchDataGridMFG(13);">
                                                <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                            </button>
                                    </div>
                                </div>
                                <div class="form-group col-xs-12 no-padding" style="overflow: scroll;">
                                    <dx:ASPxGridView ID="gridMFG" SettingsBehavior-AllowSort="false" ClientInstanceName="gridMFG" runat="server"
                                        Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="300"
                                        Width="100%"
                                        EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridMFG_CustomCallback">
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                        <Paddings Padding="0px" />
                                        <Border BorderWidth="0px" />
                                        <BorderBottom BorderWidth="1px" />
                                        <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                            PageSizeItemSettings-Visible="true">
                                            <PageSizeItemSettings Items="10, 20, 50" />
                                        </SettingsPager>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                                                <DataItemTemplate>
                                                    <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editMFG(<%# Eval("id")%>)" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>|
                                                     <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteMFG(<%# Eval("id")%>, '<%# Eval("project")%>')" title="Delete">
                                                         <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="project" Caption="Project" CellStyle-HorizontalAlign="Left" Width="80px" />
                                            <dx:GridViewDataTextColumn FieldName="model" Caption="Model" CellStyle-HorizontalAlign="Left" Width="60px" />
                                            <dx:GridViewDataTextColumn FieldName="mfg" Caption="MFG" CellStyle-HorizontalAlign="Left" Width="80px" />
                                            <%--<dx:GridViewDataTextColumn FieldName="pressure" Caption="Pressure" CellStyle-HorizontalAlign="Left" Width="60px" />
                                            <dx:GridViewDataTextColumn FieldName="power_supply" Caption="Power Supply" CellStyle-HorizontalAlign="Left" Width="120px" />
                                            <dx:GridViewDataTextColumn FieldName="phase" Caption="Phase" CellStyle-HorizontalAlign="Left" Width="60px" />
                                            <dx:GridViewDataTextColumn FieldName="hz" Caption="Hz" CellStyle-HorizontalAlign="Left" Width="60px" />--%>
                                            <dx:GridViewDataTextColumn FieldName="pressure" Caption="Detail" CellStyle-HorizontalAlign="Left" Width="120px">
                                                <DataItemTemplate>
                                                    Pressure : <%# Eval("pressure")%><br />
                                                    Power Supply : <%# Eval("power_supply")%><br />
                                                    Phase : <%# Eval("phase")%><br />
                                                    Hz : <%# Eval("hz")%>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <%--<dx:GridViewDataTextColumn FieldName="unitwarraty" Caption="Unit Warranty" CellStyle-HorizontalAlign="Left" Width="100px" />
                                            <dx:GridViewDataTextColumn FieldName="airendwarraty" Caption="Air-End Warranty" CellStyle-HorizontalAlign="Left" Width="120px" />
                                            <dx:GridViewDataTextColumn FieldName="service_fee" Caption="Service Fee" CellStyle-HorizontalAlign="Left" Width="100px" />--%>
                                            <dx:GridViewDataTextColumn FieldName="unitwarraty" Caption="Warranty" CellStyle-HorizontalAlign="Left" Width="120px">
                                                <DataItemTemplate>
                                                    Unit : <%# Eval("unitwarraty")%><br />
                                                    Air-End : <%# Eval("airendwarraty")%><br />
                                                    Free Sevice : <%# Eval("service_fee")%>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="date" Caption="Delivery Date" CellStyle-HorizontalAlign="Left" Width="60px" />
                                        </Columns>
                                        <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
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

    <!-- modal ADD Customer attention-->
    <div class="modal " id="modal_customer_attention" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">เพิ่มข้อมูลผู้ติดต่อ</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Attention Name :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control" id="txtCustomerAttentionName" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Attention Tel :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control  isValidNumber isValidMobile" id="txtCustomerAttentionTel" runat="server" maxlength="10" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Attention Email :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control isValidEmail" id="txtCustomerAttentionEmail" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="col-xs-2 text-right label-rigth">&nbsp;</label>
                            <div class="col-xs-10 text-right">
                                <span id="lbValidateAttention" style="color: #a94442; font-weight: 600; display: none; margin-right: 5px;" runat="server">*กรุณาเพิ่ม ข้อมูลผู้ติดต่อ อย่างน้อย 1 รายการ</span>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdCustomerAttentionId" />
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField1" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitAttention()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal ADD Customer MFG-->
    <div class="modal " id="modal_add_customerMFG" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">เพิ่ม MFG</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Project :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control" id="txtProject" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Model :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control" id="txtModel" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">MFG :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control" id="txt_MFG" runat="server" />
                            </div>
                            <div class="col-xs-2">
                                <label class="checkbox">
                                    <input type="checkbox" name="txtchkMFG" id="chkMFG" onclick="onClickMFG()" value="0">
                                    ไม่ระบุ MFG
                                </label>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding product-detail">
                            <label class="control-label col-xs-3 no-padding">Pressure :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtPressure" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding product-detail">
                            <label class="control-label col-xs-3 no-padding">Power Supply :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtPowerSupply" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding product-detail">
                            <label class="control-label col-xs-3 no-padding">Phase :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtPhase" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding product-detail">
                            <label class="control-label col-xs-3 no-padding">Hz :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtHz" runat="server" />
                            </div>
                        </div>
                        <hr />
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Unit Wanranty :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtUnitwarraty" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Air-End Wanranty :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtAirendwarraty" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-3 no-padding">Free Service :</label>
                            <div class="col-xs-7">
                                <input type="text" class="form-control numberic" id="txtservice_fee" runat="server" />
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="hdEditMFGID" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitMFG()">
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
                            <label class="control-label col-xs-4 no-padding" for="Model">Model :</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control" id="Model" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-6 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="MFG">MFG :</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control" id="MFG" runat="server" />
                            </div>
                        </div>

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

    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        var pageURL = "Customer.aspx";
        $(document).ready(function () {
            $("#txtPaidDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true,
            });
            //$("#txtBillDate").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#txtFirstContact").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true,
            });


            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
                $('.nav-tabs li:first-child > a').trigger('click');
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

            // Format Phone Here
            //var phoneField = document.getElementById('txtFax');
            //phoneField.addEventListener('keyup', function(){
            //    var phoneValue = phoneField.value;
            //    var output;
            //    phoneValue = phoneValue.replace(/[^0-9]/g, '');
            //    var area = phoneValue.substr(0, 3);
            //    var pre = phoneValue.substr(3, 3);
            //    var tel = phoneValue.substr(6, 4);
            //    if (area.length < 3) {
            //        //output = "(" + area;
            //        output = area;
            //    } else if (area.length == 3 && pre.length < 3) {
            //        //output = "(" + area + ")" + " " + pre;
            //        output =  area + "-" + pre;
            //    } else if (area.length == 3 && pre.length == 3) {
            //        output =  area  + "-" + pre + tel;
            //    }
            //    phoneField.value = output;

            //});

            var h = window.innerHeight;
            customerGrid.SetHeight(h - 155);

            $(".isValidNumber").keypress(function () {
                var data = $("#" + this.id).val().replace(/[^\d\.]/g, '');
                $("#" + this.id).val(data);
            });

            //$(".isValidMobile").keypress(function () {
            //    var pattern = /[0-9 -()+]+$/;
            //    if ((this.value.length < 10) || (!pattern.test(this.value))) {
            //        $("#" + this.id).focus();
            //        $(this).parent().parent().addClass("has-error");
            //    } else {
            //        $(this).parent().parent().removeClass("has-error");
            //    }
            //});

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

            $(".isValidEmail").keypress(function () {
                var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
                if (!pattern.test(this.value)) {
                    $("#" + this.id).focus();
                    $(this).parent().parent().addClass("has-error");
                } else {
                    $(this).parent().parent().removeClass("has-error");
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
            customerGrid.SetHeight(height);
        });

        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                customerGrid.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function searchDataGridMFG(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxMFGData").val();
                gridMFG.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function newItem() {
            $("#customerId").val(0);
            $.ajax({
                type: 'POST',
                url: pageURL + "/NewCustomerData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    gridCustomerAttention.PerformCallback();
                    gridMFG.PerformCallback();

                    $("#modal_form .modal-title").html("เพิ่มลูกค้า");
                    $("#modal_form").modal("show");

                }
            });
        }

        function editItem(element, id, index) {
            gridView.FocusedRowIndex = index;

            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $("#customerId").val(id);

            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditCustomerData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    console.log(result);
                    $('#txtCustomerCode').val(result.d["customer_code"]);
                    $('#txtCompanyNameTHA').val(result.d["company_name_tha"]);
                    $('#txtCompanyNameENG').val(result.d["company_name_eng"]);
                    $('#txtCompanyNameOther').val(result.d["customer_name_other"]);
                    $('#txtAddressBillTHA').val(result.d["address_bill_tha"]);
                    $('#txtAddressBillENG').val(result.d["address_bill_eng"]);
                    //$('#txtAddressInstallTHA').val(result.d["address_install_tha"]);
                    //$('#txtAddressInstallENG').val(result.d["address_install_eng"]);

                    if (result.d["customer_group_id"] == 0 || result.d["customer_group_id"] == "") {
                        //cboCustomerGroup.AddItem("--โปรดเลือก--", "0");
                        cboCustomerGroup.SetValue("0");
                    } else {
                        cboCustomerGroup.SetValue(result.d["customer_group_id"]);
                        if (cboCustomerGroup.GetSelectedItem() == null) {
                            cboCustomerGroup.SetValue("0");
                        }
                    }

                    if (result.d["customer_business_id"] == 0 || result.d["customer_business_id"] == "") {
                        //cboCustomerBusiness.AddItem("--โปรดเลือก--", "0");
                        cboCustomerBusiness.SetValue("0");
                    } else {
                        cboCustomerBusiness.SetValue(result.d["customer_business_id"]);
                        if (cboCustomerBusiness.GetSelectedItem() == null) {
                            cboCustomerBusiness.SetValue("0");
                        }
                    }

                    if (result.d["customer_industry_id"] == 0 || result.d["customer_industry_id"] == "") {
                        //cboIndustry.AddItem("--โปรดเลือก--", "0");
                        cboIndustry.SetValue("0");
                    } else {
                        cboIndustry.SetValue(result.d["customer_industry_id"]);
                        if (cboIndustry.GetSelectedItem() == null) {
                            cboIndustry.SetValue("0");
                        }
                    }

                    $('#txtTaxId').val(result.d["tax_id"]);

                    $('#txtBranchName').val(result.d["branch_name"]);

                    if (result.d["branch_code"] == "") {
                        $('#txtBranchName').val("");
                    } else {
                        $('#txtBranchCode').val(result.d["branch_code"]);
                    }

                    $('#txtBranchName').val(result.d["branch_name"]);

                    if (result.d["geo_id"] == 0 || result.d["geo_id"] == "") {
                        //cboGEO.AddItem("--โปรดเลือก--", "0");
                        cboGEO.SetValue("0");
                    } else {
                        cboGEO.SetValue(result.d["geo_id"]);
                    }

                    if (result.d["geo_id"] == 0 || result.d["geo_id"] == "") {
                        //cboGEO.AddItem("--โปรดเลือก--", "0");
                        cboGEO.SetValue("0");
                    } else {
                        cboGEO.SetValue(result.d["geo_id"]);
                    }
                    $('#txtZipcode').val(result.d["zipcode"]);
                    $('#txtTelephone').val(result.d["tel"]);
                    $('#txtMobile').val(result.d["mobile"]);
                    $('#txtFax').val(result.d["fax"]);

                    if (result.d["first_contact"] == "01-01-1900" || result.d["first_contact"] == "01/01/1900" || result.d["first_contact"] == "") {
                        $('#txtFirstContact').val("");
                    } else {
                        first_date = result.d["first_contact"];
                        $('#txtFirstContact').val(first_date);
                    }

                    $('#txtVAT').val(result.d["vat"]);
                    $('#txtCreditTerm').val(result.d["credit_term"]);

                    bill_date = result.d["bill_date"];

                    $('#txtBillDate').val(bill_date);
                    $('#txtRemark').val(result.d["remark"]);

                    $("#modal_form .modal-title").html("แก้ไขลูกค้า : " + element.title);
                    $("#modal_form").modal("show");
                    gridCustomerAttention.PerformCallback();
                    gridMFG.PerformCallback();
                    cboProvince.PerformCallback(result.d["geo_id"]);
                    cboAmphur.PerformCallback(result.d["province_id"]);
                    cboDistrict.PerformCallback(result.d["amphur_id"]);

                    setTimeout(function () {
                        if (result.d["province_id"] == 0 || result.d["province_id"] == "") {
                            cboProvince.AddItem("--โปรดเลือก--", "0");
                            cboProvince.SetValue("0");
                        } else {
                            cboProvince.SetValue(result.d["province_id"]);
                        }

                        if (result.d["amphur_id"] == 0 || result.d["amphur_id"] == "") {
                            cboAmphur.AddItem("--โปรดเลือก--", "0");
                            cboAmphur.SetValue("0");
                        } else {
                            cboAmphur.SetValue(result.d["amphur_id"]);
                        }

                        if (result.d["district_id"] == 0 || result.d["district_id"] == "") {
                            cboDistrict.AddItem("--โปรดเลือก--", "0");
                            cboDistrict.SetValue("0");
                        } else {
                            cboDistrict.SetValue(result.d["district_id"]);
                        }

                        $.LoadingOverlay("hide");
                    }, 3000);
                }
            });
        }

        function submitMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var customer_id = $("#customerId").val();
            var mfg_id = $('#hdEditMFGID').val();
            console.log("mfg_id" + mfg_id);
            if (mfg_id == "" || mfg_id == null) {
                var is_demo = 0;
                var project = $('#txtProject').val();
                var model = $('#txtModel').val();
                var mfg = $('#txt_MFG').val();
                var pressure = $('#txtPressure').val();
                var powerSupply = $('#txtPowerSupply').val();
                var phase = $('#txtPhase').val();
                var hz = $('#txtHz').val();
                var unitwarraty = $('#txtUnitwarraty').val();
                var airendwarraty = $('#txtAirendwarraty').val();
                var service_fee = $('#txtservice_fee').val();

                if ($('#chkMFG').is(':checked')) {
                    is_demo = 1;
                    mfg = "";
                }

                if (model == "" || model == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Model");
                    return;
                }
                if (unitwarraty == "" || unitwarraty == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Unit Wanranty");
                    return;
                }
                if (project == "" || project == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Project");
                    return;
                }
                if (airendwarraty == "" || airendwarraty == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Air-End Wanranty");
                    return;
                }
                if (service_fee == "" || service_fee == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Free service");
                    return;
                }
                if (!$('#chkMFG').is(':checked')) {
                    if (mfg == "" || mfg == undefined) {
                        $.LoadingOverlay("hide");

                        return;
                    }
                }
                console.log("is_demo==" + is_demo);
                $.ajax({
                    type: "POST",
                    url: "Customer.aspx/AddMFG",
                    data: '{model : "' + model + '",mfg : "' + mfg +
                        '",project : "' + project + '",is_demo : "' + is_demo + '",unitwarraty : "' + unitwarraty +
                        '",airendwarraty : "' + airendwarraty + '",service_fee : "' + service_fee + '", customer_id : "' + customer_id + '", pressure : "' + pressure + '", power_supply : "' + powerSupply + '", phase : "' + phase + '", hz : "' + hz + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "-1") {
                            alertWarning("MFG ซ้ำ, กรุณากรอกตรวจสอบอีกครั้ง");
                        } else {
                            $('#modal_add_customerMFG').modal("hide");
                            gridMFG.PerformCallback();
                        }
                        $.LoadingOverlay("hide");

                    },
                    failure: function (response) {
                        alertWarning(response.d);
                        $.LoadingOverlay("hide");
                    }
                });
            } else {
                var is_demo = 0;
                var project = $('#txtProject').val();
                var model = $('#txtModel').val();
                var mfg = $('#txt_MFG').val();
                var pressure = $('#txtPressure').val();
                var powerSupply = $('#txtPowerSupply').val();
                var phase = $('#txtPhase').val() == '' ? 0 : $('#txtPhase').val();
                var hz = $('#txtHz').val() == '' ? 0 : $('#txtHz').val();
                var unitwarraty = $('#txtUnitwarraty').val();
                var airendwarraty = $('#txtAirendwarraty').val();
                var service_fee = $('#txtservice_fee').val();

                if ($('#chkMFG').is(':checked')) {
                    is_demo = 1;
                    mfg = "";
                }

                if (model == "" || model == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Model");
                    return;
                }
                if (unitwarraty == "" || unitwarraty == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Unit Wanranty");
                    return;
                }
                if (project == "" || project == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Project");
                    return;
                }
                if (airendwarraty == "" || airendwarraty == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Air-End Wanranty");
                    return;
                }
                if (service_fee == "" || service_fee == undefined) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณากรอก Free service");
                    return;
                }
                if (!$('#chkMFG').is(':checked')) {
                    if (mfg == "" || mfg == undefined) {
                        $.LoadingOverlay("hide");

                        return;
                    }
                }
                //console.log("is_demo==" + is_demo);
                $.ajax({
                    type: "POST",
                    url: "Customer.aspx/SubmitEditMFG",
                    data: '{mfg_id: "' + mfg_id + '",model : "' + model + '",mfg : "' + mfg +
                        '",project : "' + project + '",is_demo : "' + is_demo + '",unitwarraty : "' + unitwarraty +
                        '",airendwarraty : "' + airendwarraty + '",service_fee : "' + service_fee + '", pressure : "' + pressure + '", power_supply : "' + powerSupply + '", phase : "' + phase + '", hz : "' + hz + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        $('#hdEditMFGID').val(0);

                        $('#txtProject').val("");
                        $('#txtModel').val("");
                        $('#txt_MFG').val("");
                        $('#chkMFG').is(':checked', false);
                        $('#txtModel').val("");
                        $(".product-detail").show();
                        $('#txtPressure').val("");
                        $('#txtPowerSupply').val("");
                        $('#txtHz').val("");
                        $('#txtPhase').val("");
                        $('#txtUnitwarraty').val("");
                        $('#txtAirendwarraty').val("");
                        $('#txtservice_fee').val("");

                        gridMFG.PerformCallback();
                        $('#modal_add_customerMFG').modal("hide");
                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertWarning(response.d);
                        $.LoadingOverlay("hide");
                    }
                });
            }


        }

        function editMFG(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#hdEditMFGID').val(e);
            $.ajax({
                type: "POST",
                url: pageURL + "/EditMFG",
                data: '{id : ' + e + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProject').val(data.project);
                    $('#txtModel').val(data.model);
                    $('#txt_MFG').val(data.mfg);
                    if (data.mfg == '') {
                        $("#txt_MFG").attr("disabled", "disabled");
                        $("#chkMFG").prop("checked", true);
                        $("#chkMFG").removeAttr("disabled");
                    } else {
                        $("#txt_MFG").removeAttr("disabled");
                        $("#chkMFG").prop("checked", false);
                        $("#chkMFG").attr("disabled", "disabled");
                    }
                    //$('#chkMFG').prop('disabled', true);
                    $('#txtModel').val(data.model);

                    if (data.product_id != '') {
                        $(".product-detail").hide();
                        $('#txtPressure').val(data.pressure);
                        $('#txtPowerSupply').val(data.power_supply);
                        $('#txtHz').val(data.hz);
                        $('#txtPhase').val(data.phase);
                    } else {
                        $(".product-detail").show();
                        $('#txtPressure').val(data.pressure);
                        $('#txtPowerSupply').val(data.power_supply);
                        $('#txtHz').val(data.hz);
                        $('#txtPhase').val(data.phase);
                    }

                    $('#txtUnitwarraty').val(data.unitwarraty);
                    $('#txtAirendwarraty').val(data.airendwarraty);
                    $('#txtservice_fee').val(data.service_fee);
                    $('#modal_add_customerMFG').modal("show");
                    $("#modal_add_customerMFG .modal-title").html("แก้ไข MFG");
                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }



        function validateData() {
            if ($("#txtCompanyNameTHA").val() == "") {
                $("#txtCompanyNameTHA").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#customerList").addClass("active in");
                $('a[href*=customerList]').click();
                $("#txtCompanyNameTHA").focus();

                return false;
            } else if ($("#txtCompanyNameENG").val() == "") {
                $("#txtCompanyNameENG").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#customerList").addClass("active in");
                $('a[href*=customerList]').click();
                $("#txtCompanyNameENG").focus();

                return false;
            }
            else if ($("#txtAddressBillTHA").val() == "") {
                $("#txtAddressBillTHA").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#customerList").addClass("active in");
                $('a[href*=customerList]').click();
                $("#txtAddressBillTHA").focus();

                return false;
            }
            else if ($("#txtAddressBillENG").val() == "") {
                $("#txtAddressBillENG").parent().parent().addClass("has-error");
                $(".tab-pane.fade").removeClass("active in");
                $("#customerList").addClass("active in");
                $('a[href*=customerList]').click();
                $("#txtAddressBillENG").focus();

                return false;
            }
            else if ($("#txtTaxId").val() != "" && $("#txtTaxId").val().length < 13) {
                alertError('Please input Tax id minumum 13 digit');
                $("#txtTaxId").focus();

                return false;
            }
            $.ajax({
                type: 'POST',
                url: pageURL + "/ValidateAttentionData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d != "") {
                        setTimeout(function () {
                            $("#lbValidateAttention").show();
                            $(".tab-pane.fade").removeClass("active in");
                            $("#customerAttention").addClass("active in");
                            $('a[href*=customerAttention]').click();
                            return false;
                        }, 100);
                    }
                    else {
                        submitFrom();
                    }
                }
            });
        }

        function submitFrom() {

            var customerId = $("#customerId").val();
            console.log($('#txtBranchCode').val());
            var parametersAdd = {
                customerAddData: [
                    {
                        id: parseInt(customerId),
                        customer_code: $('#txtCustomerCode').val(),
                        company_name_tha: $('#txtCompanyNameTHA').val(),
                        company_name_eng: $('#txtCompanyNameENG').val(),
                        customer_name_other: $('#txtCompanyNameOther').val(),
                        address_bill_tha: $('#txtAddressBillTHA').val(),
                        address_bill_eng: $('#txtAddressBillENG').val(),
                        //address_install_tha: $('#txtAddressInstallTHA').val(),
                        //address_install_eng: $('#txtAddressInstallENG').val(),
                        customer_group_id: cboCustomerGroup.GetValue() ? cboCustomerGroup.GetValue() : "0",
                        customer_business_id: cboCustomerBusiness.GetValue() ? cboCustomerBusiness.GetValue() : "0",
                        customer_industry_id: cboIndustry.GetValue() ? cboIndustry.GetValue() : "0",
                        tax_id: $('#txtTaxId').val(),
                        branch_name: $('#txtBranchName').val(),
                        branch_code: $('#txtBranchCode').val(),
                        geo_id: cboGEO.GetValue() ? cboGEO.GetValue() : "0",
                        province_id: cboProvince.GetValue() ? cboProvince.GetValue() : "0",
                        amphur_id: cboAmphur.GetValue() ? cboAmphur.GetValue() : "0",
                        district_id: cboDistrict.GetValue() ? cboDistrict.GetValue() : "0",
                        zipcode: $('#txtZipcode').val(),
                        tel: $('#txtTelephone').val(),
                        mobile: $('#txtMobile').val(),
                        fax: $("#txtFax").val(),
                        first_contact: $("#txtFirstContact").val(),
                        vat: $("#txtVAT").val() ? $("#txtVAT").val() : "0",
                        credit_term: $("#txtCreditTerm").val() ? $("#txtCreditTerm").val() : "0",
                        bill_date: $("#txtBillDate").val(),
                        remark: $("#txtRemark").val()
                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: pageURL + "/InsertCustomer",
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
                            url: pageURL + "/DeleteCustomer",
                            data: "{id : " + id + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                $('#hdSelectedBorrowDetailId').val(0);
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        window.location.reload();
                                    });
                            },
                            failure: function (response) {
                            }
                        });
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



        function addAttention() {
            $('#txtCustomerAttentionName').val("");
            $('#txtCustomerAttentionTel').val("");
            $('#txtCustomerAttentionEmail').val("");
            $('#hdCustomerAttentionId').val(0);
            $('#modal_customer_attention').modal('show');
        }

        function editAttention(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "Customer.aspx/EditCustomerAttention",
                data: "{id : " + e + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;

                    $('#hdCustomerAttentionId').val(e);
                    $('#txtCustomerAttentionName').val(data.attention_name);
                    $('#txtCustomerAttentionTel').val(data.attention_tel);
                    $('#txtCustomerAttentionEmail').val(data.attention_email);

                    $('#modal_customer_attention').modal('show');

                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function submitAttention() {
            $("#lbValidateAttention").hide();
            var isValid = true;
            $("#modal_customer_attention input[type=text]").each(function () {
                $(this).parent().parent().removeClass("has-error");
            });

            if ($('#txtCustomerAttentionName').val() == "") {
                isValid = false;
                $('#txtCustomerAttentionName').parent().parent().addClass("has-error");
            } else {
                isValid = true;
                $('#txtCustomerAttentionName').parent().parent().removeClass("has-error");
            }

            /*if ($('#txtCustomerAttentionTel').val() != "") {
                var pattern_phone = /^[0-9]{10}/i;

                if (!pattern_phone.test($('#txtCustomerAttentionTel').val())) {
                    isValid = true;
                    console.log("txtCustomerAttentionTel");
                    $('#txtCustomerAttentionTel').parent().parent().addClass("has-error");
                } else {
                    isValid = false;
                    $('#txtCustomerAttentionTel').parent().parent().removeClass("has-error");
                }
            }

            if ($('#txtCustomerAttentionEmail').val() != "") {
                var pattern_email = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (!pattern_email.test($('#txtCustomerAttentionEmail').val())) {
                    isValid = true;
                    console.log("txtCustomerAttentionEmail");
                    $('#txtCustomerAttentionEmail').parent().parent().addClass("has-error");
                } else {
                    isValid = false;
                    $('#txtCustomerAttentionEmail').parent().parent().removeClass("has-error");
                }
            }*/

            if (isValid) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var attention_name = $('#txtCustomerAttentionName').val();
                var attention_tel = $('#txtCustomerAttentionTel').val();
                var attention_email = $('#txtCustomerAttentionEmail').val();
                var id = ($('#hdCustomerAttentionId').val() == "" ? 0 : $('#hdCustomerAttentionId').val());

                $.ajax({
                    type: "POST",
                    url: "Customer.aspx/SubmitCustomerAttention",
                    data: "{id : " + id + " , name : '" + attention_name + "' , tel : '" + attention_tel + "' , email : '" + attention_email + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#hdCustomerAttentionId').val(0);
                        $('#txtCustomerAttentionName').val("");
                        $('#txtCustomerAttentionTel').val("");
                        $('#txtCustomerAttentionEmail').val("");

                        gridCustomerAttention.PerformCallback();

                        $('#modal_customer_attention').modal('hide');

                        $.LoadingOverlay("hide");

                        var s = '<i class="fa fa-save" aria-hidden="true"></i> เพิ่ม';
                        $('.btn-saveItem').html(s);
                    },
                    failure: function (response) {
                        //alert(response.d);
                    }
                });
            }
        }

        function deleteAttention(e) {
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
                            url: "Customer.aspx/DeleteCustomerAttention",
                            data: "{id : " + e + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                $('#hdSelectedBorrowDetailId').val(0);
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridCustomerAttention.PerformCallback();
                                    });
                            },
                            failure: function (response) {
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_customer"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                        var txtSearch = $("#txtSearchBoxData").val();
                        customerGrid.PerformCallback(txtSearch.toString());

                    }
                }
            });
        }
    </script>
</asp:Content>