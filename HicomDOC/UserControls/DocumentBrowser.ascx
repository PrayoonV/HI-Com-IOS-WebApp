<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DocumentBrowser.ascx.cs" Inherits="UserControls_DocumentBrowser" %>
<div runat="server" ID="BrowserContainer" class="BrowserContainer">
<dx:ASPxPanel runat="server" ID="BrowserPanel" CssClass="BrowserPanel" Width="100%" Visible="false">
    <PanelCollection>
        <dx:PanelContent>
            <dx:ASPxLabel runat="server" ID="FilterNameLabel"></dx:ASPxLabel>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
<div class="ViewModeContainer">
    <dx:ASPxMenu runat="server" ID="ViewModeMenu" ClientInstanceName="ViewModeMenu" CssClass="ViewModeMenu" ShowAsToolbar="true" 
        AllowSelectItem="true" ClientSideEvents-ItemClick="App.onViewModeMenuItemClick">
        <ItemStyle CssClass="ViewModeMenuItem" HoverStyle-CssClass="ViewModeMenuHoveredItem" Width="29px" Height="28px">
        </ItemStyle>
        <Items>
            <dx:MenuItem Name="Thumbnails" ToolTip="Thumbnails view" Selected="true">
                <ItemStyle>
                    <BackgroundImage ImageUrl="~/Content/Images/Icons/Thumbnails_off.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <SelectedStyle>
                        <BackgroundImage ImageUrl="~/Content/Images/Icons/Thumbnails_on.png" Repeat="NoRepeat"
                            HorizontalPosition="center" VerticalPosition="center" />
                    </SelectedStyle>
                </ItemStyle>
            </dx:MenuItem>
            <dx:MenuItem Name="Details" ToolTip="Details view">
                <ItemStyle>
                    <BackgroundImage ImageUrl="~/Content/Images/Icons/Details_off.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <SelectedStyle>
                        <BackgroundImage ImageUrl="~/Content/Images/Icons/Details_on.png" Repeat="NoRepeat"
                            HorizontalPosition="center" VerticalPosition="center" />
                    </SelectedStyle>
                </ItemStyle>
            </dx:MenuItem>
        </Items>
    </dx:ASPxMenu>
</div>
<dx:ASPxFileManager runat="server" ID="FileManager" ClientInstanceName="FileManager" Height="100%">
    <Settings RootFolder="Files" ThumbnailFolder="~\Thumb\" EnableMultiSelect="false"
        AllowedFileExtensions=".rtf,.doc,.docx,.txt,.xlsx,.png,.gif,.jpg,.jpeg,.ico,.bmp,.avi,.mp3,.xml,.pdf" />
    <SettingsEditing AllowCopy="True" AllowCreate="True" AllowDelete="True" AllowDownload="True" AllowMove="True" AllowRename="True" />
    <SettingsUpload Enabled="true" ShowUploadPanel="false" DialogTriggerID="ActionMenu_DXI1_">
        <AdvancedModeSettings EnableMultiSelect="true" EnableDragAndDrop="true" />
    </SettingsUpload>
    <SettingsFolders Visible="false" />
    <SettingsFileList ShowFolders="true" ShowParentFolder="false">
        <DetailsViewSettings ThumbnailHeight="50" ThumbnailWidth="50">
            <Columns>
                <dx:FileManagerDetailsColumn Caption=" " FileInfoType="Thumbnail" Width="78">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="ThumbnailCellTemplate_Init">
                            <a runat="server" ID="ItemLink" title='<%# Eval("Name") %>' class="ItemLink ItemThumb" href="javascript:;">
                                <img runat="server" ID="ItemIcon" class="ItemThumbnail" alt='<%# Eval("Name") %>' />
                            </a>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption="Name" FileInfoType="FileName" Width="50%">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="NameCellTemplate_Init">
                            <a runat="server" ID="ItemLink" title='<%# Eval("Name") %>' class="ItemLink ItemName" href="javascript:;">
                                <%# Eval("Name") %>
                            </a>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption="Modified" FileInfoType="LastWriteTime" Width="21%">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="ModifiedDateCellTemplate_Init">
                            <span runat="server" ID="ItemLastWriteTimeSpan" class="ItemLastWriteTime"></span>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption="Size" FileInfoType="Size" Width="9%">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="SizeCellTemplate_Init">
                            <span runat="server" ID="ItemSizeSpan" class="ItemSize"></span>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption="Owner" Width="20%">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="OwnerCellTemplate_Init">
                            <span runat="server" ID="ItemOwnerSpan" class="ItemOwner"></span>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption=" ">
                    <ItemTemplate></ItemTemplate>
                </dx:FileManagerDetailsColumn>
                <dx:FileManagerDetailsColumn Caption=" " Width="40px">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" OnInit="LockImageCell_Init">
                            <img runat="server" ID="ItemLockImage" class="itemUserIcon" Visible="false" />
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </dx:FileManagerDetailsColumn>
            </Columns>
        </DetailsViewSettings>
        <ThumbnailsViewSettings>
            <ItemTemplate>
                <asp:PlaceHolder runat="server" OnInit="ItemTemplate_Init">
                    <a runat="server" ID="ItemHolderLink" class="itemHolder" href="javascript:;">
                        <div runat="server" ID="ItemThumbDiv" class="itemThumb"></div>
                        <div class="ItemThumbGradient"></div>
                        <div runat="server" ID="ItemFooterDiv" class="itemFooter">
                            <img runat="server" ID="ItemIconImage" class="itemIcon" />
                            <div class="itemText">
                                <div class="itemName"><%# Eval("Name") %></div>
                                <div runat="server" ID="ItemInfoDiv" class="itemDate"></div>
                            </div>
                            <img runat="server" ID="ItemLockImage" class="itemUserIcon" Visible="false" />
                        </div>
                    </a>
                </asp:PlaceHolder>
            </ItemTemplate>
        </ThumbnailsViewSettings>
    </SettingsFileList>
    <SettingsBreadcrumbs Visible="true" ShowParentFolderButton="true" Position="Top" />
    <SettingsToolbar Visible="false" />
    <SettingsLoadingPanel Enabled="false" />
    <ClientSideEvents 
        ToolbarUpdating="App.onFileManagerToolbarUpdating" 
        BeginCallback="App.onControlBeginCallback" 
        EndCallback="App.onControlEndCallback" />
    <StylesDetailsView>
        <CommandColumn Width="60px" CssClass="ItemCell" />
        <Cell CssClass="ItemCell" />
    </StylesDetailsView>
    <Styles>
        <File Paddings-Padding="0px" CssClass="FileManagerItem">
            <HoverStyle CssClass="HoveredFileManagerItem" />
            <FocusedStyle CssClass="FocusedFileManagerItem" />
            <SelectionActiveStyle CssClass="SelectedActiveFileManagerItem" />
            <SelectionInactiveStyle CssClass="SelectedInactiveFileManagerItem" />
        </File>
        <FileAreaFolder Paddings-Padding="0px" CssClass="FileManagerItem">
            <HoverStyle CssClass="HoveredFileManagerItem" />
            <FocusedStyle CssClass="FocusedFileManagerItem" />
            <SelectionActiveStyle CssClass="SelectedActiveFileManagerItem" />
            <SelectionInactiveStyle CssClass="SelectedInactiveFileManagerItem" />
        </FileAreaFolder>
        <BreadCrumbs CssClass="BreadCrumbs" />
        <BreadCrumbsItem CssClass="BreadCrumbsItem">
            <HoverStyle CssClass="BreadCrumbsHoveredItem" />
        </BreadCrumbsItem>
    </Styles>
</dx:ASPxFileManager>
</div>
