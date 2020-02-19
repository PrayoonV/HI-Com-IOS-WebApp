﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CustomerGroup.aspx.cs" Inherits="HicomIOS.Master.CustomerGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
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
    <button type="button" onclick="NewItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มกลุ่มลูกค้า
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="customerGroupGrid"
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
                <dx:GridViewDataTextColumn Caption=" " FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px"  Settings-AllowSort="False">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("group_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="mac5_code" Caption="Mac5 Code" Width="50px" />--%>
                <dx:GridViewDataTextColumn FieldName="group_name_tha" Caption="Customer Group Name(THA)" Width="150px" Settings-AllowSort="True" SortOrder="Ascending" />
                <dx:GridViewDataTextColumn FieldName="group_name_eng" Caption="Customer Group Name(ENG)" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="group_remark" Caption="Remark" Width="100px" />
                 <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="55px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')"  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled=\"disabled\"" %> >
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
                        <div class="modal-title">Add Customer Group</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <%--         <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="mac5Code">Mac5 Code:</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="mac5Code" runat="server" />
                                </div>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="customerGroupNameTHA">Group Name(THA):</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="customerGroupNameTHA" runat="server"  validate-data />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="customerGroupNameENG">Group Name(ENG):</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="customerGroupNameENG" runat="server" validate-data />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="customerGroupRemark">Remark:</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="customerGroupRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding">&nbsp;</label>
                                <div class="col-xs-9">
                                    <input type="hidden" id="customerGroupID" />
                                </div>
                            </div>
                        </div>
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
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        var pageURL = "CustomerGroup.aspx";
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
            });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });
            var h = window.innerHeight;
            customerGroupGrid.SetHeight(h - 155);
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
            customerGroupGrid.SetHeight(height);
        });


        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                customerGroupGrid.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function NewItem() {
            $("#customerGroupID").val("");
            $("#modal_form .modal-title").html("Add New Customer Group");
            $("#modal_form").modal("show");
        }

        function editItem(id, index) {
            gridView.FocusedRowIndex = index;

            $("#customerGroupID").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditCustomerGroupData",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    //$('#mac5Code').val(result.d["mac5_code"]);
                    $('#customerGroupNameTHA').val(result.d["group_name_tha"]);
                    $('#customerGroupNameENG').val(result.d["group_name_eng"]);
                    $('#customerGroupRemark').val(result.d["group_remark"]);

                    $("#modal_form .modal-title").html("Edit Customer Group");
                    $("#modal_form").modal("show");
                }
            });
        }

        function submitFrom() {

            if ($("#customerGroupNameTHA").val() == "") {
                $("#customerGroupNameTHA").parent().parent().addClass("has-error");
                return false;
            } else if ($("#customerGroupNameENG").val() == "") {
                $("#customerGroupNameENG").parent().parent().addClass("has-error");
                return false;
            }

            var customerGroupID = $("#customerGroupID").val();
            if (customerGroupID == "") {
                var parametersAdd = {
                    customerGroupAddData: [
                        {
                            mac5_code: "",
                            group_name_tha: $('#customerGroupNameTHA').val(),
                            group_name_eng: $('#customerGroupNameENG').val(),
                            group_remark: $('#customerGroupRemark').val()
                        }
                    ]
                };


                $.ajax({
                    type: 'POST',
                    url: pageURL + "/InsertCustomerGroup",
                    data: JSON.stringify(parametersAdd),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            window.location.reload();
                        }
                    }
                });

            } else if (customerGroupID != "") {
                var parametersEdit = {
                    customerGroupUpdateData: [
                        {
                            id: customerGroupID,
                            mac5_code: "",
                            group_name_tha: $('#customerGroupNameTHA').val(),
                            group_name_eng: $('#customerGroupNameENG').val(),
                            group_remark: $('#customerGroupRemark').val()
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/UpdateCustomerGroup",
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
                          type: 'POST',
                          url: pageURL + "/DeleteCustomerGroup",
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_customer_group"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        
                        var txtSearch = $("#txtSearchBoxData").val();
                        customerGroupGrid.PerformCallback(txtSearch.toString());

                    }
                }
            });
        }
    </script>
</asp:Content>