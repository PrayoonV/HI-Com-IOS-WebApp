<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/Site.master" %>
<%@ Reference Control="~/UserControls/DocumentBrowser.ascx" %> 
<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <form id="form1" runat="server" action="Default.aspx" class="MainForm">
        <div class="MainPane">
            <dx:ASPxPanel runat="server" ID="LeftPanel" ClientInstanceName="LeftPanel" FixedPosition="WindowLeft" 
                FixedPositionOverlap="true" CssClass="LeftPanel" Collapsible="true">
                <SettingsCollapsing ExpandEffect="Slide" ExpandButton-Visible="true" />
                <SettingsAdaptivity CollapseAtWindowInnerWidth="1023" />
                <Styles>
                    <ExpandButton CssClass="LeftPanelExpandButton">
                        <SelectedStyle CssClass="LeftPanelSelectedExpandButton" />
                        <HoverStyle CssClass="LeftPanelHoveredExpandButton" />
                        <PressedStyle CssClass="LeftPanelPressedExpandButton" />
                    </ExpandButton>
                </Styles>
                <ClientSideEvents Collapsed="App.onLeftPanelCollapsed" />
                <PanelCollection>
                    <dx:PanelContent>
                        <div class="LogoPane">
                            <a class="LogoLink" href="Default.aspx">
                                <dx:ASPxImage runat="server" ID="LogoImage" ImageUrl="~/Content/Images/Icons/DXLogo.png" ShowLoadingImage="true"></dx:ASPxImage>
                            </a>
                        </div>
                        <div class="SidePane">
                            <dx:ASPxMenu runat="server" ID="FilterMenu" CssClass="FilterMenu" Orientation="Vertical" Width="100%"
                                AllowSelectItem="true" ClientSideEvents-ItemClick="App.onFilterMenuItemClick">
                                <ItemStyle CssClass="FilterItem">
                                    <HoverStyle CssClass="FilterHoveredItem" />
                                    <SelectedStyle CssClass="FilterSelectedItem" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="All" Text="All" Selected="true" ItemStyle-CssClass="FilterItem"></dx:MenuItem>
                                    <dx:MenuItem Name="My" Text="My"></dx:MenuItem>
                                    <dx:MenuItem Name="Recent" Text="Recent"></dx:MenuItem>
                                    <dx:MenuItem Name="RTFDocs" Text="RTF Docs"></dx:MenuItem>
                                    <dx:MenuItem Name="Sheets" Text="Sheets"></dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
            <dx:ASPxPanel runat="server" ID="TopPanel" ClientInstanceName="TopPanel" FixedPosition="WindowTop" CssClass="HeadPane">
                <PanelCollection>
                    <dx:PanelContent>
                        <table class="HeadTable">
                            <tr>
                                <td class="ActionMenuContainer">
                                    <dx:ASPxMenu runat="server" ID="ActionMenu" CssClass="ActionMenu" Width="100%" ItemAutoWidth="false" 
                                        EnableAdaptivity="true" AppearAfter="9999999" ClientSideEvents-ItemClick="App.onActionMenuItemClick"
                                        ClientSideEvents-Init="App.onActionMenuInit">
                                        <ItemStyle CssClass="ActionMenuItem" HoverStyle-CssClass="ActionMenuHoveredItem">
                                        </ItemStyle>
                                        <SubMenuStyle CssClass="ActionSubMenu" />
                                        <SubMenuItemStyle CssClass="ActionSubMenuItem" HoverStyle-CssClass="ActionSubMenuHoveredItem" />
                                        <Items>
                                            <dx:MenuItem Name="New" Text="New">
                                                <Image Url="Content/Images/MenuIcons/New.png" />
                                                <Items>
                                                    <dx:MenuItem Name="Folder" Text="Folder">
                                                        <Image Url="Content/Images/Predefined/Folder.png" Width="16px" Height="16px" />
                                                    </dx:MenuItem>
                                                    <dx:MenuItem Name="WorksheetDocument" Text="Worksheet Document" BeginGroup="true">
                                                        <Image Url="Content/Images/Predefined/Spreadsheet.png" Width="16px" Height="16px" />
                                                    </dx:MenuItem>
                                                    <dx:MenuItem Name="RichTextDocument" Text="Rich Text Document">
                                                        <Image Url="Content/Images/Predefined/RTF.png" Width="16px" Height="16px" />
                                                    </dx:MenuItem>
                                                </Items>
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Upload" Text="Upload">
                                                <Image Url="Content/Images/MenuIcons/Upload.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Refresh" Text="Refresh">
                                                <Image Url="Content/Images/MenuIcons/Refresh.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Rename" Text="Rename" BeginGroup="true" ClientVisible="false">
                                                <Image Url="Content/Images/MenuIcons/Rename.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Move" Text="Move" ClientVisible="false">
                                                <Image Url="Content/Images/MenuIcons/Move.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Copy" Text="Copy" ClientVisible="false">
                                                <Image Url="Content/Images/MenuIcons/Copy.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Delete" Text="Delete" ClientVisible="false">
                                                <Image Url="Content/Images/MenuIcons/Delete.png" />
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Download" Text="Download" ClientVisible="false">
                                                <Image Url="Content/Images/MenuIcons/Download.png" />
                                            </dx:MenuItem>
                                        </Items>
                                    </dx:ASPxMenu>
                                </td>
                                <td class="UserMenuContainer">
                                    <dx:ASPxMenu runat="server" ID="UserMenu" CssClass="UserMenu" OnItemClick="UserMenu_ItemClick" 
                                        RightToLeft="True" AppearAfter="9999999" GutterWidth="0px">
                                        <ItemStyle CssClass="UserMenuItem" HoverStyle-CssClass="UserMenuHoveredItem" />
                                        <SubMenuStyle CssClass="UserSubMenu" />
                                        <SubMenuItemStyle CssClass="UserSubMenuItem" HoverStyle-CssClass="UserSubMenuHoveredItem" />
                                        <Items>
                                            <dx:MenuItem Name="User">
                                                <Image Width="60" Height="60" />
                                                <Items>
                                                    <dx:MenuItem Name="SignOut" Text="Sign Out"></dx:MenuItem>
                                                </Items>
                                            </dx:MenuItem>
                                        </Items>
                                        <ClientSideEvents ItemClick="App.onUserMenuItemClick" />
                                    </dx:ASPxMenu>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
            <div class="ContentPane">
                <dx:ASPxLoadingPanel runat="server" ID="LoadingPanel" ClientInstanceName="LoadingPanel" Modal="true">
                    <LoadingDivStyle CssClass="LoadingDiv"></LoadingDivStyle>
                    <Template>
                        <img src="Content/Images/Icons/loading.gif"></img>
                    </Template>
                </dx:ASPxLoadingPanel>
                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel" CssClass="CallbackPanel">
                    <SettingsLoadingPanel Enabled="false" />
                    <ClientSideEvents BeginCallback="App.onControlBeginCallback" EndCallback="App.onControlEndCallback" />
                    <PanelCollection>
                        <dx:PanelContent>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>
        </div>
    </form>
</asp:Content>
