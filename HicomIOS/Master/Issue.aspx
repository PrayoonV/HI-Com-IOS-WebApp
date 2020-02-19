﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Issue.aspx.cs" Inherits="HicomIOS.Master.Issue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
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

        .txt-no {
            background-color: #e4effa !important;
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
            /*height: 250px !important;*/
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
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        //document.onkeydown = chkEvent
        //function chkEvent(e) {
        //    var keycode;
        //    if (window.event) keycode = window.event.keyCode; //*** for IE ***//
        //    else if (e) keycode = e.which; //*** for Firefox ***//
        //    if (keycode == 13) {
        //        return false;
        //    }
        //}
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $("#txtIssueDate").prop("readonly", "readonly");

            $("#txtIssueDate").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            //  Check type
            var allowType = '<%= HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"].ToString().ToUpper() %>';
            $("#cbbProductType option").each(function () {
                if (allowType.indexOf($(this).val()) == -1) {
                    $(this).remove();
                }
            });

            $('#txtIssueRemark').hide();

            $('#dvSaleOrder').show();
          
            $('#dvProject1').hide();
            //$('#dvProject2').show();
            $('#dvProject3').hide();
            //$('#dvProduct').hide();
            //$('#dvProductType').hide();

            changedIssueType(false);

            var h = window.innerHeight;
            gridViewIssue.SetHeight(h - 412);
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };

            var height = dimensions.height - 312;

            console.log(height);
            gridViewIssue.SetHeight(height);
        });

        function changeCbbIssue() {
            var key = $('#cbbIssueFor').val();
            if (key == "5") {
                $('#txtIssueRemark').show();
            }
            else {
                $('#txtIssueRemark').val("");
                $('#txtIssueRemark').hide();
            }
        }

        function changedSaleOrderSelected() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var data = cbbQuotationNo;
            var key = cbbSaleOrder.GetValue();

            $.ajax({
                type: "POST",
                url: "Issue.aspx/GetSaleOrderData",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnchangeQuotationSuccess,
                failure: function (response) {
                    //console.log(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function changedCustomerSelected() {
            var key = lbCustomerFirstName.GetValue();
            $('#hdCustomerId').val(key);
        }

        function OnchangeQuotationSuccess(response) {

            console.log(response.d);

            $("#lbCustomerID").val(response.d.customer_code);
            lbCustomerFirstName.SetValue(response.d.company_name_tha);
            $("#lbAttention").val(response.d.attention_name);

            //$("#lbFaxNumber").val(response.d.customer_fax);
            $("#lbProject").val(response.d.project_name);
            $("#lbSubject").val(response.d.quotation_subject);
            $("#lbSaleOrderDate").val(response.d.sales_order_date);
            $('#hdQuotationType').val(response.d.quotation_type);
            $('#hdCustomerId').val(response.d.customer_id);
            $('#hdQuotationNo').val(response.d.quotation_no);

            gridViewIssue.PerformCallback();

        }



        /* SALE ORDER DETAIL FUNCTION*/

        function popupSaleOrderDetail() {
            var issue_type = $('#cbbIssueType').val();
            var productType = $('#cbbProductType').val();

            if (issue_type == "1") {
                var key = cbbSaleOrder.GetValue();
                if (key == "" || key == null || key == undefined) {
                    swal("กรุณาเลือกเอกสาร Sale Order!");

                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "Issue.aspx/GetSaleOrderDetailData",
                    data: '{id: "' + key + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        gridViewSaleOrderDetail.PerformCallback();
                        //gridViewSaleOrderDetail.GoToPage(0);

                        setTimeout(function () {

                            $('#modal_saleOrder').modal('show');
                        }, 0.8);
                    }

                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "Issue.aspx/GetProductDetailData",
                    data: '{product_type: "' + productType + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        gridViewSaleOrderDetail.PerformCallback();
                        //gridViewSaleOrderDetail.GoToPage(0);

                        setTimeout(function () {

                            $('#modal_saleOrder').modal('show');
                        }, 0.8);
                    }

                });
            }
        }

        function chkproduct(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("SaleOrderDetailId");
            $.ajax({
                type: "POST",
                url: "Issue.aspx/AddIssueDetail",
                data: '{id:"' + key + '" , isSelected : ' + value + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Add Complete");
                }
            });
        }

        function submitSaleOrderDetail() {
            $.ajax({
                type: "POST",
                url: "Issue.aspx/SubmitSaleOrderDetail",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //$('#tabSaleOrder').css('display', '');
                    //$('#GridSaleOrder').css('display', '');
                    gridViewSaleOrderDetail.PerformCallback();
                    gridViewIssue.PerformCallback();
                    $('#txtSearchText').val('');
                    $('#chkSelectAllSaleOrder').prop('checked', false);

                    setTimeout(function () {
                        $('#modal_saleOrder').modal('hide');
                        //calDiscount();
                    }, 0.8);



                }

            });

        }
        /* END SALE ORDER DETAIL FUNCTION*/

        /*ISSUE DETAIL FUNCTION*/
        function editIssueDetail(element, e) {
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedIssueDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "Issue.aspx/EditIssueDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProductDescription').val(data.product_name_tha);
                    $('#txtProductQty').val(data.qty);
                    $('#txtProductRemark').val(data.remark);
                    var status = $('#hdDocumentFlag').val();
                    if (status == "CF" || status == "CP") {
                        $('#txtProductQty').prop('readonly', true);
                        $('#txtProductRemark').prop('readonly', true);
                    }
                    else {
                        $('#txtProductQty').prop('readonly', false);
                        $('#txtProductRemark').prop('readonly', false);
                    }
                    
                    $('#modal_issue_detail .modal-header').html('Edit Issue Detail : ' + element.title);
                    $('#modal_issue_detail').modal('show');
                }
            });
        }

        function deleteIssueDetail(e) {
            var key = e;
            $('#hdSelectedIssueDetailId').val(key);
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
                            url: "Issue.aspx/DeleteIssueDetail",
                            data: '{id:"' + key + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#hdSelectedIssueDetailId').val(0);
                                if (data.d != '') {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                        gridViewSaleOrderDetail.PerformCallback();
                                        gridViewIssue.PerformCallback();
                                    });
                                } else {
                                    /*swal("การลบข้อมูลผิดพลาด!", {
                                        icon: "error"
                                    });*/
                                    gridViewSaleOrderDetail.PerformCallback();
                                    gridViewIssue.PerformCallback();
                                }
                            }
                        });
                    }
                });
        }

        function submitIssueDetail() {
            var key = $('#hdSelectedIssueDetailId').val();
            var qty = $('#txtProductQty').val();
            var remark = $('#txtProductRemark').val();

            $.ajax({
                type: "POST",
                url: "Issue.aspx/SubmitIssueDetail",
                data: '{id: "' + key + '" , qty: "' + qty + '",remark : "' + remark + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProductDescription').val("");
                    $('#txtProductQty').val(0);

                    $('#hdSelectedIssueDetailId').val(0);


                    $('#modal_issue_detail').modal('hide');

                    gridViewIssue.PerformCallback()
                },
                failure: function (response) {

                }
            });
        }

        function popupMFGDetail(productId, issueDetailId) {
            //var input = s.GetMainElement();

            $('#hdSelectedIssueDetailId').val(issueDetailId);
            var issue_no = $('#txtIssueNo').val();

            $.ajax({
                type: "POST",
                url: "Issue.aspx/GetMFGDetail",
                data: '{id:"' + productId + '", issue_no: "' + issue_no + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    var status = $('#hdDocumentFlag').val();
                    $('#txtMFGProductDescription').val(data.product_name_tha);
                    $('#hdSelectedProductId').val(data.product_id);
                    gridViewIssueMFG.PerformCallback();
                    $('#modal_issue_mfg').modal('show');

                    if (status == "CP") {//(status == "CF" || status == "CP") {
                        $('#Button3').css("display", "none");
                    }
                    else {
                        $('#Button3').css("display", "");
                    }
                }
            });
        }

        function chkProductMFGDetail(s, e) {

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("mfgNo");
            var product_id = $('#hdSelectedProductId').val();
            var issueDetailId = $('#hdSelectedIssueDetailId').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/AddMFGProduct",
                data: '{mfg_no:"' + key + '" , isSelected : ' + value + ' , product_id :"' + product_id + '" , issue_detail_id : "' + issueDetailId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewIssueMFG.PerformCallback();
                    console.log("Add Complete");
                    $.LoadingOverlay("hide");
                }
            });
        }

        function changedUnitWarrantyProductMFGDetail(s, e) {
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("mfgNo");
            var product_id = $('#hdSelectedProductId').val();
            var issueDetailId = $('#hdSelectedIssueDetailId').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/TextChangedMFGProduct",
                data: '{mfg_no:"' + key + '" , value : ' + value + ' , type : "unit" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Add Complete");
                }
            });
        }
        function changedAirWarrantyProductMFGDetail(s, e) {
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("mfgNo");
            var product_id = $('#hdSelectedProductId').val();
            var issueDetailId = $('#hdSelectedIssueDetailId').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/TextChangedMFGProduct",
                data: '{mfg_no:"' + key + '" , value : ' + value + ' , type : "air" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Add Complete");
                }
            });
        }
        function changedFeeProductMFGDetail(s, e) {
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("mfgNo");
            var product_id = $('#hdSelectedProductId').val();
            var issueDetailId = $('#hdSelectedIssueDetailId').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/TextChangedMFGProduct",
                data: '{mfg_no:"' + key + '" , value : ' + value + ' , type : "fee" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Add Complete");
                }
            });
        }
        function submitMFGProduct() {
            var issueDetailId = $('#hdSelectedIssueDetailId').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/SubmitMFGProduct",
                data: "{issue_detail_id :" + issueDetailId + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        $('#hdSelectedIssueDetailId').val(0);
                        $('#txtMFGProductDescription').val("");
                        $('#hdSelectedProductId').val(0);
                        $('#modal_issue_mfg').modal('hide');
                    }
                    else {
                        alertMessage(response.d, "E");
                    }
                }
            });
        }
        /*END ISSUE*/
        function confirmSave() {
            var issue_type = $('#cbbIssueType').val();
            if (issue_type == "1") {
                if (cbbSaleOrder.GetValue() == null) {
                    swal("กรุณาเลือกเอกสาร Sale Order!");

                    return;
                }
            }
            var type = $('#hdQuotationType').val();
            if (issue_type == "0") // Set type = Product Type กรณี Issue ลอย
            {
                type = $('#cbbProductType').val();
            }
            var chkIssueDate = $('#chkIssueDate').is(":checked");
            var txtIssueDate = $('#txtIssueDate').val();
            var txtIssueNo = $('#txtIssueNo').val();
            var status = $('#hdDocumentFlag').val();
            var type = $('#cbbIssueFor').val();

            if (chkIssueDate) {
                swal("กรุณาระบุวันที่เบิก");
                return;
            }
            if (type == 'W') {
                swal("กรุณาระบุวัตถุประสงค์การเบิก");
                return;
            }
            

            $.ajax({
                type: "POST",
                url: "Issue.aspx/ValidateData",
                data: '{type : "' + type + '" , issue_type : "' + issue_type + '" ,chkIssue :  ' + chkIssueDate + ' , issue_date : "' + txtIssueDate + '", next_status: "CF", status : "' + status + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        swal("บันทึกข้อมูลสำเร็จ!", {
                            icon: "success"
                        })
                        .then((value) => {
                            $('#btnConfirm').click();
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
        function confirmCancel() {
            sendApproveMessage("", 'btnCancel', 'ยกเลิกเอกสาร');
        }

        function saveDraft() {
            var issue_type = $('#cbbIssueType').val();
            if (issue_type == "1") {
                if (cbbSaleOrder.GetValue() == null) {
                    swal("กรุณาเลือกเอกสาร Sale Order!");

                    return;
                }
            }
            var type = $('#hdQuotationType').val();
            if (issue_type == "0") // Set type = Product Type กรณี Issue ลอย
            {
                type = $('#cbbProductType').val();
            }
            if (lbCustomerFirstName.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");

                return;
            }
            var chkIssueDate = $('#chkIssueDate').is(":checked");
            var txtIssueDate = $('#txtIssueDate').val();
            var status = $('#hdDocumentFlag').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/ValidateData",
                data: '{type : "' + type + '" , issue_type : "' + issue_type + '" ,chkIssue :  ' + chkIssueDate + ' , issue_date : "' + txtIssueDate + '", next_status: "DR", status : "' + status + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        swal("บันทึกข้อมูลสำเร็จ!", {
                            icon: "success"
                        })
                            .then((value) => {
                                $('#btnSaveDraft').click();
                            });
                    } else {
                        swal(response.d).then((value) => {
                            $('#btnSaveDraft').click();
                        });
                    }
                },
                failure: function (response) {

                }
            });
        }

        function callReport() {
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Issue_Report|" + $("#txtIssueNo").val(), "_blank");
        }

        function backPage() {
            window.location.href = "../Master/IssueList.aspx";
        }
        function changedIssueType(is_refresh) {
            var issueType = $('#cbbIssueType').val();
            var productType = $('#cbbProductType').val();
            if (issueType == "1") {
                $('#dvSaleOrder').show();
                $('#dvProduct').hide();
                $('#dvProductType').hide();

               
                //$('#dvProject1').show();
                $('#dvProject2').show();
                //$('#dvProject3').show();
                //$('#cbbIssueFor').html("");
                //$('#cbbIssueFor').html('<option value="B">เบิกเพื่อยืม</option><option value="R">เบิกเพื่อใช้ในงานซ่อมแซม</option><option value="L">เบิกตัดชำรุด</option><option value="O">เบิกอื่นๆ</option>');
            }
            else {
                $('#hdQuotationType').val(productType);
                $('#dvSaleOrder').hide();
                $('#dvProduct').show();
                $('#dvProductType').show();
                //$('#dvProject1').hide();
                $('#dvProject2').hide();
                //$('#dvProject3').hide();
                //$('#cbbIssueFor').html('<option value="B">เบิกเพื่อยืม</option><option value="R">เบิกเพื่อใช้ในงานซ่อมแซม</option><option value="L">เบิกตัดชำรุด</option><option value="O">เบิกอื่นๆ</option>');
            }
            if (is_refresh) {
                $.ajax({
                    type: "POST",
                    url: "Issue.aspx/ClearSaleOrderDetail",
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        gridViewIssue.PerformCallback()
                    },
                    failure: function (response) {

                    }
                });
            }
        }
        function changedProductType() {
            var productType = $('#cbbProductType').val();
            $('#hdQuotationType').val(productType);

            $.ajax({
                type: "POST",
                url: "Issue.aspx/ClearSaleOrderDetail",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewIssue.PerformCallback();
                },
                failure: function (response) {

                }
            });


        }
        function selectAllSaleOrder() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkSelectAllSaleOrder').is(':checked');
            var searchText = $('#txtSearchText').val();
            $.ajax({
                type: "POST",
                url: "Issue.aspx/SelectAllSaleOrder",
                data: '{selected : ' + selected + ' , searchTextStr : "' + searchText + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var txtSearch = $("#txtSearchText").val();
                    var issue_type = $("#cbbIssueType").val();
                    gridViewSaleOrderDetail.PerformCallback("Search|" + issue_type + "|" + txtSearch.toString());
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {

                }
            });
        }
        function searchSeletedGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchText").val();
                var issue_type = $("#cbbIssueType").val();
                gridViewSaleOrderDetail.PerformCallback("Search|" + issue_type + "|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function onCheckIssueDate() {
            var chkIssueDate = $('#chkIssueDate').is(":checked");
            if (chkIssueDate) {
                $('#txtIssueDate').val("");
                $('#txtIssueDate').prop('disabled', true);
            }
            else {
                $('#txtIssueDate').val("");
                $('#txtIssueDate').prop('disabled', false);
            }
            console.log(chkIssueDate);
        }

        function addNewItem() {
            window.location.href = "Issue.aspx";
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
            <button type="button" runat="server" id="btnCancelItem" onclick="confirmCancel()" class="btn-addItem" >
                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Cancel
            </button>
            <button type="button" runat="server" id="btnSave" onclick="confirmSave()" class="btn-addItem">
                <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Confirm
            </button>
            <button type="button" runat="server" id="btnReportClient" onclick="callReport()" class="btn-addItem">
                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Report
            </button>
            <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List Issue
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior="false" ID="btnSaveDraft" Style="visibility: hidden" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior="false" ID="btnConfirm" Style="visibility: hidden" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior="false" ID="btnCancel" Style="visibility: hidden" Text="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
        </div>
        <div class="row">

            <fieldset>
                <legend>Header</legend>
                <div class="row" runat="server">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ประเภทการเบิก:</label>
                            <div class="col-xs-9 no-padding">
                                <select class="form-control" id="cbbIssueType" onchange="changedIssueType(true)" runat="server" data-rule-required="true" data-msg-required="ประเภทการเบิก">
                                    <option value="1">เบิกธรรมดา</option>
                                    <option value="0">เบิกลอย</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">วันที่ใบเบิก :</label>
                            <div class="col-xs-5 no-padding">
                                <input type="text" class="form-control" id="txtIssueDate" runat="server" />
                            </div>
                            <div class="col-xs-4 no-padding" style="padding-left: 10px;">
                                <input type="checkbox" id="chkIssueDate" runat="server" onchange="onCheckIssueDate()" />
                                ไม่ระบุวันที่เบิก
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">เลขที่เอกสาร :</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control txt-no" id="txtIssueNo" readonly="" runat="server" />
                            </div>
                        </div>
                    </div>

                </div>
                <div id="dvProductType" class="row" runat="server">
                    <div id="dvProduct" runat="server" class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ประเภทสินค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <select class="form-control" id="cbbProductType" onchange="changedProductType()" runat="server" data-rule-required="true" data-msg-required="ประเภทสินค้า">
                                    <option value="P">Product</option>
                                    <option value="S">Spare Part</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-4 no-padding" id="dvSaleOrder" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">เลขที่ใบสั่งขาย:</label>
                            <div class="col-xs-9 no-padding">
                                <!--<input type="text" class="form-control" id="txtSaleOrderNo" runat="server" />-->
                                <dx:ASPxComboBox ID="cbbSaleOrder" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbSaleOrder" TextField="data_text"
                                    ClientSideEvents-ValueChanged="changedSaleOrderSelected" ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-xs-4 no-padding hidden">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">รหัสลูกค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <input readonly="" class="form-control" id="lbCustomerID" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า :</label>
                            <div class="col-xs-8 no-padding">
                                <%--<input readonly="" class="form-control" id="lbCustomerFirstName" runat="server" />--%>
                                <dx:ASPxComboBox ID="lbCustomerFirstName" CssClass="form-control" runat="server"
                                    ClientInstanceName="lbCustomerFirstName" TextField="data_text"
                                    ValueField="data_value" ClientSideEvents-ValueChanged="changedCustomerSelected">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server">
                     <div  class="col-xs-4 no-padding">
                    <div class="row form-group">
                        <label class="col-xs-3 text-right label-rigth">วัตถุประสงค์การเบิก :</label>
                        <div class="col-xs-9 no-padding">
                            <select class="form-control" id="cbbIssueFor" onchange="changeCbbIssue()" runat="server" data-rule-required="true" data-msg-required="วัตถุประสงค์ในการเบิก">
                            </select>
                        </div>
                        <div class="col-xs-3 no-padding">
                            <input type="text" class="form-control" id="txtIssueRemark" runat="server" />
                        </div>
                    </div>
                    </div>
                    <div id="dvProject1" class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Subject :</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="lbSubject" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div id="dvProject2" class="col-xs-4 no-padding" runat="server">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">วันที่สั่งซื้อ :</label>
                            <div class="col-xs-8 no-padding">
                                <input readonly="" class="form-control" id="lbSaleOrderDate" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" runat="server">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">หมายเหตุ:</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control" id="txtRemark" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div id="dvProject3" class="col-xs-4 no-padding">

                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Project :</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="lbProject" runat="server" />
                            </div>
                        </div>

                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="row form-group">
                    <button type="button" id="btnSaleOrderDetail" runat="server" onclick="popupSaleOrderDetail()"
                        class="btn-info" style="height: 30px; margin-left: 15px;">
                        <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกรายการสินค้า
                    </button>
                </div>
                <div class="col-xs-12">
                    <dx:ASPxGridView ID="gridViewIssue" ClientInstanceName="gridViewIssue" runat="server" Settings-VerticalScrollBarMode="Visible"
                        Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                        OnCustomCallback="gridViewIssue_CustomCallback"
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
                                    <a id="btnEdit" class="btn btn-mini" onclick="editIssueDetail(this, <%# Eval("id")%>)" title="<%# Eval("product_no")%>">
                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                    </a>
                                    |
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteIssueDetail(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MFG Detail" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <a id="btnMFG" class="btn btn-mini" onclick="popupMFGDetail(<%# Eval("product_id")%>,<%# Eval("id")%>)" title="MFG">
                                        <i class="fa fa-list" aria-hidden="true"></i>
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="product_no" Caption="Product No" Width="100px" />
                            <dx:GridViewDataColumn FieldName="product_name_tha" Caption="Product Name" Width="150px" />
                            <dx:GridViewDataColumn FieldName="qty" Caption="Quantity" Width="80px" />
                            <dx:GridViewDataColumn FieldName="unit_tha" Caption="Unit" Width="80px" />
                            <dx:GridViewDataColumn FieldName="remark" Caption="Remark" Width="100px" />
                        </Columns>
                    </dx:ASPxGridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modal_saleOrder" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-4 text-left">
                                <input id="chkSelectAllSaleOrder" type="checkbox" class="" onclick="selectAllSaleOrder()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchText" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchText" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewSaleOrderDetail" ClientInstanceName="gridViewSaleOrderDetail" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewSaleOrderDetail_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewSaleOrderDetail_HtmlDataCellPrepared"
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
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="30px" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <%-- <input type="checkbox" id="productId" name="quotationDetailId"
                                                    onchange="chkproduct(<%# Eval("id") %>)" />--%>
                                                <dx:ASPxCheckBox ID="chkSaleOrderDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkSaleOrderDetail"
                                                    SaleOrderDetailId='<%# Eval("id")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" Width="5px" VisibleIndex="1" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="2" Width="70px" />
                                        <dx:GridViewDataTextColumn FieldName="saleorder_description" Caption="Product Name" VisibleIndex="3" Width="150px" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="4" Width="30px" />
                                        <dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" VisibleIndex="5" Width="50px" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" VisibleIndex="6" Width="100px" />
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-9 no-padding text-right" style="margin-top: 15px;">
                                        <button type="button" runat="server" id="Button1" onclick="submitSaleOrderDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button2" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_issue_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Edit Issue Detail
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
                                        <button type="button" runat="server" id="Button7" onclick="submitIssueDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_issue_mfg" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Product :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" readonly="true" id="txtMFGProductDescription" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewIssueMFG" ClientInstanceName="gridViewIssueMFG" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewIssueMFG_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewIssueMFG_HtmlDataCellPrepared"
                                    KeyFieldName="mfg_no" Width="100%">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="false">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="40px">
                                            <DataItemTemplate>
                                                <%-- <input type="checkbox" id="productId" name="quotationDetailId"
                                                    onchange="chkproduct(<%# Eval("id") %>)" />--%>
                                                <dx:ASPxCheckBox ID="chkProductMFG" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkProductMFG"
                                                    mfgNo='<%# Eval("mfg_no")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkProductMFGDetail(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="id" Visible="false" Caption="ID" VisibleIndex="1" Width="5px" />
                                        <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" VisibleIndex="2" Width="100px" />
                                        <dx:GridViewDataTextColumn Caption="Unit Warranty" FieldName="unit_warranty" VisibleIndex="3" Width="80px" Visible="false">
                                            <DataItemTemplate>
                                                <dx:ASPxTextBox runat="server" ID="txtUnitWarranty" CssClass="numberic form-control" ClientInstanceName="txtUnitWarranty"
                                                    Enabled='<%# Eval("is_selected")%>'
                                                    mfgNo='<%# Eval("mfg_no")%>' Text='<%# Eval("unit_warranty")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        changedUnitWarrantyProductMFGDetail(s, e); 
                                                                    }" />
                                                </dx:ASPxTextBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Air End Warranty" FieldName="air_end_warranty" VisibleIndex="4" Width="80px" Visible="false">
                                            <DataItemTemplate>
                                                <dx:ASPxTextBox runat="server" ID="txtAirWarranty" CssClass="numberic form-control" ClientInstanceName="txtAirWarranty"
                                                    Enabled='<%# Eval("is_selected")%>'
                                                    mfgNo='<%# Eval("mfg_no")%>' Text='<%# Eval("air_end_warranty")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        changedAirWarrantyProductMFGDetail(s, e); 
                                                                    }" />
                                                </dx:ASPxTextBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Service Fee" FieldName="service_fee" VisibleIndex="5" Width="80px" Visible="false">
                                            <DataItemTemplate>
                                                <dx:ASPxTextBox runat="server" ID="txtServiceFee" CssClass="numberic form-control" ClientInstanceName="txtServiceFee"
                                                    Enabled='<%# Eval("is_selected")%>'
                                                    mfgNo='<%# Eval("mfg_no")%>' Text='<%# Eval("service_fee")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        changedFeeProductMFGDetail(s, e); 
                                                                    }" />
                                                </dx:ASPxTextBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="pr_no" Caption="เลขที่ใบสั่งซื้อ" VisibleIndex="6" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="receive_no" Caption="เลขที่ใบแจ้งหนี้" VisibleIndex="7" Width="100px" />
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group" style="margin-top: 10px;">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-9 no-padding text-right">
                                        <button type="button" runat="server" id="Button3" onclick="submitMFGProduct()" class="btn-app btn-addItem">ยืนยัน</button>
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
            <asp:HiddenField runat="server" ID="hdQuotationType" />
            <asp:HiddenField runat="server" ID="hdQuotationNo" />
            <asp:HiddenField runat="server" ID="hdCustomerId" />
            <asp:HiddenField runat="server" ID="hdSelectedIssueDetailId" />
            <asp:HiddenField runat="server" ID="hdSelectedProductId" />
            <asp:HiddenField runat="server" ID="hdDocumentFlag" />
        </div>
    </div>
</asp:Content>