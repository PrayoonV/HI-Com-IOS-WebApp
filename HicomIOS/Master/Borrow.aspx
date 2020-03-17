﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Borrow.aspx.cs" Inherits="HicomIOS.Master.Borrow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <style>
        fieldset {
            border: 1px solid #4b4b57;
            display: block;
            margin-bottom: 10px;
            padding: 0 10px;
        }

        .txt-no {
            background-color: #e4effa !important;
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
            height:250px !important;
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
         .btn-addItem {
        margin-left: 5px! important;
        }
    </style>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            $("#txtDueDeliveryDate").prop("readonly", "readonly");

            $("#txtDueDeliveryDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });
            $("#txtPODate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });
            $("#txtReceiveDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });
            $("#txtBorrowDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });


            //$('#dvProductCust').hide();
            //$('#dvQuotation').show()
            //$('#dvQuotation2').show();
            
            var h = window.innerHeight - 300 - 36 - 100;
            gridViewBorrow.SetHeight(h);

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

            var height = dimensions.height - 300 - 36 - 100;
            gridViewBorrow.SetHeight(height);

            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });

        function changedBorrowType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "Borrow.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewBorrow.PerformCallback()
                    cbbSupplier.PerformCallback("|");
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
            if ($('#cbbBorrowType').val() == "P") {
                $('#dvQuotation').hide()
                $('#lbCustomerNo').val("");
                $('#lbCustomerFirstName').val("");
                $('#lbQuotationData').val("");
                $('#dvProductCust').show();
                $('#dvQuotation2').hide();

                var type = $('#cbbProductType').val();
                $('#hdProductType').val(type);
            }
            else {
                $('#dvQuotation').show()
                $('#lbCustomerNo').val("");
                $('#lbCustomerFirstName').val("");
                $('#lbQuotationData').val("");
                $('#dvProductCust').hide();
                $('#dvQuotation2').show();

            }
            $.LoadingOverlay("hide");

        }
        function changedQuotation() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewBorrow.PerformCallback()

                },
                failure: function (response) {
                    //alert(response.d);
                }
            });

            //var data = cbbQuotationNo;
            var key = cbbQuotation.GetValue();

            $.ajax({
                type: "POST",
                url: "Borrow.aspx/GetQuotationData",
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
                    
                    cbbSupplier.PerformCallback(data.supplier_id + "|" + data.supplier_titel);
                },
                failure: function (response) {
                    console.log(response.d);
                    $.LoadingOverlay("hide");
                }
            });
            $.LoadingOverlay("hide");
        }

        function changedCustomer() {

        }

        function changedItemCondition() {

        }

        function popupAddBorrowDetail() {
            try {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var borrowType = $('#cbbBorrowType').val();
                var supplier_id = cbbSupplier.GetValue();

                if (borrowType == "P" && supplier_id == null) {
                    $.LoadingOverlay("hide");
                    swal("กรุณาเลือก Supplier");
                    return;
                }
                var product_type = $('#cbbProductType').val();
                if (borrowType == "P") {
                    $.ajax({
                        type: "POST",
                        url: "Borrow.aspx/GetProductDetailData",
                        data: "{qu_id : " + 0 + " ,supplier_id : '" + supplier_id + "',product_type :'" + product_type + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {
                            $('#TextProduct').val("");
                            $('#chkAllProductDetail').prop('checked', false);

                            gridViewProductDetail.PerformCallback();

                            $('#modal_product').modal('show');

                            $.LoadingOverlay("hide");
                        },
                        failure: function (response) {
                            $.LoadingOverlay("hide");
                        }


                    });

                }
                else if (borrowType == "QU") {
                    var key = cbbQuotation.GetValue();
                    var supplier_id = cbbSupplier.GetValue();
                    
                    if (key == "" || key == null) {
                        $.LoadingOverlay("hide");
                        swal("กรุณาเลือกใบเสนอราคา");
                        return;

                    }

                    $.ajax({
                        type: "POST",
                        url: "Borrow.aspx/GetProductDetailData",
                        data: "{qu_id : " + key + " ,supplier_id : '" + supplier_id + "',product_type :'" + product_type + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {
                            $('#modal_quotation_detail').modal('show');
                            $('#txtSearchText').val("");
                            $('#chkAllQuotationDetail').prop('checked', false);
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
                swal(ex);
            }
        }

        function chkproduct(s, e) {

            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductNo");
            var borrowType = $('#cbbBorrowType').val();
            var product_type = $('#cbbProductType').val();
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/AddSelectedProduct",
                data: '{key:"' + key + '" , isSelected : ' + value + ' , type : "' + borrowType + '" , product_type : "' + product_type + '"}',
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
            var borrowType = $('#cbbBorrowType').val();
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/SubmitProductDetail",
                data: '{type : "' + borrowType + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "P") {
                        $('#modal_product').modal('hide');
                    }
                    else if (response.d == "QU") {
                        $('#modal_quotation_detail').modal('hide');
                    }
                    gridViewBorrow.PerformCallback();
                }
            });
            $.LoadingOverlay("hide");
        }

        function editBorrowDetail(element, e) {
            var key = e;//$(input).attr("saleOrderDetailId");
            $('#hdSelectedBorrowDetailId').val(key);

            $.ajax({
                type: "POST",
                url: "Borrow.aspx/EditBorrowDetail",
                data: '{id:"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtProductDescription').val(data.item_name);
                    $('#txtProductQty').val(data.qty);
                    $('#txtProductUnitPrice').val(data.unit_price);

                    $('#modal_borrow_detail .modal-header').html('Edit Borrow Detail : ' + element.title);
                    $('#modal_borrow_detail').modal('show');
                }
            });

        }

        function deleteBorrowDetail(e) {
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
                         url: "Borrow.aspx/DeleteBorrowDetail",
                         data: '{id:"' + key + '"}',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (response) {
                             $('#hdSelectedBorrowDetailId').val(0);
                             swal("ลบข้อมูลสำเร็จ!", {
                                 icon: "success"
                             })
                                 .then((value) => {
                                     gridViewBorrow.PerformCallback()
                                 });


                         }
                     });
                 }
             });
        }

        function submitBorrowDetail() {
            var key = $('#hdSelectedBorrowDetailId').val();
            var qty = $('#txtProductQty').val();
            var unit_price = $('#txtProductUnitPrice').val();
            //if (parseInt(ref_qty) < parseInt(qty)) {
            //    alert("จำนวนสินค้าคืนมากกว่าจำนวนสินค้าที่รับมา");
            //    return;
            //}
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/SubmitBorrowDetail",
                data: '{id: "' + key + '" , qty: "' + qty + '" , unit_price: "' + unit_price + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdSelectedBorrowDetailId').val(0);
                    $('#txtProductDescription').val("");
                    $('#txtProductQty').val(0);

                    $('#modal_borrow_detail').modal('hide');

                    gridViewBorrow.PerformCallback()
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }

        function backPage() {
            window.location.href = "../Master/BorrowList.aspx";
        }

        function saveDraft() {
            var borrowType = $('#cbbBorrowType').val();

            if (borrowType == "P" && cbbCustomer.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");
                return;
            }
            $('#btnSaveDraft').click();
        }
        function confirmSave() {
            var borrowType = $('#cbbBorrowType').val();
            if ($('#cbbBorrowType').val() == "") {
                swal("กรุณาเลือกประเภทการยืม");
                return;
            }
            if (borrowType == "P" && cbbCustomer.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");
                return;
            }
            var borrow_type = $('#cbbBorrowType').val();
            var product_type = $('#hdProductType').val();

            var txtBorrowNo = $('#txtBorrowNo').val();
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/ValidateData",
                data: '{borrow_type : "' + borrow_type + '" , product_type : "' + product_type + '"}',
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
                    //alert(response.d);
                }
            });

        }
        //function confirmSave() {

        //    $('#btnConfirm').click();
        //}

        function callReport() {
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Borrow_Report|" + $("#txtBorrowNo").val(), "_blank");
        }
        function changedProductType() {
            var type = $('#cbbProductType').val();
            $('#hdProductType').val(type);

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewBorrow.PerformCallback()
                    cbbSupplier.PerformCallback("|");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }
        function changedSupplier() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/ChangedData",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewBorrow.PerformCallback()
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }
        function searchSeletedGrid(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var borrowType = $('#cbbBorrowType').val();
                var txtSearch = $("#txtSearchText").val();
                var txtSearchProduct = $("#TextProduct").val();

                if (borrowType == "QU") {
                    console.log("qu");
                    gridViewQuotationDetail.PerformCallback("Search|" + txtSearch.toString());
                    $.LoadingOverlay("hide");
                }
                else if (borrowType == "P") {
                    console.log("p");
                    gridViewProductDetail.PerformCallback("Search|" + txtSearchProduct.toString());
                    $.LoadingOverlay("hide");
                }



            }
        }
        function popupMFGDetail(element, productId, borrowDetailId, qty) {
            //var input = s.GetMainElement();
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#hdSelectedBorrowDetailId').val(borrowDetailId);
            $('#hdItemId').val(productId);
            console.log("hdItemId==" + $('#hdItemId').val());
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/GetMFGDetail",
                data: '{id: ' + productId + ',borrowDetailId: ' + borrowDetailId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    console.log(data);
                    $('#txtMFGProductDescription').val(data.item_name);
                    $('#hdSelectedProductId').val(data.item_id);
                    $('#hdRefQty').val(data.qty);
                    setTimeout(function () {
                        gridMFG.PerformCallback();
                    }, 100);

                    $('#modal_borrow_mfg .modal-header').html('รายการ : ' + element.title);
                    $('#modal_borrow_mfg').modal('show');
                    $.LoadingOverlay("hide");


                }
            });

        }

        function submitMFG() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var mfg_no = $('#txtMFGNo').val();

            if (mfg_no == "" || mfg_no == undefined) {
                $.LoadingOverlay("hide");
                alert("กรุณากรอก MFG");
                return;
            }
            var qty_product = $('#hdRefQty').val();
            var product_id = $('#hdSelectedProductId').val();
            var borrow_detail_id = $('#hdSelectedBorrowDetailId').val()
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/AddMFG",
                data: '{mfg_no : "' + mfg_no + '" , product_id : ' + product_id + ' , borrow_detail_id : ' + borrow_detail_id + ', qty_product : ' + qty_product + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        gridMFG.PerformCallback();

                        $('#txtMFGNo').val("");
                        $('#txtReceiveDate').val("");

                        $.LoadingOverlay("hide");
                    } else {
                        $('#txtMFGNo').val("");
                        $('#txtReceiveDate').val("");

                        $.LoadingOverlay("hide");
                        swal(response.d);
                    }
                },
                failure: function (response) {
                    swal(response.d);
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
                url: "Borrow.aspx/EditMFG",
                data: '{id : ' + e + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtEditMFGMFGNo').val(data.mfg_no);


                    $('#modal_edit_mfg').modal("show");

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    swal(response.d);
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
                          url: "Borrow.aspx/DeleteMFG",
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
                              swal("การลบข้อมูลผิดพลาด!", {
                                  icon: "error"
                              });
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
            var receive_date = $('#txtEditMFGReceiveDate').val();
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/SubmitEditMFG",
                data: '{id : ' + key + ' , mfg_no : "' + mfg_no + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d == "SUCCESS") {

                        $('#hdEditMFGID').val(0);
                        $('#txtEditMFGMFGNo').val("");

                        gridMFG.PerformCallback();

                        $('#modal_edit_mfg').modal("hide");

                        $.LoadingOverlay("hide");
                    }
                    else {
                        $.LoadingOverlay("hide");
                        swal(response.d);
                        return;
                    }

                },
                failure: function (response) {
                    swal(response.d);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submitMFGProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var ItemId = $('#hdItemId').val();
            console.log("ItemId==" + ItemId);
            $.ajax({
                type: "POST",
                url: "Borrow.aspx/SubmitMFGProduct",
                data: '{id : ' + ItemId + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data == "") {
                        $('#txtMFGProductDescription').val("");
                        $('#hdSelectedProductId').val(0);
                        $('#hdSelectedBorrowDetailId').val(0)

                        gridMFG.PerformCallback();

                        $('#modal_borrow_mfg').modal('hide');

                        $.LoadingOverlay("hide");
                    } else {
                        $.LoadingOverlay("hide");
                        swal(data);
                    }
                }
            });
        }
        function selectAllQuDetail() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var borrowType = $('#cbbBorrowType').val();
            var product_type = $('#cbbProductType').val();
            var search = "";
            if (borrowType == "QU") {
                var selected = $('#chkAllQuotationDetail').is(':checked');
                search = $("#txtSearchText").val();
            } else {
                var selected = $('#chkAllProductDetail').is(':checked');
                search = $("#TextProduct").val();
            }

            $.ajax({
                type: "POST",
                url: "Borrow.aspx/SelectAllQuDetail",
                data: '{selected:' + selected + ',borrowType: "' + borrowType + '",product_type: "' + product_type + '", search: "' + search + '" }',
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

        function addNewItem() {
            window.location.href = "Borrow.aspx";
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
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List Borrow
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior ="false" ID="btnSaveDraft" Style="visibility: hidden" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem" UseSubmitBehavior ="false" ID="btnConfirm" Style="visibility: hidden" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
        </div>
        <div class="row">
            <fieldset>
                <legend>Header</legend>
                <div class="col-xs-12 no-padding">
                    <div class="row" id="Div2" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ประเภทการยืม :</label>
                                <div class="col-xs-9 no-padding">
                                    <select class="form-control" id="cbbBorrowType" onchange="changedBorrowType()"
                                        runat="server" data-rule-required="true" data-msg-required="ประเภทการคืน">
                                        <option value="QU">ยืมจากใบเสนอราคา</option>
                                        <option value="P">ยืมสินค้าจากผู้ขาย</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">เลขที่เอกสาร :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control txt-no" id="txtBorrowNo" readonly="" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วันที่ยืม :</label>
                                <div class="col-xs-8 no-padding">
                                    <input type="text" class="form-control" id="txtBorrowDate" runat="server" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="dvQuotation" runat="server">
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
                    <div class="row" id="dvProductCust" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ประเภทสินค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <select class="form-control" id="cbbProductType" onchange="changedProductType()"
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
                                        ClientInstanceName="cbbCustomer" TextField="data_text"
                                        ClientSideEvents-ValueChanged="changedCustomer" ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="dvQuotation2" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">รหัสลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <input readonly="" class="form-control" id="lbCustomerNo" runat="server" />
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
                    <div class="row" id="Div1" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">PO/ลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <input class="form-control" id="txtPO" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วันที่ PO :</label>
                                <div class="col-xs-9 no-padding">
                                    <input class="form-control" id="txtPODate" runat="server" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">วัตถุประสงค์ :</label>
                                <div class="col-xs-8 no-padding">
                                    <select class="form-control" id="cbbObjective" runat="server" data-rule-required="true" data-msg-required="วัตถุประสงค์">
                                        <option value="S">เพื่อขาย</option>
                                        <option value="R">เพื่อใช้ในงานซ่อมและติดตั้ง</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server">
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Supplier :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbSupplier" CssClass="form-control" runat="server"
                                        ClientInstanceName="cbbSupplier" TextField="data_text"
                                        OnCallback="cbbSupplier_Callback"
                                        ValueField="data_value">
                                        <ClientSideEvents ValueChanged="changedSupplier" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">หมายเหตุ:</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <div class="row">
                <div class="row form-group">
                    <button type="button" id="btnAddBorrowDetail" runat="server" onclick="popupAddBorrowDetail()"
                        class="btn-info" style="height: 30px; margin-left: 15px;">
                        <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;เลือกรายการ สินค้า
                    </button>
                </div>
                <div class="col-xs-12">
                    <dx:ASPxGridView ID="gridViewBorrow" ClientInstanceName="gridViewBorrow" runat="server" Settings-VerticalScrollBarMode="Visible"
                        Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                        OnCustomCallback="gridViewBorrow_CustomCallback"
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
                                    <a id="btnEdit" class="btn btn-mini" onclick="editBorrowDetail(this, <%# Eval("id")%>)" title="<%# Eval("item_no")%>">
                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                    </a>
                                    |
                                                <a id="btnDelete" class="btn btn-mini" onclick="deleteBorrowDetail(<%# Eval("id")%>)" title="Delete">
                                                    <i class="fa fa-trash-o" aria-hidden="true"></i></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MFG Detail" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <a id="btnMFG" class="btn btn-mini" onclick="popupMFGDetail(this, <%# Eval("item_id")%>,<%# Eval("id")%>,<%# Eval("qty")%>)" title="<%# Eval("item_no")%>">
                                        <i class="fa fa-list" aria-hidden="true"></i>
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="item_no" Caption="Product No" Width="100px" />
                            <dx:GridViewDataColumn FieldName="item_name" Caption="Product Name" Width="150px" />
                            <%--<dx:GridViewDataColumn FieldName="mfg_no" Caption="MFG No" Width="80px" />--%>
                            <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                FieldName="qty" Caption="Quantity" Width="50px">
                                <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataColumn FieldName="unit_code" Caption="Unit" Width="50px" />
                            <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                FieldName="unit_price" Caption="Unit Price" Width="50px">
                                <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>

                            <dx:GridViewDataSpinEditColumn Settings-ShowEditorInBatchEditMode="false"
                                FieldName="total_price" Caption="Total Price" Width="50px">
                                <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataColumn FieldName="item_type_text" Caption="Type" Width="100px" Visible="false" />
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
                                <input id="chkAllQuotationDetail" runat="server" type="checkbox" class="" onclick="selectAllQuDetail()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="txtSearchText" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
                                    onkeypress="searchSeletedGrid(event.keyCode)" />
                                <button type="button" class="btn-addItem" id="btnSearchText" onclick="searchSeletedGrid(13);">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                            <div class="col-xs-12">
                                <dx:ASPxGridView ID="gridViewQuotationDetail" ClientInstanceName="gridViewQuotationDetail" runat="server" Settings-VerticalScrollBarMode="Visible"
                                    Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                    OnCustomCallback="gridViewQuotationDetail_CustomCallback"
                                    OnHtmlDataCellPrepared="gridViewQuotationDetail_HtmlDataCellPrepared"
                                    KeyFieldName="product_id" Width="100%">
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
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="quotation_description" Caption="Product Name" VisibleIndex="5" Width="200px" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px" />
                                        <dx:GridViewDataTextColumn FieldName="unit_code" Caption="Unit Code" VisibleIndex="6" Width="50px" />


                                    </Columns>
                                   
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-9 no-padding text-right" style="margin-top: 10px;">
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
        <div class="modal fade" id="modal_borrow_detail" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        Edit Borrow Detail
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
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-7 no-padding text-right">
                                        <button type="button" runat="server" id="Button7" onclick="submitBorrowDetail()" class="btn-app btn-addItem">ยืนยัน</button>
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
                                <input id="chkAllProductDetail" runat="server" type="checkbox" class="" onclick="selectAllQuDetail()" />&nbsp;เลือกทั้งหมด
                            </div>
                            <div class="col-xs-8" style="float: right; margin-bottom: 5px;">
                                <input type="text" id="TextProduct" class="form-control searchBoxData" placeholder="กรอกคำค้นหา..." runat="server"
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
                                        <%--<dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="150px" VisibleIndex="2" />--%>
                                        <dx:GridViewDataTextColumn FieldName="product_id" Visible="false" Caption="Product ID" VisibleIndex="3" />
                                        <dx:GridViewDataTextColumn FieldName="product_no" Caption="Product No" VisibleIndex="4" Width="100px" />
                                        <dx:GridViewDataTextColumn FieldName="product_name" Caption="Product Name" VisibleIndex="5" Width="200px" />
                                        <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="6" Width="50px" />
                                        <dx:GridViewDataTextColumn FieldName="item_type" Caption="Type" VisibleIndex="7" Width="100px" />


                                    </Columns>
                                   
                                </dx:ASPxGridView>
                            </div>

                            <div class="col-xs-12">
                                <div class="row form-group">
                                    <label class="col-xs-3 text-right label-rigth"></label>
                                    <div class="col-xs-9 no-padding text-right" style="margin-top: 10px;">
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
        <div class="modal fade" id="modal_borrow_mfg" role="dialog" data-backdrop="static" data-keyboard="false">
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
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-2" for="cboSupplier">MFG No</label>
                                        <div class="col-xs-6 no-padding-right">
                                            <input type="text" class="form-control" id="txtMFGNo" runat="server" />
                                        </div>
                                        <div class="col-xs-2 no-padding text-left">
                                            <button type="button" class="btn-addItem" onclick="submitMFG()">
                                                <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;เพิ่ม
                                            </button>
                                        </div>
                                    </div>
                                    <div class="form-group col-xs-12 no-padding">
                                        <dx:ASPxGridView ID="gridMFG" SettingsBehavior-AllowSort="false" ClientInstanceName="gridMFG" runat="server"
                                            Settings-VerticalScrollBarMode="Visible"
                                            Settings-VerticalScrollableHeight="300"
                                            Settings-HorizontalScrollBarMode="Visible"
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
                                                            <a class="btn btn-mini" onclick="deleteMFG(<%# Eval("id")%>)" title="Delete">
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                            </a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <%--<dx:GridViewDataTextColumn FieldName="supplier_no" Caption="No" Width="80px" />--%>
                                                <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" Width="500px" CellStyle-HorizontalAlign="Left" />
                                            </Columns>
                                           

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
                                <label class="col-xs-3 text-right label-rigth"></label>
                                <div class="col-xs-9 no-padding text-right" style="margin-top: 10px;">
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
             <asp:HiddenField runat="server" ID="hdItemId" />
            <asp:HiddenField runat="server" ID="hdSales" />
            <asp:HiddenField runat="server" ID="hdCustomerCode" />
            <asp:HiddenField runat="server" ID="hdSelectedBorrowDetailId" />
            <asp:HiddenField runat="server" ID="hdRefQty" />
            <asp:HiddenField runat="server" ID="hdSelectedProductId" />
            <asp:HiddenField runat="server" ID="hdProductType" />
        </div>
    </div>
</asp:Content>
