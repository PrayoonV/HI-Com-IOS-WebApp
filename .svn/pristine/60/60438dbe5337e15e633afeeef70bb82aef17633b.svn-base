<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="ExportExcel.aspx.cs" Inherits="HicomIOS.Report.ExportExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Content/Custom.js"></script>
    <script type="text/javascript" src="../Content/Script.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <style>
        .btnexport {
            width: 25%;
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
    </style>
    <div id="div-content">
        <div class="row">
            <fieldset>
                <legend>Stock Card</legend>
                <div class="col-xs-12 no-padding">
                    <div class="col-xs-6 no-padding">
                        <div class="row form-group">
                            <label class="col-xs-1 text-right no-padding">วันที่ :</label>
                            <div class="col-xs-5">
                                <input class="form-control picker" runat="server" type="text" id="datepickerFrom" />
                            </div>
                            <label class="col-xs-1 no-padding text-center">ถึง</label>
                            <div class="col-xs-5">
                                <input class="form-control picker" runat="server" type="text" id="datepickerTo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6 no-padding">
                        <div class="row form-group">
                            <div class="col-xs-9 no-padding">
                                <button type="button"
                                    class="btn-app btnexport"
                                    id="Button1" onclick="Export_Click1()">
                                    <i class="fa fa-check" aria-hidden="true"></i>Export
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        
    </div>
    <script type="text/javascript">
        $(function () {
            $(".picker").datepicker({
                dateFormat: 'dd/mm/yy'
            });

        });
        function Export_Click1() {
            var dateFrom = $("#datepickerFrom").val();
            var dateTo = $("#datepickerTo").val()
            $.LoadingOverlay("show");

            $.ajax({
                type: "POST",
                url: "ExportExcel.aspx/Export_Quotation_Summary_Product",
                data: '{datepickerFrom:"' + $('#datepickerFrom').val() + '",datepickerTo:"' + $('#datepickerTo').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.LoadingOverlay("hide");
                    console.log("success= " + data.d);
                    window.location = data.d;

                }
            });

        }
    </script>
</asp:Content>

