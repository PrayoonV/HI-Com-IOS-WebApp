<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="Excel_QuotationSparePart.aspx.cs" Inherits="HicomIOS.Report.Excel_QuotationSparePart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .picker{
            width: 100%;
        }

    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="../Content/Site.css">
    <div class="row">
        <div class="col-xs-12">
            <div class="form-group">
                <label class="col-xs-3 text-right label-rigth">Date :</label>
                <div class="col-xs-2">
                    <input class="picker" runat="server" type="text" id="datepickerFrom">
                </div>
                <label class="col-xs-1 text-center label-rigth">To</label>
                <div class="col-xs-2">
                    <input class="picker" runat="server" type="text" id="datepickerTo">
                </div>
            </div>
        </div>
        <div class="col-xs-12">
            <div class="row form-group">
                <div class="col-xs-9">
                    <%--<button type="button"
                        class="btn-app"
                        id="Button1" onclick="Export_Click()">
                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Export
                    </button>--%>
                    <asp:Button ID="Button1" runat="server" OnClick="Export_Click" Text="Button" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#datepickerTo").datepicker();
            $("#datepickerFrom").datepicker();
        });
        //function Export_Click() {
        //    var datepickerFrom = $('#datepickerFrom').val();
        //    var datepickerTo = $('#datepickerTo').val();
        //    //window.location.href = "Excel_QuotationSparePart.aspx?datepickerFrom=" + datepickerFrom + "&datepickerTo=" + datepickerTo;

        //    $.ajax({
        //        type: "POST",
        //        url: "Excel_QuotationSparePart.aspx/Export_Click",
        //        data: '{datepickerFrom:"' + $('#datepickerFrom').val() + '",datepickerTo"' + $('#datepickerTo').val() + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data) {
        //            console.log("success");
        //        }
        //    });
        //}
    </script>
</asp:Content>



