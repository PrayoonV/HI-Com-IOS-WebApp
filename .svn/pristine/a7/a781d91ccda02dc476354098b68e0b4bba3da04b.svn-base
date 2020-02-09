using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;

public partial class UserControls_DocumentBrowser : System.Web.UI.UserControl {
    DocumentsApp DocumentsApp { get { return DocumentsApp.Instance; } }

    string CurrentViewModeName {
        get { return DocumentsApp.User.CurrentViewModeName; }
    }
    bool IsDetailsViewMode {
        get { return CurrentViewModeName == "Details"; }
    }
    FileListView CurrentViewMode {
        get {
            return IsDetailsViewMode ? FileListView.Details : FileListView.Thumbnails;
        }
    }
    Item CurrentItem { get; set; }
    
    public FileSystemProviderBase FileSystemProvider { get; set; }
    public bool IsFilterApplied { get; set; }
    public string PanelName { get; set; }
    public string SourceName { get; set; }

    protected void Page_Init(object sender, EventArgs e) {
        CustomizeFileManagerByItemsFilter();
        BrowserPanel.Visible = IsFilterApplied;
        if(BrowserPanel.Visible) {
            BrowserContainer.Attributes["class"] += " ShowPanel";
            FilterNameLabel.Text = PanelName;
        }
    }

    void CustomizeFileManagerByItemsFilter() {
        FileManager.ID = (IsFilterApplied ? "Filtered" : "") + "FileManager";
        FileManager.CustomFileSystemProvider = FileSystemProvider;
        FileManager.SettingsUpload.Enabled = !IsFilterApplied;
        FileManager.SettingsUpload.ShowUploadPanel = IsFilterApplied;
        FileManager.SettingsUpload.ValidationSettings.MaxFileSize = DocumentsApp.FileSystem.GetMaxFileSizeForUpload();
        FileManager.SettingsBreadcrumbs.Visible = !IsFilterApplied;
        FileManager.Images.FileAreaFolder.Url = Page.ResolveUrl(DocumentsApp.Image.GetFolderIconUrl());
        FileManager.SettingsFileList.ThumbnailsViewSettings.ThumbnailWidth = Unit.Pixel(DocumentsApp.Image.ThumbnailWidth);
        FileManager.SettingsFileList.ThumbnailsViewSettings.ThumbnailHeight = Unit.Pixel(DocumentsApp.Image.ThumbnailHeight);
        FileManager.StylesDetailsView.CommandColumn.Width = Unit.Pixel(80);

        FileManager.SettingsEditing.AllowCopy = !IsFilterApplied;
        FileManager.SettingsEditing.AllowDelete = !IsFilterApplied;
        FileManager.SettingsEditing.AllowRename = !IsFilterApplied;
        FileManager.SettingsEditing.AllowMove = !IsFilterApplied;
        FileManager.SettingsEditing.AllowCreate = !IsFilterApplied;
    }

    protected void Page_Load(object sender, EventArgs e) {
        FileManager.SettingsFileList.View = CurrentViewMode;
        string viewModeClass = IsDetailsViewMode ? "DetailsView" : "ThumbnailsView";
        FileManager.CssClass = "FileManager " + viewModeClass;

        if(IsDetailsViewMode) {
            FileManager.Styles.File.Width = Unit.Empty;
            FileManager.Styles.File.Height = Unit.Empty;
        }
        else {
            FileManager.Styles.File.Width = Unit.Pixel(270);
            FileManager.Styles.File.Height = Unit.Pixel(270);
        }
        DevExpress.Web.MenuItem viewModeItem = ViewModeMenu.Items.FindByName(CurrentViewModeName);
        if(viewModeItem != null)
            viewModeItem.Selected = true;
    }

    protected void ItemTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerThumbnailsViewItemTemplateContainer).Item;
        long itemId = long.Parse(fileManagerItem.Id);
        Item item = DocumentsApp.Data.FindItemById(itemId);

        HtmlGenericControl thumbDiv = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemThumbDiv");
        thumbDiv.Style.Add("background-image", fileManagerItem.ThumbnailUrl);

        HtmlImage iconImage = CommonUtils.FindControl<HtmlImage>(placeHolder, "ItemIconImage");
        string iconImageSrc = DocumentsApp.Image.GetFooterItemIconVirtPath(item);
        iconImage.Src = iconImageSrc;
        iconImage.Alt = "Item type here";
        if(string.IsNullOrEmpty(iconImageSrc))
            iconImage.Visible = false;

        HtmlGenericControl infoDiv = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemInfoDiv");
        infoDiv.InnerText = "Modified " + GetModifiedInfoString(fileManagerItem.LastWriteTime);

        HtmlAnchor holderLink = CommonUtils.FindControl<HtmlAnchor>(placeHolder, "ItemHolderLink");
        PrepareItemLink(holderLink, item.IsFolder, fileManagerItem.FullName, item);

        bool itemLocked = CheckAndPrepareLockedState(placeHolder, item);
        if(itemLocked) {
            HtmlGenericControl footerDiv = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemFooterDiv");
            footerDiv.Attributes["class"] += " itemLocked";
        }
    }
    protected void ThumbnailCellTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        HtmlAnchor holderLink = CommonUtils.FindControl<HtmlAnchor>(placeHolder, "ItemLink");
        PrepareItemLink(holderLink, CurrentItem.IsFolder, fileManagerItem.FullName, CurrentItem);

        HtmlImage iconImage = CommonUtils.FindControl<HtmlImage>(placeHolder, "ItemIcon");
        string iconImageSrc = DocumentsApp.Image.GetDetailsViewItemIcon(CurrentItem) ?? fileManagerItem.ThumbnailUrl;
        iconImage.Src = iconImageSrc;
    }
    protected void NameCellTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        HtmlAnchor holderLink = CommonUtils.FindControl<HtmlAnchor>(placeHolder, "ItemLink");
        PrepareItemLink(holderLink, CurrentItem.IsFolder, fileManagerItem.FullName, CurrentItem);
    }
    protected void ModifiedDateCellTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        HtmlGenericControl itemLastWriteTimeSpan = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemLastWriteTimeSpan");
        itemLastWriteTimeSpan.InnerText = GetModifiedInfoString(fileManagerItem.LastWriteTime);
    }
    protected void SizeCellTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        HtmlGenericControl itemSizeSpan = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemSizeSpan");
        if(CurrentItem.IsFolder)
            itemSizeSpan.Visible = false;
        else
            itemSizeSpan.InnerText = CommonUtils.GetSizeString(fileManagerItem.Length);
    }
    protected void OwnerCellTemplate_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        HtmlGenericControl itemOwnerSpan = CommonUtils.FindControl<HtmlGenericControl>(placeHolder, "ItemOwnerSpan");
        itemOwnerSpan.InnerText = GetOwnerString(CurrentItem);
    }
    protected void LockImageCell_Init(object sender, EventArgs e) {
        PlaceHolder placeHolder = (PlaceHolder)sender;
        FileManagerItem fileManagerItem = (placeHolder.NamingContainer as FileManagerDetailsViewItemTemplateContainer).Item;
        EnsureCurrentItemUpdated(fileManagerItem);

        CheckAndPrepareLockedState(placeHolder, CurrentItem);
    }
    void PrepareItemLink(HtmlAnchor itemLink, bool itemIsFolder, string itemFullName, Item item) {
        itemLink.Attributes.Add("onmousedown", "App.onItemLinkMouseDown(event)");
        if(itemIsFolder)
            itemLink.Attributes.Add("onclick", "App.onFolderItemLinkClick(event)");
        else {
            itemLink.Attributes.Add("onclick", "App.onFileItemLinkClick(event)");
            if(DocumentsApp.Document.IsDocumentEditingAllowed(item))
                itemLink.Target = "_blank";
            string itemUrl = DocumentsApp.Document.GetDocumentEditorRequestUrl(itemFullName, SourceName);
            itemLink.HRef = Server.HtmlEncode(itemUrl);
        }
    }
    bool CheckAndPrepareLockedState(Control container, Item item) {
        User user = DocumentsApp.Document.GetDocumentLockOwner(item);
        if(user != null) {
            HtmlImage itemLockImage = CommonUtils.FindControl<HtmlImage>(container, "ItemLockImage");
            itemLockImage.Visible = true;
            string iconUrl, tooltip;
            if(user.Id == DocumentsApp.User.CurrentUser.Id) {
                iconUrl = DocumentsApp.Image.EditIconVirtPath;
                tooltip = "Opened by Me";
            }
            else {
                iconUrl = DocumentsApp.Image.LockIconVirtPath;
                tooltip = "Locked by " + user.Name;
            }
            itemLockImage.Src = iconUrl;
            itemLockImage.Alt = tooltip;
            itemLockImage.Attributes["title"] = tooltip;
            return true;
        }
        return false;
    }
    string GetModifiedInfoString(DateTime date) {
        string format = date.Date == DateTime.Today ? "H:mm" : "d MMM yyy";
        return date.ToString(format);
    }
    string GetOwnerString(Item item) {
        if(item.Owner == null)
            return string.Empty;
        return item.Owner.Id == DocumentsApp.User.CurrentUser.Id ? "Me" : item.Owner.Name;
    }
    void EnsureCurrentItemUpdated(FileManagerItem fileManagerItem) {
        long currentItemId = long.Parse(fileManagerItem.Id);
        if(CurrentItem == null || CurrentItem.Id != currentItemId)
            CurrentItem = DocumentsApp.Data.FindItemById(currentItemId);
    }
}
