﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ExportSummaryQuotationContract.aspx.cs" Inherits="HicomIOS.Master.ExportSummaryQuotationContract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <style>
        .btnexport {
            margin-bottom: 7px;
            margin-left: 15px;
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
            width:  100%;
            margin-left: 15%;
        }

        .has-error {
            border-color: #e40703 !important;
        }
         .swal-modal {
            width: 420px !important;
            height: 140px !important;
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
    <div id="div-content">
        <div class="row">
            <fieldset>
                <legend>Summary Quotation Contract</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">Quotation Date:</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">To</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerTo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Customer Code :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbCustomerCode" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbCustomerCode" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Customer Name :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbCustomer" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbCustomer" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    
                </div>

                <div class="col-xs-12 no-padding">
                     <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">P/O Date:</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="PoDateFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">To</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="PoDateTo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Customer Group :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbCustomerGroup" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbCustomerGroup" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Contract No :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbContract" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbContract" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                   
                </div>
                <%------------------------------------%>
                <div class="col-xs-12 no-padding">
                     <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Type Of Contract :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbContractType" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbContractType" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Quotation No :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbQuotation" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbQuotation" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Status :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbStatus" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbStatus" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>

                    
                </div>
                <%------------------------------------%>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">Starting Date:</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="dateFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">To</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="dateTo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">Expire Date:</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="dateexpireFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">To</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="dateexpireTo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <div class="col-xs-2 no-padding">
                                 <button type="button"
                                    class="btn-info btn-view" style="height: 30px;"
                                    id="btnReportClient" onclick="view_Click()">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    View
                                </button>
                            </div>
                            <div class="col-xs-2 no-padding">
                                <button type="button"
                                    class="btn-info btn-view" style="height: 30px;margin-left:30%"
                                    id="btnClear" onclick="clear_Click()">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    Clear
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <%------------------------------------%>
                
            </fieldset>
        </div>

        <div class="row">
            <button type="button"
                class="btn-addItem btnexport"
                id="btnExport" onclick="Export_Click()">
                <i class="fa fa-check" aria-hidden="true"></i>Export
            </button>
            <div class="col-xs-12" runat="server" id="dvGridProduct">
                <dx:ASPxGridView ID="gridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridView"
                    Width="100%" KeyFieldName="id" EnableCallBacks="true"
                    EnableRowsCache="false"
                    SettingsBehavior-AllowSort="false"
                    OnCustomCallback="gridExportSummaryQuotationContract_CustomCallback">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                        PageSizeItemSettings-Visible="true">
                        <PageSizeItemSettings Items="10, 20, 50" />
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="contract_no" Caption="Contract No" />
                        <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" />
                        <dx:GridViewDataTextColumn FieldName="quotation_date" Caption="Quotation Date" />
                        <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" />
                        <dx:GridViewDataTextColumn FieldName="project_name" Caption="Project Name" />
                        <dx:GridViewDataTextColumn FieldName="attention" Caption="Attention" />
                        <dx:GridViewDataTextColumn FieldName="contract_type" Caption="Contract Type" />
                        <dx:GridViewDataTextColumn FieldName="starting_date" Caption="Starting Date" />
                        <dx:GridViewDataTextColumn FieldName="expire_date" Caption="Expire Date" />
                        <dx:GridViewDataTextColumn FieldName="schedule_date_min_mmyy" Caption="Schedule (min)" />
                        <dx:GridViewDataTextColumn FieldName="schedule_date_max_mmyy" Caption="Schedule (max)" />
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
            $('#btnExport').hide();
        });
        $(function () {
            $(".picker").datepicker({
                dateFormat: 'dd/mm/yy', "changeMonth": true,
                "changeYear": true
            });
        });
        function view_Click() {
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var customer_code = cbbCustomerCode.GetValue() == "" || cbbCustomerCode.GetValue() == null ? "" : cbbCustomerCode.GetValue();
            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var contract_no = cbbContract.GetValue() == "" || cbbContract.GetValue() == null ? "" : cbbContract.GetValue();
            var contract_type = cbbContractType.GetValue() == "" || cbbContractType.GetValue() == null ? 0 : cbbContractType.GetValue();
            var quotation_no = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var quotation_date_from = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var expire_date_from = $('#dateexpireFrom').val() == "" || $('#dateexpireFrom').val() == null ? "" : $('#dateexpireFrom').val();
            var expire_date_to = $('#dateexpireTo').val() == "" || $('#dateexpireTo').val() == null ? "" : $('#dateexpireTo').val();
            var Starting_date_from = $('#dateFrom').val() == "" || $('#dateFrom').val() == null ? "" : $('#dateFrom').val();
            var Starting_date_to = $('#dateTo').val() == "" || $('#dateTo').val() == null ? "" : $('#dateTo').val();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            if (quotation_date_from == "" || quotation_date_to == "") {
                if (po_date_from == "" || po_date_to == "") {
                    $('#datepickerFrom').addClass('has-error');
                    $('#datepickerTo').addClass('has-error');
                    $('#po_date_from').addClass('has-error');
                    $('#po_date_to').addClass('has-error');
                    console.log("กรุณาเลือกช่วงวันที่เอกสาร");
                    swal("กรุณาเลือกช่วงวันที่เอกสาร");
                    return;
                }
            } else {
                $('#datepickerFrom').removeClass('has-error');
                $('#datepickerTo').removeClass('has-error');
                $('#po_date_from').parent().parent().removeClass('has-error');
                $('#po_date_to').parent().parent().removeClass('has-error');
            }
            if (quotation_date_from > quotation_date_to || po_date_from > po_date_to) {
                swal("กรุณาเลือกช่วงวันที่เอกสารให้ถูกต้อง");
                return;
            }
            var parametersSummaryQuotationContract = {
                dataSummaryQuotationContract: [{

                    customer_id: customer_id,
                    customer_code: customer_code,
                    customer_group: customer_group,
                    contract_no: contract_no,
                    contract_type: contract_type,
                    quotation_no: quotation_no,
                    quotation_status: quotation_status,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    expire_date_from: expire_date_from,
                    expire_date_to: expire_date_to,
                    Starting_date_from: Starting_date_from,
                    Starting_date_to: Starting_date_to,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,

                }]
            };
            $.ajax({
                type: 'POST',
                url: "ExportSummaryQuotationContract.aspx/GetViewGrid",
                data: JSON.stringify(parametersSummaryQuotationContract),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    gridView.PerformCallback();
                    $('#btnExport').show();
                }
            });
        }
        function Export_Click() {
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var customer_code = cbbCustomerCode.GetValue() == "" || cbbCustomerCode.GetValue() == null ? "" : cbbCustomerCode.GetValue();
            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var contract_no = cbbContract.GetValue() == "" || cbbContract.GetValue() == null ? "" : cbbContract.GetValue();
            var contract_type = cbbContractType.GetValue() == "" || cbbContractType.GetValue() == null ? 0 : cbbContractType.GetValue();
            var quotation_no = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var quotation_date_from = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var expire_date_from = $('#dateexpireFrom').val() == "" || $('#dateexpireFrom').val() == null ? "" : $('#dateexpireFrom').val();
            var expire_date_to = $('#dateexpireTo').val() == "" || $('#dateexpireTo').val() == null ? "" : $('#dateexpireTo').val();
            var Starting_date_from = $('#dateFrom').val() == "" || $('#dateFrom').val() == null ? "" : $('#dateFrom').val();
            var Starting_date_to = $('#dateTo').val() == "" || $('#dateTo').val() == null ? "" : $('#dateTo').val();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            if (quotation_date_from == "" || quotation_date_to == "") {
                if (po_date_from == "" || po_date_to == "") {
                    $('#datepickerFrom').addClass('has-error');
                    $('#datepickerTo').addClass('has-error');
                    $('#po_date_from').addClass('has-error');
                    $('#po_date_to').addClass('has-error');
                    console.log("กรุณาเลือกช่วงวันที่เอกสาร");
                    swal("กรุณาเลือกช่วงวันที่เอกสาร");
                    return;
                }
            } else {
                $('#datepickerFrom').removeClass('has-error');
                $('#datepickerTo').removeClass('has-error');
                $('#po_date_from').parent().parent().removeClass('has-error');
                $('#po_date_to').parent().parent().removeClass('has-error');
            }
            if (quotation_date_from > quotation_date_to || po_date_from > po_date_to) {
                swal("กรุณาเลือกช่วงวันที่เอกสารให้ถูกต้อง");
                return;
            }
            var parametersSummaryQuotationContract = {
                dataSummaryQuotationContract: [{
                    customer_id: customer_id,
                    customer_code: customer_code,
                    customer_group: customer_group,
                    contract_no: contract_no,
                    contract_type: contract_type,
                    quotation_no: quotation_no,
                    quotation_status: quotation_status,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    expire_date_from: expire_date_from,
                    expire_date_to: expire_date_to,
                    Starting_date_from: Starting_date_from,
                    Starting_date_to: Starting_date_to,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,
                }]
            };
            $.LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: "ExportSummaryQuotationContract.aspx/Export_Quotation_Summary_Product",
                data: JSON.stringify(parametersSummaryQuotationContract),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.LoadingOverlay("hide");
                    //console.log("success= " + data.d);
                    //window.location = data.d;
                    window.open("../Common/DownloadFile.aspx?" + data.d, '_blank');
                }
            });
        }
    </script>
</asp:Content>
