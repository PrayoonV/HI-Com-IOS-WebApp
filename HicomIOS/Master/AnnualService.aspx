<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AnnualService.aspx.cs" Inherits="HicomIOS.Master.AnnualService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
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

        fieldset {
            border: none !important;
        }

        .modal-lg .lg {
            width: 1000px !important;
        }

        #Splitter_0 {
            display: none;
        }

        .btn-setting {
            width: 130px;
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

        $(document).ready(function () {
            $("#Splitter_0").parent().hide();

            $("input:radio[value='Free Service']").attr('checked', true);

            $('.selectDate').datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });

            $('#modal_form').on('shown.bs.modal', function () {
                $('.nav-tabs li:first-child > a').trigger('click');
            });

            $("input[validate-data]").keyup(function () {
                if ($(this).val() != "") {
                    $(this).parent().parent().removeClass("has-error");
                }
            });

            var h = window.innerHeight;
            gridViewCustomerMFG.SetHeight(h - 250);

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

            var height = dimensions.height - 272;

            console.log(height);
            gridViewCustomerMFG.SetHeight(height);

            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });
        function changeSelectCustomerMFG() {
            /*$.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbCustomerMFG.GetValue();
            var project = "";
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetMFGData",
                data: '{id: "' + key + '", project: "' + project + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    cbbProject.PerformCallback();
                    gridViewCustomerMFG.PerformCallback();
                }

            });

            $.LoadingOverlay("hide");*/
            cbbProject.PerformCallback();
        }

        function changeSelectProject() {
            /*$.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = cbbCustomerMFG.GetValue();
            var project = cbbProject.GetValue() == null ? "" : cbbProject.GetValue();
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetMFGData",
                data: '{id: "' + key + '", project: "' + project + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridViewCustomerMFG.PerformCallback();
                }

            });

            $.LoadingOverlay("hide");*/
        }

        function searchDetail(e) {
            if (e == 13) {
                $.LoadingOverlay("show", {
                    zIndex: 9999
                });
                var key = cbbCustomerMFG.GetValue() == null ? "0" : cbbCustomerMFG.GetValue();
                var project = cbbProject.GetValue() == null ? "" : cbbProject.GetValue();
                var model = $('#txtModel').val();
                var mfgNo = $('#txtMFGNo').val()
                $.ajax({
                    type: "POST",
                    url: "AnnualService.aspx/GetMFGData",
                    data: '{id: "' + key + '", project: "' + project + '", model: "' + model + '", mfg_no: "' + mfgNo + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        gridViewCustomerMFG.PerformCallback();
                    }

                });

                $.LoadingOverlay("hide");
            }
        }

        function detailItem(e, s, t = "") {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $('#hdCustomerId').val(e);
            $('#hdCustomerMfgId').val(s);
            $('#hdfCustomerContract').val(t);

            var customer_id = e;
            var customer_mfg_id = s;

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetCustomerMfg",
                data: '{customer_id: "' + customer_id + '" ,customer_mfg_id: "' + customer_mfg_id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#customer-mfg-desc').html(response.d);
                }
            });

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetServiceYearData",
                data: '{customer_id: "' + customer_id + '" ,customer_mfg_id: "' + customer_mfg_id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#setting_date').val("");
                    $('#startingDate').val("");
                    $('#txtcontractstartdate').val("");
                    $('#txtcontractexpiredate').val("");
                    $('#expireDate').val("");
                    cboTypeContract.SetValue(" ");
                    GridAnnualServiceYear.PerformCallback();
                    setTimeout(function () {
                        if (t != "") {
                            $('#annualServiceYear .modal-title').html('Contract - ' + t);
                        }
                        $('#annualServiceYear').modal("show")
                    }, 500);

                }

            });

            $.LoadingOverlay("hide");
        }

        function setting(e, s, t) {
            var service_year_id = e
            var customer_id = $('#hdCustomerId').val();
            var customer_mfg_id = $('#hdCustomerMfgId').val();
            var customer_contract = $('#hdfCustomerContract').val();
            $('#hdMaster_id').val(s);

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetDetailItem",
                data: '{customer_id: "' + customer_id + '",customer_mfg_id: "' + customer_mfg_id + '" ,service_year_id: "' + service_year_id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        $('#Hdlimit_of_type').val(response.d["time_of_contract"]);
                        $('#Hdservice_master_id').val(response.d["service_master_id"]);
                    }

                    $('#Hdservice_year_id').val(e);
                    $('#Hdcustomer_id').val($('#hdCustomerId').val());
                    $('#hdcustomer_mfg_id').val($('#hdCustomerMfgId').val());
                    GridViewmasterList.PerformCallback();
                    gridFile.PerformCallback();

                    $('#modal_form .modal-title').html('Schedule - ' + t + ',' + customer_contract);
                    $('#modal_form').modal("show");
                    $('#annualServiceYear').modal('hide');

                }

            });
        }


        function addServiceYear() {
            var service_time = $('#service_time').val();
            var service_type = $('input[name=serviceType]:checked').val()

            var service_free = $('#service_free').val();
            var type_of_contract = cboTypeContract.GetValue();
            var type_of_contract_text = cboTypeContract.GetText();
            console.log("type_of_contract_text" + type_of_contract_text);
            var setting_date = $('#setting_date').val();
            var startingDate = $('#startingDate').val();
            var expireDate = $('#expireDate').val();
            var service_location = $('#service_location').val();
            var service_remark = $('#service_remark').val();
            var customer_id = $('#hdCustomerId').val();
            var customer_mfg_id = $('#hdCustomerMfgId').val();
            var contractstartdate = $('#txtcontractstartdate').val();
            var contractexpiredate = $('#txtcontractexpiredate').val();

            if (service_time == null || service_free == null || type_of_contract == null || expireDate == "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณากรอกข้อมูลให้ครบ");
                return;
            }
            else if ((service_time != "1" || service_type != "Free Service") && setting_date != "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณาลบ Test Run Date (วันเครื่องรัน) เนื่องจากไม่ใช้ Service Year : 1 , Service Type : Free Service");
                return;
            }
            else if (contractstartdate == "" || contractexpiredate == "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณากรอกข้อมูลวันที่สัญญา");
                return;
            } else {
                $.ajax({
                    type: "POST",
                    url: "AnnualService.aspx/AddServiceYear",
                    data: '{service_time: "' + service_time + '",service_free: "'
                        + service_free + '",service_remark: "' + service_remark + '",customer_id: "'
                        + customer_id + '",customer_mfg_id: "' + customer_mfg_id + '",service_type: "'
                        + service_type + '" ,type_of_contract: "' + type_of_contract + '",setting_date: "'
                        + setting_date + '",starting_date: "'
                        + startingDate + '",expire_date: "'
                        + expireDate + '",type_of_contract_text: "'
                        + type_of_contract_text + '", service_location: "' + service_location + '", contractstarting_date: "' + contractstartdate + '", contractexpire_date: "' + contractexpiredate + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "success") {
                            GridAnnualServiceYear.PerformCallback();

                            $('#service_time').val("");
                            //$('input[name=serviceType]:checked').val("")
                            $('#service_free').val("");
                            cboTypeContract.SetValue(" ");

                            $('#setting_date').val("");
                            $('#startingDate').val("");
                            $('#expireDate').val("");
                            $('#service_location').val("");
                            $('#service_remark').val("");
                            $('#txtcontractstartdate').val("");
                            $('#txtcontractexpiredate').val("");
                            //$('#hdCustomerId').val("");
                            //$('#hdCustomerMfgId').val("");
                        } else {
                            alertError("กรุณาเลือก Service Year ให้ถูกต้อง");
                            return;
                        }
                    }
                });
            }

        }

        function addmasterList(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var limitOf_type = $('#Hdlimit_of_type').val();
            $('#idShedule').val("");
            $('#txtservice_schedule_date').val("");
            $('#txtservice_date').val("");
            $('#txtservice_report_no').val("");
            $('#txtRemark').val("");
            $('#cbbstatusCheck').val('0');
            //cbbstatusCheck.SetValue("0");
            //$('#cbbstatusCheck').val("0");
            //$('#Hdlimit_of_type').val(s);


            //var TypeContract = cboTypeContract.GetValue();
            //var setting_date = $('#setting_date').val();
            //var startingDate = $('#startingDate').val();
            //var expireDate = $('#expireDate').val();
            //if (TypeContract == null || setting_date == "" || startingDate == "" || expireDate == "") {
            //    $.LoadingOverlay("hide");
            //    alert("กรุณากรอกข้อมูลให้ครบ");
            //    return;
            $('#Hdlimit').val(limitOf_type);


            $('#txtCheckingDate').val("");
            $('#txtRemark').val("");
            //$("#statusCheck").css('display', 'none');
            //$("#serviceDate").css('display', 'none');
            //$("#serviceReportNo").css('display', 'none');

            $('#modal_add_checking').modal("show")

            $.LoadingOverlay("hide");
        }

        function autoAddmasterList() {
            var key = $('#hdMaster_id').val();
            var limit_type = $('#Hdlimit_of_type').val();

            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/AutoAddChecking",
                data: '{key : "' + key + '", limit_type : "' + limit_type + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == 0) {
                        GridViewmasterList.PerformCallback();

                        $.LoadingOverlay("hide");
                    } else {
                        swal("ไม่สามารถเพิ่มข้อมูลได้", {
                            icon: "error"
                        })
                            .then((value) => {
                                GridViewmasterList.PerformCallback();

                                $('#modal_add_checking').modal("hide")
                                $.LoadingOverlay("hide");
                            });
                    }
                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submit_add_checking() {
            if ($("#txtservice_schedule_date").val() == "") {
                $("#txtservice_schedule_date").parent().parent().addClass("has-error");
                $("#txtservice_schedule_date").focus();

                return false;
            } else {
                $("#txtservice_schedule_date").parent().parent().removeClass("has-error");
            }

            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = $('#idShedule').val();
            var service_master_id = $('#Hdservice_master_id').val();
            var statusCheck = $('#cbbstatusCheck').val(); //สถานะตรวจสอบShedule
            var limit_type = $('#Hdlimit_of_type').val();
            var service_date = $('#txtservice_date').val();
            var service_report_no = $('#txtservice_report_no').val();
            var CheckingDate = $('#txtservice_schedule_date').val();//$('#txtCheckingDate').val();
            var Remark = $('#txtRemark').val();
            console.log(" key ==" + key);
            console.log(" service_master_id ==" + service_master_id);
            console.log("  statusCheck==" + statusCheck);
            console.log(" limit_type ==" + limit_type);
            console.log(" CheckingDate ==" + CheckingDate);
            console.log(" Remark ==" + Remark);
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/AddChecking",
                data: '{id : "' + key + '",CheckingDate : "' + CheckingDate + '",Remark : "' + Remark + '",service_master_id: "' + service_master_id + '",statusCheck: "' + statusCheck + '",limit_type: "' + limit_type + '",service_date: "' + service_date + '",service_report_no: "' + service_report_no + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == 0) {
                        GridViewmasterList.PerformCallback();

                        //  Clear text
                        $('#txtservice_schedule_date').val("");
                        $('#txtservice_date').val("");
                        $('#cbbstatusCheck').val("0");
                        $('#txtservice_report_no').val("");
                        $('#txtRemark').val("");

                        $('#modal_add_checking').modal("hide")
                        $.LoadingOverlay("hide");
                    } else {
                        swal("ไม่สามารถเพิ่มข้อมูลได้ เกินจำนวนครั้งที่กำหนด!", {
                            icon: "error"
                        })
                            .then((value) => {
                                GridViewmasterList.PerformCallback();

                                $('#modal_add_checking').modal("hide")
                                $.LoadingOverlay("hide");
                            });
                        ///////

                    }
                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function editServiceYear(e, s) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/SelectServiceYear",
                data: '{id : ' + e + ', service_master_id : ' + s + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#service_time_edit').val(data.service_time);
                    $('#selectServiceFree').val(data.service_free); //Year
                    $('#HDservice_masterId').val(data.service_master_id);
                    $('#service_yearId').val(data.service_year_id);
                    $('#Hdservice_year_id').val(data.service_year_id);
                    console.log("data.service_type==" + data.service_type);
                    if (data.service_type == "Free Service") {
                        $("input:radio[id='rdoFreeService_edit']").attr('checked', true);
                        console.log("1==" + data.service_type);
                    } else if (data.service_type == "Contract") {
                        $("input:radio[id='rdoContract_edit']").attr('checked', true);
                        console.log("2==" + data.service_type);
                    } else if (data.service_type == "Contract (Out Source)") {
                        console.log("3==" + data.service_type);
                        // $("input:radio[value='Contract(OurSource)_edit']").attr('checked', true);
                        $("input:radio[id='rdoOurSource_edit']").attr('checked', true);
                    }
                    $('#type_of_contract_text').val(data.type_name);
                    $('#setting_date_edit').val(data.setting_date);
                    $('#contract_start_edit').val(data.contractstarting_date);
                    $('#contract_end_edit').val(data.contractexpire_date);
                    $('#starting_date_edit').val(data.starting_date);
                    $('#expire_date_edit').val(data.expire_date);
                    $('#location_service_year').val(data.service_location);
                    $('#remark_service_year').val(data.service_remark);
                    $('#modal_show_service_year').modal("show")

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }
        //function changedcboTypeContract() {
        //    var key = cboTypeContract.GetValue();
        //    $.ajax({
        //        type: "POST",
        //        url: "AnnualService.aspx/CleaSessionMaeter",
        //        data: {},
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //        }

        //    });
        //    $.ajax({
        //        type: "POST",
        //        url: "AnnualService.aspx/GetLimitOfType",
        //        data: '{id: "' + key + '" }',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            $('#Hdlimit_of_type').val(response.d);
        //            $('#setting_date').val("");
        //            $('#startingDate').val("");
        //            $('#expireDate').val("");
        //            GridViewmasterList.PerformCallback();
        //        },
        //        failure: function (response) {

        //        }
        //    });
        //}
        function submit_EditserviceYear() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var key = $('#Hdservice_year_id').val();
            var service_master_id = $('#HDservice_masterId').val();

            var service_time = $('#service_time_edit').val();
            var setting_date = $('#setting_date_edit').val();
            var contractstarting_date = $('#contract_start_edit').val();
            var contractexpire_date = $('#contract_end_edit').val();
            var starting_date = $('#starting_date_edit').val();
            var expire_date = $('#expire_date_edit').val();
            var serviceFree = $('#selectServiceFree').val(); // YEAR
            var service_location = $('#location_service_year').val();
            var remark = $('#remark_service_year').val();
            var service_type = $('input[name=serviceType_edit]:checked').val()
            console.log("service_type==" + service_type);


            if ((service_time != "1" || service_type != "Free Service") && setting_date != "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณาลบ Test Run Date (วันเครื่องรัน) เนื่องจากไม่ใช้ Service Year : 1 , Service Type : Free Service");
                return;
            }
            else if (contractstarting_date == "" || contractexpire_date == "") {
                $.LoadingOverlay("hide");
                alertWarning("กรุณากรอกข้อมูลวันที่สัญญา");
                return;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "AnnualService.aspx/EditserviceYear",
                    data: '{id : "' + key +
                        '",service_time : ' + service_time + ',serviceFree : "' + serviceFree + '",remark : "'
                        + remark + '",setting_date : "' + setting_date + '",starting_date : "' + starting_date +
                        '",expire_date : "' + expire_date + '",service_type : "' + service_type + '", service_location: "' + service_location + '", contractstarting_date: "' + contractstarting_date + '", contractexpire_date: "' + contractexpire_date + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "") {
                            GridAnnualServiceYear.PerformCallback();

                            $('#modal_show_service_year').modal("hide");
                        } else {
                            alertError("กรุณาเลือก Service Year ให้ถูกต้อง");
                        }
                        $.LoadingOverlay("hide");
                    },
                    failure: function (response) {
                        alertWarning(response.d);
                        $.LoadingOverlay("hide");
                    }
                });
            }
        }

        function editFile(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/SelectEditeditFile",
                data: '{id : ' + e + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#hdMaster_id').val(data.service_master_id);
                    $('#txtRunningNoEdit').val(data.running_no);
                    $('#txtDescription').val(data.description);
                    $('#txtstarting_date_edit').val(data.starting_date);
                    $('#txtmfg_upload_edit').val(data.mfg_number);
                    $('#txtmodel_upload_edit').val(data.model_number);
                    $('#btnsubmit_edit_file').val(e);
                    $('#modal_edit_file_name').modal("show")
                },
            });
            $.LoadingOverlay("hide");
        }
        function submit_edit_file_name() {
            var id = $('#btnsubmit_edit_file').val();
            var running_no = $('#txtRunningNoEdit').val();
            var description = $('#txtDescription').val();
            var starting_date = $('#txtstarting_date_edit').val();
            var mfg_number = $('#txtmfg_upload_edit').val();
            var model_number = $('#txtmodel_upload_edit').val();

            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/SubmitEditeditFile",
                data: '{id : ' + id + ' ,description : "' + description + '" ,running_no : "' + running_no + '" ,starting_date : "' + starting_date + '",mfg_number : "' + mfg_number + '",model_number : "' + model_number + '"  }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    gridFile.PerformCallback();
                    $('#modal_edit_file_name').modal("hide")

                },
            });

        }


        function editMasterList(e, s) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            $('#idShedule').val(e);
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/SelectEditChecking",
                data: '{id : ' + e + ',limit_type:' + s + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    $('#txtservice_schedule_date').val(data.checking_date);
                    $('#txtservice_date').val(data.service_date);
                    $('#txtservice_report_no').val(data.service_report_no);
                    $('#txtRemark').val(data.checking_remark);
                    //cbbstatusCheck.SetValue();
                    $('#cbbstatusCheck').val(data.checking_status);

                    $('#Hdservice_master_id').val(data.service_master_id);
                    $('#idShedule').val(e);
                    if ($('#idShedule').val() > 0) {
                        $("#statusCheck").css('display', '');
                        $("#serviceDate").css('display', '');
                        $("#serviceReportNo").css('display', '');

                    }
                    $('#Hdlimit_of_type').val(s);
                    $('#modal_add_checking').modal("show")

                    $.LoadingOverlay("hide");

                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });

        }

        function deleteMasterList(e) {
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
                            url: "AnnualService.aspx/DeleteMasterList",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        GridViewmasterList.PerformCallback();
                                    });
                            },
                            failure: function (response) {
                                alertWarning(response.d);
                            }
                        });
                    }
                });

        }
        function deleteServiceYear(e) {
            var id = e;
            GridAnnualServiceYear.PerformCallback();

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
                            url: "AnnualService.aspx/DeleteServiceYear",
                            data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        GridAnnualServiceYear.PerformCallback();
                                    });


                            },
                            failure: function (response) {
                                alertWarning(response.d);
                            }
                        });
                    }
                });
        }

        function deleteFile(e) {

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
                            url: "AnnualService.aspx/DeleteFile",
                            data: '{id: "' + e + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("ลบข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        gridFile.PerformCallback();
                                    });


                            },
                            failure: function (response) {
                                alertWarning(response.d);
                            }
                        });
                    }
                });
        }

        function closeForm() {
            $('#modal_form').modal("hide");
            $('#annualServiceYear').modal("show");
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/CleaSessionMaeter",
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                }
            });
            cboTypeContract.SetText("");
            $('#setting_date').val("");
            $('#startingDate').val("");
            $('#Hdlimit_of_type').val("");
            $('#Hdservice_master_id').val("");
            cboTypeContract.SetEnabled(true);
            $('#expireDate').val("");


            GridViewmasterList.PerformCallback();
            gridFile.PerformCallback();
        }

        function submitForm() {
            var TypeContract = cboTypeContract.GetValue();

            console.log("parseInt(cboTypeContract.GetValue())==" + parseInt(cboTypeContract.GetValue()));
            console.log("$('#Hdlimit_of_type').val()==" + $('#Hdlimit_of_type').val());
            var parametersAdd = {
                masterData: [
                    {
                        time_of_contract: $('#Hdlimit_of_type').val(),
                        service_year_id: $('#Hdservice_year_id').val() == "" ? "0" : $('#Hdservice_year_id').val(),
                        customer_id: $('#Hdcustomer_id').val() == "" ? "0" : $('#Hdcustomer_id').val(),
                        customer_mfg_id: $('#hdcustomer_mfg_id').val() == "" ? "0" : $('#hdcustomer_mfg_id').val(),
                        service_master_id: $('#Hdservice_master_id').val() == "" ? "0" : $('#Hdservice_master_id').val(),
                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: "AnnualService.aspx/SubmitData",
                data: JSON.stringify(parametersAdd),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d > 0) {
                        console.log("success");


                        $.ajax({
                            type: "POST",
                            url: "AnnualService.aspx/CleaSession",
                            data: {},
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                swal("บันทึกข้อมูลสำเร็จ!", {
                                    icon: "success"
                                })
                                    .then((value) => {
                                        $('#modal_form').modal("hide");
                                        detailItem($('#Hdcustomer_id').val(), $('#hdcustomer_mfg_id').val())

                                        //window.location.href = "AnnualService.aspx?dataId=" + result.d;
                                    });
                            }
                        });

                    }
                    else {
                        swal("บันทึกข้อมูลผิดพลาด!", {
                            icon: "error"
                        })
                    }
                }
            });
        }

        function submitServiceYear() {
            var customer_id = $('#hdCustomerId').val();
            $.ajax({
                type: 'POST',
                url: "AnnualService.aspx/SubmitServiceYear",
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d != " ") {
                        swal("บันทึกข้อมูลสำเร็จ!", {
                            icon: "success"
                        })
                            .then((value) => {
                                var customer_mfg_id = result.d;
                                $.ajax({
                                    type: "POST",
                                    url: "AnnualService.aspx/CleaSession",
                                    data: {},
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        detailItem(customer_id, customer_mfg_id)

                                    }

                                });

                            });
                    }
                    else {
                        swal("กรุณาสร้าง ServiceYear!", {
                            icon: "error"
                        })

                    }
                }
            });
        }

        function modalDoc(id, check_date) {
            $('#hdSchedule_id').val(id);
            var service_master_id = $('#hdMaster_id').val();
            var customer_contract = $('#hdfCustomerContract').val();
            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/GetFile",
                data: '{schedule_id: ' + id + ', service_master_id: ' + service_master_id + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $.LoadingOverlay("hide");

                    gridFile.PerformCallback();

                    $('#modal_upload_doc #modal-sub-title').html(check_date + ' , ' + customer_contract);
                    $('#modal_upload_doc').modal();
                },
                failure: function (response) {
                    $.LoadingOverlay("hide");
                    alertWarning(response.d);
                }
            });
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
        }

        function onUploadFilePdf(s, e) {
            var description = $('#txtFileName').val();
            var running_no = $('#txtRunningNo').val();
            var type = $('#cbbReportType').val();
            var service_master_id = $('#hdMaster_id').val();
            var schedule_id = $('#hdSchedule_id').val();
            var starting_date = $('#txtstarting_date').val();
            var mfg_number = $('#txtmfg_upload').val();
            var model_number = $('#txtmodel_upload').val();


            $.ajax({
                type: "POST",
                url: "AnnualService.aspx/UpdateDescriptionFile",
                data: '{id : ' + parseInt(e.callbackData) + ' ,description : "' + description + '" ,running_no : "' + running_no + '" ,type : ' + type + ', service_master_id : ' + service_master_id + ', schedule_id : ' + schedule_id
                    + ',starting_date : "' + starting_date + '",mfg_number : "' + mfg_number + '",model_number : "' + model_number + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#txtFileName').val("");
                    $('#txtRunningNo').val("");
                    $('#cbbReportType').val("1");
                    $('#txtstarting_date').val("");
                    $('#txtmfg_upload').val("");
                    $('#txtmodel_upload').val("");
                    gridFile.PerformCallback();
                },
                failure: function (response) {
                    alertWarning(response.d);
                    $.LoadingOverlay("hide");
                }
            });
        }

        function submitFile() {
            var TypeContract = cboTypeContract.GetValue();
            var parametersAdd = {
                masterData: [
                    {
                        time_of_contract: $('#Hdlimit_of_type').val(),
                        service_year_id: $('#Hdservice_year_id').val() == "" ? "0" : $('#Hdservice_year_id').val(),
                        customer_id: $('#Hdcustomer_id').val() == "" ? "0" : $('#Hdcustomer_id').val(),
                        customer_mfg_id: $('#hdcustomer_mfg_id').val() == "" ? "0" : $('#hdcustomer_mfg_id').val(),
                        service_master_id: $('#Hdservice_master_id').val() == "" ? "0" : $('#Hdservice_master_id').val(),
                    }
                ]
            };

            $.ajax({
                type: 'POST',
                url: "AnnualService.aspx/SubmitFile",
                data: JSON.stringify(parametersAdd),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d > 0) {
                        swal("บันทึกข้อมูลสำเร็จ!", {
                            icon: "success"
                        })
                            .then((value) => {
                                $('#modal_upload_doc').modal("hide");
                            });
                    }
                    else {
                        swal("บันทึกข้อมูลผิดพลาด!", {
                            icon: "error"
                        })
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        $('#gridViewCustomerMFG').css({
            'height': '550px;'
        });
    </script>
    <div id="div-content">
        <div class="row">
            <fieldset>
                <legend>Annual Service</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">ลูกค้า :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbCustomerMFG" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbCustomerMFG" TextField="data_text"
                                    ClientSideEvents-ValueChanged="changeSelectCustomerMFG" ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Project :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbProject" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbProject" TextField="data_text"
                                    OnCallback="cbbProject_Callback"
                                    ClientSideEvents-ValueChanged="changeSelectProject" ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Model :</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control" id="txtModel" runat="server" onkeypress="searchDetail(event.keyCode)" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">MFG No :</label>
                            <div class="col-xs-9 no-padding">
                                <input type="text" class="form-control" id="txtMFGNo" runat="server" onkeypress="searchDetail(event.keyCode)" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">&nbsp;</label>
                            <div class="col-xs-9 no-padding">
                                <button type="button" runat="server" onclick="searchDetail(13)"
                                    class="btn-info btn-addItem">
                                    <i class="fa fa-search" aria-hidden="true"></i>&nbsp;ค้นหา
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="row">
            <div class="col-xs-12" runat="server" id="dvGridProduct">
                <dx:ASPxGridView ID="gridViewCustomerMFG" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridViewCustomerMFG"
                    Width="100%" KeyFieldName="id" EnableCallBacks="true"
                    OnCustomCallback="gridViewCustomerMFG_CustomCallback">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                        PageSizeItemSettings-Visible="true">
                        <PageSizeItemSettings Items="10, 20, 50" />
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="customer_code" Caption="Customer code" Width="40px" />
                        <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer name" Width="100px" />
                        <dx:GridViewDataTextColumn FieldName="project" Caption="Project" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="model" Caption="Model" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="mfg" Caption="MFG No" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="delivery_date" Caption="Delivery Date" Width="50px" />
                        <dx:GridViewDataTextColumn FieldName="startup_date" Caption="Test Run Date" Width="50px" />
                        <dx:GridViewDataTextColumn Caption=" " FieldName="id" CellStyle-HorizontalAlign="Center" Width="55px" Settings-AllowSort="False">
                            <DataItemTemplate>
                                <button type="button" id="btnviewReceive" onclick="detailItem(<%# Eval("customer_id")%>, '<%# Eval("customer_mfg_id")%>', 'Model : <%# Eval("model")%>, MFG number : <%# Eval("mfg")%>')" class="btn-addItem btn-setting">
                                    <i class="fas fa-calendar-check"></i>&nbsp;Setting Contract
                                </button>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                    <SettingsPopup>
                        <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
                    </SettingsPopup>
                    <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                        CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                </dx:ASPxGridView>

            </div>
        </div>
    </div>
    <!-- MODAL -->
    <div class="modal fade" id="modal_form" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">Annual Service</div>
                </div>
                <div class="modal-body text-center" style="padding: 10px;">
                    <div class="row">
                        <div class="col-xs-12">
                            <fieldset>
                                <div class="row">
                                    <div class="form-group col-xs-12 text-left">
                                        <button type="button" class="btn-addItem" onclick="addmasterList()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;Add Service Schedule 
                                        </button>
                                        <button type="button" class="btn-addItem" onclick="autoAddmasterList()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;Auto Schedule 
                                        </button>
                                    </div>
                                </div>
                                <div class="form-group col-xs-12 no-padding">
                                    <dx:ASPxGridView ID="GridViewmasterList" SettingsBehavior-AllowSort="false" ClientInstanceName="GridViewmasterList" runat="server"
                                        Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="300"
                                        EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="GridViewmasterList_CustomCallback"
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
                                            <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="40">
                                                <DataItemTemplate>
                                                    <a class="btn btn-mini" onclick="editMasterList(<%# Eval("id")%> ,<%# Eval("limit_type")%>)" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deleteMasterList(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="checking_date" Caption="Service Schedule Date" CellStyle-HorizontalAlign="Left" Width="100" />
                                            <dx:GridViewDataTextColumn FieldName="service_date" Caption="Service Date" CellStyle-HorizontalAlign="Left" Width="100" />

                                            <dx:GridViewDataTextColumn FieldName="status_name" Caption="Status" CellStyle-HorizontalAlign="Left" Width="80" />
                                            <dx:GridViewDataTextColumn FieldName="service_report_no" Caption="Service Report No" CellStyle-HorizontalAlign="Left" Width="100" />
                                            <dx:GridViewDataTextColumn FieldName="checking_remark" Caption="Remark" CellStyle-HorizontalAlign="Left" Width="100" />
                                            <dx:GridViewDataTextColumn Caption="Document" FieldName="id" CellStyle-HorizontalAlign="Center" Width="100">
                                                <DataItemTemplate>
                                                    <button type="button" id="btnDoc" onclick="modalDoc(<%# Eval("id")%>, '<%# Eval("checking_date")%>')" class="btn-addItem<%# (Convert.ToInt32(Eval("id")) < 0 ? " hidden" : "") %>">
                                                        <i class="fas fa-calendar-check"></i>&nbsp;Upload
                                                    </button>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <input hidden="hidden" id="Hdlimit_of_type" runat="server" />
                    <input hidden="hidden" id="service_yearId" runat="server" />
                    <input hidden="hidden" id="Hdcustomer_id" runat="server" />
                    <input hidden="hidden" id="hdcustomer_mfg_id" runat="server" />
                    <input hidden="hidden" id="Hdservice_master_id" runat="server" />
                    <%--<input hidden="hidden"  id="editMasterListId" runat="server" />--%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitForm()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" onclick="closeForm()">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>
    <!-- modal-->
    <div class="modal fade" id="modal_add_checking" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">เพิ่มรายการ การตรวจสอบ</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2 no-padding" for="txtAddress">Service Schedule Date:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control selectDate" id="txtservice_schedule_date" runat="server" autocomplete="off" validate-data />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding" id="serviceDate">
                            <label class="control-label col-xs-2 no-padding">Service Date:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control selectDate" id="txtservice_date" runat="server" autocomplete="off" />
                            </div>
                        </div>

                        <div class="form-group col-xs-12 no-padding" id="statusCheck">
                            <label class="control-label col-xs-2 no-padding" for="txtstatusCheck">Status:</label>
                            <div class="col-xs-10">
                                <select class="form-control" id="cbbstatusCheck" runat="server">
                                    <option value="0">รอเข้าบริการ</option>
                                    <option value="1">ให้บริการเรียบร้อยแล้ว</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding" id="serviceReportNo">
                            <label class="control-label col-xs-2 no-padding">Service Report No:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control" id="txtservice_report_no" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2 no-padding" for="txtAddress">Remark:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control" id="txtRemark" runat="server" />
                            </div>
                        </div>

                    </div>
                    <input hidden="hidden" id="idShedule" runat="server" />
                    <input hidden="hidden" id="Hdlimit" runat="server" />
                    <%--idของรายการเข้าตรวจสอบ modalเล็กที่2--%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submit_add_checking()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_show_service_year" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1" style="z-index: 3000;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">แก้ไขรายการ</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding">Service Year (ครั้งที่) :</label>
                            <div class="col-xs-6">
                                <select class="form-control" id="service_time_edit" runat="server">
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                    <option value="21">21</option>
                                    <option value="22">22</option>
                                    <option value="23">23</option>
                                    <option value="24">24</option>
                                    <option value="25">25</option>
                                    <option value="26">26</option>
                                    <option value="27">27</option>
                                    <option value="28">28</option>
                                    <option value="29">29</option>
                                    <option value="30">30</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding">Service Type :</label>
                            <div class="col-xs-6">
                                <div class="form-inline shipping-step">
                                    <label class="radio-inline">
                                        <input type="radio" value="Free Service" id="rdoFreeService_edit" name="serviceType_edit">&nbsp;Free Service
                                    </label>
                                </div>
                                <div class="form-inline shipping-step">
                                    <label class="radio-inline">
                                        <input type="radio" value="Contract" id="rdoContract_edit" name="serviceType_edit">&nbsp;Contract
                                    </label>
                                </div>
                                <div class="form-inline shipping-step">
                                    <label class="radio-inline">
                                        <input type="radio" value="Contract (Out Source)" id="rdoOurSource_edit" name="serviceType_edit">&nbsp;Contract (Out Source) 
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding">Year :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control numberic" id="selectServiceFree" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtAddress">Type Of Contract :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control" disabled="disabled" id="type_of_contract_text" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding">Test Run Date :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control selectDate" id="setting_date_edit" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Contract Start Date :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control selectDate" id="contract_start_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Contract Expire Date :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control selectDate" id="contract_end_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Starting Date :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control selectDate" id="starting_date_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding">Expire Date :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control selectDate" id="expire_date_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtAddress">Location :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control" id="location_service_year" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-4 no-padding" for="txtAddress">Remark :</label>
                            <div class="col-xs-6">
                                <input type="text" class="form-control" id="remark_service_year" runat="server" />
                            </div>
                        </div>
                    </div>
                    <input hidden="hidden" id="Hdservice_year_id" runat="server" />
                    <input hidden="hidden" id="HDservice_masterId" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submit_EditserviceYear()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal แทรก-->
    <div class="modal fade" id="annualServiceYear" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">Annual Service Year</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div style="padding: 0 10px;">
                            <fieldset>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <h5 class="no-padding" id="customer-mfg-desc"></h5>
                                    </div>
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Service Year (ครั้งที่) :</label>
                                        <div class="col-xs-6">
                                            <select class="form-control" id="service_time" runat="server">
                                                <option value="1">1</option>
                                                <option value="2">2</option>
                                                <option value="3">3</option>
                                                <option value="4">4</option>
                                                <option value="5">5</option>
                                                <option value="6">6</option>
                                                <option value="7">7</option>
                                                <option value="8">8</option>
                                                <option value="9">9</option>
                                                <option value="10">10</option>
                                                <option value="11">11</option>
                                                <option value="12">12</option>
                                                <option value="13">13</option>
                                                <option value="14">14</option>
                                                <option value="15">15</option>
                                                <option value="16">16</option>
                                                <option value="17">17</option>
                                                <option value="18">18</option>
                                                <option value="19">19</option>
                                                <option value="20">20</option>
                                                <option value="21">21</option>
                                                <option value="22">22</option>
                                                <option value="23">23</option>
                                                <option value="24">24</option>
                                                <option value="25">25</option>
                                                <option value="26">26</option>
                                                <option value="27">27</option>
                                                <option value="28">28</option>
                                                <option value="29">29</option>
                                                <option value="30">30</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Service Type :</label>
                                        <div class="col-xs-6">
                                            <div class="form-inline shipping-step">
                                                <label class="radio-inline">
                                                    <input type="radio" value="Free Service" id="rdoFreeService" name="serviceType">&nbsp;Free Service
                                                </label>
                                            </div>
                                            <div class="form-inline shipping-step">
                                                <label class="radio-inline">
                                                    <input type="radio" value="Contract" id="rdoContract" name="serviceType">&nbsp;Contract
                                                </label>
                                            </div>
                                            <div class="form-inline shipping-step">
                                                <label class="radio-inline">
                                                    <input type="radio" value="Contract (Out Source)" id="rdoOurSource" name="serviceType">&nbsp;Contract (Out Source) 
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Year (ระยะเวลา) :</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control numberic" id="service_free" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding"
                                            <%--for="txtProductNameTHA"--%>>
                                            Type Of Contract (เข้า Service กี่ครั้งต่อปี):</label>
                                        <div class="col-xs-6">
                                            <dx:ASPxComboBox ID="cboTypeContract" CssClass="form-control" runat="server"
                                                ClientInstanceName="cboTypeContract" TextField="data_text"
                                                EnableCallbackMode="true"
                                                ValueField="data_value">
                                                <%--OnCallback="cboSupplier_Callback"--%>
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Test Run Date (วันเครื่องรัน):</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control selectDate" id="setting_date" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Contract Start Date (วันที่เริ่มสัญญา):</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control selectDate" id="txtcontractstartdate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Contract Expire Date (วันที่สิ้นสุดสัญญา):</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control selectDate" id="txtcontractexpiredate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding" for="txtProductNameTHA">Starting Date (วันเริ่มเข้าบริการ):</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control selectDate" id="startingDate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Expire Date (วันครบสัญญาบริการ):</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control selectDate" id="expireDate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Location :</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control" id="service_location" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-12 no-padding">
                                        <label class="control-label col-xs-4 no-padding">Remark :</label>
                                        <div class="col-xs-6">
                                            <input type="text" class="form-control" id="service_remark" runat="server" />
                                        </div>

                                        <button type="button" class="btn-addItem" onclick="addServiceYear()">
                                            <i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp;สร้าง
                                        </button>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <dx:ASPxGridView ID="GridAnnualServiceYear" SettingsBehavior-AllowSort="false" ClientInstanceName="GridAnnualServiceYear" runat="server"
                                        Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="300"
                                        EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="GridAnnualServiceYear_CustomCallback"
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
                                            <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="30px">
                                                <DataItemTemplate>
                                                    <a class="btn btn-mini" onclick="editServiceYear(<%# Eval("service_year_id")%>, '<%# Eval("service_master_id")%>')" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deleteServiceYear(<%# Eval("service_year_id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="service_time" Caption="Service Year" CellStyle-HorizontalAlign="Left" Width="45px" />
                                            <dx:GridViewDataTextColumn FieldName="service_type_text" Caption="Service Type" CellStyle-HorizontalAlign="Left" Width="70px" />
                                            <dx:GridViewDataTextColumn FieldName="service_free" Caption="Year" CellStyle-HorizontalAlign="Left" Width="30px" />
                                            <dx:GridViewDataTextColumn FieldName="type_name" Caption="Type Of Contract" CellStyle-HorizontalAlign="Left" Width="60px" />

                                            <dx:GridViewDataTextColumn FieldName="setting_date" Caption="Test Run Date" CellStyle-HorizontalAlign="Left" Width="50px" />
                                            <dx:GridViewDataTextColumn FieldName="starting_date" Caption="Starting Date" CellStyle-HorizontalAlign="Left" Width="50px" />

                                            <dx:GridViewDataTextColumn FieldName="expire_date" Caption="Expire Date" CellStyle-HorizontalAlign="Left" Width="50px" />
                                            <dx:GridViewDataTextColumn Caption="Action" FieldName="id" CellStyle-HorizontalAlign="Center" Width="55px">
                                                <DataItemTemplate>
                                                    <button type="button" id="btnSetting" onclick="setting(<%# Eval("service_year_id")%>, <%# Eval("service_master_id")%>, '<%# Eval("service_type_text")%>, <%# Eval("type_name")%>')" class="btn-addItem">
                                                        <i class="fas fa-calendar-check"></i>&nbsp;Setting
                                                    </button>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <input hidden="hidden" id="hdCustomerId" runat="server" />
                    <input hidden="hidden" id="hdCustomerMfgId" runat="server" />
                    <input hidden="hidden" id="hdfCustomerContract" runat="server" />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitServiceYear()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Upload doc -->
    <div class="modal fade" id="modal_upload_doc" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">Upload document - <span id="modal-sub-title"></span></div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div style="padding: 0 10px;">
                            <fieldset>
                                <div class="form-group col-xs-12 no-padding">
                                    <label class="control-label col-xs-2" for="cboSupplier">Running no: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <input type="text" class="form-control" id="txtRunningNo" runat="server" />
                                    </div>
                                    <label class="control-label col-xs-2" for="cboSupplier">รายละเอียด: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <input type="text" class="form-control" id="txtFileName" runat="server" />
                                    </div>
                                    <div class="clearfix">&nbsp;</div>
                                    <label class="control-label col-xs-2" for="cboSupplier">ประเภท: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <select class="form-control" id="cbbReportType" runat="server">
                                            <option value="1">Service</option>
                                            <option value="2">Part</option>
                                            <option value="3">Repairing</option>
                                            <option value="4">Test run/Contract</option>
                                        </select>
                                    </div>
                                    <label class="control-label col-xs-2" for="cboSupplier">วันที่ทำงานจริง: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <input type="text" class="form-control selectDate" id="txtstarting_date" runat="server" />
                                    </div>
                                    <div class="clearfix">&nbsp;</div>
                                    <label class="control-label col-xs-2" for="cboSupplier">MFG Number: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <input type="text" class="form-control" id="txtmfg_upload" runat="server" />
                                    </div>
                                    <label class="control-label col-xs-2" for="cboSupplier">Model Number: </label>
                                    <div class="col-xs-4  no-padding-right">
                                        <input type="text" class="form-control" id="txtmodel_upload" runat="server" />
                                    </div>
                                    <div class="clearfix">&nbsp;</div>
                                    <label class="control-label col-xs-2 no-padding" for="cboSupplier">Upload File: </label>
                                    <div class="col-xs-4">
                                        <dx:ASPxUploadControl ID="uploadPdf" ClientInstanceName="uploadPdf" runat="server" UploadMode="Auto" AutoStartUpload="True" Width="100%"
                                            ShowProgressPanel="True" CssClass="uploadControl" DialogTriggerID="externalDropZone" OnFileUploadComplete="upload_FilePdfUploadComplete">
                                            <AdvancedModeSettings EnableDragAndDrop="false" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZone" DropZoneText="" />
                                            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".pdf" ErrorStyle-CssClass="validationMessage">
                                                <ErrorStyle CssClass="validationMessage"></ErrorStyle>
                                            </ValidationSettings>
                                            <BrowseButton Text="Upload" />
                                            <DropZoneStyle CssClass="uploadControlDropZone" />
                                            <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                            <ClientSideEvents
                                                FileUploadComplete="onUploadFilePdf"></ClientSideEvents>
                                        </dx:ASPxUploadControl>
                                    </div>
                                </div>
                                <div class="form-group col-xs-12 no-padding">
                                    <dx:ASPxGridView ID="gridFile" SettingsBehavior-AllowSort="false" ClientInstanceName="gridFile" runat="server"
                                        Settings-VerticalScrollBarMode="Visible"
                                        Settings-VerticalScrollableHeight="300"
                                        EnableCallBacks="true" KeyFieldName="id" OnCustomCallback="gridFile_CustomCallback"
                                        OnHtmlDataCellPrepared="gridFile_HtmlDataCellPrepared"
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
                                            <dx:GridViewDataTextColumn Caption="Manage" FieldName="id" CellStyle-HorizontalAlign="Center" Width="20">
                                                <DataItemTemplate>
                                                    <a class="btn btn-mini" onclick="editFile(<%# Eval("id")%>)" title="Edit">
                                                        <i class="fa fa-pencil" aria-hidden="true"></i>
                                                    </a>
                                                    <a id="btnDelete" class="btn btn-mini" onclick="deleteFile(<%# Eval("id")%>)" title="Delete">
                                                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="running_no" Caption="Running No" Settings-AllowSort="True" CellStyle-HorizontalAlign="Left" Width="50" />
                                            <dx:GridViewDataTextColumn FieldName="description" Caption="รายละเอียด" CellStyle-HorizontalAlign="Left" Width="100" />
                                            <dx:GridViewDataTextColumn FieldName="mfg_number" Caption="MFG Number" CellStyle-HorizontalAlign="Left" Width="50" />
                                            <dx:GridViewDataTextColumn FieldName="model_number" Caption="Model Number" CellStyle-HorizontalAlign="Left" Width="50" />
                                            <dx:GridViewDataTextColumn FieldName="type" Caption="ประเภท" Settings-AllowSort="True" CellStyle-HorizontalAlign="Left" Width="100">
                                                <DataItemTemplate>
                                                    <dx:ASPxLabel ID="lblType" runat="server" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="file_name" Caption="ชื่อไฟล์" Settings-AllowSort="True" CellStyle-HorizontalAlign="Left" Width="150">
                                                <DataItemTemplate>
                                                    <a class="btn btn-mini" href="<%# "../Doc_pdf/" + Eval("file_name")%>" target="_blank" title="<%# Eval("file_name")%>">
                                                        <%# Eval("file_name")%>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>

                                        <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                            CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                                    </dx:ASPxGridView>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <input hidden="hidden" id="hdMaster_id" runat="server" />
                    <input hidden="hidden" id="hdSchedule_id" runat="server" />
                    <%---------------------------------------------------%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" onclick="submitFile()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal Edit file name-->
    <div class="modal fade" id="modal_edit_file_name" role="dialog" data-backdrop="static" data-keyboard="true" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="modal-title">แก้ไขรายละเอียด</div>
                </div>
                <div class="modal-body text-center">
                    <div class="row">
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2 no-padding" for="txtAddress">Running No:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control" id="txtRunningNoEdit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2 no-padding" for="txtAddress">รายละเอียด:</label>
                            <div class="col-xs-10">
                                <input type="text" class="form-control" id="txtDescription" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2" for="cboSupplier">วันที่ทำงานจริง: </label>
                            <div class="col-xs-4  no-padding-right">
                                <input type="text" class="form-control selectDate" id="txtstarting_date_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2" for="cboSupplier">MFG Number: </label>
                            <div class="col-xs-4  no-padding-right">
                                <input type="text" class="form-control" id="txtmfg_upload_edit" runat="server" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12 no-padding">
                            <label class="control-label col-xs-2" for="cboSupplier">Model Number: </label>
                            <div class="col-xs-4  no-padding-right">
                                <input type="text" class="form-control" id="txtmodel_upload_edit" runat="server" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-addItem" id="btnsubmit_edit_file" onclick="submit_edit_file_name()">
                        <i class="fa fa-save" aria-hidden="true"></i>&nbsp;บันทึก
                    </button>
                    <button type="button" class="btn-addItem" data-dismiss="modal">
                        <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;ยกเลิก
                    </button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
