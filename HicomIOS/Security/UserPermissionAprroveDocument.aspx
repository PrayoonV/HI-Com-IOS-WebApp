<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="UserPermissionAprroveDocument.aspx.cs" Inherits="HicomIOS.Security.UserPermissionAprroveDocument" %>
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

    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
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
                <dx:GridViewDataTextColumn Caption="Screen Name" FieldName="name_tha" Width="100px">
                    <DataItemTemplate>
                        <%# ((int)Eval("is_head_menu") == 1) ? Eval("name_tha") : "&nbsp;&nbsp;&nbsp;-&nbsp;" + Eval("name_tha") %>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="View" FieldName="is_view" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_view<%# Eval("id")%>" class="parent_is_view_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_view')" <%# ((bool)Eval("is_view")) ? "checked=''" : "" %> />
                            <label for="is_view<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Add" FieldName="is_create" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_create<%# Eval("id")%>" class="parent_is_create_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_create')" <%# ((bool)Eval("is_create")) ? "checked=''" : "" %> />
                            <label for="is_create<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Edit" FieldName="is_edit" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_edit<%# Eval("id")%>" class="parent_is_edit_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_edit')" <%# ((bool)Eval("is_edit")) ? "checked=''" : "" %> />
                            <label for="is_edit<%# Eval("id")%>">&nbsp;</label>
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Delete" FieldName="is_del" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <div class="checkbox checkbox-primary">
                            <input type="checkbox" id="is_del<%# Eval("id")%>" class="parent_is_del_<%# Eval("parent_id")%>" head_menu="<%# Eval("is_head_menu")%>" onchange="setPermission(<%# Eval("id")%>, 'is_del')" <%# ((bool)Eval("is_del")) ? "checked=''" : "" %> />
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
        function setPermission(id, column) {
            var menu_id = new Array();
            var is_enable = 0;
            if ($('#' + column + id).is(':checked')) {
                is_enable = 1;
            }
            menu_id.push(id);

            if ($('#' + column + id).attr('head_menu') == 1) {
                if ($('#' + column + id).is(':checked')) {
                    $('.parent_' + column + '_' + id).prop('checked', true);
                } else {
                    $('.parent_' + column + '_' + id).prop('checked', false);
                }

                $('.parent_' + column + '_' + id).each(function (index) {
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
    </script>
</asp:Content>
