<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ExportSummaryQuotationSparePart.aspx.cs" Inherits="HicomIOS.Master.ExportSummaryQuotationSparePart" %>

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
            margin-left: 5%;
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
                <legend>Summary Quotation SparePart</legend>
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
                            <label class="col-xs-3 text-right label-rigth">Shelf :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbShelf" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbShelf" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                     <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-3 text-right label-rigth">Model :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbModel" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbModel" TextField="data_text"
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
                            <label class="col-xs-3 text-right label-rigth">MFG :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbMfg" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbMfg" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
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
                            <label class="col-xs-3 text-right label-rigth">Quotation Type :</label>
                            <div class="col-xs-9 no-padding">
                                <dx:ASPxComboBox ID="cbbproducttype" CssClass="form-control"
                                    runat="server" ClientInstanceName="cbbproducttype" TextField="data_text"
                                    EnableCallbackMode="true"
                                    ValueField="data_value">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-5 no-padding">
                        </div>
                    <div class="col-xs-4 no-padding">
                        <div class="row form-group">
                            <div class="col-xs-2 no-padding">
                                <button type="button"
                                    class="btn-info btn-view" style="height: 30px;margin-left: 30%"
                                    id="btnReportClient" onclick="view_Click()">
                                    <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    View
                                </button>
                            </div>
                            <div class="col-xs-2 no-padding">
                                <button type="button"
                                    class="btn-info btn-view" style="height: 30px; margin-left: 50%"
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
        <%------------------------------------%>
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
                    OnCustomCallback="gridExportSummaryQuotationSparePart_CustomCallback">
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
                        <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" />
                        <dx:GridViewDataTextColumn FieldName="project_name" Caption="Project" />
                        <dx:GridViewDataTextColumn FieldName="attention_name" Caption="Attention" />
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
        $(function () {
            $(".picker").datepicker({
                dateFormat: 'dd/mm/yy'
            });
        });
        function editNotice(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            //var input = s.GetMainElement();
            var key = e;//$(input).attr("noticeId");

            window.location.href = "Quotation.aspx?dataId=" + key;
            $.LoadingOverlay("hide");
        }
        function clear_Click() {
            cbbCustomer.SetValue(" ");
            cbbProductCat.SetValue(" ");
            cbbStatus.SetValue(" ");
            cbbShelf.SetValue(" ");
            cbbCustomerGroup.SetValue(" ");
            cbbQuotation.SetValue(" ");
            cbbModel.SetValue(" ");
            cbbMfg.SetValue(" ");
            cbbproducttype.SetValue(" ");
            $('#datepickerFrom').val(" ");
            $('#datepickerTo').val(" ");
            $('#PoDateFrom').val(" ");
            $('#PoDateTo').val(" ");
            $('#Txtother').val(" ");
        
        }

        function view_Click() {
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var cat_id = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
            var shelf_id = cbbShelf.GetValue() == "" || cbbShelf.GetValue() == null ? 0 : cbbShelf.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var quotation = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var model = cbbModel.GetValue() == "" || cbbModel.GetValue() == null ? "" : cbbModel.GetValue();
            var mfg = cbbMfg.GetValue() == "" || cbbMfg.GetValue() == null ? "" : cbbMfg.GetValue();
            var quotation_date_from = $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            var other = $('#Txtother').val() == "" || $('#Txtother').val() == null ? "" : $('#Txtother').val();
            var producttyle = cbbproducttype.GetValue() == "" || cbbproducttype.GetValue() == null ? "" : cbbproducttype.GetValue();

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
            var parametersSummaryQuotationSparePart = {
                dataSummaryQuotationSparePart: [{
                    customer_id: customer_id,
                    cat_id: cat_id,
                    shelf_id: shelf_id,
                    quotation_status: quotation_status,
                    customer_group: customer_group,
                    quotation: quotation,
                    model: model,
                    mfg: mfg,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,
                    other: other,
                    producttype : producttyle
                }]
            };
            $.ajax({
                type: 'POST',
                url: "ExportSummaryQuotationSparePart.aspx/GetViewGrid",
                data: JSON.stringify(parametersSummaryQuotationSparePart),
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
            var cat_id = cbbProductCat.GetValue() == "" || cbbProductCat.GetValue() == null ? 0 : cbbProductCat.GetValue();
            var shelf_id = cbbShelf.GetValue() == "" || cbbShelf.GetValue() == null ? 0 : cbbShelf.GetValue();
            var quotation_status = cbbStatus.GetValue() == "" || cbbStatus.GetValue() == null ? "" : cbbStatus.GetValue();
            var customer_group = cbbCustomerGroup.GetValue() == "" || cbbCustomerGroup.GetValue() == null ? 0 : cbbCustomerGroup.GetValue();
            var quotation = cbbQuotation.GetValue() == "" || cbbQuotation.GetValue() == null ? "" : cbbQuotation.GetValue();
            var model = cbbModel.GetValue() == "" || cbbModel.GetValue() == null ? "" : cbbModel.GetValue();
            var mfg = cbbMfg.GetValue() == "" || cbbMfg.GetValue() == null ? "" : cbbMfg.GetValue();
            var quotation_date_from = $('#datepickerFrom').val();
            var quotation_date_to = $('#datepickerTo').val();
            var po_date_from = $('#PoDateFrom').val() == "" || $('#PoDateFrom').val() == null ? "" : $('#PoDateFrom').val();
            var po_date_to = $('#PoDateTo').val() == "" || $('#PoDateTo').val() == null ? "" : $('#PoDateTo').val();
            var other = $('#Txtother').val() == "" || $('#Txtother').val() == null ? "" : $('#Txtother').val();
            var producttyle = cbbproducttype.GetValue() == "" || cbbproducttype.GetValue() == null ? "" : cbbproducttype.GetValue();

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
            var parametersSummaryQuotationSparePart = {
                dataSummaryQuotationSparePart: [{
                    customer_id: customer_id,
                    cat_id: cat_id,
                    shelf_id: shelf_id,
                    quotation_status: quotation_status,
                    customer_group: customer_group,
                    quotation: quotation,
                    model: model,
                    mfg: mfg,
                    quotation_date_from: quotation_date_from,
                    quotation_date_to: quotation_date_to,
                    po_date_from: po_date_from,
                    po_date_to: po_date_to,
                    other: other,
                     producttype : producttyle
                }]
            };
            $.LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: "ExportSummaryQuotationSparePart.aspx/Export_Quotation_Summary_SparePart",
                data: JSON.stringify(parametersSummaryQuotationSparePart),
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
