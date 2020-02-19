﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="HicomIOS.Master.Config" %>

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
        &nbsp;เพิ่มตั้งค่า
    </button>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="configGrid"
            Width="100%" KeyFieldName="id"  OnCustomCallback="gridView_CustomCallback">
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
                        <a id="btnEdit" class="btn btn-mini" onclick="editItem(<%# Eval("id")%>, <%# Container.VisibleIndex %>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                        |
                        <a id="btnDelete" class="btn btn-mini" onclick="deleteItem(<%# Eval("id")%>, '<%# Eval("config_document")%>')" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="config_document" Caption="Config Document" Width="50px" Visible="false" />
                <dx:GridViewDataTextColumn FieldName="config_type" Caption="Quotation Type" Width="80px" />
                <dx:GridViewDataTextColumn FieldName="config_description" Caption="Description" Width="100px" />
                <dx:GridViewDataTextColumn Caption="Status" FieldName="is_enable" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <button type="button" id="item_<%# Eval("id")%>" class="<%# (bool)Eval("is_enable") == true ? "btn btn-left btn-toggle active " : "btn btn-left btn-toggle" %>"
                             data-toggle="button" aria-pressed="true"  onclick="changeStatusItem(<%# Eval("id")%>, '<%# Eval("is_enable")%>')" >
                             <div class="handle"></div>
                        </button>
                        </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <SettingsBehavior AllowFocusedRow="true" />
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
                        <input type="hidden" id="configId" />
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding hidden">
                                <label class="control-label col-xs-3 no-padding" for="txtConfigDocument">Config Document:</label>
                                <div class="col-xs-9">
                                    <%--<input type="text" class="form-control" id="txtConfigDocument" runat="server" />--%>
                                    <dx:ASPxComboBox runat="server" ID="cmbConfigDocument" CssClass="form-control" ClientInstanceName="cmbConfigDocument">
                                        <Items>
                                            <%--<dx:ListEditItem Selected="true" Text="--กรุณาเลือก--" />--%>
                                            <dx:ListEditItem Text="Quotation" Selected="true" Value="QU" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="txtConfigType">Quotation Type:</label>
                                <div class="col-xs-9">
                                    <%--<input type="text" class="form-control" id="txtConfigType" runat="server" />--%>
                                    <dx:ASPxComboBox runat="server" ID="cmbConfigType" CssClass="form-control" ClientInstanceName="cmbConfigType">
                                        <Items>
                                            <dx:ListEditItem Value="P" Text="Product" Selected="true" />
                                            <dx:ListEditItem Value="S" Text="Spare part" />
                                            <dx:ListEditItem Value="M" Text="Maintenance" />
                                            <dx:ListEditItem Value="C" Text="Service Charge" />
                                            <dx:ListEditItem Value="A" Text="Annual Service" />
                                            <dx:ListEditItem Value="D" Text="Design" />
                                            <dx:ListEditItem Value="O" Text="Other" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-3 no-padding" for="txtConfigDescription">Description:</label>
                                <div class="col-xs-9">
                                    <input type="text" class="form-control" id="txtConfigDescription" runat="server" />
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
        var pageURL = "Config.aspx";
        $(document).ready(function () {
            // select first input when open modal
            $('#modal_form').on('shown.bs.modal', function () {
                $(this).find('input').first().focus();
            });

            $(".modal").on("hidden.bs.modal", function () {
                //$(this).find("input,textarea,select").val('').end().find("input[type=checkbox], input[type=radio]").prop("checked", "").end();
            }); 
            var h = window.innerHeight;
            configGrid.SetHeight(h - 155);
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
            configGrid.SetHeight(height);
        });

        function NewItem() {
            $("#configId").val("");
            //cmbConfigDocument.SetValue("QU");
            cmbConfigType.SetValue("P");
            $('#txtConfigDescription').val('');
            $("#modal_form .modal-title").html("New Config Quotation");
            $("#modal_form").modal("show");
        }

        function editItem(id, index) {
            gridView.FocusedRowIndex = index;

            $("#configId").val(id);
            $.ajax({
                type: 'POST',
                url: pageURL + "/GetEditConfig",
                data: '{id: "' + id + '", index: ' + index + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {

                    //$('#txtConfigDocument').val(result.d["config_document"]);
                    //$('#txtConfigType').val(result.d["config_type"]);
                    //cmbConfigDocument.SetValue("QU");
                    cmbConfigType.SetValue(result.d["config_type"]);
                    $('#txtConfigDescription').val(result.d["config_description"]);

                    $("#modal_form .modal-title").html("Edit Config Quotation");
                    $("#modal_form").modal("show");
                }
            });
        }

        function submitFrom() {
            var configId = $("#configId").val();
            if (configId == "") {
                var parametersAdd = {
                    configAddData: [
                        {
                            config_document: cmbConfigDocument.GetValue(),//$('#txtConfigDocument').val(),
                            config_type: cmbConfigType.GetValue(),//$('#txtConfigType').val(),
                            config_description: $('#txtConfigDescription').val(),
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/InsertConfig",
                    data: JSON.stringify(parametersAdd),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            window.location.reload();
                        }
                    }
                });

            } else if (configId != "") {
                var parametersEdit = {
                    configUpdateData: [
                        {
                            id: configId,
                            config_document: cmbConfigDocument.GetValue(),//$('#txtConfigDocument').val(),
                            config_type: cmbConfigType.GetValue(),//$('#txtConfigType').val(),
                            config_description: $('#txtConfigDescription').val(),
                        }
                    ]
                };

                $.ajax({
                    type: 'POST',
                    url: pageURL + "/UpdateConfig",
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
                url: pageURL + "/DeleteConfig",
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
                data: '{id: "' + id + '", is_enable: "' + is_enable + '", table_name: "tb_config"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        configGrid.PerformCallback();

                    }
                }
            });
        }
        
    </script>
</asp:Content>