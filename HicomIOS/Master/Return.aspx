﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Return.aspx.cs" Inherits="HicomIOS.Master.Return" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <style>
        fieldset {
            border: 1px solid #4b4b57;
            display: block;
            margin-bottom: 10px;
            padding: 0 10px;
        }

        legend {
            width: auto;
            display: block;
            -webkit-padding-start: 2px;
            -webkit-padding-end: 2px;
            border-width: initial;
            border-style: none;
            border-color: initial;
            border-image: initial;
            margin-bottom: 10px;
            font-size: 14px;
            color: inherit;
            font-weight: bold;
        }

        #Splitter_0 {
            display: none;
        }

        #div-content {
            padding: 10px 30px;
            background-color: #ffffff;
            margin: 20px 0;
        }

            #div-content .row.form-group {
                margin-left: 0;
                margin-right: 0;
            }

        .no-padding {
            padding: 0px;
        }

        .no-padding-right {
            padding-right: 0px;
        }

        .no-margin {
            margin: 0px;
        }

        .label-rigth {
            padding: 4px 8px 0 0;
        }

        .form-group {
            margin-bottom: 8px;
        }

            .form-group label {
                font-weight: normal;
            }

        .form-control {
            border-radius: 0;
            padding: 0 3px;
            font-size: 11px;
            height: 22px;
        }

        input[type=checkbox], input[type=radio] {
            margin: 1px 0 0;
        }

        .nav-tabs > li.active > a, .nav-tabs > li.active > a:focus, .nav-tabs > li.active > a:hover {
            border: 1px solid #4b4b57;
            border-bottom-color: transparent;
        }

        .nav-tabs {
            border-bottom: 1px solid #4b4b57;
        }

            .nav-tabs > li > a {
                border-radius: 0;
            }

                .nav-tabs > li > a:hover {
                    border-color: #eee #eee #4b4b57;
                }

        .nav > li > a:focus, .nav > li > a:hover {
            background-color: #faedb6;
        }

        a {
            color: inherit;
        }

            a:focus, a:hover {
                color: inherit;
            }

        #div-content .tab-content .tab-pane {
            padding-top: 10px;
        }

        .txt-no {
            background-color: #e4effa !important;
        }

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
             height: 270px !important;
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
    </style>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $("#txtReturnDate").prop("readonly", "readonly");

            $("#txtReturnDate").datepicker({
                dateFormat: 'dd/mm/yy'
            });


        });

        function selectAllIssue() {

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkSelectAllIssue').is(':checked');
            $.ajax({
                type: "POST",
                url: "Return.aspx/SelectAllIssue",
                data: '{selected : ' + selected + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewRefDocDetail.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {

                }
            });
        }

        function changedReturnType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#lbIssueDate').val("");
            $('#lbCustomerNo').val("");
            $('#lbCustomerFirstName').val("");

            cbbReturnDoc.PerformCallback();

            gridViewReturn.PerformCallback();

            $.LoadingOverlay("hide");
        }
        function changedReturnDocSelected() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var data = cbbQuotationNo;
            var key = cbbReturnDoc.GetValue();
            var returnDoc = $('#cbbReturnType').val();


            $.ajax({
                type: "POST",
                url: "Return.aspx/GetRefDocData",
                data: '{id: "' + key + '" , type : "' + returnDoc + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#lbIssueDate').val(data.display_doc_date);
                    $('#lbCustomerNo').val(data.customer_code);
                    $('#lbCustomerFirstName').val(data.customer_name);
                    $('#hdCustomerId').val(data.customer_id);
                    $('#hdProductType').val(data.product_type);
                    gridViewRefDocDetail.PerformCallback();
                    gridViewReturn.PerformCallback("clear");
                },
                failure: function (response) {
                    //console.log(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function changedItemCondition() {

        }

        function popupSelectedDetail() {
            var key = cbbReturnDoc.GetValue();
            var returnDoc = $('#cbbReturnType').val();
            if (key == "" || key == null || key == undefined) {
                alert("กรุณาเลือกเอกสาร");
                return;
            }

            $.ajax({
                type: "POST",
                url: "Return.aspx/GetIssueDetailData",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    gridViewRefDocDetail.PerformCallback();

                    setTimeout(function () {

                        $('#modal_issue').modal('show');
                    }, 0.8);
                }

            });

        }

        function chkproduct(s, e) {

            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("IssueDetailId");
            $.ajax({
                type: "POST",
                url: "Return.aspx/AddReturnDetail",
                data: '{id:"' + key + '" , isSelected : ' + value + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log("Add Complete");
                }
            });
        }

        function submitIssueDetail() {
            $.ajax({
                type: "POST",
                url: "Return.aspx/SubmitIssueDetail",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewReturn.PerformCallback();

                    setTimeout(function () {
                        $('#modal_issue').modal('hide');
                    }, 0.8);
                }
            });
        }

        function editReturnDetail(element, e) {
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedReturnDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "Return.aspx/EditReturnDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProductDescription').val(data.product_name);
                    $('#txtProductQty').val(data.qty);
                    $('#txtProductRemark').val(data.remark);
                    $('#hdRefQty').val(data.ref_qty);
                    $('#modal_return_detail .modal-header').html('Edit Return Detail : ' + element.title);
                    $('#modal_return_detail').modal('show');
                }
            });

        }

        function deleteReturnDetail(e) {
            var key = e;

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
                       url: "Return.aspx/DeleteReturnDetail",
                       data: '{id:"' + key + '"}',
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       success: function (data) {
                           swal("ลบข้อมูลสำเร็จ!", {
                               icon: "success"
                           })
                           .then((value) => {
                               $('#hdSelectedReturnDetailId').val(0);
                               gridViewReturn.PerformCallback()
                           });
                       }
                   });
               }
           });
        }

        function submitReturnDetail() {
            var key = $('#hdSelectedReturnDetailId').val();
            var qty = $('#txtProductQty').val();
            var remark = $('#txtProductRemark').val();
            var ref_qty = $('#hdRefQty').val();
            if (parseInt(ref_qty) < parseInt(qty)) {
                alert("จำนวนสินค้าคืนมากกว่าจำนวนสินค้าที่รับมา");
                return;
            }
            $.ajax({
                type: "POST",
                url: "Return.aspx/SubmitReturnDetail",
                data: '{id: "' + key + '" , qty: "' + qty + '" , remark: "' + remark + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProductDescription').val("");
                    $('#txtProductQty').val(0);
                    $('#hdSelectedReturnDetailId').val(0);

                    $('#modal_return_detail').modal('hide');

                    gridViewReturn.PerformCallback()
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }
        function backPage() {
            var returnDoc = $('#cbbReturnType').val();
            window.location.href = "../Master/ReturnList.aspx?type=" + returnDoc;
        }

        function saveDraft() {
            if (cbbReturnDoc.GetValue() == null) {
                alert("กรุณาเลือกเอกสารสำหรับการคืน");
                return;
            }
            $('#btnSaveDraft').click();
        }

        function searchSeletedGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchText").val();
                gridViewRefDocDetail.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function confirmSave() {
            if (cbbReturnDoc.GetValue() == null) {
                swal("กรุณาเลือกเอกสารสำหรับการคืน");
                return;
            }

            $.ajax({
                type: "POST",
                url: "Return.aspx/ValidateData",
                data: '{type : "1" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        var txtReturnNo = $('#txtReturnNo').val();
                        //var confirm_true = confirm("ต้องการยืนยันเอกสาร " + txtReturnNo + "ใช่หรือไม่?", 'btnConfirm');
                        swal({
                            title: "ต้องการยืนยันเอกสาร " + txtReturnNo + "ใช่หรือไม่?",
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                        })
                      .then((confirmSave) => {
                          if (confirmSave) {
                              swal("บันทึกข้อมูลสำเร็จ!", {
                                  icon: "success"
                              })
                                .then((value) => {
                                    $('#btnConfirm').click();
                                });
                          }
                      });
                    }
                    else {
                        swal(response.d);
                    }

                },
                failure: function (response) {

                }
            });
        }
        function callReport() {
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Return|" + $("#txtReturnNo").val(), "_blank");
        }

        function addNewItem() {
            var returnDoc = $('#cbbReturnType').val();
            window.location.href = "Return.aspx?type=" + returnDoc;
        }
    </script>
    <div id="div-content">
        <div class="row">
            <button type="button" runat="server" onclick="addNewItem()" id="btnNew" class="btn-addItem">
                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;New
            </button>
            <button type="button" runat="server" id="btnDraft" onclick="saveDraft()" class="btn-addItem">
                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
            </button>
            <button type="button" runat="server" id="btnSave" onclick="confirmSave()" class="btn-addItem">
                <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Confirm
            </button>
            <button type="button" runat="server" id="btnReportClient" onclick="callReport()" class="btn-addItem">
                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Report
            </button>
            <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List Return
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnSaveDraft" UseSubmitBehavior="false"   Style="visibility: hidden" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnConfirm" UseSubmitBehavior="false"  Style="visibility: hidden" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
        </div>
        <div class="row">
            <fieldset>
                <legend>Header</legend>
                <div class="col-xs-12 no-padding">
                    <div id="returnType" runat="server" class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ประเภทการคืน :</label>
                            <div class="col-xs-9 no-padding">
                                <select class="form-control" id="cbbReturnType" onchange="changedReturnType()"
                                    runat="server" data-rule-required="true" data-msg-required="ประเภทการคืน">
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">วันที่เบิก/ยืม :</label>
                            <div class="col-xs-9 no-padding">
                                <input readonly="" class="form-control" id="lbIssueDate" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">เลขที่เอกสารใบคืน :</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control txt-no" id="txtReturnNo" readonly="" runat="server" />
                            </div>
                        </div>
                    </div>
                    </div>
                 <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">เลขที่ใบเบิก/ยืม :</label>
                            <div class="col-xs-9 no-padding">
                                <!--<input type="text" class="form-control" id="txtSaleOrderNo" runat="server" />-->
                                <dx:ASPxComboBox ID="cbbReturnDoc" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbReturnDoc" TextField="data_text" OnCallback="cbbReturnDoc_Callback"
                                    ClientSideEvents-ValueChanged="changedReturnDocSelected" ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding hidden">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">รหัสลูกค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <input readonly="" class="form-control" id="lbCustomerNo" runat="server" />
                            </div>
                        </div>
                    </div>
                     <div class="col-xs-4 no-padding" id="dvSupplier" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ชื่อผู้ขาย :</label>
                            <div class="col-xs-9 no-padding">
                                <input readonly="" class="form-control" id="lbSupplierName" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <input readonly="" class="form-control" id="lbCustomerFirstName" runat="server" />
                            </div>
                        </div>
                    </div>
                 </div>
                 <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">วันที่รับคืน :</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control" id="txtReturnDate" readonly="" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">สภาพสินค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <select class="form-control" id="cbbCondition" runat="server" data-rule-required="true" data-msg-required="วัตถุประสงค์ในการเบิก">
                                    <option value="G">Good</option>
                                    <option value="D">Defective</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-12 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-1 text-right label-rigth">หมายเหตุ :</label>
                            <div class="col-xs-7 no-padding">
                                <input type="text" class="form-control" id="TxtRemark" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="row form-group">
                    <button type="button" id="btnIssueDetail" runat="server" onclick="popupSelectedDetail()"
                        class="btn-info" style="height: 30px; margin-left: 15px">
                        <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกรายการ สินค้า
                    </button>
                </div>
                <div class="col-xs-12">
                    <dx:ASPxGridView ID="gridViewReturn" ClientInstanceName="gridViewReturn" runat="server" Settings-VerticalScrollBarMode="Visible"
                        Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                        OnCustomCallback="gridViewReturn_CustomCallback"
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
                            <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <a id="btnEdit" class="btn btn-mini" onclick="editReturnDetail(this, <%# Eval("id")%>)" title="<%# Eval("product_no")%>">
                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                    </a>
                                    |
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteReturnDetail(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="product_no" Caption="Product No" Width="150px" />
                            <dx:GridViewDataColumn FieldName="product_name" Caption="Product Name" Width="300px" />
                            <dx:GridViewDataColumn FieldName="mfg_no" Caption="MFG No" Width="80px" />
                            <dx:GridViewDataColumn FieldName="qty" Caption="Quantity" Width="80px" />
                            <dx:GridViewDataColumn FieldName="product_unit" Caption="Unit" Width="100px" />
                        </Columns>
                    </dx:ASPxGridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modal_issue" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-4 text-left">
                                <input id="chkSelectAllIssue" type="checkbox" class="" onclick="selectAllIssue()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchText" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchText" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewRefDocDetail" ClientInstanceName="gridViewRefDocDetail" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewRefDocDetail_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewRefDocDetail_HtmlDataCellPrepared"
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
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="30px">
                                            <DataItemTemplate>
                                               
                                                <dx:ASPxCheckBox ID="chkIssueDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkIssueDetail"
                                                    IssueDetailId='<%# Eval("id")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="80px" />
                                        <dx:GridViewDataTextColumn FieldName="product_name_tha" Caption="Product Name" VisibleIndex="5" Width="120px" />
                                        <dx:GridViewDataColumn FieldName="mfg_no" Caption="MFG No" Width="60px" VisibleIndex="1" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px" />
                                        <dx:GridViewDataTextColumn FieldName="unit_tha" Caption="Unit Code" VisibleIndex="7" Width="50px" />


                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-9 no-padding text-right" style="margin-top: 15px;">
                                        <button type="button" runat="server" id="Button1" onclick="submitIssueDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button2" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_return_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Edit Return Detail
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Description :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" readonly="true" id="txtProductDescription" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtProductQty" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Remark :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtProductRemark" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <%--  <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Unit Price :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtProductUnitPrice" runat="server" />
                                    </div>
                                </div>
                            </div>--%>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-7 no-padding text-right">
                                        <button type="button" runat="server" id="Button7" onclick="submitReturnDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="hidden">
            <asp:HiddenField runat="server" ID="hdCustomerId" />
            <asp:HiddenField runat="server" ID="hdSelectedReturnDetailId" />
            <asp:HiddenField runat="server" ID="hdRefQty" />
            <asp:HiddenField runat="server" ID="hdProductType" />
        </div>
    </div>
</asp:Content>
