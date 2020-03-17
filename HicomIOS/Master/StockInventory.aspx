<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="StockInventory.aspx.cs" Inherits="HicomIOS.Master.StockInventory" %>

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
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
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
            gridView.SetHeight(h - 250);
        });

        function changedProductType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "StockInventory.aspx/ChangedData",
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
                $('#Div2').hide();
                $('.sp-label').show();
                $('.pp-label').hide();
                $('#btnGroup').show();
                cbbShelf.PerformCallback();
            }
            if (key == "PP") {
                $('#shelf').hide();
                $('#Div2').show();
                $('.pp-label').show();
                $('.sp-label').hide();
                $('#btnGroup').show();
                cbbProductModel.PerformCallback();
            }
            if (key == "SS") {
                $('#btnGroup').hide();
            }
            cbbProduct.PerformCallback();
            cbbProductCat.PerformCallback(key);
            //cbbProduct.PerformCallback(key);

            $.LoadingOverlay("hide");

        }

    </script>
    <div id="div-content">
        <div class="row">
            <fieldset>
                <legend>Stock Checking</legend>
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
                    <div class="col-xs-4 no-padding hidden">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Product Name :</label>
                            <div class="col-xs-7 no-padding">
                                <input class="form-control" runat="server" type="text" id="txt_product_name" />
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
                    <div class="col-xs-4 no-padding" id="Div2" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">Product Model :</label>
                            <div class="col-xs-7 no-padding">
                                <dx:ASPxComboBox ID="cbbProductModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbProductModel" TextField="data_text"
                                    EnableCallbackMode="true" OnCallback="cbbProductModel_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
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
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth hidden">Date From :</label>
                            <div class="col-xs-3 no-padding hidden">
                                <input class="form-control picker" runat="server" type="text" id="datepickerFrom" value="01/01/1970" />
                            </div>
                            <label class="col-xs-4 text-right label-rigth">Date :</label>
                            <div class="col-xs-7 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerTo" autocomplete="off" />
                            </div>
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
                                <asp:Button ID="btnReportExcel" runat="server" Text="Show Excel" CssClass="btn-addItem" OnClick="btnReportExcel_Click" style="height: 30px; width: 90%;" />
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
                    OnCustomCallback="gridStockInventory_CustomCallback">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                        PageSizeItemSettings-Visible="true">
                        <PageSizeItemSettings Items="10, 20, 50" />
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="cat_name" Caption="ประเภทสินค้า" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="no_" Caption="รหัสสินค้า" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="secondary_code" Caption="Secondary Code" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="name_" Caption="ชื่อสินค้า/อะไหล่" Width="80px" />
                        <dx:GridViewDataTextColumn FieldName="shelf_name" Caption="Shelf" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="previous_balance" Caption="ยอดยกมา" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="in_" Caption="รวมขาเข้า" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="out_" Caption="รวมขาออก" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="adj_" Caption="รวมปรับปรุง" Width="35px" Visible="false">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <%--
                        <dx:GridViewDataTextColumn FieldName="stock_balance" Caption="คงเหลือ" Width="40px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        --%>
                        <dx:GridViewDataTextColumn FieldName="stock_current" Caption="รวมก่อนจอง" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="reserve_" Caption="จอง" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="stock_current_after" Caption="รวมหลังจอง" Width="35px">
                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn FieldName="transaction_date" Caption="เคลื่อนไหวล่าสุด" Width="80px" Settings-SortMode="Value" Settings-AllowSort="True" Visible="false">
                            <PropertiesDateEdit DisplayFormatString="dd/MMM/yyyy hh:mm"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $('#Splitter_1_CC').css('height', '610px');
            var h = window.innerHeight;
            gridView.SetHeight(h - 250);
            
            //  Set default from
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
            $('#datepickerFrom').val("01/01/1970");

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
            cbbProductType.SetValue("");
            cbbProductCat.SetValue("");
            cbbProduct.setValue("");
            cbbProductModel.setValue("");
            $('#txt_product_no').val("");
            $.ajax({
                type: "POST",
                url: "StockInventory.aspx/ChangedData",
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

                var product_name = $('#txt_product_name').val();
                var shelf = cbbShelf.GetValue() == "" || cbbShelf.GetValue() == null ? 0 : cbbShelf.GetValue();
                var product_type = cbbProductType.GetValue() == "" || cbbProductType.GetValue() == null ? 0 : cbbProductType.GetValue();
                var product_cat = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
                var product_no = cbbProduct.GetValue() == "" || cbbProduct.GetValue() == null ? 0 : cbbProduct.GetValue();//$('#txt_product_no').val();
                var product_model = cbbProductModel.GetValue() == "" || cbbProductModel.GetValue() == null ? 0 : cbbProductModel.GetValue();

                if (productType == "PP") {
                    var parametersStockInventory = {
                        dataStockInventory: [{
                            datepickerFrom: datepickerFrom,
                            datepickerTo: datepickerTo,
                            product_name: product_name,
                            shelf: 0,
                            product_type: product_type,
                            product_cat: product_cat,
                            product_no: product_no,
                            product_model: product_model
                        }]
                    };
                } else {
                    var parametersStockInventory = {
                        dataStockInventory: [{
                            datepickerFrom: datepickerFrom,
                            datepickerTo: datepickerTo,
                            product_name: product_name,
                            shelf: shelf,
                            product_type: product_type,
                            product_cat: product_cat,
                            product_no: product_no,
                            product_model: ''
                        }]
                    };
                }
                $.ajax({
                    type: 'POST',
                    url: "StockInventory.aspx/GetViewGrid",
                    data: JSON.stringify(parametersStockInventory),
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
            var product_model = cbbProductModel.GetValue() == "" || cbbProductModel.GetValue() == null ? 0 : cbbProductModel.GetValue();
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
                window.open("../Report/DocumentViewer.aspx?ReportArgs=Stock_Inventory_Products|" + product_name + "|" + datepickerFrom + "|" + datepickerTo + "|" + product_type + "|" + shelf + "|" + product_cat + "|" + product_no, "_blank");
                $.LoadingOverlay("hide");
            }


        }

    </script>
</asp:Content>
