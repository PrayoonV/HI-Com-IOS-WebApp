﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="HicomIOS.Master.Employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        #div_user {
            display: none;
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
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script>
        function OnUserGroupChanged() {
            $("#cboUserGroup").parent().parent().removeClass("has-error");
            cboUserGroup.GetMainElement().style.border = "";
        }

        function OnPrefixChanged() {
            $("#cboPrefix").parent().parent().removeClass("has-error");
            cboPrefix.GetMainElement().style.border = "";
        }

        function OnDepartmentChanged() {
            $("#cboDepartment").parent().parent().removeClass("has-error");
            cboDepartment.GetMainElement().style.border = "";
        }

        function OnPositionChanged() {
            $("#cboPosition").parent().parent().removeClass("has-error");
            cboPosition.GetMainElement().style.border = "";
        }

        // Image Function
        function onUploadControlFileUploadComplete(s, e) {
            if (e.isValid)
                document.getElementById("uploadedImage").src = "/Images/" + e.callbackData;
            setElementVisible("uploadedImage", e.isValid);
        }
        function setElementVisible(elementId, visible) {
            document.getElementById(elementId).className = visible ? "" : "hidden";
        }
        function onUploadSignComplete(s, e) {
            if (e.isValid)
                document.getElementById("imgSign").src = "/Images/Sign/" + e.callbackData;
            setElementVisible("imgSign", e.isValid);
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
        &nbsp;เพิ่มพนักงาน
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="employeeGrid"
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
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(this, <%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="<%# Eval("employee_code") + " - " + Eval("first_name")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("first_name")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Picture" FieldName="cover_image" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <img style="width: 50px; height: 50px;" src="/Images/<%# Eval("cover_image")%>" alt="<%# Eval("employee_code")%>">
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="employee_code" Caption="Employee Code" Width="50px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="employee_name" Caption="Full Name" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="mobile" Caption="Mobile" Width="80px" />
                <dx:GridViewDataTextColumn FieldName="email" Caption="Email" Width="80px" />
                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="55px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                            data-toggle="button" aria-pressed="true" onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %>>
                            <div class="handle"></div>
                        </button>
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
                        <div class="modal-title">เพิ่มลูกค้า</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboUserGroup">User Group</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboUserGroup" CssClass="form-control" runat="server"
                                        ClientInstanceName="cboUserGroup" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnUserGroupChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="div_user">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtUsername">Username:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" readonly="" id="txtUsername" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtPassword">New Password:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtPassword" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEmployeeCode">Employee Code:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEmployeeCode" runat="server" maxlength="5" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboPrefix">Prefix:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboPrefix" CssClass="form-control" runat="server" ClientInstanceName="cboPrefix" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnPrefixChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtFirstName">First Name:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtFirstName" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtLastName">Last Name:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtLastName" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtCardId">Card ID:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control isValidNumber" id="txtCardId" runat="server" maxlength="13" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboDepartment">Department:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboDepartment" CssClass="form-control" runat="server" ClientInstanceName="cboDepartment" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnDepartmentChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboPosition">Position:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboPosition" CssClass="form-control" runat="server" ClientInstanceName="cboPosition" TextField="data_text"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnPositionChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtNickName">Nick Name:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtNickName" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtLineId">Line ID:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtLineId" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBrithDay">Birth Day:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control numberic" autocomplete="off" readonly="true" id="txtBrithDay" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEmail">Email:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEmail" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtMobile">Mobile:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control isValidMobile " id="txtMobile" runat="server" maxlength="11" />
                                    <%--ismobilenumberic--%>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="imgCoverImage">Image:</label>
                                <div class="col-xs-6">
                                    <dx:ASPxUploadControl ID="imgCoverImage" ClientInstanceName="UploadControl" runat="server" UploadMode="Auto" AutoStartUpload="True" Width="100%"
                                        ShowProgressPanel="True" CssClass="uploadControl" DialogTriggerID="externalDropZone" OnFileUploadComplete="UploadControl_FileUploadComplete">
                                        <AdvancedModeSettings EnableDragAndDrop="false" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZone" DropZoneText="" />
                                        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage">
                                            <ErrorStyle CssClass="validationMessage"></ErrorStyle>
                                        </ValidationSettings>
                                        <BrowseButton Text="Upload" />
                                        <DropZoneStyle CssClass="uploadControlDropZone" />
                                        <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                        <ClientSideEvents
                                            FileUploadComplete="onUploadControlFileUploadComplete"></ClientSideEvents>
                                    </dx:ASPxUploadControl>

                                </div>
                                <div class="col-xs-2">
                                    <img id="uploadedImage" runat="server" width="50" height="50" src="#" class="hidden" alt="" />&nbsp;
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="rdoGrade">Gender:</label>
                                <div class="col-xs-4">
                                    <label class="radio-inline">
                                        <input type="radio" name="rdoGrade" id="genderMale" value="0" />Male
                                    </label>
                                    <label class="radio-inline">
                                        <input type="radio" name="rdoGrade" id="genderFemale" value="1" />Female
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtAddress">Address:</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtAddress" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboProvince">Province:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboProvince" CssClass="form-control" runat="server" ClientInstanceName="cboProvince"
                                        TextField="data_text" ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboAmphur">Amphur:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboAmphur" CssClass="form-control" runat="server" ClientInstanceName="cboAmphur" TextField="data_text"
                                        ValueField="data_value" OnCallback="cboAmphur_Callback">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnAmphurChanged(s); }" EndCallback=" OnEndCallback2" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="cboDistrict">District:</label>
                                <div class="col-xs-8">
                                    <dx:ASPxComboBox ID="cboDistrict" CssClass="form-control" runat="server" ClientInstanceName="cboDistrict" TextField="data_text"
                                        ValueField="data_value" OnCallback="cboDistrict_Callback">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnDistrictChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtZipcode">Zipcode:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtZipcode" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="imgSign">ลายเซ็น:</label>
                                <div class="col-xs-6">
                                    <dx:ASPxUploadControl ID="uploadSign" ClientInstanceName="uploadSign" runat="server" UploadMode="Auto" AutoStartUpload="True" Width="100%"
                                        ShowProgressPanel="True" CssClass="uploadControl" DialogTriggerID="externalDropZone" OnFileUploadComplete="uploadSign_FileUploadComplete">
                                        <AdvancedModeSettings EnableDragAndDrop="false" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZone" DropZoneText="" />
                                        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage">
                                            <ErrorStyle CssClass="validationMessage"></ErrorStyle>
                                        </ValidationSettings>
                                        <BrowseButton Text="Upload" />
                                        <DropZoneStyle CssClass="uploadControlDropZone" />
                                        <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                        <ClientSideEvents
                                            FileUploadComplete="onUploadSignComplete"></ClientSideEvents>
                                    </dx:ASPxUploadControl>

                                </div>
                                <div class="col-xs-2">
                                    <img id="imgSign" runat="server" width="50" height="50" src="#" class="hidden" alt="" />&nbsp;
                                </div>
                            </div>
                        </div>
                        <input type="hidden" id="employeeId" />
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
    </div>
    <script type="text/javascript">
        var pageURL = "Employee.aspx";
        $(document).ready(function () {
            $("#txtBrithDay").datepicker({ dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });

            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
                $("#modal_form input[type=text]").each(function () {
                    $(this).parent().parent().removeClass("has-error");
                });

                $("#cboUserGroup").parent().parent().removeClass("has-error");
                $("#cboPrefix").parent().parent().removeClass("has-error");
                $("#cboDepartment").parent().parent().removeClass("has-error");
                $("#cboPosition").parent().parent().removeClass("has-error");
            });

            clearHasError();

            $("#modal_form input[type=radio]").click(function () {
                $("#genderMale").parent().parent().parent().removeClass("has-error");
            });
            var h = window.innerHeight;
            employeeGrid.SetHeight(h - 155);

            $(".isValidNumber").keypress(function () {
                var data = $("#" + this.id).val().replace(/[^\d\.]/g, '');
                $("#" + this.id).val(data);
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

            $(".ismobilenumberic").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
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
            employeeGrid.SetHeight(height);
        });


        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                employeeGrid.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function newItem() {
            $("#div_user").hide();
            $("#txtEmployeeCode").prop('readonly', false);
            $("#uploadedImage").addClass("hidden");
            $("#imgSign").addClass("hidden");
            cboUserGroup.GetMainElement().style.border = "";
            $("#employeeId").val("");
            $.ajax({
                type: 'POST',
                url: pageURL + "/NewEmployeeData",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                     cboDepartment.AddItem("--โปรดเลือก--", "0");
                        cboDepartment.SetValue("0");

                      cboPosition.AddItem("--โปรดเลือก--", "0");
                    cboPosition.SetValue("0");

                    $("#modal_form .modal-title").html("เพิ่มพนักงาน");
                    $("#modal_form").modal("show");
                }
            });

        }

        function editItem(element, id, index) {
            gridView.FocusedRowIndex = index;

            $("#employeeId").val(id);
            cboUserGroup.GetMainElement().style.border = "";
            $("#div_user").show();
            $("#txtEmployeeCode").prop('readonly', true);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditEmployeeData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d.amphurList == 0) {
                        cboAmphur.AddItem("--โปรดเลือก--", "0");
                        cboAmphur.SetValue("0");
                    } else {
                        for (var i = 0; i < result.d.amphurList.length; i++) {
                            var rowAmphur = result.d.amphurList[i];
                            cboAmphur.AddItem(rowAmphur["name"], rowAmphur["id"]);
                        }
                    }

                    if (result.d.districtList == 0) {
                        cboDistrict.AddItem("--โปรดเลือก--", "0");
                        cboDistrict.SetValue("0");
                    } else {
                        for (var i = 0; i < result.d.districtList.length; i++) {
                            var rowDistrict = result.d.districtList[i];
                            cboDistrict.AddItem(rowDistrict["name"], rowDistrict["id"]);
                        }
                    }

                    $('#txtEmployeeCode').val(result.d["employee_code"]);
                    $("#txtUsername").val(result.d["username"]);
                    if (result.d["prefix_id"] == 0 || result.d["prefix_id"] == "") {
                        cboPrefix.AddItem("--โปรดเลือก--", "0");
                        cboPrefix.SetValue("0");
                    } else {
                        cboPrefix.SetValue(result.d["prefix_id"]);
                    }

                    $('#txtFirstName').val(result.d["first_name"]);
                    $('#txtLastName').val(result.d["last_name"]);
                    $('#txtCardId').val(result.d["card_id"]);
                    $('input[name=rdoGrade]:checked').val(result.d["gender"]);

                    if (result.d["department_id"] == 0 || result.d["department_id"] == "") {
                        cboDepartment.AddItem("--โปรดเลือก--", "0");
                        cboDepartment.SetValue("0");
                    } else {
                        cboDepartment.SetValue(result.d["department_id"]);
                    }

                    if (result.d["position_id"] == 0 || result.d["position_id"] == "") {
                        cboPosition.AddItem("--โปรดเลือก--", "0");
                        cboPosition.SetValue("0");
                    } else {
                        cboPosition.SetValue(result.d["position_id"]);
                    }

                    $('#txtNickName').val(result.d["nick_name"]);
                    $('#txtLineId').val(result.d["line_id"]);
                    $("#txtMobile").val(result.d["mobile"]);
                    $("#txtEmail").val(result.d["email"]);
                    $("#txtBrithDay").val(result.d["birth_day"]);
                    $("#txtAddress").val(result.d["address"]);

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

                    $("#txtZipcode").val(result.d["zipcode"]);

                    if (result.d["group_id"] == 0 || result.d["group_id"] == "") {
                        cboUserGroup.AddItem("--โปรดเลือก--", "0");
                        cboUserGroup.SetValue("0");
                    } else {
                        cboUserGroup.SetValue(result.d["group_id"]);
                    }

                    if (result.d["gender"] == 0) {
                        $("#genderMale").prop("checked", true);
                    } else {
                        $("#genderFemale").prop("checked", true);
                    }
                    if (result.d.cover_image != "") {
                        console.log(result.d.cover_image);
                        document.getElementById("uploadedImage").src = "/Images/" + result.d.cover_image;
                        document.getElementById("uploadedImage").className = "";
                    }
                    else {
                        document.getElementById("uploadedImage").className = "hidden";
                    }

                    if (result.d.sign_image != "") {
                        console.log(result.d.sign_image);
                        document.getElementById("imgSign").src = "/Images/Sign/" + result.d.sign_image;
                        document.getElementById("imgSign").className = "";

                    }
                    else {
                        document.getElementById("imgSign").className = "hidden";
                    }
                    $("#modal_form .modal-title").html("แก้ไขพนักงาน : " + element.title);
                    $("#modal_form").modal("show");
                }
            });
        }

        function validateData() {

            if (!cboUserGroup.GetValue()) {
                $("#cboUserGroup").parent().parent().addClass("has-error");
                cboUserGroup.GetMainElement().style.border = "1px solid #a94442";

                return false;
            } else if ($("#txtEmployeeCode").val() == "") {
                $("#txtEmployeeCode").parent().parent().addClass("has-error");
                $("#txtEmployeeCode").focus()

                return false;
            }
            else if ($("#txtEmployeeCode").val() != "" && $("#txtEmployeeCode").val().length < 5) {
                alertError('Please input employee code minumum 5 digit');
                $("#txtEmployeeCode").focus()

                return false;
            }
            else if ($('#txtCardId').val() != "" && $("#txtCardId").val().length < 13) {
                alertError('Please input card id minumum 13 digit');
                $("#txtCardId").focus()

                return false;

            } else if (!cboPrefix.GetValue()) {
                $("#cboPrefix").parent().parent().addClass("has-error");
                cboPrefix.GetMainElement().style.border = "1px solid #a94442";

                return false;
            } else if ($("#txtFirstName").val() == "") {
                $("#txtFirstName").parent().parent().addClass("has-error");
                $("#txtFirstName").focus();

                return false;
            } else if ($("#txtLastName").val() == "") {
                $("#txtLastName").parent().parent().addClass("has-error");
                $("#txtLastName").focus();

                return false;
            } else if (cboDepartment.GetValue() == 0) {
                $("#cboDepartment").parent().parent().addClass("has-error");
                cboDepartment.GetMainElement().style.border = "1px solid #a94442";

                return false;
            } else if (cboPosition.GetValue() == 0) {
                $("#cboPosition").parent().parent().addClass("has-error");
                cboPosition.GetMainElement().style.border = "1px solid #a94442";

                return false;
            } else if ($("#txtEmail").val() == "") {
                $("#txtEmail").parent().parent().addClass("has-error");
                $("#txtEmail").focus();

                return false;
            } else if ($("#txtMobile").val() == "") {
                $("#txtMobile").parent().parent().addClass("has-error");
                $("#txtMobile").focus();

                return false;
            } else if ($("#txtMobile").val() != "" && $("#txtMobile").val().length < 11) {
                  alertError('Please input mobile phone minumum 10 digit');
                $("#txtMobile").focus();

                return false;
            } else if ($("#genderMale").prop("checked") == false && $("#genderFemale").prop("checked") == false) {
                $("#genderMale").parent().parent().parent().addClass("has-error");

                return false;
            } else {
                submitFrom();
            }
        }

        function submitFrom() {
            var employeeId = $("#employeeId").val();

            if (employeeId == "") {
                var parametersAdd = {
                    employeeAddData: [
                        {
                            employee_code: $('#txtEmployeeCode').val(),
                            prefix_id: cboPrefix.GetValue() ? cboPrefix.GetValue() : "0",
                            first_name: $('#txtFirstName').val(),
                            last_name: $('#txtLastName').val(),
                            card_id: $('#txtCardId').val(),
                            gender: $('input[name=rdoGrade]:checked').val(),
                            cover_image: "",
                            department_id: cboDepartment.GetValue() ? cboDepartment.GetValue() : "0",
                            position_id: cboPosition.GetValue() ? cboPosition.GetValue() : "0",
                            nick_name: $('#txtNickName').val(),
                            line_id: $('#txtLineId').val(),
                            mobile: $("#txtMobile").val(),
                            email: $("#txtEmail").val(),
                            birth_day: $("#txtBrithDay").val(),
                            address: $("#txtAddress").val(),
                            province_id: cboProvince.GetValue() ? cboProvince.GetValue() : "0",
                            amphur_id: cboAmphur.GetValue() ? cboAmphur.GetValue() : "0",
                            district_id: cboDistrict.GetValue() ? cboDistrict.GetValue() : "0",
                            zipcode: $("#txtZipcode").val(),
                            group_id: cboUserGroup.GetValue() ? cboUserGroup.GetValue() : "0"
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/InsertEmployee",
                    data: JSON.stringify(parametersAdd),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d == "") {
                            window.location.reload();
                        } else {
                            alertError(result.d);
                        }
                    }
                });

            } else if (employeeId != "") {
                var username = "";
                if ($("#txtUsername").val() == "") {
                    username = "HI00" + $("#txtEmployeeCode").val()
                }
                var parametersEdit = {
                    employeeUpdateData: [
                        {
                            id: employeeId,
                            employee_code: $('#txtEmployeeCode').val(),
                            username: username,
                            password: $('#txtPassword').val(),
                            prefix_id: cboPrefix.GetValue() ? cboPrefix.GetValue() : "0",
                            first_name: $('#txtFirstName').val(),
                            last_name: $('#txtLastName').val(),
                            card_id: $('#txtCardId').val(),
                            gender: $('input[name=rdoGrade]:checked').val(),
                            cover_image: "",
                            department_id: cboDepartment.GetValue() ? cboDepartment.GetValue() : "0",
                            position_id: cboPosition.GetValue() ? cboPosition.GetValue() : "0",
                            nick_name: $('#txtNickName').val(),
                            line_id: $('#txtLineId').val(),
                            mobile: $("#txtMobile").val(),
                            email: $("#txtEmail").val(),
                            birth_day: $("#txtBrithDay").val(),
                            address: $("#txtAddress").val(),
                            province_id: cboProvince.GetValue() ? cboProvince.GetValue() : "0",
                            amphur_id: cboAmphur.GetValue() ? cboAmphur.GetValue() : "0",
                            district_id: cboDistrict.GetValue() ? cboDistrict.GetValue() : "0",
                            zipcode: $("#txtZipcode").val(),
                            group_id: cboUserGroup.GetValue() ? cboUserGroup.GetValue() : "0"
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/UpdateEmployee",
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
                            url: pageURL + "/DeleteEmployee",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                console.log(data.d);
                                if (data.d == 'success') {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                            window.location.reload();
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



        //function changeStatusItem(id) {
        //    var is_enable = 0;
        //    if ($('#item_' + id).is(':checked')) {
        //        is_enable = 1;
        //    }

        //    $.ajax({
        //        type: 'POST',
        //        url: "Config.aspx/ChangeStatus",
        //        data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_employee"}',
        //        contentType: 'application/json; charset=utf-8',
        //        dataType: 'json',
        //        success: function (result) {
        //            if (result.d) {
        //                //window.location.reload();
        //            }
        //        }
        //    });
        //} 
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_employee"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                        var txtSearch = $("#txtSearchBoxData").val();
                        employeeGrid.PerformCallback(txtSearch.toString());
                    }
                }
            });
        }


        function genEmpCode() {
            var empCodeLength = $("#txtEmployeeCode").val().length;
            console.log(empCodeLength);
        }
    </script>
</asp:Content>
