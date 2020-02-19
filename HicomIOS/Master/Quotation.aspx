﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Quotation.aspx.cs" Inherits="HicomIOS.Master.Quotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <style>
        #txtQuatationNo {
            width: 130px;
            font-weight: bold;
        }

        #txtQuatationDate {
            width: 125px;
        }

        .txt-no {
            background-color: #e4effa !important;
        }

        .ui-autocomplete-input {
            border: none;
            font-size: 12px;
            margin-bottom: 0px;
            padding-top: 0px;
            border: 1px solid #CCC !important;
            padding-top: 0px !important;
            z-index: 3000;
            position: relative;
        }

        .ui-menu .ui-menu-item a {
            font-size: 12px;
        }

        .ui-autocomplete {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 999999 !important;
            float: left;
            display: none;
            min-width: 160px;
            width: 160px;
            padding: 4px 0;
            margin: 2px 0 0 0;
            list-style: none;
            background-color: #ffffff;
            border-color: #ccc;
            border-color: rgba(0, 0, 0, 0.2);
            border-style: solid;
            border-width: 1px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            -webkit-box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2);
            -moz-box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2);
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2);
            -webkit-background-clip: padding-box;
            -moz-background-clip: padding;
            background-clip: padding-box;
            *border-right-width: 2px;
            *border-bottom-width: 2px;
        }

        .ui-menu-item > a.ui-corner-all {
            display: block;
            padding: 3px 15px;
            clear: both;
            font-weight: normal;
            line-height: 18px;
            color: #555555;
            white-space: nowrap;
            text-decoration: none;
        }

        .ui-state-hover, .ui-state-active {
            color: #ffffff;
            text-decoration: none;
            background-color: #0088cc;
            border-radius: 0px;
            -webkit-border-radius: 0px;
            -moz-border-radius: 0px;
            background-image: none;
        }

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
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">

        var autoCompleteServiceDescData = [];
        var autoCompleteMFGData = [];
        var autoCompleteModelData = [];
        $(document).ready(function () {

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });

            $("#Splitter_0").parent().hide();

            //  Check type
            if ($("#cbbQuotationType").val() != "M") {
                $('#btnAddPartList').css('display', 'none');
                $('#btnAddAllPartList').css('display', 'none');
            }
            if ($("#cbbQuotationType").val() == "S") {
                $('#btnAddPartList').css('display', '');
            }
            var allowType = '<%= HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"].ToString().ToUpper() %>';
            $("#cbbQuotationType option").each(function () {
                if (allowType.indexOf($(this).val()) == -1) {
                    $(this).remove();
                }
            });
            //changedQuotationType();
            if (getParameterByName('dataId') == null
                && getParameterByName('cloneId') == null
                && getParameterByName('revisionId') == null) {
                setTimeout(changedQuotationType, 1000);
            }

            $("#txtDateTimeFreebies").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true, "changeYear": true
            });

            $("#txtQuatationDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true, "changeYear": true
            });

            $("#txtDateTimeFreebies").prop("disabled", true);

            /*PrepareData*/
            $('#hdTotal').val(0);
            $('#hdSumDiscountDetail').val(0);
            $('#hdTotalWithDiscountDetail').val(0);

            $('#hdSelectedFreebiesId').val(0);
            $('#hdSelectedNoticeId').val(0);
            $('#hdSelectedProductId').val(0);
            $('#hdSelectedServiceProductId').val(0);
            $('#StatusQu').css('display', 'none');
            //$('#dvIsService').css('display', 'none');

            //$("#txtDateTimeNotice").datetimepicker({
            //    dateFormat: 'dd/mm/yy'
            //});
            //$('#txtDateTimeNotice').datetimepicker('show');

            /*$("#txtDateTimeNotice").datepicker({
                dateFormat: 'dd/mm/yy'
            });*/

            //console.log(autoCompleteData);
            $("#txtDescriptionServiceParent").autocomplete({
                source: autoCompleteServiceDescData
            });
            $("#txtDescriptionServiceDetail").autocomplete({
                source: autoCompleteServiceDescData
            });

            $("#txtDataModel").autocomplete({
                source: autoCompleteModelData
            });
            $("#txtDataMfg").autocomplete({
                source: autoCompleteMFGData
            });
            var h = window.innerHeight;
            gridQuotationDetail.SetHeight(h - 300 - 36 - 100);
            gridViewService.SetHeight(h - 300 - 36 - 100);
            gridViewServiceDetail.SetHeight(h - 300 - 36 - 100);

            $('#quotation-detail-content').height(h - 300 - 36 - 30);

            var height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });

        function getParameterByName(name) {
            var url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {

            $("[name='is_enable']").bootstrapToggle();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };

            var height = dimensions.height - 300 - 36 - 100;
            console.log(height);
            gridQuotationDetail.SetHeight(height);
            gridViewService.SetHeight(height);
            gridViewServiceDetail.SetHeight(height);

            $('#quotation-detail-content').height(height + 70);

            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });


        /* FUNCION FREEBIES */
        function newFreebies() {
            gridViewFreeBie.FocusedRowIndex = -1;
            $('#hdSelectedFreebiesId').val(0);
            gridViewFreeBie.Refresh();
            $("#cbIsNotice").prop("checked", false);
            $("#txtDateTimeFreebies").val("");
            $('#txtFreebies').val("");

            $('#popupFreebiesQuatation').modal('show');
        }

        function addFreebies() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            var key = $('#hdSelectedFreebiesId').val();

            var descText = $('#txtFreebies').val();
            var dateNotificataion = $("#txtDateTimeFreebies").val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddFreeBie",
                data: '{id: "' + key + '" ,description: "' + descText + '" , date: "' + dateNotificataion + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onAddFreebieSuccess,
                failure: function (response) {
                    //alert(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function editFreebies(s, e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var input = s.GetMainElement();
            var key = $(input).attr("freebiesId");
            $('#hdSelectedFreebiesId').val(key);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ShowEditFreebies",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: showPopupFreebies,
                failure: function (response) {
                    alert(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function deleteFreebies(e) {

            var id = e;
            $('#modal_deleteFreebie').modal("show");
            $('#btn_deleteFreebie').val(id);

        }

        function submitdeleteFreebies(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var id = $('#btn_deleteNotice').val();

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/DeleteFreebies",
                data: '{id: "' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onAddFreebieSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
            $.LoadingOverlay("hide");

        }


        function showPopupFreebies(response) {
            console.log(response.d);
            var date = new Date(Date.parse(response.d.notice_date));
            $('#txtFreebies').val(response.d.description);
            $('#txtDateTimeFreebies').val(response.d.display_notice_date);
            if (response.d.display_notice_date != "") {
                $("#cbIsNotice").prop("checked", true);
            }
            $('#popupFreebiesQuatation').modal('show');
        }

        function onAddFreebieSuccess() {
            $('#modal_deleteFreebie').modal("hide");
            gridViewFreeBie.PerformCallback();
            $("#cbIsNotice").prop("checked", false);
            $("#txtDateTimeFreebies").val("");
            $('#txtFreebies').val("");
            $('#popupFreebiesQuatation').modal('hide');
        }

        function onCheckedFreeBies() {
            //console.info("CheckedFreeBies", $('#cbIsNotice').is(':checked'));
            if ($('#cbIsNotice').is(':checked')) {
                $("#txtDateTimeFreebies").prop("disabled", false);
            }
            else {
                $("#txtDateTimeFreebies").val("");
                $("#txtDateTimeFreebies").prop("disabled", true);
            }
        }
        /* END FUNCION FREEBIES */

        /* FUNCION NOTICE */
        function newNotice() {
            $('#hdSelectedNoticeId').val(0);
            $("#txtSubjectNotice").val("");
            $('#txtDescriptionNotice').val("");
            $("#txtDateTimeNotice").val("");

            $('#popupAddNoticeQuatation').modal('show');
        }

        function addNotice() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var subjectText = $('#txtSubjectNotice').val();
            var descText = $('#txtDescriptionNotice').val();
            var dateNotification = $("#txtDateTimeNotice").val();

            if (dateNotification == "") {
                alert("กรุณาเลือกวันที่แจ้งเตือน");
                $.LoadingOverlay("hide");
                return;
            }
            var date = $('#txtDateTimeNotice').handleDtpicker('getDate');
            console.log('date ' + date);
            var aa = date.toISOString();

            var key = $('#hdSelectedNoticeId').val();
            console.log("key==" + key);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddNotice",
                data: '{id: "' + key + '" ,subject: "' + subjectText + '", description: "' + descText + '" , date: "' + aa + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onAddNoticeSuccess,
                failure: function (response) {
                    //alert(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function editNotice(s) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var input = s;
            var key = $(input).attr("noticeId");

            console.log("key==" + key);

            $('#hdSelectedNoticeId').val(key);

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ShowEditNotice",
                data: '{id: "' + input + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response.d);
                    var date = new Date(Date.parse(response.d.notice_date));
                    $('#txtSubjectNotice').val(response.d.subject);
                    $('#txtDescriptionNotice').val(response.d.description);
                    $('#txtDateTimeNotice').val(response.d.display_notice_date);
                    $('#HDidNoti').val(response.d.id);
                    $('#hdSelectedNoticeId').val(input);
                    console.log("hdSelectedNoticeId==" + input);
                    $('#popupAddNoticeQuatation').modal('show');
                },
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
                            url: "Quotation.aspx/DeleteNotice",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#modal_deleteNotice').modal("hide");
                                        gridViewNotice.PerformCallback();
                                        $("#txtSubjectNotice").val("");
                                        $('#txtDescriptionNotice').val("");
                                        $("#txtDateTimeNotice").val("");
                                        $('#popupAddNoticeQuatation').modal('hide');
                                    });
                            }
                        });
                    }
                });
        }
        function onAddNoticeSuccess() {
            $('#modal_deleteNotice').modal("hide");
            gridViewNotice.PerformCallback();
            $("#txtSubjectNotice").val("");
            $('#txtDescriptionNotice').val("");
            $("#txtDateTimeNotice").val("");
            $('#popupAddNoticeQuatation').modal('hide');
        }

        function showPopupNotice(response) {
            console.log(response.d);
            var date = new Date(Date.parse(response.d.notice_date));
            $('#txtSubjectNotice').val(response.d.subject);
            $('#txtDescriptionNotice').val(response.d.description);
            $('#txtDateTimeNotice').val(response.d.display_notice_date);
            $('#HDidNoti').val(response.d.id);
            //$('#hdSelectedNoticeId').val(id);
            console.log("hdSelectedNoticeId==" + $('#hdSelectedNoticeId').val());
            $('#popupAddNoticeQuatation').modal('show');
        }

        /* END FUNCION NOTICE */

        /*FUNCTION QUOTATION DETAIL*/

        function getCheckBoxValue(s, e) {
            var value = s.GetChecked();
            var quotationType = $('#cbbQuotationType').val();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");


            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ClearSelectedCheckBox",
                data: '{quotationType : "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                },
                failure: function (response) {

                }
            });
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SelectedProduct",
                data: '{id: "' + key + '" , isSelected: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                },
                failure: function (response) {

                }
            });
        }

        function addQuotationDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#txtDiscountByItem').val(0);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SubmitAddProductQuotation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    // productSetGrid2.PerformCallback();
                    gridQuotationDetail.PerformCallback();

                    setTimeout(function () {
                        calDiscount();

                    }, 100);
                },
                failure: function (response) {

                }
            });

            //selList.ClearItems();


            $('#popupAddQuotationDetail').modal('hide');
            $.LoadingOverlay("hide");

        }

        function editProductDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/EditProductDetail",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    var quotationType = $('#cbbQuotationType').val();

                    if (quotationType == "A") {
                        $('#dvMFG').css('display', '');
                        cbMFGNo.PerformCallback();
                        if (data.is_history_issue_mfg) {
                            cbMFGNo.SetEnabled(false);
                        }
                        else {
                            cbMFGNo.SetEnabled(true);
                        }
                        $('#dvProductDescription2').hide();
                        $('#dvProductLine').hide();
                    }
                    else {
                        $('#dvMFG').css('display', 'none');
                        cbMFGNo.SetEnabled(false);
                        if (quotationType == "P") {
                            if ($('#rdoQuotationLine').is(':checked')) {
                                $('#dvProductDescription').hide();
                                $('#dvProductLine').show();

                            }
                            else {
                                $('#dvProductDescription').show();
                                $('#dvProductLine').hide();
                            }

                        }
                        else {
                            $('#dvProductDescription').show();
                            $('#dvProductLine').hide();
                        }
                    }


                    $('#txtPartNo').val(data.product_no);
                    $('#txtProductDescription').val(data.quotation_description);
                    $('#txtProductQty').val(data.qty);
                    $('#txtProductUnitPrice').val(data.unit_price);
                    $('#hdSelectedProduceId').val(data.id);
                    $('#txtDiscountAmountItem').val(data.discount_amount);
                    $('#txtDiscountPercentItem').val(data.discount_percentage);
                    $('#hdMinUnitPrice').val(data.min_unit_price);
                    $('#txtProductLine1').val(data.quotation_line_1);
                    $('#txtProductLine2').val(data.quotation_line_2);
                    $('#txtProductLine3').val(data.quotation_line_3);
                    $('#txtProductLine4').val(data.quotation_line_4);
                    $('#txtProductLine5').val(data.quotation_line_5);
                    $('#txtProductLine6').val(data.quotation_line_6);
                    $('#txtProductLine7').val(data.quotation_line_7);
                    $('#txtProductLine8').val(data.quotation_line_8);
                    $('#txtProductLine9').val(data.quotation_line_9);
                    $('#txtProductLine10').val(data.quotation_line_10);
                    //$('#cbMFGNo').val(data.mfg_no)
                    cbMFGNo.SetValue(data.mfg_no);

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
                    $('#popupAddQuotationDetailEdit').modal('show');
                    $.LoadingOverlay("hide");

                },
                failure: function (response) {

                }
            });
        }

        function submitProductDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#txtDiscountByItem').val(0);
            var key = $('#hdSelectedProduceId').val();
            var qty = $('#txtProductQty').val();
            var unit_price = $('#txtProductUnitPrice').val();
            var amount = $('#txtDiscountAmountItem').val();
            var percent = $('#txtDiscountPercentItem').val();
            var discountType = $('#cbbDiscountByItem').val();
            var minUnitPrice = $('#hdMinUnitPrice').val();
            var description = $('#txtProductDescription').val();
            var line1 = $('#txtProductLine1').val();
            var line2 = $('#txtProductLine2').val();
            var line3 = $('#txtProductLine3').val();
            var line4 = $('#txtProductLine4').val();
            var line5 = $('#txtProductLine5').val();
            var line6 = $('#txtProductLine6').val();
            var line7 = $('#txtProductLine7').val();
            var line8 = $('#txtProductLine8').val();
            var line9 = $('#txtProductLine9').val();
            var line10 = $('#txtProductLine10').val();
            var mfgNo = cbMFGNo.GetValue();
            if (parseInt(unit_price) < parseInt(minUnitPrice)) {
                $.LoadingOverlay("hide");
                swal("ไม่สามารถกำหนดราคาต่ำกว่า ราคาที่กำหนดได้");
                return;
            }
            if ($('#cbDiscountByItem').is(':checked')) {
                if ($('#cbbDiscountByItem').val() == "P") {
                    if (parseInt(percent) > 100) {
                        $.LoadingOverlay("hide");
                        swal("เปอร์เซนต์ส่วนลดไม่สามารถเกิน 100%");
                        return;
                    }
                }
                else if ($('#cbbDiscountByItem').val() == "A") {
                    var totalPrice = parseInt(unit_price) * parseInt(qty);
                    if (parseInt(amount) > totalPrice) {
                        $.LoadingOverlay("hide");
                        swal("ส่วนลดไม่สามารถลดเกินมูลค่าที่ซื้อได้");
                        return;
                    }
                }
            }

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SubmitProductDetail",
                data: '{id: "' + key + '" , qty: "' + qty + '",unit_price : "' + unit_price + '"' +
                    ', amount :"' + amount + '" , percent : "' + percent + '" , discountType : "' + discountType + '" , mfgNo : "' + mfgNo + '"' +
                    ', description : "' + description + '" , line1 : "' + line1 + '"  , line2 : "' + line2 + '"  , line3 : "' + line3 + '"' +
                    ', line4 : "' + line4 + '" , line5 : "' + line5 + '"  , line6 : "' + line6 + '"  , line7 : "' + line7 + '"' +
                    ', line8 : "' + line8 + '" , line9 : "' + line9 + '"  , line10 : "' + line10 + '" }',

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtPartNo').val("");
                    $('#txtProductDescription').val("");
                    $('#txtProductLine1').val("");
                    $('#txtProductLine2').val("");
                    $('#txtProductLine3').val("");
                    $('#txtProductLine4').val("");
                    $('#txtProductLine5').val("");
                    $('#txtProductLine6').val("");
                    $('#txtProductLine7').val("");
                    $('#txtProductLine8').val("");
                    $('#txtProductLine9').val("");
                    $('#txtProductLine10').val("");
                    $('#txtProductQty').val(0);
                    $('#txtProductUnitPrice').val(0);
                    $('#hdSelectedProduceId').val(0);
                    $('#txtDiscountAmountItem').val(0);
                    $('#txtDiscountPercentItem').val(0);
                    $('#hdMinUnitPrice').val(0);

                    $('#popupAddQuotationDetailEdit').modal('hide');

                    gridQuotationDetail.PerformCallback();

                },
                failure: function (response) {
                    $.LoadingOverlay("hide");

                }
            });
            calDiscount();
            $.LoadingOverlay("hide");
        }



        function deleteProductDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.LoadingOverlay("hide");
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
                            url: "Quotation.aspx/DeleteProductDetail",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d) {
                                    swal("ลบข้อมูลสำเร็จ!", {
                                        icon: "success"
                                    })
                                        .then((value) => {
                                            gridQuotationDetail.PerformCallback();
                                            calDiscount();
                                        });
                                }
                            }
                        });
                    }
                });
        }

        function changedProductDetail(s, e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var value = s.GetValue();
            var input = s.GetMainElement();
            var index = gridQuotationDetail.GetFocusedRowIndex();
            var key = gridQuotationDetail.keys[index];
            if (e == "discount_amount" || e == "discount_percent") {
                if (!$('#cbDiscountByItem').is(':checked')) {
                    value = "0";
                }
                else {
                    if ($("#cbbDiscountByItem").val() == "P") {
                        value = e == "discount_percent" ? value : "0";
                    }
                    else if ($("#cbbDiscountByItem").val() == "A") {
                        value = e == "discount_amount" ? value : "0";
                    }
                    else {
                        value = "0";
                    }
                }
            }
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/EditProductDetail",
                data: '{id: "' + key + '" , value: "' + value + '" , col : "' + e + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var sumTotal = 0;
                    var sumTotalWithDiscountDetail = 0;

                    //console.log(sumTotal);
                    $('#hdTotal').val(sumTotal);
                    $('#hdTotalWithDiscountDetail').val(sumTotalWithDiscountDetail);
                    gridQuotationDetail.PerformCallback();
                    calDiscount();

                },
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");
        } // ไม่ได้ใช้

        function changeDiscount() {
            gridQuotationDetail.PerformCallback();
        }

        function selectOnCheckedDiscountByItem() {

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SlectOnCheckedDiscountByItem",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onCheckedDiscountByItem,
            });
        }


        function onCheckedDiscountByItem() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var typeDiscount = 0;
            var check = 0;
            if ($('#cbDiscountByItem').is(':checked')) {
                check = 1;
            }

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/CheckedDiscountByItem",
                data: '{check:"' + check + '",typeDiscount:"' + typeDiscount + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "Null") {
                        $.LoadingOverlay("hide");
                        alert("กรุณาเพิ่มสินค้า");
                        $('#cbDiscountByItem').prop('checked', false);

                    } else {
                        if ($('#cbDiscountByItem').is(':checked')) {
                            //$("#txtDiscountByItem").prop("disabled", false);
                            $("#txtDiscountByItem").prop("disabled", true);
                            $("#cbbDiscountByItem").prop("disabled", false);
                            $('#txtDiscountByItem').val(0);

                            if ($('#cbbDiscountByItem').val() == "P") {

                                $("#txtDiscountByItem").prop("disabled", false);
                                $("#dvDiscountDetailBaht").hide();
                                $("#dvDiscountDetailPercent").show();
                            }
                            else if ($('#cbbDiscountByItem').val() == "A") {
                                $("#txtDiscountByItem").prop("disabled", false);
                                $("#dvDiscountDetailBaht").show();
                                $("#dvDiscountDetailPercent").hide();
                            }

                        }
                        else {
                            $("#txtDiscountByItem").val("");
                            $("#txtDiscountByItem").prop("disabled", true);
                            $("#cbbDiscountByItem").prop("disabled", true);
                            $("#dvDiscountDetailBaht").prop("disabled", true);
                            $("#dvDiscountDetailPercent").prop("disabled", true);
                        }

                        calDiscount();

                        gridQuotationDetail.PerformCallback();
                        gridViewService.PerformCallback();


                        $.LoadingOverlay("hide");
                    }
                },
                failure: function (response) {

                }
            });

        }

        function onDiscountByItemTextChange() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var discount_item_qty = parseInt($('#txtDiscountByItem').val());
            var discount_item_type = $('#cbbDiscountByItem').val();
            var cbbQuotationType = $('#cbbQuotationType').val();
            console.log("cc=" + cbbQuotationType);
            if ($('#cbbDiscountByItem').val() == "P") {
                if (discount_item_qty > 100) {
                    $.LoadingOverlay("hide");
                    swal("ไม่สามารถลดได้เกิน 100%");
                    return;
                }
            }


            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SetDiscountByItemAll",
                data: '{discount_item_qty : "' + discount_item_qty + '", discount_item_type : "' + discount_item_type + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (cbbQuotationType == "D" || cbbQuotationType == "C" || cbbQuotationType == "o") {

                        calDiscount();
                        gridViewService.PerformCallback();

                        $.LoadingOverlay("hide");
                    } else {

                        calDiscount();
                        gridQuotationDetail.PerformCallback();

                        $.LoadingOverlay("hide");
                    }

                },
                failure: function (response) {

                    $.LoadingOverlay("hide");
                }
            });



        }
        function selectOnCheckedDiscountBottomBill1() {
            $("#txtDiscountBottomBill1").val("");
            onCheckedDiscountBottomBill1();
        }

        function onCheckedDiscountBottomBill1() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var typeDiscount = 1;
            var check = 0;
            if ($('#cbDiscountBottomBill1').is(':checked')) {
                console.log("check! Bill1!!!");
                check = 1;
            }
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/CheckedDiscountByItem",
                data: '{check:"' + check + '",typeDiscount:"' + typeDiscount + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "Null") {
                        $.LoadingOverlay("hide");
                        swal("กรุณาเพิ่มสินค้า");
                        $('#cbDiscountBottomBill1').prop('checked', false);

                    } else {
                        if ($('#cbDiscountBottomBill1').is(':checked')) {
                            $("#txtDiscountBottomBill1").prop("disabled", false);
                            $("#cbbDiscountBottomBill1").prop("disabled", false);

                            //$("#txtDiscountBottomBill2").prop("disabled", false);
                            //$("#cbbDiscountBottomBill2").prop("disabled", false);
                            $("#cbDiscountBottomBill2").prop("disabled", false);
                        }
                        else {
                            $("#txtDiscountBottomBill1").val("");
                            $("#txtDiscountBottomBill1").prop("disabled", true);
                            $("#cbbDiscountBottomBill1").prop("disabled", true);

                            $("#txtDiscountBottomBill2").val("");
                            $("#txtDiscountBottomBill2").prop("disabled", true);
                            $("#cbbDiscountBottomBill2").prop("disabled", true);
                            $("#cbDiscountBottomBill2").prop("disabled", true);
                            $("#cbDiscountBottomBill2").prop("checked", false);

                        }
                        gridQuotationDetail.PerformCallback();
                        calDiscount();

                        $.LoadingOverlay("hide");
                    }
                },
                failure: function (response) {

                }
            });


        }

        function selectOnCheckedDiscountBottomBill2() {
            $("#txtDiscountBottomBill2").val("");
            onCheckedDiscountBottomBill2();
        }

        function onCheckedDiscountBottomBill2() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var typeDiscount = 2
            var check = 0;
            if ($('#cbDiscountBottomBill2').is(':checked')) {
                check = 1;
                console.log("check! Bill222222!!!");
            }
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/CheckedDiscountByItem",
                data: '{check:"' + check + '",typeDiscount:"' + typeDiscount + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "Null") {
                        $.LoadingOverlay("hide");
                        swal("กรุณาเพิ่มสินค้า");
                        $('#cbDiscountBottomBill2').prop('checked', false);

                    } else {
                        if ($('#cbDiscountBottomBill2').is(':checked')) {
                            $("#txtDiscountBottomBill2").prop("disabled", false);
                            $("#cbbDiscountBottomBill2").prop("disabled", false);
                        }
                        else {
                            $("#txtDiscountBottomBill2").val("");
                            $("#txtDiscountBottomBill2").prop("disabled", true);
                            $("#cbbDiscountBottomBill2").prop("disabled", true);
                        }
                        gridQuotationDetail.PerformCallback();
                        calDiscount();

                        $.LoadingOverlay("hide");
                    }
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
                url: "Quotation.aspx/CalcurateDiscount",
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
                console.log("total==" + total);

                var discountDetail = data.discount_total;
                console.log("discountDetail==" + discountDetail);

                var totalWithDiscountDetail = total - discountDetail;
                console.log("total - discountDetail==" + totalWithDiscountDetail);

                var sumBottomDiscount1 = 0;
                var sumBottomDiscount2 = 0;
                var grandTotal = 0;
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
                    $("#txtSumDiscount1").val(number_format(sumBottomDiscount1, 2));
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
                    $("#txtSumDiscount1").val(number_format(sumBottomDiscount1, 2));
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
                    $("#txtSumDiscount2").val(number_format(sumBottomDiscount2, 2));
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
                    $("#txtSumDiscount2").val(number_format(sumBottomDiscount2, 2));
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
                $('#txtGrandTotal').val(number_format(grandTotal, 2));
            }, 0.8);
            gridViewService.PerformCallback();
            gridQuotationDetail.PerformCallback();
        }

        function popupQuotationDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            $('#txtSearchBoxData').val("");
            $('#txtSearchMaintenance').val("");
            var quotationType = $('#cbbQuotationType').val();
            if (quotationType == "A") {
                if (cbbCustomerID.GetValue() == null) {
                    $.LoadingOverlay("hide");
                    swal("กรุณาเลือกลูกค้า สำหรับเอกสาร Annual Service");

                    return;
                }
            }

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ClearSelectedCheckBox",
                data: '{quotationType : "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                },
                failure: function (response) {

                }
            });

            $('#txtDescriptionServiceParent').val("");
            $('#txtQtyServiceParent').val("");
            $('#txtUnitPriceServiceParent').val("");
            $('#cbbUnitType').val("");
            $('#txtDiscount').val("");
            $('#txtDiscountType').val("");

            $('#btnAddPartList').css('display', 'none');
            $('#btnAddAllPartList').css('display', 'none');

            if (quotationType == "C" || quotationType == "D" || quotationType == "O") {
                $('#dvIsService').css('display', '');
                $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                $('#dvGridService').css('display', ''); // SHOW DIV GRID
                $('#dvIsProductDescription').css('display', 'none');

                if ($('#rdoLumpsum').is(':checked')) {
                    $('#dvIsLumpsum').css('display', '');
                    $('#dvIsLumpsum2').css('display', '');
                    $('#dvIsLumpsum3').css('display', '');
                    $('#dvIsLumpsum4').css('display', '');
                } else if ($('#rdoNotLumpsum').is(':checked')) {
                    $('#dvIsLumpsum').css('display', 'none');
                    $('#dvIsLumpsum2').css('display', 'none');
                    $('#dvIsLumpsum3').css('display', 'none');
                    $('#dvIsLumpsum4').css('display', 'none');
                } else {
                    swal("กรุณาเลือก Service Type");
                    $.LoadingOverlay("hide");
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "Quotation.aspx/GetQuotationDesignText",
                    data: '{ quotationType:"' + quotationType + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        autoCompleteServiceDescData = JSON.parse(response.d);


                        $("#txtDescriptionServiceParent").autocomplete({
                            source: autoCompleteServiceDescData
                        });
                        $("#txtDescriptionServiceDetail").autocomplete({
                            source: autoCompleteServiceDescData
                        });
                        setTimeout(function () {

                            if ($('#cbDiscountByItem').is(':checked')) {

                                if ($('#cbbDiscountByItem').val() == "P") {
                                    $('#lbDiscount').text("ส่วนลด จำนวนเงิน");
                                    $('#txtDiscountType').val("P");

                                }
                                else if ($('#cbbDiscountByItem').val() == "A") {
                                    $('#lbDiscount').text("ส่วนลด %");
                                    $('#txtDiscountType').val("A");
                                }
                            }
                            else {
                                $('#dvIsLumpsum4').css('display', 'none');
                            }
                            $('#popupAddQuotationDetailParentService').modal('show');
                            $.LoadingOverlay("hide");
                        }, 0.8);

                    },
                    failure: function (response) {

                    }
                });


            }
            else if (quotationType == "P") {
                $('#dvIsService').css('display', 'none');
                $('#dvGridProduct').css('display', ''); // HIDE DIV GRID
                $('#dvGridService').css('display', 'none'); // SHOW DIV GRID
                $('#dvIsProductDescription').css('display', '');
                var txtSearch = "";
                var quotationType = "";
                //productSetGrid2.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString());

                //productSetGrid2.PerformCallback();

                $.ajax({
                    type: "POST",
                    url: "Quotation.aspx/PopupSelectProduct",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#popupAddQuotationDetail').modal('show');
                        productSetGrid2.PerformCallback();
                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {

                    }
                });
            }
            else if (quotationType == "S") {
                $('#dvIsService').css('display', 'none');
                $('#dvIsProductDescription').css('display', 'none');
                $('#btnAddAllPartList').css('display', 'inline-block');
                $("#popupAddQuotationMaintenance .modal-header").html("เลือก Model");
                $('#popupAddQuotationMaintenance').modal('show');
                $('#dvMaintenanceModel').css('display', '');
                $('#dvMaintenancePackage').css('display', 'none');
                setTimeout(function () {
                    gridViewMaintenance.PerformCallback();
                    $.LoadingOverlay("hide");
                }, 0.8);
            }
            else if (quotationType == "M") {
                $('#btnAddPartList').css('display', 'inline-block');
                $('#btnAddAllPartList').css('display', 'inline-block');
                $('#dvIsService').css('display', 'none');
                $('#dvIsProductDescription').css('display', 'none');
                $("#popupAddQuotationMaintenance .modal-header").html("เลือก Model");
                $('#popupAddQuotationMaintenance').modal('show');
                $('#dvMaintenanceModel').css('display', '');
                $('#dvMaintenancePackage').css('display', 'none');
                setTimeout(function () {

                    gridViewMaintenance.PerformCallback();
                    $.LoadingOverlay("hide");
                }, 0.8)
            }
            else if (quotationType == "A") {
                $('#dvIsService').css('display', 'none');
                $('#dvIsProductDescription').css('display', 'none');
                $('#dvGridProduct').css('display', ''); // SHOW DIV GRID
                $('#dvGridService').css('display', 'none'); // HIDE DIV GRID

                //gridViewAnnualServiceItem.PerformCallback();

                $('#chkBoxAllAnnual').prop('checked', false);
                setTimeout(function () {
                    $.ajax({
                        type: "POST",
                        url: "Quotation.aspx/PopupSelectAnnualService",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $('#popupAddQuotationAnnualService').modal('show');
                            gridviewAnnualServiceHistory.PerformCallback();
                            $.LoadingOverlay("hide");
                        },
                        failure: function (response) {

                        }
                    });
                }, 100)
            }
            $('#hdSelectedServiceProductId').val(0);
            calDiscount();

        }

        function addServiceParent() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#txtDiscountByItem').val(0);
            var description = $('#txtDescriptionServiceParent').val();
            var qty = $('#txtQtyServiceParent').val();
            var unitPrice = $("#txtUnitPriceServiceParent").val();
            var unitType = cbbUnitType.GetText();
            var txtDiscount = $("#txtDiscount").val();
            var txtDiscountType = $("#txtDiscountType").val();

            //console.log("unit=" + unitType);
            //console.log("unitId=" + cbbUnitType.GetValue());

            var serviceType = ($('#rdoLumpsum').is(':checked')) ? "IsLumpsum" : ""
            var key = $('#hdSelectedServiceProductId').val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddServiceParent",
                data: '{id: "' + key + '" ,description: "' + description + '", qty: "' + qty + '" , unitPrice: "' + unitPrice + '", serviceType :"' + serviceType + '" , unitType :"' + unitType + '" , discountValue :"'
                    + txtDiscount + '", discountType :"' + txtDiscountType + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    calDiscount();
                    gridViewService.PerformCallback();
                    //gridViewServiceDetail.PerformCallback(key);
                    $('#popupAddQuotationDetailParentService').modal('hide');
                },
                failure: function (response) {

                }
            });

            //selList.ClearItems();

            $.LoadingOverlay("hide");
        }

        function editServiceParent(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;// $(input).attr("quotationDetailId");
            $('#hdSelectedServiceProductId').val(key);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ShowEditServiceDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdSelectedParentId').val(data.parent_id);
                    $('#txtDescriptionServiceParent').val(data.quotation_description);
                    $('#txtQtyServiceParent').val(data.qty);
                    $('#txtUnitPriceServiceParent').val(data.unit_price);
                    console.log($('#rdoLumpsum').is(':checked'));

                    if ($('#cbDiscountByItem').is(':checked')) {

                        if ($('#rdoLumpsum').is(':checked')) {

                            $('#dvIsLumpsum4').css('display', '');
                            if ($('#cbbDiscountByItem').val() == "P") {
                                $('#lbDiscount').text("ส่วนลด %");
                                $('#txtDiscountType').val("P");
                                $('#txtDiscount').val(data.discount_percentage);

                            }
                            else if ($('#cbbDiscountByItem').val() == "A") {
                                $('#lbDiscount').text("ส่วนลด จำนวนเงิน");
                                $('#txtDiscountType').val("A");
                                $('#txtDiscount').val(data.discount_amount);
                            }
                        }

                        else {
                            console.log("checked = F");
                            $('#dvIsLumpsum4').css('display', 'none');
                        }
                    }

                    else {
                        if ($('#rdoNotLumpsum').is(':checked')) {
                            $('#dvIsLumpsum').css('display', 'none');
                            $('#dvIsLumpsum2').css('display', 'none');
                            $('#dvIsLumpsum3').css('display', 'none');
                        }

                        $('#dvIsLumpsum4').css('display', 'none');
                    }

                    cbbUnitType.SetText(data.unit_code);
                    $('#hdMinUnitPrice').val(data.min_unit_price);
                    $('#popupAddQuotationDetailParentService').modal('show');
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function popupServiceDetail() {
            //var input = s.GetMainElement();
            //var key = e;//$(input).attr("quotationDetailId");
            $('#dvDiscount').css('display', 'none');
            if ($('#rdoLumpsum').is(':checked')) {
                $('#dvIsLumpsumDetail').css('display', 'none');
                $('#dvIsLumpsumDetail2').css('display', 'none');
                $('#dvIsLumpsumUnittypeServiceDetail').css('display', 'none');

                $('#dvDiscount').css('display', 'none');
            }
            else if ($('#rdoNotLumpsum').is(':checked')) {
                $('#dvIsLumpsumUnittypeServiceDetail').css('display', '');
                $('#dvIsLumpsumDetail').css('display', '');
                $('#dvIsLumpsumDetail2').css('display', '');
                if ($('#cbDiscountByItem').is(':checked')) {
                    $('#dvDiscount').css('display', '');
                    if ($('#cbbDiscountByItem').val() == "P") {
                        $('#lbDiscountDetail').text("ส่วนลด %");
                        $('#txtDiscountDetailType').val("P");
                        $('#txtDiscountDetailValue').val(0);

                    }
                    else if ($('#cbbDiscountByItem').val() == "A") {
                        $('#lbDiscountDetail').text("ส่วนลด จำนวนเงิน");
                        $('#txtDiscountDetailType').val("A");
                        $('#txtDiscountDetailValue').val(0);
                    }
                }
            }
            //$('#hdSelectedParentId').val(key);
            $('#txtDescriptionServiceDetail').val("");
            $('#txtQtyServiceDetail').val("");
            $('#cbbUnitTypeServiceDetail').val("");

            $('#txtUnitPriceServiceDetail').val("");
            $('#hdSelectedServiceProductId').val(0);
            $('#hdMinUnitPrice').val(0);

            $('#popupAddQuotationDetailServiceDetail').modal('show');


        }

        function addServiceDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#txtDiscountByItem').val(0);
            var description = $('#txtDescriptionServiceDetail').val();
            var qty = $('#txtQtyServiceDetail').val();

            var unitPrice = $("#txtUnitPriceServiceDetail").val();
            var unitType = cbbUnitTypeServiceDetail.GetText();
            var serviceType = ($('#rdoLumpsum').is(':checked')) ? "IsLumpsum" : "";
            var key = $('#hdSelectedServiceProductId').val();
            var parentId = $('#hdSelectedParentId').val();

            var discountValue = $('#txtDiscountDetailValue').val();
            var discountType = $('#txtDiscountDetailType').val();

            //console.log("hhh" + unitType);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddServiceDetail",
                data: '{id: "' + key + '",parentId: "' + parentId + '" ,description: "' + description + '", qty: "' + qty + '" , unitPrice: "' + unitPrice + '" , serviceType :"' + serviceType + '", unitType :"' + unitType + '" , discountValue : "' + discountValue + '" , discountType : "' + discountType + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#popupAddQuotationDetailServiceDetail').modal('hide');

                    calDiscount();


                },
                failure: function (response) {

                }
            });
            console.log(parentId);
            gridViewServiceDetail.PerformCallback(parentId);

            //selList.ClearItems();

            $.LoadingOverlay("hide");
        }

        function editServiceDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("quotationDetailId");
            var serviceType = ($('#rdoLumpsum').is(':checked')) ? "IsLumpsum" : "";

            $('#dvDiscount').css('display', 'none');
            if ($('#rdoLumpsum').is(':checked')) {
                $('#dvIsLumpsumDetail').css('display', 'none');
                $('#dvIsLumpsumDetail2').css('display', 'none');
                $('#dvIsLumpsumUnittypeServiceDetail').css('display', 'none');

                $('#dvDiscount').css('display', 'none');
            }
            else if ($('#rdoNotLumpsum').is(':checked')) {
                $('#dvIsLumpsumUnittypeServiceDetail').css('display', '');
                $('#dvIsLumpsumDetail').css('display', '');
                $('#dvIsLumpsumDetail2').css('display', '');
                if ($('#cbDiscountByItem').is(':checked')) {
                    $('#dvDiscount').css('display', '');
                    if ($('#cbbDiscountByItem').val() == "P") {
                        $('#lbDiscountDetail').text("ส่วนลด %");
                        $('#txtDiscountDetailType').val("P");
                        $('#txtDiscountDetailValue').val(0);

                    }
                    else if ($('#cbbDiscountByItem').val() == "A") {
                        $('#lbDiscountDetail').text("ส่วนลด จำนวนเงิน");
                        $('#txtDiscountDetailType').val("A");
                        $('#txtDiscountDetailValue').val(0);
                    }
                }
            }

            $('#hdSelectedServiceProductId').val(key);
            $('#dvDiscount').css('display', 'none');
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ShowEditServiceDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdSelectedParentId').val(data.parant_id);
                    $('#txtDescriptionServiceDetail').val(data.quotation_description);
                    $('#txtQtyServiceDetail').val(data.qty);

                    $('#txtUnitPriceServiceDetail').val(data.unit_price);
                    $('#hdMinUnitPrice').val(data.min_unit_price);

                    if ($('#cbDiscountByItem').is(':checked')) {
                        if ($('#rdoNotLumpsum').is(':checked')) {
                            $('#dvDiscount').css('display', '');
                            if ($('#cbbDiscountByItem').val() == "P") {
                                $('#lbDiscountDetail').text("ส่วนลด %");
                                $('#txtDiscountDetailType').val("P");
                                $('#txtDiscountDetailValue').val(data.discount_percentage);

                            }
                            else if ($('#cbbDiscountByItem').val() == "A") {
                                $('#lbDiscountDetail').text("ส่วนลด จำนวนเงิน");
                                $('#txtDiscountDetailType').val("A");
                                $('#txtDiscountDetailValue').val(data.discount_amount);
                            }
                        }
                        else {
                            $('#dvDiscount').css('display', 'none');
                        }
                    }
                    else {
                        $('#dvDiscount').css('display', 'none');
                    }

                    cbbUnitTypeServiceDetail.SetText(data.unit_code);
                    $('#popupAddQuotationDetailServiceDetail').modal('show');

                    $('#dvProductDescription').show();
                    $('#dvProductLine').hide();
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function deleteServiceDetail(e) {
            var id = e;
            $('#hdSelectedServiceProductId').val(id);
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
                            url: "Quotation.aspx/DeleteServiceDetail",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#hdSelectedServiceProductId').val(0);
                                        calDiscount();

                                        var parent_id = $('#hdSelectedParentId').val();
                                        gridViewServiceDetail.PerformCallback(parent_id);
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

        var modelPackageString = "";
        var modelPackageHour = "";
        function getPackageFromModel(e, m) {
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("ModelId");
            $('#hdMaintenanceModelId').val(key);
            modelPackageString = m;
            $('#lbMaintenanceModel').html(modelPackageString);

            var quotationType = $('#cbbQuotationType').val();
            if (quotationType == 'S') {
                $('#popupAddQuotationMaintenance').modal('hide');
                popupPartList()
            } else {
                $.ajax({
                    type: "POST",
                    url: "Quotation.aspx/GetMaintenanceServicePackage",
                    data: '{id: "' + key + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var data = response.d;
                        gridViewMaintenancePackage.PerformCallback();
                        $('#dvMaintenanceModel').css('display', 'none');
                        $('#dvMaintenancePackage').css('display', '');
                    },
                    failure: function (response) {

                    }
                });
            }
        }

        function selectedMaintenancePackage(e, s) {
            modelPackageHour = s;
            var key = e;//$(input).attr("PackageId");
            var model_id = $('#hdMaintenanceModelId').val();
            $('#hdPackageId').val(key);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SelectedMaintenancePackage",
                data: '{id: "' + key + '", model_id: "' + model_id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        function backToModel() {
            gridViewMaintenance.PerformCallback();
            $('#dvMaintenanceModel').css('display', '');
            $('#dvMaintenancePackage').css('display', 'none');
            gridViewMaintenancePackage.PerformCallback();
        }

        function addMaintenancePackage() {

            $('#btnAddPartList').css('display', '');
            $('#btnAddAllPartList').css('display', '');

            $('#lblModelPackage').html("Model : " + modelPackageString + ", Running hours : " + modelPackageHour);
            $('#txtHour').val(modelPackageHour);

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/RefreshMAPackage",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#popupAddQuotationMaintenance').modal('hide');

                    gridQuotationDetail.PerformCallback()
                    calDiscount();
                },
                failure: function (response) {

                }
            });
        }

        function popupPartList() {
            isAllPartSearch = false;

            $('#txtSearchPartList').val("");
            var key = $('#hdMaintenanceModelId').val();
            var package_id = '0';//$('#hdPackageId').val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetPartListData",
                data: '{id: "' + key + '",package_id: "' + package_id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartList.PerformCallback();
                    //$('#btnAddPartList').css('display', '');
                    //$('#btnAddAllPartList').css('display', '');
                    //var data = response.d;
                    //$('#lbMaintenanceModel').html(data[0].model_name);
                    //gridViewMaintenancePackage.PerformCallback();
                    //$('#dvMaintenanceModel').css('display', 'none');
                    //$('#dvMaintenancePackage').css('display', '');
                },
                failure: function (response) {

                }
            });
            $("#chkselectAllPartListPackage").prop("checked", false);
            $("#popupAddQuotationPartList .modal-header").html("เพิ่ม PartList ย่อย");
            $('#popupAddQuotationPartList').modal('show');
        }

        var isAllPartSearch = false;
        function popupAllPartList() {
            isAllPartSearch = true;

            $('#txtSearchPartList').val("")
            var key = 0;//$('#hdMaintenanceModelId').val();
            var package_id = 0;//$('#hdPackageId').val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetPartListData",
                data: '{id: "' + key + '",package_id: "' + package_id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartList.PerformCallback();
                    //var data = response.d;
                    //$('#lbMaintenanceModel').html(data[0].model_name);
                    //gridViewMaintenancePackage.PerformCallback();
                    //$('#dvMaintenanceModel').css('display', 'none');
                    //$('#dvMaintenancePackage').css('display', '');
                },
                failure: function (response) {

                }
            });
            $("#chkselectAllPartListPackage").prop("checked", false);
            $("#popupAddQuotationPartList .modal-header").html("เพิ่ม Spare Part");
            $('#popupAddQuotationPartList').modal('show');
        }

        function selectedPartList(s, e) {
            s
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddPartListData",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    //var data = response.d;
                    //$('#lbMaintenanceModel').html(data[0].model_name);
                    //gridViewMaintenancePackage.PerformCallback();
                    //$('#dvMaintenanceModel').css('display', 'none');
                    //$('#dvMaintenancePackage').css('display', '');
                },
                failure: function (response) {

                }
            });
        }

        function submitPartList() {
            $('#txtDiscountByItem').val(0);
            var quotationType = $('#cbbQuotationType').val();

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SubmitPartList",
                data: '{quotationType: "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridQuotationDetail.PerformCallback();
                    calDiscount();
                    //$('#btnAddPartList').css('display', '');
                    //$('#btnAddAllPartList').css('display', '');
                    $('#popupAddQuotationPartList').modal('hide');
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function clearPartList() {

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ClearPartList",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridQuotationDetail.PerformCallback();
                },
                failure: function (response) {

                }
            });
        }

        function popupSparePart() {
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetSparePartData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewSparepart.PerformCallback();
                },
                failure: function (response) {

                }
            });
            $('#popupAddQuotationSparePart').modal('show');
        }

        function selectedSparePart(s, e) {
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddSparePartData",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        function submitSparePart() {
            $('#txtDiscountByItem').val(0);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SubmitSparePart",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    //$('#btnAddPartList').css('display', '');
                    $('#popupAddQuotationSparePart').modal('hide');
                    gridQuotationDetail.PerformCallback();
                    calDiscount();
                },
                failure: function (response) {

                }
            });
        }

        function CancelSparePart() {
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/CancelSparePart",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#popupAddQuotationSparePart').modal('hide');
                },
                failure: function (response) {

                }
            });
        }

        function moveUpProductDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/moveUpProductDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridQuotationDetail.PerformCallback();
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function moveDownProductDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/moveDownProductDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridQuotationDetail.PerformCallback();
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function selectAnnualServiceItem(s, e) {
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddAnnualService",
                data: '{id: "' + key + '", type : "product"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        function selectAnnualServiceHistoryItem(s, e) {
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddAnnualService",
                data: '{id: "' + key + '" , type : "history"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                failure: function (response) {

                }
            });
        }
        function selectAllAnnualServiceHistoryItem(s, e) {
            var value = s.GetChecked();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SelectAllAnnualService",
                data: '{isSelect : ' + value + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridviewAnnualServiceHistory.PerformCallback();
                },
                failure: function (response) {

                }
            });
        }

        function submitAnnualService() {
            $('#txtDiscountByItem').val(0);
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SubmitAnnualService",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridQuotationDetail.PerformCallback();
                    calDiscount();
                    //$('#btnAddPartList').css('display', '');
                    $('#popupAddQuotationAnnualService').modal('hide');
                },
                failure: function (response) {

                }
            });
        }
        /*END FUNCTION QUOTATION DETAIL*/

        /*HEADER FUNCTION*/
        function changedCustomer() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbCustomerID.GetValue();
            var quotationType = $('#cbbQuotationType').val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetCustomerData",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onChangedCustomerSuccess,
                failure: function (response) {

                }
            });
            if (quotationType == "A") {
                $.ajax({
                    type: "POST",
                    url: "Quotation.aspx/GetAnnualServiceHistory",
                    data: '{customerId: "' + key + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        setTimeout(function () {
                            gridViewAnnualServiceItem.PerformCallback();
                            gridviewAnnualServiceHistory.PerformCallback();
                        }, 100);
                        //var data = response.d;
                        //$('#txtSubject').val(data);
                    },
                    failure: function (response) {

                    }
                });
            }
            $.LoadingOverlay("hide");

            $('#txtShowMFG').val('');
        }

        function changedCustomerModel() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbModel.GetValue();
            var text = cbbModel.GetText().split(';');
            $('#hdModelName').val(text[0]);

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetModelData",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#txtShowMFG').val(response.d);
                },
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");

        }


        function onChangedCustomerSuccess(response) {
            $('#txtAddress').val(response.d.customer_address);
            //$('#txtAttention').val(response.d.contract_name);
            $('#txtAttentionTel').val("");
            $('#txtAttentionEmail').val("");


            gridViewOrderHistory.PerformCallback();
            cbbCustomerAttention.PerformCallback();
            cbbModel.PerformCallback();
        }

        function changedQuotationType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var quotationType = $('#cbbQuotationType').val();

            clearDataSetQuotationDetail();

            $('#cbDiscountByItem').prop('checked', false);
            $("#cbbDiscountByItem").prop("disabled", true);
            $('#cbDiscountByItem').prop('disabled', false);

            $('#btnAddPartList').css('display', 'none');
            $('#btnAddAllPartList').css('display', 'none');

            //  Clear model package
            $('#lblModelPackage').html('');
            /*cbbSubject.ClearItems();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetConfig",
                data: '{quotationType: "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    //$('#txtSubject').val(data);
                    console.log(data);
                    for (i = 0; i < data.length; i++) {
                        //cboProvince.AddItem("--โปรดเลือก--", "0");
                        cbbSubject.AddItem(data[i].document_description.toString(), data[0].id.toString());
                    }
                },
                failure: function (response) {

                }
            });*/
            cbbSubject.PerformCallback();
            // clear all remark
            for (var i = 0; i < 7; i++) {
                var remarkStr = "txtRemark" + (i + 1);
                $('#' + remarkStr).val("");

            }
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetRemark",
                data: '{quotationType: "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    for (var i = 0; i < data.length; i++) {
                        var remarkStr = "txtRemark" + (i + 1);
                        $('#' + remarkStr).val(data[i]);

                    }
                },
                failure: function (response) {

                }
            });

            setTimeout(function () {

                if (quotationType == "C") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');
                    $('#btnAddQuotationDetail').inner
                    $("#btnAddQuotationDetail").html('Service / Overhaul Motor');

                }
                if (quotationType == "D") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');
                    $("#btnAddQuotationDetail").html('เพิ่มสินค้า');


                }
                else if (quotationType == "O") {
                    $('#contactor').css('display', '');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', 'none');
                    $("#btnAddQuotationDetail").html('เพิ่มสินค้า');


                }
                else if (quotationType == "P") {
                    $('#contactor').css('display', '');
                    $('#dvIsService').css('display', 'none');
                    $('#dvGridProduct').css('display', ''); // HIDE DIV GRID
                    $('#dvGridService').css('display', 'none'); // SHOW DIV GRID
                    $('#dvIsProductDescription').css('display', '');
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', 'none');
                    $("#btnAddQuotationDetail").html('เพิ่มสินค้า');
                }
                else if (quotationType == "S") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');

                    $('#btnAddAllPartList').css('display', '');
                    $("#btnAddQuotationDetail").html('เลือก Model');
                }
                else if (quotationType == "M") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');

                    $('#btnAddPartList').css('display', '');
                    $('#btnAddAllPartList').css('display', '');
                    $("#btnAddQuotationDetail").html('เลือก Model');
                }
                else if (quotationType == "A") {
                    //if (cbbCustomerID.GetValue() == null) {

                    //    return;
                    //}
                    if (cbbCustomerID.GetValue() != null) {
                        $.ajax({
                            type: "POST",
                            url: "Quotation.aspx/GetAnnualServiceHistory",
                            data: '{customerId: "' + cbbCustomerID.GetValue() + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                setTimeout(function () {
                                    gridViewAnnualServiceItem.PerformCallback();
                                    gridviewAnnualServiceHistory.PerformCallback();
                                }, 100);
                                //var data = response.d;
                                //$('#txtSubject').val(data);
                                console.log("response AN = " + response.d);
                            },
                            failure: function (response) {

                            }
                        });
                    }
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService').css('display', '');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#dvGridProduct').css('display', ''); // SHOW DIV GRID
                    $('#dvGridService').css('display', 'none'); // HIDE DIV GRID

                    $('#txtDiscountAmountItem').prop("readonly", true);
                    $('#txtDiscountPercentItem').prop("readonly", true);
                    $('#txtDiscountAmountItem').val(0);
                    $('#txtDiscountPercentItem').val(0);
                    $('#tabOtherDetail').css('display', '');
                    //$('#cbDiscountByItem').attr('checked', false);
                    //$('#cbDiscountByItem').prop('disabled', true);
                    //$("#cbbDiscountByItem").prop("disabled", false);
                    $("#btnAddQuotationDetail").html('เลือกรายการ');
                }
                $.LoadingOverlay("hide");
            }, 200);


        }
        function changedQuotationType1() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var quotationType = $('#cbbQuotationType').val();

            clearDataSetQuotationDetail();

            $('#cbDiscountByItem').prop('checked', false);
            $("#cbbDiscountByItem").prop("disabled", true);
            $('#cbDiscountByItem').prop('disabled', false);

            $('#btnAddPartList').css('display', 'none');
            $('#btnAddAllPartList').css('display', 'none');

            for (var i = 0; i < 7; i++) {
                var remarkStr = "txtRemark" + (i + 1);
                $('#' + remarkStr).val("");

            }
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/GetRemark",
                data: '{quotationType: "' + quotationType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    for (var i = 0; i < data.length; i++) {
                        var remarkStr = "txtRemark" + (i + 1);
                        $('#' + remarkStr).val(data[i]);

                    }
                },
                failure: function (response) {

                }
            });

            setTimeout(function () {

                if (quotationType == "C") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');


                }
                if (quotationType == "D") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');


                }
                else if (quotationType == "O") {
                    $('#contactor').css('display', '');
                    $('#dvIsService').css('display', '');
                    $('#dvGridProduct').css('display', 'none'); // HIDE DIV GRID
                    $('#dvGridService').css('display', ''); // SHOW DIV GRID
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', 'none');


                }
                else if (quotationType == "P") {
                    $('#contactor').css('display', '');
                    $('#dvIsService').css('display', 'none');
                    $('#dvGridProduct').css('display', ''); // HIDE DIV GRID
                    $('#dvGridService').css('display', 'none'); // SHOW DIV GRID
                    $('#dvIsProductDescription').css('display', '');
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', 'none');

                }
                else if (quotationType == "S") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsAnnualService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');


                }
                else if (quotationType == "M") {
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#tabOtherDetail').css('display', '');

                    $('#btnAddPartList').css('display', '');
                    $('#btnAddAllPartList').css('display', '');
                }
                else if (quotationType == "A") {
                    //if (cbbCustomerID.GetValue() == null) {

                    //    return;
                    //}
                    if (cbbCustomerID.GetValue() != null) {
                        $.ajax({
                            type: "POST",
                            url: "Quotation.aspx/GetAnnualServiceHistory",
                            data: '{customerId: "' + cbbCustomerID.GetValue() + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                setTimeout(function () {
                                    gridViewAnnualServiceItem.PerformCallback();
                                    gridviewAnnualServiceHistory.PerformCallback();
                                }, 100);
                                //var data = response.d;
                                //$('#txtSubject').val(data);
                            },
                            failure: function (response) {

                            }
                        });
                    }
                    $('#contactor').css('display', 'none');
                    $('#dvIsService').css('display', 'none');
                    $('#dvIsProductDescription').css('display', 'none');
                    $('#dvIsAnnualService').css('display', '');
                    $('#dvIsAnnualService_Discount').css('display', '');

                    $('#txtDiscountAmountItem').prop("readonly", true);
                    $('#txtDiscountPercentItem').prop("readonly", true);
                    $('#txtDiscountAmountItem').val(0);
                    $('#txtDiscountPercentItem').val(0);
                    $('#tabOtherDetail').css('display', '');
                    //$('#cbDiscountByItem').attr('checked', false);
                    //$('#cbDiscountByItem').prop('disabled', true);
                    //$("#cbbDiscountByItem").prop("disabled", false);
                }

            }, 200);

            $.LoadingOverlay("hide");
        }
        function changedStatusQuotation() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var StatusQuotation = $('#StatusQuotation').val();

            setTimeout(function () {

                if (StatusQuotation == "CC" || StatusQuotation == "LO") {
                    $('#StatusQu').css('display', '');
                }
                else {
                    $('#StatusQu').css('display', 'none');
                }

            }, 100);

            $.LoadingOverlay("hide");

        }


        function changeProductDescription() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            gridQuotationDetail.PerformCallback();
            productSetGrid2.PerformCallback();

            $.LoadingOverlay("hide");
        }

        function clearDataSetQuotationDetail() {
            var quStatus = $("#quotationStatus").val();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ClearDataQuotationDetail",
                data: '{quStatus: "' + quStatus + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onClearDataSuccess,
                failure: function (response) {

                }
            });

            cbbModel.SetValue("")
            $('#txtShowMFG').val("");
            $('#txtMachine').val("");
            $('#txtHour').val("");
            $('#txtPressure').val("");
            $('#txtLocation1').val("");
            $('#txtLocation2').val("");
            $('#div-content .nav-tabs li:first-child > a').click();
        }

        function onClearDataSuccess() {

            calDiscount();
            gridQuotationDetail.PerformCallback();
            gridViewService.PerformCallback();

        }

        function confirmSave() {

            if (cbbCustomerID.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");
                return;
            }

            else if ($('#txtQuatationDate').val() == "") {
                swal("กรุณาใส่วันที่เอกสาร");
                return;
            }
            var quotationNo = $('#txtQuatationNo').val();
            //if (quotationNo != "") {
            //    var confirm_true = confirm("ต้องการยืนยันเอกสาร " + quotationNo + "ใช่หรือไม่?", 'btnConfirm');
            //    if (confirm_true) {
            //        $('#btnConfirm').click();
            //    }
            //}
            //else {
            //    $('#btnConfirm').click();
            //}

            sendApproveMessage(quotationNo, 'btnConfirm', 'ส่งเอกสาร');

        }

        function saveDraft() {
            if (cbbCustomerID.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");
                return;
            }
            var quotationType = $('#cbbQuotationType').val();
            if ($("#txtContractNo").val() == "" && quotationType == 'A') {
                $("#txtContractNo").parent().parent().addClass("has-error");
                $("#txtContractNo").focus();
                return false;
            }

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/ValidateData",
                data: '{type : ""}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        $('#btnSaveDraft').click();
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
            //openReport('Quotation', 4);
            var QuotationType = $("#cbbQuotationType").val();

            //console.log(QuotationType + " :: " + $('#cbDiscountByItem').is(':checked'));
            var quotation_no = $("#txtQuatationNo").val();
            var isDiscount = $('#cbDiscountByItem').is(':checked');
            var is_net = $('#cbIsNet').is(':checked');
            //console.log("is_net=" + is_net);

            if (!is_net) {
                if (QuotationType == "P" && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Discount_Item_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "P" && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                }
                else if (QuotationType == "A" && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Discount_Item_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "A" && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "S" && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Discount_Item_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "S" && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "M" && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Discount_Item_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "M" && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Report|" + quotation_no, "_blank");
                } else if ((QuotationType == "C" || QuotationType == "O") && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Discount_Item_Report|" + quotation_no, "_blank");
                } else if ((QuotationType == "C" || QuotationType == "O") && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "D" && isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Discount_Item_Report|" + quotation_no, "_blank");
                } else if (QuotationType == "D" && !isDiscount) {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                }
            }
            else {

                if (QuotationType == "S") {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Report|" + quotation_no, "_blank");
                }
                else if (QuotationType == "P") {
                    //window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                    if (isDiscount) {
                        window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Discount_Item_Report|" + quotation_no, "_blank");
                    } else {
                        window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                    }
                }
                else if (QuotationType == "A") {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                }
                else if (QuotationType == "M") {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_PartNo_Report|" + quotation_no, "_blank");
                }
                else if (QuotationType == "C" || QuotationType == "O") {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                }
                else if (QuotationType == "D") {
                    window.open("../Report/DocumentViewer.aspx?ReportArgs=Quotaion_Description_Report|" + quotation_no, "_blank");
                }
            }

        }

        function backPage() {
            window.location.href = "../Master/QuotationList.aspx";
        }
        function addCustomerAttention() {
            if (cbbCustomerID.GetValue() == null) {
                $.LoadingOverlay("hide");
                swal("กรุณาเลือกลูกค้า");

                return;
            }

            $('#txtCustomerAttentionName').val("");
            $('#txtCustomerAttentionTel').val("");
            $('#txtCustomerAttentionEmail').val("");

            $('#modal_customer_attention').modal('show');
        }

        function AddDataModel() {
            if (cbbCustomerID.GetValue() == null) {
                $.LoadingOverlay("hide");
                swal("กรุณาเลือกลูกค้า");

                return;
            }
            $('#txtDataModel').val("");
            $('#txtDataMfg').val("");
            $('#modal_model_customer').modal('show');
        }

        function submitCustomerAttention() {
         
            if ($("#txtCustomerAttentionTel").val() == "") {
                $("#txtCustomerAttentionTel").parent().parent().addClass("has-error");
                $("#txtCustomerAttentionTel").addClass("active in");
                $("#txtCustomerAttentionTel").focus();

            }
             else if ($("#txtCustomerAttentionTel").val() != "" && $("#txtCustomerAttentionTel").val().length < 9) {
                alertError('Please input Customer Attention Tel minumum 9-10 digit');
                $("#txtCustomerAttentionTel").focus();

            }
            else {
                   $.LoadingOverlay("show", {
                zIndex: 9999
            });

                var attention_name = $('#txtCustomerAttentionName').val();
                var attention_tel = $('#txtCustomerAttentionTel').val();
                var attention_email = $('#txtCustomerAttentionEmail').val();
                var customer_id = cbbCustomerID.GetValue();
                $.ajax({
                    type: "POST",
                    url: "Quotation.aspx/AddCustomerAttention",
                    data: "{customer_id : '" + customer_id + "' , name : '" + attention_name + "' , tel : '" + attention_tel + "' , email : '" + attention_email + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        $('#txtCustomerAttentionName').val("");
                        $('#txtCustomerAttentionTel').val("");
                        $('#txtCustomerAttentionEmail').val("");
                        $('#txtAttentionTel').val("");
                        $('#txtAttentionEmail').val("");

                        cbbCustomerAttention.PerformCallback();

                        $('#modal_customer_attention').modal('hide');

                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {

                    }
                });
            }

        }

        function submitCustomerModel() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var model = $('#txtDataModel').val();
            var mfg = $('#txtDataMfg').val();
            var customer_id = cbbCustomerID.GetValue();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddCustomerModel",
                data: "{customer_id : '" + customer_id + "' , model : '" + model + "', mfg : '" + mfg + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#txtDataModel').val("");
                    $('#txtDataMfg').val("");
                    cbbModel.PerformCallback();
                    $('#modal_model_customer').modal('hide');
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                }
            });
        }



        function changedCustomerAttention() {

            var id = cbbCustomerAttention.GetValue();
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SelectCustomerAttention",
                data: "{id : '" + id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    var splitStr = data.split("|");
                    console.log(splitStr);
                    if (splitStr.length > 0) {
                        $('#txtAttentionTel').val(splitStr[0]);
                        $('#txtAttentionEmail').val(splitStr[1]);
                    }

                },
                failure: function (response) {

                }
            });
        }
        function viewQu(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var key = e;

            window.location.href = "Quotation.aspx?dataId=" + key;
            $.LoadingOverlay("hide");
        }
        function searchProductGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                var quotationType = $("#cbbQuotationType").val();
                productSetGrid2.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchSparePartGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchSparePart").val();
                var quotationType = $("#cbbQuotationType").val();
                gridViewSparepart.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchAnnualServiceGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchAnnualService").val();
                var quotationType = $("#cbbQuotationType").val();
                gridviewAnnualServiceHistory.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchMaintenanceGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchMaintenance").val();
                var quotationType = $("#cbbQuotationType").val();
                gridViewMaintenance.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }
        function searchPartListGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var key = (isAllPartSearch ? 0 : $('#hdMaintenanceModelId').val());
                var txtSearch = $("#txtSearchPartList").val();
                var quotationType = $("#cbbQuotationType").val();
                gridViewPartList.PerformCallback("Search|" + quotationType + "|" + txtSearch.toString() + "|" + key);

                $.LoadingOverlay("hide");
            }
        }
        function popupViewDetail(id) {
            $('#hdSelectedParentId').val(id);
            gridViewServiceDetail.PerformCallback(id);
            $('#popupServiceDetailGridview').modal("show");
        }
        function sendDocument(status) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var quotationNo = $('#txtQuatationNo').val();
            if (status == "AP") {
                sendApproveMessage(quotationNo, 'btnConfirm', 'อนุมัติเอกสาร');
            }
            else if (status == "RE") {
                sendApproveMessage(quotationNo, 'btnReject', 'ปฎิเสธเอกสาร');
            }
            else if (status == "LO") {
                sendApproveMessage(quotationNo, 'btnLost', 'เอกสารหาย');
            }
            else if (status == "CC") {
                sendApproveMessage(quotationNo, 'btnCancel', 'ยกเลิกเอกสาร');
                //onclickcancelquotation(quotationNo);
            }
            else if (status == "PO") {
                sendApproveMessage(quotationNo, 'btnPO', 'PO เอกสาร');
            }
            $.LoadingOverlay("hide");
        }

        function onclickcancelquotation(quono) {
            $("#lblmsgcnacel").html("ยืนยัน ยกเลิกเอกสาร " + quono + " ใช่หรือไม่ ? ");
            $("#modal_message_cancel").modal("show");
        }

        function addNewItem() {
            window.location.href = "Quotation.aspx";
        }

        function submitcancel() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "Quotation.aspx/AddCustomerAttention",
                data: "{customer_id : '" + customer_id + "' , name : '" + attention_name + "' , tel : '" + attention_tel + "' , email : '" + attention_email + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#txtCustomerAttentionName').val("");
                    $('#txtCustomerAttentionTel').val("");
                    $('#txtCustomerAttentionEmail').val("");
                    $('#txtAttentionTel').val("");
                    $('#txtAttentionEmail').val("");

                    cbbCustomerAttention.PerformCallback();

                    $('#modal_customer_attention').modal('hide');

                    $.LoadingOverlay("hide");
                },
                failure: function (response) {

                }
            });

        }

        function SelectAllPartList() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var selected = $('#chkselectAllPartListPackage').is(':checked');
            $.ajax({
                type: "POST",
                url: "Quotation.aspx/SelectAllPartList",
                data: '{selected : ' + selected + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPartList.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                }
            });
        }

    </script>
    <div id="div-content">
        <div class="col-lg-12">
            <div class="form-inline">
                <button type="button" runat="server" onclick="addNewItem()" id="btnNew" class="btn-addItem">
                    <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;New
                </button>
                <button type="button" runat="server" id="btnDraft" onclick="saveDraft()" class="btn-addItem">
                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                </button>
                <button type="button" runat="server" id="btnSave" onclick="confirmSave()" class="btn-addItem">
                    <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Send to Approve
                </button>
                <button type="button" runat="server" id="btnReportClient" onclick="callReport()" class="btn-addItem noDisabled">
                    <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Report
                </button>
                <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                    <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;Quotation List
                </button>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnSaveDraft" Style="visibility: hidden" UseSubmitBehavior="false" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnConfirm" Style="visibility: hidden" UseSubmitBehavior="false" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnCancel" Style="visibility: hidden" UseSubmitBehavior="False" Text="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnLost" Style="visibility: hidden" UseSubmitBehavior="false" Text="Lost" OnClick="btnLost_Click"></dx:ASPxButton>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnPO" Style="visibility: hidden" UseSubmitBehavior="false" Text="PO" OnClick="btnPO_Click"></dx:ASPxButton>
                <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnReject" Style="visibility: hidden" UseSubmitBehavior="false" Text="Reject" OnClick="btnReject_Click"></dx:ASPxButton>
                <div id="dvApprove1Button" runat="server" class="form-inline">
                    <button type="button" runat="server" id="Button12" onclick="sendDocument('AP')" class="btn-addItem noDisabled">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Approve
                    </button>
                    <button type="button" runat="server" id="Button25" onclick="sendDocument('RE')" class="btn-addItem noDisabled">
                        <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Reject
                    </button>
                </div>
                <div id="dvApprove2Button" runat="server" class="form-inline">
                    <button type="button" runat="server" id="Button26" onclick="sendDocument('LO')" class="btn-addItem noDisabled">
                        <i class="fa fa-trash" aria-hidden="true"></i>&nbsp;Lost
                    </button>
                    <button type="button" runat="server" id="Button27" onclick="sendDocument('CC')" class="btn-addItem noDisabled">
                        <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Cancel
                    </button>
                    <button type="button" runat="server" id="Button28" onclick="sendDocument('PO')" class="btn-addItem noDisabled">
                        <i class="fa fa-file" aria-hidden="true"></i>&nbsp;PO
                    </button>
                </div>
            </div>
        </div>
        <div class="row">
            <fieldset>
                <legend>Header</legend>
                <div class="col-xs-5 no-padding">
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">ลูกค้า :</label>
                        <div class="col-xs-10 no-padding">
                            <dx:ASPxComboBox ID="cbbCustomerID" CssClass="form-control" runat="server" ClientInstanceName="cbbCustomerID" TextField="data_text"
                                ClientSideEvents-ValueChanged="changedCustomer" EnableCallbackMode="true"
                                ValueField="data_value">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">ที่อยู่ :</label>
                        <div class="col-xs-10 no-padding">
                            <input type="text" class="form-control" id="txtAddress" readonly="true" runat="server" />
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">Attention :</label>
                        <div class="col-xs-9 no-padding">
                            <dx:ASPxComboBox ID="cbbCustomerAttention" CssClass="form-control" runat="server" ClientInstanceName="cbbCustomerAttention" TextField="attention_name"
                                ClientSideEvents-ValueChanged="changedCustomerAttention" EnableCallbackMode="true" OnCallback="cbbCustomerAttention_Callback"
                                ValueField="id">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-xs-1 no-padding">
                            <button type="button" runat="server" id="Button19" onclick="addCustomerAttention()" class="form-control">
                                <i class="fa fa-plus-circle" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">Email :</label>
                        <div class="col-xs-4 no-padding">
                            <input type="text" class="form-control" id="txtAttentionEmail" runat="server" />
                        </div>
                        <label class="col-xs-2 text-right label-rigth">Tel :</label>
                        <div class="col-xs-4 no-padding">
                            <input type="text" class="form-control" id="txtAttentionTel" runat="server" />
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">Subject :</label>
                        <div class="col-xs-10 no-padding">
                            <%--<input type="text" class="form-control" id="txtSubject" runat="server" />--%>
                            <dx:ASPxComboBox runat="server" ID="cbbSubject" ClientInstanceName="cbbSubject" CssClass="form-control" OnCallback="cbbSubject_Callback"></dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-xs-2 text-right label-rigth">Project :</label>
                        <div class="col-xs-10 no-padding">
                            <input type="text" class="form-control" id="txtProject" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-3 no-padding" style="padding-left: 1%">
                    <div class="row form-group">
                        <div class="col-xs-6 form-inline">
                            <label>วันที่เอกสาร :</label>
                        </div>
                        <div class="col-xs-6 form-inline no-padding">
                            <input type="text" class="form-control" style="width: 70%;" id="txtQuatationDate" autocomplete="off" runat="server" disabled="disabled" readonly="true" />
                        </div>
                    </div>
                    <div class="row form-group" id="dvIsAnnualService_Discount" runat="server">
                        <div class="col-xs-6 form-inline" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbDiscountByItem" enableviewstate="false" onclick="onCheckedDiscountByItem()" value="" />&nbsp;ส่วนลดแบบรายการ
                            </label>
                        </div>
                        <div class="col-xs-2 no-padding">
                            <input type="text" class="form-control numberic" id="txtDiscountByItem" enableviewstate="false" onchange="onDiscountByItemTextChange()" runat="server" />
                        </div>
                        <div class="col-xs-4 no-padding">
                            <select class="form-control" runat="server" id="cbbDiscountByItem" onchange="selectOnCheckedDiscountByItem()" style="width: 80px;">
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-xs-6 form-inline" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbDiscountBottomBill1" enableviewstate="false" onclick="onCheckedDiscountBottomBill1()" value="" />&nbsp;ส่วนลดแบบท้ายบิล 1
                            </label>
                        </div>
                        <div class="col-xs-2 no-padding">
                            <input type="text" class="form-control numberic" id="txtDiscountBottomBill1" enableviewstate="false" onchange="onCheckedDiscountBottomBill1()" runat="server" />
                        </div>
                        <div class="col-xs-4 no-padding">
                            <select class="form-control" runat="server" id="cbbDiscountBottomBill1" onchange="selectOnCheckedDiscountBottomBill1()" style="width: 80px;">
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                        </div>

                    </div>
                    <div class="row form-group">
                        <div class="col-xs-6 form-inline" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbDiscountBottomBill2" enableviewstate="false" onclick="onCheckedDiscountBottomBill2()" value="" />&nbsp;ส่วนลดแบบท้ายบิล 2
                            </label>
                        </div>
                        <div class="col-xs-2 no-padding">
                            <input type="text" class="form-control numberic" id="txtDiscountBottomBill2" enableviewstate="false" onchange="onCheckedDiscountBottomBill2()" runat="server" />
                        </div>
                        <div class="col-xs-4 no-padding">
                            <select class="form-control" runat="server" id="cbbDiscountBottomBill2" onchange="selectOnCheckedDiscountBottomBill2()" style="width: 80px;">
                                <option value="P">%</option>
                                <option value="A">จำนวนเงิน</option>
                            </select>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-xs-4 form-inline" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbShowVat" value="" />&nbsp; VAT
                            </label>
                        </div>
                        <div class="col-xs-5 form-inline" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbIsNet" value="" />&nbsp; ราคา Net
                            </label>
                        </div>
                        <div class="col-xs-8 form-inline" runat="server" id="contactor" style="padding: 0 5px;">
                            <label>
                                <input type="checkbox" runat="server" id="cbContactor" value="" />&nbsp; ลูกค้า Contactor 
                            </label>
                        </div>
                    </div>

                </div>
                <div class="col-xs-3 no-padding">
                    <div class="row form-group">
                        <div class="col-xs-5 form-inline label-rigth text-right">
                            <label>เลขที่ :</label>
                        </div>
                        <div class="col-xs-6 form-inline">
                            <input type="text" class="form-control txt-no" id="txtQuatationNo" readonly="true" runat="server" />
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-xs-5 form-inline label-rigth text-right">
                            <label>ประเภทเอกสาร :</label>
                        </div>
                        <div class="col-xs-6 form-inline">
                            <select class="form-control" runat="server" id="cbbQuotationType" onchange="changedQuotationType()" style="width: 115px;">
                                <option value="P">Product</option>
                                <option value="S">Spare part</option>
                                <option value="M">Maintenance</option>
                                <option value="C">Service Charge</option>
                                <option value="A">Annual Service</option>
                                <option value="D">Design</option>
                                <option value="O">Other</option>
                            </select>
                        </div>
                    </div>
                    <div class="row form-group" runat="server" id="dvIsService">
                        <label class="col-xs-5 label-rigth text-right">Service Type :</label>
                        <div class="col-xs-6 text-left no-padding">
                            <input type="radio" runat="server" value="1" onchange="changedQuotationType1()" id="rdoLumpsum" name="serviceType">&nbsp;เหมา
                            <br />
                            <input type="radio" runat="server" value="0" onchange="changedQuotationType1()" id="rdoNotLumpsum" name="serviceType">&nbsp;แยก
                        </div>
                    </div>
                    <div class="row form-group" runat="server" id="dvIsProductDescription">
                        <label class="col-xs-5 label-rigth text-right">Description Type :</label>
                        <div class="col-xs-6 text-left no-padding">
                            <input type="radio" runat="server" value="0" onchange="changeProductDescription()" id="rdoQuotationLine" name="productDesc">&nbsp;แสดงรายละเอียดเต็ม
                            <br />
                            <input type="radio" runat="server" value="1" onchange="changeProductDescription()" id="rdoDescription" name="productDesc">&nbsp;แสดงรายละเอียดปกติ
                        </div>
                    </div>
                    <div class="row form-group" runat="server" id="dvIsAnnualService">
                        <label class="col-xs-5 label-rigth text-right">เลขที่สัญญา :</label>
                        <div class="col-xs-6 text-left no-padding">
                            <input type="text" class="form-control" id="txtContractNo" runat="server" validate-data />
                        </div>
                    </div>
                    <div class="row form-group" runat="server" id="dvCloneQuotation">
                        <label class="col-xs-5 label-rigth text-right">Ref Quotation :</label>
                        <div class="col-xs-6 text-left">
                            <input type="text" readonly="" class="form-control" id="txtCloneQuotation" runat="server" />
                        </div>
                        <div class="row"></div>
                        <label class="col-xs-5 label-rigth text-right">Revision :</label>
                        <div class="col-xs-6 text-left">
                            <input type="text" readonly="" class="form-control" id="txtRevision" runat="server" />
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-xs-5 form-inline label-rigth text-right">
                            <label>สกุลเงิน :</label>
                        </div>
                        <div class="col-xs-6 form-inline">
                            <select class="form-control" runat="server" id="Cbbcurrency" style="width: 115px;">
                                <option value="1">Thai</option>
                                <option value="2">Yen</option>
                                <option value="3">Dollar</option>
                                <option value="4">Singapore Dollar</option>
                                <option value="5">Ringgit</option>
                            </select>
                        </div>
                    </div>


                </div>
            </fieldset>
        </div>
        <div class="modal fade" id="modal_customer_attention" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Add Customer Attention
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Attention Name :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtCustomerAttentionName" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Attention Tel :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtCustomerAttentionTel" maxlength="10" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Attention Email :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtCustomerAttentionEmail" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="">
                            <button type="button" runat="server" id="Button17" onclick="submitCustomerAttention()" class="btn-app btn-addItem">ยืนยัน</button>
                            <button type="button" runat="server" id="Button18" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_model_customer" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        Add Customer Model
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">Model :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtDataModel" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">MFG :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" id="txtDataMfg" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="">
                            <button type="button" runat="server" id="Button20" onclick="submitCustomerModel()" class="btn-app btn-addItem">ยืนยัน</button>
                            <button type="button" runat="server" id="Button21" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="row form-group">
                <div class="col-xs-5"></div>
                <div class="col-xs-7 no-padding">
                    <div class="form-inline text-right">
                        <div class="form-group">
                            <label for="txtSumDiscount1">ยอดรวมก่อนลด :</label>
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
        <div class="row">
            <ul class="nav nav-tabs" style="margin-top: -35px;">
                <li class="active"><a data-toggle="tab" href="#productList">รายการ</a></li>
                <li id="tabOtherDetail" runat="server"><a data-toggle="tab" href="#otherDetail">รายละเอียดเพิ่มเติม</a></li>
                <li><a data-toggle="tab" href="#remark">หมายเหตุ</a></li>

                <li><a data-toggle="tab" href="#orderHistory">ประวัติสั่งซื้อ</a></li>
                <li><a data-toggle="tab" href="#notice">แจ้งเตือน</a></li>
                <%--<li><a data-toggle="tab" href="#freebies">ของแถม</a></li>--%>
            </ul>
            <div id="quotation-detail-content" class="tab-content" style="overflow: auto;">
                <div id="productList" class="tab-pane fade in active">
                    <div class="row">
                        <div class="col-xs-12">
                            <button type="button" runat="server" id="btnAddQuotationDetail" class="btn-addItem" onclick="popupQuotationDetail()" style="margin-bottom: 5px;">
                                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เลือก Model
                            </button>
                            <button type="button" runat="server" id="btnAddPartList" class="btn-addItem" onclick="popupPartList()">
                                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม PartList ย่อย
                            </button>
                            <button type="button" runat="server" id="btnAddAllPartList" class="btn-addItem" onclick="popupAllPartList()">
                                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม Spare Part
                            </button>
                            <asp:Label ID="lblModelPackage" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-xs-12" runat="server" id="dvGridProduct">
                            <!-- Grideview -->
                            <dx:ASPxGridView ID="gridQuotationDetail" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridQuotationDetail"
                                Width="100%" KeyFieldName="id" EnableCallBacks="true"
                                EnableRowsCache="false"
                                SettingsBehavior-AllowSort="false"
                                OnCustomCallback="gridQuotationDetail_CustomCallback">
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
                                    <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                                        <DataItemTemplate>

                                            <a id="btnEdit" class="btn btn-mini" onclick="editProductDetail(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                            </a>
                                            |
                                         
                                            <a id="btnDelete" class="btn btn-mini" onclick="deleteProductDetail(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i></a>

                                            <a id="btnMoveUp" class="btn btn-mini" onclick="moveUpProductDetail(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                            </a>


                                            |
                                         
                                            <a id="btnMoveDown" class="btn btn-mini" onclick="moveDownProductDetail(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                    <dx:GridViewDataTextColumn FieldName="model_id" Caption="model_id" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                    <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" ReadOnly="True" Visible="true" Width="60px" VisibleIndex="6" />
                                    <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Description" Width="150px" VisibleIndex="6" />
                                    <dx:GridViewDataTextColumn FieldName="quotation_line" Caption="Quotation Line" ReadOnly="True" Visible="true" Width="150px" VisibleIndex="6" />
                                    <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" ReadOnly="True" Visible="true" Width="60px" VisibleIndex="6" />
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="qty" Caption="Qty" VisibleIndex="7" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" ReadOnly="True" Visible="true" Width="30px" VisibleIndex="7" />
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="unit_price" Caption="Unit Price" VisibleIndex="7" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="discount_amount" Caption="Discount (THB)" VisibleIndex="7" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="discount_percentage" Caption="Discount (%)" VisibleIndex="7" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>

                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="total_amount" Caption="Total Amount" VisibleIndex="7" Width="60px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                                <SettingsPopup>
                                    <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                </SettingsPopup>
                                <%--<ClientSideEvents BeginCallback="gridViewFreeBie_Callback" />--%>
                            </dx:ASPxGridView>
                        </div>
                        <div class="col-xs-12" runat="server" id="dvGridService" style="height: 450px">
                            <!-- Grideview -->
                            <dx:ASPxGridView ID="gridViewService" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewService"
                                Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewService_CustomCallback"
                                SettingsBehavior-AllowSort="false">
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
                                    <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40px">
                                        <DataItemTemplate>

                                            <%--  <a id="btnAdd" class="btn btn-mini" onclick="popupServiceDetail(<%# Eval("id")%>)" title="Add Detail">
                                                <i class="fa fa-plus" aria-hidden="true"></i>
                                            </a>
                                            |--%>

                                            <a id="btnEdit" class="btn btn-mini" onclick="editServiceParent(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                            </a>
                                            |
                                            <a id="btnDelete" class="btn btn-mini" onclick="deleteServiceDetail(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="id" Caption="View Detail" ReadOnly="True" Width="40px" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                        <DataItemTemplate>
                                            <a id="btnViewDetail" class="btn btn-mini" onclick="popupViewDetail(<%# Eval("id")%>)" title="View">
                                                <i class="fa fa-list" aria-hidden="true"></i>
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="2" />
                                    <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Description" Width="120px" VisibleIndex="3" />
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="unit_price" Caption="Unit Price" VisibleIndex="4" Width="50px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="qty" Caption="Qty" VisibleIndex="5" Width="30px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" Width="30px" VisibleIndex="6" />

                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                        FieldName="total_amount" Caption="Total Amount" VisibleIndex="7" Width="50px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>


                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                        FieldName="discount_amount" Caption="Discount (THB)" VisibleIndex="8" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>

                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                        FieldName="discount_percentage" Caption="Discount (%)" VisibleIndex="9" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>

                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                        FieldName="discount_total" Caption="Discount Total" VisibleIndex="10" Width="40px">
                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>


                                </Columns>

                                <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                    <!-- Modal -->
                    <div class="modal fade" id="popupAddQuotationDetail" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add Product
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
                                            <dx:ASPxGridView ID="gridView2" SettingsBehavior-AllowSort="false"
                                                ClientInstanceName="productSetGrid2" runat="server" Settings-VerticalScrollBarMode="Visible"
                                                Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                                OnCustomCallback="gridView2_CustomCallback" OnHtmlDataCellPrepared="gridView2_HtmlDataCellPrepared"
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
                                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                                        <DataItemTemplate>
                                                            <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                                TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                                                ProductId='<%# Eval("id")%>'>
                                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        getCheckBoxValue(s, e); 
                                                                    }" />
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataColumn FieldName="product_no" Caption="Product No" Width="100px" VisibleIndex="1" />
                                                    <dx:GridViewDataColumn FieldName="product_name_tha" Caption="Product Name" Width="400px" VisibleIndex="2" />
                                                    <dx:GridViewDataColumn FieldName="quotation_line" Caption="Quotation Line" Width="400px" VisibleIndex="3" />

                                                    <%-- <dx:GridViewDataColumn FieldName="description_tha" VisibleIndex="2" />
                                                    <dx:GridViewDataColumn FieldName="unit_eng" VisibleIndex="4" />
                                                    <dx:GridViewDataColumn FieldName="brand_name_tha" VisibleIndex="5" />
                                                    <dx:GridViewDataColumn FieldName="cat_name_tha" VisibleIndex="6" />--%>
                                                </Columns>
                                                <%--<SettingsSearchPanel Visible="true" />--%>
                                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                                            </dx:ASPxGridView>
                                        </div>
                                        <div class="col-xs-12">
                                            <div class="row form-group">
                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button1" onclick="addQuotationDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button2" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationDetailParentService" role="dialog" data-backdrop="static" data-keyboard="false" style="z-index: 5000">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add Service / Overhaul Motor
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="row form-group ui-widget">
                                                <label class="col-xs-3 text-right label-rigth" for="txtDescriptionServiceParent">Description :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" autocomplete="on" id="txtDescriptionServiceParent" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsum">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtQtyServiceParent" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsum2">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Unit Price :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtUnitPriceServiceParent" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsum3">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Unit Type :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <dx:ASPxComboBox ID="cbbUnitType" CssClass="form-control" runat="server" ClientInstanceName="cbbUnitType" TextField="data_text"
                                                        EnableCallbackMode="true" ValueField="data_value">
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsum4">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth" id="lbDiscount"></label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtDiscount" runat="server" />
                                                    <input type="text" disabled="disabled" style="display: none" id="txtDiscountType" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12">
                                            <div class="row form-group">
                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button3" onclick="addServiceParent()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button4" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationDetailServiceDetail" role="dialog" data-backdrop="static" data-keyboard="false" style="z-index: 50000">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add Service Detail
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Description :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" autocomplete="on" id="txtDescriptionServiceDetail" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsumDetail">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtQtyServiceDetail" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsumDetail2">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Unit Price :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtUnitPriceServiceDetail" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvIsLumpsumUnittypeServiceDetail">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Unit Type :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <dx:ASPxComboBox ID="cbbUnitTypeServiceDetail" CssClass="form-control" runat="server" ClientInstanceName="cbbUnitTypeServiceDetail" TextField="data_text"
                                                        EnableCallbackMode="true" ValueField="data_value">
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-xs-12" id="dvDiscount">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth" id="lbDiscountDetail"></label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtDiscountDetailValue" runat="server" />
                                                    <input type="text" disabled="disabled" style="display: none" id="txtDiscountDetailType" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12">
                                            <div class="row form-group">
                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button5" onclick="addServiceDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button6" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationDetailEdit" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Edit Product Detail
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-xs-12" id="dvProductDescription">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Part No :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtPartNo" runat="server" readonly />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvProductDescription2">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Description :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductDescription" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvProductLine">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 1 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine1" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 2 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine2" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 3 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine3" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 4 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine4" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 5 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine5" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 6 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine6" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 7 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine7" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 8 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine8" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 9 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine9" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Quotation Line 10 :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control" id="txtProductLine10" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvMFG">
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">MFG No :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <dx:ASPxComboBox ID="cbMFGNo" CssClass="form-control" runat="server" ClientInstanceName="cbMFGNo"
                                                        TextField="data_text" OnCallback="cbMFGNo_Callback"
                                                        EnableCallbackMode="true"
                                                        ValueField="data_value">
                                                    </dx:ASPxComboBox>
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
                                            <div class="row form-group">
                                                <label class="col-xs-3 text-right label-rigth">Qty :</label>
                                                <div class="col-xs-7 no-padding">
                                                    <input type="text" class="form-control numberic" id="txtProductQty" runat="server" />
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
                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button7" onclick="submitProductDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button8" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationMaintenance" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add Maintenance Service
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <%-- <div class="col-xs-12">
                                            <input type="text" />
                                        </div>--%>
                                        <div class="col-xs-12" id="dvMaintenanceModel">
                                            <div style="float: right; margin-bottom: 5px;">
                                                <input type="text" id="txtSearchMaintenance" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                                    onkeypress="searchMaintenanceGrid(event.keyCode)" />
                                                <button type="button" class="btn-addItem" id="btnSubmitSearchMaintenance" onclick="searchMaintenanceGrid(13);">
                                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                                </button>
                                            </div>
                                            <dx:ASPxGridView ID="gridViewMaintenance" ClientInstanceName="gridViewMaintenance" runat="server"
                                                Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowSort="false"
                                                Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                                OnCustomCallback="gridViewMaintenance_CustomCallback"
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
                                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                                        <DataItemTemplate>

                                                            <input type="radio" id="radioButton" onchange="getPackageFromModel(<%# Eval("id")%>,'<%# Eval("model_name")%>')"
                                                                name="Model">
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataColumn FieldName="model_name" Caption="Model" VisibleIndex="1" />
                                                    <dx:GridViewDataColumn FieldName="description_tha" Caption="Description" VisibleIndex="2" />
                                                </Columns>
                                                <%--<SettingsSearchPanel Visible="true" />--%>
                                            </dx:ASPxGridView>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                        <button type="button" runat="server" id="Button23" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12" id="dvMaintenancePackage">
                                            Model of :
                                            <label id="lbMaintenanceModel"></label>
                                            <a onclick="backToModel()"><< Back to select Model</a>
                                            <dx:ASPxGridView ID="gridViewMaintenancePackage" ClientInstanceName="gridViewMaintenancePackage" runat="server"
                                                Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowSort="false"
                                                Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                                OnCustomCallback="gridViewMaintenancePackage_CustomCallback"
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
                                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                                        <DataItemTemplate>

                                                            <input type="radio" id="radioButton" onchange="selectedMaintenancePackage(<%# Eval("package_id")%>,'<%# Eval("running_hour")%>')"
                                                                name="MaintenacePack">
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="model_id" Caption="model_id" ReadOnly="True" Width="50px" Visible="false" VisibleIndex="5" />
                                                    <dx:GridViewDataColumn FieldName="count_year" Caption="YEAR" Width="80px" VisibleIndex="1" />
                                                    <dx:GridViewDataColumn FieldName="item_of_part" Caption="ITEM OF PARTS" Width="160px" VisibleIndex="2" />
                                                    <dx:GridViewDataColumn FieldName="running_hour" Caption="RUNNING HOURS OF AIR COMPRESSOR" VisibleIndex="3" />
                                                </Columns>
                                            </dx:ASPxGridView>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                        <button type="button" runat="server" id="Button9" onclick="addMaintenancePackage()" class="btn-app btn-addItem">ยืนยัน</button>
                                                        <button type="button" runat="server" id="Button10" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationPartList" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add PartList
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <%-- <div class="col-xs-12">
                                            <input type="text" />
                                        </div>--%>
                                        <div class="col-xs-4 text-left">
                                            <input id="chkselectAllPartListPackage" type="checkbox" class="" onclick="SelectAllPartList()" />&nbsp;เลือกทั้งหมด
                                        </div>
                                        <div class="col-xs-12">
                                            <div style="float: right; margin-bottom: 5px;">
                                                <input type="text" id="txtSearchPartList" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                                    onkeypress="searchPartListGrid(event.keyCode)" />
                                                <button type="button" class="btn-addItem" id="btnSubmitSearchPartList" onclick="searchPartListGrid(13);">
                                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                                </button>
                                            </div>
                                            <dx:ASPxGridView ID="gridViewPartList" ClientInstanceName="gridViewPartList" runat="server"
                                                Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowSort="false"
                                                Settings-VerticalScrollableHeight="250" EnableCallBacks="true"
                                                OnCustomCallback="gridViewPartList_CustomCallback"
                                                OnHtmlDataCellPrepared="gridViewPartList_HtmlDataCellPrepared"
                                                KeyFieldName="part_list_id" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                                <Paddings Padding="0px" />
                                                <Border BorderWidth="0px" />
                                                <BorderBottom BorderWidth="1px" />
                                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                    PageSizeItemSettings-Visible="false">
                                                    <PageSizeItemSettings Items="10, 20, 50" />
                                                </SettingsPager>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" CellStyle-HorizontalAlign="Center" Width="50px">
                                                        <DataItemTemplate>

                                                            <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                                TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                                                ProductId='<%# Eval("part_list_id")%>'>
                                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        selectedPartList(s,e); 
                                                                    }" />
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataColumn FieldName="item_no" Caption="Item No" VisibleIndex="1" Width="60px" />
                                                    <dx:GridViewDataColumn FieldName="part_no" VisibleIndex="2" Caption="Part No" Width="80px" />
                                                    <dx:GridViewDataColumn FieldName="part_name_tha" Caption="Part Name" Width="200px" VisibleIndex="3" />
                                                    <%--<dx:GridViewDataColumn FieldName="description_tha" VisibleIndex="4" />--%>
                                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                                        FieldName="selling_price" Caption="Selling Price" VisibleIndex="4" Width="80px">
                                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        </PropertiesSpinEdit>
                                                    </dx:GridViewDataSpinEditColumn>
                                                </Columns>
                                                <%-- <SettingsSearchPanel Visible="true" />--%>
                                            </dx:ASPxGridView>
                                        </div>

                                        <div class="col-xs-12">
                                            <div class="row form-group">

                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button11" onclick="submitPartList()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button22" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationSparePart" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Add Spare Part
                                </div>
                                <div class="modal-body">
                                    <div class="row">

                                        <div class="col-xs-12">
                                            <div style="float: right; margin-bottom: 5px;">
                                                <input type="text" id="txtSearchSparePart" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                                    onkeypress="searchSparePartGrid(event.keyCode)" />
                                                <button type="button" class="btn-addItem" id="btnSubmitSparePartSearch" onclick="searchSparePartGrid(13);">
                                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                                </button>
                                            </div>
                                            <dx:ASPxGridView ID="gridViewSparepart" ClientInstanceName="gridViewSparepart" runat="server"
                                                Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowSort="false"
                                                Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                                OnHtmlDataCellPrepared="gridViewSparepart_HtmlDataCellPrepared"
                                                OnCustomCallback="gridViewSparepart_CustomCallback"
                                                KeyFieldName="part_list_id" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                                <Paddings Padding="0px" />
                                                <Border BorderWidth="0px" />
                                                <BorderBottom BorderWidth="1px" />
                                                <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                    PageSizeItemSettings-Visible="false">
                                                    <PageSizeItemSettings Items="10, 20, 50" />
                                                </SettingsPager>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" CellStyle-HorizontalAlign="Center" Width="50px">
                                                        <DataItemTemplate>

                                                            <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                                TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                                                ProductId='<%# Eval("part_list_id")%>'>
                                                                <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        selectedSparePart(s,e); 
                                                                    }" />
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataColumn FieldName="part_no" VisibleIndex="2" Caption="Part No" Width="80px" />
                                                    <dx:GridViewDataColumn FieldName="part_name_tha" Caption="Part Name" Width="200px" VisibleIndex="3" />
                                                    <%--<dx:GridViewDataColumn FieldName="description_tha" VisibleIndex="4" />--%>
                                                    <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                                        FieldName="selling_price" Caption="Selling Price" VisibleIndex="4" Width="80px">
                                                        <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        </PropertiesSpinEdit>
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <%--<dx:GridViewDataColumn FieldName="" VisibleIndex="4" Width="80px" />--%>
                                                    <%--<dx:GridViewDataColumn FieldName="quantity_package" VisibleIndex="5" />--%>
                                                </Columns>
                                                <%-- <SettingsSearchPanel Visible="true" />--%>
                                            </dx:ASPxGridView>
                                        </div>
                                        <%--     <div class="col-xs-12">
                                            <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" Height="150px"
                                                Width="100%" />
                                        </div>--%>
                                        <div class="col-xs-12">
                                            <div class="row form-group">

                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button13" onclick="submitSparePart()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="Button14" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupAddQuotationAnnualService" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    เลือกรายการ
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="tab-content">
                                            <div id="annualServiceHistory" class="tab-pane fade in active">
                                                <div class="col-xs-12">
                                                    <div style="float: left; margin-bottom: 5px;">
                                                        <dx:ASPxCheckBox ID="chkBoxAllAnnual" runat="server" ValueType="System.Boolean" Text="Select All"
                                                            TextField="Name" ValueField="is_selected" ClientInstanceName="chkBoxAllAnnual">
                                                            <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        selectAllAnnualServiceHistoryItem(s, e); 
                                                                    }" />
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div style="float: right; margin-bottom: 5px;">
                                                        <input type="text" id="txtSearchAnnualService" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                                            onkeypress="searchAnnualServiceGrid(event.keyCode)" />
                                                        <button type="button" class="btn-addItem" id="btnSearchAnnualService" onclick="searchAnnualServiceGrid(13);">
                                                            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                                        </button>
                                                    </div>
                                                    <dx:ASPxGridView ID="gridviewAnnualServiceHistory" ClientInstanceName="gridviewAnnualServiceHistory"
                                                        runat="server" Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowSort="false"
                                                        Settings-VerticalScrollableHeight="0" EnableCallBacks="true"
                                                        OnCustomCallback="gridviewAnnualServiceHistory_CustomCallback"
                                                        OnHtmlDataCellPrepared="gridviewAnnualServiceHistory_HtmlDataCellPrepared"
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
                                                            <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="50px">
                                                                <DataItemTemplate>
                                                                    <dx:ASPxCheckBox ID="chkBox" runat="server" ValueType="System.Boolean"
                                                                        TextField="Name" ValueField="is_selected" ClientInstanceName="cbEnable"
                                                                        ProductId='<%# Eval("mfg_no")%>'>
                                                                        <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        selectAnnualServiceHistoryItem(s, e); 
                                                                    }" />
                                                                    </dx:ASPxCheckBox>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataColumn FieldName="project" Caption="Project" Width="100px" VisibleIndex="1" />
                                                            <dx:GridViewDataColumn FieldName="product_no" Caption="Model" Width="80px" VisibleIndex="1" />

                                                            <dx:GridViewDataColumn FieldName="mfg_no" Caption="MFG No" Width="60px" VisibleIndex="3" />

                                                        </Columns>
                                                        <%--<SettingsSearchPanel Visible="true" />--%>
                                                        <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="0" />
                                                    </dx:ASPxGridView>
                                                </div>
                                            </div>

                                            <div class="col-xs-12">
                                                <div class="row form-group">

                                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                        <button type="button" runat="server" id="Button15" onclick="submitAnnualService()" class="btn-app btn-addItem">ยืนยัน</button>
                                                        <button type="button" runat="server" id="Button16" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="popupServiceDetailGridview" role="dialog" data-backdrop="static" data-keyboard="false" style="z-index: 5000">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    Detail
                                </div>

                                <div class="modal-body">
                                    <div class="col-xs-12">
                                        <div class="row">

                                            <button type="button" runat="server" id="btnAddServiceDetail" class="btn-addItem" onclick="popupServiceDetail()" style="margin-bottom: 5px;">
                                                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                            </button>
                                        </div>
                                        <div class="row">
                                            &nbsp;
                                        </div>
                                    </div>
                                    <div class="row">
                                        <dx:ASPxGridView ID="gridViewServiceDetail" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewServiceDetail"
                                            Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewServiceDetail_CustomCallback"
                                            SettingsBehavior-AllowSort="false" Settings-VerticalScrollBarMode="Visible">
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
                                                <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="60px">
                                                    <DataItemTemplate>
                                                        <a id="btnEdit" class="btn btn-mini" onclick="editServiceDetail(<%# Eval("id")%>)" title="Edit">
                                                            <i class="fa fa-pencil" aria-hidden="true"></i>
                                                        </a>
                                                        |
                                                        <a id="btnDelete" class="btn btn-mini" onclick="deleteServiceDetail(<%# Eval("id")%>)" title="Delete">
                                                            <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="1" />
                                                <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Description" VisibleIndex="2" />
                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                                    FieldName="qty" Caption="Qty" VisibleIndex="3" Width="100px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                                    FieldName="unit_price" Caption="Unit Price" VisibleIndex="4" Width="100px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                                    FieldName="total_amount" Caption="Total Amount" VisibleIndex="5" Width="100px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                                    FieldName="discount_amount" Caption="Discount (THB)" VisibleIndex="6" Width="40px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>

                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                                    FieldName="discount_percentage" Caption="Discount (%)" VisibleIndex="7" Width="40px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>

                                                <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false" Visible="false"
                                                    FieldName="discount_total" Caption="Discount Total" VisibleIndex="8" Width="60px">
                                                    <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                    </PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                            </Columns>
                                            <Settings ShowFooter="True" />
                                            <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="350" />
                                        </dx:ASPxGridView>
                                        <div class="col-xs-12">
                                            <div class="row form-group">
                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="Button24" class="btn-app btn-addItem noDisabled" data-dismiss="modal">บันทึก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="otherDetail" class="tab-pane fade">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">Model :</label>
                                <div class="col-xs-6 no-padding">
                                    <dx:ASPxComboBox ID="cbbModel" CssClass="form-control" runat="server" ClientInstanceName="cbbModel"
                                        ClientSideEvents-ValueChanged="changedCustomerModel" EnableCallbackMode="true" OnCallback="cbbModel_Callback"
                                        ValueField="id">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="model" Width="70%" />
                                            <dx:ListBoxColumn FieldName="mfg" Width="30%" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-xs-1 no-padding">
                                    <button type="button" runat="server" id="BtnAddModel" onclick="AddDataModel()" class="form-control">
                                        <i class="fa fa-plus-circle" aria-hidden="true"></i>
                                    </button>
                                </div>

                            </div>
                        </div>

                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">MFG :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" readonly="true" id="txtShowMFG" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">Machine No :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtMachine" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">Hour :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtHour" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">Pressure :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtPressure" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">สถานที่ตั้ง 1 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtLocation1" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">สถานที่ตั้ง 2 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtLocation2" runat="server" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div id="remark" class="tab-pane fade">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 1 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark1" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 2 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark2" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 3 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark3" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 4 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark4" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 5 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark5" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 6 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark6" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 7 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark7" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 8 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark8" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 9 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark9" runat="server" />
                                </div>
                            </div>
                        </div>
                         <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 10 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark10" runat="server" />
                                </div>
                            </div>
                        </div>
                         <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุ 11 :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemark11" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <label class="col-xs-2 text-right label-rigth">หมายเหตุอื่นๆ :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="txtRemarkOther" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
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
                                    <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="6">
                                        <DataItemTemplate>
                                            <a id="btnEdit" class="btn-labelEdit  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="viewQu(<%# Eval("id")%>)" title="Edit">
                                                <%# Eval("quotation_no")%>
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="quotation_subject" Caption="Project" VisibleIndex="7" />
                                    <dx:GridViewDataTextColumn FieldName="quotation_type" Caption="Quotation Type" VisibleIndex="7" />
                                    <dx:GridViewDataTextColumn FieldName="quotation_status_display" Caption="Status" VisibleIndex="7" />

                                    <dx:GridViewDataTextColumn FieldName="is_enable" Caption="Enable" Visible="false" VisibleIndex="4" />
                                </Columns>
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="230" />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                </div>
                <div id="notice" class="tab-pane fade">
                    <div class="row">
                        <div class="col-xs-12">
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
                                            </a>
                                            |
                                            <a id="btnDelete" class="btn btn-mini" onclick="deleteNotice(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i></a>


                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                    <dx:GridViewDataTextColumn FieldName="subject" Caption="Subject" VisibleIndex="6" />
                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="6" />
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                                <SettingsPopup>
                                    <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                </SettingsPopup>
                                <%--<ClientSideEvents BeginCallback="gridViewFreeBie_Callback" />--%>
                            </dx:ASPxGridView>
                        </div>
                    </div>

                    <!-- Modal -->
                    <div class="modal fade" id="popupAddNoticeQuatation" role="dialog" data-backdrop="static" data-keyboard="false">
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

                                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                    <button type="button" runat="server" id="btnSaveNoticeQuatation" onclick="addNotice()" class="btn-app btn-addItem">ยืนยัน</button>
                                                    <button type="button" runat="server" id="btnCancelNoticeQuatation" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="freebies" class="tab-pane fade" style="visibility: hidden">
                    <div class="row">
                        <div class="col-xs-12">
                            <button type="button" runat="server" id="btnAddFreebiesQuatation" class="btn-addItem" onclick="newFreebies()">เพิ่มรายการของแถม</button>
                        </div>
                        <div class="col-xs-12">
                            <!-- Grideview -->
                            <dx:ASPxGridView ID="gridViewFreeBie" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewFreeBie"
                                Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridViewFreeBie_CustomCallback">
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
                                            <a id="btnEdit" class="btn btn-mini" onclick="editFreebies(<%# Eval("id")%>)" title="Edit">
                                                <i class="fa fa-pencil" aria-hidden="true"></i>
                                            </a>
                                            |
                                            <a id="btnDelete" class="btn btn-mini" onclick="deleteFreebies(<%# Eval("id")%>)" title="Delete">
                                                <i class="fa fa-trash-o" aria-hidden="true"></i></a>


                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="id" Caption="ID" ReadOnly="True" Visible="false" Width="50px" VisibleIndex="5" />
                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="6" />
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" EnableCustomizationWindow="true" AllowClientEventsOnLoad="false" />
                                <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />

                                <SettingsPopup>
                                    <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                                </SettingsPopup>
                            </dx:ASPxGridView>
                        </div>

                        <!-- Modal -->
                        <div class="modal fade" id="popupFreebiesQuatation" role="dialog" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        Add Freebies
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">รายการของแถม :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtFreebies" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <label class="col-xs-3 text-right label-rigth">&nbsp;</label>
                                                <div class="col-xs-3 no-padding">
                                                    <div class="checkbox no-margin">
                                                        <label>
                                                            <input type="checkbox" id="cbIsNotice" onchange="onCheckedFreeBies()" value="" />แจ้งเตือน
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <label class="col-xs-3 text-right label-rigth">วันที่ / เวลาแจ้งเตือน :</label>
                                                    <div class="col-xs-7 no-padding">
                                                        <input type="text" class="form-control" id="txtDateTimeFreebies" autocomplete="off" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                                        <button type="button" runat="server" id="btnSaveFreebies" onclick="addFreebies()" class="btn-app btn-addItem">ยืนยัน</button>
                                                        <button type="button" runat="server" id="btnCancelFreebies" class="btn-app btn-addItem noDisabled" data-dismiss="modal">ยกเลิก</button>
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
        <div>
            <asp:HiddenField runat="server" ID="HDidNoti" />
            <asp:HiddenField runat="server" ID="hdTotal" />
            <asp:HiddenField runat="server" ID="hdTotalWithDiscountDetail" />
            <asp:HiddenField runat="server" ID="hdSumDiscountDetail" />
            <asp:HiddenField runat="server" ID="hdSelectedFreebiesId" />
            <asp:HiddenField runat="server" ID="hdSelectedNoticeId" />
            <asp:HiddenField runat="server" ID="hdSelectedProduceId" />
            <asp:HiddenField runat="server" ID="hdSelectedServiceProductId" />
            <asp:HiddenField runat="server" ID="hdSelectedParentId" />
            <asp:HiddenField runat="server" ID="hdMaintenanceModelId" />
            <asp:HiddenField runat="server" ID="hdPackageId" />
            <asp:HiddenField runat="server" ID="hdMinUnitPrice" />
            <asp:HiddenField runat="server" ID="hdCustomerCode" />
            <asp:HiddenField runat="server" ID="quotationStatus" />
            <asp:HiddenField runat="server" ID="hdModelName" />
        </div>
    </div>
</asp:Content>
