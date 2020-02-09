<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AdjustProduct.aspx.cs" Inherits="HicomIOS.Master.AdjustProduct" %>
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
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();
            var h = window.innerHeight;
            gridView.SetHeight(h - 345 - 30);

            $("#dtAdjustDate").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });

            $('#popupSelectedItem').on('shown.bs.modal', function () {
                $('#txtSearchBoxData').val("");
            })

            changedProductType();

            var h = window.innerHeight;
            gridView.SetHeight(h - 300 - 35 - 30);

            var height = $('#Splitter_1').height();
            $('#Splitter_1').height(height);
            $('#Splitter_1_CC').height(height);
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            console.log(s);
            var dimensions = {
                height: (event.srcElement || event.currentTarget).innerHeight,
                width: (event.srcElement || event.currentTarget).innerWidth
            };
            
            var height = dimensions.height - 300 - 35 - 30;
            gridView.SetHeight(height);
            
            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height);
            $('#Splitter_1_CC').height(height);
        });
        function addItemDetail(item_type) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/PopupItemDetail",
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
        function getCheckBoxValue(s, e) {
            var value = s.GetChecked();
            var input = s.GetMainElement();
            var key = $(input).attr("ProductId");
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/SelectedItem",
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
        function submitSelectItem() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/SubmitSelectedItem",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback();
                    $('#popupSelectedItem').modal("hide");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    $.LoadingOverlay("hide");
                }
            });
        }
        function changedQuantityType(s, e) {
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("DetailId");
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/ChangeQuantityType",
                data: '{key: ' + key + ' , value: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //gridView.PerformCallback();
                    // $('#popupSelectedItem').modal("hide");
                    // $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    // $.LoadingOverlay("hide");
                }
            });
        }

        function changedQuantity(s, e) {
            //$.LoadingOverlay("show", {
            //    zIndex: 9999x
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("DetailId");

            if (value < 0) {
                value = 0;
                s.SetValue("0");
            }

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/ChangeQuantity",
                data: '{key: ' + key + ' , value: ' + value + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // gridView.PerformCallback();
                    // $('#popupSelectedItem').modal("hide");
                    // $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    // $.LoadingOverlay("hide");
                }
            });
        }
        function changedRemark(s, e) {
            //$.LoadingOverlay("show", {
            //    zIndex: 9999
            //});
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("DetailId");

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/ChangeRemark",
                data: '{key: ' + key + ' , value: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // gridView.PerformCallback();
                    // $('#popupSelectedItem').modal("hide");
                    // $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    // $.LoadingOverlay("hide");
                }
            });
        }
        function changedSortNo(s, e) {
            //$.LoadingOverlay("show", {
            //    zIndex: 9999
            //});
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("DetailId");

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/ChangeSortno",
                data: '{key: ' + key + ' , value: ' + value + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // gridView.PerformCallback();
                    // $('#popupSelectedItem').modal("hide");
                    // $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    // $.LoadingOverlay("hide");
                }
            });
        }
        function getRadioValue(s, e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var value = s.GetValue();
            var input = s.GetMainElement();
            var key = $(input).attr("DetailId");
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/CheckedAdjust",
                data: '{key: ' + key + ' , value: "' + value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback();
                    //$('#popupSelectedItem').modal("hide");
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    alert("Some Problem");
                    $.LoadingOverlay("hide");
                }
            });
        }
        function deleteAdjust(id) {
            swal({
                    title: "คุณต้องการลบข้อมูลใช่หรือไม่ ?",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                })
                .then((willDelete) => {
                    if (willDelete) {
                        $.LoadingOverlay("show", {
                            zIndex: 9999
                        });
                        $.ajax({
                            type: "POST",
                            url: "AdjustProduct.aspx/DeleteAdjustItem",
                            data: '{id: ' + id + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                gridView.PerformCallback();
                                //$('#popupSelectedItem').modal("hide");
                                $.LoadingOverlay("hide");
                            },
                            failure: function (response) {
                                alert("Some Problem");
                                $.LoadingOverlay("hide");
                            }
                        });
                    }
                });
        }
        function saveDraft() {
            var isValid = true;

            /*var store = $('#txtStore').val();
            if (store == '') {
                isValid = false;
                $('#txtStore').parent().parent().addClass("has-error");
            }*/

            var subject = $('#txtSubject').val();
            if (subject == '') {
                isValid = false;
                $('#txtSubject').parent().parent().addClass("has-error");
            }

            var adjustDate = $('#dtAdjustDate').val();
            if (adjustDate == '') {
                isValid = false;
                $('#dtAdjustDate').parent().parent().addClass("has-error");
            } else {
                $('#dtAdjustDate').parent().parent().removeClass("has-error");
            }

            if (!isValid) {
                return;
            }

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/ValidateData",
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

        function confirmSave() {

            if (cbbCustomer.GetValue() == null) {
                swal("กรุณาเลือกลูกค้า");
                return;
            }
            sendApproveMessage("", 'btnConfirm', 'ส่งเอกสาร');
        }
        function confirmCancel() {
            sendApproveMessage("", 'btnCancel', 'ยกเลิกเอกสาร');
        }
        function backPage() {
            window.location.href = "AdjustProductList.aspx";
        }

        function popupQty() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/CheckProduct",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        startAdjustProduct();
                    } else {
                        swal(response.d);
                    }
                    $.LoadingOverlay("hide");
                },
                failure: function (response) {
                    $.LoadingOverlay("hide");
                }
            });
        }

        function startAdjustProduct() {
            swal({
                    title: "คุณต้องการปรับปรุงสินค้าใช่หรือไม่ ?",
                    icon: "success",
                    buttons: true,
                    dangerMode: true,
                })
                .then((willDelete) => {
                    if (willDelete) {
                        $('#btnConfirm').click();
                    }
                });
        }

        function changedProductNoID() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbProductNoID.GetValue();
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/GetProductName",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onChangedSuccess,
                failure: function (response) {

                }
            });
            $.LoadingOverlay("hide");
        }
        function onChangedSuccess(response) {
        }

        function callReport() {
            window.open("../Report/DocumentViewer.aspx?ReportArgs=Adjust_Product|" + $("#hdDataId").val(), "_blank");
        }

        function changedProductType() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbProductType.GetValue();
            if (key == "S") {
                $('#btnProduct').hide();
            }
            if (key == "P") {
                $('#btnProduct').show();
            }

            $.LoadingOverlay("hide");
        }

        function changedQuotation() {
            //var data = cbbQuotationNo;
            var key = cbbQuotation.GetValue();
            if (key != "0") {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });

                $.ajax({
                    type: "POST",
                    url: "AdjustProduct.aspx/GetQuotationData",
                    data: '{id: "' + key + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var data = response.d;
                        $('#txtSubject').val(data.quotation_subject);
                        $('#txtProject').val(data.project_name);
                        $('#dvProject').removeClass('hidden');
                        $('#txtPO').val(data.po_no);
                        cbbCustomer.SetValue(data.customer_id);
                    },
                    failure: function (response) {
                        console.log(response.d);
                        $.LoadingOverlay("hide");
                    }

                });

                $.LoadingOverlay("hide");
            } else {
                $('#dvProject').addClass('hidden');
            }
        }

        function moveUpListModel(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/MoveUpListModel",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function moveDownListModel(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("detailId");
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/MoveDownListModel",
                data: '{id: "' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridView.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("hide");
        }

        function popupMFGDetail(productId, adjDetailId) {
            $('#hdSelectedAdjDetailId').val(adjDetailId);
            var adjust_no = $('#txtSetNo').val();

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/GetMFGDetail",
                data: '{id:"' + productId + '", adjust_no: "' + adjust_no + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdSelectedProductId').val(productId);

                    gridViewMFG.PerformCallback();
                    $('#modal_adj_mfg').modal('show');
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
            var adjDetailId = $('#hdSelectedAdjDetailId').val();
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/AddMFGProduct",
                data: '{mfg_no:"' + key + '" , isSelected : ' + value + ' , product_id :"' + product_id + '" , adj_detail_id : "' + adjDetailId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewMFG.PerformCallback();
                    console.log("Add Complete");
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submitMFGProduct() {
            var adjDetailId = $('#hdSelectedAdjDetailId').val();
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/SubmitMFGProduct",
                data: "{adj_detail_id :" + adjDetailId + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        $('#hdSelectedAdjDetailId').val(0);
                        $('#hdSelectedProductId').val(0);
                        $('#modal_adj_mfg').modal('hide');
                    }
                    else {
                        alertMessage(response.d, "E");
                    }
                }
            });
        }

        function popupMFGDetail2(element, productId, detailId) {
            //var input = s.GetMainElement();
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#hdSelectedAdjDetailId').val(detailId);

            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/GetCustomMFGDetail",
                data: '{id: ' + productId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data.quantity_type == 0) {
                        $('#hdSelectedProductId').val(data.item_id);

                        $('#hdMaxQty').val(data.quantity);

                        setTimeout(function () {
                            gridMFG.PerformCallback();
                        }, 100);

                        $('#modal_custom_mfg .modal-header').html('รายการ : ' + element.title);
                        $('#modal_custom_mfg').modal('show');
                    } else {
                        popupMFGDetail(productId, detailId);
                    }
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
                return;
            }
            var product_id = $('#hdSelectedProductId').val();
            var adj_detail_id = $('#hdSelectedAdjDetailId').val()
            var maxQty = $('#hdMaxQty').val();
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/AddMFG",
                data: '{mfg_no : "' + mfg_no + '" , product_id : ' + product_id + ' , adj_detail_id : ' + adj_detail_id + ', maxQty: ' + maxQty + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        gridMFG.PerformCallback();

                        $('#txtMFGNo').val("");
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
                url: "AdjustProduct.aspx/EditMFG",
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
                            url: "AdjustProduct.aspx/DeleteMFG",
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
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/SubmitEditMFG",
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

        function submitCustomMFGProduct() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var adjDetailId = $('#hdSelectedAdjDetailId').val();
            $.ajax({
                type: "POST",
                url: "AdjustProduct.aspx/SubmitCustomMFGProduct",
                data: '{adjDetailId : "' + adjDetailId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data == "") {
                        $('#hdSelectedProductId').val(0);
                        $('#hdSelectedAdjDetailId').val(0)

                        gridMFG.PerformCallback();

                        $('#modal_custom_mfg').modal('hide');
                    } else {
                        swal(data);
                    }

                    $.LoadingOverlay("hide");
                }
            });
        }

    </script>
    <div id="div-content">
        <div class="row">
            <button type="button" runat="server" id="btnDraft" onclick="saveDraft()" class="btn-addItem">
                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
            </button>
            <button type="button" runat="server" id="btnSave" onclick="confirmSave()" class="btn-addItem hidden">
                <i class="fa fa-check-square-o" aria-hidden="true"></i>&nbsp;Confirm
            </button>
            <button type="button" runat="server" id="btnReportClient" onclick="callReport()" class="btn-addItem noDisabled">
                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp;Report
            </button>
            <button type="button" runat="server" id="btnBack" onclick="backPage()" class="btn-addItem">
                <i class="fa fa-list-ol" aria-hidden="true"></i>&nbsp;List Adjust
            </button>
            <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnSaveDraft" Style="visibility: hidden" UseSubmitBehavior="false" Text="Save" OnClick="btnSaveDraft_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnConfirm" Style="visibility: hidden" UseSubmitBehavior="false" Text="Confirm" OnClick="btnConfirm_Click"></dx:ASPxButton>
            <dx:ASPxButton runat="server" CssClass="btn-addItem noDisabled" ID="btnCancel" Style="visibility: hidden" UseSubmitBehavior="false" Text="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>

            <button type="button" runat="server" id="btnAdjust" onclick="popupQty()" class="btn-addItem pull-right" visible="false">
                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Adjust Now
            </button>
            <button type="button" runat="server" id="btnCancelAdj" onclick="confirmCancel()" class="btn-addItem pull-right" >
                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Cancel Adjust
            </button>
        </div>
        <div class="row">
            <fieldset>
                <legend>ใบปรับปรุง</legend>
                <div class="col-xs-12 no-padding">
                    <div class="row">
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Product Type :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbProductType" CssClass="form-control"
                                        runat="server" ClientInstanceName="cbbProductType" TextField="data_text"
                                        EnableCallbackMode="true" ClientSideEvents-ValueChanged="changedProductType"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">เลขที่ :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtSetNo" validate-data runat="server" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5 no-padding">
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
                        <div class="col-xs-3 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-5 text-right label-rigth">วันที่ :</label>
                                <div class="col-xs-6 no-padding">
                                    <input type="text" class="form-control" id="dtAdjustDate" runat="server" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-3 no-padding hidden">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">สโตร์ :</label>
                                <div class="col-xs-5 no-padding">
                                    <input type="text" class="form-control" id="txtStore" validate-data runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row hidden" id="dvProject">
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Project :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtProject" validate-data runat="server" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">เลขที่ใบสั่งซื้อ :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtPO" validate-data runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">ชื่อลูกค้า :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbCustomer" CssClass="form-control" runat="server"
                                        ClientInstanceName="cbbCustomer" TextField="data_text"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Subject :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtSubject" validate-data runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-5 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Remark :</label>
                                <div class="col-xs-9 no-padding">
                                    <input type="text" class="form-control" id="txtRemark2" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
            </fieldset>
        </div>
        <div class="row">
            <div class="row form-group">
                <button type="button" runat="server" id="btnProduct" onclick="addItemDetail('P')" class="btn-addItem">
                    <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;เลือกสินค้า
                </button>
                <button type="button" runat="server" id="btnSparePart" onclick="addItemDetail('S')" class="btn-addItem">
                    <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;เลือกอะไหล่
                </button>
            </div>

            <div class="col-xs-12 no-padding">
                <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
                    Width="100%" KeyFieldName="id" EnableCallBacks="true" OnCustomCallback="gridView_CustomCallback"
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
                        <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="50px">
                            <DataItemTemplate>
                                <a id="btnDelete" class="btn btn-mini <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_del) ? "" : "" %>"
                                    onclick="deleteAdjust(<%# Eval("id")%>)" title="Delete">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                    |
                                <a id="btnMoveUp" class="btn btn-mini" onclick="moveUpListModel(<%# Eval("id")%>)" title="MoveUp"<%# Container.ItemIndex == 0 ? " disabled=\"disabled\"" : "" %>>
                                    <i class="fa fa-arrow-up" aria-hidden="true"></i>
                                </a>|
                                <a id="btnMoveDown" class="btn btn-mini" onclick="moveDownListModel(<%# Eval("id")%>)" title="MoveDown">
                                    <i class="fa fa-arrow-down" aria-hidden="true"></i>
                                </a>|
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MFG Detail" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                            <DataItemTemplate>
                                <a id="btnMFG" class="btn btn-mini" onclick="popupMFGDetail2(this, <%# Eval("item_id")%>,<%# Eval("id")%>)" title="<%# Eval("item_no")%>" <%# Eval("item_type").ToString() == "S" ? " style=\"display:none;\"" : "" %>>
                                    <i class="fa fa-list" aria-hidden="true"></i>
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="item_no" Caption="Item No" Width="60px" />
                        <dx:GridViewDataTextColumn FieldName="item_name" Caption="Item Name" Width="120px" />
                        <dx:GridViewDataTextColumn FieldName="item_type" Caption="Item Type" Width="50px">
                            <DataItemTemplate>
                                <%# Eval("item_type").ToString() == "P" ? "Product" : "Spare Part" %>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <%-- <dx:GridViewDataSpinEditColumn PropertiesSpinEdit-AllowUserInput="true"
                            FieldName="quantity" Caption="Quantity" Width="60px" PropertiesSpinEdit-ClientSideEvents-NumberChanged="test()">
                            <PropertiesSpinEdit DisplayFormatString="n" DisplayFormatInEditMode="true" MinValue="0" MaxValue="99999999">
                                <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>--%>
                        <dx:GridViewDataTextColumn Caption="ประเภทการปรับ" FieldName="quantity" CellStyle-HorizontalAlign="Center" Width="50px">
                            <CellStyle VerticalAlign="Middle"></CellStyle>
                            <DataItemTemplate>
                                <dx:ASPxRadioButtonList ID="rdoAdjust" DetailId='<%# Eval("id")%>' runat="server" Border-BorderStyle="None">
                                    <Items>
                                        <dx:ListEditItem Value="0" Text="เพิ่ม" Selected />
                                        <dx:ListEditItem Value="1" Text="ลด" />
                                    </Items>
                                    <CaptionSettings Position="Top" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { 
                                                                        changedQuantityType(s, e); 
                                                                    }" />
                                </dx:ASPxRadioButtonList>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quantity" FieldName="quantity" CellStyle-HorizontalAlign="Center" Width="100px">
                            <CellStyle VerticalAlign="Middle"></CellStyle>
                            <DataItemTemplate>
                                <dx:ASPxSpinEdit runat="server" ID="txtQuantity" DetailId='<%# Eval("id")%>' AllowMouseWheel="false"
                                    DisplayFormatString="n0" NumberType="Integer" Width="100%" Height="100%" MinValue="0" MaxValue="99999" AllowUserInput="true">
                                    <ClientSideEvents NumberChanged="function(s, e) { 
                                                                        changedQuantity(s, e); 
                                                                    }"
                                        />
                                    <SpinButtons ShowIncrementButtons="false">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>

                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                    <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                    <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                        CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                </dx:ASPxGridView>
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
                                                        ProductId='<%# Eval("item_id")%>'>
                                                        <ClientSideEvents ValueChanged="function(s, e) { 
                                                                        getCheckBoxValue(s, e); 
                                                                    }" />
                                                    </dx:ASPxCheckBox>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataColumn FieldName="item_no" Caption="Item No" Width="100px" VisibleIndex="1" />
                                            <dx:GridViewDataColumn FieldName="item_name" Caption="Item Name" Width="200px" VisibleIndex="2" />
                                            <dx:GridViewDataColumn FieldName="quantity" Caption="Quantity" Width="50px" VisibleIndex="3" />


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

            <div class="modal fade" id="modal_adj_mfg" role="dialog" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            รายการ
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <dx:ASPxGridView ID="gridViewMFG" ClientInstanceName="gridViewMFG" runat="server" Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="400" EnableCallBacks="true"
                                        OnCustomCallback="gridViewMFG_CustomCallback"
                                        OnHtmlDataCellPrepared="gridViewMFG_HtmlDataCellPrepared"
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

            <div class="modal fade" id="modal_custom_mfg" role="dialog" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            รายการ
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div style="padding: 0 10px;">
                                    <fieldset>
                                        <legend style="text-align: left; margin-bottom: 5px;">MFG</legend>
                                        <div class="form-group col-xs-6 no-padding MFGDetailSearch">
                                            <label class="control-label col-xs-4 text-right" for="txtMFGNo">MFG No</label>
                                            <div class="col-xs-8 no-padding-right">
                                                <input type="text" class="form-control" id="txtMFGNo" runat="server" />
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
                                                                <a class="btn btn-mini" onclick="deleteMFG(<%# Eval("id")%>)" title="Delete" <%# Convert.ToInt32((Eval("id"))) > 0 ? "disabled='disabled'" : "" %>>
                                                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                                </a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="mfg_no" Caption="MFG No" Width="200px" CellStyle-HorizontalAlign="Left" />
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
                                        <button type="button" runat="server" id="Button5" onclick="submitCustomMFGProduct()" class="btn-app btn-addItem">ยืนยัน</button>
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
        </div>
        <div class="row">
            <asp:HiddenField runat="server" ID="hdDocStatus" />
            <asp:HiddenField runat="server" ID="hdDataId" />
            
            <asp:HiddenField runat="server" ID="hdSelectedAdjDetailId" />
            <asp:HiddenField runat="server" ID="hdSelectedProductId" />

            <asp:HiddenField runat="server" ID="hdMaxQty" />
        </div>
    </div>
</asp:Content>
