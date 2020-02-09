﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Remark.aspx.cs" Inherits="HicomIOS.Master.Remark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }
    </style>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="remarkGrid"
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
                        <a id="btnEdit" class="btn btn-mini" onclick="editItem(this, '<%# Eval("remark_typedocument")%>','<%#Eval("remarktype")%>', <%# Container.VisibleIndex %>)" title="<%# Eval("remark_type") + ", " + Eval("remark_type_document")%>">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="remark_type" Caption="Remark Type" Width="80px" VisibleIndex="7" />
                <dx:GridViewDataTextColumn FieldName="remark_type_document" Caption="Remark Type Document" Width="300px" VisibleIndex="7" />
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
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
                        <div class="modal-title">แก้ไขหมายเหตุ</div>
                    </div>
                    <div class="modal-body text-center" id="rsDataRemark">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn-addItem" onclick="submitEdit()">
                            <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                        </button>
                        <button type="button" class="btn-addItem" data-dismiss="modal">
                            <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                        </button>
                    </div>
                </div>
            </div>


        </div>
        <script type="text/javascript">
             $(document).ready(function () {
                var h = window.innerHeight;
                remarkGrid.SetHeight(h - 155);
            });
             ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                console.log(s);
                var dimensions = {
                    height: (event.srcElement || event.currentTarget).innerHeight,
                    width: (event.srcElement || event.currentTarget).innerWidth
                };
                //console.log(dimensions);
                //var a = $('#remarkGrid').length;
                var height = dimensions.height - 152;
                //document.getElementById('gridCell').setAttribute("style", "height:" + height + "px");
                console.log(height);
                remarkGrid.SetHeight(height);
            });

            var totalRemark = 0;
            function editItem(element, e, d, i) {
                $.ajax({
                    type: 'POST',
                    url: "Remark.aspx/RemarkEdit",
                    data: '{remark_type_document: "' + e + '",remark_type: "' + d + '", index: ' + i + '}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        console.log(result.d);
                        totalRemark = result.d.length;
                        var dataHtml = "";
                        for (var i = 0; i < result.d.length; i++) {
                            var index = i + 1;
                            dataHtml += '<div class="row">';
                            dataHtml += '   <div class="form-group col-xs-12 no-padding">';
                            dataHtml += '       <label class="control-label col-xs-1 no-padding" id="lbRemark' + (index) + '">' + (index) + '</label>';
                            dataHtml += '       <div class="col-xs-11">';
                            dataHtml += '           <input type="text" class="form-control" id="txtRemark' + (index) + '" value=\'' + result.d[i].remark_description + '\' />';
                            dataHtml += '           <input type="hidden"  class="form-control" id="txtIdRemark' + (index) + '" value="' + result.d[i].id + '" />';
                            dataHtml += '       </div>';
                            dataHtml += '   </div>';
                            dataHtml += '</div>';
                        }
                        $('#rsDataRemark').html(dataHtml);
                        $('#modal_form .modal-title').html("แก้ไขหมายเหตุ : " + element.title);
                        $('#modal_form').modal("show");
                    }
                });
            }

            function submitEdit() {

                var parametersEdit = new Array();
                for (var i = 0; i < totalRemark; i++) {
                    var index = i + 1;
                    parametersEdit.push({
                        id: parseInt($('#txtIdRemark' + index).val()),
                        remark_description: $('#txtRemark' + index).val()

                    });
                }
                console.log(parametersEdit);
                var a = JSON.stringify(parametersEdit);
                console.log(a);
                $.ajax({
                    type: 'POST',
                    url: "Remark.aspx/SubmitRemarkEdit",
                    data: '{ parametersEdit: ' + JSON.stringify(parametersEdit) + '  }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            window.location.reload();
                        }
                    }
                });
            }

        </script>
</asp:Content>
