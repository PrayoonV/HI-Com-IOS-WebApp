<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SupplierBranch.aspx.cs" Inherits="HicomIOS.Master.SupplierBranch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }
        .btn-labelEdit{
                cursor: pointer; 
       }
          a:link {
            text-decoration: none !important;
        }

        a:active {
            text-decoration: underline !important;
        }

        a:hover {
            text-decoration: underline !important;;
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
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script>
        $('#gridCell').css({
            'height': '550px;'
        });
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
        &nbsp;เพิ่มสาขาผู้จัดจำหน่าย
    </button>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="supplierBranchGrid"
            Width="100%" KeyFieldName="id" OnCustomCallback="gridView_CustomCallback">
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
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("branch_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="supplier_code" Caption="Supplier Code" Width="50px" >
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                           <%# Eval("supplier_code")%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="branch_no" Caption="Branch No" Width="50px" />
                <dx:GridViewDataTextColumn FieldName="branch_name_tha" Caption="Branch Name (THA)" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="branch_name_tha" Caption="Branch Name (THA)" Width="150px" />
                 <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> >
                             <div class="handle"></div>
                        </button>
                        </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsContextMenu Enabled="true">
                <ColumnMenuItemVisibility ShowFooter="false" ShowFilterRow="false" GroupByColumn="false" ShowGroupPanel="false" />
                <RowMenuItemVisibility NewRow="false"></RowMenuItemVisibility>
            </SettingsContextMenu>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>

        <div class="modal fade" id="modal_form" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <div class="modal-title">เพิ่มสาขาผู้จัดจำหน่าย</div>
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
                                <label class="control-label col-xs-4 no-padding" for="txtBranchNo">Branch No:</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtBranchNo" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBranchNameTHA">Branch Name(THA):</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtBranchNameTHA" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtBranchNameENG">Branch Name(ENG):</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtBranchNameENG" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtBranchAddressTHA">Address(THA):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtBranchAddressTHA" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-2 no-padding" for="txtBranchAddressENG">Address(ENG):</label>
                                <div class="col-xs-10">
                                    <input type="text" class="form-control" id="txtBranchAddressENG" runat="server" />
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
                                &nbsp;
                            </div>
                        </div>
                        <input type="hidden" id="supplierBranchId" />
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
        var pageURL = "SupplierBranch.aspx";
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
                $("#cboGEO").parent().parent().removeClass("has-error");
                $("#cboProvince").parent().parent().removeClass("has-error");
                $("#cboAmphur").parent().parent().removeClass("has-error");
                $("#cboDistrict").parent().parent().removeClass("has-error");
            }); 
            var h = window.innerHeight;
            supplierBranchGrid.SetHeight(h - 155);
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
            supplierBranchGrid.SetHeight(height);
        });
        function NewItem() {
            $("#supplierBrachId").val("");
            $("#modal_form .modal-title").html("เพิ่มสาขาผู้จัดจำหน่าย");
            $("#modal_form").modal("show");
        }

        function editItem(id) {
            $("#supplierBranchId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditSupplierBranchData",
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

                    $('#txtSupplierCode').val(result.d["supplier_code"]);
                    $('#txtBranchNo').val(result.d["branch_no"]);
                    $('#txtBranchNameTHA').val(result.d["branch_name_tha"]);
                    $('#txtBranchNameENG').val(result.d["branch_name_eng"]);
                    $('#txtBranchAddressTHA').val(result.d["address_tha"]);
                    $('#txtBranchAddressENG').val(result.d["address_eng"]);
                    cboGEO.SetValue(result.d["geo_id"]);
                    cboProvince.SetValue(result.d["province_id"]);
                    cboAmphur.SetValue(result.d["amphur_id"]);
                    cboDistrict.SetValue(result.d["district_id"]);
                    $('#txtZipcode').val(result.d["zipcode"]);

                    $("#modal_form .modal-title").html("แก้ไขสาขาผู้จัดจำหน่าย");
                    $("#modal_form").modal("show");
                }
            });
        }

        function submitFrom() {

            var isValid = false;

            var isValidCombo = new Array();
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
                var supplierBranchId = $("#supplierBranchId").val();
                if (supplierBranchId == "") {
                    var parametersAdd = {
                        supplierBranchAddData: [
                            {
                                supplier_code: $('#txtSupplierCode').val(),
                                branch_no: $('#txtBranchNo').val(),
                                branch_name_tha: $('#txtBranchNameTHA').val(),
                                branch_name_eng: $('#txtBranchNameENG').val(),
                                address_tha: $('#txtBranchAddressTHA').val(),
                                address_eng: $('#txtBranchAddressENG').val(),
                                geo_id: cboGEO.GetValue(),
                                province_id: cboProvince.GetValue(),
                                amphur_id: cboAmphur.GetValue(),
                                district_id: cboDistrict.GetValue(),
                                zipcode: $('#txtZipcode').val(),
                            }
                        ]
                    };

                    $.ajax({
                        type: 'POST',
                        url: pageURL + "/InsertSupplierBranch",
                        data: JSON.stringify(parametersAdd),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (result) {
                            if (result.d) {
                                window.location.reload();
                            }
                        }
                    });

                } else if (supplierBranchId != "") {
                    var parametersEdit = {
                        supplierBranchUpdateData: [
                            {
                                id: supplierBranchId,
                                supplier_code: $('#txtSupplierCode').val(),
                                branch_no: $('#txtBranchNo').val(),
                                branch_name_tha: $('#txtBranchNameTHA').val(),
                                branch_name_eng: $('#txtBranchNameENG').val(),
                                address_tha: $('#txtBranchAddressTHA').val(),
                                address_eng: $('#txtBranchAddressENG').val(),
                                geo_id: cboGEO.GetValue(),
                                province_id: cboProvince.GetValue(),
                                amphur_id: cboAmphur.GetValue(),
                                district_id: cboDistrict.GetValue(),
                                zipcode: $('#txtZipcode').val(),
                            }
                        ]
                    };

                    $.ajax({
                        type: 'POST',
                        url: pageURL + "/UpdateSupplierBranch",
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
                       type: "POST",
                       url: pageURL + "/DeleteSupplierBranch",
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_supplier_branch"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        supplierBranchGrid.PerformCallback();

                    }
                }
            });
        }
      
    </script>
</asp:Content>
