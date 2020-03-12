<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ExportSummaryQuotationProduct.aspx.cs" Inherits="HicomIOS.Master.ExportSummaryQuotationProduct" %>

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
            width: 100%;
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
                <legend>Summary Quotation Product</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">วันที่ :</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">ถึง</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="datepickerTo" />
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
                    <div class="col-xs-12 no-padding">
                        <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right ">วันที่P/O :</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="PoDateFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">ถึง</label>
                            <div class="col-xs-4 no-padding">
                                <input class="form-control picker" runat="server" type="text" id="PoDateTo" />
                            </div>
                        </div>
                    </div>
                        <div class="col-xs-4 no-padding">
                            <div class="row form-group">
                                <label class="col-xs-3 text-right label-rigth">Customer :</label>
                                <div class="col-xs-9 no-padding">
                                    <dx:ASPxComboBox ID="cbbCustomer" CssClass="form-control"
                                        runat="server" ClientInstanceName="cbbCustomer" TextField="data_text"
                                        EnableCallbackMode="true"
                                        ValueField="data_value">
                                    </dx:ASPxComboBox>
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
                    </div>
                </div>
                <%------------------------------------%>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Salename :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbEmployee" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbEmployee" TextField="data_text"
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
                            <label class="col-xs-3 text-right label-rigth">Product Category :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbProductCat" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbProductCat" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">AC Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbAirCompressorModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbAirCompressorModel" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">AC Presure :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbAirCompressorPresure" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbAirCompressorPresure" TextField="data_text"
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
                            <label class="col-xs-3 text-right label-rigth">AD Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbAirDryerModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbAirDryerModel" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">LF Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbLineFilterModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbLineFilterModel" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">MF Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbMistFilterModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbMistFilterModel" TextField="data_text"
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
                            <label class="col-xs-3 text-right label-rigth">AT Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbAirTankModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbAirTankModel" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Other Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbOtherModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbOtherModel" TextField="data_text"
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
                            <label class="col-xs-3 text-right label-rigth">ข้อมูลอื่น ๆ  :</label>
                            <div class="col-xs-9 no-padding">
                                <input class="form-control" id="Txtother" runat="server" />
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
                    OnCustomCallback="gridExportSummaryQuotationProduct_CustomCallback">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                        PageSizeItemSettings-Visible="true">
                        <PageSizeItemSettings Items="10, 20, 50" />
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" >
                            <DataItemTemplate>
                                <a id="btnEdit" class="btn-labelEdit  <%# ((bool)BasePage.SESSION_PERMISSION_SCREEN.is_edit) ? "" : "disabled" %>" onclick="editNotice(<%# Eval("id")%>)" title="Edit">
                                    <%# Eval("quotation_no")%>
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="quotation_date_from" Caption="Quotation Date" />
                        <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" />
                        <dx:GridViewDataTextColumn FieldName="grand_total" Caption="Price List Air Compressor" />
                        <dx:GridViewDataTextColumn FieldName="po_no" Caption="Po No" />
                        <dx:GridViewDataTextColumn FieldName="amount_po" Caption="PO Date" />
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
                dateFormat: 'dd/mm/yy'
            });
        });
        function editNotice(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            window.location.href = "Quotation.aspx?dataId=" + key;
            $.LoadingOverlay("hide");
        }
        function clear_Click() {
            cbbCustomerGroup.SetValue(" ");
            cbbProductCat.SetValue(" ");
            cbbCustomer.SetValue(" ");
            cbbEmployee.SetValue(" ");
            cbbStatus.SetValue(" ");
            $('#datepickerFrom').val(" ");
            $('#datepickerTo').val(" ");
            cbbQuotation.SetValue(" ");
            cbbAirCompressorModel.SetValue(" ");
            cbbAirCompressorPresure.SetValue(" ");
            cbbAirDryerModel.SetValue(" ");
            cbbLineFilterModel.SetValue(" ");
            cbbMistFilterModel.SetValue(" ");
            cbbAirTankModel.SetValue(" ");
            cbbOtherModel.SetValue(" ");
            $('#PoDateFrom').val(" ");
            $('#PoDateTo').val(" ");
            $('#Txtother').val(" ");
        }
        function view_Click() {
            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var cat_id = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var employee_id = cbbEmployee.GetValue() == "" || cbbEmployee.GetValue() == null ? 0 : cbbEmployee.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var quotation_date_from = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var quotation_no = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var model_list_air_compressor = cbbAirCompressorModel.GetValue() == "" || cbbAirCompressorModel.GetValue() == null ? "" : cbbAirCompressorModel.GetValue();
            var model_list_air_presure = cbbAirCompressorPresure.GetValue() == "" || cbbAirCompressorPresure.GetValue() == null ? "" : cbbAirCompressorPresure.GetValue();
            var model_list_air_dryer = cbbAirDryerModel.GetValue() == "" ? "" : cbbAirDryerModel.GetValue();
            var model_list_line_filter = cbbLineFilterModel.GetValue() == "" || cbbLineFilterModel.GetValue() == null ? "" : cbbLineFilterModel.GetValue();
            var model_list_mist_filter = cbbMistFilterModel.GetValue() == "" || cbbMistFilterModel.GetValue() == null ? "" : cbbMistFilterModel.GetValue();
            var model_list_air_tank = cbbAirTankModel.GetValue() == "" || cbbAirTankModel.GetValue() == null ? "" : cbbAirTankModel.GetValue();
            var model_list_other = cbbOtherModel.GetValue() == "" || cbbOtherModel.GetValue() == null ? "" : cbbOtherModel.GetValue();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            var other = $('#Txtother').val() == "" || $('#Txtother').val() == null ? "" : $('#Txtother').val();

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

            var parametersSummaryQuotationProduct = {
                dataSummaryQuotationProduct: [{
                    customer_id: customer_id,

                    employee_id: employee_id,
                    customer_group: customer_group,
                    cat_id: cat_id,
                    quotation_status: quotation_status,
                    quotation_no: quotation_no,
                    model_list_air_compressor: model_list_air_compressor,
                    model_list_air_presure: model_list_air_presure,
                    model_list_air_dryer: model_list_air_dryer,
                    model_list_line_filter: model_list_line_filter,
                    model_list_mist_filter: model_list_mist_filter,
                    model_list_air_tank: model_list_air_tank,
                    model_list_other: model_list_other,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    other: other,
                }]
            };
            $.ajax({
                type: 'POST',
                url: "ExportSummaryQuotationProduct.aspx/GetViewGrid",
                data: JSON.stringify(parametersSummaryQuotationProduct),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    gridView.PerformCallback();
                    $('#btnExport').show();

                }
            });
        }
        function Export_Click() {

            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var cat_id = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var employee_id = cbbEmployee.GetValue() == "" || cbbEmployee.GetValue() == null ? 0 : cbbEmployee.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var quotation_date_from = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var quotation_no = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var model_list_air_compressor = cbbAirCompressorModel.GetValue() == "" || cbbAirCompressorModel.GetValue() == null ? "" : cbbAirCompressorModel.GetValue();
            var model_list_air_presure = cbbAirCompressorPresure.GetValue() == "" || cbbAirCompressorPresure.GetValue() == null ? "" : cbbAirCompressorPresure.GetValue();
            var model_list_air_dryer = cbbAirDryerModel.GetValue() == "" ? "" : cbbAirDryerModel.GetValue();
            var model_list_line_filter = cbbLineFilterModel.GetValue() == "" || cbbLineFilterModel.GetValue() == null ? "" : cbbLineFilterModel.GetValue();
            var model_list_mist_filter = cbbMistFilterModel.GetValue() == "" || cbbMistFilterModel.GetValue() == null ? "" : cbbMistFilterModel.GetValue();
            var model_list_air_tank = cbbAirTankModel.GetValue() == "" || cbbAirTankModel.GetValue() == null ? "" : cbbAirTankModel.GetValue();
            var model_list_other = cbbOtherModel.GetValue() == "" || cbbOtherModel.GetValue() == null ? "" : cbbOtherModel.GetValue();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            var other = $('#Txtother').val() == "" || $('#Txtother').val() == null ? "" : $('#Txtother').val();
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
            var parametersSummaryQuotationProduct = {
                dataSummaryQuotationProduct: [{
                    customer_id: customer_id,

                    employee_id: employee_id,
                    customer_group: customer_group,
                    cat_id: cat_id,
                    quotation_status: quotation_status,
                    quotation_no: quotation_no,
                    model_list_air_compressor: model_list_air_compressor,
                    model_list_air_presure: model_list_air_presure,
                    model_list_air_dryer: model_list_air_dryer,
                    model_list_line_filter: model_list_line_filter,
                    model_list_mist_filter: model_list_mist_filter,
                    model_list_air_tank: model_list_air_tank,
                    model_list_other: model_list_other,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    other: other,
                }]
            };
            $.LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: "ExportSummaryQuotationProduct.aspx/Export_Quotation_Summary_Product",
                data: JSON.stringify(parametersSummaryQuotationProduct),
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
