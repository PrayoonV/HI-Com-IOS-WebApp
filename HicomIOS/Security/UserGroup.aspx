﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="HicomIOS.Master.UserGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }
    </style>
    <button type="button" onclick="NewItem()" class="btn-addItem btn-new btn-new1">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;เพิ่มกลุ่มผู้ใช้งาน
    </button>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="usergroupGrid"
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
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("group_name_tha")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="group_name_tha" Caption="User Group Name (THA)" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="group_name_eng" Caption="User Group Name (ENG)" Width="150px" />
                  <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="60px">
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
                        <div class="modal-title">เพิ่มกลุ่มผู้ใช้งาน</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="txtUserGroupNameTHA">Group Name(THA):</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="txtUserGroupNameTHA" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-6 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="txtUserGroupNameENG">Group Name(ENG):</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="txtUserGroupNameENG" runat="server" />
                                </div>
                            </div>
                        </div>
                        <input type="hidden" id="userGroupID" />
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
        var pageURL = "UserGroup.aspx";
        $(document).ready(function () {
            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
            });
            
            var h = window.innerHeight;
            usergroupGrid.SetHeight(h - 155);
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
            usergroupGrid.SetHeight(height);
        });
        function NewItem() {
            $("#userGroupID").val("");
            $("#modal_form .modal-title").html("เพิ่มกลุ่มผู้ใช้งาน");
            $("#modal_form").modal("show");
        }

        function editItem(id) {
            $("#userGroupID").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditUserGroupData",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    $('#txtUserGroupNameTHA').val(result.d["group_name_tha"]);
                    $('#txtUserGroupNameENG').val(result.d["group_name_eng"]);

                    $("#modal_form .modal-title").html("แก้ไขกลุ่มผู้ใช้งาน");
                    $("#modal_form").modal("show");
                }
            });
        }

        function submitFrom() {
            var userGroupID = $("#userGroupID").val();
            if (userGroupID == "") {
                var parametersAdd = {
                    userGroupAddData: [
                        {
                            group_name_tha: $('#txtUserGroupNameTHA').val(),
                            group_name_eng: $('#txtUserGroupNameENG').val()
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/InsertUserGroup",
                    data: JSON.stringify(parametersAdd),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            window.location.reload();
                        }
                    }
                });

            } else if (userGroupID != "") {
                var parametersEdit = {
                    userGroupUpdateData: [
                        {
                            id: userGroupID,
                            group_name_tha: $('#txtUserGroupNameTHA').val(),
                            group_name_eng: $('#txtUserGroupNameENG').val()
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/UpdateUserGroup",
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
            $('#modal_delete').modal("show");
            $('#btn_delete').val(id);
            $('#modal_delete .modal-body').html('คุณต้องการลบข้อมูล "' + dataText + '" ใช่หรือไม่ ?');
        }

        function confirmDelete() {
            var id = $('#btn_delete').val();
            $.ajax({
                type: 'POST',
                url: pageURL + "/DeleteUserGroup",
                data: '{id: "' + id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        window.location.reload();
                    }
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_security_users_group"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        usergroupGrid.PerformCallback();

                    }
                }
            });
        }
      
    </script>

</asp:Content>
