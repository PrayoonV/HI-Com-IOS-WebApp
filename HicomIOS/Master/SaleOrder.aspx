﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SaleOrder.aspx.cs" Inherits="HicomIOS.Master.SaleOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #txtQuatationNo {
            width: 248px;
            font-weight: bold;
        }

        .txt-no {
            background-color: #e4effa !important;
        }

        .btn-danger {
            height: 25px !important;
        }

        #checkInAdvance {
            display: none;
        }

        #checkDownPayment {
            display: none;
        }

        #checkFinalePayment {
            display: none;
        }

        #checkCredit {
            display: none;
        }

        #checkPeriod {
            display: none;
        }

        #cbbQuotation {
            padding: 0;
        }

        .disabled {
            pointer-events: none;
            cursor: default;
            opacity: 0.6;
        }

        #cboRefIssueNo {
            //display: none;
        }

        #div-content {
            height: 577px;
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
    <script type="text/javascript">

        $(document).ready(function () {

            cbbQuotation.Focus();

            $("#Splitter_0").parent().hide();

            $("#txtDateQuatationNo").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            $("#dtInvoiceDate").datepicker({
                dateFormat: 'dd/mm/yy'
            });
            $("#txtPODate").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            $("#dtDateShipping").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            //$("#dtDateBill").datepicker({
            //    dateFormat: 'dd/mm/yy'
            //});

            $('#modal_quotation').on('shown.bs.modal', function (e) {
                $('#txtSearchText').val("");
                $('#chkSelectAllQuotation').prop('checked', false);
            });

            $("#txtDateCheckInAdvance").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            $('#cbTypeSendTaxPost').change(function () {
                $("#txtTypeSendAttention").prop("disabled", !$(this).is(':checked'));
                if (!$(this).is(':checked')) {
                    $("#txtTypeSendAttention").val("");
                }
            });

            $('#cbTypeSendTaxOther').change(function () {
                $("#txtTypeSendOther").prop("disabled", !$(this).is(':checked'));
                if (!$(this).is(':checked')) {
                    $("#txtTypeSendOther").val("");
                }
            });

            $('input:radio').click(function () {
                $("#txtRemarkOther").prop("disabled", true);
                if ($(this).attr('id') == 'rdoRemarkOthers') {
                    $("#txtRemarkOther").prop("disabled", false);
                    if ($("#txtRemarkOther").val() == "") {
                        var rdoRemarkOthers = $('#formradio');
                        var rdoRemarkOthers_txt = '*กรุณากรอกหมายเหตุอื่น ๆ';
                        console.log(rdoRemarkOthers);
                        var error_msg_rdoRemarkOthers = '<span id="' + (rdoRemarkOthers.attr('id')) + '-error" class="help-block" style="margin-left:1%">' + rdoRemarkOthers_txt + '</span>';
                        console.log(error_msg_rdoRemarkOthers);
                        rdoRemarkOthers.parent().addClass('has-error');
                        if ($('#' + rdoRemarkOthers.attr('id') + '-error').hasClass('help-block') === false) {
                            rdoRemarkOthers.after(error_msg_rdoRemarkOthers);
                        }
                    }
                }
                else {
                    $("#txtRemarkOther").val("");
                }

            });

            $('#cboTermOfPayment').on('change', function () {
                var TermOfPayment = $(this).val();
                $("#checkInAdvance").hide();
                if (TermOfPayment == 2) {
                    $("#checkInAdvance").show();
                    $("#checkDownPayment").hide();
                    $("#checkFinalPayment").hide();
                    $("#checkCredit").hide();
                    $("#checkPeriod").hide();
                }
                if (TermOfPayment == 3) {
                    $("#checkInAdvance").hide();
                    $("#checkDownPayment").hide();
                    $("#checkFinalPayment").hide();
                    $("#checkCredit").show();
                    $("#checkPeriod").hide();
                }
                else if (TermOfPayment == 4) {
                    $("#checkDownPayment").show();
                    $("#checkFinalPayment").show();
                    $("#checkInAdvance").hide();
                    $("#checkCredit").hide();
                    $("#checkPeriod").hide();
                }
                /*else if (TermOfPayment == 5) {
                    $("#checkFinalPayment").show();
                    $("#checkInAdvance").hide();
                    $("#checkDownPayment").hide();
                    $("#checkCredit").hide();
                    $("#checkPeriod").hide();
                }*/
                else if (TermOfPayment == 6) {
                    $("#checkPeriod").show();
                    $("#checkFinalPayment").hide();
                    $("#checkInAdvance").hide();
                    $("#checkDownPayment").hide();
                    $("#checkCredit").hide();
                    $("#txtPeriodNo").focus();
                    $("#txtPeriodNo").val(0);
                    $("#txtCheckPeriodAmount").val(0);
                }
                else {
                    $("#checkFinalPayment").hide();
                    $("#checkInAdvance").hide();
                    $("#checkDownPayment").hide();
                    $("#checkCredit").hide();
                    $("#checkPeriod").hide();
                }
            });

            $('#txtSaleOrderNo').keyup(function () {
                if ($('#txtSaleOrderNo').val() != "") {
                    $('#txtSaleOrderNo').parent().removeClass("has-error");
                    $('#txtSaleOrderNo').next().remove('.help-block');
                }
            });

            $('#txtPONo').keyup(function () {
                if ($('#txtPONo').val() != "") {
                    $('#txtPONo').parent().removeClass("has-error");
                    $('#txtPONo').next().remove('.help-block');
                }
            });


            $('#cboTermOfPayment').change(function () {
                if ($('#cboTermOfPayment').val() != "") {
                    $('#cboTermOfPayment').parent().removeClass("has-error");
                    $('#cboTermOfPayment').next().remove('.help-block');
                }
            });

            $('#cbTypeSendTaxCustomer').change(function () {

                $('#formcheckbox').parent().removeClass("has-error");
                $('#formcheckbox').next().remove('.help-block');
            });

            $('#cbTypeSendTaxNormal').change(function () {

                $('#formcheckbox').parent().removeClass("has-error");
                $('#formcheckbox').next().remove('.help-block');

            });

            $('#txtTypeSendAttention').keyup(function () {

                $('#formcheckbox').parent().removeClass("has-error");
                $('#formcheckbox').next().remove('.help-block');
            });
            $('#txtTypeSendOther').keyup(function () {

                $('#formcheckbox').parent().removeClass("has-error");
                $('#formcheckbox').next().remove('.help-block');
            });

            $('#txtRemarkOther').keyup(function () {

                $('#formradio').parent().removeClass("has-error");
                $('#formradio').next().remove('.help-block');
            });

            //$('#tabSaleOrder').css('display', 'none');
            //$('#btnService').css('display', 'none');

            $('#cbRefIssueNo').change(function () {
                $("#cboRefIssueNo").prop("disabled", !$(this).is(':checked'));
                if (!$(this).is(':checked')) {
                    $("#cboRefIssueNo option").removeAttr("selected");
                    $('#cboRefIssueNo option[value=""]').attr("selected", true);
                }
            });
        });


        function save_sale_order() {

        }

        /* HEADER FUNCTION*/
        function changedQuotationSelect() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var data = cbbQuotationNo;
            var key = cbbQuotation.GetValue();
            console.log("key = " + key);
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/GetQuotationData",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response.d);

                    $('#txtCustomerAddress').val(response.d.customer_address);
                    $('#txtDateQuatationNo').val(response.d.display_quotation_date);
                    $('#lbCustomerCode').html(response.d.customer_code);
                    $('#txtCustomerName').val(response.d.customer_name);
                    $('#txtCustomerTel').val(response.d.customer_tel);
                    $('#txtCustomerFax').val(response.d.customer_fax);
                    $('#txtAttentionName').val(response.d.attention_name);
                    $('#txtSubject').val(response.d.quotation_subject);
                    $('#txtProject').val(response.d.project_name);
                    $('#detailQuotation').val(response.d.quotation_no);
                    $('#hdCustomerId').val(response.d.customer_id);
                    $('#hdQuotationType').val(response.d.quotation_type);
                    $('#hdQuotationItemDiscount').val(response.d.is_discount_by_item);

                    $('#txtTotal').val(number_format(response.d.total_amount, 2));
                    $('#txtSumDiscount1').val(number_format(response.d.discount1_total, 2));
                    $('#txtSumDiscount2').val(number_format(response.d.discount2_total, 2));
                    $('#txtGrandTotal').val(number_format(response.d.grand_total - response.d.vat_total, 2));

                    //$('#txtTotal').val(response.d.total_amount);

                    if (response.d.is_product_description) {
                        $('#rdoDescription').prop("checked", false);
                        $('#rdoQuotationLine').prop("checked", true);
                    }
                    else {
                        $('#rdoDescription').prop("checked", true);
                        $('#rdoQuotationLine').prop("checked", false);
                    }

                    if (response.d.is_discount_by_item) {
                        $('#cbDiscountByItem').prop("checked", true);
                        $('#cbbDiscountByItem').prop("disabled", false);
                        $('#cbbDiscountByItem').val(response.d.discount_item_type);

                    }
                    else {
                        $('#cbDiscountByItem').prop("checked", false);
                        $('#cbbDiscountByItem').prop("disabled", true);
                    }

                    if (response.d.discount1_type == "A" || response.d.discount1_type == "P") {
                        $('#cbbDiscountBottomBill1').val(response.d.discount1_type);
                        $('#cbDiscountBottomBill1').prop("checked", true);

                    }
                    else {
                        $('#cbbDiscountBottomBill1').val("");
                        $('#cbDiscountBottomBill1').prop("checked", false);

                    }
                    if (response.d.discount2_type == "A" || response.d.discount2_type == "P") {
                        $('#cbbDiscountBottomBill2').val(response.d.discount2_type);
                        $('#cbDiscountBottomBill2').prop("checked", true);
                    }
                    else {
                        $('#cbbDiscountBottomBill2').val("");
                        $('#cbDiscountBottomBill2').prop("checked", false);
                    }
                    if (response.d.discount1_type == "P") {
                        $('#txtDiscountBottomBill1').val(response.d.discount1_percentage);
                        $('#txtDiscountBottomBill1').prop("disabled", false);
                        $('#cbbDiscountBottomBill1').prop("disabled", false);
                    }
                    else if (response.d.discount1_type == "A") {
                        $('#txtDiscountBottomBill1').val(response.d.discount1_amount);
                        $('#txtDiscountBottomBill1').prop("disabled", false);
                        $('#cbbDiscountBottomBill1').prop("disabled", false);
                    }
                    else {
                        $('#txtDiscountBottomBill1').val(0);
                        $('#txtDiscountBottomBill1').prop("disabled", true);
                        $('#cbbDiscountBottomBill1').prop("disabled", true);
                    }


                    if (response.d.discount2_type == "P") {
                        $('#txtDiscountBottomBill2').val(response.d.discount2_percentage);
                        $('#txtDiscountBottomBill2').prop("disabled", false);
                        $('#cbbDiscountBottomBill2').prop("disabled", false);
                    }
                    else if (response.d.discount2_type == "A") {
                        $('#txtDiscountBottomBill2').val(response.d.discount2_amount);
                        $('#txtDiscountBottomBill2').prop("disabled", false);
                        $('#cbbDiscountBottomBill2').prop("disabled", false);
                    }
                    else {
                        $('#txtDiscountBottomBill2').val(0);
                        $('#txtDiscountBottomBill2').prop("disabled", true);
                        $('#cbbDiscountBottomBill2').prop("disabled", true);
                    }

                    if (response.d.is_vat == true)
                        $('#cbShowVat').prop("checked", true);
                    else
                        $('#cbShowVat').prop("checked", false);

                    gridViewDetailSaleOrder.PerformCallback();
                    gridViewDetailList.PerformCallback();
                }
                ,
                failure: function (response) {
                    console.log(response);

                }
            });
            $.LoadingOverlay("hide");
        }

        function popupQuotationDetail() {

            var key = cbbQuotation.GetText();
            if (key == "" || key == null || key == undefined) {
                swal("กรุณาเลือกเอกสาร Quotation");
                return;
            }
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/GetQuotationDetailData",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#modal_quotation').modal('show');
                    setTimeout(function () {
                        gridViewDetailList.PerformCallback();
                    }, 0.8);
                }
            });
        }

        function onCheckedDiscountByItem() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            if ($('#cbDiscountByItem').is(':checked')) {
                //$("#txtDiscountByItem").prop("disabled", false);
                $("#cbbDiscountByItem").prop("disabled", false);
            }
            else {
                //$("#txtDiscountByItem").val("");
                //$("#txtDiscountByItem").prop("disabled", true);
                $("#cbbDiscountByItem").prop("disabled", true);
            }
            gridViewDetailSaleOrder.PerformCallback();
            calDiscount();

            $.LoadingOverlay("hide");
        }

        function onCheckedDiscountBottomBill1() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            if ($('#cbDiscountBottomBill1').is(':checked')) {
                $("#txtDiscountBottomBill1").prop("disabled", false);
                $("#cbbDiscountBottomBill1").prop("disabled", false);
            }
            else {
                $("#txtDiscountBottomBill1").val("");
                $("#txtDiscountBottomBill1").prop("disabled", true);
                $("#cbbDiscountBottomBill1").prop("disabled", true);

            }
            gridViewDetailSaleOrder.PerformCallback();
            calDiscount();

            $.LoadingOverlay("hide");
        }

        function onCheckedDiscountBottomBill2() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            if ($('#cbDiscountBottomBill2').is(':checked')) {
                $("#txtDiscountBottomBill2").prop("disabled", false);
                $("#cbbDiscountBottomBill2").prop("disabled", false);
            }
            else {
                $("#txtDiscountBottomBill2").val("");
                $("#txtDiscountBottomBill2").prop("disabled", true);
                $("#cbbDiscountBottomBill2").prop("disabled", true);
            }
            gridViewDetailSaleOrder.PerformCallback();
            calDiscount();

            $.LoadingOverlay("hide");
        }
        /* END HEADER FUNCTION*/

        /* QUOTATION DETAIL FUNCTION*/
        function chkproduct(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("QuotationDetailId");
            $.ajax({
                type: "POST",
                url: "Saleorder.aspx/AddSaleOrderDetail",
                data: '{id:"' + key + '" , isSelected : ' + value + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Add Complete");
                }
            });
        }

        function submitQuotationDetail() {
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/SubmitQuotationDetail",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    setTimeout(function () {
                        $("#txtTotal").val(number_format(response.d.total_amount, 2));
                        $("#txtSumDiscount1").val(number_format(response.d.discount1_total, 2));
                        $("#txtSumDiscount2").val(number_format(response.d.discount2_total, 2));
                        $("#txtGrandTotal").val(number_format(response.d.grand_total - response.d.vat_total, 2));
                        gridViewDetailList.PerformCallback();
                        gridViewDetailSaleOrder.PerformCallback();
                        calDiscount();
                    }, 100);

                    $('#modal_quotation').modal('hide');

                }

            });

        }
        /* END QUOTATION DETAIL FUNCTION*/

        /*SALE ORDER DETAIL FUNCTION*/
        function editSaleorderDetail(element, e) {
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedSaleOrderDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "Saleorder.aspx/EditSaleOrderDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var data = response.d;
                    //var discount_item = $('#cbbDiscountByItem').val()
                    if (!$('#cbDiscountByItem').is(':checked')) {
                        $('#txtDiscountAmountItem').prop("readonly", true);
                        $('#txtDiscountPercentItem').prop("readonly", true);
                        $('#txtDiscountAmountItem').val(0);
                        $('#txtDiscountPercentItem').val(0);
                        $("#dvDiscountDetailBaht").hide();
                        $("#dvDiscountDetailPercent").hide();
                    }
                    else {
                        if ($('#cbbDiscountByItem').val() == "P") {
                            $('#txtDiscountAmountItem').prop("readonly", true);
                            $('#txtDiscountPercentItem').prop("readonly", false);
                            $('#txtDiscountAmountItem').val(0);
                            $("#dvDiscountDetailBaht").hide();
                            $("#dvDiscountDetailPercent").show();
                        }
                        else if ($('#cbbDiscountByItem').val() == "A") {
                            $('#txtDiscountAmountItem').prop("readonly", false);
                            $('#txtDiscountPercentItem').prop("readonly", true);
                            $('#txtDiscountPercentItem').val(0);
                            $("#dvDiscountDetailBaht").show();
                            $("#dvDiscountDetailPercent").hide();
                        }
                    }

                    $('#txtProductDescription').val(data.saleorder_description);
                    $('#txtProductQty').val(data.qty);
                    $('#txtProductUnitPrice').val(data.unit_price);
                    $('#txtDiscountAmountItem').val(data.discount_amount);
                    $('#txtDiscountPercentItem').val(data.discount_percentage);
                    //$('#hdMinUnitPrice').val(data.min_unit_price);

                    $('#modal_sale_order_detail .modal-header').html('Edit Sale Order Detail : ' + element.title);
                    $('#modal_sale_order_detail').modal('show');
                }
            });
        }

        function deleteSaleorderDetail(e) {
            var id = e;
            $('#hdSelectedSaleOrderDetailId').val(id);
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
                            url: "Saleorder.aspx/DeleteSaleOrderDetail",
                            data: '{id:"' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#hdSelectedSaleOrderDetailId').val(0);
                                        calDiscount();
                                        gridViewDetailList.PerformCallback();
                                        gridViewDetailSaleOrder.PerformCallback();

                                    });
                            }
                        });
                    }
                });
        }


        function submitSaleOrderDetail() {
            var key = $('#hdSelectedSaleOrderDetailId').val();
            var qty = $('#txtProductQty').val();
            var unit_price = $('#txtProductUnitPrice').val();
            var amount = $('#txtDiscountAmountItem').val();
            var percent = $('#txtDiscountPercentItem').val();
            var discountType = $('#cbbDiscountByItem').val();
            //var minUnitPrice = $('#hdMinUnitPrice').val();
            //if (parseInt(unit_price) < parseInt(minUnitPrice)) {

            //    return;
            //}
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/SubmitSaleOrderDetail",
                data: '{id: "' + key + '" , qty: "' + qty + '",unit_price : "' + unit_price + '" , amount :"' + amount + '" , percent : "' + percent + '" , discountType : "' + discountType + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $("#txtTotal").val(number_format(response.d.total_amount, 2));
                    alert(number_format(response.d.total_amount, 2));
                    $("#txtSumDiscount1").val(number_format(response.d.discount1_total, 2));
                    $("#txtSumDiscount2").val(number_format(response.d.discount2_total, 2));
                    $("#txtGrandTotal").val(number_format(response.d.grand_total - response.d.vat_total, 2));
                    $('#txtProductDescription').val("");
                    $('#txtProductQty').val(0);
                    $('#txtProductUnitPrice').val(0);
                    $('#hdSelectedSaleOrderDetailId').val(0);
                    $('#txtDiscountAmountItem').val(0);
                    $('#txtDiscountPercentItem').val(0);
                    $('#hdMinUnitPrice').val(0);

                    $('#modal_sale_order_detail').modal('hide');
                    calDiscount();
                    gridViewDetailSaleOrder.PerformCallback()
                },
                failure: function (response) {

                }
            });
        }

        function calDiscount() {
            var key = "";
            if ($('#cbDiscountByItem').is(':checked')) {
                key = $("#cbbDiscountByItem").val();
            }
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/CalcurateDiscount",
                data: '{discount_type : "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onCalcurateSuccess,
                failure: function (response) {

                }
            });
        }

        function onCalcurateSuccess(response) {
            setTimeout(function () {
                
                var data = response.d;
                var total = data.total;
                
                var discountDetail = data.discount_total;
                var totalWithDiscountDetail = total - discountDetail;
                
                var sumBottomDiscount1 = 0;
                var sumBottomDiscount2 = 0;
                var grandTotal = 0;
                //$("#txtTotal").val(accounting.formatMoney(totalWithDiscountDetail));
                $("#txtTotal").val(number_format(totalWithDiscountDetail, 2));
                $("#txtSumDiscount1").val(0);
                $("#txtSumDiscount2").val(0);
                var discountButoomBill1 = parseInt($("#txtDiscountBottomBill1").val() == "" ? 0 : $("#txtDiscountBottomBill1").val());
                var discountButoomBill2 = parseInt($("#txtDiscountBottomBill2").val() == "" ? 0 : $("#txtDiscountBottomBill2").val())
                if ($("#cbbDiscountBottomBill1").val() == "P") {
                    if (parseInt($("#txtDiscountBottomBill1").val()) > 100) {
                        swal("เปอร์เซ็นส่วนลดท้ายบิล 1 ไม่สามารถเกิน 100 %");
                        $("#txtDiscountBottomBill1").val(0);
                        grandTotal = totalWithDiscountDetail;
                        return;
                    }

                    sumBottomDiscount1 = (totalWithDiscountDetail * discountButoomBill1) / 100;
                    $("#txtSumDiscount1").val(accounting.formatMoney(sumBottomDiscount1));
                    //grandTotal = totalWithDiscountDetail - sumBottomDiscount1;
                }
                else if ($("#cbbDiscountBottomBill1").val() == "A") {

                    if (parseInt($("#txtDiscountBottomBill1").val()) > totalWithDiscountDetail) {
                        swal("ส่วนลดท้ายบิล 1 เกินราคาที่กำหนด");

                        $("#txtDiscountBottomBill1").val(0);
                        grandTotal = totalWithDiscountDetail;
                        return;
                    }

                    sumBottomDiscount1 = discountButoomBill1;
                    $("#txtSumDiscount1").val(accounting.formatMoney(sumBottomDiscount1));
                    //grandTotal = totalWithDiscountDetail - sumBottomDiscount1;
                }
                if ($("#cbbDiscountBottomBill2").val() == "P") {
                    if (parseInt($("#txtDiscountBottomBill2").val()) > 100) {
                        swal("เปอร์เซ็นส่วนลดท้ายบิล 2 ไม่สามารถเกิน 100 %");

                        $("#txtDiscountBottomBill2").val(0);
                        grandTotal = (totalWithDiscountDetail - sumBottomDiscount1);
                        return;
                    }
                    console.log(totalWithDiscountDetail);
                    console.log(discountButoomBill1);
                    sumBottomDiscount2 = ((totalWithDiscountDetail - sumBottomDiscount1) * discountButoomBill2) / 100;
                    $("#txtSumDiscount2").val(accounting.formatMoney(sumBottomDiscount2));
                    //grandTotal = totalWithDiscountDetail - sumBottomDiscount2;
                }
                else if ($("#cbbDiscountBottomBill2").val() == "A") {
                    if (parseInt($("#txtDiscountBottomBill2").val()) > totalWithDiscountDetail) {

                        swal("ส่วนลดท้ายบิล 2 เกินราคาที่กำหนด");
                        $("#txtDiscountBottomBill2").val(0);
                        grandTotal = (totalWithDiscountDetail - sumBottomDiscount1);
                        return;
                    }

                    sumBottomDiscount2 = discountButoomBill2
                    $("#txtSumDiscount2").val(accounting.formatMoney(sumBottomDiscount2));
                    //grandTotal = totalWithDiscountDetail - sumBottomDiscount2;
                }

                grandTotal = (totalWithDiscountDetail - sumBottomDiscount1) - sumBottomDiscount2;
                if (parseInt(grandTotal) < 0) {
                    swal("ส่วนลดเกินราคาที่กำหนด");
                    $("#txtDiscountBottomBill2").val(1);
                    $("#txtDiscountBottomBill2").val(0);
                    grandTotal = (totalWithDiscountDetail);
                    return;
                }
                $('#txtGrandTotal').val(accounting.formatMoney(grandTotal));
            }, 0.8);
        }
        /*END SALE ORDER DETAIL FUNCTION*/

        /* FUNCION NOTICE */
        function newNotice() {
            $('#hdSelectedNoticeId').val(0);
            $("#txtSubjectNotice").val("");
            $('#txtDescriptionNotice').val("");
            $("#txtDateTimeNotice").val("");

            $('#popupAddNoticeSaleOrder').modal('show');
        }

        function addNotice() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var subjectText = $('#txtSubjectNotice').val();
            var descText = $('#txtDescriptionNotice').val();
            var dateNotification = $("#txtDateTimeNotice").val();

            if (dateNotification == "") {
                swal("กรุณาเลือกวันที่แจ้งเตือน");
                $.LoadingOverlay("hide");
                return;
            }
            var date = $('#txtDateTimeNotice').handleDtpicker('getDate');
            var aa = date.toISOString();
            //var index = gridViewNotice.GetFocusedRowIndex();
            //var key = gridViewNotice.keys[index];
            //key = index == -1 ? 0 : key;
            var key = $('#hdSelectedNoticeId').val();
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/AddNotice",
                data: '{id: "' + key + '" ,subject: "' + subjectText + '", description: "' + descText + '" , date: "' + aa + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onAddNoticeSuccess,
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");
        }

        function editNotice(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var key = e;
            $('#hdSelectedNoticeId').val(key);
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/ShowEditNotice",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: showPopupNotice,
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function deleteNotice(e) {


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
                            url: "SaleOrder.aspx/DeleteNotice",
                            data: '{id:"' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridViewNotice.PerformCallback();
                                        $("#txtSubjectNotice").val("");
                                        $('#txtDescriptionNotice').val("");
                                        $("#txtDateTimeNotice").val("");
                                    });
                            }
                        });
                    }
                });

        }

        function showPopupNotice(response) {
            console.log(response.d);
            var date = new Date(Date.parse(response.d.notice_date));
            $('#txtSubjectNotice').val(response.d.subject);
            $('#txtDescriptionNotice').val(response.d.description);
            $('#txtDateTimeNotice').val(response.d.display_notice_date);

            $('#popupAddNoticeSaleOrder').modal('show');
        }

        function onAddNoticeSuccess() {

            gridViewNotice.PerformCallback();
            $("#txtSubjectNotice").val("");
            $('#txtDescriptionNotice').val("");
            $("#txtDateTimeNotice").val("");
            $('#popupAddNoticeSaleOrder').modal('hide');
        }
        /* END FUNCION NOTICE */
        /* PO DETAIL FUNCTION */
        function newPO() {
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/CheckDataPayment",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "PENDING") {
                        $('#hdSelectedPOId').val(0);
                        $("#txtPONo").val("");
                        $("#txtPODate").val("");
                        $("#txtDownPayment").val(0);
                        $("#txtFinalPayment").val(0);
                        $("#txtInvoiceNo").val("");
                        $("#dtInvoiceDate").val("");
                        $("#txtDateCheckInAdvance").val("");
                        $("#checkFinalPayment").hide();
                        $("#checkInAdvance").hide();
                        $("#checkDownPayment").hide();
                        $("#dtDateShipping").val("");
                        //$("#dtDateBill").val("");
                        $("#checkCredit").hide();
                        $("#txtCreditDay").val(0);
                        $("#txtTempNoDelivery").val("");
                        $("#txtPeriodNo").val(0);
                        $("#txtCheckPeriodAmount").val(0);
                        $("#txtpercent").val(0);

                        $('#cboTermOfPayment').val("");

                        $('#popupAddPOSaleOrder').modal('show');
                    }
                    else {
                        swal("เอกสารใบนี้ระบุขั้นตอนการชำระเงินแล้ว");
                    }

                },
                failure: function (response) {

                }
            });


        }

        function addPO() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var po_no = $('#txtPONo').val();
            var po_date = ($("#txtPODate").val());
            var payment_type = $('#cboTermOfPayment').val();
            var payment_val = "";
            var diff_deposit = "";
            if (payment_type == "") {

                swal("กรุณาเลือกเงือนไขการชำระเงิน");
                $.LoadingOverlay("hide");
                return

            }
            if (payment_type == 4) {
                payment_val = $('#txtDownPayment').val();
                diff_deposit = $('#txtFinalPayment').val();
            }
            else if (payment_type == 5) {
                payment_val = $('#txtFinalPayment').val();
            }
            else if (payment_type == 6) {
                payment_val = $('#txtpercent').val();
            }
            else {
                payment_val = "";
            }
            var invoice_no = $('#txtInvoiceNo').val();
            var invoice_date = $('#dtInvoiceDate').val();
            //var bill_date = $('#dtDateBill').val();
            var delivery_date = $('#dtDateShipping').val();
            var tempNoDelivery = $('#txtTempNoDelivery').val();
            var credit_day = $('#txtCreditDay').val();
            var period_no = $("#txtPeriodNo").val();
            var period_amount = $("#txtCheckPeriodAmount").val();

            if (period_amount != undefined) {
                var res = period_amount.split(".");
                if (res != undefined && res.length > 1)
                    if (res[1].length > 2) {
                        swal("กรุณากรอกค่างวดสูงสุดไม่เกิน 2 ตำแหน่ง");
                        $.LoadingOverlay("hide");
                        return
                    }

            }

            var key = $('#hdSelectedPOId').val();
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/AddPO",
                data: '{id: "' + key + '" ,po_no: "' + po_no + '", po_date: "' + po_date + '"' +
                    ', payment_type : "' + payment_type + '" , payment_val : "' + payment_val + '" , invoice_no : "' + invoice_no + '"' +
                    ', invoice_date : "' + invoice_date + '" , temp_no_delivery : "' + tempNoDelivery + '" , delivery_date : "' + delivery_date + '" , credit_day : "' + credit_day + '", period_no : "' + period_no + '", period_amount : "' + period_amount + '", diff_deposit : "' + diff_deposit + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#popupAddPOSaleOrder').modal('hide');
                    gridViewPO.PerformCallback();
                },
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");
        }

        function editPO(key) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            $('#hdSelectedPOId').val(key);
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/ShowEditPO",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var data = response.d;
                    console.log(data);
                    $('#txtPONo').val(data.ref_po_no);
                    $("#txtPODate").val(data.display_po_date);
                    var payment_type = data.payment_type;
                    $('#cboTermOfPayment').val(payment_type);

                    if (payment_type == 2) {
                        $('#txtDateCheckInAdvance').val(data.display_cheque_date);
                        $("#checkDownPayment").hide();
                        $("#checkInAdvance").hide();
                        $("#checkFinalPayment").hide();
                        $('#txtFinalPayment').val(0);
                        $('#txtDownPayment').val(0)
                        $("#checkCredit").hide();
                        $('#txtCreditDay').val(0);
                    }
                    else if (payment_type == 3) {
                        $("#checkDownPayment").hide();
                        $("#checkInAdvance").hide();
                        $("#checkFinalPayment").hide();
                        $("#checkCredit").show();
                        $('#txtFinalPayment').val(0);
                        $('#txtDownPayment').val(0)
                        $('#txtCreditDay').val(data.credit_day);
                    }


                    else if (payment_type == 4) {
                        $('#txtDownPayment').val(data.percent_price);
                        $("#checkDownPayment").show();
                        $("#checkFinalPayment").show();
                        $('#txtFinalPayment').val(data.diff_deposit);
                        $("#checkInAdvance").hide();
                        $('#txtDateCheckInAdvance').val("");
                        $("#checkCredit").hide();
                        $('#txtCreditDay').val(0);
                    }
                    else if (payment_type == 5) {
                        $('#txtFinalPayment').val(data.percent_price);
                        $("#checkDownPayment").hide();
                        $("#checkInAdvance").hide();
                        $("#checkFinalPayment").show();
                        $('#txtDateCheckInAdvance').val("");
                        $('#txtDownPayment').val(0);
                        $("#checkCredit").hide();
                        $('#txtCreditDay').val(0);
                    }
                    else if (payment_type == 6) {
                        $("#checkPeriod").show();
                        $("#checkFinalPayment").hide();
                        $("#checkInAdvance").hide();
                        $("#checkDownPayment").hide();
                        $("#checkCredit").hide();
                        $("#txtPeriodNo").focus();
                        $("#txtPeriodNo").val(data.period_no);
                        $("#txtCheckPeriodAmount").val(data.period_amount);
                    }
                    else {
                        $('#txtDateCheckInAdvance').val("");
                        $('#txtFinalPayment').val(0);
                        $('#txtDownPayment').val(0)
                        $("#checkFinalPayment").hide();
                        $("#checkCredit").hide();
                        $('#txtCreditDay').val(0);
                    }
                    $('#txtTempNoDelivery').val(data.temp_delivery_no);
                    $('#txtInvoiceNo').val(data.invoice_no);
                    $('#dtInvoiceDate').val(data.display_invoice_date);
                    $('#dtDateBill').val(data.display_bill_date);
                    $('#dtDateShipping').val(data.display_delivery_date);
                    $('#popupAddPOSaleOrder').modal('show');
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function deletePO(e) {
            var id = e;
            $('#hdSelectedPOId').val(id);

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
                            url: "SaleOrder.aspx/DeletePODetail",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridViewPO.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                swal("การลบข้อมูลผิดพลาด!", {
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
        }

        /* PO DETAIL FUNCTION */
        /* TAX DETAIL FUNCTION*/
        function newTax() {
            $('#hdSelectedTaxId').val(0);
            $("#txtTaxNo").val("");
            $('#txtTaxDescription').val("");
            $("#txtTaxDate").val("");

            $('#popupAddTaxSaleOrder').modal('show');
        }
        function searchSeletedGrid(e) {
            if (e == 13) {


                var txtSearch = $("#txtSearchText").val();

                gridViewDetailList.PerformCallback("Search|" + txtSearch.toString());


                $('#modal_quotation').modal('show');

            }
        }
        function addTax() {

        }
        function editTax(e) {

        }
        function deleteTax(e) {

        }
        /*END TAX DETAIL FUNCTION*/
        function confirmSave() {
            if (cbbQuotation.GetValue() == null) {
                swal("กรุณาเลือกเอกสาร Quotation");
                return;
            }
            else if ($('#formcheckbox input[type="checkbox"]:checked').length <= 0) {
                console.log($('#formcheckbox input[type="checkbox"]:checked').length);
                var cbTypeSendTax = $('#formcheckbox');
                var cbTypeSendTax_txt = '*กรุณาเลือกวิธีจัดส่งเอกสารใบกำกับภาษี';
                var error_msg = '<span id="' + (cbTypeSendTax.attr('id')) + '-error" class="help-block" style="margin-left:1%">' + cbTypeSendTax_txt + '</span>';
                cbTypeSendTax.parent().addClass('has-error');
                if ($('#' + cbTypeSendTax.attr('id') + '-error').hasClass('help-block') === false) {
                    cbTypeSendTax.after(error_msg);
                }
                return false;
            }
            var type = $('#hdQuotationType').val();
            var sale_order_no = $('#txtSaleOrder').val();
            var status = $('#hdSaleOrderStatus').val();
            var issueNo = $('#cboRefIssueNo').val();

            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/ValidateData",
                data: '{type : "' + type + '", status : "' + status + '", issue_no : "' + issueNo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        console.log("testconfirm");
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

        function saveDraft() {
            $('#btnSaveDraft').click();
            //var type = $('#hdQuotationType').val();
            //console.log("txtGrandTotal==" + $('#txtGrandTotal').val());
            //$.ajax({
            //    type: "POST",
            //    url: "SaleOrder.aspx/ValidateData",
            //    data: '{type : "' + type + '"}',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (response) {
            //        if (response.d == "") {
            //            $('#btnSaveDraft').click();
            //        }
            //        else {
            //            swal(response.d);
            //        }
            //    },
            //    failure: function (response) {

            //    }
            //});
        }

        function selectAllQuotation() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkSelectAllQuotation').is(':checked');
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/SelectAllQuotation",
                data: '{selected : ' + selected + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewDetailList.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {

                }
            });
        }

        function callReport() {
            var isDiscount = false;
            if ($("#hdQuotationItemDiscount").val() == 'true') {
                isDiscount = true;
            }
            /*if ($("#hdQuotationType").val() == "S" || $("#hdQuotationType").val() == "A" || $("#hdQuotationType").val() == "M" || $("#hdQuotationType").val() == "C") {
                window.open("../Report/DocumentViewer.aspx?ReportArgs=Sale_Order_Spare_Part|" + $("#txtSaleOrder").val(), "_blank");
            } else */if (!isDiscount) {
                window.open("../Report/DocumentViewer.aspx?ReportArgs=Sale_Order_Product|" + $("#txtSaleOrder").val(), "_blank");
            } else if (isDiscount) {
                window.open("../Report/DocumentViewer.aspx?ReportArgs=Sale_Order_Product_Discount|" + $("#txtSaleOrder").val(), "_blank");
            }
        }
        function backPage() {
            window.location.href = "../Master/SaleOrderList.aspx";
        }
        function callService() {
            Id = $('#hdCustomerId').val();
            $.ajax({
                type: "POST",
                url: "SaleOrder.aspx/CheckCustomerMFG",
                data: '{id : ' + Id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d != "") {
                        window.location.href = "AnnualService.aspx?dataId=" + Id;
                    } else {
                        swal("ไม่พบข้อมูล MFG", {
                            icon: "error"
                        });
                    }
                }
            });

        }
        function confirmCancel() {
            var saleOrderNo = $("#txtSaleOrder").val();
            swal({
                title: "คุณต้องการยกเลิกใบสั่งขายใช่หรือไม่ ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: "Saleorder.aspx/CancelSaleOrderDetail",
                            data: '{saleOrderNo:"' + saleOrderNo + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ยกเลิกใบสั่งขายสำเร็จ!", {
                                    icon: "success"

                                })
                                    .then((value) => {
                                        window.location.href = "SaleorderList.aspx";


                                    });
                            }
                        });
                    }
                });
        }

        function addNewItem() {
            window.location.href = "SaleOrder.aspx";
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
            <button type="button" runat="server" id="btnCancel" onclick="confirmCancel()" class="btn-danger">
                <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Cancel
            </button>
            <button type="button" runat="server" id="btnReportClient" onclick="callReport()" class="btn-addItem">
                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Report
            </button>
            <button type="button" runat="server" id="btnService" onclick="callService()" class="btn-into">
                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Service
            </button>
            <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List Sale Order
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnSaveDraft" UseSubmitBehavior="false" Style="visibility: hidden" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnConfirm" UseSubmitBehavior="false" Style="visibility: hidden" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
        </div>
        <div class="row">
            <fieldset>
                <legend>Header</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">เลขที่ใบเสนอราคา:</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbQuotation" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbQuotation" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ClientSideEvents-ValueChanged="changedQuotationSelect" ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า:</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="txtCustomerName" runat="server" value="" readonly="" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ผู้ติดต่อ:</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="txtAttentionName" runat="server" value="" readonly="" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Subject:</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="txtSubject" runat="server" value="" readonly="" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Project:</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="txtProject" runat="server" value="" readonly="" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">หมายเหตุ:</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control" id="txtRemark" runat="server" />
                            </div>
                        </div>

                        <div class="row form-group" style="display: none">
                            <label class="col-xs-3 text-right label-rigth">Description Type :</label>
                            <div class="col-xs-9 form-inline">
                                <input type="radio" runat="server" value="1" id="rdoDescription" disabled="disabled" name="productDesc" />&nbsp;แสดงรายละเอียดปกติ
                                    <input type="radio" runat="server" value="0" id="rdoQuotationLine" disabled="disabled" name="productDesc" />&nbsp;แสดงรายละเอียดเต็ม
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4" style="padding-left: 2%;">
                        <div class="row form-group" hidden="hidden">
                            <div class="col-xs-3 form-inline">
                                <label>วันที่ใบเสนอราคา:</label>
                            </div>
                            <div class="col-xs-9 form-inline no-padding">
                                <input type="text" class="form-control" id="txtDateQuatationNo" disabled="disabled" readonly="" runat="server" style="width: 85px;" />
                            </div>
                        </div>

                        <div class="row form-group">
                            <div class="col-xs-5 label-rigth">
                                <strong>หมายเหตุจัดส่ง</strong>
                            </div>
                        </div>
                        <div class="col-xs-12 text-left no-padding">
                            <div class="form-inline shipping-step">
                                <label class="radio-inline">
                                    <input type="radio" runat="server" value="1" id="rdoRemarkBillDelivery" name="remark">&nbsp;เปิดบิลพร้อมส่งของ
                                </label>
                            </div>
                            <div class="form-inline shipping-step">
                                <label class="radio-inline">
                                    <input type="radio" runat="server" value="2" id="rdoRemarkSubmitted" name="remark">&nbsp;ส่งของแล้ว
                                </label>
                            </div>
                            <div class="form-inline shipping-step">
                                <label class="radio-inline">
                                    <input type="radio" runat="server" value="3" id="rdoRemarkOthers" name="remark">อื่นๆ 
                                </label>
                                <input type="text" class="form-control" id="txtRemarkOther" disabled="disabled" runat="server" />
                                &nbsp;
                            </div>
                        </div>

                        <div class="row form-group">
                            <div class="col-xs-12 label-rigth">
                                <strong>วิธีจัดส่งเอกสารใบกำกับภาษี(เลือกได้มากกว่า 1 ข้อ)</strong>
                            </div>
                        </div>

                        <div class="col-xs-12 no-padding" id="formcheckbox">
                            <div class="row" style="padding: 2px 0;">
                                <div class="form-inline shipping-step">
                                    <label class="checkbox">
                                        <input type="checkbox" name="cbTypeSendTax" id="cbTypeSendTaxNormal" value="1" runat="server">
                                        ปกติ
                                    </label>
                                </div>
                                <div class="form-inline shipping-step">
                                    <label class="checkbox">
                                        <input type="checkbox" name="cbTypeSendTax" id="cbTypeSendTaxCustomer" value="2" runat="server">
                                        ลูกค้ามารับ
                                    </label>
                                </div>
                                <div class="form-inline shipping-step">
                                    <label class="checkbox">
                                        <input type="checkbox" name="cbTypeSendTax" id="cbTypeSendTaxPost" value="3" runat="server">
                                        ส่งไปรษณีย์ ATTN
                                    </label>
                                    &nbsp;
                                    <input type="text" class="form-control" id="txtTypeSendAttention" disabled="disabled" runat="server" />
                                </div>
                            </div>

                            <div class="row" style="padding: 2px 0;">
                                <div class="form-inline shipping-step">
                                    <label class="checkbox">
                                        <input type="checkbox" name="cbTypeSendTax" id="cbTypeSendTaxOther" value="4" runat="server">
                                        อื่น ๆ
                                    </label>
                                    &nbsp;
                                    <input type="text" class="form-control" id="txtTypeSendOther" disabled="" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <div class="col-xs-3  label-rigth text-right form-inline">
                                <label>เลขที่เอกสาร :</label>
                            </div>
                            <div class="col-xs-9 form-inline no-padding">
                                <input type="text" class="form-control txt-no" id="txtSaleOrder" disabled="disabled" readonly="" runat="server" />
                            </div>
                        </div>
                        <div class="row form-group" id="refIssueNo" runat="server" style="display: none;">
                            <div class="col-xs-4  label-rigth text-right form-inline">
                                <label class="checkbox">
                                    <input type="checkbox" name="cbRefIssueNo" id="cbRefIssueNo" value="1" runat="server">
                                    Ref. Issue No.
                                </label>
                            </div>
                            <div class="col-xs-6 form-inline no-padding">
                                <select class="form-control" id="cboRefIssueNo" runat="server" disabled="disabled">
                                    <option value="">--โปรดเลือก-</option>
                                </select>
                            </div>
                        </div>
                        <div class="hidden">
                            <input type="checkbox" runat="server" id="cbDiscountByItem" enableviewstate="false" onclick="onCheckedDiscountByItem()" value="" />
                            <select class="form-control" runat="server" id="cbbDiscountByItem" onchange="onCheckedDiscountByItem()">
                                <option value="">-</option>
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                            <input type="checkbox" runat="server" id="Checkbox1" disabled="disabled" readonly="" enableviewstate="false" onclick="onCheckedDiscountBottomBill1()" value="" />
                            <input type="checkbox" runat="server" id="cbDiscountBottomBill1" enableviewstate="false" onclick="onCheckedDiscountBottomBill1()" value="" />
                            <input type="text" class="form-control numberic" id="txtDiscountBottomBill1" enableviewstate="false" onchange="onCheckedDiscountBottomBill1()" runat="server" />
                            <select class="form-control" runat="server" id="cbbDiscountBottomBill1" onchange="onCheckedDiscountBottomBill1()">
                                <option value="">-</option>
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                            <input type="checkbox" runat="server" id="cbDiscountBottomBill2" enableviewstate="false" onclick="onCheckedDiscountBottomBill2()" value="" />
                            <input type="text" class="form-control numberic" id="txtDiscountBottomBill2" enableviewstate="false" onchange="onCheckedDiscountBottomBill2()" runat="server" />
                            <select class="form-control" runat="server" id="cbbDiscountBottomBill2" onchange="onCheckedDiscountBottomBill2()" style="width: 80px;">
                                <option value="">-</option>
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                            <input type="checkbox" runat="server" id="cbShowVat" value="" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="row form-group">
                    <div class="col-xs-5"></div>
                    <div class="col-xs-7">
                        <div class="form-inline text-right">
                            <div class="form-group">
                                <label for="txtTotal">ยอดรวมก่อนลด :</label>
                                <input type="text" class="form-control" id="txtTotal" enableviewstate="false" readonly="true" disabled="disabled" runat="server" style="width: 100px;" />
                            </div>
                            <div class="form-group">
                                <label for="txtSumDiscount1">ส่วนลด 1 :</label>
                                <input type="text" class="form-control" id="txtSumDiscount1" enableviewstate="false" readonly="true" disabled="disabled" runat="server" style="width: 100px;" />
                            </div>
                            <div class="form-group">
                                <label for="txtSumDiscount2">ส่วนลด 2 :</label>
                                <input type="text" class="form-control" id="txtSumDiscount2" enableviewstate="false" readonly="true" disabled="disabled" runat="server" style="width: 100px;" />
                            </div>
                            <div class="form-group">
                                <label for="txtGrandTotal">ยอดสุทธิ :</label>
                                <input type="text" class="form-control" id="txtGrandTotal" enableviewstate="false" readonly="true" disabled="disabled" runat="server" style="width: 100px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row form-group">
                <ul class="nav nav-tabs" style="margin-top: -35px;">
                    <li class="active"><a data-toggle="tab" href="#detailList">รายการ</a></li>
                    <li><a data-toggle="tab" href="#poDetail">รายการชำระเงิน</a></li>
                    <%--<li><a data-toggle="tab" href="#invoiceTax">รายการใบกำกับภาษี</a></li>--%>
                    <li><a data-toggle="tab" href="#orderHistory">ประวัติสั่งซื้อ</a></li>
                    <li><a data-toggle="tab" href="#notice">แจ้งเตือน</a></li>
                </ul>
                <div class="tab-content" style="overflow: auto; height: 292px">
                    <div id="detailList" class="tab-pane fade in active">
                        <div class="col-xs-6 no-padding">
                            <div class="row form-group">
                                <button type="button" id="detailQuotation" runat="server" onclick="popupQuotationDetail()"
                                    class="btn-info" style="height: 30px;">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกรายการสินค้า
                                </button>
                            </div>
                        </div>
                        <div class="col-xs-12 no-padding">
                            <dx:ASPxGridView ID="gridViewDetailSaleOrder" ClientInstanceName="gridViewDetailSaleOrder" runat="server"
                                SettingsBehavior-AllowSort="false"
                                OnCustomCallback="gridViewDetailSaleOrder_CustomCallback"
                                KeyFieldName="id" Width="100%">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                <Paddings Padding="0px" />
                                <Border BorderWidth="0px" />
                                <BorderBottom BorderWidth="1px" />
                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                    PageSizeItemSettings-Visible="true">
                                    <PageSizeItemSettings Items="10, 20, 50" />
                                </SettingsPager>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="175" />
                                <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="80">
                                        <DataItemTemplate>

                                            <a id="btnEdit" class="btn btn-mini" onclick="editSaleorderDetail(this, <%# Eval("id")%>)" title="<%# Eval("product_no")%>">
                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                            </a>
                                            |
                                            <a id="btnDelete" class="btn btn-mini" onclick="deleteSaleorderDetail(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" Width="80px" />
                                    <dx:GridViewDataTextColumn FieldName="saleorder_description" Caption="Product Name" Width="150px" />
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Width="40px"
                                        FieldName="qty" Caption="Qty">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Width="70px"
                                        FieldName="unit_price" Caption="Unit Price">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Width="70px"
                                        FieldName="discount_amount" Caption="Discount (THB)">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Width="70px"
                                        FieldName="discount_percentage" Caption="Discount (%)">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Width="70px"
                                        FieldName="total_amount" Caption="Total Amount">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                </Columns>
                            </dx:ASPxGridView>
                        </div>

                    </div>
                    <div class="tab-pane fade" id="poDetail">
                        <div class="col-xs-12 no-padding">
                            <div class="col-xs-6 no-padding">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <button type="button" runat="server" id="Button6" class="btn-addItem" onclick="newPO()" style="margin-bottom: 10px;">เลขที่ PO ลูกค้า/การชำระเงิน</button>
                            </div>
                            <div class="col-xs-12">
                                <!-- Grideview -->
                                <dx:ASPxGridView ID="gridViewPO" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewPO"
                                    Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewPO_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewPO_HtmlDataCellPrepared">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>

                                    <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100px">
                                            <DataItemTemplate>
                                                <a id="btnEditPO" class="btn btn-mini <%# (Eval("invoice_no") == "" && Eval("invoice_date") == "") ? "" : ""%>" onclick="editPO(<%# Eval("id")%>)" title="Edit">
                                                    <i class="fa fa-pencil" aria-hidden="true"></i>
                                                </a>
                                                |
                                                <a id="btnDeletePO" class="btn btn-mini <%# (Eval("invoice_no") == "" && Eval("invoice_date") == "") ? "" : ""%>" onclick="deletePO(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                        <dx:GridViewDataTextColumn FieldName="ref_po_no" Caption="PO No" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="display_po_date" Caption="PO Date" VisibleIndex="6" />
                                        <%--<dx:GridViewDataTextColumn FieldName="display_payment_type" Caption="Payment Type" VisibleIndex="6" />--%>
                                        <dx:GridViewDataTextColumn Caption="Payment Detail" FieldName="percent_price" CellStyle-HorizontalAlign="Left" Width="250px" VisibleIndex="6">
                                            <DataItemTemplate>
                                                <asp:Label ID="lblDetail" runat="server"></asp:Label>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="percent_price" Caption="Payment Detail" VisibleIndex="6" />--%>
                                        <dx:GridViewDataTextColumn FieldName="invoice_no" Caption="Invoice No" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="display_invoice_date" Caption="Invoice Date" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="display_delivery_date" Caption="Delivery Date" VisibleIndex="6" />

                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="170" />
                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                    </SettingsPopup>
                                    <%--<ClientSideEvents BeginCallback="gridViewFreeBie_Callback" />--%>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                        <!-- Modal -->
                        <div class="modal fade" id="popupAddPOSaleOrder" role="dialog" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        New P/O
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">เลขที่ P/O ลูกค้า :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtPONo" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">วันที่ P/O ลูกค้า :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control numberic" id="txtPODate" readonly="true"
                                                            runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">เงือนไขการชำระเงิน :</label>
                                                    <div class="col-xs-3 no-padding">
                                                        <select class="form-control" id="cboTermOfPayment" runat="server" data-rule-required="true" data-msg-required="กรุณาเลือกเงือนไขการชำระเงิน" style="width: 330px;">
                                                            <option value="">--เลือกเงือนไขการชำระเงิน-</option>
                                                            <option value="1">เงินสด</option>
                                                            <option value="2">รับเช็คล่วงหน้า ณ วันที่ส่งของ</option>
                                                            <option value="3">เครดิต</option>
                                                            <option value="4">มัดจำ ก่อนส่งให้ลูกค้า</option>
                                                            <%--<option value="5">ส่วนต่างมัดจำ นับจากวันที่ส่งของ</option>--%>
                                                            <option value="6">แบ่งชำระ</option>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12" id="checkInAdvance">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">รับเช็ค</label>
                                                    <div class="col-xs-4 no-padding">
                                                        <div class="row form-group">
                                                            <div class="col-xs-12 no-padding-right">
                                                                <input type="text" class="form-control" id="txtDateCheckInAdvance" readonly="" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12" id="checkDownPayment">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">มัดจำ :</label>
                                                    <div class="col-xs-2 no-padding">
                                                        <div class="row form-group">
                                                            <div class="col-xs-12 no-padding">
                                                                <input type="text" class="form-control numberic" id="txtDownPayment" runat="server" style="margin-top: -1px;" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <label class="col-xs-2 text-right label-rigth">ส่วนต่าง :</label>
                                                    <div class="col-xs-2 no-padding" id="checkFinalPayment">
                                                        <div class="row form-group">
                                                            <div class="col-xs-12 no-padding">
                                                                <input type="text" class="form-control numberic" id="txtFinalPayment" runat="server" style="margin-top: -1px;" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12" id="checkCredit">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">จำนวนวัน :</label>
                                                    <div class="col-xs-4 no-padding">
                                                        <div class="row form-group">
                                                            <div class="col-xs-12 no-padding">
                                                                <input type="text" class="form-control numberic" id="txtCreditDay" runat="server" style="margin-top: -3px;" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12" id="checkPeriod">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">จำนวนงวด</label>
                                                    <div class="col-xs-1 no-padding">
                                                        <input type="text" class="form-control numberic" id="txtPeriodNo" runat="server" style="margin-top: -4px;" />
                                                    </div>
                                                    <label class="col-xs-2 text-right label-rigth hidden">เปอร์เซ็น</label>
                                                    <div class="col-xs-2 no-padding hidden">
                                                        <input type="text" class="form-control numberic" id="txtpercent" runat="server" style="margin-top: -4px;" />
                                                    </div>

                                                    <label class="col-xs-2 text-right label-rigth">งวดละ (บาท)</label>
                                                    <div class="col-xs-2 no-padding">
                                                        <input type="text" class="form-control numberic" id="txtCheckPeriodAmount" runat="server" style="margin-top: -4px;" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">เลขใบกำกับภาษี</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtInvoiceNo"
                                                            runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">วันที่ใบกำกับภาษี:</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="dtInvoiceDate" readonly="true"
                                                            runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <%-- <div class="col-xs-3  label-rigth text-right form-inline">
                                                        <label>วันที่วางบิล :</label>
                                                    </div>
                                                    <div class="col-xs-7 form-inline no-padding">
                                                        <input type="text" class="form-control numberic" id="dtDateBill"
                                                            runat="server" />
                                                    </div>--%>
                                                    <div class="row form-group">
                                                        <label class="col-xs-3 text-right label-rigth">เลขที่ใบส่งของชั่วคราว:</label>
                                                        <div class="col-xs-7 no-padding">
                                                            <input type="text" class="form-control" id="txtTempNoDelivery"
                                                                runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-xs-3  label-rigth text-right form-inline">
                                                        <label>วันที่ส่งของ :</label>
                                                    </div>
                                                    <div class="col-xs-7 form-inline no-padding">
                                                        <input type="text" class="form-control numberic" id="dtDateShipping"
                                                            readonly="true" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth"></label>
                                                    <div class="col-xs-7 no-padding text-right">
                                                        <button type="button" runat="server" id="Button9" onclick="addPO()" class="btn-app btn-addItem">ยืนยัน</button>
                                                        <button type="button" runat="server" id="Button10" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="invoiceTax">
                        <div class="row">
                            <div class="col-xs-12">
                                <button type="button" runat="server" id="Button3" class="btn-addItem" onclick="newTax()">เพิ่มรายการใบกำกับภาษี</button>
                            </div>
                            <div class="col-xs-12">
                                <!-- Grideview -->
                                <dx:ASPxGridView ID="gridViewTax" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewTax"
                                    Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewTax_CustomCallback">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>

                                    <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100px">
                                            <DataItemTemplate>
                                                <a id="btnEdit" class="btn btn-mini" onclick="editTax(<%# Eval("id")%>)" title="Edit">
                                                    <i class="fa fa-pencil" aria-hidden="true"></i>
                                                </a>
                                                |
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteTax(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                        <dx:GridViewDataTextColumn FieldName="tax_no" Caption="Tax No" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="tax_description" Caption="Tax Description" VisibleIndex="6" />
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                    </SettingsPopup>
                                    <%--<ClientSideEvents BeginCallback="gridViewFreeBie_Callback" />--%>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                        <!-- Modal -->
                    </div>
                    <div id="orderHistory" class="tab-pane fade">
                        <div class="row">
                            <div class="col-xs-12">
                                <!-- Grideview -->
                                <dx:ASPxGridView ID="gridViewOrderHistory" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewOrderHistory"
                                    Width="100%" KeyFieldName="id" EnableCallBacks="true"
                                    OnCustomCallback="gridViewOrderHistory_CustomCallback">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>

                                    <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Width="50px" Visible="false" VisibleIndex="5" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_subject" Caption="Subject" VisibleIndex="7" />
                                        <dx:GridViewDataTextColumn FieldName="is_enable" Caption="Enable" Visible="false" VisibleIndex="4" />
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="175" />

                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                    </SettingsPopup>

                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                    <div id="notice" class="tab-pane fade">
                        <div class="row">
                            <div class="col-xs-12" style="margin-bottom: 10px;">
                                <button type="button" runat="server" id="btnAddNoticeQuatation" class="btn-addItem" onclick="newNotice()">เพิ่มหัวข้อแจ้งเตือน</button>
                            </div>
                            <div class="col-xs-12">
                                <!-- Grideview -->
                                <dx:ASPxGridView ID="gridViewNotice" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewNotice"
                                    Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewNotice_CustomCallback">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                    <BorderBottom BorderWidth="1px" />
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="true">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>

                                    <SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100px">
                                            <DataItemTemplate>
                                                <a id="btnEdit" class="btn btn-mini" onclick="editNotice(<%# Eval("id")%>)" title="Edit">
                                                    <i class="fa fa-pencil" aria-hidden="true"></i>
                                                </a>|
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteNotice(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                        <dx:GridViewDataTextColumn FieldName="subject" Caption="Subject" VisibleIndex="6" />
                                        <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="6" />
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="170" />
                                    <SettingsPopup>
                                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                    </SettingsPopup>
                                    <%--<ClientSideEvents BeginCallback="gridViewFreeBie_Callback" />--%>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                        <!-- Modal -->
                        <div class="modal fade" id="popupAddNoticeSaleOrder" role="dialog" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        New Notification
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">หัวข้อการแจ้งเตือน :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtSubjectNotice" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">รายการแจ้งเตือน :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtDescriptionNotice" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">วันที่ / เวลาแจ้งเตือน :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control datepicker numberic" id="txtDateTimeNotice"
                                                            runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth"></label>
                                                    <div class="col-xs-7 no-padding text-right">
                                                        <button type="button" runat="server" id="btnSaveNoticeSaleOrder" onclick="addNotice()" class="btn-app btn-addItem">ยืนยัน</button>
                                                        <button type="button" runat="server" id="btnCancelNoticeSaleOrder" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- Modal -->
        <div class="modal fade" id="modal_quotation" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12 text-left">
                                <input id="chkSelectAllQuotation" type="checkbox" class="" onclick="selectAllQuotation()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchText" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchText" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewDetailList" ClientInstanceName="gridViewDetailList" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewDetailList_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewDetailList_HtmlDataCellPrepared"
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
                                                <%-- <input type="checkbox" id="productId" name="quotationDetailId"
                                                    onchange="chkproduct(<%# Eval("id") %>)" />--%>
                                                <dx:ASPxCheckBox ID="chkQuotationDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkQuotationDetail"
                                                    QuotationDetailId='<%# Eval("id")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="70px" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Product Name" VisibleIndex="5" Width="170px" />
                                        <%--<dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" VisibleIndex="7" Width="70px" />--%>
                                        <dx:GridViewDataTextColumn FieldName="qu_qty" Caption="Qty" VisibleIndex="6" Width="50px" />
                                        <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" FieldName="unit_price"
                                            Caption="Unit Price" VisibleIndex="7" Width="70px">
                                            <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            </PropertiesSpinEdit>
                                        </dx:GridViewDataSpinEditColumn>
                                        <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" FieldName="total_amount"
                                            Caption="Total Amount" VisibleIndex="8" Width="100px">
                                            <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            </PropertiesSpinEdit>
                                        </dx:GridViewDataSpinEditColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12 text-right" style="padding-top: 5px;">
                                <button type="button" runat="server" id="Button1" onclick="submitQuotationDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                <button type="button" runat="server" id="Button2" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_sale_order_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Edit Sale Order Detail
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
                                    <label class="col-xs-3 text-right label-rigth">Unit Price :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtProductUnitPrice" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group" id="dvDiscountDetailBaht">
                                    <label class="col-xs-3 text-right label-rigth">Discount (THB) :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtDiscountAmountItem" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12" id="dvDiscountDetailPercent">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Discount (%) :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtDiscountPercentItem" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-7 no-padding text-right">
                                        <button type="button" runat="server" id="Button7" onclick="submitSaleOrderDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button8" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdSelectedSaleOrderDetailId" />
        <asp:HiddenField runat="server" ID="hdMinUnitPrice" />
        <asp:HiddenField runat="server" ID="hdSelectedNoticeId" />
        <asp:HiddenField runat="server" ID="hdSelectedPOId" />
        <asp:HiddenField runat="server" ID="hdSelectedTaxId" />
        <asp:HiddenField runat="server" ID="hdCustomerId" />
        <asp:HiddenField runat="server" ID="hdQuotationType" />
        <asp:HiddenField runat="server" ID="hdQuotationItemDiscount" />
        <asp:HiddenField runat="server" ID="hdSaleOrderStatus" />
    </div>


</asp:Content>