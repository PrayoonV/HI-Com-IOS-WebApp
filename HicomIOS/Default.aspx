﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Default.aspx.cs" Inherits="HicomIOS._Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Dashboard/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Dashboard/css/sb-admin.css" rel="stylesheet" />
    <style>
        .card-body .card-body-icon .fa-fw {
            width: 130px;
        }

        #SearchBox {
            display: none;
        }

        #Splitter_1_CC {
            overflow-y: scroll;
        }
        .userlogin .dropdown-toggle::after {
            content: none;
        }
    </style>
    <%--<script src="../Content/sweetalert/sweetalert.js"></script>--%>
    <script src="../Content/sweetalert/sweetalert.js"></script>
    <script type="text/javascript">
        $('#gridCellApprove').css({
            'height': '550px;'
        });
    </script>
    <div class="content-wrapper">
        <div class="container-fluid">
            <!-- Icon Cards-->
            <div class="card">
                <div class="card-header">
                    <div style="float: left;">
                        <i class="fa fa-file-text" aria-hidden="true"></i>&nbsp;Document Sum
                    </div>
                    <div style="float: left; margin-left: 15px;">
                        <select class="form-control">
                            <option value="Wk">Weekly</option>
                            <option value="ML">Monthly</option>
                            <option value="AN">Annual</option>
                        </select>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-xl-2">
                            <div class="card text-white bg-primary o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="qu_sum" runat="server"></label>
                                        Quotation
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="Master/QuotationList.aspx">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                        <div class="col-xl-2">
                            <div class="card text-white bg-warning o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="so_sum" runat="server"></label>
                                        Sale Order
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="Master/QuotationList.aspx">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                        <div class="col-xl-2">
                            <div class="card text-white bg-success o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="is_sum" runat="server"></label>
                                        Issue
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="#">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                        <div class="col-xl-2">
                            <div class="card text-white bg-info o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="pr_sum" runat="server"></label>
                                        Purchase Request
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="#">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                        <div class="col-xl-2">
                            <div class="card text-white bg-danger o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="br_sum" runat="server"></label>
                                        Borrow
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="#">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                        <div class="col-xl-2">
                            <div class="card text-white bg-dark o-hidden h-100">
                                <div class="card-body">
                                    <div class="card-body-icon">
                                        <i class="fa fa-file-text" aria-hidden="true"></i>
                                    </div>
                                    <div class="mr-5">
                                        <label id="rt_sum" runat="server"></label>
                                        Return
                                    </div>
                                </div>
                                <a class="card-footer text-white clearfix small z-1" href="#">
                                    <span class="float-left">View Details</span>
                                    <span class="float-right">
                                        <i class="fa fa-angle-right"></i>
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Approve Item-->
            <div class="card mb-3" id="ApproveContainer" runat="server" style="margin-top: 10px;">
                <div class="card-header">
                    <div style="float: left;">
                        <i class="fa fa-area-chart"></i>&nbsp;Approve
                    </div>
                    <div style="float: left; margin-left: 15px;">
                        <select class="form-control" id="typeListApprove">
                            <option value="WK" selected="selected">Weekly</option>
                            <option value="ML">Monthly</option>
                            <option value="AN">Annual</option>
                        </select>
                    </div>
                </div>
                <div class="card-body">    
                      <ul class="nav nav-tabs"> 
                        <li><a data-toggle="tab" id="ApproveTabQU" runat="server" href="#ApproveContentTabQU">Quotation</a></li>
                       <li><a data-toggle="tab" id="ApproveTabSO" runat="server" href="#ApproveContentTabSO">Sale Order</a></li> 
                        <li><a data-toggle="tab" id="ApproveTabIS" runat="server" href="#ApproveContentTabIS">Issue</a></li> 
                        <li><a data-toggle="tab" id="ApproveTabDN" runat="server" href="#ApproveContentTabDN">Delivery Note</a></li> 
                        <li><a data-toggle="tab" id="ApproveTabPR" runat="server" href="#ApproveContentTabRP">Purchase Request</a></li> 
                        <li><a data-toggle="tab" id="ApproveTabRE" runat="server" href="#ApproveContentTabRE">Return</a></li>
                        <li><a data-toggle="tab" id="ApproveTabBR" runat="server" href="#ApproveContentTabBR">Borrow</a></li> 
                      </ul>
                </div>
       

                  <div class="tab-content" id="gridCellApprove">
                    <div id="ApproveContentTabQU" runat="server" class="tab-pane fade">
                      <dx:ASPxGridView ID="gridViewQU" runat="server" AutoGenerateColumns="False"
                            Width="100%" KeyFieldName="id" EnableCallBacks="true"
                           OnCustomCallback="gridViewQU_CustomCallback"
                           >
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns> 
                                <dx:GridViewDataTextColumn Caption="Status Approve" FieldName="is_approve" CellStyle-HorizontalAlign="Center" Width="55px">
                                    <DataItemTemplate>
                                          <%--<button type="button" id="item_<%# Eval("id")%>" class="btn btn-left btn-toggle"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>,'QU')">
                                            <div class="handle"></div>
                                        </button>--%>
                                         nnnn-toggle active"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>, 'QU')">
                                            <div class="handle"></div>
                                        </button>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn> 
                                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="quotation_subject" Caption="Employee Code" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="project_name" Caption="Employee Code" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="company_name_tha" Caption="Comapany Name" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="sales_name" Caption="Sale Name" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="created_date" Caption="Created Date" Width="50px" />  
                            </Columns>
                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                        </dx:ASPxGridView>
                    </div>
                   
                   <div id="ApproveContentTabSO" runat="server" class="tab-pane fade">
                      <dx:ASPxGridView ID="gridViewSO" runat="server" AutoGenerateColumns="False"
                            Width="100%" KeyFieldName="id" EnableCallBacks="true"
                           OnCustomCallback="gridViewSO_CustomCallback"
                           >
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns> 
                                <dx:GridViewDataTextColumn Caption="Status Approve" FieldName="is_approve" CellStyle-HorizontalAlign="Center" Width="55px">
                                    <DataItemTemplate>
                                          <button type="button" id="item_<%# Eval("id")%>" class="btn btn-left btn-toggle"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>,'SO')">
                                            <div class="handle"></div>
                                        </button>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn> 
                                <dx:GridViewDataTextColumn FieldName="sale_order_no" Caption="Sale Order No" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation no" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="50px" />    
                                <dx:GridViewDataTextColumn FieldName="total_amount" Caption="Total Amount" Width="30px" CellStyle-HorizontalAlign="Center">
                                    <PropertiesTextEdit DisplayFormatString="#,###"></PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="sale_order_date" Caption="Sale Order Date" Width="50px" />  
                            </Columns>
                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                        </dx:ASPxGridView>
                    </div>
                     <div id="ApproveContentTabIS" runat="server" class="tab-pane fade">
                      <dx:ASPxGridView ID="gridViewIS" runat="server" AutoGenerateColumns="False"
                            Width="100%" KeyFieldName="id" EnableCallBacks="true"
                           OnCustomCallback="gridViewIS_CustomCallback"
                           >
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns> 
                                <dx:GridViewDataTextColumn Caption="Status Approve" FieldName="is_approve" CellStyle-HorizontalAlign="Center" Width="55px">
                                    <DataItemTemplate>
                                          <button type="button" id="item_<%# Eval("id")%>" class="btn btn-left btn-toggle"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>,'IS')">
                                            <div class="handle"></div>
                                        </button>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn> 
                                <dx:GridViewDataTextColumn FieldName="issue_stock_no" Caption="Issue No" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="issue_stock_status_display" Caption="Employee Code" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="issue_stock_status_display" Caption="Status Issue" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="issue_stock_date" Caption="Issue Date" Width="50px" />  
                            </Columns>
                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                        </dx:ASPxGridView>
                    </div>
                     <div id="ApproveContentTabDN" runat="server" class="tab-pane fade">
                      <dx:ASPxGridView ID="gridViewDN" runat="server" AutoGenerateColumns="False"
                            Width="100%" KeyFieldName="id" EnableCallBacks="true"
                           OnCustomCallback="gridViewDN_CustomCallback"
                           >
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns> 
                                <dx:GridViewDataTextColumn Caption="Status Approve" FieldName="is_approve" CellStyle-HorizontalAlign="Center" Width="55px">
                                    <DataItemTemplate>
                                          <button type="button" id="item_<%# Eval("id")%>" class="btn btn-left btn-toggle"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>,'IS')">
                                            <div class="handle"></div>
                                        </button>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn> 
                                <dx:GridViewDataTextColumn FieldName="delivery_no" Caption="Delivery No" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="issue_no" Caption="Issue No" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="quotation_no" Caption="Quotation No" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="delivery_status_display" Caption="Delivery Satatus" Width="50px" /> 
                                <dx:GridViewDataTextColumn FieldName="created_date" Caption="Issue Date" Width="50px" />  
                            </Columns>
                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                        </dx:ASPxGridView> 
                    </div>
                    <div id="ApproveContentTabPR" runat="server" class="tab-pane fade">
                      <dx:ASPxGridView ID="gridViewPR" runat="server" AutoGenerateColumns="False"
                            Width="100%" KeyFieldName="id" EnableCallBacks="true"
                           OnCustomCallback="gridViewPR_CustomCallback"
                           >
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                            <Paddings Padding="0px" />
                            <Border BorderWidth="0px" />
                            <BorderBottom BorderWidth="1px" />
                            <SettingsPager Position="Bottom" PageSize="<%$ appSettings:GridViewPageSize %>" PageSizeItemSettings-Position="Right"
                                PageSizeItemSettings-Visible="true">
                                <PageSizeItemSettings Items="10, 20, 50" />
                            </SettingsPager>
                            <Columns> 
                                <dx:GridViewDataTextColumn Caption="Status Approve" FieldName="is_approve" CellStyle-HorizontalAlign="Center" Width="55px">
                                    <DataItemTemplate>
                                          <button type="button" id="item_<%# Eval("id")%>" class="btn btn-left btn-toggle"
                                            data-toggle="button" aria-pressed="true" onclick="changeStatusApprove(<%# Eval("id")%>,'SO')">
                                            <div class="handle"></div>
                                        </button>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn> 
                                <dx:GridViewDataTextColumn FieldName="purchase_request_no" Caption="Purchase Request No" Width="50px" />  
                                <dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer Name" Width="50px" />    
                                <dx:GridViewDataTextColumn FieldName="supplier_name_tha" Caption="Supplier Name" Width="50px" />    
                                <dx:GridViewDataTextColumn FieldName="purchase_request_type_display" Caption="Purchase Request Status" Width="50px" />   
                            </Columns>
                            <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                            <SettingsSearchPanel CustomEditorID="SearchBox"></SettingsSearchPanel>
                            <ClientSideEvents Init="SPlanet.Page.ControlGrid_Init" ContextMenuItemClick="SPlanet.Page.ControlGrid_ContextMenuItemClick"
                                CustomizationWindowCloseUp="SPlanet.GridCustomizationWindow_CloseUp" />
                        </dx:ASPxGridView> 
                    </div>
                    <div id="ApproveContentTabRE" runat="server" class="tab-pane fade">
                      <h3>ApproveContentTabRE</h3> 
                    </div>
                    <div id="ApproveContentTabBR" runat="server" class="tab-pane fade">
                      <h3>ApproveContentTabBR</h3> 
                    </div> 
                  </div>
            </div>

            <!-- Area Chart Product-->
            <div class="card mb-3" style="margin-top: 10px;">
                <div class="card-header">
                    <div style="float: left;">
                        <i class="fa fa-area-chart"></i>&nbsp;Summary Product
                    </div>
                    <div style="float: left; margin-left: 15px;">
                        <select class="form-control" id="typeViewSummaryProduct">
                            <option value="WK" selected="selected">Weekly</option>
                            <option value="ML">Monthly</option>
                            <option value="AN">Annual</option>
                        </select>
                    </div>
                </div>
                <div class="card-body">
                    <canvas id="summaryProduct" width="100%" height="30"></canvas>
                </div>
            </div>

            <!-- Area Chart Spare Part-->
            <div class="card mb-3" style="margin-top: 10px;">
                <div class="card-header">
                    <div style="float: left;">
                        <i class="fa fa-area-chart"></i>&nbsp;Summary Spare Part
                    </div>
                    <div style="float: left; margin-left: 15px;">
                        <select class="form-control" id="typeViewSummarySparePart">
                            <option value="WK" selected="selected">Weekly</option>
                            <option value="ML">Monthly</option>
                            <option value="AN">Annual</option>
                        </select>
                    </div>
                </div>
                <div class="card-body">
                    <canvas id="summarySparePart" width="100%" height="30"></canvas>
                </div>
            </div>

            <!-- Area Chart Annual-->
            <div class="card mb-3" style="margin-top: 10px;">
                <div class="card-header">
                    <div style="float: left;">
                        <i class="fa fa-area-chart"></i>&nbsp;Summary Annual
                    </div>
                    <div style="float: left; margin-left: 15px;">
                        <select class="form-control" id="typeViewSummaryAnnual">
                            <option value="WK" selected="selected">Weekly</option>
                            <option value="ML">Monthly</option>
                            <option value="AN">Annual</option>
                        </select>
                    </div>
                </div>
                <div class="card-body">
                    <canvas id="summaryAnnual" width="100%" height="30"></canvas>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-8">
                    <!-- Example Bar Chart Card-->
                    <div class="card mb-3">
                        <div class="card-header">
                            <i class="fa fa-bar-chart"></i>&nbsp;Summary Sales Total 
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-sm-8 my-auto">
                                    <canvas id="myBarChart" width="100" height="50"></canvas>
                                </div>
                                <div class="col-sm-4 text-center my-auto">
                                    <div class="h4 mb-0 text-primary">$34,693</div>
                                    <div class="small text-muted">YTD Revenue</div>
                                    <hr>
                                    <div class="h4 mb-0 text-warning">$18,474</div>
                                    <div class="small text-muted">YTD Expenses</div>
                                    <hr>
                                    <div class="h4 mb-0 text-success">$16,219</div>
                                    <div class="small text-muted">YTD Margin</div>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer small text-muted">Updated yesterday at 11:59 PM</div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <!-- Example Pie Chart Card-->
                    <div class="card mb-3">
                        <div class="card-header">
                            <i class="fa fa-pie-chart"></i>Pie Chart Example
                        </div>
                        <div class="card-body">
                            <canvas id="myPieChart" width="100%" height="100"></canvas>
                        </div>
                        <div class="card-footer small text-muted">Updated yesterday at 11:59 PM</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="Content/Dashboard/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="Content/Dashboard/vendor/jquery-easing/jquery.easing.min.js"></script>
    <script src="Content/Dashboard/vendor/chart.js/Chart.js"></script>
    <script src="Content/Dashboard/js/sb-admin.js"></script>
    <%--<script src="Content/Dashboard/js/sb-admin-charts.js"></script>--%>
    <script>
        $(document).ready(function () {
            $("#Splitter_0").parent().remove();

            $("#Splitter_1").removeAttr("style");
            $("#Splitter_1").attr("style", "border-top-width: 0px;");

            DashboardData();

            $("#typeViewSummaryProduct").change(function () {
                if ($(this).val() == "WK") {
                    fnSummaryProduct(firstDayOfWeekly(), 6);
                } else if ($(this).val() == "ML") {
                    fnSummaryProduct(getCurrentMonth(), lastDaysInMonth());
                } else if ($(this).val() == "AN") {
                    var currentYear = (new Date).getFullYear();
                    fnSummaryProductAN(currentYear);
                }
            });

            $("#typeViewSummarySparePart").change(function () {
                if ($(this).val() == "WK") {
                    fnSummarySparePart(firstDayOfWeekly(), 6);
                } else if ($(this).val() == "ML") {
                    fnSummarySparePart(getCurrentMonth(), lastDaysInMonth());
                } else if ($(this).val() == "AN") {
                    var currentYear = (new Date).getFullYear();
                    fnSummarySparePartAN(currentYear);
                }
            });

            $("#typeViewSummaryAnnual").change(function () {
                if ($(this).val() == "WK") {
                    fnSummaryAnnual(firstDayOfWeekly(), 6);
                } else if ($(this).val() == "ML") {
                    fnSummaryAnnual(getCurrentMonth(), lastDaysInMonth());
                } else if ($(this).val() == "AN") {
                    var currentYear = (new Date).getFullYear();
                    fnSummaryAnnualAN(currentYear);
                }
            });
            

            gridViewQU.SetHeight(500);
            gridViewSO.SetHeight(50);
            gridViewIS.SetHeight(50);
            gridViewDN.SetHeight(50);
            //gridViewRE.SetHeight(50);
            gridViewPR.SetHeight(50);
            //gridViewBR.SetHeight(50);

        });

        function DashboardData() {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            var date_start = firstDayOfWeekly();
            var last_day = 6;
            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetDashboardData",
                data: '{date_start: "' + date_start + '", last_day_in_month: "' + last_day + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        $("#qu_sum").html(result.d.DashboardSumDocument[0]["qu_sum"]);
                        $("#so_sum").html(result.d.DashboardSumDocument[0]["so_sum"]);
                        $("#is_sum").html(result.d.DashboardSumDocument[0]["is_sum"]);
                        $("#pr_sum").html(result.d.DashboardSumDocument[0]["pr_sum"]);
                        $("#br_sum").html(result.d.DashboardSumDocument[0]["br_sum"]);
                        $("#rt_sum").html(result.d.DashboardSumDocument[0]["rt_sum"]);

                        graphDashboardProductSummary(result.d.DashboardSumTotalGraphProduct);
                        graphDashboardSparePartSummary(result.d.DashboardSumTotalGraphSparePart);
                        graphDashboardAnnualSummary(result.d.DashboardSumTotalGraphAnnual);

                        summarySaleTotal();

                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function graphDashboardProductSummary(dataArray) {
            var ctx = document.getElementById("summaryProduct");
            var dataLabel = new Array();
            var dataValue = new Array();
            for (var i = 0; i < dataArray.length; i++) {
                dataLabel.push(dataArray[i]["dates"]);
                dataValue.push(dataArray[i]["total"])
            }

            var dataMaxValue = Math.max.apply(null, dataValue) == 0 ? 20000 : Math.max.apply(null, dataValue);
            var dataMaxTicksLimit = 7;
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: dataLabel,
                    datasets: [{
                        label: "Baht",
                        lineTension: 0.3,
                        backgroundColor: "rgba(2,117,216,0.2)",
                        borderColor: "rgba(2,117,216,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(2,117,216,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(2,117,216,1)",
                        pointHitRadius: 20,
                        pointBorderWidth: 2,
                        data: dataValue,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: dataMaxValue,
                                maxTicksLimit: dataMaxTicksLimit
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        }

        function graphDashboardSparePartSummary(dataArray) {
            var ctx = document.getElementById("summarySparePart");
            var dataLabel = new Array();
            var dataValue = new Array();
            for (var i = 0; i < dataArray.length; i++) {
                //console.log(dataArray[i]["dates"] + " : " + dataArray[i]["total"]);
                dataLabel.push(dataArray[i]["dates"]);
                dataValue.push(dataArray[i]["total"])
            }
            var dataMaxValue = Math.max.apply(null, dataValue) == 0 ? 20000 : Math.max.apply(null, dataValue);
            var dataMaxTicksLimit = 7;
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: dataLabel,
                    datasets: [{
                        label: "Baht",
                        lineTension: 0.3,
                        backgroundColor: "rgba(2,117,216,0.2)",
                        borderColor: "rgba(2,117,216,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(2,117,216,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(2,117,216,1)",
                        pointHitRadius: 20,
                        pointBorderWidth: 2,
                        data: dataValue,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: dataMaxValue,
                                maxTicksLimit: dataMaxTicksLimit
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        }

        function graphDashboardAnnualSummary(dataArray) {
            var ctx = document.getElementById("summaryAnnual");
            var dataLabel = new Array();
            var dataValue = new Array();
            for (var i = 0; i < dataArray.length; i++) {
                // console.log(dataArray[i]["dates"] + " : " + dataArray[i]["total"]);
                dataLabel.push(dataArray[i]["dates"]);
                dataValue.push(dataArray[i]["total"])
            }
            var dataMaxValue = Math.max.apply(null, dataValue) == 0 ? 20000 : Math.max.apply(null, dataValue);
            var dataMaxTicksLimit = 7;
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: dataLabel,
                    datasets: [{
                        label: "Baht",
                        lineTension: 0.3,
                        backgroundColor: "rgba(2,117,216,0.2)",
                        borderColor: "rgba(2,117,216,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(2,117,216,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(2,117,216,1)",
                        pointHitRadius: 20,
                        pointBorderWidth: 2,
                        data: dataValue,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: dataMaxValue,
                                maxTicksLimit: dataMaxTicksLimit
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        }

        function summarySaleTotal() {
            var ctx = document.getElementById("myBarChart");
            var myLineChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["Product", "Spare Part", "Annual Service", "Service Charge", "Design"],
                    datasets: [{
                        label: "Revenue",
                        backgroundColor: "rgba(2,117,216,1)",
                        borderColor: "rgba(2,117,216,1)",
                        data: [4215, 5312, 6251, 7841, 9821],
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false
                            },
                            ticks: {
                                maxTicksLimit: 6
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: 15000,
                                maxTicksLimit: 5
                            },
                            gridLines: {
                                display: true
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        }

        function fnSummaryProduct(startDate, amountDay) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummaryProduct",
                data: '{dateStart: "' + startDate + '", amountDay: "' + amountDay + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardProductSummary(result.d.DashboardSumTotalGraphProduct);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function fnSummaryProductAN(thisYear) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummaryAnnualProduct",
                data: '{thisYear: "' + thisYear + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardProductSummary(result.d.DashboardSumTotalGraphProduct);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function fnSummarySparePart(startDate, amountDay) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummarySparePart",
                data: '{dateStart: "' + startDate + '", amountDay: "' + amountDay + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardSparePartSummary(result.d.DashboardSumTotalGraphSparePart);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function fnSummarySparePartAN(thisYear) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummaryAnnualSparePart",
                data: '{thisYear: "' + thisYear + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardSparePartSummary(result.d.DashboardSumTotalGraphSparePart);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function fnSummaryAnnual(startDate, amountDay) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummaryAnnual",
                data: '{dateStart: "' + startDate + '", amountDay: "' + amountDay + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardAnnualSummary(result.d.DashboardSumTotalGraphAnnual);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function fnSummaryAnnualAN(thisYear) {
            $.LoadingOverlay("show", {
                zIndex: 9999
            });

            $.ajax({
                type: 'POST',
                url: "Default.aspx/GetSummaryAnnualAN",
                data: '{thisYear: "' + thisYear + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {
                        graphDashboardAnnualSummary(result.d.DashboardSumTotalGraphAnnual);
                        $.LoadingOverlay("hide");
                    }
                }
            });
        }

        function getCurrentMonth() {
            var d = new Date();

            var month = d.getMonth() + 1;
            var day = d.getDate();

            var currentDate = d.getFullYear() + '-' +
                (('' + month).length < 2 ? '0' : '') + month + '-1';

            return currentDate;
        }

        function lastDaysInMonth() {
            var date = new Date();
            var month = (date.getMonth() + 1);
            var year = date.getFullYear();

            var maxDate = new Date(year, month, 0).getDate();
            return maxDate - 1;
        }

        function firstDayOfWeekly() {
            var startOfWeek = moment().startOf('week').toDate();

            var startDay = startOfWeek.getDate();
            var startMonth = (startOfWeek.getMonth() + 1);
            var startYear = startOfWeek.getFullYear();

            if (startDay < 10) {
                startDay = "0" + startDay;
            }

            if (startMonth < 10) {
                startMonth = "0" + startMonth;
            }

            return firstday = startYear + "-" + startMonth + "-" + startDay;
        }

        function changeStatusApprove(id, e) {
            var refer_name = e; 
            /*$.ajax({
                type: 'POST',
                url: "Default.aspx/ChangeStatus",
                data: '{refer_id: "' + id + '", refer_name: "' + refer_name + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.d) {

                        if (refer_name=="QU") {
                            gridViewQU.PerformCallback();
                        } else if (refer_name == "SO") {
                            gridViewSO.PerformCallback();
                        } 
                    }
                }
            });*/
        }
    </script>
</asp:Content>
