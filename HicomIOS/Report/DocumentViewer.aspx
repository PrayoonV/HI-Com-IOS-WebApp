﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="DocumentViewer.aspx.cs" Inherits="HicomIOS.Report.DocumentViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        /*@font-face 
        {
            font-family: "THSarabun";
            src: url(Fonts/THSarabun.ttf);
        }*/

        body{
              font-family: Tahoma;
        }
    </style>
    <div class="reportContainer">
        <dx:ASPxDocumentViewer ID="DocumentViewerControl" runat="server" ClientInstanceName="documentViewer" 
            EnableTheming="True" Theme="Office2003Silver" EnableViewState="False" >
            <SettingsReportViewer PrintUsingAdobePlugIn="False" />
            <stylesparameterspanelbuttons controlbuttonwidth="0px">
            </stylesparameterspanelbuttons>
            <ToolbarItems>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <%--<dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />--%>
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="FirstPage" />
                <dx:ReportToolbarButton ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="80px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="80px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </ToolbarItems>
            <SettingsSplitter SidePaneVisible="false" />
            <SettingsReportViewer EnableReportMargins="True" EnableRequestParameters="false"  UseIFrame="false"/>
        </dx:ASPxDocumentViewer>
    </div>
</asp:Content>