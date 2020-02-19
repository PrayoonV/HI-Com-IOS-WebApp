﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="StockChecking.aspx.cs" Inherits="HicomIOS.Master.StockChecking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn-addItem {
            height: 25px;
            padding: 3px 15px;
            color: #1E395B;
            font: 11px Verdana, Geneva, sans-serif;
            border: 1px solid #abbad0;
            background: #d1dfef url(/DXR.axd?r=0_3934-wP3qf) repeat-x left top;
        }

        .btn-addItem:hover {
            background: #fcf8e5 url(/DXR.axd?r=0_3935-wP3qf) repeat-x left top;
            border: 1px solid #eecc53;
        }

        .btn-addItem i {
            font-size: 12px;
        }

        .btn-app {
            width: 65px;
        }

        .modal-open {
            opacity: 0.7;
        }

        .modal-content {
            border-radius: 0;
            background-color: white;
            /*border: 1px solid #909aa6;*/
            border-collapse: separate;
        }

        .modal-content .modal-header {
            background: #bbcee6 url(/DXR.axd?r=0_4062-wP3qf) repeat-x left top;
            border-bottom: 1px solid #909aa6;
            padding: 8px 0 6px 12px;
            color: #1e395b;
            font: 11px Verdana, Geneva, sans-serif;
            height: 29px;
        }

        .modal-backdrop {
            opacity: 0.5 !important;
        }

        .hr-line {
            border-top: 1px solid #000;
        }

        .shipping-step {
            float: left;
            padding: 0 15px;
        }

        .swal-modal {
            width: 420px !important;
            height: 150px !important;
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
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $('#Splitter_1_CC').css('height', '610px');
            var h = window.innerHeight;
            gridViewStockCheckDetail.SetHeight(h - 220);

            var allowType = '<%= HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"].ToString().ToUpper() %>';
            $("#cbbProductType option").each(function () {
                if (allowType.indexOf($(this).val()) == -1) {
                    $(this).remove();
                }
            });
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
            //console.log(height);
            gridViewStockCheckDetail.SetHeight(height);
        });


        function changedProductType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({

                type: "POST",
                url: "StockChecking.aspx/ClearData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var key = $('#cbbProductType').val();
                    $('#hdProductTypeId').val(key);
                    gridViewStockCheckDetail.PerformCallback();

                },
                failure: function (response) {
                    console.log(response.d);
                    $.LoadingOverlay("hide");
                }
            });
            $.LoadingOverlay("hide");

        }

        function popupAddProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var type = $('#cbbProductType').val();
            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/GetProductDetail",
                data: '{type: "' + type + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewProductDetail.PerformCallback();
                    $('#modal_product').modal('show');
                },
                failure: function (response) {
                    console.log(response.d);
                    $.LoadingOverlay("hide");
                }
            });
            $.LoadingOverlay("hide");
        }

        function chkproduct(s, e) {

            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductNo");
            var type = $('#cbbProductType').val();
            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/AddSelectedProduct",
                data: '{key:"' + key + '" , isSelected : ' + value + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response.d);
                }
            });
        }
        function submitProductDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/SubmitProductDetail",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#modal_product').modal('hide');
                    gridViewStockCheckDetail.PerformCallback();
                }
            });
            $.LoadingOverlay("hide");
        }

        function editStockCheckDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/GetStockDetailEdit",
                data: "{id : '" + e + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdStockDetailId').val(e);
                    $('#txtProductDescription').val(data.product_name);
                    $('#txtRemainQty').val(data.quantity_remain);
                    $('#txtProductRemark').val(data.remark);
                    $('#txtQty').val(data.quantity);
                    $('#txtReserveQty').val(data.quantity_reserve);
                    $('#modal_stock_detail').modal('show');
                    gridViewStockCheckDetail.PerformCallback();
                }
            });
            $.LoadingOverlay("hide");

        }
        function submitStockDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var qty_remain = $('#txtRemainQty').val();
            var remark = $('#txtProductRemark').val();
            var key = $('#hdStockDetailId').val();
            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/SubmitStockDetailEdit",
                data: "{id : '" + key + "', qty_remain : '" + qty_remain + "' , remark : '" + remark + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdStockDetailId').val(0);
                    $('#txtProductDescription').val("");
                    $('#txtProductRemark').val(0);
                    $('#txtRemark').val("")
                    $('#txtReserveQty').val(0);

                    gridViewStockCheckDetail.PerformCallback();
                    $('#modal_stock_detail').modal('hide');
                }
            });
            $.LoadingOverlay("hide");

        }
        function exportExcel() {

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var type = $('#cbbProductType').val();
            $.ajax({
                type: "POST",
                url: "StockChecking.aspx/ExportExcel2",
                data: "{type : '" + type + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $.LoadingOverlay("hide");

                    var data = response.d;
                    console.log(data);

                    window.open("../Common/DownloadFile.aspx?" + data, '_blank');

                }
            });
            //$.LoadingOverlay("hide");
        }
        function onUploadControlFileUploadStart(s, e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            console.log(s);
            console.log(e);
        }
        function onUploadControlFileUploadComplete(s, e) {

            if (e.isValid) {
                $('#cbbProductType').val(e.callbackData);
            }
            gridViewStockCheckDetail.PerformCallback();

            $.LoadingOverlay("hide");
        }
        function confirmSave() {
            swal({
                title: "ต้องการยืนยันการนับสต๊อค ใช่หรือไม่?",

                buttons: true,
            })
             .then((confirm_true) => {
                 if (confirm_true) {
                     $('#btnConfirm').click();
                 }
             });

            //$('#btnConfirm').click();
        }
        function backPage() {
            window.location.href = "StockCheckingList.aspx";
        }
    </script>
    <div id="div-content">
        <div class="row">
            <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                <i class="fa fa-chevron-circle-left" aria-hidden="true"></i>&nbsp;ย้อนกลับ
            </button>
        </div>
        <div class="row">
            <fieldset id="fieldset_step1" runat="server" class="col-xs-11" style="margin-top: 10px;">
                <legend id="legend_step1" runat="server">Step 1: เลือกประเภทข้อมูล เพื่อทำการนับสต๊อก </legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-5 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ประเภทข้อมูล :</label>
                            <div class="col-xs-7 no-padding">
                                <select class="form-control" id="cbbProductType" onchange="changedProductType()"
                                    runat="server" data-rule-required="true" data-msg-required="ประเภทสินค้า">
                                    <option value="P">Product</option>
                                    <option value="S">Spare Part</option>
                                </select>
                            </div>
                            <div class="col-xs-2 no-padding">
                                <button type="button" runat="server" id="btnExport" onclick="exportExcel()" class="form-control">
                                    <i class="fa fa-cloud-download" aria-hidden="true"></i>&nbsp;Export</button>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-3 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">เลขที่เอกสาร :</label>
                            <div class="col-xs-8 no-padding">
                                <input type="text" class="form-control" id="txtStockCheckNo" readonly="" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-3 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-4 text-right label-rigth">วันที่เอกสาร :</label>
                            <div class="col-xs-8 no-padding">
                                <input type="text" class="form-control" id="txtStockCheckDate" readonly="" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="row">&nbsp</div>
                </div>
            </fieldset>
            <fieldset id="fieldset_step2" runat="server" class="col-xs-7" style="display: inline-block; height: 70px;">
                <legend>Step 2 : ทำการเลือกรายการ หรือนำเข้าไฟล์นับสต๊อก </legend>
                <div class="row form-group">
                    <div class="col-xs-12 no-padding">
                        <button type="button" id="btnAddProduct" runat="server" onclick="popupAddProduct()"
                            class="btn-info" style="height: 30px; margin-left: 15px;">
                            <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกบางรายการที่ต้องการนับ
                        </button>
                        หรือ นำเข้าไฟล์ : 
                        <div style="display: inline-block; vertical-align: middle;">
                            <dx:ASPxUploadControl ID="btnImportExcel" ClientInstanceName="btnImportExcel" runat="server" UploadMode="Auto"
                                AutoStartUpload="True"
                                ShowProgressPanel="True" CssClass="uploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete">
                                <AdvancedModeSettings EnableDragAndDrop="false" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZone" DropZoneText="" />
                                <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".xlsx" ErrorStyle-CssClass="validationMessage">
                                    <ErrorStyle CssClass="validationMessage"></ErrorStyle>
                                </ValidationSettings>
                                <BrowseButton Text="Import" Image-SpriteProperties-CssClass="fa-download" />
                                <DropZoneStyle CssClass="uploadControlDropZone" />
                                <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                <ClientSideEvents
                                    FilesUploadStart="onUploadControlFileUploadStart"
                                    FileUploadComplete="onUploadControlFileUploadComplete"></ClientSideEvents>
                            </dx:ASPxUploadControl>
                        </div>
                    </div>
                </div>
            </fieldset>
            &nbsp&nbsp&nbsp
            <fieldset id="fieldset_step3"  runat="server" class="col-xs-4" style="display: inline-block; height: 70px;">
                <legend>Step 3 : ทำการยืนยันการนับสต๊อก</legend>
                <div class="row form-group">
                    <div class="col-xs-12 no-padding">
                        <button type="button" runat="server" id="btnSave" onclick="confirmSave()" class="btn-addItem">
                            <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Confirm
                        </button>
                        <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnSaveDraft" Style="visibility: hidden" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
                        <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnConfirm" Style="visibility: hidden" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="col-xs-12">
                    <dx:ASPxGridView ID="gridViewStockCheckDetail" ClientInstanceName="gridViewStockCheckDetail" runat="server"
                        EnableCallBacks="true"
                        OnCustomCallback="gridViewStockCheckDetail_CustomCallback"
                        KeyFieldName="id" Width="100%">
                        <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                        <Paddings Padding="0px" />
                        <Border BorderWidth="0px" />
                        <BorderBottom BorderWidth="1px" />
                        <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                            PageSizeItemSettings-Visible="false">
                            <PageSizeItemSettings Items="10, 20, 50" />
                        </SettingsPager>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                                <DataItemTemplate>
                                    <a id="btnEdit" class="btn btn-mini" onclick="editStockCheckDetail(<%# Eval("id")%>)" title="Edit">
                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                    </a>
                                    |
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteStockCheckDetail(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="product_no" Caption="รหัสสินค้า" Width="80px" />
                            <dx:GridViewDataColumn FieldName="product_name" Caption="ชื่อสินค้า/อะไหล่" Width="200px" />
                            <dx:GridViewDataColumn FieldName="unit_code" Caption="หน่วย" Width="40px" />
                            <dx:GridViewDataTextColumn FieldName="before_qty" Caption="จำนวนก่อนนับ" Width="50px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="quantity_remain" Caption="จำนวนหลังนับ" Width="50px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="remark" Caption="หมายเหตุ" Width="100px" />
                        </Columns>
                        <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                    </dx:ASPxGridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modal_stock_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content" style="width: 600px">
                    <div class="modal-header">
                        กำหนดจำนวนสต๊อกที่ต้องการปรับปรุง
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">ชื่อสินค้า/อะไหล่ :</label>
                                    <div class="col-xs-8 no-padding">
                                        <input type="text" class="form-control" readonly="true" disabled="disabled" id="txtProductDescription" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">จำนวนปัจจุบัน:</label>
                                    <div class="col-xs-3 no-padding">
                                        <input type="text" class="form-control numberic" readonly="" disabled="disabled" id="txtQty" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">จำนวนที่จอง :</label>
                                    <div class="col-xs-3 no-padding">
                                        <input type="text" class="form-control numberic" readonly="" disabled="disabled" id="txtReserveQty" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">จำนวนที่จะปรับปรุง :</label>
                                    <div class="col-xs-3 no-padding">
                                        <input type="text" class="form-control numberic" id="txtRemainQty" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">หมายเหตุ :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtProductRemark" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <div class="col-xs-12 text-center" style="padding: 10px">
                                        <button type="button" runat="server" id="Button7" onclick="submitStockDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_product" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content" style="width: 900px">
                    <div class="modal-header">
                        เลือกรายการที่ต้องการนับสต๊อก
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewProductDetail" ClientInstanceName="gridViewProductDetail" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewProductDetail_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewProductDetail_HtmlDataCellPrepared"
                                    KeyFieldName="product_no" Width="100%">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsSearchPanel Visible="true" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="false">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="30px">
                                            <DataItemTemplate>
                                                <%-- <input type="checkbox" id="productId" name="quotationDetailId"
                                                    onchange="chkproduct(<%# Eval("id") %>)" />--%>
                                                <dx:ASPxCheckBox ID="chkProductDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkProductDetail"
                                                    ProductNo='<%# Eval("product_no")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="200px" />
                                        <dx:GridViewDataTextColumn FieldName="product_name" Caption="Product Name" VisibleIndex="5" Width="500px" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px">
                                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="item_type" Caption="Type" VisibleIndex="7" Width="100px" />--%>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <div class="col-xs-12 text-center" style="padding: 10px">
                                        <button type="button" runat="server" id="Button3" onclick="submitProductDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button4" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="hidden">
            <asp:HiddenField runat="server" ID="hdStockDetailId" />
            <asp:HiddenField runat="server" ID="hdProductTypeId" />
        </div>
    </div>
</asp:Content>