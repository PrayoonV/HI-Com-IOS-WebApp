<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="Spreadsheet.aspx.cs" Inherits="HicomIOS.Report.Spreadsheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="spreadsheetContainer">
        <dx:ASPxSpreadsheet ID="SpreadsheetControl" runat="server" ClientInstanceName="spreadsheet" Height="800" OnCallback="SpreadsheetControl_Callback" />
    </div>
</asp:Content>
