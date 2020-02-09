﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AdjustProductSparePartList.aspx.cs" Inherits="HicomIOS.Master.AdjustProductSparePartList" %>

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
            font-size: 15px !important;
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
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
    </script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <button type="button" onclick="addNewItem()" class="btn-addItem btn-new btn-new1">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;New Adjust Stock
    </button>
    <div class="div-search-box">
        <input type="text" id="txtSearchBoxData" class="form-control" placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchDataGrid(event.keyCode)" />
        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchDataGrid(13);">
            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
        </button>
    </div>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
            Width="100%" KeyFieldName="id" OnCustomCallback="gridView_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
            <Columns>
                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </a>|
                        <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "disabled" %>" onclick="deleteItem(<%# Eval("id")%>)" title="Delete">
                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <%-- <dx:GridViewDataTextColumn Caption="Print" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                    <DataItemTemplate>
                        <a id="btnPrint" class="btn btn-mini" onclick="printPurchaseRequest(<%# Eval("id")%>)" title="Print">
                            <i class="fa fa-print" aria-hidden="true"></i>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <%-- <dx:GridViewDataTextColumn FieldName="purchase_request_no" Caption="Purchase Request No" Width="110px" >
                     <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <%# Eval("purchase_request_no")%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="150px" />
                <dx:GridViewDataTextColumn FieldName="adjust_date" Caption="Adjust Date" Width="100px" />
                <dx:GridViewDataTextColumn FieldName="adjust_objective" Caption="Adjust Objective" Width="200px" />
                <dx:GridViewDataTextColumn FieldName="po_no" Caption="PO No" Width="110px" />
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>

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
        //function printPurchaseRequest(e) {
        //    $.LoadingOverlay("show", {
        //        zIndex: 9999
        //    });
        //    var id = e;
        //    $.ajax({
        //        type: "POST",
        //        url: "PurchaseRequestList.aspx/GetPurchaseRequesData",
        //        data: '{id:"' + id + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data) {
        //            var data = data.d;

        //            window.open("../Report/DocumentViewer.aspx?ReportArgs=Purchase_Request|" + data.purchase_request_no, "_blank");
        //            $.LoadingOverlay("hide");
        //        }
        //    });
        //}

        function searchDataGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                purchaseRequestGrid.PerformCallback(txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function addNewItem() {
            window.location.href = "AdjustProductSparePart.aspx";
        }

        function editItem(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            window.location.href = "AdjustProductSparePart.aspx?dataId=" + key;
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
                            url: "AdjustProductSparePartList.aspx/DeleteItem",
                            data: '{id:"' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d == 'success') {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                            location.reload();
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
