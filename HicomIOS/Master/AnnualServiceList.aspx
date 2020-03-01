<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AnnualServiceList.aspx.cs" Inherits="HicomIOS.Master.AnnualServiceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="../Lib/css/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.10/css/all.css" integrity="sha384-+d0P83n9kaQMCwj8F4RJB66tzIwOKmrdb46+porD/OvrJ+37WqIM7UoBtwHO6Nlg" crossorigin="anonymous">
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

        #Splitter_0 {
            display: none;
        }

        .btn-view {
            width: 15%;
            margin-left: 2%;
        }

        .has-error {
            border-color: #e40703 !important;
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

        .btn-linkview {
            cursor: pointer;
            font-size: 15px;
            color: #5bc0de !important;
        }
    </style>
     <script type="text/javascript">

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
    </script>
    <fieldset>
        <legend>Annual Service List</legend>
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding">
                <div class="row form-group">
                    <label class="col-xs-4 text-right ">Test Run Date From :</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="datepickerFrom" />
                    </div>
                    <label class="col-xs-2 text-center">Test Run Date To</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="datepickerTo" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding">
                <div class="row form-group">
                    <label class="col-xs-4 text-right ">Starting Date From :</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="starttingdateFrom" />
                    </div>
                    <label class="col-xs-2 text-center">Expire Date To</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="starttingdateTo" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding hidden">
            <div class="col-xs-12 no-padding">
                <div class="row form-group">
                    <label class="col-xs-4 text-right ">Expire Date From :</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="expiredateFrom" />
                    </div>
                    <label class="col-xs-2 text-center">Expire Date To </label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="expiredateTo" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding">
                <div class="row form-group">
                    <label class="col-xs-4 text-right ">Schedule Date From :</label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="scheduleFrom" />
                    </div>
                    <label class="col-xs-2 text-center">Schedule Date To </label>
                    <div class="col-xs-2 no-padding">
                        <input class="form-control picker" runat="server" type="text" id="scheduleTo" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding">
                <div class="row form-group">
                    <label class="col-xs-4 text-right label-rigth">Customer :</label>
                    <div class="col-xs-2 no-padding">
                        <dx:ASPxComboBox ID="cbbCustomer" CssClass="form-control"
                            runat="server" ClientInstanceName="cbbCustomer" TextField="data_text"
                            ClientSideEvents-ValueChanged="changeSelectCustomerMFG"
                            ValueField="data_value">

                        </dx:ASPxComboBox>
                    </div>
                    <label class="col-xs-2 text-center">Project : </label>
                    <div class="col-xs-2 no-padding">
                          <dx:ASPxComboBox ID="cbbProject" CssClass="form-control" runat="server"
                                    ClientInstanceName="cbbProject" TextField="data_text"
                                    OnCallback="cbbProject_Callback"
                                    ClientSideEvents-ValueChanged="changeSelectProject" ValueField="data_value">
                                </dx:ASPxComboBox>


                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding text-center">
            <div class="row form-group">
                <div class="col-xs-4 no-padding">
                    <button type="button"
                        class="btn-info btn-view" style="height: 30px; float: right;"
                        id="btnReportClient" onclick="view_Click()">
                        <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    View
                    </button>
                </div>
                <div class="col-xs-4 no-padding">
                    <button type="button"
                        class="btn-info btn-view" style="height: 30px; float: left;"
                        id="btnClear" onclick="clear_Click()">
                        <i class="fa fa-archive" aria-hidden="true"></i>&nbsp;
                                    Clear
                    </button>
                    <%--<asp:Button ID="test" Text="excle" runat="server" OnClick="test_Click" />--%>
                </div>
                <div class="col-xs-4 no-padding">
                    <button type="button"
                        class="btn-addItem btnexport"
                        id="btnExport" onclick="Export_Click()">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Export
                    </button>
                </div>
            </div>
        </div>
    </fieldset>

    <div id="dvAnnuualServiceList">
        <dx:ASPxGridView ID="gridAnnualServiceList" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridAnnualServiceList"
            Width="100%" KeyFieldName="id" EnableCallBacks="true"
            EnableRowsCache="false"
            SettingsBehavior-AllowSort="false"
            OnCustomCallback="gridAnnualServiceList_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
            <Paddings Padding="0px" />
            <Border BorderWidth="0px" />
            <BorderBottom BorderWidth="1px" />
            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Items="10, 20, 50" />
            </SettingsPager>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="id" CellStyle-HorizontalAlign="Center" Caption="#" Width="50px">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-linkview " onclick="viewItem(<%# Eval("id")%>)" title="View">
                            <i class="fas fa-search"></i>
                        </a>

                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer" Caption="Customer">
                    <DataItemTemplate>
                        <a id="btnEdit" class="btn-labelEdit " onclick="editItem(<%# Eval("id")%>)" title="Edit">
                            <%# Eval("customer")%>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="project" Caption="Project" />
                <dx:GridViewDataTextColumn FieldName="model" Caption="Model" />
                <dx:GridViewDataTextColumn FieldName="mfg" Caption="MFG" />


                <dx:GridViewDataTextColumn FieldName="start_date" Caption="Test Run Date" />
                <dx:GridViewDataTextColumn FieldName="starting_date" Caption="Starting Date" />
                <dx:GridViewDataTextColumn FieldName="expire_date" Caption="Expire Date" />
                <dx:GridViewDataTextColumn FieldName="schedule_date" Caption="Schedule Date" />

                <dx:GridViewDataTextColumn FieldName="checking_location" Caption="Location" />
                <dx:GridViewDataTextColumn FieldName="checking_remark" Caption="Remark" />
            </Columns>
            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="<%$ appSettings:GridViewHeight %>" />
            <SettingsPopup>
                <CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="Below" Width="220px" Height="300px" />
            </SettingsPopup>
            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
        </dx:ASPxGridView>
    </div>

    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#dvAnnuualServiceList').css({
            'height': '50px;'
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Splitter_0").parent().hide();

            $('#btnExport').hide();
            var h = window.innerHeight;
            gridAnnualServiceList.SetHeight(h - 310);

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

            var height = dimensions.height - 310;

            console.log(height);
            gridAnnualServiceList.SetHeight(height);

            height = $('#Splitter_1').height();
            $('#Splitter_1').height(height + 40);
            $('#Splitter_1_CC').height(height + 40);
        });
        $(function () {
            $(".picker").datepicker({
                dateFormat: 'dd/mm/yy'
            });
        });
        function editItem(e) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            var key = e;
            window.location.href = "AnnualService.aspx?dataId=" + key;
            $.LoadingOverlay("hide");
        }
        function viewItem(e) {
            editItem(e)
        }
        function clear_Click() {
            $('#datepickerFrom').val("");
            $('#datepickerTo').val("");
            $('#starttingdateFrom').val("");
            $('#starttingdateTo').val("");
            $('#expiredateFrom').val("");
            $('#expiredateTo').val("");
            $('#scheduleFrom').val("");
            $('#scheduleTo').val("");
            cbbCustomer.SetValue("");
            $('#txtproject').val("");
            
        }

        function view_Click() {
            var datepickerFrom = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var datepickerTo = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var starttingdateFrom = $('#starttingdateFrom').val() == "" || $('#starttingdateFrom').val() == null ? "" : $('#starttingdateFrom').val();
            var starttingdateTo = $('#starttingdateTo').val() == "" || $('#starttingdateTo').val() == null ? "" : $('#starttingdateTo').val();
            var expiredateFrom = $('#expiredateFrom').val() == "" || $('#expiredateFrom').val() == null ? "" : $('#expiredateFrom').val();
            var expiredateTo = $('#expiredateTo').val() == "" || $('#expiredateTo').val() == null ? "" : $('#expiredateTo').val();

            var scheduleFrom = $('#scheduleFrom').val() == "" || $('#scheduleFrom').val() == null ? "" : $('#scheduleFrom').val();
            var scheduleTo = $('#scheduleTo').val() == "" || $('#scheduleTo').val() == null ? "" : $('#scheduleTo').val();
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var project_search = cbbProject.GetValue() == "" || cbbProject.GetValue() == null ? "" : cbbProject.GetValue();

            var parametersAdd = {
                masterData: [
                    {
                        datepickerFrom: datepickerFrom,
                        datepickerTo: datepickerTo,
                        starttingdateFrom: starttingdateFrom,
                        starttingdateTo: starttingdateTo,
                        expiredateFrom: expiredateFrom,
                        expiredateTo: expiredateTo,
                        scheduleFrom: scheduleFrom,
                        scheduleTo: scheduleTo,
                        customerid: customer_id,
                        project_search : project_search
                    }
                ]
            };
            $.LoadingOverlay("show");
            $.ajax({
                type: 'POST',
                url: "AnnualServiceList.aspx/GetViewGrid",
                data: JSON.stringify(parametersAdd),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    $.LoadingOverlay("hide");

                    gridAnnualServiceList.PerformCallback();
                    $('#btnExport').show();
                }
            });
        }

        function Export_Click() {
            var datepickerFrom = $('#datepickerFrom').val() == "" || $('#datepickerFrom').val() == null ? "" : $('#datepickerFrom').val();
            var datepickerTo = $('#datepickerTo').val() == "" || $('#datepickerTo').val() == null ? "" : $('#datepickerTo').val();
            var starttingdateFrom = $('#starttingdateFrom').val() == "" || $('#starttingdateFrom').val() == null ? "" : $('#starttingdateFrom').val();
            var starttingdateTo = $('#starttingdateTo').val() == "" || $('#starttingdateTo').val() == null ? "" : $('#starttingdateTo').val();
            var expiredateFrom = $('#expiredateFrom').val() == "" || $('#expiredateFrom').val() == null ? "" : $('#expiredateFrom').val();
            var expiredateTo = $('#expiredateTo').val() == "" || $('#expiredateTo').val() == null ? "" : $('#expiredateTo').val();

            var scheduleFrom = $('#scheduleFrom').val() == "" || $('#scheduleFrom').val() == null ? "" : $('#scheduleFrom').val();
            var scheduleTo = $('#scheduleTo').val() == "" || $('#scheduleTo').val() == null ? "" : $('#scheduleTo').val();
            var customer_id = cbbCustomer.GetValue() == "" || cbbCustomer.GetValue() == null ? 0 : cbbCustomer.GetValue();
            var project_search = $('#txtproject').val() == "" || $('#txtproject').val() == null ? "" : $('#txtproject').val();

            var parametersExport = {
                annualServiceListData: [
                    {
                        datepickerFrom: datepickerFrom,
                        datepickerTo: datepickerTo,
                        starttingdateFrom: starttingdateFrom,
                        starttingdateTo: starttingdateTo,
                        expiredateFrom: expiredateFrom,
                        expiredateTo: expiredateTo,
                        scheduleFrom: scheduleFrom,
                        scheduleTo: scheduleTo,
                         customerid: customer_id,
                        project_search : project_search
                    }
                ]
            };
            $.LoadingOverlay("show");
            $.ajax({
                type: "POST",
                url: "AnnualServiceList.aspx/ExportAnnualServiceList",
                data: JSON.stringify(parametersExport),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.LoadingOverlay("hide");
                    window.open("../Common/DownloadFile.aspx?" + data.d, '_blank');
                }
            });
        }
    </script>
</asp:Content>
