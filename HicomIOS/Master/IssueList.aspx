﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="IssueList.aspx.cs" Inherits="HicomIOS.Master.IssueList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn-labelEdit {
            cursor: pointer;
        }

        a:link {
            text-decoration: none !important;
        }

        a:active {
            text-decoration: underline !important;
        }

        a:hover {
            text-decoration: underline !important;
            
        }
         .swal-modal {
            width: 420px !important;
            height: 250px !important;
        }

        .swal-title {
            font-size: 18px !important;
        }

        .swal-overlay:before {
            height: 40% !important;
        }

        .swal-button {
            padding: 5px 15px;
        }
        .disabled {
          pointer-events: none;
         }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
    </script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <button type="button" onclick="addNewItem()" class="btn-addItem btn-new btn-new1 <%= ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_create) ? "" : "hidden" %>">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;New Issue
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
            Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridView_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="60px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>|
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>)" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Print" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                    <DataItemTemplate>
                        <a id="btnPrint" class="btn btn-mini" onclick="printIssue(<%# Eval("id")%>)" title="Print">
                            <i class="fa fa-print" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="issue_stock_no" Caption="Issue Stock No" Width="60px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <%# Eval("issue_stock_no")%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="issue_stock_status_display" Caption="Status" Width="50px" />
                <dx:GridViewDataTextColumn FieldName="issue_stock_date" Caption="Issue Date" Width="60px" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="dd/MM/yyyy" />
                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="60px" />
                <dx:GridViewDataTextColumn FieldName="sale_order_no" Caption="Sale Order No" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="200px" />

                <dx:GridViewDataTextColumn FieldName="objective_type_display" Caption="Objective Type" Width="80px" />
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>
        <div class="modal fade" id="modal_delete" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <div class="modal-title">ลบข้อมูล</div>
                    </div>
                    <div class="modal-body text-center">
                        <input type="hidden" name="delete_link" id="delete_link" class="hide" value="">
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
        $(document).ready(function () {

            var h = window.innerHeight;
            gridView.SetHeight(h - 155);
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };
            //console.log(dimensions);
            //var a = $('#gridView').length;
            var height = dimensions.height - 152;
            //document.getElementById('gridCell').setAttribute("style", "height:" + height + "px");
            console.log(height);
            gridView.SetHeight(height);
        });
        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                gridView.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function printIssue(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var id = e;
            $.ajax({
                type: "POST",
                url: "IssueList.aspx/GetIssueData",
                data: '{id:"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var data = data.d;

                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Issue_Report|" + data.issue_stock_no, "_blank");
                    $.LoadingOverlay("hide");
                }
            });
        }
        function addNewItem() {
            window.location.href = "Issue.aspx";
        }
        function deliveryOrder() {
            window.location.href = "DeliveryOrder.aspx";
        }
        function editItem(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            window.location.href = "Issue.aspx?dataId=" + key;
            $.LoadingOverlay("hide");
        }

        function deleteItem(e) {
            var id = e;

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
                           url: "IssueList.aspx/DeleteIssue",
                           data: '{id:"' + id + '"}',
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
    </script>
</asp:Content>
