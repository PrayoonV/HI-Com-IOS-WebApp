﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="StockCheckingList.aspx.cs" Inherits="HicomIOS.Master.StockCheckingList" %>

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
            ;
        }

        .disabled {
            pointer-events: none;
        }
    </style>
    <script type="text/javascript">
        $('#gridCell').css({
            'height': '550px;'
        });
    </script>
    <button type="button" onclick="addNewItem()" class="btn-addItem btn-new btn-new1" style="height: 30px; position: absolute; top: 77px;">
        <i class="fa fa-plus-circle" aria-hidden="true"></i>
        &nbsp;New Stock Checking
    </button>
    <div id="gridCell">
        <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
            Width="100%" KeyFieldName="transection_no" OnCustomColumnDisplayText="gridView_CustomColumnDisplayText" OnHtmlRowPrepared="gridView_HtmlRowPrepared">
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
                <dx:GridViewDataTextColumn Caption="#" FieldName="transection_no" CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <a id="btnView" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="viewItem('<%# Eval("transection_no").ToString()%>')" title="กดดูรายละเอียด">
                            รายละเอียด
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="transection_no" Caption="เลขที่เอกสาร" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit " onclick="viewItem('<%# Eval("transection_no").ToString()%>')" title="รายละเอียด">
                            <%# Eval("transection_no")%>
                        </a>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="transection_date" Caption="วันที่บันทึก" Width="50px" Settings-SortMode="Value" Settings-AllowSort="True">
                    <PropertiesDateEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="display_product_type" Caption="ประเภทข้อมูล" Width="60px" />
                <dx:GridViewDataTextColumn FieldName="count_items" Caption="รายการทั้งหมด" Width="50px">
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="count_correct" Caption="รายการที่นับถูกต้อง" Width="50px" >
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="count_stock_in" Caption="รายการที่เกินจริง" Width="50px" >
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="count_stock_out" Caption="รายการที่หายไป" Width="50px" >
                    <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="250" />
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
        function addNewItem() {
            window.location.href = "StockChecking.aspx";
        }

        function viewItem(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            window.location.href = "StockChecking.aspx?dataNo=" + key;
            $.LoadingOverlay("hide");
        }

    </script>
</asp:Content>
