<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="StockCard.aspx.cs" Inherits="HicomIOS.Master.StockCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--<script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">-->
    <style>
        .modal .control-label {
            margin: 5px 0 0 0;
            padding: 0;
            text-align: right;
        }

        #SearchBox {
            display: none;
        }

        .no-padding {
            padding: 0px;
        }

        .btn-reportItem {
            margin-bottom: 7px;
            margin-left: 15px;
        }

        .btn-view {
            width: 25%;
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
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        
        $(function () {
            $(".picker").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            $('#shelf').hide();
            $('#Div2').hide();
            $('.sp-label').hide();
            $('.pp-label').show();
            $('#btnGroup').hide();
        });

        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };

            var height = dimensions.height;

            var height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
            var h = window.innerHeight;
            gridView.SetHeight(h - 270);
        });

        function changedProductType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "StockCard.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback()
                },
                failure: function (response) {
                }
            });
            var key = cbbProductType.GetValue();
            if (key == "SP") {
                $('#shelf').show();
                $('#Div2').show();
                cbbShelf.PerformCallback();
                cbbSecondaryCode.PerformCallback();
                $('.sp-label').show();
                $('.pp-label').hide();
                //$('#btnSparePart').removeClass('hidden');
                //$('#btnProduct').addClass('hidden');
                $('#btnGroup').show();
            }
            if (key == "PP") {
                $('#shelf').hide();
                $('#Div2').hide();
                $('.pp-label').show();
                $('.sp-label').hide();
                //$('#btnProduct').removeClass('hidden');
                //$('#btnSparePart').addClass('hidden');
                $('#btnGroup').show();
            }
            if (key == "SS") {
                $('#btnGroup').hide();
            }
            cbbSupplier.PerformCallback();

            cbbProductCat.PerformCallback(key);
            cbbProduct.PerformCallback();

            $.LoadingOverlay("hide");

        }

    </script>
    <div id="div-content">
        <div class="row">
            <fieldset>
                <legend>Stock Card</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Product Type :</label>
                            <div class="col-xs-7 no-padding">
                                <dx:ASPxComboBox ID="cbbProductType" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbProductType" TextField="data_text"
                                    EnableCallbackMode="true" ClientSideEvents-ValueChanged="changedProductType"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth pp-label">Product Category :</label>
                            <label class="col-xs-4 text-right label-rigth sp-label">Part Category :</label>
                            <div class="col-xs-7 no-padding">
                                <dx:ASPxComboBox ID="cbbProductCat" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbProductCat" TextField="data_text"
                                    OnCallback="cbbProductCat_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding" id="Div1" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth pp-label">Product No :</label>
                            <label class="col-xs-4 text-right label-rigth sp-label">Part No :</label>
                            <div class="col-xs-7 no-padding">
                                <input class="form-control hidden" runat="server" type="text" id="txt_product_no" />
                                <dx:ASPxComboBox ID="cbbProduct" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbProduct" TextField="data_text"
                                    EnableCallbackMode="true" OnCallback="cbbProduct_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding" id="shelf" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Shelf :</label>
                            <div class="col-xs-7 no-padding">
                                <dx:ASPxComboBox ID="cbbShelf" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbShelf" TextField="data_text"
                                    EnableCallbackMode="true" OnCallback="cbbShelf_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding" id="Div2" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Secondary Code :</label>
                            <div class="col-xs-7 no-padding">
                                <input class="form-control hidden" runat="server" type="text" id="txt_secondary_code" />
                                <dx:ASPxComboBox ID="cbbSecondaryCode" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbSecondaryCode" TextField="data_text"
                                    EnableCallbackMode="true" OnCallback="cbbSecondaryCode_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Supplier :</label>
                            <div class="col-xs-7 no-padding">
                                <dx:ASPxComboBox ID="cbbSupplier" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbSupplier" TextField="data_text"
                                    EnableCallbackMode="true" OnCallback="cbbSupplier_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Date From :</label>
                            <div class="col-xs-3 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerFrom" autocomplete="off"/>
                            </div>
                            <label class="col-xs-2 no-padding text-center"> To </label>
                            <div class="col-xs-3 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerTo" autocomplete="off" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding-right">
                        <div class="row form-group">
                            <button type="button" runat="server" id="btnProduct" onclick="addItemDetail('P')" class="btn-addItem hidden">เพิ่มสินค้า</button>
                            <button type="button" runat="server" id="btnSparePart" onclick="addItemDetail('S')" class="btn-addItem hidden">เพิ่มสินค้าอะไหล่</button>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-5 no-padding">
                        <div class="row form-group" id="btnGroup">
                            <div class="col-xs-4 no-padding">
                                <button type="button"
                                    class="btn-info btn-view" style="height: 30px; width: 90%;"
                                    id="btnView" onclick="view_Click()">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    View
                                </button>
                            </div>
                            <div class="col-xs-4 no-padding">
                                <button type="button"
                                    class="btn-info btn-view" style="height: 30px; width: 90%;"
                                    id="btnClear" onclick="clear_Click()">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    Clear
                                </button>
                            </div>
                            <div class="col-xs-4 no-padding">
                                <button type="button" runat="server" id="btnReportClient" onclick="callReport()" style="height: 30px; width: 90%;" class="btn-addItem hidden">
                                    <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Show Report
                                </button>
                                <asp:Button ID="btnReportExcel" runat="server" Text="Show Excel" UseSubmitBehavior="false" CssClass="btn-addItem" OnClick="btnReportExcel_Click" style="height: 30px; width: 90%;" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="row">
            <div class="col-xs-12" runat="server" id="dvGridProduct">
                <dx:ASPxGridViewExporter ID="gridViewExporter" GridViewID="gridView" runat="server"></dx:ASPxGridViewExporter>
                <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
                    Width="100%" KeyFieldName="id"
                    OnCustomCallback="gridStockCard_CustomCallback"
                    OnHtmlDataCellPrepared="gridView_HtmlDataCellPrepared">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                        PageSizeItemSettings-Visible="true">
                        <PageSizeItemSettings Items="10, 20, 50" />
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="cat_name" Caption="ประเภทสินค้า" Width="70px" />
                        <dx:GridViewDataTextColumn FieldName="no_" Caption="รหัสสินค้า" Width="70px" />
                        <dx:GridViewDataTextColumn FieldName="name_" Caption="ชื่อสินค้า/อะไหล่" Width="180px" />
                        <dx:GridViewDataTextColumn FieldName="in_" Caption="ขาเข้า" Width="30px" >
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="out_" Caption="ขาออก" Width="30px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="adj_" Caption="ปรับปรุง" Width="30px" Visible="false">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="stock_balance" Caption="คงเหลือ" Width="30px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="remark" Caption="Remark" Width="80px" />
                    </Columns>
                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
                    <SettingsPopup>
                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                    </SettingsPopup>
                    <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                        CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                </dx:ASPxGridView>
            </div>
        </div>
    </div>

    <div class="modal fade" id="popupSelectedItem" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    Add Product/Spare Part
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchBoxData" class="form-control searchBoxData"
                                    placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchProductGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchProductGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <dx:ASPxGridView ID="gridViewSelectedItem" SettingsBehavior-AllowSort="false"
                                ClientInstanceName="gridViewSelectedItem" runat="server" Settings-VerticalScrollBarMode="Visible"
                                Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                OnCustomCallback="gridViewSelectedItem_CustomCallback" OnHtmlDataCellPrepared="gridViewSelectedItem_HtmlDataCellPrepared"
                                KeyFieldName="item_no" Width="100%">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                <Paddings Padding="0px" />
                                <Border BorderWidth="0px" />
                                <BorderBottom BorderWidth="1px" />
                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                    PageSizeItemSettings-Visible="false">
                                    <PageSizeItemSettings Items="10, 20, 50" />
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                        <DataItemTemplate>
                                            <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                TextField="Name" ValueField="is_selected" ClientInstanceName="chkBox"
                                                ProductId='<%# Eval("id")%>'>
                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                getCheckBoxValue(s, e); 
                                                            }" />
                                            </dx:ASPxCheckBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn FieldName="item_no" Caption="Item No" Width="100px" VisibleIndex="1" />
                                    <dx:GridViewDataColumn FieldName="item_name" Caption="Item Name" Width="200px" VisibleIndex="2" />


                                </Columns>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="350" />
                            </dx:ASPxGridView>
                        </div>

                        <div class="col-xs-12">
                            <div class="row form-group">
                                <div class="col-xs-12 no-padding text-right" style="margin-top: 15px;padding: 0 15px;">
                                    <button type="button" runat="server" id="Button1" onclick="submitSelectItem()" class="btn-app btn-addItem">ยืนยัน</button>
                                    <button type="button" runat="server" id="Button2" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $('#Splitter_1_CC').css('height', '610px');
            var h = window.innerHeight;
            gridView.SetHeight(h - 270);

            //  Set default from
            var year = (new Date()).getFullYear();
            $('#datepickerFrom').val('01/01/' + year);

            //  Set default to
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth()+1; 
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            $('#datepickerTo').val(dd + '/' + mm + '/' + yyyy);
        });
        
        function clear_Click() {
            //$('#datepickerFrom').val("");
            //  Set default to
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth()+1; 
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            $('#datepickerTo').val(dd + '/' + mm + '/' + yyyy);

            $('#txt_product_name').val("");
            cbbShelf.SetValue("");
            cbbSupplier.SetValue("");
            cbbProductType.SetValue("");
            cbbProductCat.SetValue("");
            cbbProduct.SetValue("");
            cbbSecondaryCode.SetValue("");
            //$('#txt_product_no').val("");
            $.ajax({
                type: "POST",
                url: "StockCard.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback()
                },
                failure: function (response) {
                }
            });
        }
        function view_Click() {

            var productType = cbbProductType.GetValue();
            var datepickerFrom = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var datepickerTo = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            if (productType == "" || productType == null) {
                alert("กรุณาเลือกประเภทสินค้า");
                //} else if (datepickerFrom == "" || datepickerFrom == null) {
                //    alert("กรุณาเลือกวันที่");
                //}
                //else if (datepickerTo == "" || datepickerTo == null) {
                //    alert("กรุณาเลือกวันที่");
            } else {

                var product_name = '';//$('#txt_product_name').val();
                var shelf = cbbShelf.GetValue() == "" || cbbShelf.GetValue() == null ? 0 : cbbShelf.GetValue();
                var product_type = cbbProductType.GetValue() == "" || cbbProductType.GetValue() == null ? 0 : cbbProductType.GetValue();
                var product_cat = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
                var product_no = cbbProduct.GetValue() == "" || cbbProduct.GetValue() == null ? 0 : cbbProduct.GetValue();//$('#txt_product_no').val();
                var secondary_code = cbbSecondaryCode.GetValue() == "" || cbbSecondaryCode.GetValue() == null ? 0 : cbbSecondaryCode.GetValue();//$('#txt_secondary_code').val();
                var supplier = cbbSupplier.GetValue() == "" || cbbSupplier.GetValue() == null ? 0 : cbbSupplier.GetValue();

                if (productType == "PP") {
                    var parametersStockCard = {
                        dataStockCard: [{
                            datepickerFrom: datepickerFrom,
                            datepickerTo: datepickerTo,
                            product_name: product_name,
                            shelf: 0,
                            secondary_code: 0,
                            product_type: product_type,
                            product_cat: product_cat,
                            product_no: product_no,
                            supplier: supplier
                        }]
                    };
                } else {
                    var parametersStockCard = {
                        dataStockCard: [{
                            datepickerFrom: datepickerFrom,
                            datepickerTo: datepickerTo,
                            product_name: product_name,
                            shelf: shelf,
                            secondary_code: secondary_code,
                            product_type: product_type,
                            product_cat: product_cat,
                            product_no: product_no,
                            supplier: supplier
                        }]
                    };
                }
                $.ajax({
                    type: 'POST',
                    url: "StockCard.aspx/GetViewGrid",
                    data: JSON.stringify(parametersStockCard),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d != "error") {
                            gridView.PerformCallback();
                            $('#btnReportClient').show();
                        } else {
                            alert("ไม่พบข้อมูลที่ระบุ");
                            location.reload();
                        }

                    }
                });
            }

        }

        function callReport() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var datepickerFrom = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var datepickerTo = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var product_name = $('#txt_product_name').val();
            var shelf = cbbShelf.GetValue() == "" || cbbShelf.GetValue() == null ? 0 : cbbShelf.GetValue();
            var product_type = cbbProductType.GetValue() == "" || cbbProductType.GetValue() == null ? 0 : cbbProductType.GetValue();
            var product_cat = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
            var product_no = cbbProduct.GetValue() == "" || cbbProduct.GetValue() == null ? 0 : cbbProduct.GetValue();//$('#txt_product_no').val();
            //console.log("datepickerFrom====" + datepickerFrom);
            //console.log("datepickerTo====" + datepickerFrom);
            //console.log("product_id====" + product_id);

            if (product_type == "" || product_type == null) {
                alert("กรุณาเลือกประเภทสินค้า");
                //}
                //else if (datepickerFrom == "" || datepickerFrom == null) {
                //    alert("กรุณาเลือกวันที่");
                //}
                //else if (datepickerTo == "" || datepickerTo == null) {
                //    alert("กรุณาเลือกวันที่");
            } else {
                window.open("../Report/DocumentViewer.aspx?ReportArgs=Stock_Card|" + product_name + "|" + datepickerFrom + "|" + datepickerTo + "|" + product_type + "|" + shelf + "|" + product_cat + "|" + product_no, "_blank");
                $.LoadingOverlay("hide");
            }
        }

        function addItemDetail(item_type) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "StockCard.aspx/PopupItemDetail",
                data: "{item_type : '" + item_type + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewSelectedItem.PerformCallback();
                    $('#popupSelectedItem').modal("show");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {

                }
            });
        }

        function submitSelectItem() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "StockCard.aspx/SubmitSelectedItem",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //gridView.PerformCallback()
                    $('#txt_product_no').val(response.d);
                    $('#popupSelectedItem').modal("hide");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    $.LoadingOverlay("hide");
                }
            });
        }

        function getCheckBoxValue(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            $.ajax({
                type: "POST",
                url: "StockCard.aspx/SelectedItem",
                data: '{key: ' + key + ' , isSelected: ' + value + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //gridView.PerformCallback();
                    //$('#popupSelectedItem').modal("hide");
                },
                failure: function (response) {

                }
            });
        }

        function searchProductGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                gridViewSelectedItem.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
    </script>
</asp:Content>
