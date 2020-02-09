﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="UserPermission.aspx.cs" Inherits="HicomIOS.Security.UserPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/CheckboxStyle.css" rel="stylesheet" />
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        .checkbox label, .radio label {
            min-height: 0;
        }

        .checkbox, .radio {
            margin-top: 3px;
            margin-bottom: 5px;
        }
    </style>
    <script>
        function OnUserGroupChanged(cboUserGroup) {
            var user_group_id = cboUserGroup.GetValue().toString();
            $.ajax({
                type: 'POST',
                url: "UserPermission.aspx/SelectUserGroupList",
                data: '{id: "' + user_group_id + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        gridView.PerformCallback();
                        $.LoadingOverlay("show", {
                            zIndex: 9999
                        });
                    }
                }
            });
        }

        function onEndCallbackGridView() {
            $.LoadingOverlay("hide");
        }
    </script>

    <div style="position: absolute; top: 90px; width: 100%;">
        <div class="form-group col-xs-12 no-padding">
            <label class="control-label col-xs-1 no-padding" for="cboUserGroup" style="margin-bottom: 0px; margin-top: 3px; text-align: right;">กลุ่มผู้ใช้งาน:</label>
            <div class="col-xs-3">
                <dx:ASPxComboBox ID="cboUserGroup" CssClass="form-control" runat="server" ClientInstanceName="cboUserGroup" TextField="data_text"
                    ValueField="data_value">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnUserGroupChanged(s); }" />
                </dx:ASPxComboBox>
            </div>
            <div class="col-xs-4" >
                 <label class="control-label" style="margin-bottom: 0px; margin-top: 3px; text-align: right;">Check All :&nbsp;&nbsp;&nbsp;</label>
                <label>
                    <input type="checkbox" runat="server" id="Checkbox1" enableviewstate="false" onclick="onCheckedAll(1, this)" value="" />&nbsp;View
                </label>&nbsp;&nbsp;
                <label>
                    <input type="checkbox" runat="server" id="Checkbox2" enableviewstate="false" onclick="onCheckedAll(2, this)" value="" />&nbsp;Add
                </label>&nbsp;&nbsp;
                <label>
                    <input type="checkbox" runat="server" id="Checkbox3" enableviewstate="false" onclick="onCheckedAll(3, this)" value="" />&nbsp;Edit
                </label>&nbsp;&nbsp;
                <label>
                    <input type="checkbox" runat="server" id="Checkbox4" enableviewstate="false" onclick="onCheckedAll(4, this)" value="" />&nbsp;Delete
                </label>&nbsp;&nbsp;&nbsp;&nbsp;
                <button type="button" runat="server" id="Button27" onclick="saveData()" class="btn-addItem noDisabled">
                    <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Save
                </button>
            </div>
        </div>
        <div class="form-group">
            
        </div>
    </div>

    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
            SettingsBehavior-AllowSort="false"
            Width="100%" KeyFieldName="id"  OnCustomCallback="gridView_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <%--<SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>--%>
            <SettingsPager Mode="ShowAllRecords" />

            <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
            <Columns>
                <dx:GridViewDataTextColumn Caption="Screen Name" FieldName="name_tha" Width="100px">
                    <DataItemTemplate>
                        <%# ((int)Eval("is_head_menu") == 1) ? Eval("name_tha") : "&nbsp;&nbsp;&nbsp;-&nbsp;" + Eval("name_tha") %>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="View" FieldName="is_view" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_view<%# Eval("id")%>" class="parent_is_view_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_view', <%# ((int)Eval("is_head_menu") == 1 ? Eval("screen_id") : Eval("parent_id"))%>)" <%# ((bool)Eval("is_view")) ? "checked=''" : "" %> />
                            <label for="is_view<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Add" FieldName="is_create" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_create<%# Eval("id")%>" class="parent_is_create_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_create', <%# ((int)Eval("is_head_menu") == 1 ? Eval("screen_id") : Eval("parent_id"))%>)" <%# ((bool)Eval("is_create")) ? "checked=''" : "" %> />
                            <label for="is_create<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Edit" FieldName="is_edit" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_edit<%# Eval("id")%>" class="parent_is_edit_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_edit', <%# ((int)Eval("is_head_menu") == 1 ? Eval("screen_id") : Eval("parent_id"))%>)" <%# ((bool)Eval("is_edit")) ? "checked=''" : "" %> />
                            <label for="is_edit<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Delete" FieldName="is_del" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_del<%# Eval("id")%>" class="parent_is_del_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_del', <%# ((int)Eval("is_head_menu") == 1 ? Eval("screen_id") : Eval("parent_id"))%>)" <%# ((bool)Eval("is_del")) ? "checked=''" : "" %> />
                            <label for="is_del<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" EndCallback="onEndCallbackGridView" />
        </dx:ASPxGridView>
    </div>
    <script>
        function setPermission(id, column, parent_id) {
            var menu_id = new Array();
            var is_enable = 0;
            if ($('#' + column + id).is(':checked')) {
                is_enable = 1;
            }
            menu_id.push(id);

            if ($('#' + column + id).attr('head_menu') == 1) {
                if ($('#' + column + id).is(':checked')) {
                    $('.parent_' + column + '_' + parent_id).prop('checked', true);
                } else {
                    $('.parent_' + column + '_' + parent_id).prop('checked', false);
                }

                $('.parent_' + column + '_' + parent_id).each(function (index) {
                    var submenu_id = $(this).attr('id').replace(column, '');
                    menu_id.push(parseInt(submenu_id));
                });
            }

            for (var i = 0; i < menu_id.length; i++) {
                sendData(menu_id[i], column, is_enable);
            }

        }

        function sendData(id, column, is_enable) {
            $.ajax({
                type: 'POST',
                url: "UserPermission.aspx/SetPermissionUserGroup",
                data: '{id: "' + id + '", column: "' + column + '", is_enable: "' + is_enable + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                    }
                }
            });
        }

        function onCheckedAll(id, checkbox) {
            var group_id = cboUserGroup.GetValue().toString();
            var is_enable = ($(checkbox).is(":checked") ? 1 : 0);
             $.ajax({
                type: 'POST',
                url: "UserPermission.aspx/CheckPermissionAll",
                data: '{id: "' + id + '", group_id: "' + group_id + '", is_enable: "' + is_enable + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d == "success") {
                        gridView.PerformCallback();
                    }
                }
            });
        }

        function saveData() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            setTimeout(function ()
            {
                $.LoadingOverlay("hide");
            }, 1000);
        }

        $(document).ready(function () {
            var h = window.innerHeight;
            gridView.SetHeight(h - 155);
        });
    </script>
</asp:Content>

