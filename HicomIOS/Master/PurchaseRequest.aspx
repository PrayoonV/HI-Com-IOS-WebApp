<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="PurchaseRequest.aspx.cs" Inherits="HicomIOS.Master.PurchaseRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <style>
        /*a.disabled {
            pointer-events: none;
        }

        span.disable-links {
            pointer-events: none;
        }*/

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
                height: 35px;
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

        .dv {
            margin-left: 0.1px;
        }

        .swal-modal {
            width: 420px !important;
            /*height: 250px !important;*/
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
         input:read-only {
        background-color:white !important;
        }
    </style>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
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
            //$("#txtDueDeliveryDate").prop("readonly", "readonly");
            //$("#txtPODate").prop("readonly", "readonly");

            $("#txtDueDeliveryDate").datepicker({
                dateFormat: 'dd/mm/yy' , "changeMonth": true,"changeYear": true
            });
            $("#txtPODate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true
            });
            $("#txtReceiveDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true
            });
            $("#mTxtReceiveDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true
                , minDate: new Date()
            });
            $('#txtEditMFGReceiveDate').datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,"changeYear": true
                , minDate: new Date()
            });

            $('#modal_product').on('shown.bs.modal', function (e) {
                $('#txtSearchProduct').val("");
                $('#chkAllProductDetail').prop('checked', false);
            });
            $('#modal_quotation_detail').on('shown.bs.modal', function (e) {
                $('#txtSearchQuotation').val("");
                $('#Checkbox1').prop('checked', false);
            });

            var h = window.innerHeight;
            gridViewPR.SetHeight(h - 300 - 36 - 100 - 10);
            //gridViewProductDetail.SetHeight(h - 300 - 36 - 100);
            
            //$('#quotation-detail-content').height(h - 300 - 36 - 30);

            var height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });

        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };

            var height = dimensions.height - 300 - 36 - 100 - 10;
            gridViewPR.SetHeight(height);
            
            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });

        function changedPRType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            if ($('#cbbPRType').val() == "P") {
                $('#dvQuotation').hide()
                $('#lbCustomerNo').val("");
                $('#lbCustomerFirstName').val("");
                $('#lbQuotationData').val("");
                $('#dvProductCust').show();
                $('#dvQuotation2').hide();
                $('#hdProductType').val("P");
            }
            else {
                $('#dvQuotation').show()
                $('#lbCustomerNo').val("");
                $('#lbCustomerFirstName').val("");
                $('#lbQuotationData').val("");
                $('#dvProductCust').hide();
                $('#dvQuotation2').show();
                $('#hdProductType').val("");
            }
            clearGridView();
        }

        function clearGridView() {
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/ClearGridViewPR",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback();
                    cbbSupplier.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response);
                    alertError(response);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function changedQuotation() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var data = cbbQuotationNo;
            var key = cbbQuotation.GetValue();


            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/GetQuotationData",
                data: '{id: "' + key + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#lbQuotationDate').val(data.display_quotation_date);
                    $('#lbCustomerNo').val(data.customer_code);
                    $('#lbCustomerFirstName').val(data.customer_name);
                    $('#hdCustomerId').val(data.customer_id);
                    $('#hdProductType').val(data.quotation_type);

                    $('#txtPO').val(data.po_no);
                    $('#txtPODate').val(data.po_date);

                    cbbSupplier.PerformCallback();
                    gridViewPR.PerformCallback();
                },
                failure: function (response) {
                    console.log(response.d);
                    $.LoadingOverlay("hide");
                }

            });

            $.LoadingOverlay("hide");
        }

        function changedItemCondition() {

        }

        function searchSeletedGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var prType = $('#cbbPRType').val();
                var txtSearchProduct = $("#txtSearchProduct").val();
                var txtSearchQuotation = $("#txtSearchQuotation").val();


                if (prType == "QU") {
                    console.log("qu");
                    gridViewQuotationDetail.PerformCallback("Search|" + txtSearchQuotation.toString());
                    $.LoadingOverlay("hide");
                }
                else if (prType == "P") {
                    console.log("p");
                    gridViewProductDetail.PerformCallback("Search|" + txtSearchProduct.toString());
                    $.LoadingOverlay("hide");
                }
            }
        }

        function popupAddPRDetail() {
            try {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var prType = $('#cbbPRType').val();
                var product_type = $('#cbbProductType').val();
                var supplier_id = cbbSupplier.GetValue();
                if (supplier_id == null) {
                    $.LoadingOverlay("hide");
                    alertWarning("กรุณาเลือก Supplier");
                    return;
                }
                if (prType == "P") {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseRequest.aspx/GetProductDetailData",
                        data: "{qu_id : " + 0 + " ,supplier_id : '" + supplier_id + "',product_type :'" + product_type + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {
                            gridViewProductDetail.PerformCallback();

                            $('#modal_product').modal('show');

                            $.LoadingOverlay("hide");
                        },
                        failure: function (response) {
                            $.LoadingOverlay("hide");
                        }
                    });
                }
                else if (prType == "QU") {
                    var key = cbbQuotation.GetValue();
                    if (key == "" || key == null) {
                        $.LoadingOverlay("hide");
                        alert("กรุณาเลือกใบเสนอราคา");
                        return;
                    }

                    $.ajax({
                        type: "POST",
                        url: "PurchaseRequest.aspx/GetProductDetailData",
                        data: "{qu_id : '" + key + "' ,supplier_id : '" + supplier_id + "',product_type :'" + product_type + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {
                            $('#modal_quotation_detail').modal('show');
                            gridViewQuotationDetail.PerformCallback();
                            $.LoadingOverlay("hide");
                        },
                        failure: function (response) {
                            console.log(response.d);
                            $.LoadingOverlay("hide");
                        }

                    });
                }

            }
            catch (ex) {
                alert(ex);
            }
        }

        function selectAllProductDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var prType = $('#cbbPRType').val();
            var product_type = $('#cbbProductType').val();
            var search = "";
            if (prType == "QU") {
                var selected = $('#Checkbox1').is(':checked');
                search = $("#txtSearchQuotation").val();
            } else {
                var selected = $('#chkAllProductDetail').is(':checked');
                search = $("#txtSearchProduct").val();
            }
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/SelectAllProductDetail",
                data: '{selected:' + selected + ', type : "' + prType + '" , product_type : "' + product_type + '", search : "' + search + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "P") {
                        gridViewProductDetail.PerformCallback("Search|" + search);
                        $.LoadingOverlay("hide");
                    } else if (data.d == "QU") {
                        gridViewQuotationDetail.PerformCallback("Search|" + search);
                        $.LoadingOverlay("hide");
                    }
                },
                failure: function (data) {

                }
            });

        }


        function chkproduct(s, e) {

            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductNo");
            var prType = $('#cbbPRType').val();
            var product_type = $('#cbbProductType').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/AddSelectedProduct",
                data: '{key:"' + key + '" , isSelected : ' + value + ' , type : "' + prType + '" , product_type : "' + product_type + '"}',
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
            var prType = $('#cbbPRType').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/SubmitProductDetail",
                data: '{type : "' + prType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (prType == "P") {
                        $('#modal_product').modal('hide');

                    }
                    else if (prType == "QU") {
                        $('#modal_quotation_detail').modal('hide');
                    }
                    gridViewPR.PerformCallback();
                }
            });
            $.LoadingOverlay("hide");
        }

        function editPRDetail(element, e) {
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedPRDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/EditPRDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    var pr_status = $('#hdDocumentStatus').val();
                    if (pr_status == "PT" || pr_status == "CF") {
                        //$('#dvReceive').show();
                        $('#txtProductQty').prop('readonly', true);
                        $('#dvReceive').hide();
                        $('#dvReceive2').hide();
                        $('#dvCheckbox').hide();
                    }
                    else {
                        $('#dvReceive').hide();
                        $('#dvReceive2').hide();
                        $('#dvCheckbox').hide();
                    }
                    $('#txtProductDescription').val(data.item_description);
                    $('#txtProductQty').val(data.qty);
                    $('#txtReceiveQty').val(data.receive_qty);
                    $('#txtReceiveDate').val(data.display_receive_date);
                    $('#purchaseOrderNo').val(data.purchaseOrderNo);
                    $('#receiveNo').val(data.receiveNo);

                    $('#modal_pr_detail .modal-header').html('Edit Purchase Request Detail : ' + element.title);
                    $('#modal_pr_detail').modal('show');

                    //  Check product type
                    var product_type = $('#hdProductType').val();
                    if (product_type == 'P') {
                        $('#dvRefNo').hide();
                        $('#dvReceiveDate').hide();
                        $('#dvHistory').hide();
                    } else {
                        $('#dvRefNo').show();
                        $('#dvReceiveDate').show();
                        $('#dvHistory').hide();
                    }
                    $('#dvPO').hide();    
                }
            });

        }

        function viewReceive(element, e) {
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedPRDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/EditPRDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;

                    var pr_status = $('#hdDocumentStatus').val();
                    if (pr_status == "PT" || pr_status == "FL" || pr_status == "PO") {
                        $('#dvReceive').show();
                        $('#dvReceive2').show();
                        $('#dvCheckbox').show();
                        $('#txtProductQty').prop('readonly', true);
                    }
                    else {
                        $('#dvReceive').hide();
                        $('#dvReceive2').hide();
                        $('#dvCheckbox').hide();
                    }
                    if (data.receive_qty_balance == 0) {
                        $('txtReceiveQty').prop('readonly', true);
                    }
                    $('#txtProductDescription').val(data.item_description);
                    $('#txtProductQty').val(data.qty);
                    $('#txtReceiveQty').val(data.receive_qty);
                    $('#txtReceiveDate').val(data.display_receive_date);
                    $('#purchaseOrderNo').val(data.purchaseOrderNo);
                    $('#receiveNo').val(data.receiveNo);
                    $('#txtReceivedQty').val(data.receive_qty_balance);

                    if (data.qty == data.receive_qty_balance) {
                        $('.btn-submit-detail').hide();
                    } else {
                        $('.btn-submit-detail').show();
                    }

                    if (data.history_log == null) {
                        data.history_log = "";
                    }
                    $('#lblHistory').html(data.history_log.split(',').join('<br>'));

                    $('#modal_pr_detail .modal-header').html('Edit Purchase Request Detail : ' + $(element).data('title'));
                    $('#modal_pr_detail').modal('show');

                    //  Check product type
                    var product_type = $('#hdProductType').val();
                    if (product_type == 'P') { 
                        $('#dvRefNo').hide();
                        $('#dvReceiveDate').hide();
                        $('#dvHistory').hide();
                    } else {
                        $('#dvRefNo').show();
                        $('#dvReceiveDate').show();
                        $('#dvHistory').show();
                    }
                    $('#dvPO').show();
                }
            });

        }
        function deletePRDetail(e) {
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
                            url: "PurchaseRequest.aspx/DeletePRDetail",
                            data: '{id:"' + key + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $('#hdSelectedPRDetailId').val(0);
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridViewPR.PerformCallback()
                                    });
                            }
                        });
                    }
                });
        }

        function submitPRDetail() {
            var key = $('#hdSelectedPRDetailId').val();
            var qty = $('#txtProductQty').val();
            var receive_qty = $('#txtReceiveQty').val();
            var receive_date = $('#txtReceiveDate').val();
            if (receive_date == undefined) {
                receive_date = '';
            }
            var received_qty = $('#txtReceivedQty').val();
            console.log('receive_qty ' + receive_qty);
            console.log('received_qty ' + received_qty);
            console.log('qty ' + qty);
            var purchaseOrderNo = $('#purchaseOrderNo').val();
            var receiveNo = $('#receiveNo').val();
            var pr_status = $('#hdDocumentStatus').val();
            var isAllCheck = $('#cbAllCheck').prop('checked');

            //validate receive qty
            //if (pr_status == "CF" || pr_status == "PT") {
                //if (parseInt(received_qty) > 0) {
                    if ((parseInt(receive_qty) + parseInt(received_qty)) > parseInt(qty)) {
                        swal("", "จำนวนสินค้าที่รับเกินกำหนด", "warning");
                        return;
                    }
                /*}
                else {
                    if (parseInt(receive_qty) > parseInt(qty)) {
                        swal("Warning !", "จำนวนสินค้าที่รับเกินกำหนด", "warning");
                        return;
                    }
                }*/
            //}
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/SubmitPRDetail",
                data: '{id:"' + key + '",qty:"' + qty + '",receive_qty:"' + receive_qty + '",receive_date:"' + receive_date + '",purchaseOrderNo:"' + purchaseOrderNo + '",receiveNo:"' + receiveNo + '", isAllCheck: "' + isAllCheck + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdSelectedPRDetailId').val(0);
                    $('#txtProductDescription').val("");
                    $('#txtProductQty').val(0);
                    $('#txtReceiveQty').val(0);
                    $('#txtReceiveDate').val("");
                    $('#purchaseOrderNo').val("");
                    $('#receiveNo').val("");
                    $('#cbAllCheck').prop('checked', false);
                    $('#modal_pr_detail').modal('hide');

                    gridViewPR.PerformCallback()
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function backPage() {
            window.location.href = "../Master/PurchaseRequestList.aspx";
        }

        function saveDraft() {
            $('#btnSaveDraft').click();
        }

        function confirmSave() {

            var pr_no = $('#txtPRNo').val();

            if ($('#cbbPRType').val() == "") {
                alert("กรุณาเลือกประเภทการยืม");
                return;
            }
            if ($('#cbbPRType').val() != "QU" && cbbCustomer.GetValue == null) {
                alert("กรุณาเลือกลูกค้า");
                return;
            }
            if ($('#txtDueDeliveryDate').val() == '') {
                swal("กรูณากรอกวันกำหนดส่งของ");
                return;
            }
            var pr_type = $('#cbbPRType').val();
            var product_type = $('#hdProductType').val();
            var pr_status = $('#hdDocumentStatus').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/ValidateData",
                data: '{pr_type : "' + pr_type + '" , product_type : "' + product_type + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        //console.log("testconfirm");
                        sendApproveMessage(pr_no, 'btnConfirm', 'ส่งเอกสาร');
                    }
                    else {
                        swal(response.d);
                    }
                },

                failure: function (response) {
                    //alert(response.d);
                }
            });
            //$('#btnConfirm').click();
        }
        function changedSupplier() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/ChangedSupplier",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback()
                    cbbSupplier.PerformCallback();
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function changedCustomer(s) {
            var customer_id = s.GetValue().toString();
            console.log('customer_id ' + customer_id);
            $('#hdCustomerId').val(customer_id);
        }

        function popupMFGDetail(element, productId, prDetailId, maxQty) {
            //var input = s.GetMainElement();
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#hdSelectedPRDetailId').val(prDetailId);
            $('#hdMaxQty').val(maxQty);

            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/GetMFGDetail",
                data: '{id: ' + productId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtMFGProductDescription').val(data.item_description);
                    $('#hdSelectedProductId').val(data.item_id);

                    setTimeout(function () {
                        gridMFG.PerformCallback();
                    }, 100);

                    if ($('#hdDocumentStatus').val() == 'CP') {
                        $('.MFGDetailSearch').hide();
                        $('#Button5').hide();
                    }
                    $('#modal_pr_mfg .modal-header').html('รายการ : ' + element.title);
                    $('#modal_pr_mfg').modal('show');
                    $.LoadingOverlay("hide");
                }
            });

        }

        function submitMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });


            var mfg_no = $('#txtMFGNo').val();
            var receive_no = $('#mReceiveNo').val();
            var receive_date = $('#mTxtReceiveDate').val();

            if (mfg_no == "" || mfg_no == undefined) {
                $.LoadingOverlay("hide");
                return;
            }
            var product_id = $('#hdSelectedProductId').val();
            var pr_detail_id = $('#hdSelectedPRDetailId').val()
            var maxQty = $('#hdMaxQty').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/AddMFG",
                data: '{mfg_no : "' + mfg_no + '" , product_id : ' + product_id + ' , pr_detail_id : ' + pr_detail_id + ' , receive_no :\'' + receive_no + '\' , receive_date : \'' + receive_date + '\', maxQty: ' + maxQty + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        gridMFG.PerformCallback();

                        $('#txtMFGNo').val("");
                        $('#mReceiveNo').val("");
                        $('#mTxtReceiveDate').val("");
                    } else {
                        alertError(response.d);
                    }

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alert(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function editMFG(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#hdEditMFGID').val(e);
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/EditMFG",
                data: '{id : ' + e + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtEditMFGMFGNo').val(data.mfg_no);
                    $('#txtEditMFGReceiveNo').val(data.receive_no);
                    $('#txtEditMFGReceiveDate').val(data.display_receive_date);

                    $('#modal_edit_mfg').modal("show");

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alert(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function deleteMFG(e) {
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
                            url: "PurchaseRequest.aspx/DeleteMFG",
                            data: '{id : ' + e + ' }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridMFG.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        });
                    }
                });
        }

        function submitEditMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = $('#hdEditMFGID').val();
            var mfg_no = $('#txtEditMFGMFGNo').val();
            var receive_no = $('#txtEditMFGReceiveNo').val();
            var receive_date = $('#txtEditMFGReceiveDate').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/SubmitEditMFG",
                data: '{id : ' + key + ' , mfg_no : "' + mfg_no + '", receive_no : "' + receive_no + '", receive_date : "' + receive_date + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "SUCCESS") {

                        $('#hdEditMFGID').val(0);
                        $('#txtEditMFGMFGNo').val("");
                        $('#txtEditMFGReceiveNo').val("");
                        $('#txtEditMFGReceiveDate').val("");

                        gridMFG.PerformCallback();

                        $('#modal_edit_mfg').modal("hide");

                        $.LoadingOverlay("hide");
                    }
                    else {
                        $.LoadingOverlay("hide");
                        alert(response.d);
                        return;
                    }

                },
                failure: function (response) {
                    alert(response.d);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submitMFGProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var prDetailId = $('#hdSelectedPRDetailId').val();
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/SubmitMFGProduct",
                data: '{prDetailId : "' + prDetailId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data == "") {
                        $('#txtMFGProductDescription').val("");
                        $('#hdSelectedProductId').val(0);
                        $('#hdSelectedPRDetailId').val(0)

                        gridMFG.PerformCallback();
                        gridViewPR.PerformCallback();

                        $('#modal_pr_mfg').modal('hide');
                    } else {
                        swal(data);
                    }

                    $.LoadingOverlay("hide");
                }
            });
        }

        function autoAddQty() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/AutoAddQty",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback();

                    $.LoadingOverlay("hide");
                }
            });
        }

        function autoClearQty() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/AutoClearQty",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback();

                    $.LoadingOverlay("hide");
                }
            });
        }

        function callReport() {
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Purchase_Request|" + $("#txtPRNo").val(), "_blank");
        }

        function searchProductGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                var txtSearch = $("#txtSearchBoxData").val();
                gridViewPR.PerformCallback("Search|" + txtSearch.toString());

                $.LoadingOverlay("hide");
            }
        }

        function moveUpProductDetail(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "PurchaseRequest.aspx/moveUpProductDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback();
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
                url: "PurchaseRequest.aspx/moveDownProductDetail",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewPR.PerformCallback();
                },
                failure: function (response) {
                    swal(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function addNewItem() {
            window.location.href = "PurchaseRequest.aspx";
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
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List PurchaseRequest
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnSaveDraft" Style="visibility: hidden" UseSubmitBehavior="false" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" ID="btnConfirm" Style="visibility: hidden" Text="Confirm" UseSubmitBehavior="false" OnClick="btnConfirm_Click"></dx:ASPxButton>
        </div>
        <div class="row">
            <fieldset>
                <legend>Header</legend>
                <div class="col-xs-12 no-padding">
                    <div class="row dv" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ประเภทการขอซื้อ :</label>
                                <div class="col-xs-9 no-padding">
                                    <select class="form-control" id="cbbPRType" onchange="changedPRType()"
                                        runat="server" data-rule-required="true" data-msg-required="ประเภทการคืน">
                                        <option value="QU">ขอซื้อจากใบเสนอราคา(Quotation)</option>
                                        <option value="P">สั่งซื้อสินค้า</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">เลขที่เอกสาร :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtPRNo" readonly="" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วันที่เอกสาร :</label>
                                <div class="col-xs-8 no-padding">
                                    <input type="text" class="form-control" id="txtPRDate" readonly="" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row dv" id="dvQuotation" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ใบเสนอราคา :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbQuotation" CssClass="form-control" runat="server"
                                        ClientInstanceName="cbbQuotation" TextField="data_text"
                                        ClientSideEvents-ValueChanged="changedQuotation" ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วันที่ใบเสนอราคา :</label>
                                <div class="col-xs-9 no-padding">
                                    <input readonly="" class="form-control" id="lbQuotationDate" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row dv" id="dvProductCust" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ประเภทสินค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <select class="form-control" id="cbbProductType" onchange="changedSupplier()"
                                        runat="server" data-rule-required="true" data-msg-required="ประเภทสินค้า">
                                        <option value="P">สินค้า</option>
                                        <option value="S">อะไหล่</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbCustomer" CssClass="form-control" runat="server"
                                        ClientInstanceName="cbbCustomer" TextField="data_text" onchange="changedCustomer()"
                                        ValueField="data_value">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { changedCustomer(s); }" /> 
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row dv" id="dvQuotation2" runat="server">
                          <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <input readonly="" class="form-control" id="lbCustomerFirstName" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding" style="display:none;">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">รหัสลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <input readonly="" class="form-control" id="lbCustomerNo" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row dv" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">เลขที่ใบสั่งซื้อลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <input class="form-control" id="txtPO" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วันที่ PO :</label>
                                <div class="col-xs-9 no-padding">
                                    <input class="form-control" id="txtPODate" readonly="true" runat="server" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">กำหนดส่งของ :</label>
                                <div class="col-xs-8 no-padding">
                                    <input class="form-control" id="txtDueDeliveryDate" readonly="true" runat="server" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row dv" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วัตถุประสงค์ :</label>
                                <div class="col-xs-9 no-padding">
                                    <select class="form-control" id="cbbRequestFor" runat="server" data-rule-required="true" data-msg-required="วัตถุประสงค์">
                                        <option value="S">เพื่อขาย</option>
                                        <option value="R">เพื่อใช้ในงานซ่อมและติดตั้ง</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="row dv" runat="server">
                    <label class="col-xs-1 text-right label-rigth">Supplier :</label>
                    <div class="col-xs-3 no-padding">
                        <div class="row form-group">
                            <div class="col-xs-12 no-padding">
                                <dx:ASPxComboBox ID="cbbSupplier" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbSupplier" TextField="data_text" OnCallback="cbbSupplier_Callback"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <button type="button" id="btnAddPRDetail" runat="server" onclick="popupAddPRDetail()"
                            class="btn-info" style="height: 30px; margin-left: 15px;">
                            <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกรายการ สินค้า
                        </button>
                    </div>
                </div>
                <br />
                <div class="col-xs-12">
                    <div style="margin-bottom: 5px;">
                        <input type="text" id="txtSearchBoxData" class="form-control searchBoxData"
                            placeholder="กรอกคำค้นหา..." runat="server" onkeypress="searchProductGrid(event.keyCode)" />
                        <button type="button" class="btn-addItem" id="btnSubmitSearch" onclick="searchProductGrid(13);">
                            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                        </button>

                        <div style="float: right;">
                            <button type="button" class="btn-addItem" id="btnGetAll" onclick="autoAddQty();">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;รับทั้งหมด
                            </button>
                            &nbsp;
                            <button type="button" class="btn-addItem" id="btnNone" onclick="autoClearQty();">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;ไม่รับทั้งหมด
                            </button>
                        </div>
                    </div>
                    <div class="clear">&nbsp;</div>
                    <dx:ASPxGridView ID="gridViewPR" ClientInstanceName="gridViewPR" runat="server" Settings-VerticalScrollBarMode="Visible"
                        Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                        OnCustomCallback="gridViewPR_CustomCallback" 
                        KeyFieldName="id" Width="100%">
                        <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                        <Paddings Padding="0px" />
                        <Border BorderWidth="0px" />
                        <BorderBottom BorderWidth="1px" />
                        <%--<SettingsResizing ColumnResizeMode="NextColumn" />--%>
                        <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                            PageSizeItemSettings-Visible="false">
                            <PageSizeItemSettings Items="10, 20, 50" />
                        </SettingsPager>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="#" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                                <DataItemTemplate>
                                    <a id="btnEdit" class="btn btn-mini" onclick="editPRDetail(this, <%# Eval("id")%>)" title="<%# Eval("item_no")%>">
                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                    </a>
                                    |
                                    <a id="btnDelete" <%# Convert.ToInt32((Eval("receive_qty_balance"))) > 0 ? "disabled='disabled'" : "" %>
                                        class="btn btn-mini"
                                        onclick="deletePRDetail(<%# Eval("id")%>)"
                                        title="Delete">
                                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                                    </a>
                                    
                                    <a id="btnMoveUp" class="btn btn-mini" onclick="moveUpProductDetail(<%# Eval("id")%>)" title="Edit">
                                        <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                    </a>
                                    |           
                                    <a id="btnMoveDown" class="btn btn-mini" onclick="moveDownProductDetail(<%# Eval("id")%>)" title="Edit">
                                        <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="supplier_name" Caption="Supplier" Width="30px" />
                            <dx:GridViewDataColumn FieldName="item_no" Caption="Product No" Width="30px" />
                            <dx:GridViewDataColumn FieldName="item_description" Caption="Product Name" Width="40px" />
                            <dx:GridViewDataTextColumn FieldName="qty" Caption="จำนวนที่สั่งซื้อ" Width="20px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="receive_qty_balance" Caption="รับมาแล้ว" Width="20px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="receive_qty" Caption="จำนวนครั้งนี้" Width="20px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="remain_qty" Caption="จำนวนค้างรับ" Width="20px">
                                <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="purchaseOrderNo" Caption="เลขที่ใบสั่งซื้อ" Width="30px" />
                            <dx:GridViewDataTextColumn Caption="รับเข้า" FieldName="item_Receive" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="30px">
                                <DataItemTemplate>
                                    <button type="button" id="btnviewReceive" onclick="viewReceive(this, <%# Eval("id") %>)" data-title="<%# Eval("item_no")%>"
                                        <% //Convert.ToInt32((Eval("receive_qty_balance"))) == Convert.ToInt32((Eval("qty"))) ? "disabled='disabled'" : "" %>
                                        class="btn-addItem">
                                        <i class="fas fa-calendar-check"></i>&nbsp;<%# Convert.ToInt32((Eval("receive_qty_balance"))) == Convert.ToInt32((Eval("qty"))) ? "รับเข้าสมบูรณ์" : "กดเพื่อรับ" %>
                                    </button>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MFG Detail" FieldName="mfg_detail" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="20px">
                                <DataItemTemplate>
                                    <a id="btnMFG" class="btn btn-mini" onclick="popupMFGDetail(this, <%# Eval("item_id")%>,<%# Eval("id")%>,<%# Eval("qty")%>)" title="<%# Eval("item_no")%>">
                                        <i class="fa fa-list" aria-hidden="true"></i>
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modal_quotation_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-4 text-left">
                                <input id="Checkbox1" runat="server" type="checkbox" class="" onclick="selectAllProductDetail()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchQuotation" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnTextQuotation" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewQuotationDetail" ClientInstanceName="gridViewQuotationDetail" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewQuotationDetail_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewQuotationDetail_HtmlDataCellPrepared"
                                    KeyFieldName="product_no" Width="100%">
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
                                                <dx:ASPxCheckBox ID="chkQuotationDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkQuotationDetail"
                                                    ProductNo='<%# Eval("product_no")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Product Name" VisibleIndex="5" Width="200px" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px">
                                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" VisibleIndex="6" Width="50px" />
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                        <button type="button" runat="server" id="Button1" onclick="submitProductDetail()" class="btn-app btn-addItem">ยืนยัน</button>
                                        <button type="button" runat="server" id="Button2" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_pr_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Edit Purchase Request Detail
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">รายการที่สั่งซื้อ :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control" readonly="true" id="txtProductDescription" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">จำนวนที่สั่งซื้อ :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" class="form-control numberic" id="txtProductQty" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="dvReceive">

                                <div class="col-xs-12">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">จำนวนที่เคยรับไปแล้ว :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" class="form-control numberic" id="txtReceivedQty" readonly="readonly" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">จำนวนรับครั้งนี้ :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" style="background-color: #ddffdd;" class="form-control numberic" id="txtReceiveQty" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12" id="dvPO">
                                <hr />
                                <h5><b>เอกสารประกอบการสั่งซื้อ</b></h5>
                                <br />
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">เลขที่ใบสั่งซื้อ :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" style="background-color: #ddffdd;" class="form-control" id="purchaseOrderNo" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12" id="dvRefNo">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth">เลขที่ใบแจ้งหนี้ :</label>
                                    <div class="col-xs-7 no-padding">
                                        <input type="text" style="background-color: #ddffdd;" class="form-control" id="receiveNo" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="dvReceive2">
                                <div class="col-xs-12" id="dvReceiveDate">
                                    <div class="row form-group">
                                        <label class="col-xs-3 text-right label-rigth">วันที่รับสินค้า :</label>
                                        <div class="col-xs-7 no-padding">
                                            <input type="text" style="background-color: #ddffdd;" class="form-control numberic" readonly="true" id="txtReceiveDate" runat="server" autocomplete="off" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="dvCheckbox">
                                 <div class="col-xs-12">
                                    <div class="row form-group">
                                        <div class="col-xs-3">&nbsp;</div>
                                        <div class="col-xs-7 no-padding">
                                            <label>
                                                <input type="checkbox" runat="server" id="cbAllCheck" enableviewstate="false" value="1" />&nbsp;<b>ใช้ข้อมูลชุดนี้กับรายการทั้งหมด</b>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12" id="dvHistory">
                                <hr />
                                <h5><b>ประวัติการรับสินค้า</b></h5>
                                <br />
                                <div class="row form-group">
                                    <label class="col-xs-2 text-right label-rigth"></label>
                                    <label class="col-xs-10" id="lblHistory" style="font-size: 15px;"></label>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
                                        <button type="button" runat="server" id="Button7" onclick="submitPRDetail()" class="btn-app btn-addItem btn-submit-detail">ยืนยัน</button>
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
                <div class="modal-content">
                    <div class="modal-header">
                        รายการ
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-4 text-left">
                                <input id="chkAllProductDetail" runat="server" type="checkbox" class="" onclick="selectAllProductDetail()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchProduct" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnTextProduct" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
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
                                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                        PageSizeItemSettings-Visible="false">
                                        <PageSizeItemSettings Items="10, 20, 50" />
                                    </SettingsPager>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="#" FieldName="is_selected" Width="30px">
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkProductDetail" runat="server" ValueType="System.Boolean"
                                                    TextField="Name" ValueField="is_selected" ClientInstanceName="chkProductDetail"
                                                    ProductNo='<%# Eval("product_no")%>'>
                                                    <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        chkproduct(s, e); 
                                                                    }" />
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="product_name" Caption="Product Name" VisibleIndex="5" Width="200px" />
                                        <%--<dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px">
                                            <PropertiesTextEdit DisplayFormatString="#,##0"></PropertiesTextEdit>
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                        </dx:GridViewDataTextColumn>--%>
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px">
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <%# Convert.ToInt32(Eval("qty")) %>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="item_type" Caption="Type" VisibleIndex="7" Width="100px" />
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <div class="col-xs-12 text-right" style="padding-top: 10px">
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
        <div class="modal fade" id="modal_pr_mfg" role="dialog" data-backdrop="static" data-keyboard="false">
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
                            <div style="padding: 0 10px;">
                                <fieldset>
                                    <legend style="text-align: left; margin-bottom: 5px;">MFG</legend>
                                    <div class="form-group col-xs-6 no-padding MFGDetailSearch">
                                        <label class="control-label col-xs-4 text-right" for="cboSupplier">MFG No</label>
                                        <div class="col-xs-8 no-padding-right">
                                            <input type="text" class="form-control" id="txtMFGNo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group col-xs-6 no-padding MFGDetailSearch">
                                        <label class="control-label col-xs-4 text-right" for="cboSupplier">เลขที่ใบแจ้งหนี้</label>
                                        <div class="col-xs-8 no-padding-right">
                                            <input type="text" class="form-control" id="mReceiveNo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group col-xs-6 no-padding MFGDetailSearch">
                                        <label class="control-label col-xs-4 text-right" for="cboSupplier">วันที่รับสินค้า</label>
                                        <div class="col-xs-8 no-padding-right">
                                            <input type="text" class="form-control" id="mTxtReceiveDate" readonly="true" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group col-xs-6 text-left MFGDetailSearch">
                                        <button type="button" class="btn-addItem" onclick="submitMFG()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                        </button>
                                    </div>
                                    <div class="form-group col-xs-16 no-padding">
                                        <dx:ASPxGridView ID="gridMFG" SettingsBehavior-AllowSort="false" ClientInstanceName="gridMFG" runat="server"
                                            Settings-VerticalScrollBarMode="Visible"
                                            Settings-VerticalScrollableHeight="300"
                                            EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridMFG_CustomCallback"
                                            Width="100%">
                                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                            <Paddings Padding="0px" />
                                            <Border BorderWidth="0px" />
                                            <BorderBottom BorderWidth="1px" />
                                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                                PageSizeItemSettings-Visible="true">
                                                <PageSizeItemSettings Items="10, 20, 50" />
                                            </SettingsPager>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="80">
                                                    <DataItemTemplate>
                                                        <a class="btn btn-mini" onclick="editMFG(<%# Eval("id")%>)" title="Edit">
                                                            <i class="fa fa-pencil" aria-hidden="true"></i>
                                                        </a>|
                                                            <a class="btn btn-mini" onclick="deleteMFG(<%# Eval("id")%>)" title="Delete" <%# Convert.ToInt32((Eval("id"))) <= 0 ? "disabled='disabled'" : "" %>>
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                            </a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" Width="200px" CellStyle-HorizontalAlign="Left" />
                                                <dx:GridViewDataTextColumn FieldName="receive_no" Caption="เลขที่ใบแจ้งหนี้" Width="150px" CellStyle-HorizontalAlign="Left" />
                                                <dx:GridViewDataTextColumn FieldName="display_receive_date" Caption="วันที่รับสินค้า" Width="100px" CellStyle-HorizontalAlign="Left" />
                                            </Columns>
                                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                        </dx:ASPxGridView>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-xs-12">
                            <div class="row form-group">
                                <div class="col-xs-12 text-right" style="padding-top: 10px">
                                    <button type="button" runat="server" id="Button5" onclick="submitMFGProduct()" class="btn-app btn-addItem">ยืนยัน</button>
                                    <button type="button" runat="server" id="Button6" class="btn-app btn-addItem" data-dismiss="modal">ยกเลิก</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal_edit_mfg" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <div class="modal-title">Edit MFG</div>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row">
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEditMFGMFGNo">MFG No :</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEditMFGMFGNo" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEditMFGReceiveNo">เลขที่เอกสาร :</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEditMFGReceiveNo" runat="server" />
                                </div>
                            </div>
                            <div class="form-group col-xs-12 no-padding">
                                <label class="control-label col-xs-4 no-padding" for="txtEditMFGReceiveDate">วันที่รับสินค้า :</label>
                                <div class="col-xs-8">
                                    <input type="text" class="form-control" id="txtEditMFGReceiveDate" readonly="true" runat="server" />
                                </div>
                            </div>
                            <asp:HiddenField runat="server" ID="hdEditMFGID" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn-addItem" onclick="submitEditMFG()">
                            <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                        </button>
                        <button type="button" class="btn-addItem" data-dismiss="modal">
                            <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div class="hidden">
            <asp:HiddenField runat="server" ID="hdCustomerId" />
            <asp:HiddenField runat="server" ID="hdSales" />
            <asp:HiddenField runat="server" ID="hdCustomerCode" />
            <asp:HiddenField runat="server" ID="hdSelectedPRDetailId" />
            <asp:HiddenField runat="server" ID="hdRefQty" />
            <asp:HiddenField runat="server" ID="hdSelectedProductId" />
            <asp:HiddenField runat="server" ID="hdMaxQty" />
            <asp:HiddenField runat="server" ID="hdDocumentStatus" />
            <asp:HiddenField runat="server" ID="hdProductType" />
        </div>
    </div>
</asp:Content>
