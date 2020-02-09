<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" Inherits="HicomIOS.Master.Approve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }
    </style>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="configGrid"
            Width="100%" KeyFieldName="id">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini" onclick="editItem(this, <%# Eval("id")%>)" title="<%# Eval("approve_doc") + ", " + Eval("approve_type")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>

                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="approve_doc" Caption="Document" Width="70px" />
                <dx:GridViewDataTextColumn FieldName="approve_type" Caption="Type" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="name_app1" Caption="ผู้จัดทำ" Width="150px" Visible="false" />
                <dx:GridViewDataTextColumn FieldName="name_app2" Caption="ผู้ตรวจสอบ" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="name_app3" Caption="ผู้อนุมัติ" Width="150px" />
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>

        <div class="modal fade" id="modal_formEdit" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <div class="modal-title">Add Customer Group</div>
                    </div>
                    <div class="modal-body text-center">
                        <input type="hidden" id="approve_id" />
                        <div class="row hidden">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ผู้จัดทำ:</label>
                                <div class="col-xs-7">
                                    <dx:ASPxComboBox ID="cboApprove1" CssClass="form-control" runat="server"
                                        ClientInstanceName="cboApprove1" TextField="data_text"
                                        EnableCallbackMode="true"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ผู้ตรวจสอบ:</label>
                                <div class="col-xs-7">
                                    <dx:ASPxComboBox ID="cboApprove2" CssClass="form-control" runat="server"
                                        ClientInstanceName="cboApprove2" TextField="data_text"
                                        EnableCallbackMode="true"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ผู้อนุมัติ:</label>
                                <div class="col-xs-7">
                                    <dx:ASPxComboBox ID="cboApprove3" CssClass="form-control" runat="server"
                                        ClientInstanceName="cboApprove3" TextField="data_text"
                                        EnableCallbackMode="true"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
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
    <script type="text/javascript">
        $(document).ready(function () {
            var h = window.innerHeight;
            configGrid.SetHeight(h - 155);
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };
            //console.log(dimensions);
            //var a = $('#configGrid').length;
            var height = dimensions.height - 152;
            //document.getElementById('gridCell').setAttribute("style", "height:" + height + "px");
            console.log(height);
            configGrid.SetHeight(height);
        });

        var edit_time_out;
        function editItem(element, id) {
            clearTimeout(edit_time_out);
            ck_old_pass = setTimeout(function validate() {
                $.ajax({
                    type: 'POST',
                    url: "Approve.aspx/EditApprove",
                    data: '{ id:"' + id + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        console.log(result.d);
                        cboApprove1.SetValue(result.d.approve_id1);
                        if (cboApprove1.GetSelectedItem() == null) {
                            cboApprove1.SetValue("0");
                        }
                        cboApprove2.SetValue(result.d.approve_id2);
                        if (cboApprove2.GetSelectedItem() == null) {
                            cboApprove2.SetValue("0");
                        }
                        cboApprove3.SetValue(result.d.approve_id3);
                        if (cboApprove3.GetSelectedItem() == null) {
                            cboApprove3.SetValue("0");
                        }

                        $('#approve_id').val(result.d.id);

                        $("#modal_formEdit .modal-title").html("Edit Approve : " + element.title);
                        $("#modal_formEdit").modal("show");


                    }
                });
            },500);
            
        }

        function submitFrom(id) {
            var parametersEdit = {
                approveData: [
                    {
                        id: $('#approve_id').val(),
                        approve_id1: cboApprove1.GetValue(),
                        approve_id2: cboApprove2.GetValue(),
                        approve_id3: cboApprove3.GetValue(),
                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: "Approve.aspx/SubmitApprove",
                data: JSON.stringify(parametersEdit),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                        location.reload();
                }
            });
        }

    </script>
</asp:Content>

