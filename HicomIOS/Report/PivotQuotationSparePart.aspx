﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="PivotQuotationSparePart.aspx.cs" Inherits="HicomIOS.Report.Pivot_QuotationSummaryReport_Spare_Part" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="reportContainer">
        <dx:ASPxPivotGrid ID="pivotGridControl" runat="server" Width="100%" Height="800px" OptionsPager-Visible="False" ClientIDMode="AutoID">
            <OptionsCustomization CustomizationFormStyle="Excel2007" />
            <OptionsPager Visible="False"></OptionsPager>
            <OptionsData DataProcessingEngine="LegacyOptimized" />
        </dx:ASPxPivotGrid>
        <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="pivotGridControl">
        </dx:ASPxPivotGridExporter>
    </div>
</asp:Content>
